using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services.Repositories
{
    public class DanhMucServices : IDanhMucServices
    {
        #region Database Connection
        private readonly IConfiguration _configuration;
        public DanhMucServices(IConfiguration configuration)
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
        
        public async Task<List<DanhMuc>> Get_LoaiCabinet()
        {
            List<DanhMuc> data = new List<DanhMuc>();
            string query = "SELECT ID,Des FROM dbo.Report_LoaiCabinet WHERE Mainscreen = 1";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc>(query)).ToList();
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
        
        public async Task<List<DanhMuc>> Get_LoaiVanBanHienThi()
        {
            List<DanhMuc> data = new List<DanhMuc>();
            string query = "SELECT ID,Des FROM dbo.Report_LoaiVBHienThi";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc>(query)).ToList();
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
        
        public async Task<List<DanhMuc_Depts>> Get_DM_Depts()
        {
            List<DanhMuc_Depts> data = new List<DanhMuc_Depts>();
            string query = "SELECT [STT],[Code],[KhoaP],[Status],[MaKPHCNS],[KhoaPKD] FROM [Tools].[dbo].[Depts]";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_Depts>(query)).ToList();
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
        
        public async Task<List<DanhMuc_DoiTuongApDung>> Get_DM_DoiTuongApDung()
        {
            List<DanhMuc_DoiTuongApDung> data = new List<DanhMuc_DoiTuongApDung>();
            string query = "SELECT ID,TypeDes FROM dbo.HRMUserType WHERE Enabled = 1";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_DoiTuongApDung>(query)).ToList();
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
        
        public async Task<List<DanhMuc_HRMUsers>> Get_DM_NguoiSoanThao()
        {
            List<DanhMuc_HRMUsers> data = new List<DanhMuc_HRMUsers>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_HRMUsers>("sp_HRMUsers",
                    new
                    {

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
        
        public async Task<List<DanhMuc_LoaiBieuMau>> Get_DM_LoaiBieuMau()
        {
            List<DanhMuc_LoaiBieuMau> data = new List<DanhMuc_LoaiBieuMau>();
            string query = "SELECT ID, MoTa FROM dbo.Report_DMBieuMau WHERE TrangThai = 1";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_LoaiBieuMau>(query)).ToList();
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
        
        public async Task<List<DanhMuc_NhomQuyen>> Get_DM_NhomQuyen()
        {
            List<DanhMuc_NhomQuyen> data = new List<DanhMuc_NhomQuyen>();
            string query = "SELECT DISTINCT IDGroup,Groupname FROM dbo.Report_ViewRoleGroup WHERE Status = 1";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_NhomQuyen>(query)).ToList();
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
        
        public async Task<List<DanhMuc_LoaiBieuMau>> Get_DM_TrangThaiVanBan()
        {
            List<DanhMuc_LoaiBieuMau> data = new List<DanhMuc_LoaiBieuMau>();
            string query = "SELECT ID,MoTa FROM dbo.Report_DMTrangThaiVB WHERE TrangThai = 1";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_LoaiBieuMau>(query)).ToList();
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

        public async Task<List<DanhMuc_Cabinet>> Get_DM_Cabinet()
        {
            List<DanhMuc_Cabinet> data = new List<DanhMuc_Cabinet>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_Cabinet>("sp_Report_DMCabinet",
                    new
                    {

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

        public async Task<List<DanhMuc_QuyenLoaiBM>> Get_DM_QuyenLoaiVB()
        {
            List<DanhMuc_QuyenLoaiBM> data = new List<DanhMuc_QuyenLoaiBM>();
            string query = @"SELECT
	                ID
	                ,MoTa AS TenLoaiBM
	                ,CASE WHEN ReadR = 1 THEN N'Có' ELSE '' END AS Doc
	                ,CASE WHEN PrintR = 1 THEN N'Có' ELSE '' END AS InRa
	                ,CASE WHEN DownloadR = 1 THEN N'Có' ELSE '' END AS TaiVe
	                ,CASE WHEN TrangThai = 1 THEN N'Đang sử dụng' ELSE '' END AS TrangThai
                FROM Report_DMBieuMau";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_QuyenLoaiBM>(query)).ToList();
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

        public async Task<List<DanhMuc_KhoaPhong>> Get_DM_KhoaPhong()
        {
            List<DanhMuc_KhoaPhong> data = new List<DanhMuc_KhoaPhong>();
            string query = @"SELECT STT AS ID,KhoaP FROM dbo.Depts";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhMuc_KhoaPhong>(query)).ToList();
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
    }
}
