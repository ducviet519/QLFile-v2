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
    public class ThongKeServices : IThongKeServices
    {
        #region Database Connection
        private readonly IConfiguration _configuration;
        public ThongKeServices(IConfiguration configuration)
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
        public async Task<List<ThongKe_BieuDo>> Get_DataBieuDo()
        {
            List<ThongKe_BieuDo> data = new List<ThongKe_BieuDo>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<ThongKe_BieuDo>("sp_Report_Counting",
                    new { },
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

        public async Task<List<ThongKe_BaoCaoDocHieu>> GetData_BaoCaoDocHieu(SearchThongKe search)
        {
            List<ThongKe_BaoCaoDocHieu> data = new List<ThongKe_BaoCaoDocHieu>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<ThongKe_BaoCaoDocHieu>("sp_Report_BaoCaoDocHieu",
                    new
                    {
                        NgayBHBD = search.NgayBHBD,
	                    NgayBHKT = search.NgayBHKT,
	                    NgayXNBD = search.NgayXNBD,
	                    NgayXNKT = search.NgayXNKT,
	                    NgayDocBD = search.NgayDocBD,
	                    NgayDocKT = search.NgayDocKT,
	                    DoiTuong = search.DoiTuong,
	                    DonVi = search.DonVi,
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

        public async Task<List<ThongKe_PhamViVB>> GetData_ThongKePhamVi(SearchThongKe search)
        {
            List<ThongKe_PhamViVB> data = new List<ThongKe_PhamViVB>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<ThongKe_PhamViVB>("sp_Report_BaoCaoTKTheoPhamVi",
                    new
                    {
                        NgayBHBD = search.NgayBHBD,
                        NgayBHKT = search.NgayBHKT,
                        DoiTuong = search.DoiTuong,
                        DonVi = search.DonVi,
                        LoaiVB = search.LoaiVB,
                        PhamVi = search.PhamVi,
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

        public async Task<List<ThongKe_TongHop>> GetData_ThongKeTongHop(SearchThongKe search)
        {
            List<ThongKe_TongHop> data = new List<ThongKe_TongHop>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<ThongKe_TongHop>("sp_Report_BaoCaoTKTH",
                    new
                    {
                        NgayBD = search.NgayBHBD,
                        NgayKT = search.NgayBHKT,
                        DoiTuong = search.DoiTuong,
                        DonVi = search.DonVi,
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
    }
}
