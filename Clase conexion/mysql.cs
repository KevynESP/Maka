using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Maka2.Clase_conexion
{

    class BD : IDisposable
    {
        public bool Login(string usuario, string password)
        {
            bool resultado = false;
            string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string sql = "SELECT * FROM Usuarios_Registrados where usuario = " + usuario.Trim() + " and password = " + password.Trim();
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
        public bool Registrar(string usuario, string password)
        {
            bool resultado = false;
            string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Usuarios_Registrados where usuario = " + usuario.Trim();
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (!rdr.HasRows)
                    {
                        resultado = true;


                    }
                }
            }


            if (resultado)
            using (MySqlCommand cmd2 = conn.CreateCommand())
            {
                cmd2.CommandText = "insert into Usuarios_Registrados (usuario, password) values (" + usuario.Trim() + "," + password.Trim() + ")";
                int rowCount = cmd2.ExecuteNonQuery();

                if (rowCount == 1) //or you can use > 0
                {
                    resultado = true;
                }

            }


            


            conn.Close();
            return resultado;
        }
        static void Mymethod(string[] args)
        {
            string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string sql = "SELECT * FROM usuarios";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    MessageBox.Show(rdr.GetString(0));
                    /* iterate once per row */
                }
            }
        }
        public void Dispose()
        {
        }

    }
}
