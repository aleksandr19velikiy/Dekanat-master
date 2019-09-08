using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class Course
    {
        public int? CourseId { get; set; }
        public string CourseName { get; set; }

        public static ObservableCollection<Course> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<Course>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new Course
                {
                    CourseId = rows[i].ItemArray[0] as int?,
                    CourseName = rows[i].ItemArray[1] as string
                });
            }

            return collection;
        }
    }
}
