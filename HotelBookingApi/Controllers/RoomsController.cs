using AutoMapper;
using HotelBookingApi.Data;
using HotelBookingApi.Dtos.RoomDTOS;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using HotelBookingApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        IRoomRepo room;
        IMapper map;

        private readonly IImageUrlService _imageUrlService;
        public RoomsController(IRoomRepo _room, IMapper _map, IImageUrlService imageUrlService)
        {
            room = _room;
            map = _map;
            _imageUrlService = imageUrlService;
        }
        /*
        [HttpGet]
        public IActionResult GetAll()
        {
           List<Room>Rooms= room.GetAll();
            if (Rooms == null)
                return BadRequest("No data ");
            return Ok(map.Map<List<displayRoom>>((Rooms)));

        }
        */
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Room> rooms = room.GetAll();
            if (rooms == null)
                return BadRequest("No data");

            var dtoList = map.Map<List<displayRoom>>(rooms);

            foreach (var roomDto in dtoList)
            {
                roomDto.ImageUrl = _imageUrlService.GenerateRoomImageUrl(
                    Path.GetFileName(roomDto.ImageUrl)
                );
            }

            return Ok(dtoList);
        }
        /*
        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            Room r = room.GetbyId(id);

            if (r== null) return NotFound("No Room Matched");

                displayRoom RoomDto = map.Map<displayRoom>(r);
                return Ok(RoomDto);

        }
        */

        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            Room r = room.GetbyId(id);
            if (r == null) return NotFound("No Room Matched");

            displayRoom roomDto = map.Map<displayRoom>(r);

            roomDto.ImageUrl = _imageUrlService.GenerateRoomImageUrl(
                Path.GetFileName(roomDto.ImageUrl)
            );

            return Ok(roomDto);
        }

        [HttpPost]
        public IActionResult Add([FromForm] AddRoom RDTO)
        {
            if (RDTO.HotelId == 0)
            {
                ModelState.AddModelError("HotelId", "You must select a hotel.");
                return BadRequest(ModelState);
            }
            if (RDTO == null) return BadRequest();

            if (ModelState.IsValid)
            {
                Room r = map.Map<Room>(RDTO);

                if (RDTO.Image != null && RDTO.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(RDTO.Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        RDTO.Image.CopyTo(stream);
                    }

                    //r.ImageUrl = "/uploads/" + uniqueFileName;
                    r.ImageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";

                }

                try
                {
                    room.Add(r);
                    room.Save();

                    return CreatedAtAction("GetById", new { id = r.Id }, r);
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(new
                    {
                        errors = new { Duplicate = new[] { "Room number and hotel are already Inserted before they must be unique. Please change them." } }
                    });
                }
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

                existingRoom.ImageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";
            }

            try
            {
                room.Update(existingRoom);
                room.Save();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Rooms_RoomNumber_HotelId") == true ||
                    ex.InnerException?.Message.Contains("duplicate") == true ||
                    ex.InnerException?.Message.Contains("UNIQUE") == true)
                {
                    return BadRequest(new { error = "This room number is already used for the selected hotel." });
                }

                return StatusCode(500, "An unexpected database error occurred.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while updating the room.");
            }
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
        /*  [HttpGet("hotel/{hotelId}")]
          public async Task<ActionResult<IEnumerable<displayRoom>>> GetRoomsByHotel(int hotelId)
          {
              {
                  var rooms = await _context.Rooms.Include(r => r.Hotel)
                        .Where(r => r.HotelId == hotelId)
                        .Select(r => new displayRoom
                        {
                            Id = r.Id,
                            RoomNumber = r.RoomNumber,
                            RoomType = r.RoomType,
                            Description = r.Description!,
                            PricePerNight = r.PricePerNight,
                            IsAvailable = r.IsAvailable,
                            ImageUrl = r.ImageUrl,
                            HotelName = r.Hotel!.Name!
                        })
                        .ToListAsync();

                  if (rooms == null || !rooms.Any())
                  {
                      return NotFound("No rooms found for the specified hotel.");
                  }

                  return Ok(rooms);
              }
          }
*/

    }
}