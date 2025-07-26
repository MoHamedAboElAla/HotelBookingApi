using AutoMapper;
using HotelBooking.Domain.Models;
using HotelBookingApi.DTOs.SeasonDTOs;

namespace HotelBookingApi.MappingConfig
{
    public class MappConfig:Profile
    {
        public MappConfig()
        {
            CreateMap<Season, DisplaySeasonDTO>().AfterMap(
                (src, dest) => dest.BookingIds = src.Bookings.Select(b => b.Id).ToList()
                );

            CreateMap<AddSeasonDTO, Season>();


            CreateMap<EditSeasonDTO, Season>();
        }
    }
}
