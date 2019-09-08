using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security;
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
using Dekanat.Authorization;
using Dekanat.Serialization;

namespace Dekanat
{
  
    public partial class AuthorizationWindow : Window//p 
    {
        private readonly string _userFilePath
            = string.Format("{0}\\user.txt", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        private readonly SqlConnectionStringBuilder _connectionStringBuilder = new SqlConnectionStringBuilder()//new object or construct
        {
            DataSource = @"DESKTOP-VDBN6O7\SQLEXPRESS",
            InitialCatalog = "dbStudentsFinal",
            IntegratedSecurity = true
        };

        public AuthorizationWindow()
        {
            InitializeComponent();
            TestSqlServerConnectionAsync(_connectionStringBuilder.ConnectionString);
            AutoAuthorization();
            CloseButton.Click += CloseButton_Click;
            EnterButton.Click += EnterButton_Click;

            Login.GotFocus += HideErrorBlock;
            Password.GotFocus += HideErrorBlock;
        }

        private void HideErrorBlock(object sender, EventArgs e)
        {
            ErrorBlock.Visibility = Visibility.Hidden;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            _connectionStringBuilder.IntegratedSecurity = false;

            try
            {
                MainWindow mainWindow;

                var password = Password.SecurePassword;
                password.MakeReadOnly();
                var credential = new SqlCredential(Login.Text, password);
                

                using (var connection = new SqlConnection(_connectionStringBuilder.ConnectionString, credential))
                {
                    connection.Open();

                    mainWindow = new MainWindow(connection.ConnectionString, credential);
                }

                if (RememberMeCheckBox.IsChecked.Value)
                {
                    var userInformation = new UserInformation
                    {
                        Login = Base64Converter.ToBase64(Login.Text),
                        Password = Base64Converter.ToBase64(Password.Password)
                    };

                    DataSerializer.Serialize(_userFilePath, userInformation);
                }
                else
                {
                    File.Delete(_userFilePath);
                }

                mainWindow.Show();
                Close();
            }
            catch(SqlException ex)
            {
                if (ex.Number == 18456) //Login failed for user
                {
                    Login.Text = "";
                    Password.Password = "";
                    ErrorMessage.Text = "Неправильний логін або пароль!";
                    ErrorBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    throw;
                }
            }
        }
        /// <summary>
       // типу, а не певному об'єкту

        /// </summary>
        private void AutoAuthorization()
        {
            if (File.Exists(_userFilePath))
            {
                var userInformation = (UserInformation) DataSerializer.Deserialize(_userFilePath);//не явно 
                Login.Text = Base64Converter.FromBase64(userInformation.Login);
                Password.Password = Base64Converter.FromBase64(userInformation.Password);
                RememberMeCheckBox.IsChecked = true;
            }
        }

        private bool GetSqlServerConnectionStatus(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        private async void TestSqlServerConnectionAsync(string connectionString)
        {
            var sqlServerConnectionStatus = await Task.Run(() => GetSqlServerConnectionStatus(connectionString));

            if (sqlServerConnectionStatus)
            {
                ConnectionStatus.Fill = Brushes.Lime;
                EnterButton.IsEnabled = true;
            }
            else
            {
                ConnectionStatus.Fill = Brushes.Red;
            }
        }
    }
}
