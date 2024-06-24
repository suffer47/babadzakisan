
using babadzakisan.Windows;
using System.Linq;
using System.Windows;

namespace babadzakisan.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminGardenStoreWindow.xaml
    /// </summary>
    public partial class AdminGardenStoreWindow : Window
    {
        private GARDENSTOREEntities dbContext;
        private Users user;
        public string CustomerName { get; private set; }
        public AdminGardenStoreWindow(Users user, GARDENSTOREEntities dbContext)
        {
            InitializeComponent();
            this.dbContext = dbContext;    
            this.user = user;
            LoadData();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close(); 
        }
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addProductWindow = new AddOrderWindow(dbContext);
            if (addProductWindow.ShowDialog() == false) LoadData();
        }

        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var item = ordersListBox.SelectedItem;
            AddOrderWindow addProductWindow = new AddOrderWindow(user, (Orders)item, dbContext);
            if (addProductWindow.ShowDialog() == false) LoadData();
        }

        private async void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (ordersListBox.SelectedItem is Orders selectedOrder)
            {
                dbContext.Orders.Remove(selectedOrder);
                await dbContext.SaveChangesAsync();
                LoadData(); // Обновление данных после удаления заказа
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.");
            }

        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow(dbContext);
            if (addProductWindow.ShowDialog() == false) LoadData();
        }

        private async void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (plantsListBox.SelectedItem is Products selectedProduct)
            {
                dbContext.Products.Remove(selectedProduct);
                await dbContext.SaveChangesAsync();
                LoadData(); // Обновление данных после удаления товара
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для удаления.");
            }
        }
        private  void LoadData()
        {
            ordersListBox.ItemsSource = dbContext.Orders.ToList();
            plantsListBox.ItemsSource = dbContext.Products.ToList();
        }
    }
}
