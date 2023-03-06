using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet
{
    public class PersonModel
    {
        public int person_id { get; set; }
        public string person_name { get; set; }
        public List<ProjectModel> projects { get; set; }
    }
}
