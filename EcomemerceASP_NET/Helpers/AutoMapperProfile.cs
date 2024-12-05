using AutoMapper;
using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;

namespace EcomemerceASP_NET.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, KhachHang>();
                //.ForMember(kh => kh.HoTen, opt => opt.MapFrom(RegisterVM => RegisterVM.HoTen))
        }
    }
}
