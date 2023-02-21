using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class PhanQuyen
    {
    }

    public class PhanQuyen_Users
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Source { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }

    }

    public class PhanQuyen_Roles
    {
        public string RoleID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Checked { get; set; }

    }

    public class PhanQuyen_Permissions
    {
        public string PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string PermissionKey { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Checked { get; set; }

    }
}
