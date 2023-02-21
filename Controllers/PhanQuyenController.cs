using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Models.ViewModel;
using WebTools.Services;

namespace WebTools.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PhanQuyenController : Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        public PhanQuyenController(IUnitOfWork services)
        {
            _services = services;
        }
        #endregion

        #region Phân quyền Văn bản
        public IActionResult QuyenLoaiVanBan()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Upsert_PhanQuyenVanBan([FromBody] List<ViewRoleData> listRole)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                result = await _services.PhanQuyen.Upsert_ViewRole(listRole, user);
                
                if (result == "OK")
                {
                    message = $"Đã cập nhật lại quyền";                
                    title = "Thành công!";
                    result = "success";
                }
                else
                {
                    message = $"Lỗi! {result}";
                    title = "Lỗi!";
                    result = "error";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }
       
        public async Task<IActionResult> CapNhat_PhanQuyenVanBan(string id = null)
        {
            ViewRoleVM model = new ViewRoleVM();
            if (!String.IsNullOrEmpty(id))
            {
                var data = await _services.PhanQuyen.Get_ViewRoleList();
                model.viewRole = data.FirstOrDefault(i => i.ID == id);
            }
            else
            {
                model.viewRole = new ViewRoleList()
                {
                    ID = String.Empty,
                    Dept_UserID = String.Empty,
                    DocID = String.Empty,
                    DoiTuong = String.Empty,
                    HRMUserType = String.Empty,
                    LevelType = null,
                    PhanCap = String.Empty,
                    DownloadR = "0",
                    PrintR = "0",
                    ReadR = "0",
                    STT = String.Empty,
                    TenKhoaPhongNV = String.Empty,
                    TenVanBan = String.Empty,
                    UserType = null,
                };
            }    
            return PartialView("_QuyenLoaiVanBan_CapNhat", model);
        }

        public async Task<JsonResult> Get_ListPhanQuyenVanBan(int doctype, string docid = null, string leveltype = null)
        {
            var data = (await _services.PhanQuyen.Get_ViewRoleList(docid, leveltype)).ToList();
            if(doctype == 2)
            {
                data = data.Where(i => i.DocID != docid).ToList();
            }
            if (doctype == 1)
            {
                data = data.Where(i => i.DocID == docid).ToList();
            }
            return Json( new { data });
        }

        public IActionResult QuyenTheoVanBan(string idvb)
        {
            ViewBag.IDVB = idvb;
            return PartialView("_QuyenTheoVanBan");
        }
        #endregion

        #region Phân quyền theo Module hệ thống (Permission)
        public IActionResult QuyenHeThong()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetListHRMUsers()
        {
            return Json(new { data = (await _services.DanhMuc.Get_DM_NguoiSoanThao()).Where(i => i.UserName != null).ToList() });
        }

        [HttpGet]
        public IActionResult VaiTroNguoiDung(DanhMuc_HRMUsers user)
        {
            PhanQuyenVM model = new PhanQuyenVM();
            model.Users = user;
            return PartialView("_VaiTroHeThong", model);
        }

        [HttpGet]
        public async Task<JsonResult> GetListRole(string username)
        {
            return Json(new { data = (await _services.PhanQuyen.GetAllRoles(username)).ToList() });
        }

        [HttpPost]
        public async Task<JsonResult> AddRole(Roles role)
        {
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string success = await _services.PhanQuyen.AddRole(role, user);
            return Json(new { success });
        }

        [HttpPost]
        public async Task<JsonResult> Upsert_RolesInUser([FromBody] List<RolesInUser> listRoles)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                result = await _services.PhanQuyen.UpsertRoleInUse(listRoles, user);

                if (result == "OK")
                {
                    message = $"Đã cập nhật lại quyền";
                    title = "Thành công!";
                    result = "success";
                }
                else
                {
                    message = $"Lỗi! {result}";
                    title = "Lỗi!";
                    result = "error";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }

        public async Task<IActionResult> Permission(Roles role)
        {
            PhanQuyenVM model = new PhanQuyenVM();
            model.Role = role;
            model.Area = new SelectList((await _services.PhanQuyen.GetAllRoles()), "RoleName", "RoleName");
            return PartialView("_ModuleHeThong", model);
        }

        [HttpGet]
        public async Task<JsonResult> GetListPermission(string roleID, string roleName)
        {
            var data = (await _services.PhanQuyen.GetListPermission(roleID)).Where(i => i.Area.Contains(roleName)).OrderBy(i => i.Area).ToList();
            return Json(new { data });
        }

        [HttpPost]
        public async Task<JsonResult> AddPermission(ModulePhanMem permission)
        {
            permission.Area = Request.Form["Area"];
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string success = await _services.PhanQuyen.AddPermission(permission, user);
            return Json(new { success });
        }

        [HttpPost]
        public async Task<JsonResult> Upsert_PermissionsInRole([FromBody] List<PermissionsInRole> listPermission)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                result = await _services.PhanQuyen.UpsertPermissionsInRole(listPermission, user);

                if (result == "OK")
                {
                    message = $"Đã cập nhật lại quyền";
                    title = "Thành công!";
                    result = "success";
                }
                else
                {
                    message = $"Lỗi! {result}";
                    title = "Lỗi!";
                    result = "error";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }
        #endregion
    }
}
