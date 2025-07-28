using BAL;
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
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        private readonly CategoryService _service;
        private Category _selectedCategory;

        public CategoryWindow()
        {
            InitializeComponent();
            _service = new CategoryService();
            LoadCategories();
        }

        private void LoadCategories()
        {
            dgCategories.ItemsSource = _service.GetAll();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var cat = new Category
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                Picture = txtPicture.Text
            };
            _service.Add(cat);
            LoadCategories();
            ClearFields();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null) return;

            _selectedCategory.Name = txtName.Text;
            _selectedCategory.Description = txtDescription.Text;
            _selectedCategory.Picture = txtPicture.Text;
            _service.Update(_selectedCategory);
            LoadCategories();
            ClearFields();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null)
            {
                MessageBox.Show("Please select a category to delete.");
                return;
            }


            var result = MessageBox.Show(
                $"Are you sure you want to delete the category \"{_selectedCategory.Name}\"?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                _service.Delete(_selectedCategory.CategoryId);
                LoadCategories();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("can not delele :may be this category have conection with product");
            }
        }


        private void dgCategories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedCategory = dgCategories.SelectedItem as Category;
            if (_selectedCategory != null)
            {
                txtName.Text = _selectedCategory.Name;
                txtDescription.Text = _selectedCategory.Description;
                txtPicture.Text = _selectedCategory.Picture;
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtDescription.Clear();
            txtPicture.Clear();
            dgCategories.SelectedIndex = -1;
            _selectedCategory = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow();
            productWindow.Show();
            this.Close();
        }
    }
}
