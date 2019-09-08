using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class Student
    {
        public int? StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}", LastName, FirstName, MiddleName);
            }
        }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string CourseName { get; set; }
        public int? GroupNum { get; set; }
        public string MobilePhone { get; set; }
        public int? StudyGroup { get; set; }

        public static ObservableCollection<Student> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<Student>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new Student
                {
                    StudentId = rows[i].ItemArray[0] as int?,
                    FirstName = rows[i].ItemArray[1] as string,
                    LastName = rows[i].ItemArray[2] as string,
                    MiddleName = rows[i].ItemArray[3] as string,
                    BirthDate = rows[i].ItemArray[4] as DateTime?,
                    Address = rows[i].ItemArray[5] as string,
                    CourseName = rows[i].ItemArray[6] as string,
                    GroupNum = rows[i].ItemArray[7] as int?,
                    MobilePhone = rows[i].ItemArray[8] as string,
                    StudyGroup = rows[i].ItemArray[9] as int?
                });
            }

            return collection;
        }
    }
}
