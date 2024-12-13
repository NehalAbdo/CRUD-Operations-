using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfiles
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<EmployeeVM, Employee>().ReverseMap();/*.ForMember(d => d.Name, opt => opt.MapFrom(s => s.EmployeeName));*/
        }
    }
}
