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
using System.Windows.Shapes;
using Dekanat.DataModel;
using System.Diagnostics;

namespace Dekanat
{
    /// <summary>
    /// Логика взаимодействия для EditStudentWindow.xaml
    /// </summary>
    public partial class EditStudentWindow : Window, INotifyPropertyChanged
    {

        private readonly SqlCredential _userCredential;
        private readonly string _connectionString;
        private readonly MainWindow _mainWindow;
        private readonly Student _selectedStudent;

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

        private readonly Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

        public EditStudentWindow()
        {
            InitializeComponent();

            InfoVisibility = Visibility.Hidden;

            _images.Add("error", new BitmapImage(new Uri("../Resources/error24.png", UriKind.Relative)));
            _images.Add("success", new BitmapImage(new Uri("../Resources/check.png", UriKind.Relative)));

            CloseButton.Click += CloseButton_Click;
            InstitutesComboBox.SelectionChanged += UpdateStudyGroups;
            EditStudentButton.Click += EditStudentButton_Click;

            LastName.GotFocus += HideInfoBlock;
            FirstName.GotFocus += HideInfoBlock;
            MiddleName.GotFocus += HideInfoBlock;
            BirthDate.GotFocus += HideInfoBlock;
            Address.GotFocus += HideInfoBlock;
            MobilePhone.GotFocus += HideInfoBlock;
            InstitutesComboBox.GotFocus += HideInfoBlock;
            StudyGroupsComboBox.GotFocus += HideInfoBlock;
        }

        public EditStudentWindow(MainWindow mainWindow, string connectionString, SqlCredential credential,
            Student selectedStudent)
            : this()
        {
            _mainWindow = mainWindow;

            _connectionString = connectionString;
            _userCredential = credential;

            InstitutesComboBox.ItemsSource = _mainWindow.Institutes.Select(i => i.InstituteSName).ToList();
            _selectedStudent = selectedStudent;

            LastName.Text = selectedStudent.LastName;
            FirstName.Text = selectedStudent.FirstName;
            MiddleName.Text = selectedStudent.MiddleName;
            BirthDate.SelectedDate = selectedStudent.BirthDate;
            Address.Text = selectedStudent.Address;
            MobilePhone.Text = selectedStudent.MobilePhone;
            InstitutesComboBox.SelectedIndex = _mainWindow.InstitutesComboBox.SelectedIndex;
            StudyGroupsComboBox.SelectedIndex = _mainWindow.StudyGroupsComboBox.SelectedIndex;
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

            StudyGroupsComboBox.ItemsSource = StudyGroups.Select(g => g.CourseName + " " + g.GroupNum);
        }

        private void EditStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (LastName.Text == "" || FirstName.Text == "" || MiddleName.Text == ""
                || Address.Text == "")
            {
                DisplayInfo("Заповнено не всі обов'язкові поля!", true);
                return;
            }

            if (BirthDate.SelectedDate == null)
            {
                DisplayInfo("Дату народження студента не вказано!", true);
                return;
            }

            if (StudyGroupsComboBox.SelectedIndex == -1)
            {
                DisplayInfo("Групу студента не вибрано!", true);
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter {ParameterName = "@StudentId", Value = _selectedStudent.StudentId}, 
                new SqlParameter {ParameterName = "@FirstName", Value = FirstName.Text},
                new SqlParameter {ParameterName = "@LastName", Value = LastName.Text},
                new SqlParameter {ParameterName = "@MiddleName", Value = MiddleName.Text},
                new SqlParameter {ParameterName = "@BirthDate", Value = BirthDate.SelectedDate},
                new SqlParameter {ParameterName = "@Address", Value = Address.Text},
                new SqlParameter {ParameterName = "@MobilePhone", Value = MobilePhone.Text},
                new SqlParameter
                {
                    ParameterName = "@StudyGroup",
                    Value = StudyGroups[StudyGroupsComboBox.SelectedIndex].GroupId
                }
            };

            using (var connection = new SqlConnection(_connectionString, _userCredential))
            {
                connection.Open();

                var command = new SqlCommand("UpdateStudent", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(sqlParams);

                try
                {
                    command.ExecuteNonQuery();
                    DisplayInfo("Інформацію про студента успішно оновлено!", false);
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            _mainWindow.UpdateStudentsDataGrid(this, null);
            _mainWindow.UpdateStudyGroupsDataGrid(this, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _mainWindow.EditStudentWindow = null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
