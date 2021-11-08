using Maka2.Clase_conexion;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            //
            if (File.Exists(@"C:\Users\Kekon\Documents\clase\MakaPruebas\usuario.txt"))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
                this.Close();
            }
            
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
            this.WindowState = WindowState.Minimized;
        }

        private void Registrarse(object sender, RoutedEventArgs e)
        {
            if (PassWord.Text.Equals(RPassWord.Text))
            {
                if (string.IsNullOrEmpty(PassWord.Text))
                    MessageBox.Show("Debe indicar un password");
                else
                    using (BD bd = new BD())
                    {
                        if (bd.Registrar(UserName.Text, PassWord.Text))
                        {
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("error agregarndo usuario");
                        }
                    }



            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden");
            }
            
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnInicio_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            InicioSesion is1 = new InicioSesion();
            is1.Show();
            this.Close();
        }
    }
}
