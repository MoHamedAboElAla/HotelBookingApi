using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        IHotelRepository _repo;

        public HotelsController(IHotelRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public IActionResult GetAllHotels()
        {
            var Hotels = _repo.GetAll();
            if (Hotels == null)
                return NotFound("No Hotels Found");
            return Ok(Hotels);
        }
        [HttpGet("{id}")]
        public IActionResult GetHotelById(int id)
        {
            var hotel = _repo.GetById(id);
            if (hotel == null)
                return NotFound($"hotel with id {id} not found");
            return Ok(hotel);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Hotel hotel)
        {
            if (hotel == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();
            _repo.add(hotel);
            _repo.save();
            
            return CreatedAtAction(nameof(GetHotelById), new { id = hotel.Id }, hotel);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, Hotel hotel)
        {
            if (hotel == null)
                return BadRequest();
            if (hotel.Id != id)
                return BadRequest("Id mismatched");
            _repo.edit(hotel);
            _repo.save();

            return NoContent(); //204 

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            Hotel hotel = _repo.GetById(id);
            if (hotel == null)
             return NotFound();
            _repo.remove(id);
            _repo.save();     
            return Ok(hotel);
        }
    }
}
