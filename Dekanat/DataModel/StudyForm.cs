using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class StudyForm
    {
        public int? StudyFormId { get; set; }
        public string StudyFormName { get; set; }

        public static ObservableCollection<StudyForm> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<StudyForm>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new StudyForm
                {
                    StudyFormId = rows[i].ItemArray[0] as int?,
                    StudyFormName = rows[i].ItemArray[1] as string
                });
            }

            return collection;
        }
    }
}
