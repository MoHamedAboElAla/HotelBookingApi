using AutoMapper;

using HotelBookingApi.Dtos;

using HotelBookingApi.Dtos.RoomDTOS;
using HotelBookingApi.DTOs.SeasonDTOs;
using HotelBookingApi.Models;

namespace HotelBookingApi.MappingConfig
{
    /*
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

            CreateMap<Room, RoomDto>();
            CreateMap<Hotel, HotelViewDto>();
            CreateMap<RoomDto, Room>();
        }
    }

    */
    public class MappConfig : Profile
    {
        public MappConfig()
        {
            CreateMap<Season, DisplaySeasonDTO>()
                .AfterMap((src, dest) => dest.BookingIds = src.Bookings.Select(b => b.Id).ToList());

            CreateMap<AddSeasonDTO, Season>();
            CreateMap<EditSeasonDTO, Season>();

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
