
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace babadzakisan.Views
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private GARDENSTOREEntities dbContext;
        private byte[] imageData;
        public AddProductWindow(GARDENSTOREEntities dbContext)
        {
            InitializeComponent();
            this.dbContext = dbContext;
        }
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                    productImage.Source = bitmap;

                    imageData = ImageToByteArray(bitmap);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"{ex.ToString()}");
            }

            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                productImage.Source = bitmap;

                imageData = ImageToByteArray(bitmap);
            }
        }
        private byte[] ImageToByteArray(BitmapImage image)
        {
            byte[] data;
            BitmapEncoder encoder;

            if (image != null)
            {
                encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    data = ms.ToArray();
                }
                return data;
            }
            return null;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(productNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(descriptionTextBox.Text) ||
                string.IsNullOrWhiteSpace(priceTextBox.Text) || imageData == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите изображение.");
                return;
            }

            if (decimal.TryParse(priceTextBox.Text, out decimal price))
            {
                Products newProduct = new Products
                {
                    ProductName = productNameTextBox.Text,
                    Description = descriptionTextBox.Text,
                    Price = price,
                    Image = imageData,
                    QuantityAvailable = Convert.ToInt32(totalTextBox.Text), 
                    ProductId = dbContext.Products.Count()+1
                };

                dbContext.Products.Add(newProduct);
                dbContext.SaveChanges();
                MessageBox.Show("Товар успешно добавлен.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите корректную цену.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
