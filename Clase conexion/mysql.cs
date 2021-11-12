using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Maka2.Model;
using MySql.Data.MySqlClient;

namespace Maka2.Clase_conexion
{

    class BD : IDisposable
    {
        public ObservableCollection<Contacto> Contactos { get; set; }
        public ObservableCollection<Usuario> Usuarios { get; set; }
        public bool Login(string usuario, string password)
        {
            bool resultado = false;
            string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string sql = "SELECT * FROM Usuarios_Registrados where usuario = '" + usuario.Trim() + "' and password = '" + password.Trim()+"'";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            { 
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                        Maka2.App.UsuarioLogeado = usuario;
                        resultado = true;
                }
             }
            conn.Close();
            return resultado;
        }
        public bool Registrar(string usuario, string name, string password)
        {
            bool resultado = false;
            string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT usuario FROM Usuarios_Registrados where usuario = '"+ usuario.Trim()+"'";
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (!rdr.HasRows)
                    {
                        Maka2.App.UsuarioLogeado = usuario;
                        resultado = true;
                    }
                }
            }


            if (resultado)
            using (MySqlCommand cmd2 = conn.CreateCommand())
            {
                cmd2.CommandText = "insert into Usuarios_Registrados (usuario, nombre, password) values ('" + usuario.Trim() + "', '" + name.Trim()+ "', '" + password.Trim() + "')";
                int rowCount = cmd2.ExecuteNonQuery();

                if (rowCount == 1) //or you can use > 0
                {
                    resultado = true;
                }
            }
            conn.Close();
            return resultado;
        }

        public bool NContacto(string usuario)
        {
            bool nresultado = false;
            bool existe = false;
            if (!usuario.Trim().Equals(App.UsuarioLogeado.Trim()))
            {
                string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT usuario FROM Usuarios_Registrados where usuario = '" + usuario.Trim() + "'";
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            existe = true;
                        }
                        else
                        {
                            MessageBox.Show("Usuario no existe.");
                        }
                    }
                }
                if (existe)
                {
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT usuario_contacto FROM Contactos where usuario = '" + App.UsuarioLogeado.Trim() + "' and usuario_contacto = '" + usuario.Trim() + "'";
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (!rdr.HasRows)
                            {
                                nresultado = true;
                            }
                            else
                            {
                                MessageBox.Show("Usuario ya resgistrado.");
                            }
                        }
                    }
                }

                if (nresultado)
                {
                    string sql = "insert into Contactos (usuario, usuario_contacto) values ('" + Maka2.App.UsuarioLogeado + "','" + usuario.Trim() + "')";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        int rowCount = cmd.ExecuteNonQuery();

                        if (rowCount == 1) //or you can use > 0
                        {
                            nresultado = true;
                        }
                    }
                }

                conn.Close();
            }
            else
            {
                MessageBox.Show("No puedes tenerte como contacto.");
            }
            
            return nresultado;
        }
        
        public void EnviarMensaje()
        {
            string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "";
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        
                    }
                    else
                    {
                        MessageBox.Show("");
                    }
                }
            }
        }
        public void Dispose()
        {
        }

    }
}
