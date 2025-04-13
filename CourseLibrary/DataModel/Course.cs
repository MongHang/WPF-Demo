using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.DataModel
{
    public class CourseInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class CourseScheduleCreateModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public DateTime Sdate { get; set; }
        public DateTime Edate { get; set; }
        public string Location { get; set; }

        public CourseScheduleCreateModel()
        {
            this.Id = Guid.NewGuid();
            this.Sdate = DateTime.Now;
            this.Edate = DateTime.Now;
        }
    }
}
