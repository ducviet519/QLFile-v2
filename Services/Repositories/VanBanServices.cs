using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services.Repositories
{
    public class VanBanServices : IVanBanServices
    {
        #region Database Connection
        private readonly IConfiguration _configuration;
        public VanBanServices(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("ToolsDB");
            provideName = "System.Data.SqlClient";
        }
        public string ConnectionString { get; }
        public string provideName { get; }
        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }
        #endregion

        public async Task<List<BangTinVanBan>> Get_BangTinVanBan(string user, string loaivb = null)
        {
            List<BangTinVanBan> data = new List<BangTinVanBan>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<BangTinVanBan>("sp_Report_Mainscreen_List",
                    new
                    {
                        LoaiVB = loaivb,
                        user = user,
                    },
                    commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }
        public async Task<List<VanBan_PhienBan>> Get_DanhSachPhienBan(string idvb)
        {
            List<VanBan_PhienBan> data = new List<VanBan_PhienBan>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<VanBan_PhienBan>("sp_Report_DocDetail",
                    new
                    {
                        DocID = idvb,
                    },
                    commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }
        public async Task<List<PreviewVanBan>> Preview_VanBan(string user, string idvanban = null)
        {
            List<PreviewVanBan> data = new List<PreviewVanBan>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<PreviewVanBan>("sp_Report_Preview",
                    new
                    {
                        IDVanBan = idvanban,
                        user = user,
                    },
                    commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }
        public async Task<List<VanBanChiTiet>> Table_VanBanChiTiet(Search_VanBanChiTiet search = null)
        {
            List<VanBanChiTiet> data = new List<VanBanChiTiet>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<VanBanChiTiet>("sp_Report_Detail_List",
                    new
                    {
                        TenVB = search.TenVB,
                        LoaiVB = search.LoaiVB,
                        NgayBHBD = search.NgayBHBD,
                        NgayBHKT = search.NgayBHKT,
                        NgayHLBD = search.NgayHLBD,
                        NgayHLKT = search.NgayHLKT,
                        BPSoanThao = search.BPSoanThao,
                        DonViApDung = search.DonViApDung,
                        DoiTuongApDung = search.DoiTuongApDung,
                        user = search.user,
                    },
                    commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }
        public async Task<List<VanBanChiTiet>> Table_VanBanChiTiet_Google(Search_VanBanChiTiet search = null, List<GoogleDriveFile> table = null)
        {
            List<VanBanChiTiet> data = new List<VanBanChiTiet>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<VanBanChiTiet>("sp_Report_Detail_List_Content",
                    new
                    {
                        TenVB = search.TenVB,
                        LoaiVB = search.LoaiVB,
                        NgayBHBD = search.NgayBHBD,
                        NgayBHKT = search.NgayBHKT,
                        NgayHLBD = search.NgayHLBD,
                        NgayHLKT = search.NgayHLKT,
                        BPSoanThao = search.BPSoanThao,
                        DonViApDung = search.DonViApDung,
                        DoiTuongApDung = search.DoiTuongApDung,
                        user = search.user,
                        FileName = table.AsTableValuedParameter("dbo.ReportFileName", new[] { "FileName" }),
                    },
                    commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }
        public async Task<string> ViewLog(string idvb, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_ViewLog",
                        new
                        {
                            IDVanBan = idvb,
                            user = user
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data > 0)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<string> SendLike(string idvb, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_Like",
                        new
                        {
                            IDVanBan = idvb,
                            user = user
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data > 0)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<string> Upsert_PhienBan(VanBan_PhienBan phienban, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteScalarAsync("sp_Report_Version_Add",
                        new
                        {
                            IDBieuMau = phienban.ID,
                            NgayBH = phienban.NgayBanHanh,
                            NgayHieuLuc = phienban.NgayApDung,
                            FileLink = phienban.FileLink,
                            GhiChu = phienban.GhiChu,
                            PhienBan = phienban.PhienBan,
                            BPSoanThao = phienban.BPSoanThao,
                            NguoiSoanThao = phienban.NguoiSoanThao,
                            User = user
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data.ToString() == "0")
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = data.ToString();
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<string> StatUpdate_PhienBan(string idvb, string idphienban, int cmdtype, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_Version_StatUpdate",
                        new
                        {
                            DocID = idvb,
                            VerID = idphienban,
                            CmdType = cmdtype,
                            User = user
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data > 0)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<string> Upsert_VanBan_ThuMuc(List<VanBan_ID> listID, int IDCabinet, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_DocCabinetUpsert",
                        new
                        {
                            IDVanBan = listID.AsTableValuedParameter("dbo.ReportID", new[] { "ReportID" }),
                            IDCabinet = IDCabinet,
                            User = user
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data > 0)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<List<VanBan_ListName>> Get_ListTenVanBan(string text = null)
        {
            List<VanBan_ListName> data = new List<VanBan_ListName>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<VanBan_ListName>("sp_Report_DocList",
                    new
                    {
                        Text = text
                    },
                    commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }
        public async Task<string> Add_VanBan(VanBan_CRUD vanban, List<VanBan_ID> listID, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteScalarAsync("sp_Report_New",
                        new
                        {                            
                            TenBM = vanban.TenVanBan,
                            MaBM = vanban.MaVanBan,
                            NgayBH = vanban.NgayBanHanh,
                            NgayHieuLuc = vanban.NgayApDung,
                            FileLink = vanban.FileLink,
                            GhiChu = vanban.GhiChu,
                            KhoaPhongSD = vanban.DonViApDung,
                            DoiTuongApDung = vanban.DoiTuongApDung,
                            PhienBan = vanban.PhienBan,
                            TheLoai = vanban.LoaiBieuMau,
                            MainParentID = vanban.ParentID,
                            IDCabinet = vanban.LoaiVanBan,
                            BPSoanThao = vanban.BPSoanThao,
                            NguoiSoanThao = vanban.NguoiSoanThao,
                            Watermark = Convert.ToInt32(vanban.Watermark),
                            TaiLieuLQ = listID.AsTableValuedParameter("dbo.ReportID", new[] { "ReportID" }),
                            User = user
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data.ToString() == "0")
                    {
                        result = "OK";
                    }
                    else { result = data.ToString(); }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<List<VanBan_ByID>> Get_VanBanByID(string docID = null)
        {
            List<VanBan_ByID> data = new List<VanBan_ByID>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<VanBan_ByID>("sp_Report_DocEditView",
                    new
                    {
                        DocID = docID
                    },
                    commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }
        public async Task<string> Update_VanBan(VanBan_CRUD vanban, List<VanBan_ID> listID, string user)
        {
            string result = String.Empty;
            int Watermark = Convert.ToInt32(vanban.Watermark);
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteScalarAsync("sp_Report_Edit",
                        new
                        {
                            IDBieuMau = vanban.ID,
                            TenBM = vanban.TenVanBan,
                            MaBM = vanban.MaVanBan,
                            KhoaPhongSD = vanban.DonViApDung,
                            DoiTuongApDung = vanban.DoiTuongApDung,
                            TheLoai = vanban.LoaiBieuMau,
                            IDCabinet = vanban.LoaiVanBan,
                            Watermark = Watermark,
                            TaiLieuLQ = listID.AsTableValuedParameter("dbo.ReportID", new[] { "ReportID" }),
                            User = user
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data.ToString() == "0")
                    {
                        result = "OK";
                    }
                    else { result = data.ToString(); }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<string> Delete_VanBan(string idvb)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteScalarAsync("sp_Report_Delete",
                        new
                        {
                            IDBieuMau = idvb
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data.ToString() == "0")
                    {
                        result = "OK";
                    }
                    else { result = data.ToString(); }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<string> FileImport(string loaiFile, DataTable table, string user)
        {
            string result = String.Empty;
            var parameter = new DynamicParameters();
            parameter.Add("LoaiFile", loaiFile);
            parameter.Add("UserAcc", user);
            parameter.Add("FileTable", value: table, dbType: DbType.Object);
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteScalarAsync("sp_FileImport", parameter, commandType: CommandType.StoredProcedure);
                    if (data.ToString() == "0")
                    {
                        result = "OK";
                    }
                    else { result = data.ToString(); }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        public async Task<string> UpdateFileLink(string IDFileLink, string FileLink)
        {
            string result = String.Empty;
            string query = "UPDATE [Tools].[dbo].[Report_File] SET FileLink = @FileLink WHERE ID = @IDFileLink";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync(query,
                        new
                        {
                            FileLink = FileLink,
                            IDFileLink = IDFileLink
                        });
                    if (data > 0)
                    {
                        result = "OK";
                    }
                    else { result = data.ToString(); }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
    }
}
