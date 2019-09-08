using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DekanatForTeacher
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window, INotifyPropertyChanged
    {
        private Visibility _errorInfoBlockVisibility;
        public Visibility ErrorInfoBlockVisibility
        {
            get { return _errorInfoBlockVisibility; }
            set
            {
                _errorInfoBlockVisibility = value;
                OnPropertyChanged("ErrorInfoBlockVisibility");
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private readonly SqlConnectionStringBuilder _connectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = @"DESKTOP-VDBN6O7\SQLEXPRESS",
            InitialCatalog = "dbStudentsFinal",
            IntegratedSecurity = true
        };

        public AuthorizationWindow()
        {
            InitializeComponent();

            ErrorInfoBlockVisibility = Visibility.Hidden;

            EnterButton.Click += EnterButton_Click;
            CloseButton.Click += CloseButton_Click;

            Login.GotFocus += HideErrorInfoBlock;
            Password.GotFocus += HideErrorInfoBlock;
        }

        private void EnterButton_Click(object sendet, RoutedEventArgs e)
        {
            if (Login.Text == "")
            {
                DisplayErrorInfo("Введіть ім'я входу!");
                return;
            }

            if (Password.Password == "")
            {
                DisplayErrorInfo("Введіть пароль!");
                return;
            }

            string sha1Password;
            int? teacherId;

            var sqlParams = new[]
            {
                new SqlParameter
                {
                    ParameterName = "@username",
                    Value = Login.Text
                },
                new SqlParameter
                {
                    ParameterName = "@password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 40,
                    Direction = ParameterDirection.Output
                },
                new SqlParameter
                {
                    ParameterName = "@TeacherId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                }
            };

            using (var connection = new SqlConnection(_connectionStringBuilder.ConnectionString))
            {
                var command = new SqlCommand("GetCredential", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddRange(sqlParams);

                connection.Open();

                command.ExecuteNonQuery();

                sha1Password = command.Parameters["@password"].Value as string;
                teacherId = command.Parameters["@TeacherId"].Value as int?;
            }

            if (GetSHA1(Password.Password) != sha1Password)
            {
                DisplayErrorInfo("Неправильний логін або пароль!");
            }
            else
            {
                new MainWindow(teacherId).Show();
                Close();
            }
    }

        private void CloseButton_Click(object sendet, RoutedEventArgs e)
        {
            Close();
        }

        private void DisplayErrorInfo(string message)
        {
            ErrorMessage = message;
            ErrorInfoBlockVisibility = Visibility.Visible;
            Login.Text = "";
            Password.Password = "";
        }

        private void HideErrorInfoBlock(object sender, EventArgs e)
        {
            ErrorInfoBlockVisibility = Visibility.Hidden;
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
