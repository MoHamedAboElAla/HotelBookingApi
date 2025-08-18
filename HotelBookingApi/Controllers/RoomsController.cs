using AutoMapper;
using HotelBookingApi.Data;
using HotelBookingApi.Dtos;
using HotelBookingApi.Dtos.RoomDTOS;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using HotelBookingApi.Repository;
using HotelBookingApi.Services;
using Microsoft.AspNetCore.Hosting;
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
        private readonly AppDbContext _context;

        private readonly IImageUrlService _imageUrlService;
        private readonly IWebHostEnvironment _env;
        public RoomsController(IRoomRepo _room, IMapper _map, IImageUrlService imageUrlService, AppDbContext context, IWebHostEnvironment env)
        {
            room = _room;
            map = _map;
            _imageUrlService = imageUrlService;
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Room> rooms = room.GetAll();
            if (rooms == null)
                return BadRequest("No data");

            var dtoList = map.Map<List<displayRoom>>(rooms);

            foreach (var roomDto in dtoList)
            {
                roomDto.ImageUrl = _imageUrlService.GenerateRoomImageUrl(roomDto.ImageUrl!);

            }

            return Ok(dtoList);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetRoomsCount() => Ok(await _context.Rooms.CountAsync());


        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
           
            Room r = room.GetbyId(id);
            if (r == null)
                return NotFound("No Room Matched");

            displayRoom roomDto = map.Map<displayRoom>(r);

            if (!string.IsNullOrEmpty(roomDto.ImageUrl))

            {

                roomDto.ImageUrl = _imageUrlService.GenerateRoomImageUrl(
                    Path.GetFileName(roomDto.ImageUrl)
                );
            }

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
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "rooms");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(RDTO.Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        RDTO.Image.CopyTo(stream);
                    }

                    r.ImageUrl = uniqueFileName;

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


        //Add Multiple Rooms
        [HttpPost("bulk")]
        public IActionResult AddMultipleRooms([FromForm] AddMultipleRoomsDto dto)
        {
            if (dto.HotelId == 0 || dto.Count <= 0)
                return BadRequest("HotelId and Count are required.");

            var intendedRoomNumbers = Enumerable.Range(dto.StartRoomNumber, dto.Count).ToList();

            // Step 1: Check for duplicates in DB
            var existingRoomNumbers = room
                .GetRoomNumbersForHotel(dto.HotelId)
                .Where(rn => intendedRoomNumbers.Contains(rn))
                .ToList();

            if (existingRoomNumbers.Any())
            {
                return Conflict(new
                {
                    message = "Some room numbers already exist for this hotel.",
                    duplicatedNumbers = existingRoomNumbers
                });
            }

            // Step 2: Proceed with creation
            List<Room> roomsToAdd = new();
            for (int i = 0; i < dto.Count; i++)
            {
                var roomNumber = dto.StartRoomNumber + i;

                var room = new Room
                {
                    RoomNumber = roomNumber,
                    RoomType = dto.RoomType,
                    Description = dto.Description,
                    PricePerNight = dto.PricePerNight,
                    IsAvailable = dto.IsAvailable,
                    HotelId = dto.HotelId
                };
                if (dto.Image != null && dto.Image.Length > 0)

                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "rooms");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        dto.Image.CopyTo(stream);
                    }

                    room.ImageUrl = uniqueFileName;

                }

                roomsToAdd.Add(room);
            }

            try
            {
                room.AddRange(roomsToAdd);
                room.Save();
                return Ok(new { message = $"{dto.Count} rooms added successfully." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Unexpected DB error occurred.");
            }
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
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "rooms");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.Image.CopyTo(stream);
                }

                existingRoom.ImageUrl = uniqueFileName;
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
            if (!r.IsAvailable)
                return BadRequest("Cannot delete a room that is already booked.");

            displayRoom roomDTO = map.Map<displayRoom>(r);
            room.Delete(r);
            room.Save();
            return Ok(roomDTO);
        }
        [HttpGet("available")]
        public ActionResult<IEnumerable<displayRoom>> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int hotelId)
        {
            if (startDate >= endDate)
                return BadRequest("Start date must be before end date.");

            var availableRooms = room.GetAvailableRoomsUsingDate(startDate, endDate, hotelId);
            if (availableRooms == null)
                return NotFound("No available rooms found for the given date range.");
            return Ok(availableRooms);
        }
        [HttpPost("upload-room-image")]
        public async Task<IActionResult> UploadRoomImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");

            string imagesFolder = Path.Combine(_env.WebRootPath, "images", "rooms");

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(imagesFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            string imageUrl = $"/images/rooms/{uniqueFileName}";
            return Ok(new { imageUrl });
        }


    }
}