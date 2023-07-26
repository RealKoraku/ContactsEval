using System;
using System.Data.SqlClient;
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
using System.IO;
using Dapper;

namespace ContactsAttempt {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        static string connectionString = Contact.ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);

        public MainWindow() {
            InitializeComponent();
            ReadConnectionString();
            Contact.contactsList = GetData();
            CC.Content = new HomeScreen();
        }

        #region SQL

        static bool TestConnection() {
            SqlConnection connection = new SqlConnection(connectionString);
            try {
                using (connection) {

                    connection.Open();
                    connection.Close();
                    return true;
                }

                } catch (Exception exception) {
                return false;
            }
        }

        private List<Contact> GetData() {
            var newConnection = new SqlConnection(connectionString);
            using(newConnection) {
                return newConnection.Query<Contact>("SELECT * FROM tblContact").ToList();
            }
        }

        private void ReadConnectionString() {

            var workingDirectory = Environment.CurrentDirectory;
            string path = $"{workingDirectory}\\connectionString.txt";

            if (File.Exists(path)) {
                Contact.ConnectionString = File.ReadAllText(path);
                connectionString = Contact.ConnectionString;
            } else {
                try {
                    File.WriteAllText(path, "Server=localhost;Database=Contacts;Trusted_Connection=true");
                } catch (Exception error) { }
            }
        }

        #endregion

        #region XAML controls

        #endregion

        private void EnterBtn_Click(object sender, RoutedEventArgs e) {
        //    Contact.DatabaseName = DatabaseBox.Text;

        //    bool allSpaces = true;
        //    for (int i = 0; i < Contact.DatabaseName.Length; i++) {
        //        if (Contact.DatabaseName[i] != ' ') {
        //            allSpaces = false;
        //        }
        //    }

        //    if (Contact.DatabaseName == "" || allSpaces) {
        //        FailedLbl.Content = "Name cannot be null";
        //        FailedLbl.Opacity = 100;
        //        return;
        //    }

        //    connectionString = $"Server=localhost;Database={Contact.DatabaseName};Trusted_Connection=true";

        //    bool connected = TestConnection();

        //    if (connected) {

        //        FailedLbl.Opacity = 0;
        //        EnterBtn.Visibility = Visibility.Collapsed;

        //        Contact.contactsList = GetData();

        //        CC.Content = new HomeScreen();
        //    } else {
        //        FailedLbl.Content = "Connection Failed";
        //        FailedLbl.Opacity = 100;
        //    }
        }
    }
}