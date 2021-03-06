using Maka2.Clase_conexion;
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

namespace Maka2.Views
{
    /// <summary>
    /// Lógica de interacción para NuevoUser.xaml
    /// </summary>
    public partial class NuevoUser : Window
    {
        public NuevoUser()
        {
            InitializeComponent();

        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (BD bd = new BD())
            {
                bd.NContacto(UserName.Text.ToString());
                this.Close();
            }
        }
    }
}
