using DAL.Models;
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
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private OrderService orderService = new OrderService();
        private List<Product> availableProducts;
        private int currentCustomerId = -1; // giả định người dùng đang đăng nhập

        public OrderWindow(int customerId)
        {
            InitializeComponent();
            currentCustomerId = customerId;
            LoadProducts();
        }

        private void LoadProducts()
        {
            using var context = new FuminiTikiSystemContext();
            availableProducts = context.Products.Where(p => p.OrderId == null).ToList();
            listBoxProducts.ItemsSource = availableProducts;
            listBoxProducts.DisplayMemberPath = "Name";
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedProducts = listBoxProducts.SelectedItems.Cast<Product>().ToList();
            var productIds = selectedProducts.Select(p => p.ProductId).ToList();

            orderService.PlaceOrder(currentCustomerId, productIds);
            MessageBox.Show("Đặt hàng thành công, meow~ 😽");
            LoadProducts(); // refresh lại list
        }

        private void btnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            var orders = orderService.GetOrdersByCustomer(currentCustomerId);
            listBoxOrders.ItemsSource = orders;
        }

        private void listBoxOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
