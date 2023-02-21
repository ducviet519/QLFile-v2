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
    public class ThuMucServices : IThuMucServices
    {
        #region Database Connection
        private readonly IConfiguration _configuration;
        public ThuMucServices(IConfiguration configuration)
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
        public async Task<List<ThuMuc>> GetList_ThuMuc(int? parentid, string type)
        {
            List<ThuMuc> data = new List<ThuMuc>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<ThuMuc>("sp_Report_Tree",
                        new
                        {
                            ParentID = parentid,
                            Type = type,
                        }
                        ,commandType: CommandType.StoredProcedure)).ToList();
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

        public async Task<string> Rename_ThuMuc(ThuMuc thumuc)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_CabinetUpSert",
                        new
                        {
                            CmdType = 2,
                            Cabinetname = thumuc.foldername,
                            IDCabinet = thumuc.id
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

        public async Task<string> Switch_ThuMuc(ThuMuc thumuc)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_CabinetUpSert",
                        new
                        {
                            CmdType = 3,
                            ParentID = thumuc.parentid,
                            IDCabinet = thumuc.id
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

        public async Task<string> Add_ThuMuc(ThuMuc thumuc)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_CabinetUpSert",
                        new
                        {
                            CmdType = 1,
                            CabinetName = thumuc.foldername,
                            ParentID = thumuc.parentid
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
    }
}
