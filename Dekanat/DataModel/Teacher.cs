using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class Teacher
    {
        public int? TeacherId { get; set; }
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
        public string MobilePhone { get; set; }
        public int? DepartmentId { get; set; }
        public string AcademicRankName { get; set; }
        public bool? HasCredential { get; set; }

        public static ObservableCollection<Teacher> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<Teacher>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new Teacher
                {
                    TeacherId = rows[i].ItemArray[0] as int?,
                    FirstName = rows[i].ItemArray[1] as string,
                    LastName = rows[i].ItemArray[2] as string,
                    MiddleName = rows[i].ItemArray[3] as string,
                    BirthDate = rows[i].ItemArray[4] as DateTime?,
                    Address = rows[i].ItemArray[5] as string,
                    MobilePhone = rows[i].ItemArray[6] as string,
                    DepartmentId = rows[i].ItemArray[7] as int?,
                    AcademicRankName = rows[i].ItemArray[8] as string,
                    HasCredential = rows[i].ItemArray[9] as bool?
                });
            }

            return collection;
        }
    }
}
