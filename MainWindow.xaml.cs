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
using Dapper;

namespace ContactsAttempt {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        static string connectionString = "Server=localhost;Database=Contacts;Trusted_Connection=true";

        public MainWindow() {
     
            InitializeComponent();
            //CC.Content = new HomeScreen();
            bool connected = TestConnection();
            List<Contact> contacts = GetData();
            UpdateContactScreen(contacts[1]);
            ContactsListBox.ItemsSource = contacts;
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

        private List<Contact> GetData() {
            var connection = new SqlConnection(connectionString);
            using(connection) {
                return connection.Query<Contact>("SELECT * FROM tblContact").ToList();
            }
        }

        private List<Contact> AddContact(List<Contact> contacts) {
            Contact addedContact = new Contact();
            var connection = new SqlConnection(connectionString);

            using(connection) {
                connection.Query<Contact>("INSERT INTO");
            }
            contacts.Add(addedContact);
            return contacts;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) {

            if (Window.GetWindow(this) is MainWindow mainWindow) {
                mainWindow.ShowAddScreen();
            }
        }

        private void ShowAddScreen() {
            CC.Content = new AddContact();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e) {

            if (Window.GetWindow(this) is MainWindow mainWindow) {
                mainWindow.ShowEditScreen();
            }
        }

        private void ShowEditScreen() {
            CC.Content = new EditContact();
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
    }
}