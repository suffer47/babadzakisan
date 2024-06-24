using babadzakisan.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace babadzakisan.Views
{
    /// <summary>
    /// Логика взаимодействия для CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window, INotifyPropertyChanged
    {
        private GARDENSTOREEntities dbContext;
        private Users user;
        public static ObservableCollection<CartItem> CartItems = new ObservableCollection<CartItem>();

        
        public CartWindow(Users user, GARDENSTOREEntities dbContext)
        {
            InitializeComponent();
            cartListBox.ItemsSource = CartItems;
            this.dbContext = dbContext;
            this.user = user;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (cartListBox.SelectedItem is CartItem selectedCartItem)
            {
                selectedCartItem.RemoveOneFromCart();
                MessageBox.Show($"Удалено одно {selectedCartItem.Product.ProductName} из корзины.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для удаления из корзины.");
            }
        }

        private async void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int totalQuantity = CalculateTotalQuantity();
                dbContext.Orders.Add
                (
                    new Orders { OrderId = dbContext.Orders.Count() + 1, UserId = user.UserID, OrderDate = DateTime.Now, TotalAmount = totalQuantity }
                );
                await dbContext.SaveChangesAsync();
                MessageBox.Show("Заказ оформлен");
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private int CalculateTotalQuantity()
        {
            int totalQuantity = 0;
            foreach (var cartItem in CartItems)
            {
                totalQuantity += cartItem.Quantity;
            }
            return totalQuantity;
        }
    } 
}
