using Com.DanLiris.Service.Core.Lib.Helpers;
using Com.DanLiris.Service.Core.Lib.Interfaces;
using Com.DanLiris.Service.Core.Lib.Models.Account_and_Roles;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.Lib.ViewModels.Account_and_Roles;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.Services.Account_and_Roles
{
    public class RolesService : BasicService<CoreDbContext, Role>, IMap<Role, RoleViewModel>
    {

        public RolesService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public Role MapToModel(RoleViewModel roleVM)
        {
            Role role = new Role();
            role.Permissions = new List<Permission>();

            role.Code = roleVM.Code;
            role.Name = roleVM.Name;
            role.Description = roleVM.Description;

            foreach (PermissionViewModel permissionVM in roleVM.Permissions)
            {
                Permission permission = new Permission();

                permission.Id = permissionVM.Id;
                permission._CreatedBy = permissionVM._CreatedBy;
                permission._CreatedAgent = permissionVM._CreatedAgent;
                permission._CreatedUtc = permissionVM._CreatedUtc;
                permission.RoleId = permissionVM.RoleId;
                permission.UnitId = permissionVM.Unit.Id;
                permission.Unit = permissionVM.Unit.Name;
                permission.UnitCode = permissionVM.Unit.Code;
                permission.permission = permissionVM.Permission;
                permission.Division = permissionVM.Unit.Division != null ? permissionVM.Unit.Division.Name : null;

                role.Permissions.Add(permission);
            }

            return role;
        }

        public RoleViewModel MapToViewModel(Role role)
        {
            RoleViewModel roleVM = new RoleViewModel();
            roleVM.Permissions = new List<PermissionViewModel>();

            roleVM.Code = role.Code;
            roleVM.Name = role.Name;
            roleVM.Description = role.Description;

            if (role.Permissions != null)
            {
                foreach (Permission permission in role.Permissions)
                {
                    PermissionViewModel permissionVM = new PermissionViewModel();
                    permissionVM.Unit = new UnitViewModel();
                    permissionVM.Unit.Division = new DivisionViewModel();

                    permissionVM.Id = permission.Id;
                    permissionVM._CreatedBy= permission._CreatedBy;
                    permissionVM._CreatedAgent = permission._CreatedAgent;
                    permissionVM._CreatedUtc = permission._CreatedUtc;
                    permissionVM.RoleId = permission.RoleId;
                    permissionVM.Unit.Id= permission.UnitId;
                    permissionVM.Unit.Name = permission.Unit;
                    permissionVM.Unit.Code = permission.UnitCode;
                    permissionVM.Permission = permission.permission;
                    permissionVM.Unit.Division.Name = permission.Division;

                    roleVM.Permissions.Add(permissionVM);
                }
            }

            return roleVM;
        }

        public override Tuple<List<Role>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            throw new NotImplementedException();
        }
    }
}
