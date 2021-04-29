using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MasterData.UserDepartments;
using Volo.Abp.Identity;
using CreateModalModel = Abp.AspNetCore.Mvc.UI.Theme.AdminLTE.Identity.Users.CreateModalModel;
using EditModalModel = Abp.AspNetCore.Mvc.UI.Theme.AdminLTE.Identity.Users.EditModalModel;

namespace Abp.AspNetCore.Mvc.UI.Theme.AdminLTE
{
    public class AdminLTEAutoMapperProfile : Profile
    {
        public AdminLTEAutoMapperProfile()
        {
            CreateMap<IdentityRoleDto, CreateModalModel.AssignedRoleViewModel>()
                .ForMember(c => c.IsAssigned, options => options.Ignore());

            CreateMap<CreateModalModel.UserInfoViewModel, IdentityUserCreateDto>()
                 .ForMember(c => c.RoleNames, options => options.Ignore());

            CreateMap<IdentityRoleDto, EditModalModel.AssignedRoleViewModel>()
               .ForMember(c => c.IsAssigned, options => options.Ignore());

            CreateMap<EditModalModel.UserInfoViewModel, IdentityUserUpdateDto>()
                 .ForMember(c => c.RoleNames, options => options.Ignore());

            CreateMap<IdentityUserDto, EditModalModel.UserInfoViewModel>()
                .ForMember(c => c.Password, options => options.Ignore());

            CreateMap<UserDepartmentDto, EditModalModel.AssignedDepartmentViewModel>();
        }
    }
}
