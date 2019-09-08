using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Windows.Shapes;
using Dekanat.DataModel;

namespace Dekanat
{
    /// <summary>
    /// Логика взаимодействия для EditAssociatedSubjectWindow.xaml
    /// </summary>
    public partial class EditAssociatedSubjectWindow : Window, INotifyPropertyChanged
    {
        private readonly SqlCredential _userCredential;
        private readonly string _connectionString;
        private readonly MainWindow _mainWindow;
        private readonly AssociatedSubject _selectedAssociatedSubject;

        private Visibility _infoVisibility;
        public Visibility InfoVisibility
        {
            get
            {
                return _infoVisibility;
            }
            set
            {
                _infoVisibility = value;
                OnPropertyChanged("InfoVisibility");
            }
        }

        private BitmapImage _infoImage;
        public BitmapImage InfoImage
        {
            get
            {
                return _infoImage;
            }
            set
            {
                _infoImage = value;
                OnPropertyChanged("InfoImage");
            }
        }

        private string _infoMessage;
        public string InfoMessage
        {
            get
            {
                return _infoMessage;
            }
            set
            {
                _infoMessage = value;
                OnPropertyChanged("InfoMessage");
            }
        }

        private readonly Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

        private ObservableCollection<StudyGroup> _studyGroups;
        public ObservableCollection<StudyGroup> StudyGroups
        {
            get { return _studyGroups; }
            set { _studyGroups = value;
                OnPropertyChanged("StudyGroups");
            }
        }

        private ObservableCollection<SubjectInfo> _subjects;
        public ObservableCollection<SubjectInfo> Subjects
        {
            get { return _subjects; }
            set
            {
                _subjects = value;
                OnPropertyChanged("Subjects");
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

        public EditAssociatedSubjectWindow()
        {
            InitializeComponent();

            InfoVisibility = Visibility.Hidden;

            _images.Add("error", new BitmapImage(new Uri("../Resources/error24.png", UriKind.Relative)));
            _images.Add("success", new BitmapImage(new Uri("../Resources/check.png", UriKind.Relative)));

            CloseButton.Click += CloseButton_Click;
            InstitutesComboBox.SelectionChanged += UpdateStudyGroups;
            StudyGroupComboBox.SelectionChanged += UpdateSubjects;
            InstitutesComboBox2.SelectionChanged += UpdateDepartments;
            DepartmentsComboBox.SelectionChanged += UpdateTeachers;
            EditAssociatedSubjectButton.Click += EditAssociatedSubjectButton_Click;

            InstitutesComboBox.GotFocus += HideInfoBlock;
            StudyGroupComboBox.GotFocus += HideInfoBlock;
            SubjectsComboBox.GotFocus += HideInfoBlock;
            InstitutesComboBox2.GotFocus += HideInfoBlock;
            DepartmentsComboBox.GotFocus += HideInfoBlock;
            TeachersComboBox.GotFocus += HideInfoBlock;
        }

        public EditAssociatedSubjectWindow(MainWindow mainWindow, string connectionString, SqlCredential credential,
            AssociatedSubject selectedAssociatedSubject)
            : this()
        {
            _mainWindow = mainWindow;

            _connectionString = connectionString;
            _userCredential = credential;
            _selectedAssociatedSubject = selectedAssociatedSubject;

            InstitutesComboBox.ItemsSource = _mainWindow.Institutes.Select(i => i.InstituteSName).ToList();
            InstitutesComboBox2.ItemsSource = _mainWindow.Institutes.Select(i => i.InstituteSName).ToList();

            InstitutesComboBox.SelectedIndex = _mainWindow.InstitutesComboBoxTab5.SelectedIndex;
            StudyGroupComboBox.SelectedItem = selectedAssociatedSubject.GroupName;
            SubjectsComboBox.SelectedItem = selectedAssociatedSubject.SubjectName;
            InstitutesComboBox2.SelectedItem = selectedAssociatedSubject.InstituteSName;
            DepartmentsComboBox.SelectedItem = selectedAssociatedSubject.DepartmentName;
            TeachersComboBox.SelectedItem = selectedAssociatedSubject.AcademicRankName
                + " " + selectedAssociatedSubject.TeacherFullName;
        }

        private void UpdateStudyGroups(object sender, SelectionChangedEventArgs e)
        {
            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = _mainWindow.Institutes[InstitutesComboBox.SelectedIndex].InstituteId
                }
            };

            var studyGroups = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetStudyGroupsByInstitute", "StudyGroupsView", sqlParams).Tables["StudyGroupsView"].Rows;
            StudyGroups = StudyGroup.GetCollection(studyGroups);

            StudyGroupComboBox.ItemsSource = StudyGroups.Select(g => g.CourseName + " " + g.GroupNum).ToList();
        }

