using AutoMapper;
using HotelBooking.Domain.Models;
using HotelBookingApi.Dtos;

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


            CreateMap<HotelDto, Hotel>()
          .ForMember(dest => dest.ImageFileName, opt => opt.Ignore());

            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore());

            CreateMap<Hotel, HotelViewDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<Room, displayRoom>().AfterMap(
                (src, dest) => dest.HotelName = src?.Hotel != null ? src.Hotel.Name : "Unknown Hotel"

                );
            CreateMap<AddRoom, Room>().ReverseMap();
            CreateMap<UpdateRoomDto, Room>().ReverseMap();

        }
    }
}
