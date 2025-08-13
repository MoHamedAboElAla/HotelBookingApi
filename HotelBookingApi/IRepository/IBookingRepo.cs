using HotelBookingApi.Dtos;
using HotelBookingApi.Models;
using System.Net;

namespace HotelBookingApi.IRepository
{
    public interface IBookingRepo
    {

        Task<BookingResponseDto> CreateBooking(CreateBookingDto bookingDto, int agentId);
       BookingWithAgentAndRoomDto GetBookingById(int id);
        IEnumerable<BookingWithAgentAndRoomDto> GetAllBookings();
        void UpdateBooking(Booking booking);
       // void DeleteBooking(int id);
        Task<List<Booking>> GetBookingsByHotelIdAsync(int hotelId);
        Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId);
        Task Save();

        Task<int> GetBookingsCountInDateRangeAsync(DateTime startDate, DateTime endDate);

        public bool HasCompletedBooking(int agentId, int hotelId);


    }
}
