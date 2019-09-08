using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class Department
    {
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public static ObservableCollection<Department> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<Department>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new Department
                {
                    DepartmentId = rows[i].ItemArray[0] as int?,
                    DepartmentName = rows[i].ItemArray[1] as string
                });
            }

            return collection;
        }
    }
}
