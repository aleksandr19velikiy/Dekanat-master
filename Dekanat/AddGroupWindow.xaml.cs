using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;
using Dekanat.DataModel;
using System.Data;
using System.Diagnostics;

namespace Dekanat
{
    /// <summary>
    /// Логика взаимодействия для AddGroupWindow.xaml
    /// </summary>
    public partial class AddGroupWindow : Window, INotifyPropertyChanged
    {
        private readonly SqlCredential _userCredential;
        private readonly string _connectionString;
        private readonly MainWindow _mainWindow;

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

        public AddGroupWindow()
        {
            InitializeComponent();

            InfoVisibility = Visibility.Hidden;

            _images.Add("error", new BitmapImage(new Uri("../Resources/error24.png", UriKind.Relative)));
            _images.Add("success", new BitmapImage(new Uri("../Resources/check.png", UriKind.Relative)));

            CloseButton.Click += CloseButton_Click;
            InstitutesComboBox.SelectionChanged += UpdateDepartments;
            DepartmentsComboBox.SelectionChanged += UpdateCourses;
            AddGroupButton.Click += AddGroupButton_Click;

            InstitutesComboBox.GotFocus += HideInfoBlock;
            DepartmentsComboBox.GotFocus += HideInfoBlock;
            CoursesComboBox.GotFocus += HideInfoBlock;
            GroupNum.GotFocus += HideInfoBlock;
            QualificationLevelsComboBox.GotFocus += HideInfoBlock;
            StudyFormsComboBox.GotFocus += HideInfoBlock;
        }

        public AddGroupWindow(MainWindow mainWindow, string connectionString, SqlCredential credential)
            : this()
        {
            _mainWindow = mainWindow;

            _connectionString = connectionString;
            _userCredential = credential;

            InstitutesComboBox.ItemsSource = _mainWindow.Institutes.Select(i => i.InstituteSName).ToList();
            QualificationLevelsComboBox.ItemsSource =
                _mainWindow.QualificationLevels.Select(ql => ql.QualificationLevelName).ToList();
            StudyFormsComboBox.ItemsSource = _mainWindow.StudyForms.Select(sf => sf.StudyFormName).ToList();
        }

        private void UpdateDepartments(object sender, SelectionChangedEventArgs e)
        {
            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = _mainWindow.Institutes[InstitutesComboBox.SelectedIndex].InstituteId
                }
            };

            var departments = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetDepartmentsByInstitute", "Departments", sqlParams).Tables["Departments"].Rows;
            Departments = Department.GetCollection(departments);

            DepartmentsComboBox.ItemsSource = Departments.Select(d => d.DepartmentName).ToList();
        }

        private void UpdateCourses(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentsComboBox.SelectedIndex == -1)
            {
                CoursesComboBox.ItemsSource = null;
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

            var courses = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetCoursesByDepartment", "Courses", sqlParams).Tables["Courses"].Rows;
            Courses = Course.GetCollection(courses);

            CoursesComboBox.ItemsSource = Courses.Select(c => c.CourseName).ToList();
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesComboBox.SelectedIndex == -1 || GroupNum.Text == ""
                || QualificationLevelsComboBox.SelectedIndex == -1
                || StudyFormsComboBox.SelectedIndex == -1)
            {
                DisplayInfo("Заповнено не всі обов'язкові поля!", true);
                return;
            }

            int groupNum;

            if (!int.TryParse(GroupNum.Text, out groupNum))
            {
                DisplayInfo("Неправильний формат номера групи!", true);
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter { ParameterName = "@GroupNum", Value = groupNum },
                new SqlParameter { ParameterName = "@Course", Value = Courses[CoursesComboBox.SelectedIndex].CourseId },
                new SqlParameter
                {
                    ParameterName = "@QualificationLevel",
                    Value = _mainWindow.QualificationLevels[QualificationLevelsComboBox.SelectedIndex].QualificationLevelId
                },
                new SqlParameter
                {
                    ParameterName = "@StudyForm",
                    Value = _mainWindow.StudyForms[StudyFormsComboBox.SelectedIndex].StudyFormId
                } 
            };

            using (var connection = new SqlConnection(_connectionString, _userCredential))
            {
                connection.Open();

                var command = new SqlCommand("AddGroup", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(sqlParams);

                try
                {
                    command.ExecuteNonQuery();
                    DisplayInfo("Групу успішно додано!", false);
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            _mainWindow.UpdateStudyGroupsDataGrid(this, null);
            _mainWindow.UpdateStudyGroups(this, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _mainWindow.AddGroupWindow = null;
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
