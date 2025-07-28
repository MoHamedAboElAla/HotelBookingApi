using HotelBookingApi.Dtos;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
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

        public BookingController(IBookingRepo bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        [HttpPost("Book")]
       [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto bookingDto)
        {

            
            var agentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            

            var result = await _bookingRepo.CreateBooking(bookingDto, agentId);
            if (result == null)
                return BadRequest("Room or Agent not found.");

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
    }
}
