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

namespace babadzakisan.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        GARDENSTOREEntities dbContext;
        Orders orders;
        public AddOrderWindow(Users user, Orders order, GARDENSTOREEntities dbContext)
        {
            InitializeComponent();
            orders = order;
            this.dbContext = dbContext;
            customerNameTextBox.Text = user.UserName;
            productsComboBox.ItemsSource = dbContext.Products.ToList();
            quantityTextBox.Text = order.TotalAmount.ToString();
        }
        public AddOrderWindow(GARDENSTOREEntities dbContext)
        {
            InitializeComponent();
            this.dbContext = dbContext;
            productsComboBox.ItemsSource = dbContext.Products.ToList();
        }
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string customerName = customerNameTextBox.Text;
            var user = dbContext.Users.FirstOrDefault(a => a.UserName == customerName);
            Products selectedProduct = productsComboBox.SelectedItem as Products;
            if (!int.TryParse(quantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Пожалуйста введите количество.");
                return;
            }
            if (selectedProduct == null)
            {
                MessageBox.Show("Пожалуйста выберите товар.");
                return;
            }
            if (orders != null)
            {
                orders.UserId = user.UserID;
                orders.TotalAmount = Convert.ToInt32(quantityTextBox.Text);
            }
            else 
            {
                Orders newOrder = new Orders
                {
                    UserId = user.UserID,
                    OrderDate = DateTime.Now,
                    OrderId = dbContext.Orders.Count() + 1,
                    TotalAmount = Convert.ToInt32(quantityTextBox.Text)
                };
                dbContext.Orders.Add(newOrder);
            }
            dbContext.SaveChanges();
            MessageBox.Show("Заказ успешно добавлен.");
            this.Close();
        }
    }

}

