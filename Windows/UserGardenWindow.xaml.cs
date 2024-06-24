using babadzakisan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace babadzakisan.Views
{
    /// <summary>
    /// Логика взаимодействия для UserGardenWindow.xaml
    /// </summary>
    public partial class UserGardenWindow : Window
    {
        private Users user;
        private GARDENSTOREEntities dbContext;

        public UserGardenWindow(Users user, GARDENSTOREEntities dbContext)
        {
            InitializeComponent();

            this.dbContext = dbContext;
            this.user = user;
            LoadAllPlants();
        }

        private void LoadAllPlants()
        {
            plantsListBox.ItemsSource =  dbContext.Products.ToList();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private async void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                plantsListBox.ItemsSource = await dbContext.Products.ToListAsync();
            }
            else
            {
                var filteredPlants = await dbContext.Products
                    .Where(p => p.ProductName.ToLower().Contains(searchTerm) || p.Description.ToLower().Contains(searchTerm))
                    .ToListAsync();

                plantsListBox.ItemsSource = filteredPlants;
            }
        }

        private void ViewCartButton_Click(object sender, RoutedEventArgs e)
        {
           new CartWindow(user ,dbContext).ShowDialog();

        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (plantsListBox.SelectedItem is Products selectedPlant)
            {
                var existingCartItem = CartWindow.CartItems.FirstOrDefault(item => item.Product.ProductId == selectedPlant.ProductId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity++;
                }
                else
                {
                    CartWindow.CartItems.Add(new CartItem(selectedPlant, 1)); 
                }

                MessageBox.Show($"{selectedPlant.ProductName} добавлено в корзину.");
            }
        }
    }
}
