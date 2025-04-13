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
    public class CourseRepository
    {
        private string _dbConnString;

        public CourseRepository(string dbConnString)
        {
            _dbConnString = dbConnString;
        }

        public ObservableCollection<CourseInfo> Query()
        {
            var result = new ObservableCollection<CourseInfo>();

            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select id,name from course ";
                cmd.Connection.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(
                        new CourseInfo()
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
