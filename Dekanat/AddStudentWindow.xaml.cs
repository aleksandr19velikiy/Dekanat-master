using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using Dekanat.DataModel;
using System;
using System.Collections.Generic;
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

namespace Dekanat
{
    /// <summary>
    /// Логика взаимодействия для AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window, INotifyPropertyChanged
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

        public AddStudentWindow()
        {
            InitializeComponent();

            InfoVisibility = Visibility.Hidden;

            _images.Add("error", new BitmapImage(new Uri("../Resources/error24.png", UriKind.Relative)));
            _images.Add("success", new BitmapImage(new Uri("../Resources/check.png", UriKind.Relative)));

            InstitutesComboBox.SelectionChanged += UpdateStudyGroups;
            AddStudentButton.Click += AddStudentButton_Click;
            CloseButton.Click += CloseButton_Click;

            LastName.GotFocus += HideInfoBlock;
            FirstName.GotFocus += HideInfoBlock;
            MiddleName.GotFocus += HideInfoBlock;
            BirthDate.GotFocus += HideInfoBlock;
            Address.GotFocus += HideInfoBlock;
            MobilePhone.GotFocus += HideInfoBlock;
            InstitutesComboBox.GotFocus += HideInfoBlock;
            StudyGroupsComboBox.GotFocus += HideInfoBlock;
        }

        public AddStudentWindow(MainWindow mainWindow, string connectionString, SqlCredential credential)
            : this()
        {
            _mainWindow = mainWindow;

            _connectionString = connectionString;
            _userCredential = credential;

            InstitutesComboBox.ItemsSource = _mainWindow.Institutes.Select(i => i.InstituteSName).ToList();
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
       
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
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
                new SqlParameter {ParameterName = "@FirstName", Value = FirsName.Text},
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

                var command = new SqlCommand("AddStudent", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(sqlParams);

                try
                {
                    command.ExecuteNonQuery();
                    DisplayInfo("Студента успішно додано!", false);
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

            _mainWindow.AddStudentWindow = null;
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
