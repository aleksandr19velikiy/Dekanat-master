using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dekanat.DataModel;

namespace Dekanat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly SqlCredential _userCredential;
        private readonly string _connectionString;
        public RegistrationWindow RegistrationWindow { private get; set; }
        public AddStudentWindow AddStudentWindow { private get; set; }
        public EditStudentWindow EditStudentWindow { private get; set; }
        public AddGroupWindow AddGroupWindow { private get; set; }
        public EditGroupWindow EditGroupWindow { private get; set; }
        public AddSubjectInfoWindow AddSubjectInfoWindow { private get; set; }
        public EditSubjectInfoWindow EditSubjectInfoWindow { private get; set; }
        public AddTeacherWindow AddTeacherWindow { private get; set; }
        public EditTeacherWindow EditTeacherWindow { private get; set; }
        public AddAssociatedSubjectWindow AddAssociatedSubjectWindow { private get; set; }
        public EditAssociatedSubjectWindow EditAssociatedSubjectWindow { private get; set; }

        private int _serverPermission;
        public int ServerPermission
        {
            get { return _serverPermission; }
            set
            {
                _serverPermission = value;
                OnPropertyChanged("ServerPermission");
            }
        }

        private string _currentUserName;
        public string CurrentUserName
        {
            get { return _currentUserName; }
            set
            {
                _currentUserName = value;
                OnPropertyChanged("CurrentUserName");
            }
        }

        #region DataModelCollections

        private ObservableCollection<Institute> _institutes;

        public ObservableCollection<Institute> Institutes
        {
            get { return _institutes; }
            set
            {
                _institutes = value;
                OnPropertyChanged("Institutes");
            }
        }

        private ObservableCollection<StudyGroup> _studyGroups;

        public ObservableCollection<StudyGroup> StudyGroups
        {
            get { return _studyGroups; }
            set
            {
                _studyGroups = value;
                OnPropertyChanged("StudyGroups");
            }
        }

        private ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged("Students");
            }
        }

        private ObservableCollection<Department> _departments;

        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set
            {
                _departments = value;
                OnPropertyChanged("Departments");
            }
        }

        private ObservableCollection<QualificationLevel> _qualificationLevels;

        public ObservableCollection<QualificationLevel> QualificationLevels
        {
            get { return _qualificationLevels; }
            set
            {
                _qualificationLevels = value;
                OnPropertyChanged("QualificationLevels");
            }
        }

        private ObservableCollection<StudyForm> _studyForms;

        public ObservableCollection<StudyForm> StudyForms
        {
            get { return _studyForms; }
            set
            {
                _studyForms = value;
                OnPropertyChanged("StudyForms");
            }
        }

        private ObservableCollection<StudyGroup> _groups;

        public ObservableCollection<StudyGroup> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged("Groups");
            }
        }

        private ObservableCollection<Course> _courses;

        public ObservableCollection<Course> Courses
        {
            get { return _courses; }
            set
            {
                _courses = value;
                OnPropertyChanged("Courses");
            }
        }

        private ObservableCollection<SubjectInfo> _subjectInfo;

        public ObservableCollection<SubjectInfo> SubjectInfo
        {
            get { return _subjectInfo; }
            set
            {
                _subjectInfo = value;
                OnPropertyChanged("SubjectInfo");
            }
        }

        private ObservableCollection<Department> _departmentsTab4; // :D

        public ObservableCollection<Department> DepartmentsTab4
        {
            get { return _departmentsTab4; }
            set
            {
                _departmentsTab4 = value;
                OnPropertyChanged("DepartmentsTab4");
            }
        }

        private ObservableCollection<Teacher> _teachers;

        public ObservableCollection<Teacher> Teachers
        {
            get { return _teachers; }
            set
            {
                _teachers = value;
                OnPropertyChanged("Teachers");
            }
        }

        private ObservableCollection<AcademicRank> _academicRanks;

        public ObservableCollection<AcademicRank> AcademicRanks
        {
            get { return _academicRanks; }
            set
            {
                _academicRanks = value;
                OnPropertyChanged("AcademicRanks");
            }
        }

        private ObservableCollection<AssociatedSubject> _associatedSubjects;

        public ObservableCollection<AssociatedSubject> AssociatedSubjects
        {
            get { return _associatedSubjects; }
            set
            {
                _associatedSubjects = value;
                OnPropertyChanged("AssociatedSubjects");
            }
        }

        private ObservableCollection<Course> _coursesTab5;

        public ObservableCollection<Course> CoursesTab5
        {
            get { return _coursesTab5; }
            set
            {
                _coursesTab5 = value;
                OnPropertyChanged("CoursesTab5");
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            AddUserButton.Click += AddUserButton_Click;
            ExitButton.Click += ExitButton_Click;

            MainWindowTabControl.SelectionChanged += UpdateCollections;

            InstitutesComboBox.SelectionChanged += UpdateStudyGroups;
            StudyGroupsComboBox.SelectionChanged += UpdateStudentsDataGrid;
            AddStudentButton.Click += AddStudentButton_Click;
            EditStudentButton.Click += EditStudentButton_Click;
            DeleteStudentButton.Click += DeleteStudentButton_Click;

            InstitutesComboBoxTab2.SelectionChanged += UpdateDepartments;
            DepartmentsComboBox.SelectionChanged += UpdateStudyGroupsDataGrid;
            AddGroupButton.Click += AddGroupButton_Click;
            EditGroupButton.Click += EditGroupButton_Click;

            InstitutesComboBoxTab3.SelectionChanged += UpdateCourses;
            CoursesComboBox.SelectionChanged += UpdateSubjectInfoDataGrid;
            AddSubjectInfoButton.Click += AddSubjectInfoButton_Click;
            EditSubjectInfoButton.Click += EditSubjectInfoButton_Click;

            InstitutesComboBoxTab4.SelectionChanged += UpdateDepartmentsTab4;
            DepartmentsComboBoxTab4.SelectionChanged += UpdateTeachersDataGrid;
            AddTeacherButton.Click += AddTeacherButton_Click;
            EditTeacherButton.Click += EditTeacherButton_Click;

            InstitutesComboBoxTab5.SelectionChanged += UpdateCoursesTab5;
            CoursesComboBoxTab5.SelectionChanged += UpdateAssociatedSubjectsDataGrid;
            AddAssociatedSubjectButton.Click += AddAssociatedSubjectButton_Click;
            EditAssociatedSubjectButton.Click += EditAssociatedSubjectButton_Click;
        }

        public MainWindow(string connectionString, SqlCredential credential)
            : this()
        {
            _connectionString = connectionString;
            _userCredential = credential;

            ServerPermission = SqlServer.SqlServer.GetServerPermission(connectionString, credential);
            CurrentUserName = credential.UserId;
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegistrationWindow != null) return;

            RegistrationWindow = new RegistrationWindow(this, _connectionString, _userCredential);
            RegistrationWindow.Show();
        }

        private void UpdateCollections(object sender, SelectionChangedEventArgs e)
        {
            var institutes = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetInstitutes", "Institutes").Tables["Institutes"].Rows;
            Institutes = Institute.GetCollection(institutes);

            InstitutesComboBox.ItemsSource = Institutes.Select(i => i.InstituteSName).ToList();

            var qualificationLevels = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetQualificationLevels", "QualificationLevels").Tables["QualificationLevels"].Rows;
            QualificationLevels = QualificationLevel.GetCollection(qualificationLevels);

            var studyForms = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetStudyForms", "StudyForms").Tables["StudyForms"].Rows;
            StudyForms = StudyForm.GetCollection(studyForms);

            var academicRanks = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetAcademicRanks", "AcademicRanks").Tables["AcademicRanks"].Rows;
            AcademicRanks = AcademicRank.GetCollection(academicRanks);
        }

        #region Tab Students

        public void UpdateStudyGroups(object sender, SelectionChangedEventArgs e)
        {
            if (InstitutesComboBox.SelectedIndex == -1)
            {
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = Institutes[InstitutesComboBox.SelectedIndex].InstituteId
                }
            };

            var studyGroups = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetStudyGroupsByInstitute", "StudyGroupsView", sqlParams).Tables["StudyGroupsView"].Rows;
            StudyGroups = StudyGroup.GetCollection(studyGroups);

            StudyGroupsComboBox.ItemsSource = StudyGroups.Select(g => g.CourseName + " " + g.GroupNum).ToList();
        }

        public void UpdateStudentsDataGrid(object sender, SelectionChangedEventArgs e)
        {
            if (StudyGroupsComboBox.SelectedIndex == -1)
            {
                Students = null;
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@GroupId",
                    Value = StudyGroups[StudyGroupsComboBox.SelectedIndex].GroupId
                }
            };

            var students = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetStudents", "StudentsView", sqlParams);

            Students = Student.GetCollection(students.Tables["StudentsView"].Rows);
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddStudentWindow != null) return;

            AddStudentWindow = new AddStudentWindow(this, _connectionString, _userCredential);
            AddStudentWindow.Show();
        }

        private void EditStudentButton_Click(object sencer, RoutedEventArgs e)
        {
            if (EditStudentWindow != null) return;

            EditStudentWindow = new EditStudentWindow(this, _connectionString, _userCredential,
                Students[StudentInfoDataGrid.SelectedIndex]);
            EditStudentWindow.Show();
        }

        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var sqlParameter = new SqlParameter
            {
                ParameterName = "@StudentId",
                Value = Students[StudentInfoDataGrid.SelectedIndex].StudentId
            };


            var messageBoxText = string.Format("Ви дійсно бажаєте видалити студента: {0}, {1} {2}?",
                Students[StudentInfoDataGrid.SelectedIndex].FullName,
                Students[StudentInfoDataGrid.SelectedIndex].CourseName,
                Students[StudentInfoDataGrid.SelectedIndex].GroupNum);

            var messageBoxResult = MessageBox.Show(messageBoxText, "Підтвердження видалення студента",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (messageBoxResult != MessageBoxResult.Yes) return;

            using (var connection = new SqlConnection(_connectionString, _userCredential))
            {
                connection.Open();

                var command = new SqlCommand("DeleteStudentByStudentId", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(sqlParameter);

                command.ExecuteNonQuery();
            }

            UpdateStudentsDataGrid(this, null);
            UpdateStudyGroupsDataGrid(this, null);
        }

        #endregion

        #region Tab Groups

        private void UpdateDepartments(object sender, SelectionChangedEventArgs e)
        {
            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = Institutes[InstitutesComboBoxTab2.SelectedIndex].InstituteId
                }
            };

            var departments = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetDepartmentsByInstitute", "Departments", sqlParams).Tables["Departments"].Rows;
            Departments = Department.GetCollection(departments);

            DepartmentsComboBox.ItemsSource = Departments.Select(d => d.DepartmentName).ToList();
        }

        public void UpdateStudyGroupsDataGrid(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentsComboBox.SelectedIndex == -1)
            {
                Groups = null;
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@DepartmentId",
                    Value = Departments[DepartmentsComboBox.SelectedIndex].DepartmentId
                }
            };

            var groups = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetStudyGroupsByDepartment", "StudyGroupsView", sqlParams).Tables["StudyGroupsView"].Rows;
            Groups = StudyGroup.GetCollection(groups);
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddGroupWindow != null) return;

            AddGroupWindow = new AddGroupWindow(this, _connectionString, _userCredential);
            AddGroupWindow.Show();
        }

        private void EditGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditGroupWindow != null) return;

            EditGroupWindow = new EditGroupWindow(this, _connectionString, _userCredential,
                Groups[StudyGroupsDataGrid.SelectedIndex]);
            EditGroupWindow.Show();
        }

        #endregion

        #region Tab Subjects

        private void UpdateCourses(object sender, SelectionChangedEventArgs e)
        {
            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = Institutes[InstitutesComboBoxTab3.SelectedIndex].InstituteId
                }
            };

            var courses = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetCoursesByInstitute", "Courses", sqlParams).Tables["Courses"].Rows;
            Courses = Course.GetCollection(courses);

            CoursesComboBox.ItemsSource = Courses.Select(c => c.CourseName).ToList();
        }

        public void UpdateSubjectInfoDataGrid(object sender, SelectionChangedEventArgs e)
        {
            if (CoursesComboBox.SelectedIndex == -1)
            {
                SubjectInfoDataGrid.ItemsSource = null;
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@CourseId",
                    Value = Courses[CoursesComboBox.SelectedIndex].CourseId
                }
            };

            var subjectInfo = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetSubjectInfoByCourse", "SubjectInfo", sqlParams).Tables["SubjectInfo"].Rows;
            SubjectInfo = DataModel.SubjectInfo.GetCollection(subjectInfo);

            SubjectInfoDataGrid.ItemsSource = SubjectInfo;
        }

        private void AddSubjectInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddSubjectInfoWindow != null) return;

            AddSubjectInfoWindow = new AddSubjectInfoWindow(this, _connectionString, _userCredential);
            AddSubjectInfoWindow.Show();
        }

        private void EditSubjectInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditSubjectInfoWindow != null) return;

            EditSubjectInfoWindow = new EditSubjectInfoWindow(this, _connectionString, _userCredential,
                SubjectInfo[SubjectInfoDataGrid.SelectedIndex]);
            EditSubjectInfoWindow.Show();
        }

        #endregion

        #region Tab Teachers

        private void UpdateDepartmentsTab4(object sender, SelectionChangedEventArgs e)
        {
            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = Institutes[InstitutesComboBoxTab4.SelectedIndex].InstituteId
                }
            };

            var departments = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetDepartmentsByInstitute", "Departments", sqlParams).Tables["Departments"].Rows;
            DepartmentsTab4 = Department.GetCollection(departments);

            DepartmentsComboBoxTab4.ItemsSource = DepartmentsTab4.Select(d => d.DepartmentName).ToList();
        }

        public void UpdateTeachersDataGrid(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentsComboBoxTab4.SelectedIndex == -1)
            {
                Teachers = null;
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@DepartmentId",
                    Value = DepartmentsTab4[DepartmentsComboBoxTab4.SelectedIndex].DepartmentId
                }
            };

            var teachers = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetTeachersByDepartment", "TeachersView", sqlParams).Tables["TeachersView"].Rows;
            Teachers = Teacher.GetCollection(teachers);
            Console.WriteLine();
        }

        private void AddTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddTeacherWindow != null) return;

            AddTeacherWindow = new AddTeacherWindow(this, _connectionString, _userCredential);
            AddTeacherWindow.Show();
        }

        private void EditTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditTeacherWindow != null) return;

            EditTeacherWindow = new EditTeacherWindow(this, _connectionString, _userCredential,
                Teachers[TeachersDataGrid.SelectedIndex]);
            EditTeacherWindow.Show();
        }

        #endregion

        #region Tab AssociatedSubjects

        private void UpdateCoursesTab5(object sender, SelectionChangedEventArgs e)
        {
            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = Institutes[InstitutesComboBoxTab5.SelectedIndex].InstituteId
                }
            };

            var courses = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetCoursesByInstitute", "Courses", sqlParams).Tables["Courses"].Rows;
            CoursesTab5 = Course.GetCollection(courses);

            CoursesComboBoxTab5.ItemsSource = CoursesTab5.Select(c => c.CourseName).ToList();
        }

        public void UpdateAssociatedSubjectsDataGrid(object sender, SelectionChangedEventArgs e)
        {
            if (CoursesComboBoxTab5.SelectedIndex == -1)
            {
                AssociatedSubjects = null;
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@CourseId",
                    Value = CoursesTab5[CoursesComboBoxTab5.SelectedIndex].CourseId
                }
            };

            var associatedSubjects = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetAssociatedSubjectsByCourse", "AssociatedSubjectsView", sqlParams).Tables["AssociatedSubjectsView"]
                .Rows;
            AssociatedSubjects = AssociatedSubject.GetCollection(associatedSubjects);
        }

        private void AddAssociatedSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddAssociatedSubjectWindow != null) return;

            AddAssociatedSubjectWindow = new AddAssociatedSubjectWindow(this, _connectionString, _userCredential);
            AddAssociatedSubjectWindow.Show();
        }

        private void EditAssociatedSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditAssociatedSubjectWindow != null) return;

            EditAssociatedSubjectWindow = new EditAssociatedSubjectWindow(this, _connectionString, _userCredential,
                AssociatedSubjects[AssociatedSubjectsDataGrid.SelectedIndex]);
            EditAssociatedSubjectWindow.Show();
        }

        #endregion

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            new AuthorizationWindow().Show();

            if (RegistrationWindow != null) RegistrationWindow.Close();
            if (AddStudentWindow != null) AddStudentWindow.Close();
            if (EditStudentWindow != null) EditStudentWindow.Close();
            if (AddGroupWindow != null) AddGroupWindow.Close();
            if (EditGroupWindow != null) EditGroupWindow.Close();
            if (AddSubjectInfoWindow != null) AddSubjectInfoWindow.Close();
            if (EditSubjectInfoWindow != null) EditSubjectInfoWindow.Close();
            if (AddTeacherWindow != null) AddTeacherWindow.Close();
            if (EditTeacherWindow != null) EditTeacherWindow.Close();
            if (AddAssociatedSubjectWindow != null) AddAssociatedSubjectWindow.Close();
            if (EditAssociatedSubjectWindow != null) EditAssociatedSubjectWindow.Close();

            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}