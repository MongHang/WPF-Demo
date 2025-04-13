using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.Repostiory
{
    public class StudentScheduleRepository
    {
        private string _dbConnString;
        public StudentScheduleRepository(string dbConnString)
        {
            _dbConnString = dbConnString;
        }

        public bool IsExistOnStuSchedule(Guid courseScheduleId)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from stucourseschedule where cscheduleid=@coursescheduleid";
                cmd.Parameters.AddWithValue("@coursescheduleid", courseScheduleId);
                cmd.Connection = conn;
                conn.Open();
                var cnt = (int)cmd.ExecuteScalar();
                return cnt > 0;
            }
        }
    }
}
