using AutoMapper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Models.AccountViewModels;

namespace ProjectManagementSystem.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, RegisterViewModel>().
                ForMember(dest =>
                    dest.Role,
                    opt => opt.MapFrom(src => src.RoleName))
                .ReverseMap();
            
          
        }
    }
}