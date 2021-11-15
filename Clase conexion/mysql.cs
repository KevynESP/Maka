using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            if (usuario != "" && password != "")
            {

                string sql = "SELECT * FROM Usuarios_Registrados where usuario like binary '" + usuario.Trim() + "' and password = '" + password.Trim() + "'";
                using (MySqlCommand cmd = new MySqlCommand(sql, App.connxx))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            Maka2.App.UsuarioLogeado = usuario;
                            resultado = true;
                        }       
                    }
                }
            }
            return resultado;
        }
        public bool Registrar(string usuario, string name, string password)
        {
            bool resultado = false;

            using (MySqlCommand cmd = App.connxx.CreateCommand())
            {
                cmd.CommandText = "SELECT usuario FROM Usuarios_Registrados where usuario like binary '"+ usuario.Trim()+"'";
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
            using (MySqlCommand cmd2 = App.connxx.CreateCommand())
            {
                cmd2.CommandText = "insert into Usuarios_Registrados (usuario, nombre, password) values ('" + usuario.Trim() + "', '" + name.Trim()+ "', '" + password.Trim() + "')";
                int rowCount = cmd2.ExecuteNonQuery();

                if (rowCount == 1)
                {
                    resultado = true;
                }
            }
            return resultado;
        }

        public bool NContacto(string usuario)
        {
            bool nresultado = false;
            bool existe = false;
            if (!usuario.Trim().Equals(App.UsuarioLogeado.Trim()))
            {
                using (MySqlCommand cmd = App.connxx.CreateCommand())
                {
                    cmd.CommandText = "SELECT usuario FROM Usuarios_Registrados where usuario like binary '" + usuario.Trim() + "'";
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
                    using (MySqlCommand cmd = App.connxx.CreateCommand())
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
                    using (MySqlCommand cmd = new MySqlCommand(sql, App.connxx))
                    {
                        int rowCount = cmd.ExecuteNonQuery();

                        if (rowCount == 1) //or you can use > 0
                        {
                            nresultado = true;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No puedes tenerte como contacto.");
            }
            
            return nresultado;
        }
        

        public DataTable Cargar_Chats(string chat_actual)
        {
            DataTable dt = new DataTable();
               string cond1 = "(usuario_origen like binary '" + App.UsuarioLogeado.Trim() + "' and usuario_destino like binary '" + chat_actual.Trim() + "')";
                string cond2 = "(usuario_origen like binary '" + chat_actual.Trim() + "' and usuario_destino like binary '" + App.UsuarioLogeado.Trim() + "')";

            using (MySqlCommand cmd = App.connxxt4.CreateCommand())
            {
 
                cmd.CommandText = "select * from Mensajes where " + cond1 + " or " + cond2 + " order by fecha";
                dt.Load(cmd.ExecuteReader());

            }
            using (MySqlCommand cmd = App.connxxt4.CreateCommand())
            {
                cmd.CommandText = "update Mensajes set descargado =1 where " + cond1 + " or " + cond2;
                cmd.ExecuteNonQuery();
            }
            return dt;
        }

        public DataTable Refrescar_Chats(string chat_actual, DateTime Ultimo)
        {
            DataTable dt = new DataTable();

            string cond2 = "(usuario_origen like binary '" + chat_actual.Trim() + "' and usuario_destino like binary '" + App.UsuarioLogeado.Trim() + "')";
            string cond3 = "fecha > '" + Ultimo.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            using (MySqlCommand cmd2 = App.connxxt1.CreateCommand())
            {
                cmd2.CommandText = "select * from Mensajes where (" + cond2 + ") and " + cond3 + " order by fecha";
                dt.Load(cmd2.ExecuteReader());
            }

            using (MySqlCommand cmd2 = App.connxxt1.CreateCommand())
            {
                cmd2.CommandText = "update Mensajes set descargado =1 where (" + cond2 + ") and " + cond3;
                cmd2.ExecuteNonQuery();
            }
            return dt;
        }

        public void Enviar_Mensaje(string chat_actual, string mensaje)
        {
            using (MySqlCommand cmd = App.connxxt3.CreateCommand())
            {
                string cond1 = App.UsuarioLogeado.Trim();
                cmd.CommandText = "insert into Mensajes  (Usuario_Origen, Usuario_Destino, Mensaje,fecha) values ('" + cond1 + "','" + chat_actual + "','" + mensaje.Trim() + "', '"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                cmd.ExecuteNonQuery();
            }
        }
        public void Dispose()
        {
        }

    }
}
