using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet
{
    public class ProjectModel
    {
        //Getters and setters for the project fields from the database
        public int id { get; set; }
        public int project_id { get; set; }
        public string project_name { get; set; }
        public int project_time { get; set; }

    }
}
