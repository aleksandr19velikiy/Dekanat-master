using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class Institute
    {
        public int? InstituteId { get; set; }
        public string InstituteName { get; set; }
        public string InstituteSName { get; set; }

        public static ObservableCollection<Institute> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<Institute>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new Institute
                {
                    InstituteId = rows[i].ItemArray[0] as int?,
                    InstituteName = rows[i].ItemArray[1] as string,
                    InstituteSName = rows[i].ItemArray[2] as string
                });
            }

            return collection;
        }
    }
}
