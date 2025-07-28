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
                ProductWindow productWindow = new ProductWindow();
                productWindow.Show();
                this.Close();
                return;
            }

            // Kiểm tra login thường
            if (_accountService.Login(email, password))
            {

                var customer = _accountService.GetByEmail(email);
                if (customer != null)
                {
                    OrderWindow win = new OrderWindow(customer.CustomerId);
                    win.Show();
                    this.Close(); // Đóng login window nếu muốn
                }
                else
                {
                    MessageBox.Show("Lỗi khi lấy thông tin khách hàng!");
                }
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }
    }
}