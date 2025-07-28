using BAL;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly AccountService _accountService = new();
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            // Clear lỗi cũ
            txtNameError.Text = "";
            txtEmailError.Text = "";
            txtPasswordError.Text = "";

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(name))
            {
                txtNameError.Text = "Full name is required.";
                hasError = true;
            }
            else if (!IsValidFullName(name))
            {
                txtNameError.Text = "Name must be like 'Nguyen Van A'";
                hasError = true;
            }

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
            else if (password.Length < 6)
            {
                txtPasswordError.Text = "Password must be at least 6 characters.";
                hasError = true;
            }

            if (hasError) return;

            if (_accountService.Register(name, email, password, out string message))
            {
                MessageBox.Show(message, "Success");
                new LoginWindow().Show();
                this.Close();
            }
            else
            {
                txtEmailError.Text = message;
            }
        }

        private bool IsValidFullName(string name)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(name, @"^[A-Z][a-z]+(?:\s[A-Z][a-z]+)+$");
        }


        private bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^\S+@\S+\.\S+$");
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
