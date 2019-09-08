using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class QualificationLevel
    {
        public int? QualificationLevelId { get; set; }
        public string QualificationLevelName { get; set; }

        public static ObservableCollection<QualificationLevel> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<QualificationLevel>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new QualificationLevel
                {
                    QualificationLevelId = rows[i].ItemArray[0] as int?,
                    QualificationLevelName = rows[i].ItemArray[1] as string
                });
            }

            return collection;
        }
    }
}
