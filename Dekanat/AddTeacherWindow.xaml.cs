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
    /// Логика взаимодействия для AddTeacherWindow.xaml
    /// </summary>
    public partial class AddTeacherWindow : Window, INotifyPropertyChanged
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

        public AddTeacherWindow()
        {
            InitializeComponent();

            InfoVisibility = Visibility.Hidden;

            _images.Add("error", new BitmapImage(new Uri("../Resources/error24.png", UriKind.Relative)));
            _images.Add("success", new BitmapImage(new Uri("../Resources/check.png", UriKind.Relative)));

            InstitutesComboBox.SelectionChanged += UpdateDepartments;
            AddTeacherButton.Click += AddTeacherButton_Click;
            CloseButton.Click += CloseButton_Click;

            LastName.GotFocus += HideInfoBlock;
            FirstName.GotFocus += HideInfoBlock;
            MiddleName.GotFocus += HideInfoBlock;
            BirthDate.GotFocus += HideInfoBlock;
            Address.GotFocus += HideInfoBlock;
            MobilePhone.GotFocus += HideInfoBlock;
            InstitutesComboBox.GotFocus += HideInfoBlock;
            DepartmentsComboBox.GotFocus += HideInfoBlock;
            AcademicRanksComboBox.GotFocus += HideInfoBlock;
        }

        public AddTeacherWindow(MainWindow mainWindow, string connectionString, SqlCredential credential)
            : this()
        {
            _mainWindow = mainWindow;

            _connectionString = connectionString;
            _userCredential = credential;

            InstitutesComboBox.ItemsSource = _mainWindow.Institutes.Select(i => i.InstituteSName).ToList();
            AcademicRanksComboBox.ItemsSource = _mainWindow.AcademicRanks.Select(ar => ar.AcademicRankName).ToList();
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
       
        private void AddTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            if (LastName.Text == "" || FirstName.Text == "" || MiddleName.Text == ""
                || Address.Text == "")
            {
                DisplayInfo("Заповнено не всі обов'язкові поля!", true);
                return;
            }

            if (BirthDate.SelectedDate == null)
            {
                DisplayInfo("Дату народження викладача не вказано!", true);
                return;
            }
            if (DepartmentsComboBox.SelectedIndex == -1)
            {
                DisplayInfo("Кафедру викладача не вибрано!", true);
                return;
            }
            if (AcademicRanksComboBox.SelectedIndex == -1)
            {
                DisplayInfo("Академічний ранг викладача не вибрано!", true);
                return;
            }

            var sqlParams = new[]
            {
                new SqlParameter {ParameterName = "@FirstName", Value = FirstName.Text},
                new SqlParameter {ParameterName = "@LastName", Value = LastName.Text},
                new SqlParameter {ParameterName = "@MiddleName", Value = MiddleName.Text},
                new SqlParameter {ParameterName = "@BirthDate", Value = BirthDate.SelectedDate},
                new SqlParameter {ParameterName = "@Address", Value = Address.Text},
                new SqlParameter {ParameterName = "@MobilePhone", Value = MobilePhone.Text},
                new SqlParameter
                {
                    ParameterName = "@DepartmentId",
                    Value = Departments[DepartmentsComboBox.SelectedIndex].DepartmentId
                },
                new SqlParameter
                {
                    ParameterName = "@AcademicRankId",
                    Value = _mainWindow.AcademicRanks[AcademicRanksComboBox.SelectedIndex].AcademicRankId
                }
            };

            using (var connection = new SqlConnection(_connectionString, _userCredential))
            {
                connection.Open();

                var command = new SqlCommand("AddTeacher", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(sqlParams);

                try
                {
                    command.ExecuteNonQuery();
                    DisplayInfo("Викладача успішно додано!", false);
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            _mainWindow.UpdateTeachersDataGrid(this, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _mainWindow.AddTeacherWindow = null;
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
