using AutoMapper;
using HotelBooking.Domain.Models;
using HotelBookingApi.Dtos.RoomDTOS;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        IRoomRepo room;
        IMapper map;
        public RoomsController(IRoomRepo _room, IMapper _map)
        {
            room = _room;
            map = _map;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
           List<Room>Rooms= room.GetAll();
            if (Rooms == null)
                return BadRequest("No data ");
            return Ok(map.Map<List<displayRoom>>((Rooms)));

        }
        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            Room r = room.GetbyId(id);

            if (r== null) return NotFound("No Room Matched");
            
                displayRoom RoomDto = map.Map<displayRoom>(r);
                return Ok(RoomDto);
            
        }

        [HttpPost]
        public IActionResult add( AddRoom RDTO)
        {
            if (RDTO == null) return BadRequest();
            if (ModelState.IsValid)
            {
                Room r = map.Map<Room>(RDTO);
                room.Add(r);
                room.Save();
                return CreatedAtAction("GetById", new { id = r.Id }, r);
            }
            else return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public IActionResult Update(Room r, int id)
        {
            if (r == null) return BadRequest();
            if (r.Id != id) return BadRequest();
            if (ModelState.IsValid)
            {
                room.Update(r);
                room.Save();
                return NoContent();
            }
            else return BadRequest(ModelState);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Room r = room.GetbyId(id);
            if (r == null) return NotFound();
            displayRoom roomDTO = map.Map<displayRoom>(r);
            room.Delete(r);
            room.Save();
            return Ok(roomDTO);
        }

    }
}
