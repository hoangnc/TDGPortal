using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.Abp.Identity.Web.Pages.Identity;
using Volo.Abp.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using MasterData.UserDepartments;
using MasterData.Departments;

namespace Abp.AspNetCore.Mvc.UI.Theme.AdminLTE.Identity.Users
{
    public class EditModalModel : IdentityPageModel
    {
        [BindProperty]
        public UserInfoViewModel UserInfo { get; set; }

        [BindProperty]
        public AssignedRoleViewModel[] Roles { get; set; }

        [BindProperty]
        public List<SelectListItem> Departments { get; set; }

        [BindProperty]
        public AssignedDepartmentViewModel Department { get; set; }

        protected IIdentityUserAppService IdentityUserAppService { get; }
        protected IUserDepartmentAppService UserDepartmentAppService { get; }
        protected IDepartmentAppService DepartmentAppService { get; }

        public EditModalModel(IIdentityUserAppService identityUserAppService,
            IUserDepartmentAppService userDepartmentAppService,
            IDepartmentAppService departmentAppService)
        {
            IdentityUserAppService = identityUserAppService;
            UserDepartmentAppService = userDepartmentAppService;
            DepartmentAppService = departmentAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync(Guid id)
        {
            UserInfo = ObjectMapper.Map<IdentityUserDto, UserInfoViewModel>(await IdentityUserAppService.GetAsync(id));

            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>((await IdentityUserAppService.GetAssignableRolesAsync()).Items);

            var userRoleNames = (await IdentityUserAppService.GetRolesAsync(UserInfo.Id)).Items.Select(r => r.Name).ToList();
            foreach (var role in Roles)
            {
                if (userRoleNames.Contains(role.Name))
                {
                    role.IsAssigned = true;
                }
            }

            var departmentDtoList = (await DepartmentAppService.GetListAsync(new GetDepartmentsInput
            {
                MaxResultCount = 1000,
                SkipCount = 0
            }));

            Department = ObjectMapper.Map<UserDepartmentDto, AssignedDepartmentViewModel>(await UserDepartmentAppService.GetByUserNameAsync(UserInfo.UserName));

            Departments = departmentDtoList.Items.Select(d =>
            {

                if (Department != null && !Department.DepartmentCode.IsNullOrEmpty())
                {
                    return new SelectListItem
                    {
                        Selected = true,
                        Text = d.Name,
                        Value = d.Code
                    };
                }
            
                return new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Code
                };
            }).ToList();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserUpdateDto>(UserInfo);
            input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();
            await IdentityUserAppService.UpdateAsync(UserInfo.Id, input);

            var userDepartment = new UserDepartmentUpdateDto
            {
                UserName = input.UserName,
                DepartmentCode = Department.DepartmentCode
            };

            await UserDepartmentAppService.UpdateAsync(0, userDepartment);

            return NoContent();
        }

        public class UserInfoViewModel : ExtensibleObject, IHasConcurrencyStamp
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }

            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            public string UserName { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
            public string Name { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
            public string Surname { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string Password { get; set; }

            [Required]
            [EmailAddress]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
            public string Email { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
            public string PhoneNumber { get; set; }

            public bool LockoutEnabled { get; set; }
        }

        public class AssignedDepartmentViewModel
        {
            public long Id { get; set; }
            public string DepartmentCode { get; set; }
        }
        public class AssignedRoleViewModel
        {
            [Required]
            [HiddenInput]
            public string Name { get; set; }

            public bool IsAssigned { get; set; }
        }
    }
}
