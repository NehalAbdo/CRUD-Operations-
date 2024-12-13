using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Demo.PL.MappingProfiles
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleVM>().ReverseMap();
        }
    }
}
