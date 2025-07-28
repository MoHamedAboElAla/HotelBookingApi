using HotelBookingApi.Data;
using HotelBookingApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly AppDbContext _context;
        public CartController(AppDbContext context)
        {
            _context = context;
        }
        private int GetAgentIdFromToken()
        {
            var agentIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

            if (agentIdClaim == null)
                throw new UnauthorizedAccessException("Invalid token: Agent ID not found.");

            return int.Parse(agentIdClaim.Value);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems()
        {
            var agentId = GetAgentIdFromToken();
            var items = await _context.CartItems
            .Where(c => c.AgentId == agentId)
            .Include(c => c.Booking)
                .ThenInclude(b => b.Room)
                    .ThenInclude(r => r.Hotel)

                    .Select(c => new CartItemDto
                    {
                        BookingId = c.Booking.Id,
                        HotelName = c.Booking!.Room!.Hotel!.Name!,
                        RoomNumber = c.Booking.Room.RoomNumber,
                        RoomType = c.Booking.Room.RoomType!,
                        CheckInDate = c.Booking.CheckInDate,
                        CheckOutDate = c.Booking.CheckOutDate,
                        TotalPrice = c.Booking.TotalPrice,
                        AgentEmail = c.Agent!.Email


                    })
            .ToListAsync();

            return Ok(items);
        }

        [HttpDelete("bookingId")]
        public async Task<IActionResult> RemoveFromCart(int bookingId)
        {
            //1-Get AgentId
            var agentId = GetAgentIdFromToken();

            //2-get cart item by bookingId and agentId
            var item = await _context.CartItems
           .FirstOrDefaultAsync(c => c.AgentId == agentId && c.BookingId == bookingId);

            if (item == null)
            {
                return NotFound("Cart item not found.");
            }
            _context.CartItems.Remove(item);
            // Remove related booking 
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                if (room != null)
                {
                    room.IsAvailable = true; 
                    _context.Rooms.Update(room); 
                }
                _context.Bookings.Remove(booking);
            }
            await _context.SaveChangesAsync();
            return Ok("Item removed from cart successfully.");
        }
      
    }
    }
