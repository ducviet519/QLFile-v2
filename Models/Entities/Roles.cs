using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class Roles
    {

        public string RoleID { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

    }

    public class RolesInUser
    {
        public string RoleID { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
    }

    public class ViewRoleList
    {
        public string STT { get; set; }
        public string ID { get; set; }
        public string PhanCap { get; set; }
        public string TenVanBan { get; set; }
        public string TenKhoaPhongNV { get; set; }
        public string DoiTuong { get; set; }
        public string DocID { get; set; }
        public string LevelType { get; set; }
        public string UserType { get; set; }
        public string Dept_UserID { get; set; }
        public string HRMUserType { get; set; }
        public string ReadR { get; set; }
        public string PrintR { get; set; }
        public string DownloadR { get; set; }
    }

    public class ViewRoleUpsert
    {
        public string ID { get; set; }
        public string LevelType { get; set; }
        public string DocID { get; set; }
        public string UserType { get; set; }
        public string Dept_UserID { get; set; }
        public string HRMUserType { get; set; }
        public int ReadR { get; set; }
        public int PrintR { get; set; }
        public int DownloadR { get; set; }
    }

    public class ViewRoleData
    {
        public string DocID { get; set; }
        public string LevelType { get; set; }
        public string UserType { get; set; }
        public string DeptUserGroupID { get; set; }
        public string HRMUserType { get; set; }
        public string ReadR { get; set; }
        public string PrintR { get; set; }
        public string DownloadR { get; set; }
    }
}
