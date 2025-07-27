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
        public IActionResult Add([FromForm] AddRoom RDTO)
        {
            if (RDTO == null) return BadRequest();

            if (ModelState.IsValid)
            {
                Room r = map.Map<Room>(RDTO);

                if (RDTO.Image != null && RDTO.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder); // إن لم يكن موجودًا

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(RDTO.Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        RDTO.Image.CopyTo(stream);
                    }

                    r.ImageUrl = "/uploads/" + uniqueFileName; 
                }

                room.Add(r);
                room.Save();

                return CreatedAtAction("GetById", new { id = r.Id }, r);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromForm] UpdateRoomDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingRoom = room.GetbyId(id);
            if (existingRoom == null)
                return NotFound($"Room with id {id} not found.");

            map.Map(dto, existingRoom);

            if (dto.Image != null && dto.Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.Image.CopyTo(stream);
                }

                existingRoom.ImageUrl = "/uploads/" + uniqueFileName;
            }

            room.Update(existingRoom);
            room.Save();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Room r = room.GetbyId(id);
            if (r == null) return NotFound();
            displayRoom roomDTO = map.Map<displayRoom>(r);
            room.Delete(r);
            room.Save();
            return Ok(roomDTO);
        }
        [HttpGet("available")]
        public ActionResult<IEnumerable<Room>> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate >= endDate)
                return BadRequest("Start date must be before end date.");

            var availableRooms = room.GetAvailableRooms(startDate, endDate);
            if (availableRooms == null)
                return NotFound("No available rooms found for the given date range.");
            return Ok(availableRooms);
        }

    }
}
