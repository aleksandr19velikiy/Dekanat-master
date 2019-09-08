using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window, INotifyPropertyChanged
    {
        private readonly SqlCredential _userCredential;
        private readonly string _connectionString;
        private readonly MainWindow _mainWindow;

        private Visibility _registrationInfoVisibility;
        public Visibility RegistrationInfoVisibility
        {
            get
            {
                return _registrationInfoVisibility;
            }
            set
            {
                _registrationInfoVisibility = value;
                OnPropertyChanged("RegistrationInfoVisibility");
            }
        }

        private BitmapImage _registrationInfoImage;
        public BitmapImage RegistrationInfoImage
        {
            get
            {
                return _registrationInfoImage;
            }
            set
            {
                _registrationInfoImage = value;
                OnPropertyChanged("RegistrationInfoImage");
            }
        }

        private string _registrationInfoMessage;
        public string RegistrationInfoMessage
        {
            get
            {
                return _registrationInfoMessage;
            }
            set
            {
                _registrationInfoMessage = value;
                OnPropertyChanged("RegistrationInfoMessage");
            }
        }

        private readonly Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();

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

        public RegistrationWindow()
        {
            InitializeComponent();

            RegistrationInfoVisibility = Visibility.Hidden;

            _images.Add("error", new BitmapImage(new Uri("../Resources/error24.png", UriKind.Relative)));
            _images.Add("success", new BitmapImage(new Uri("../Resources/check.png", UriKind.Relative)));

            CloseButton.Click += CloseButton_Click;
            CreateUserButton.Click += CreateUserButton_Click;
            Username.GotFocus += HideRegistrationInfoBlock;
            Password1.GotFocus += HideRegistrationInfoBlock;
            Password2.GotFocus += HideRegistrationInfoBlock;
        }

        public RegistrationWindow(MainWindow mainWindow, string connectionString, SqlCredential credential)
            : this()
        {
            _mainWindow = mainWindow;

            _connectionString = connectionString;
            _userCredential = credential;

            var teachers = SqlServer.SqlServer.ExecuteStoredProcedure(connectionString, credential,
                "GetTeachers", "Teachers").Tables["Teachers"].Rows;
            Teachers = Teacher.GetCollection(teachers);

            TeachersList.ItemsSource = Teachers.Select(t => t.AcademicRankName + " " + t.FullName).ToList();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _mainWindow.RegistrationWindow = null;
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (Password1.Password != Password2.Password)
            {
                DisplayRegistrationInfo("Введені паролі не співпадають!", true);
                return;
            }

            if (UserType1.IsChecked.Value)
            {
                var command = new SqlCommand("CreateUser") { CommandType = CommandType.StoredProcedure };

                command.Parameters.AddRange(new[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@username",
                        Value = Username.Text
                    },
                    new SqlParameter
                    {
                        ParameterName = "@password",
                        Value = Password1.Password
                    }
                });


                using (var connection = new SqlConnection(_connectionString, _userCredential))
                {
                    command.Connection = connection;
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();

                        DisplayRegistrationInfo("Користувача успішно створено!", false);
                    }
                    catch (SqlException ex)
                    {
                        string errorMessage;

                        switch (ex.Number)
                        {
                            case 15025:
                                errorMessage = "Користувач з даним ім'ям входу вже існує!";
                                break;
                            case 15118:
                                errorMessage = "Недостатньно складний пароль!";
                                break;
                            case 1038:
                                errorMessage = "Введіть ім'я входу!";
                                break;
                            default:
                                throw;
                        }

                        DisplayRegistrationInfo(errorMessage, true);
                    }
                }
            }
            else
            {
                if (Username.Text == "")
                {
                    DisplayRegistrationInfo("Введіть ім'я входу!", true);
                    return;
                }

                if (Password1.Password == "")
                {
                    DisplayRegistrationInfo("Недостатньно складний пароль!", true);
                    return;
                }

                if (TeachersList.SelectedIndex == -1)
                {
                    DisplayRegistrationInfo("Викладача не вибрано!", true);
                    return;
                }

                if (Teachers[TeachersList.SelectedIndex].HasCredential == true)
                {
                    DisplayRegistrationInfo("Викладача уже зареєстровано!", true);
                    return;
                }

                var command = new SqlCommand("CreateCredential") { CommandType = CommandType.StoredProcedure };
                
                command.Parameters.AddRange(new[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@username",
                        Value = Username.Text
                    },
                    new SqlParameter
                    {
                        ParameterName = "@password",
                        Value = GetSHA1(Password1.Password)
                    },
                    new SqlParameter
                    {
                        ParameterName = "@TeacherId",
                        Value = Teachers[TeachersList.SelectedIndex].TeacherId
                    }
                });


                using (var connection = new SqlConnection(_connectionString, _userCredential))
                {
                    command.Connection = connection;
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();

                        DisplayRegistrationInfo("Користувача успішно створено!", false);
                        Teachers[TeachersList.SelectedIndex].HasCredential = true;

                        _mainWindow.UpdateTeachersDataGrid(this, null);
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number != 2627) throw;

                        DisplayRegistrationInfo("Користувач з даним ім'ям входу вже існує!", true);
                    }
                }
            }
        }

        private void DisplayRegistrationInfo(string message, bool isError)
        {
            RegistrationInfoImage = isError ? _images["error"] : _images["success"];
            RegistrationInfoMessage = message;
            RegistrationInfoVisibility = Visibility.Visible;
        }

        private void HideRegistrationInfoBlock(object sender, EventArgs e)
        {
            RegistrationInfoVisibility = Visibility.Hidden;
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

        private static string GetSHA1(string text)
        {
            var csp = new SHA1CryptoServiceProvider();
            csp.ComputeHash(Encoding.UTF8.GetBytes(text));
            var hash = csp.Hash;

            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}