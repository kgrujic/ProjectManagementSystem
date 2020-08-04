using AutoMapper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Models.TaskViewModels;

namespace ProjectManagementSystem.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, TaskViewModel>().ForMember(dest => dest.Assignee, opt => opt.NullSubstitute(new ApplicationUser()
                )).ReverseMap();
        }
      
    }
}