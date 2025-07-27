using BAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace PhamDucHungWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly AccountService _accountService = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            // Load Admin info from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var adminEmail = config["AdminAccount:Email"];
            var adminPassword = config["AdminAccount:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                MessageBox.Show("Welcome Admin!");
                // Mở màn hình quản lý admin
                return;
            }

            // Kiểm tra login thường
            if (_accountService.Login(email, password))
            {
                MessageBox.Show("Login successful!");
                // Mở giao diện khách hàng
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }
    }
}