        private void UpdateSubjects(object sender, SelectionChangedEventArgs e)
        {
            if (StudyGroupComboBox.SelectedIndex == -1)
            {
                SubjectsComboBox.ItemsSource = null;
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@CourseId",
                    Value = StudyGroups[StudyGroupComboBox.SelectedIndex].CourseId
                },
                new SqlParameter
                {
                    ParameterName = "@QualificationLevelId",
                    Value = StudyGroups[StudyGroupComboBox.SelectedIndex].QualificationLevelId
                },
                new SqlParameter
                {
                    ParameterName = "@StudyFormId",
                    Value = StudyGroups[StudyGroupComboBox.SelectedIndex].StudyFormId
                }
            };

            var subjects = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetSubjectInfoByCourseQlSf", "SubjectInfoView", sqlParams).Tables["SubjectInfoView"].Rows;
            Subjects = SubjectInfo.GetCollection(subjects);

            SubjectsComboBox.ItemsSource = Subjects.Select(s => s.SubjectName).ToList();
        }

        private void UpdateDepartments(object sender, SelectionChangedEventArgs e)
        {
            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = _mainWindow.Institutes[InstitutesComboBox2.SelectedIndex].InstituteId
                }
            };

            var departments = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetDepartmentsByInstitute", "Departments", sqlParams).Tables["Departments"].Rows;
            Departments = Department.GetCollection(departments);

            DepartmentsComboBox.ItemsSource = Departments.Select(d => d.DepartmentName).ToList();
        }

        private void UpdateTeachers(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentsComboBox.SelectedIndex == -1)
            {
                TeachersComboBox.ItemsSource = null;
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

            var teachers = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetTeachersByDepartment", "Teachers", sqlParams).Tables["Teachers"].Rows;
            Teachers = Teacher.GetCollection(teachers);

            TeachersComboBox.ItemsSource = Teachers.Select(t => t.AcademicRankName + " " + t.FullName).ToList();
        }

        private void EditAssociatedSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudyGroupComboBox.SelectedIndex == -1)
            {
                DisplayInfo("Навчальну групу не вибрано!", true);
                return;
            }

            if (SubjectsComboBox.SelectedIndex == -1)
            {
                DisplayInfo("Дисципліну не вибрано!", true);
                return;
            }

            if (TeachersComboBox.SelectedIndex == -1)
            {
                DisplayInfo("Викладача не вибрано!", true);
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@AssociatedSubjectId",
                    Value = _selectedAssociatedSubject.AssociatedSubjectId
                },
                new SqlParameter
                {
                    ParameterName = "@TeacherId",
                    Value = Teachers[TeachersComboBox.SelectedIndex].TeacherId
                },
                new SqlParameter
                {
                    ParameterName = "@SubjectInfoId",
                    Value = Subjects[SubjectsComboBox.SelectedIndex].SubjectInfoId
                },
                new SqlParameter
                {
                    ParameterName = "@StudyGroupId",
                    Value = StudyGroups[StudyGroupComboBox.SelectedIndex].GroupId
                }
            };

            using (var connection = new SqlConnection(_connectionString, _userCredential))
            {
                connection.Open();

                var command = new SqlCommand("UpdateAssociatedSubject", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(sqlParams);

                try
                {
                    command.ExecuteNonQuery();
                    DisplayInfo("Запис успішно оновлено!", false);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        DisplayInfo("Вибрана дисципліна уже закріплена за групою!", true);
                        return;
                    }

                    Debug.WriteLine(ex.Message);
                }
            }

            _mainWindow.UpdateAssociatedSubjectsDataGrid(this, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _mainWindow.EditAssociatedSubjectWindow = null;
        }

        private void DisplayInfo(string message, bool isError)
        {
            InfoImage = isError ? _images["error"] : _images["success"];
            InfoMessage = message;
            InfoVisibility = Visibility.Visible;
        }

        private void HideInfoBlock(object sender, EventArgs e)
        {
            InfoVisibility = Visibility.Hidden;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
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
