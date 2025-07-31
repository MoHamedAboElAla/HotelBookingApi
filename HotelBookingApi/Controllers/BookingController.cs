using HotelBookingApi.Data;
using HotelBookingApi.Dtos;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using HotelBookingApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly ISeasonRepo _seasonRepo;
        private readonly AppDbContext _context;

        public BookingController(IBookingRepo bookingRepo   , IRoomRepo roomRepo, ISeasonRepo seasonRepo,AppDbContext context)
        {
            _bookingRepo = bookingRepo;
            _roomRepo = roomRepo;
            _seasonRepo = seasonRepo;
            _context = context;
        }

        [HttpPost("Book")]
       [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto bookingDto)
        {

            
            var agentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            

            var result = await _bookingRepo.CreateBooking(bookingDto, agentId);
            if (result == null)
                return BadRequest("يرجي التأكد من صحة البيانات المدخلة");

            return Ok(result);

        }

        [HttpGet("{id}")]
        public IActionResult GetBookingById(int id)
        {
            var booking = _bookingRepo.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpGet]
        public IActionResult GetAllBookings()
        {
            var bookings = _bookingRepo.GetAllBookings();
            return Ok(bookings);
        }
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {


            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            if (booking.CheckInDate <= DateTime.Now && DateTime.Now < booking.CheckOutDate)
            {
                return BadRequest("Cannot delete a booking that is currently active.");
            }

            var room =  _roomRepo.GetbyId(booking.RoomId ??0 );
            var otherBookings = await _bookingRepo.GetBookingsByRoomIdAsync(room.Id);

            if (!otherBookings.Any(b =>
                b.Id != booking.Id && 
                b.CheckInDate < booking.CheckOutDate &&
                b.CheckOutDate > booking.CheckInDate))
            {
                room.IsAvailable = true;
                _roomRepo.Update(room);
                 _roomRepo.Save();
            }
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
       

    

    }
}
