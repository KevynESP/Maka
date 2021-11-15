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
    /// Lógica de interacción para InicioSesion.xaml
    /// </summary>
    public partial class InicioSesion : Window
    {
        public InicioSesion()
        {
            if (App.connxx.State == System.Data.ConnectionState.Closed && App.connxxt1.State == System.Data.ConnectionState.Closed && App.connxxt2.State == System.Data.ConnectionState.Closed && App.connxxt3.State == System.Data.ConnectionState.Closed && App.connxxt4.State == System.Data.ConnectionState.Closed )
            {
                App.connxx.Open();
                App.connxxt2.Open();
                App.connxxt1.Open();
                App.connxxt3.Open();
                App.connxxt4.Open();
            }
            InitializeComponent();
            UserName.Focus();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnRegistrarse_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Window1 w1 = new Window1();
            w1.Show();
            this.Close();
        }

        private void BtnConectar_Click(object sender, RoutedEventArgs e)
        {
            using (BD bd = new BD())
            {
                if (bd.Login(UserName.Text,PassWord.Text) )
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("no");
                }
            }
        }
    }
}
