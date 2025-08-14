using AutoMapper;

using HotelBookingApi.Dtos;
using HotelBookingApi.Dtos.HotelReviewsDTOs;
using HotelBookingApi.Dtos.RoomDTOS;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.Models;

namespace HotelBookingApi.MappingConfig
{
    public class MappConfig : Profile
    {
        public MappConfig()
        {
            CreateMap<Season, DisplaySeasonDTO>();

            CreateMap<AddSeasonDTO, Season>();
            CreateMap<EditSeasonDTO, Season>();

            CreateMap<HotelReview, DisplayReviewDTO>().AfterMap(
            (src, dest) =>
            {
                dest.Agent = src.Agent.Name;
                dest.HotelName = src.Hotel.Name;
            });
            CreateMap<AddReviewDTO, HotelReview>();
            CreateMap<EditReviewDTO, HotelReview>();

            // Room <-> RoomDto
            CreateMap<Room, RoomDto>();
            CreateMap<RoomDto, Room>();

            CreateMap<Hotel, HotelViewDto>();
            CreateMap<Room, RoomDto>();



            // Room -> displayRoom
            CreateMap<Room, displayRoom>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .AfterMap((src, dest) => dest.HotelName = src?.Hotel != null ? src.Hotel.Name : "Unknown Hotel");

            // Hotel <-> HotelDto

            CreateMap<HotelDto, Hotel>()
            .ForMember(dest => dest.ImageFileName, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            // Hotel -> HotelViewDto
            CreateMap<Hotel, HotelViewDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            // Add / Update Room
            CreateMap<AddRoom, Room>().ReverseMap();
            CreateMap<UpdateRoomDto, Room>().ReverseMap();
        }
    }

}
