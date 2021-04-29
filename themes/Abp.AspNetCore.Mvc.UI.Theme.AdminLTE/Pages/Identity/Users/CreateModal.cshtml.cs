using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.Abp.Identity.Web.Pages.Identity;
using Volo.Abp.Identity;
using MasterData.UserDepartments;
using Microsoft.AspNetCore.Mvc.Rendering;
using MasterData.Departments;
using Volo.Abp.Uow;

namespace Abp.AspNetCore.Mvc.UI.Theme.AdminLTE.Identity.Users
{
    public class CreateModalModel : IdentityPageModel
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
        protected IDepartmentAppService DepartmentAppService {get;} 
        public CreateModalModel(IIdentityUserAppService identityUserAppService,
            IUserDepartmentAppService userDepartmentAppService,
            IDepartmentAppService departmentAppService)
        {
            IdentityUserAppService = identityUserAppService;
            UserDepartmentAppService = userDepartmentAppService;
            DepartmentAppService = departmentAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            UserInfo = new UserInfoViewModel();

            var roleDtoList = (await IdentityUserAppService.GetAssignableRolesAsync()).Items;

            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>(roleDtoList);

            foreach (var role in Roles)
            {
                role.IsAssigned = role.IsDefault;
            }

            var departmentDtoList = (await DepartmentAppService.GetListAsync(new GetDepartmentsInput { 
                MaxResultCount = 1000,
                SkipCount = 0
            }));

            Departments = departmentDtoList.Items.Select(d => new SelectListItem { 
                Text = d.Name,
                Value = d.Code
            }).ToList();

            return Page();
        }

        [UnitOfWork]
        public virtual async Task<NoContentResult> OnPostAsync()
        {
            ValidateModel();

            var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserCreateDto>(UserInfo);
            input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();

            await IdentityUserAppService.CreateAsync(input);

            var userDepartment = new UserDepartmentCreateDto
            {
                UserName = input.UserName,
                DepartmentCode = Department.DepartmentCode
            };

            await UserDepartmentAppService.CreateAsync(userDepartment);

            return NoContent();
        }

        public class UserInfoViewModel : ExtensibleObject
        {
            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            public string UserName { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
            public string Name { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
            public string Surname { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [DisableAuditing]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            public string Password { get; set; }

            [Required]
            [EmailAddress]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
            public string Email { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
            public string PhoneNumber { get; set; }

            public bool LockoutEnabled { get; set; } = true;
        }

        public class AssignedDepartmentViewModel
        {
            public string DepartmentCode { get; set; }
        }
        public class AssignedRoleViewModel
        {
            [Required]
            [HiddenInput]
            public string Name { get; set; }

            public bool IsAssigned { get; set; }

            public bool IsDefault { get; set; }
        }
    }
}
