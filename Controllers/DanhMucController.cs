using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebTools.Models.Entities;
using WebTools.Services;
using WebTools.Extensions;

namespace WebTools.Controllers
{
    public class DanhMucController : Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        public DanhMucController(IUnitOfWork services)
        {
            _services = services;
        }
        #endregion

        #region Select List Json
        public async Task<JsonResult> Get_Select_Depts(string key = null, string term = null, int page = 1)
        {
            var data = (await _services.DanhMuc.Get_DM_Depts()).OrderBy(i => i.KhoaP).Select(i => new { id = i.STT, text = i.KhoaP });
            if (!String.IsNullOrEmpty(term))
            {
                data = data.Where(i => StaticHelper.convertToUnSign(i.text.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower()))).ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = data.FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            else
            {
                data = data.Skip(10 * (page - 1)).Take(10).ToList();
                return Json(new { data });
            }
        }
        public async Task<JsonResult> Get_List_Depts()
        {
            var data = (await _services.DanhMuc.Get_DM_Depts()).OrderBy(i => i.KhoaP);
            return Json(new { data });
        }
        public async Task<JsonResult> Get_Select_LoaiVanBan(string key = null, string term = null, int page = 1)
        {
            var data = (await _services.DanhMuc.Get_LoaiCabinet()).Select(i => new { id = i.ID, text = i.DES });
            if (!String.IsNullOrEmpty(term))
            {
                data = data.Where(i => StaticHelper.convertToUnSign(i.text.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower()))).ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = data.FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            else
            {
                data = data.Skip(10 * (page - 1)).Take(10).ToList();
                return Json(new { data });
            }
        }
        public async Task<JsonResult> Get_Select_DoiTuongApDung(string key = null, string term = null, int page = 1)
        {
            var data = (await _services.DanhMuc.Get_DM_DoiTuongApDung()).OrderBy(i => i.TypeDes).Select(i => new { id = i.ID, text = i.TypeDes });
            if (!String.IsNullOrEmpty(term))
            {
                data = data.Where(i => StaticHelper.convertToUnSign(i.text.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower()))).ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = data.FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            else
            {
                data = data.Skip(10 * (page - 1)).Take(10).ToList();
                return Json(new { data });
            }
        }
        public async Task<JsonResult> Get_List_DoiTuongApDung()
        {
            var data = (await _services.DanhMuc.Get_DM_DoiTuongApDung()).OrderBy(i => i.TypeDes);
            return Json(new { data });
        }
        public async Task<JsonResult> Get_Select_PhamViThongKe(string key = null, string term = null, int page = 1)
        {
            var data = (await _services.DanhMuc.Get_LoaiVanBanHienThi()).OrderBy(i => i.DES).Select(i => new { id = i.ID, text = i.DES });
            if (!String.IsNullOrEmpty(term))
            {
                data = data.Where(i => StaticHelper.convertToUnSign(i.text.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower()))).ToList();
                return Json(new { data });
            }
            else
            {
                data = data.Skip(10 * (page - 1)).Take(10).ToList();
                return Json(new { data });
            }
        }
        public async Task<JsonResult> Get_Select_NguoiSoanThao(string key = null, string term = null, int page = 1)
        {
            var list = (await _services.DanhMuc.Get_DM_NguoiSoanThao()).OrderBy(i => i.MaNV);
            if (!String.IsNullOrEmpty(term))
            {
                var data = list.Where(i => StaticHelper.convertToUnSign(i.MaNV.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower())) || StaticHelper.convertToUnSign(i.HoTen.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower())))
                    .Select(i => new { id = i.ID, text = $"{i.MaNV}|{i.HoTen}" })
                    .ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = list.Select(i => new { id = i.ID, text = $"{i.MaNV}|{i.HoTen}" })
                    .FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            return Json(new { data = list.Skip(10 * (page - 1)).Take(10).Select(i => new { id = i.ID, text = $"{i.MaNV}|{i.HoTen}" }).ToList() });
        }
        public async Task<JsonResult> Get_Select_LoaiBieuMau(string key = null, string term = null, int page = 1)
        {
            var list = (await _services.DanhMuc.Get_DM_LoaiBieuMau());
            if (!String.IsNullOrEmpty(term))
            {
                var data = list.Where(i => StaticHelper.convertToUnSign(i.MoTa.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower())))
                    .Select(i => new { id = i.ID, text = i.MoTa })
                    .ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = list.Select(i => new { id = i.ID, text = i.MoTa })
                    .FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            return Json(new { data = list.Skip(10 * (page - 1)).Take(10).Select(i => new { id = i.ID, text = i.MoTa }).ToList() });
        }
        public async Task<JsonResult> Get_Select_TenVanBan(string key = null, string term = null, int page = 1)
        {
            var list = (await _services.VanBan.Get_ListTenVanBan());
            if (!String.IsNullOrEmpty(term))
            {
                var data = list.Where(i => i.MaVB != null && StaticHelper.RemoveSpecialCharacters(i.MaVB.ToUpper()).Contains(StaticHelper.RemoveSpecialCharacters(term.ToUpper())) || StaticHelper.convertToUnSign(i.TenVB.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower())))
                    .Skip(10 * (page - 1)).Take(10)
                    .ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = list.Select(i => new { id = i.ID, text = $"{i.MaVB}|{i.TenVB}" })
                    .FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            return Json(new { data = list.Skip(10 * (page - 1)).Take(10).ToList() });
        }
        public async Task<JsonResult> Get_Select_NhomQuyen(string key = null, string term = null, int page = 1)
        {
            var list = (await _services.DanhMuc.Get_DM_NhomQuyen()).Select(i => new { id = i.IDGroup, text = i.Groupname });
            if (!String.IsNullOrEmpty(term))
            {
                var data = list.Where(i => StaticHelper.convertToUnSign(i.text.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower())))
                    .ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = list.FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            return Json(new { data = list.Skip(10 * (page - 1)).Take(10).ToList() });
        }
        public async Task<JsonResult> Get_Select_TrangThaiVanBan(string key = null, string term = null, int page = 1)
        {
            var list = (await _services.DanhMuc.Get_DM_TrangThaiVanBan());
            if (!String.IsNullOrEmpty(term))
            {
                var data = list.Where(i => StaticHelper.convertToUnSign(i.MoTa.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower())))
                    .Select(i => new { id = i.ID, text = i.MoTa })
                    .ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = list.Select(i => new { id = i.ID, text = i.MoTa })
                    .FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            return Json(new { data = list.Skip(10 * (page - 1)).Take(10).Select(i => new { id = i.ID, text = i.MoTa }).ToList() });
        }
        public async Task<JsonResult> Get_Select_PhamViThongKeChiTiet(string key = null, string term = null, int page = 1)
        {
            var list = new List<DanhMuc>();
            list.Add(new DanhMuc() { ID = "1", DES = "Đã được kiểm soát" });
            list.Add(new DanhMuc() { ID = "2", DES = "Đến hạn kiểm soát" });
            list.Add(new DanhMuc() { ID = "3", DES = "Phát hành mới" });
            list.Add(new DanhMuc() { ID = "4", DES = "Cập nhật phiên bản" });
            list.Add(new DanhMuc() { ID = "5", DES = "Ngưng sử dụng" });

            if (!String.IsNullOrEmpty(term))
            {
                var data = list.Where(i => StaticHelper.convertToUnSign(i.DES.ToLower()).Contains(StaticHelper.convertToUnSign(term.ToLower())))
                    .Select(i => new { id = i.ID, text = i.DES })
                    .ToList();
                return Json(new { data });
            }
            if (!String.IsNullOrEmpty(key) && String.IsNullOrEmpty(term))
            {
                var item = list.Select(i => new { id = i.ID, text = i.DES })
                    .FirstOrDefault(i => i.id != null && i.id == key);
                return Json(new { data = item });
            }
            return Json(new { data = list.Skip(10 * (page - 1)).Take(10).Select(i => new { id = i.ID, text = i.DES }).ToList() });
        }
        #endregion



        [ActionName("Cabinet")]
        public IActionResult DanhMucCabinet()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get_DanhMucCabinet()
        {
            return Json(new { data = await _services.DanhMuc.Get_DM_Cabinet() });
        }

        [ActionName("LoaiVanBan")]
        public IActionResult DanhMucQuyenLoaiVB()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get_DanhMucQuyenLoaiVB()
        {
            return Json(new { data = await _services.DanhMuc.Get_DM_QuyenLoaiVB() });
        }

        [ActionName("KhoaPhong")]
        public IActionResult DanhMucKhoaPhong()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get_DanhMucKhoaPhong()
        {
            return Json(new { data = await _services.DanhMuc.Get_DM_KhoaPhong() });
        }
    }
}
