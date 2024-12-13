using AutoMapper;
using Demo.Dal.Entities;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfiles
{
    public class UsersProfile :Profile
    {
        public UsersProfile() 
        {
            CreateMap<AppUser, UserVMcs>(); 
        }
    }
}
