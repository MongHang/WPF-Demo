using CourseLibrary.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.Repostiory
{
    public class TeacherRepository
    {
        private string _dbConnString;

        public TeacherRepository(string dbConnString)
        {
            _dbConnString = dbConnString;
        }
        public ObservableCollection<TeacherInfo> Query()
        {
            var result = new ObservableCollection<TeacherInfo>();
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select id,name from teacher";
                cmd.Connection.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(
                        new TeacherInfo()
                        {
                            Id = Guid.Parse(dr["id"].ToString()),
                            Name = dr["name"].ToString(),
                        });
                }
                dr.Close();
            }
            return result;
        }
    }
}
