using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class SubjectInfo
    {
        public int? SubjectInfoId { get; set; }
        public decimal? Credits { get; set; }
        public DateTime? SubjectInfoDate { get; set; }
        public string SubjectName { get; set; }
        public string QualificationLevelName { get; set; }
        public string StudyFormName { get; set; }
        public string CourseName { get; set; }
        public int? CourseId { get; set; }
        public int? QualificationLevelId { get; set; }
        public int? StudyFormId { get; set; }

        public static ObservableCollection<SubjectInfo> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<SubjectInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new SubjectInfo
                {
                    SubjectInfoId = rows[i].ItemArray[0] as int?,
                    Credits = rows[i].ItemArray[1] as decimal?,
                    SubjectInfoDate = rows[i].ItemArray[2] as DateTime?,
                    SubjectName = rows[i].ItemArray[3] as string,
                    QualificationLevelName = rows[i].ItemArray[4] as string,
                    StudyFormName = rows[i].ItemArray[5] as string,
                    CourseName = rows[i].ItemArray[6] as string,
                    CourseId = rows[i].ItemArray[7] as int?,
                    QualificationLevelId = rows[i].ItemArray[8] as int?,
                    StudyFormId = rows[i].ItemArray[9] as int?
                });
            }

            return collection;
        }
    }
}
