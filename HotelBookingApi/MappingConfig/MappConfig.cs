using AutoMapper;
using HotelBookingApi.Dtos.RoomDTOS;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.Models;

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
            CreateMap<Room, displayRoom>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .AfterMap((src, dest) => dest.HotelName = src.Hotel?.Name);


            CreateMap<AddRoom, Room>().ReverseMap();
        }
    }
}
