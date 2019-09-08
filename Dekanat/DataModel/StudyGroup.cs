using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class StudyGroup
    {
        public int? GroupId { get; set; }
        public int? GroupNum { get; set; }
        public int? GroupQuantity { get; set; }
        public string CourseName { get; set; }
        public string QualificationLevelName { get; set; }
        public string StudyFormName { get; set; }
        public int? InstituteId { get; set; }
        public int? DepartmentId { get; set; }
        public string ElderFirstName { get; set; }
        public string ElderLastName { get; set; }
        public string ElderMiddleName { get; set; }
        public string ElderFullName
        {
            get { return string.Format("{0} {1} {2}", ElderLastName, ElderFirstName, ElderMiddleName); }
        }
        public int? QualificationLevelId { get; set; }
        public int? StudyFormId { get; set; }
        public int? CourseId { get; set; }
        public static ObservableCollection<StudyGroup> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<StudyGroup>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new StudyGroup
                {
                    GroupId = rows[i].ItemArray[0] as int?,
                    GroupNum = rows[i].ItemArray[1] as int?,
                    GroupQuantity = rows[i].ItemArray[2] as int?,
                    CourseName = rows[i].ItemArray[3] as string,
                    QualificationLevelName = rows[i].ItemArray[4] as string,
                    StudyFormName = rows[i].ItemArray[5] as string,
                    InstituteId = rows[i].ItemArray[6] as int?,
                    DepartmentId = rows[i].ItemArray[7] as int?,
                    ElderFirstName = rows[i].ItemArray[8] as string,
                    ElderLastName = rows[i].ItemArray[9] as string,
                    ElderMiddleName = rows[i].ItemArray[10] as string,
                    QualificationLevelId = rows[i].ItemArray[11] as int?,
                    StudyFormId = rows[i].ItemArray[12] as int?,
                    CourseId = rows[i].ItemArray[13] as int?
                });
            }

            return collection;
        }
    }
}
