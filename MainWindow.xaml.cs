using babadzakisan.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace babadzakisan
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GARDENSTOREEntities dbContext;
        public MainWindow()
        {
            dbContext = new GARDENSTOREEntities();  
            InitializeComponent();
        }

        private  void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow(dbContext);
            registrationWindow.Show();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;
            string email = emailTextBox.Text;

            Users user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password && u.Email == email);

            if (user != null)
            {
                if (MessageBoxResult.OK == MessageBox.Show("Добро пожаловать!"))
                {
                    if (user.RoleID == 2)
                    {
                        UserGardenWindow userGardenWindow = new UserGardenWindow(user, dbContext);
                        userGardenWindow.Show();
                        Close();
                    }
                    else 
                    {
                        AdminGardenStoreWindow admin = new AdminGardenStoreWindow(user, dbContext);
                        admin.Show();
                        Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Введены неверные данные");
            }
        }
    }
}

