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

        public BookingRepo(AppDbContext context, ISeasonRepo seasonRepo, IRoomRepo roomRepo)
        {
            _context = context;
            _seasonRepo = seasonRepo;
            _roomRepo = roomRepo;
        }


        public async Task<List<Booking>> GetBookingsByHotelIdAsync(int hotelId)
        {
            return await _context.Bookings
           .Where(b => b.HotelId == hotelId)
           .Include(b => b.Room)
           .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId)
        {
            return await _context.Bookings
            .Where(b => b.RoomId == roomId)
            .Include(b => b.Hotel)
            .ToListAsync();
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
            var room = _roomRepo.GetbyId(bookingDto.RoomId);
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
                CheckOutDate = bookingDto.EndDate,
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


        //public void DeleteBooking(int id)
        //{

        //    var booking = _context.Bookings.Find(id);
        //    if (booking == null)
        //    {
        //        return;
        //    }

        //    var room = _context.Rooms.Find(booking.RoomId);
        //    if (room != null)
        //    {
        //        room.IsAvailable = true;
        //        _context.Rooms.Update(room);
        //    }

        //    _context.Bookings.Remove(booking);
        //    _context.SaveChanges();

        //}

        


        public IEnumerable<BookingWithAgentAndRoomDto> GetAllBookings()
        {

            var booking = _context.Bookings
     .Include(b => b.Agent)
     .Include(b => b.Room).ToList();


            return booking.Select(b => new BookingWithAgentAndRoomDto
            {

               BookingId =b.Id,
                AgentName = b.Agent!.Name,
                RoomNumber = b.Room!.RoomNumber,
                AgentEmail = b.Agent!.Email,
                RoomType = b.Room!.RoomType!,
                RoomPrice = b.Room.PricePerNight,
                CheckIn = b.CheckInDate,
                CheckOut = b.CheckOutDate,
                TotalPrice = b.TotalPrice
               
            });
        }

        public BookingWithAgentAndRoomDto GetBookingById(int id)
        {
            var booking = _context.Bookings
      .Include(b => b.Agent)
      .Include(b => b.Room)
      .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return null!;

            var dto = new BookingWithAgentAndRoomDto
            {
                BookingId = booking.Id,
                CheckIn = booking.CheckInDate,
                CheckOut = booking.CheckOutDate,
                AgentName = booking!.Agent!.Name,
                AgentEmail = booking.Agent.Email,
                RoomNumber = booking!.Room!.RoomNumber,
                RoomType = booking.Room.RoomType!,
                RoomPrice = booking.Room.PricePerNight,
                TotalPrice = booking.TotalPrice,
            };

            return dto;
        }

        public  void UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
        }

        public Task Save()
        {

            return _context.SaveChangesAsync();
        }
    }
}