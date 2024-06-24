using babadzakisan.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babadzakisan.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity;

        public Products Product { get; set; }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public CartItem(Products product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void RemoveOneFromCart()
        {
            if (Quantity > 1)
            {
                Quantity--;
            }
            else
            {
                CartWindow.CartItems.Remove(this); 
            }
        }
    }
}
