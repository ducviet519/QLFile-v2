using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Extensions;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Models.ViewModel;
using WebTools.Services;

namespace WebTools.Controllers
{
    [Authorize]
    public class VanBanController : Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        private readonly IAuthorizationService _AuthorizationService;
        public VanBanController(IUnitOfWork services, IAuthorizationService AuthorizationService)
        {
            _services = services;
            _AuthorizationService = AuthorizationService;
        }
        #endregion

        #region Bảng tin Văn bản - Trang chính

        public async Task<IActionResult> DanhMuc_VanBan()
        {
            BangTinVanBanVM model = new BangTinVanBanVM();
            model.LoaiVanBanHienThi = await _services.DanhMuc.Get_LoaiVanBanHienThi();
            model.LoaiCabinet = await _services.DanhMuc.Get_LoaiCabinet();
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetData_BangTinVanBan(string loaivb)
        {
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var data = (await _services.VanBan.Get_BangTinVanBan(user, loaivb)).ToList();
            return Json(new { data });
        }

        public JsonResult DanhMuc_VanBan_TimKiem()
        {
            return Json(new { });
        }

        public JsonResult DanhMuc_VanBan_TimKiemNangCao()
        {

            return Json(new { });
        }

        public async Task<JsonResult> Get_Table_VanBan(string param = null)
        {
            var data = (await _services.Report_List.GetReportListAsync()).Take(10).ToList();
            return Json(new { data });
        }

        [Authorize(Roles = "Admin, Document")]
        public IActionResult BangTin_XemXetLai(string idvb, string tenvb, string mavb)
        {
            BangTinVanBan_XemXetLai model = new BangTinVanBan_XemXetLai()
            {
                IDVB = idvb ?? "",
                TenVB = tenvb ?? "",
                MaVB = mavb ?? "",
            };
            return PartialView("_BangTin_XemXetLai", model);
        }

        [HttpPost]
        public JsonResult Upsert_BangTin_XemXetLai(string IDVB, string ngayxemxetlai)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                //
                if (result == "OK")
                {
                    message = $"Đã cập nhật lại vai trò người dùng";
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

        public IActionResult BangTin_ChiTiet(BangTinVanBan vanban)
        {
            BangTinVanBan model = new BangTinVanBan()
            {
                IDVB = vanban.IDVB ?? "",
                TenVB = vanban.TenVB ?? "",
                MaVB = vanban.MaVB ?? "",
                BPSoanThao = vanban.BPSoanThao ?? "",
                DVApDung = vanban.DVApDung ?? "",
                DoiTuongApDung = vanban.DoiTuongApDung ?? "",
            };
            return PartialView("_BangTin_ChiTiet", model);
        }
        #endregion

        #region Danh sách Văn bản chi tiết
        [HttpGet]
        public IActionResult DanhSach_VanBanChiTiet(string loaivb = null)
        {
            VanBanChiTietVM model = new VanBanChiTietVM();
            model.SearchList = new Search_VanBanChiTiet()
            {
                loaitimkiem = "",
                TenVB = "",
                NgayBHBD = "",
                NgayBHKT = "",
                NgayHLBD = "",
                NgayHLKT = "",
                BPSoanThao = "",
                DoiTuongApDung = "",
                DonViApDung = "",
                LoaiVB = loaivb,
            };
            if ((_AuthorizationService.AuthorizeAsync(User, "Permission.ThemPhienBan")).Result.Succeeded) {
                model.themphienban = true;
            }
            if ((_AuthorizationService.AuthorizeAsync(User, "Permission.SuaVanBan")).Result.Succeeded)
            {
                model.suavanban = true;
            }
            if ((_AuthorizationService.AuthorizeAsync(User, "Permission.PhanQuyenVanBan")).Result.Succeeded)
            {
                model.phanquyenvanban = true;
            }
            if ((_AuthorizationService.AuthorizeAsync(User, "Permission.XoaVanBan")).Result.Succeeded)
            {
                model.xoavanban = true;
            }

            ViewBag.exID = loaivb;
            return View(model);
        }

        [HttpPost]
        public IActionResult DanhSach_VanBanChiTiet(Search_VanBanChiTiet search = null)
        {
            VanBanChiTietVM model = new VanBanChiTietVM();
            model.SearchList = new Search_VanBanChiTiet()
            {
                loaitimkiem = search.loaitimkiem ?? "",
                TenVB = search.TenVB ?? "",
                NgayBHBD = search.NgayBHBD ?? "",
                NgayBHKT = search.NgayBHKT ?? "",
                NgayHLBD = search.NgayHLBD ?? "",
                NgayHLKT = search.NgayHLKT ?? "",
                BPSoanThao = search.BPSoanThao ?? "",
                DoiTuongApDung = search.DoiTuongApDung ?? "",
                DonViApDung = search.DonViApDung ?? "",
                LoaiVB = search.LoaiVB ?? "",
            };
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetData_VanBanChiTiet(Search_VanBanChiTiet search = null)
        {
            search.user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var data = (await _services.VanBan.Table_VanBanChiTiet(search)).ToList();
            return Json(new { data });
        }

        public async Task<JsonResult> DoiThuMuc_VanBanChiTiet([FromBody] VanBan_DoiThuMuc data)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                result = await _services.VanBan.Upsert_VanBan_ThuMuc(data.listID, data.listCabinet[0].CabinetID, user);
                if (result == "OK")
                {
                    message = $"Đã cập nhật lại trạng thái phiên bản";
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

        #region Thư mục
        [HttpGet]
        [Authorize(Roles = "Admin, Tree")]
        public IActionResult ThuMuc_ThemMoi(int? parentid)
        {
            ThuMuc model = new ThuMuc();
            model.parentid = parentid;
            return PartialView("_ThuMuc_ThemMoi", model);
        }

        [HttpGet]
        public async Task<JsonResult> Get_ThuMucCha(int? parentid, string type = null)
        {
            if(!String.IsNullOrEmpty(type) && type == "2")
            {
                return Json(new { rows = (await _services.ThuMuc.GetList_ThuMuc(parentid, type)).ToList() });
            }
            return Json(new { rows = (await _services.ThuMuc.GetList_ThuMuc(parentid, "1")).ToList() });
        }

        [HttpPost]
        public async Task<JsonResult> Add_ThuMuc(ThuMuc thumuc)
        {
            string message = "";
            string title = "";
            string result = "";
            try
            {
                result = await _services.ThuMuc.Add_ThuMuc(thumuc);
                if (result == "OK")
                {
                    message = $"Đã thêm thư mục:<b>{thumuc.foldername}</b>";
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

        [HttpGet]
        [Authorize(Roles = "Admin, Tree")]
        public IActionResult ThuMuc_DoiTen(int id, string foldername)
        {
            ThuMuc model = new ThuMuc();
            model.id = id;
            model.foldername = foldername;
            return PartialView("_ThuMuc_DoiTen", model);
        }

        [HttpPost]
        public async Task<JsonResult> Rename_ThuMuc(ThuMuc thumuc)
        {
            string message = "";
            string title = "";
            string result = "";
            try
            {
                result = await _services.ThuMuc.Rename_ThuMuc(thumuc);
                if (result == "OK")
                {
                    message = $"Đổi tên thư mục thành công";
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

        [HttpGet]
        [Authorize(Roles = "Admin, Tree")]
        public IActionResult ThuMuc_ChuyenNhom(int id)
        {
            ThuMuc model = new ThuMuc();
            model.id = id;
            return PartialView("_ThuMuc_ChuyenNhom", model);
        }

        [HttpPost]
        public async Task<JsonResult> Switch_ThuMuc(ThuMuc thumuc)
        {
            string message = "";
            string title = "";
            string result = "";
            try
            {
                result = await _services.ThuMuc.Switch_ThuMuc(thumuc);
                if (result == "OK")
                {
                    message = $"Đã chuyển thư mục sang thư mục mới";
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

        [HttpGet]
        public IActionResult ThuMuc_ChiTiet(int id)
        {
            ThuMuc model = new ThuMuc();
            model.id = id;
            return PartialView("_ThuMuc_ChiTiet", model);
        }
        #endregion

        #region Văn bản
        [HttpGet]
        [Authorize(Roles = "Admin, Document")]
        public IActionResult ImportExcel()
        {
            return PartialView("_VanBan_ThemExcel");
        }

        [HttpPost]
        public async Task<JsonResult> UploadExcelFile(IFormFile fileUpload)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            FileImport data = new FileImport();
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                if (fileUpload != null)
                {
                    data = await _services.UploadFile.ReadExcelFile(fileUpload);
                    if (data.status == "OK")
                    {
                        string check = await _services.VanBan.FileImport("1", data.dataExcels, user);
                        message = $"Lấy thành công {data.dataExcels.Rows.Count} văn bản";
                        title = "Thành công!";
                        result = "success";
                    }
                    else
                    {
                        message = data.status;
                        title = "Lỗi!";
                        result = "error";
                    }
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


        [HttpGet]
        [Authorize(Roles = "Admin, Document")]
        public IActionResult ThemVanBan(int? parentid)
        {
            ViewBag.ParentID = parentid;
            return PartialView("_VanBan_ThemMoi");
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<JsonResult> Upsert_VanBan(VanBan_CRUD fromData)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            List<VanBan_ID> listID = JsonConvert.DeserializeObject<List<VanBan_ID>>(fromData.listID);
            try
            {
                fromData.FileLink = await _services.UploadFile.UploadFileAsync(fromData.fileUpload);
                if (fromData.fileUpload != null && String.IsNullOrEmpty(fromData.FileLink))
                {
                    message = $"File <b>{fromData.fileUpload.FileName}</b> tải lên không thành công. Vui lòng kiểm tra lại file.";
                    title = "Lỗi!";
                    result = "error";
                    return Json(new { Result = result, Title = title, Message = message });
                }

                result = await _services.VanBan.Add_VanBan(fromData, listID, user);
                if (result == "OK")
                {
                    message = $"Đã cập nhật lại trạng thái phiên bản";
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

        [HttpGet]
        public IActionResult TaiLieuLienQuan()
        {
            return PartialView("_TaiLieuLienQuan");
        }


        #endregion

        #region Tiện ích văn bản
        [Authorize(Roles = "Admin, Document")]
        public async Task<IActionResult> VanBan_PhienBan(string idvb)
        {
            VanBanChiTietVM model = new VanBanChiTietVM();
            var data = await _services.VanBan.Get_DanhSachPhienBan(idvb);
            model.VanBanInfo = data.FirstOrDefault();
            ViewBag.PhienBan = Convert.ToInt32(data.FirstOrDefault().PhienBan.Substring(data.FirstOrDefault().PhienBan.IndexOf("V") + 1)) + 1;
            return PartialView("_VanBan_PhienBan", model);
        }

        [HttpGet]
        public async Task<JsonResult> Get_DataPhienBan(string idvb)
        {
            return Json(new { data = await _services.VanBan.Get_DanhSachPhienBan(idvb) });
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<JsonResult> Add_PhienBanVB(VanBan_PhienBan vanBan_PhienBan)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                vanBan_PhienBan.FileLink = await _services.UploadFile.UploadFileAsync(vanBan_PhienBan.fileUpload);
                if (vanBan_PhienBan.fileUpload != null && String.IsNullOrEmpty(vanBan_PhienBan.FileLink))
                {
                    message = $"File <b>{vanBan_PhienBan.fileUpload.FileName}</b> tải lên không thành công. Vui lòng kiểm tra lại file.";
                    title = "Lỗi!";
                    result = "error";
                    return Json(new { Result = result, Title = title, Message = message });
                }
                else
                {
                    vanBan_PhienBan.NguoiSoanThao = Request.Form["NguoiSoanThao"];
                    vanBan_PhienBan.BPSoanThao = Request.Form["BPSoanThao2"];
                    result = await _services.VanBan.Upsert_PhienBan(vanBan_PhienBan, user);
                    if (result == "OK")
                    {
                        message = $"Thêm phiên bản: <b>{vanBan_PhienBan.PhienBan}</b>";
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
            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }

        [Authorize(Roles = "Admin, Document")]
        public async Task<IActionResult> VanBan_Sua(string idvb)
        {
            var data = await _services.VanBan.Get_VanBanByID(idvb);
            VanBanVM model = new VanBanVM();
            model.selectDonVi = new SelectList((await _services.DanhMuc.Get_DM_Depts()).OrderBy(i => i.KhoaP), "STT", "KhoaP");
            model.selectDoiTuong = new SelectList((await _services.DanhMuc.Get_DM_DoiTuongApDung()).OrderBy(i => i.TypeDes), "ID", "TypeDes");
            model.selectTheLoai = new SelectList((await _services.DanhMuc.Get_DM_LoaiBieuMau()), "ID", "MoTa");
            model.listVanBan = data.Where(i => i.TenVB != null).ToList();
            model.vanbanInfo = data.FirstOrDefault();
            return PartialView("_VanBan_Sua", model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<JsonResult> Update_VanBan(VanBan_CRUD fromData)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            List<VanBan_ID> listID = JsonConvert.DeserializeObject<List<VanBan_ID>>(fromData.listID);
            try
            {
                result = await _services.VanBan.Update_VanBan(fromData, listID, user);
                if (result == "OK")
                {
                    message = $"Đã cập nhật lại thông tin văn bản";
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

        public async Task<IActionResult> VanBan_ChiTiet(string idvb)
        {
            VanBanChiTietVM model = new VanBanChiTietVM();
            var data = await _services.VanBan.Get_DanhSachPhienBan(idvb);
            model.ListPhienBan = data;
            model.VanBanInfo = data.FirstOrDefault();
            return PartialView("_VanBan_ChiTiet", model);
        }

        [HttpGet]
        public IActionResult VanBan_PhienBan_Delete(string idvb, string idphienban, string trangthai)
        {
            VanBanChiTietVM model = new VanBanChiTietVM();
            model.idvb = idvb;
            model.idphienban = idphienban;
            switch (trangthai)
            {
                case "Đang sử dụng":
                    trangthai = "1";
                    break;
                case "Chờ ban hành":
                    trangthai = "2";
                    break;
                case "Đã hủy":
                    trangthai = "3";
                    break;
                default:
                    trangthai = "0";
                    break;
            }
            model.trangthai = trangthai;
            return PartialView("_VanBan_PhienBan_Delete", model);
        }

        [HttpPost]
        public async Task<JsonResult> PhienBan_Delete(string idvb, string idphienban, int trangthai)
        {
            string message = "";
            string title = "";
            string result = "";
            string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                result = await _services.VanBan.StatUpdate_PhienBan(idvb, idphienban, trangthai, user);
                if (result == "OK")
                {
                    message = $"Đã cập nhật lại trạng thái phiên bản";
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

        [Authorize(Roles = "Admin, Document")]
        public IActionResult VanBan_ThongKe(string idvb)
        {
            VanBanChiTietVM model = new VanBanChiTietVM();
            return PartialView("_VanBan_ThongKe");
        }

        public async Task<JsonResult> VanBan_Delete(string idvb, string tenVB)
        {
            string message = "";
            string title = "";
            string result = "";           
            try
            {
                result = await _services.VanBan.Delete_VanBan(idvb);
                if (result == "OK")
                {
                    message = $"Đã xóa văn bản: {tenVB}";
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

        #region Google Docs API 
        [Authorize(Roles = "Admin")]
        public IActionResult GoogleDocs()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateGoogleDocs(string doctitle, string email)
        {
            var data = await _services.GoogleDriveAPI.CreateDocumentFile(doctitle, email);
            return Json(new { data });
        }

        [HttpPost]
        public async Task<JsonResult> GetFileComments(string fileId)
        {
            var comments = await _services.GoogleDriveV2.GetDriverComments(fileId);
            var data = comments.Select(i => new { FileTitle = i.FileTitle, Author = i.Author.DisplayName, Content = i.Content, createdDate = i.CreatedDate, modifiedDate = i.ModifiedDate }).ToList();
            return Json(new { data });
        }
        #endregion
    }
}
