using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;
using Maka2.Views;
using Maka2.Clase_conexion;
using Maka2.Model;
using System.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Maka2
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel datos = new MainViewModel();
        DataTable Chats = new DataTable();
        Timer t1 = new Timer();
        Timer t2 = new Timer();
        Contacto selectedUser;
        DateTime ultimo_enviado;
        bool xsw = false;
        
        public MainWindow()
        {
            InitializeComponent();
            lMensajes.IsReadOnly = true;
            TMensaje.Visibility = Visibility.Hidden;
            lMensajes.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            t1.Interval = 1000;
            t1.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            t2.Interval = 1000;
            t2.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
            t2.Start();
        }

        private void AnadirTexto(string texto,DateTime fecha, int fuente, TextAlignment alineacion)
        {
            this.Dispatcher.Invoke(() =>
            {
                Paragraph para = new Paragraph();

                para.Inlines.Add(texto);
                para.TextAlignment = alineacion;
                if (fuente > 0)
                {
                    Run run = new Run("  " + fecha.ToString("HH:mm:ss"));
                    run.FontSize = fuente;
                    para.Inlines.Add(run);
                }
                
                lMensajes.Document.Blocks.Add(para);
                lMensajes.ScrollToEnd();
            });
        }
        
        private void ActualizarLista()
        {
            this.Dispatcher.Invoke(() =>
            {
                bool sw = false;
                MainViewModel obj2 = new MainViewModel();
                if (obj2.Contactos.Count != datos.Contactos.Count)
                    sw = true;
                else
                { 
                    for (int i = 0; i < obj2.Contactos.Count  ; i++)
                    {
                        if(obj2.Contactos[i].UserName != datos.Contactos[i].UserName 
                        || obj2.Contactos[i].LastMessage != datos.Contactos[i].LastMessage
                        || obj2.Contactos[i].numeroMensajes != datos.Contactos[i].numeroMensajes)
                        {
                            sw = true;
                            break;
                        }
                    }
                }
                if (sw)
                {
                    this.DataContext = null;
                    datos.GetContactos();
                    this.DataContext = datos;
                }
                t2.Start();
            });
        }
        private void OnTimedEvent2(object sender, ElapsedEventArgs e)
        { 
            t2.Stop();
            ActualizarLista();
            
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            t1.Stop();
            if (!xsw) return;
            
            DateTime oDate;
            if (Chats.Rows.Count > 0)
            {
              oDate =(DateTime)Chats.Rows[Chats.Rows.Count - 1][3];
            }
            else
            {
                oDate = Convert.ToDateTime("05/05/1920");
            }

            using (BD datos = new BD())
            {
                DataTable temporal = new DataTable();
                temporal = datos.Refrescar_Chats(selectedUser.UserName.Trim(), oDate);
                if (temporal.Rows.Count > 0)
                {
                    string fecha = oDate.Date.ToString("dd/MM/yyyy");

                    for (int i = 0; i < temporal.Rows.Count; i++)
                    {
                        string fec = ((DateTime)temporal.Rows[i][3]).Date.ToString("dd/MM/yyyy");
                        ultimo_enviado = (DateTime)temporal.Rows[i][3];
                        if (fec != fecha)
                        {
                            AnadirTexto(fec,DateTime.Now,0, TextAlignment.Center);
                            fecha = fec;
                        }

                        if (temporal.Rows[i][0].ToString().Trim() == App.UsuarioLogeado.Trim())
                        {
                            AnadirTexto(temporal.Rows[i][2].ToString().Trim(), ((DateTime)temporal.Rows[i][3]), 8, TextAlignment.Right);
                            //AnadirTexto("", ((DateTime)temporal.Rows[i][3]), 8, TextAlignment.Right);
                        }
                        else
                        {
                            AnadirTexto(temporal.Rows[i][2].ToString().Trim(), ((DateTime)temporal.Rows[i][3]), 8, TextAlignment.Left);
                            //AnadirTexto("", ((DateTime)temporal.Rows[i][3]), 8, TextAlignment.Left);
                        }
                    }
                    //lMensajes.Document = mcFlowDoc;
                    Chats = temporal;
                }     
            }
            t1.Start();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMaximaze_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void EnviarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TMensaje.Text != "")
            {
                using (BD bd = new BD())
                {
                    bd.Enviar_Mensaje(selectedUser.UserName.Trim(), TMensaje.Text);
                    string fecha = ultimo_enviado.ToString("dd/MM/yyyy");
                    string fec = DateTime.Now.ToString("dd/MM/yyyy");
                    if (fec != fecha)
                    {
                        AnadirTexto(fec, DateTime.Now, 0, TextAlignment.Center);
                        ultimo_enviado = DateTime.Now;
                    }
                    AnadirTexto(TMensaje.Text, DateTime.Now, 8, TextAlignment.Right);
                    // AnadirTexto("", DateTime.Now, 8, TextAlignment.Right);
                }
                TMensaje.Text = "";
            }
        }

        private void AñadirUsuario_Click(object sender, RoutedEventArgs e)
        {
            NuevoUser nu = new NuevoUser();
            nu.ShowDialog();
            this.DataContext = null;
            datos.GetContactos();
            this.DataContext =  datos;
        }
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void Cargar_Chat(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            EnviarBtn.IsEnabled = true;
            EnviarBtn.Visibility = Visibility.Visible;
            TMensaje.Visibility = Visibility.Visible;
            
            TMensaje.Focus();

            if (ListaContactos.Items.Count <= 0)
            {
                return;
            }
            xsw = false;
            
           selectedUser = (Contacto) ListaContactos.Items[ListaContactos.SelectedIndex];
            lMensajes.Document.Blocks.Clear();
            using (BD datos = new BD())
            {
                Chats = datos.Cargar_Chats(selectedUser.UserName);
                string fecha = "";

                for (int i= 0; i < Chats.Rows.Count; i++)
                {
                    string fec = ((DateTime)Chats.Rows[i][3]).Date.ToString("dd/MM/yyyy");
                    if (fec != fecha)
                    {
                        AnadirTexto(fec, DateTime.Now, 0, TextAlignment.Center);
                        fecha = fec;
                    }
                    ultimo_enviado = (DateTime)Chats.Rows[i][3];

                    if (Chats.Rows[i][0].ToString().Trim() == App.UsuarioLogeado.Trim())
                    {
                        AnadirTexto(Chats.Rows[i][2].ToString().Trim(), ((DateTime)Chats.Rows[i][3]), 8, TextAlignment.Right);

                       // AnadirTexto("", ((DateTime)Chats.Rows[i][3]), 8, TextAlignment.Right);
                    }
                    else
                    {
                        AnadirTexto(Chats.Rows[i][2].ToString().Trim(), ((DateTime)Chats.Rows[i][3]), 8, TextAlignment.Left);
                        //AnadirTexto("", ((DateTime)Chats.Rows[i][3]), 8, TextAlignment.Left);
                    }
                }
                //lMensajes.Document.Blocks.Add(mcFlowDoc);
            }
            lMensajes.ScrollToEnd();
            LblUser.Content = selectedUser.UserName;
            //ultimo=ultima fila del dt en la columna fecha
            t1.Start();
            xsw = true;
            

        }

        private void TMensaje_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EnviarBtn_Click(sender, e);
            }
        }
    }
}
