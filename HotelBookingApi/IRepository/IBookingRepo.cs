using HotelBookingApi.Dtos;
using HotelBookingApi.Models;

namespace HotelBookingApi.IRepository
{
    public interface IBookingRepo
    {

        Task<BookingResponseDto> CreateBooking(CreateBookingDto bookingDto, int agentId);
        Booking GetBookingById(int id);
        IEnumerable<Booking> GetAllBookings();
        void UpdateBooking(Booking booking);
        void DeleteBooking(int id);
    }
}
