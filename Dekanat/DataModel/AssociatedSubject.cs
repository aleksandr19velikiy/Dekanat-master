using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.DataModel
{
    public class AssociatedSubject
    {
        public int? AssociatedSubjectId { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public int? GroupNum { get; set; }
        public string SubjectName { get; set; }
        public decimal? Credits { get; set; }
        public string QualificationLevelName { get; set; }
        public string StudyFormName { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string TeacherMiddleName { get; set; }
        public string TeacherFullName
        {
            get { return string.Format("{0} {1} {2}", TeacherLastName, TeacherFirstName, TeacherMiddleName); }
        }
        public string DepartmentName { get; set; }
        public string AcademicRankName { get; set; }
        public string GroupName
        {
            get { return string.Format("{0} {1}", CourseName, GroupNum); }
        }
        public string InstituteSName { get; set; }

        public static ObservableCollection<AssociatedSubject> GetCollection(DataRowCollection rows)
        {
            var collection = new ObservableCollection<AssociatedSubject>();

            for (int i = 0; i < rows.Count; i++)
            {
                collection.Add(new AssociatedSubject
                {
                    AssociatedSubjectId = rows[i].ItemArray[0] as int?,
                    CourseId = rows[i].ItemArray[1] as int?,
                    CourseName = rows[i].ItemArray[2] as string,
                    GroupNum = rows[i].ItemArray[3] as int?,
                    SubjectName = rows[i].ItemArray[4] as string,
                    Credits = rows[i].ItemArray[5] as decimal?,
                    QualificationLevelName = rows[i].ItemArray[6] as string,
                    StudyFormName = rows[i].ItemArray[7] as string,
                    TeacherFirstName = rows[i].ItemArray[8] as string,
                    TeacherLastName = rows[i].ItemArray[9] as string,
                    TeacherMiddleName = rows[i].ItemArray[10] as string,
                    DepartmentName = rows[i].ItemArray[11] as string,
                    AcademicRankName = rows[i].ItemArray[12] as string,
                    InstituteSName = rows[i].ItemArray[13] as string
                });
            }

            return collection;
        }
    }
}
