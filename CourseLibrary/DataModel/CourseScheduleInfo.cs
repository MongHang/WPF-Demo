using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.DataModel
{
    public class CourseScheduleInfo
    {
        //course schedule Id
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public DateTime Sdate { get; set; }
        public DateTime Edate { get; set; }
        public int Times { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
