using DAL.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private List<Category> _categories;
        private Product _selectedProduct;


        public ProductWindow()
        {
            InitializeComponent();
            LoadCategories();
            LoadProducts();
        }

        private FuminiTikiSystemContext GetDbContext()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FuminiTikiSystemContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new FuminiTikiSystemContext(optionsBuilder.Options);
        }

        private void LoadCategories()
        {
            using var context = GetDbContext();
            _categories = context.Categories.ToList();
            cbCategory.ItemsSource = _categories;
            cbFormCategory.ItemsSource = _categories;
        }

        private void LoadProducts()
        {
            using var context = GetDbContext();
            lvProducts.ItemsSource = context.Products.Include(p => p.Category).ToList();
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
            cbFormCategory.SelectedIndex = -1;
            lvProducts.SelectedItem = null;
            imgCategory.Source = null;
            _selectedProduct = null;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm()) return;

            using var context = GetDbContext();
            var product = new Product
            {
                Name = txtName.Text.Trim(),
                Price = decimal.Parse(txtPrice.Text),
                Description = txtDescription.Text.Trim(),
                CategoryId = (int)cbFormCategory.SelectedValue
            };
            context.Products.Add(product);
            context.SaveChanges();
            LoadProducts();
            ClearForm();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct == null)
            {
                MessageBox.Show("Please select a product to edit.");
                return;
            }

            if (!ValidateForm()) return;

            using var context = GetDbContext();
            var product = context.Products.Find(_selectedProduct.ProductId);
            if (product != null)
            {
                product.Name = txtName.Text.Trim();
                product.Price = decimal.Parse(txtPrice.Text);
                product.Description = txtDescription.Text.Trim();
                product.CategoryId = (int)cbFormCategory.SelectedValue;


                context.SaveChanges();
                LoadProducts();
                ClearForm();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            // Thêm confirm trước khi xóa
            var result = MessageBox.Show(
                $"Are you sure you want to delete the product \"{_selectedProduct.Name}\"?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            using var context = GetDbContext();
            var product = context.Products.Find(_selectedProduct.ProductId);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
                LoadProducts();
                ClearForm();
            }
        }


        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
            ClearForm();
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            using var context = GetDbContext();
            var keyword = txtSearch.Text.Trim().ToLower();
            lvProducts.ItemsSource = context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(keyword))
                .ToList();
        }

        private void lvProducts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvProducts.SelectedItem is Product product)
            {
                _selectedProduct = product;
                txtName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                txtDescription.Text = product.Description;
                cbFormCategory.SelectedValue = product.CategoryId;
                if (!string.IsNullOrEmpty(product.Category?.Picture))
                {
                    try
                    {
                        imgCategory.Source = new BitmapImage(new Uri(product.Category.Picture));
                    }
                    catch
                    {
                        imgCategory.Source = null;
                    }
                }
                else
                {
                    imgCategory.Source = null;
                }
            }
        }




        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            LoadProducts();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch_TextChanged(sender, null);
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter product name.");
                return false;
            }
            if (!decimal.TryParse(txtPrice.Text, out _))
            {
                MessageBox.Show("Invalid price format.");
                return false;
            }
            if (cbFormCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.");
                return false;
            }
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var CategoryWindow = new CategoryWindow();
                CategoryWindow.Show();
                this.Close();
        }
    }
}
