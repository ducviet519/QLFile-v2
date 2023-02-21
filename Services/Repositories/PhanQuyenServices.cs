using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services.Repositories
{
    public class PhanQuyenServices : IPhanQuyenServices
    {
        #region Connection Database
        private readonly IConfiguration _configuration;

        public PhanQuyenServices(IConfiguration configuration)
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

        public async Task<List<ViewRoleList>> Get_ViewRoleList(string docid = null, string leveltype = null)
        {
            List<ViewRoleList> data = new List<ViewRoleList>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<ViewRoleList>("sp_Report_ViewRoleList",
                    new
                    {
                        DocID = docid,
                        LevelType = leveltype,
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

        public async Task<string> Upsert_ViewRole(List<ViewRoleData> listRole, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_ViewRoleUpsert", new
                    {
                        ReportRoleData = listRole.AsTableValuedParameter("dbo.ReportRole", new[] { "LevelType", "DocID", "UserType", "DeptUserGroupID", "HRMUserType", "ReadR", "PrintR", "DownloadR" }),
                        user = user
                    },
                    commandType: CommandType.StoredProcedure);
                    dbConnection.Close();
                    if (data > 0)
                    {
                        result = "OK";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public async Task<List<Roles>> GetAllRoles(string username = null)
        {
            List<Roles> data = new List<Roles>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<Roles>("sp_Auth_Management", new {
                        Action = "GetRolesInUser",
                        UserName = username
                    }, commandType: CommandType.StoredProcedure)).ToList();
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

        public async Task<string> AddRole(Roles roles, string user)
        {
            string result = String.Empty;
            string query = "INSERT INTO [Tools].[dbo].[Auth_Roles] ([RoleID] ,[RoleName] ,[Description] ,[CreatedBy] ,[DateCreated]) VALUES(NEWID(),@RoleName, @Description, @user, GETDATE())";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync(query, new 
                    {
                        RoleName = roles.RoleName,
                        Description = roles.Description,
                        user = user
                    });
                    dbConnection.Close();
                    if (data > 0)
                    {
                        result = "OK";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public async Task<string> UpsertRoleInUse(List<RolesInUser> roles, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteScalarAsync("sp_Auth_Management", new
                    {
                        Action = "AddRolesInUser",
                        RoleUser = roles.AsTableValuedParameter("dbo.AuthRoleUser", new[] { "RoleID", "RoleName", "UserName", "Status" }),
                        user = user
                    },
                    commandType: CommandType.StoredProcedure);
                    dbConnection.Close();
                    if (data.ToString() == "0")
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = data.ToString();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public async Task<string> AddPermission(ModulePhanMem permission, string user)
        {
            string result = String.Empty;
            string query = "  INSERT INTO [Tools].[dbo].[Auth_Permissions]([PermissionID], [PermissionName], [PermissionKey], [Area], [Description], [CreatedBy], [DateCreated]) VALUES(NEWID(), @PermissionName, @PermissionKey, @Area, @Description, @user, GETDATE())";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync(query, new
                    {
                        PermissionName = permission.PermissionName,
                        PermissionKey = permission.PermissionKey,
                        Area = permission.Area,
                        Description = permission.Description,
                        user = user
                    });
                    dbConnection.Close();
                    if (data > 0)
                    {
                        result = "OK";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public async Task<List<ModulePhanMem>> GetListPermission(string roleID = null)
        {
            List<ModulePhanMem> data = new List<ModulePhanMem>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<ModulePhanMem>("sp_Auth_Management", new
                    {
                        Action = "PermissionsInRole",
                        RoleID = roleID
                    }, commandType: CommandType.StoredProcedure)).ToList();
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

        public async Task<string> UpsertPermissionsInRole(List<PermissionsInRole> permission, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.ExecuteScalarAsync("sp_Auth_Management", new
                    {
                        Action = "AddPermissionInRole",
                        Permission = permission.AsTableValuedParameter("dbo.AuthPermissionsRole", new[] { "RoleID", "PermissionID", "Status" }),
                        user = user
                    },
                    commandType: CommandType.StoredProcedure);
                    dbConnection.Close();
                    if (data.ToString() == "0")
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = data.ToString();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public async Task<List<PermissionLKey>> GetListPermissionKeys(string username = null)
        {
            List<PermissionLKey> data = new List<PermissionLKey>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<PermissionLKey>("sp_Auth_Management", new
                    {
                        Action = "GetPermissionInUser",
                        UserName = username
                    }, commandType: CommandType.StoredProcedure)).ToList();
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
