using AutoMapper;
using HomeApi.Configuration;
using HomeApi.Contracts.Models.Home;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;

namespace HomeApi.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Address, AddressInfo>();
            CreateMap<HomeOptions, InfoResponse>()
                .ForMember(
                x => x.AddressInfo, 
                opt => opt.MapFrom(src => src.Address));

            CreateMap<AddDeviceRequest, Device>()
                .ForMember(
                    x => x.Location,
                    opt => opt.MapFrom(src => src.Location));
            CreateMap<AddRoomRequest, Room>();
            CreateMap<Device, DeviceView>();
        }
    }
}
