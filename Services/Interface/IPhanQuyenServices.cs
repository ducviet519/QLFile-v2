using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IPhanQuyenServices
    {       

        public Task<List<ViewRoleList>> Get_ViewRoleList(string docid = null, string leveltype = null);
        public Task<string> Upsert_ViewRole(List<ViewRoleData> listRole, string user);
        public Task<List<Roles>> GetAllRoles(string username = null);
        public Task<string> AddRole(Roles roles, string user);
        public Task<string> UpsertRoleInUse(List<RolesInUser> roles, string user);
        public Task<string> AddPermission(ModulePhanMem permission, string user);
        public Task<List<ModulePhanMem>> GetListPermission(string roleID = null);
        public Task<string> UpsertPermissionsInRole(List<PermissionsInRole> permission, string user);
        public Task<List<PermissionLKey>> GetListPermissionKeys(string username);
    }
}
