using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class AcademicRank
    {
        public AcademicRank()
        {
        }

        public int? AcademicRankId { get; set; }
        public string AcademicRankName { get; set; }

        public static ObservableCollection<AcademicRank> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<AcademicRank>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new AcademicRank
                {
                    AcademicRankId = rows[i].ItemArray[0] as int?,
                    AcademicRankName = rows[i].ItemArray[1] as string
                });
            }

            return collection;
        }
    }
}
