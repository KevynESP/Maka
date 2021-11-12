
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;


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
            string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string sql = "SELECT usuario_contacto FROM Contactos where Usuario = '" + App.UsuarioLogeado + "';";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            Contactos.Add(new Contacto
                            {
                                UserName = rdr[0].ToString(),
                                LastMessage = "nuevo usuario",
                            });
                        }
                    }

                }
            }
            conn.Close();
        }

        public void GetName()
        {
            string connStr = "server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string sql = "SELECT Nombre FROM Usuarios_Registrados where Usuario = '" + App.UsuarioLogeado + "'";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
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
            conn.Close();
        }

    }
}
