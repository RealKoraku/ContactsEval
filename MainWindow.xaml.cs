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

namespace ContactsAttempt {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        static string connectionString = "Server=localhost;Database=Contacts;Trusted_Connection=true";

        public MainWindow() {
     
            InitializeComponent();

            bool connected = TestConnection();
            PopulateListBox();
        }

        static bool TestConnection() {
            try {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                connection.Close();
                return true;

            } catch (Exception exception) {
                return false;
            }
        }

        static void AddContact(string tableName) {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string commandString = $"SELECT * FROM dbo.{tableName}";

            SqlCommand command = new SqlCommand(commandString);

            try {
                SqlDataReader result = command.ExecuteReader();

                while (result.Read()) {
                    for (int fieldIndex = 0; fieldIndex < result.FieldCount; fieldIndex++) {
                        Console.WriteLine(($"{result[fieldIndex]} "));
                    }
                }

            } catch (Exception exception) {
                Console.WriteLine(exception);
            }
            connection.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            NameId.Content = "Hello";

            AddContact("tblContact");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
        }

        private void UpdateContactScreen(Contact currentContact) {
            NameId.Content = currentContact.FirstName + " " + currentContact.MiddleName + " " + currentContact.LastName;
            StreetId.Content = currentContact.Street;
            CityId.Content = currentContact.City;
            StateId.Content = currentContact.State;
            ZipId.Content = currentContact.ZipCode;
            EmailId.Content = currentContact.Email;
            PhoneId.Content = currentContact.Phone;
            WebId.Content = currentContact.Website;
            NotesId.Content = currentContact.Notes;

        }

        private void PopulateListBox() { 
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            ListBox contactsList = new ListBox();

            string commandString = "SELECT firstName FROM dbo.tblContact";

            SqlCommand command = new SqlCommand(commandString, connection);

            try {
                SqlDataReader result = command.ExecuteReader();

                while (result.Read()) {
                    for (int fieldIndex = 0; fieldIndex < result.FieldCount; fieldIndex++) {
                        contactsList.Items.Add(($"{result[fieldIndex]} "));
                    }
                }

            } catch (Exception exception) {
                Console.WriteLine(exception);
            }
            connection.Close();

        }
    }
}