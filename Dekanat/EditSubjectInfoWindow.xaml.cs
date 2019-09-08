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
    /// Логика взаимодействия для EditSubjectInfoWindow.xaml
    /// </summary>
    public partial class EditSubjectInfoWindow : Window, INotifyPropertyChanged
    {
        private readonly SqlCredential _userCredential;
        private readonly string _connectionString;
        private readonly MainWindow _mainWindow;
        private readonly SubjectInfo _selectedSubjectInfo;

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

        public EditSubjectInfoWindow()
        {
            InitializeComponent();

            InfoVisibility = Visibility.Hidden;

            _images.Add("error", new BitmapImage(new Uri("../Resources/error24.png", UriKind.Relative)));
            _images.Add("success", new BitmapImage(new Uri("../Resources/check.png", UriKind.Relative)));

            CloseButton.Click += CloseButton_Click;
            InstitutesComboBox.SelectionChanged += UpdateCourses;
            EditSubjectInfoButton.Click += EditSubjectInfoButton_Click;

            SubjectName.GotFocus += HideInfoBlock;
            Credits.GotFocus += HideInfoBlock;
            InstitutesComboBox.GotFocus += HideInfoBlock;
            CoursesComboBox.GotFocus += HideInfoBlock;
            QualificationLevelsComboBox.GotFocus += HideInfoBlock;
            StudyFormsComboBox.GotFocus += HideInfoBlock;
            SubjectInfoDate.GotFocus += HideInfoBlock;
        }

        public EditSubjectInfoWindow(MainWindow mainWindow, string connectionString, SqlCredential credential, 
            SubjectInfo selectedSubjectInfo)
            : this()
        {
            _mainWindow = mainWindow;

            _connectionString = connectionString;
            _userCredential = credential;
            _selectedSubjectInfo = selectedSubjectInfo;

            InstitutesComboBox.ItemsSource = _mainWindow.Institutes.Select(i => i.InstituteSName).ToList();
            QualificationLevelsComboBox.ItemsSource =
                _mainWindow.QualificationLevels.Select(ql => ql.QualificationLevelName).ToList();
            StudyFormsComboBox.ItemsSource = _mainWindow.StudyForms.Select(sf => sf.StudyFormName).ToList();

            InstitutesComboBox.SelectedItem = _mainWindow.InstitutesComboBoxTab3.SelectedItem;
            CoursesComboBox.SelectedItem = selectedSubjectInfo.CourseName;
            SubjectName.Text = selectedSubjectInfo.SubjectName;
            Credits.Text = selectedSubjectInfo.Credits.ToString();
            QualificationLevelsComboBox.SelectedItem = selectedSubjectInfo.QualificationLevelName;
            StudyFormsComboBox.SelectedItem = selectedSubjectInfo.StudyFormName;
            SubjectInfoDate.SelectedDate = selectedSubjectInfo.SubjectInfoDate;
        }

        private void UpdateCourses(object sender, SelectionChangedEventArgs e)
        {
            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@InstituteId",
                    Value = _mainWindow.Institutes[InstitutesComboBox.SelectedIndex].InstituteId
                }
            };

            var courses = SqlServer.SqlServer.ExecuteStoredProcedure(_connectionString, _userCredential,
                "GetCoursesByInstitute", "Courses", sqlParams).Tables["Courses"].Rows;
            Courses = Course.GetCollection(courses);

            CoursesComboBox.ItemsSource = Courses.Select(c => c.CourseName).ToList();
        }

        private void EditSubjectInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectName.Text == "" || CoursesComboBox.SelectedIndex == -1
                || QualificationLevelsComboBox.SelectedIndex == -1
                || StudyFormsComboBox.SelectedIndex == -1)
            {
                DisplayInfo("Заповнено не всі обов'язкові поля!", true);
                return;
            }

            decimal credits;

            if (!decimal.TryParse(Credits.Text, out credits))
            {
                DisplayInfo("Неправильний формат кількості кредитів!", true);
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter { ParameterName = "@SubjectName", Value = SubjectName.Text },
                new SqlParameter { ParameterName = "@Credits", Value = credits },
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
                },
                new SqlParameter { ParameterName = "@SubjectInfoDate", Value = SubjectInfoDate.SelectedDate },
                new SqlParameter { ParameterName = "@SubjectInfoId", Value = _selectedSubjectInfo.SubjectInfoId }
            };

            using (var connection = new SqlConnection(_connectionString, _userCredential))
            {
                connection.Open();

                var command = new SqlCommand("UpdateSubjectInfo", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(sqlParams);

                try
                {
                    command.ExecuteNonQuery();
                    DisplayInfo("Навчальну дисципліну успішно оновленно!", false);
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            _mainWindow.UpdateSubjectInfoDataGrid(this, null);
            _mainWindow.UpdateAssociatedSubjectsDataGrid(this, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _mainWindow.EditSubjectInfoWindow = null;
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
