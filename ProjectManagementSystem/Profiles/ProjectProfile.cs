using AutoMapper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Models.ProjectViewModels;

namespace ProjectManagementSystem.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectViewModel>().ReverseMap();
        }
    }
}