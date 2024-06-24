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

namespace babadzakisan
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private GARDENSTOREEntities dbContext;

        public RegistrationWindow(GARDENSTOREEntities dbContext)
        {
            InitializeComponent();
            this.dbContext = dbContext;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Пожалуйста, введите эл.почту, логин и пароль.");
                return;
            }
            if (!email.Contains("@"))
            {
                MessageBox.Show("Пожалуйста, введите корректную эл.почту!");
                return;
            }
            if (await dbContext.Users.AnyAsync(u => u.UserName == username && u.Email == email))
            {
                MessageBox.Show("Это имя пользователя и почта заняты.");
                return;
            }

            Users newUser = new Users
            {
                UserName = username,
                Password = password, 
                Email = email,
                RoleID = 2 
            };

            dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();

            MessageBox.Show("Registration successful.");
        }
    }
}
