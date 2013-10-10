using System;
using System.Data;
using System.Security.Cryptography;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace KartaPacjenta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            login.Focus();
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkLoginForm(login.Text, pass.Password))
            {
                MessageBox.Show("Welcome");
            }
            else
            {
                MessageBox.Show("Podano nieprawidłowy login i\\lub hasło.");
            }
        }

        private bool checkLoginForm(String login, String password)
        {
            string connectionString = String.Format("Server={0};Port={1};" +
                   "User Id={2};Password={3};Database={4};",
                   "localhost", "5432", "saba",
                   "saba12", "Karta");
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string sqlQuery = String.Format("SELECT * FROM public.user WHERE name=\'{0}\'"+
                                            " AND password=\'{1}\';", 
                                            login, GetMd5Hash(password));
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            connection.Close();

            return dataSet.Tables[0].Rows.Count > 0;
        }

        private string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


    }
}