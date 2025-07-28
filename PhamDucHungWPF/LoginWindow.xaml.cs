using BAL;
using DAL.Models;
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
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            // Clear error messages
            txtEmailError.Text = "";
            txtPasswordError.Text = "";

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(email))
            {
                txtEmailError.Text = "Email is required.";
                hasError = true;
            }
            else if (!IsValidEmail(email))
            {
                txtEmailError.Text = "Invalid email format.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                txtPasswordError.Text = "Password is required.";
                hasError = true;
            }

            if (hasError) return;

            // Load Admin info from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var adminEmail = config["AdminAccount:Email"];
            var adminPassword = config["AdminAccount:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                ProductWindow productWindow = new ProductWindow();
                productWindow.Show();
                this.Close();
                return;
            }

            // Check login thường
            if (_accountService.Login(email, password))
            {
                var customer = _accountService.GetByEmail(email);
                if (customer != null)
                {
                    OrderWindow win = new OrderWindow(customer.CustomerId);
                    win.Show();
                    this.Close();
                }
                else
                {
                    txtEmailError.Text = "Error retrieving account data.";
                }
            }
            else
            {
                txtPasswordError.Text = "Invalid email or password.";
            }
        }


        private bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^\S+@\S+\.\S+$");
        }

    }
}