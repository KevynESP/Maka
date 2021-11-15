using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Maka2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summar
    /// y>
    /// 

    public partial class App : Application

    {
        public static MySqlConnection connxx = new MySqlConnection("server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;");
        public static MySqlConnection connxxt1 = new MySqlConnection("server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;");
        public static MySqlConnection connxxt2 = new MySqlConnection("server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;");
        public static MySqlConnection connxxt3 = new MySqlConnection("server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;");
        public static MySqlConnection connxxt4 = new MySqlConnection("server=23.91.70.27;user=maka_maka;database=maka_maka;password=Maka2021*;");

        public static string UsuarioLogeado = "";
        public static bool NewUser = false;
    }
}
