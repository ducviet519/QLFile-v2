using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class ModulePhanMem
    {
        public string PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string PermissionKey { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }

    public class PermissionsInRole
    {
        public string PermissionID { get; set; }
        public string RoleID { get; set; }
        public string Status { get; set; }
    }

    public class PermissionLKey
    {
        public string PermissionKey { get; set; }
    }
}
