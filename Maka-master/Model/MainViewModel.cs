
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace Maka2.Model
{
    class MainViewModel
    {
        public ObservableCollection<Contacto> Contactos { get; set; }
        public ObservableCollection<Usuario> Usuarios { get; set; }

        public MainViewModel()
        {
            Contactos = new ObservableCollection<Contacto>();
            Usuarios = new ObservableCollection<Usuario>();

            GetContactos();
            GetName();
        }
        public void GetContactos()
        {
            Contactos.Clear();
            DataTable dt = new DataTable();
            DataTable data = new DataTable();



            string aa = " select * from(SELECT usuario_contacto , (select sum(CASE WHEN Descargado = 0 then 1 ELSE 0 END) as sindescargar from Mensajes where STRCMP(Mensajes.Usuario_Origen,con.usuario_contacto)=0) as sindescargar, (select mensaje from Mensajes where (strcmp(Mensajes.Usuario_Origen ,con.usuario_contacto)=0 and strcmp(Usuario_Destino, '"+ App.UsuarioLogeado.ToString().Trim()+"')=0) or (strcmp(Mensajes.Usuario_Origen,'"+ App.UsuarioLogeado.ToString().Trim()+"')=0 and strcmp(Mensajes.Usuario_Destino,con.usuario_contacto)=0) order by fecha desc limit 0,1) as mensaje, (select fecha from Mensajes where (STRCMP(Mensajes.Usuario_Origen,con.usuario_contacto)=0 and   strcmp(Usuario_Destino, '"+ App.UsuarioLogeado.ToString().Trim()+"')=0 ) or (STRCMP(Mensajes.Usuario_Origen,'"+ App.UsuarioLogeado.ToString().Trim()+"')=0 and STRCMP(Mensajes.Usuario_Destino,con.usuario_contacto)=0) order by fecha desc limit 0,1) as fecha from Contactos as con where STRCMP(con.Usuario,'"+ App.UsuarioLogeado.ToString().Trim()+"')=0) c order by fecha desc";
            string sql = aa;
            using (MySqlCommand cmd =  App.connxxt2.CreateCommand())
            {
                cmd.CommandText = sql;  
                data.Rows.Clear();
                data.Load(cmd.ExecuteReader()); 
            }
            if (data.Rows != null)
            {
                foreach (DataRow row in data.Rows)
                {

                    string nombre = row[0].ToString();
                    string mensa = row[2].ToString().Trim();
                    int numero;
                    try
                    {
                        numero = Int32.Parse(row[1].ToString());
                    }
                    catch (Exception)
                    {

                        numero=0;
                    }


                    Contactos.Add(new Contacto
                    {
                        UserName = nombre,
                        LastMessage = mensa,
                        numeroMensajes = numero
                    });

                }
            }
        }

        public void GetName()
        {
            string sql = "SELECT Nombre FROM Usuarios_Registrados where Usuario = '" + App.UsuarioLogeado + "'";
            using (MySqlCommand cmd = new MySqlCommand(sql, App.connxxt2))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        Usuarios.Add(new Usuario
                        {
                            UserName = rdr.GetString(0),
                            Estado = "Conectado"
                        });
                    }
                }
            }
        }

    }
}
