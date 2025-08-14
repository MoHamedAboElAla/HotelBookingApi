using AutoMapper;
using HotelBookingApi.Data;
using HotelBookingApi.Dtos.HotelReviewsDTOs;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using HotelBookingApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelReviewsController : ControllerBase
    {
        private readonly IHotelReviewsRepo repo;
        private readonly IMapper map;
        private readonly IBookingRepo bookrepo;
        private readonly AppDbContext db;

        public HotelReviewsController(IHotelReviewsRepo Repo, IMapper Map, IBookingRepo bookrepo, AppDbContext db)
        {
            repo = Repo;
            map = Map;
            this.bookrepo = bookrepo;
            this.db = db;
        }


        //getall
        [HttpGet]
        public IActionResult GetAll()
        {
            List<HotelReview> reviews = repo.GetAll();
            if (reviews == null) return NotFound();
            return Ok(map.Map<List<DisplayReviewDTO>>((reviews)));
        }
        //getbyid
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            HotelReview r = repo.GetById(id);

            if (r == null) return NotFound("No Reviews Found");
            else
            {
                DisplayReviewDTO sDTO = map.Map<DisplayReviewDTO>(r);
                return Ok(sDTO);
            }
        }


        //add
        [HttpPost]
        [Authorize]
        public IActionResult add(AddReviewDTO rDTO)
        {
            if (rDTO == null) return BadRequest("Review data is required.");
            
            var agentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            bool userExists = db.Bookings.Any(b => b.AgentId == agentId);
            if (!userExists)
                return Unauthorized("User not found.");

            // 
            bool hasCompletedBooking = bookrepo.HasCompletedBooking(agentId, rDTO.HotelId);
            if (!hasCompletedBooking)
                return BadRequest("You must have stayed at this hotel before leaving a review.");

            if (ModelState.IsValid)
            {
                HotelReview r = map.Map<HotelReview>(rDTO);
                r.AgentId = agentId;
                repo.Add(r);
                repo.save();
                return CreatedAtAction("GetById", new { id = r.Id }, r);
            }
            else return BadRequest(ModelState);
        }



       //edit
        [HttpPut("{id}")]
        public IActionResult Update([FromBody] EditReviewDTO rDTO, int id)
        {
            if (rDTO == null) return BadRequest();
            if (rDTO.Id != id) return BadRequest();
            if (ModelState.IsValid)
            {
                HotelReview r = map.Map<HotelReview>(rDTO);
                repo.update(r);
                repo.save();
                return NoContent();
            }
            else return BadRequest(ModelState);
        }


        //delete
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            HotelReview r = repo.GetById(id);
            if (r == null) return NotFound();
            DisplayReviewDTO rDTO = map.Map<DisplayReviewDTO>(r);
            repo.delete(r);
            repo.save();
            return Ok(rDTO);
        }

    }
}
