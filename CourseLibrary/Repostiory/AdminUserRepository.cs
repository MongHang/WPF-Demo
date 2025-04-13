using CourseLibrary.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace CourseLibrary.Repostiory
{
    public class AdminUserRepository
    {
        private string _dbConnString;
        public AdminUserRepository(string dbConnString)
        {
            _dbConnString = dbConnString;
        }

        public AdminUserInfo GetAdminUser(string userName)
        {
            AdminUserInfo adminUser = null;

            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT id,username,password,email FROM sysadmin WHERE UserName = @UserName";
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    adminUser = new AdminUserInfo()
                    {
                        Id = Guid.Parse(dr["id"].ToString()),
                        UserName = dr["username"].ToString(),
                        Password = dr["password"].ToString(),
                        Email = dr["email"].ToString()
                    };
                }
                dr.Close();
            }

            return adminUser;
        }

        public bool UpdatePassword(Guid adminUserId, string newPwdHash)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE sysadmin SET password = @Password WHERE id = @Id";
                cmd.Parameters.AddWithValue("@Password", newPwdHash);
                cmd.Parameters.AddWithValue("@Id", adminUserId);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public ObservableCollection<AdminUserInfo> Query(AdminUserQueryParameter adminUserQueryParameter)
        {
            var _adminUsers = new ObservableCollection<AdminUserInfo>();

            var querySql = " SELECT id,username,password,email FROM sysadmin ";
            var whereSql = string.Empty;

            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;

                if (!string.IsNullOrWhiteSpace(adminUserQueryParameter.UserName))
                {
                    if (!string.IsNullOrWhiteSpace(whereSql))
                    {
                        whereSql += " AND ";
                    }
                    whereSql += " username like @UserName ";
                    cmd.Parameters.AddWithValue("@UserName", $"%{adminUserQueryParameter.UserName}%");
                }

                if (!string.IsNullOrWhiteSpace(adminUserQueryParameter.Email))
                {
                    if (!string.IsNullOrWhiteSpace(whereSql))
                    {
                        whereSql += " AND ";
                    }
                    whereSql += " email like @Email ";
                    cmd.Parameters.AddWithValue("@Email", $"%{adminUserQueryParameter.Email}%");
                }

                if (!string.IsNullOrWhiteSpace(whereSql))
                {
                    querySql = $"{querySql} WHERE {whereSql}";
                }
                querySql += " order by username ";

                cmd.CommandText = querySql;
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var adminUser = new AdminUserInfo()
                    {
                        Id = Guid.Parse(dr["id"].ToString()),
                        UserName = dr["username"].ToString(),
                        Password = dr["password"].ToString(),
                        Email = dr["email"].ToString()
                    };
                    _adminUsers.Add(adminUser);
                }
                dr.Close();
            }

            return _adminUsers;
        }

        public bool CreateAdminUser(AdminUserInfo adminUser)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO sysadmin(id,username,password,email) VALUES(@Id,@UserName,@Password,@Email)";
                cmd.Parameters.AddWithValue("@Id", adminUser.Id);
                cmd.Parameters.AddWithValue("@UserName", adminUser.UserName);
                cmd.Parameters.AddWithValue("@Password", adminUser.Password);
                cmd.Parameters.AddWithValue("@Email", adminUser.Email);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public bool UpdateAdminUser(AdminUserInfo adminUser)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE sysadmin SET username = @UserName, email = @Email WHERE id = @Id";
                cmd.Parameters.AddWithValue("@Id", adminUser.Id);
                cmd.Parameters.AddWithValue("@UserName", adminUser.UserName);

                if (!string.IsNullOrWhiteSpace(adminUser.Email))
                {
                    cmd.Parameters.AddWithValue("@Email", adminUser.Email);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                }

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public bool DeleteAdminUser(Guid adminUserId)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM sysadmin WHERE id = @Id";
                cmd.Parameters.AddWithValue("@Id", adminUserId);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }
    }

    public class AdminUserQueryParameter
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime RegDate { get; set; }
    }

}
