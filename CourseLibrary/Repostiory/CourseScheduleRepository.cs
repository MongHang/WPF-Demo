using CourseLibrary.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.Repostiory
{
    public class CourseScheduleRepository
    {
        private string _dbConnString;
        public CourseScheduleRepository(string dbConnString)
        {
            _dbConnString = dbConnString;
        }

        public ObservableCollection<CourseScheduleInfo> Query(CourseScheduleQueryParameter courseScheduleQueryParameter)
        {
            var courseScheduleList = new ObservableCollection<CourseScheduleInfo>();

            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;

                var querySql = @"select csh.id,c.code,c.name as cname,t.name as tname,csh.sdate,csh.edate
                                    ,c.times,csh.location,c.description
                                    from courseschedule as csh
                                    inner join course as c on csh.courseid = c.id
                                    inner join teacher as t on csh.teacherid = t.id";
                var whereSql = string.Empty;

                if (!string.IsNullOrWhiteSpace(courseScheduleQueryParameter.CourseName))
                {
                    if (!string.IsNullOrWhiteSpace(whereSql))
                    {
                        whereSql += " AND ";
                    }
                    whereSql += " c.name like @CourseName ";
                    cmd.Parameters.AddWithValue("@CourseName", $"%{courseScheduleQueryParameter.CourseName}%");
                }

                if (!string.IsNullOrWhiteSpace(courseScheduleQueryParameter.TeacherName))
                {
                    if (!string.IsNullOrWhiteSpace(whereSql))
                    {
                        whereSql += " AND ";
                    }
                    whereSql += " t.name like @TeacherName ";
                    cmd.Parameters.AddWithValue("@TeacherName", $"%{courseScheduleQueryParameter.TeacherName}%");
                }
                if (!string.IsNullOrWhiteSpace(whereSql))
                {
                    querySql = $"{querySql} WHERE {whereSql}";
                }
                querySql += " order by csh.sdate desc ";

                cmd.CommandText = querySql;
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    courseScheduleList.Add(new CourseScheduleInfo()
                    {
                        Id = Guid.Parse(dr["id"].ToString()),
                        Code = dr["code"].ToString(),
                        CourseName = dr["cname"].ToString(),
                        TeacherName = dr["tname"].ToString(),
                        Times = int.Parse(dr["times"].ToString()),
                        Sdate = DateTime.Parse(dr["sdate"].ToString()),
                        Edate = DateTime.Parse(dr["edate"].ToString()),
                        Location = dr["location"].ToString(),
                        Description = dr["description"].ToString()
                    });
                }
                dr.Close();
            }

            return courseScheduleList;
        }


        public bool CreateSchedule(CourseScheduleCreateModel courseScheduleCreateModel)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"insert into courseschedule (id,courseid,teacherid,sdate,edate,location)
                                    values(@id,@courseid,@teacherid,@sdate,@edate,@location)";

                cmd.Parameters.AddWithValue("@id", courseScheduleCreateModel.Id);
                cmd.Parameters.AddWithValue("@courseid", courseScheduleCreateModel.CourseId);
                cmd.Parameters.AddWithValue("@teacherid", courseScheduleCreateModel.TeacherId);
                cmd.Parameters.AddWithValue("@sdate", courseScheduleCreateModel.Sdate);
                cmd.Parameters.AddWithValue("@edate", courseScheduleCreateModel.Edate);
                cmd.Parameters.AddWithValue("@location", courseScheduleCreateModel.Location);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public bool IsexistScheduleByTeacher(Guid teacherId, Guid courseId, DateTime sDate, DateTime eDate)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select id from courseschedule
                                    where courseid=@courseid 
                                    and teacherid=@teacherid 
                                    and @sdate<=edate
                                    and @edate>=sdate";
                cmd.Parameters.AddWithValue("@courseid", courseId);
                cmd.Parameters.AddWithValue("@teacherid", teacherId);
                cmd.Parameters.AddWithValue("@sdate", sDate);
                cmd.Parameters.AddWithValue("@edate", eDate);
                cmd.Connection.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return true;
                }
                dr.Close();
            }
            return false;
        }

        public bool UpdateSchedule(CourseScheduleInfo courseScheduleInfo)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"update courseschedule set sdate=@sdate,edate=@edate,location=@location where id=@id";
                cmd.Parameters.AddWithValue("@id", courseScheduleInfo.Id);
                cmd.Parameters.AddWithValue("@sdate", courseScheduleInfo.Sdate);
                cmd.Parameters.AddWithValue("@edate", courseScheduleInfo.Edate);
                cmd.Parameters.AddWithValue("@location", courseScheduleInfo.Location);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public bool DelSchedule(Guid courseScheduleId)
        {
            using (var conn = new SqlConnection(_dbConnString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"delete courseschedule where id=@id";
                cmd.Parameters.AddWithValue("@id", courseScheduleId);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            return true;
        }
    }

    public class CourseScheduleQueryParameter
    {
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
    }
}
