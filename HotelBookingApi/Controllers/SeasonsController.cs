using AutoMapper;
using HotelBooking.Domain.Models;
using HotelBookingApi.Data;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.IRepository;
using HotelBookingApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private readonly ISeasonRepo repo;
        private readonly IMapper map;

        public SeasonsController(ISeasonRepo Repo, IMapper map)
        {
            this.repo = Repo;
            this.map = map;
        }


        //getall
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Season>seasons =  repo.GetAll();
            if (seasons == null) return NotFound();
            return Ok(map.Map<List<DisplaySeasonDTO>>((seasons)));
        }


        //getbyid
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Season s = repo.GetById(id);

            if (s == null) return NotFound("No Seasons Found");
            else
            {
                DisplaySeasonDTO sDTO = map.Map<DisplaySeasonDTO>(s);
                return Ok(sDTO);
            }
        }


        //add
        [HttpPost]
        public IActionResult add(AddSeasonDTO sDTO)
        {
            if (sDTO == null) return BadRequest();
            if(ModelState.IsValid)
            {
                Season s = map.Map<Season>(sDTO);
                repo.Add(s);
                repo.save();
                return CreatedAtAction("GetById", new {id = s.Id},s);
            }
            else return BadRequest(ModelState);
        }


        //edit
        [HttpPut("{id}")]
        public IActionResult Update([FromBody]EditSeasonDTO sDTO, int id)
        {
            if(sDTO == null) return BadRequest();
            if (sDTO.Id != id) return BadRequest();
            if (ModelState.IsValid)
            {
                Season s = map.Map<Season>(sDTO);
                repo.update(s);
                repo.save();
                return NoContent();
            }
            else return BadRequest(ModelState);
        }


        //delete
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Season s = repo.GetById(id);
            if (s == null) return NotFound();
            DisplaySeasonDTO sDTO = map.Map< DisplaySeasonDTO >(s);
            repo.delete(s);
            repo.save();
            return Ok(sDTO);
        }


        //Pagnation
        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Page number and page size must be greater than zero.");

            var seasons = repo.GetPaged(pageNumber, pageSize);
            var totalCount = repo.GetTotalCount();

            var result = new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = map.Map<List<DisplaySeasonDTO>>(seasons)
            };

            return Ok(result);
        }



    }
}
