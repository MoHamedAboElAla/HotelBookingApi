using HotelBookingApi.Data;
using HotelBookingApi.Dtos;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;

namespace HotelBookingApi.Repository
{
    public class BookingRepo : IBookingRepo
    {
        private AppDbContext _context;
        private readonly ISeasonRepo _seasonRepo;
        private readonly IRoomRepo _roomRepo;

        public BookingRepo(AppDbContext context, ISeasonRepo seasonRepo , IRoomRepo roomRepo)
        {
            _context = context;
            _seasonRepo = seasonRepo;
            _roomRepo = roomRepo;
        }



        public async Task<BookingResponseDto> CreateBooking(CreateBookingDto bookingDto, int agentId)
        {
            var existingBookings = await _context.Bookings
            .Where(b => b.RoomId == bookingDto.RoomId)
            .ToListAsync();

            bool isRoomAvailable = !existingBookings.Any(b =>
                bookingDto.StartDate < b.CheckOutDate &&
                bookingDto.EndDate > b.CheckInDate);

            if (!isRoomAvailable)
            {
                return null!; 
            }
            if (bookingDto.StartDate < DateTime.Today)
            {
                return null!; 
            }

            if (bookingDto.EndDate < DateTime.Today)
            {
                return null!;
            }

            if (bookingDto.EndDate <= bookingDto.StartDate)
            {
                return null!;
            }
            var room =  _roomRepo.GetbyId(bookingDto.RoomId);
            if (room == null)
                return null!;

            var agent = await _context.Agents.FindAsync(agentId);
            if (agent == null)
                return null!;

            var season = await _seasonRepo.GetSeasonByDateRangeAsync(bookingDto.StartDate, bookingDto.EndDate);
            decimal basePrice = room.PricePerNight;
            decimal adjustedPrice = season != null ? basePrice + (basePrice * season.PriceFactor) : basePrice;

            var nights = (bookingDto.EndDate - bookingDto.StartDate).Days;
            var totalPrice = adjustedPrice * nights;

            var booking = new Booking
            {
                RoomId = bookingDto.RoomId,
                CheckInDate = bookingDto.StartDate,
                CheckOutDate= bookingDto.EndDate,
                AgentId = agentId,
                TotalPrice = totalPrice,
                CreatedAt = DateTime.Now,
                HotelId = room.HotelId,
               
            };
            room.IsAvailable = false;
            _roomRepo.Update(room);
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            var cartItem = new CartItem
            {
                AgentId = agentId,
                BookingId = booking.Id
            };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return new BookingResponseDto
            {
                RoomId = room.Id,
                RoomNumber = room.RoomNumber,
                StartDate = booking.CheckInDate,
                EndDate = booking.CheckOutDate,
                AgentName = agent.Name,
                TotalPrice = totalPrice
            };
        }


        public void DeleteBooking(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            throw new NotImplementedException();
        }

        public Booking GetBookingById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateBooking(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
