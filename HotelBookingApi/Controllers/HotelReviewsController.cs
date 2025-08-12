using AutoMapper;
using HotelBookingApi.Dtos.HotelReviewsDTOs;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelReviewsController : ControllerBase
    {
        private readonly IHotelReviewsRepo repo;
        private readonly IMapper map;

        public HotelReviewsController(IHotelReviewsRepo Repo, IMapper Map)
        {
            repo = Repo;
            map = Map;
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

            if (r == null) return NotFound("No Seasons Found");
            else
            {
                DisplayReviewDTO sDTO = map.Map<DisplayReviewDTO>(r);
                return Ok(sDTO);
            }
        }


        //add
        [HttpPost]
        public IActionResult add(AddReviewDTO rDTO)
        {
            if (rDTO == null) return BadRequest();
            if (ModelState.IsValid)
            {
                HotelReview r = map.Map<HotelReview>(rDTO);
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
