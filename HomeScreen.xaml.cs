using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for HomeScreen.xaml+
    /// </summary>
    public partial class HomeScreen : UserControl {

        static string connectionString = $"Server=localhost;Database={Contact.DatabaseName};Trusted_Connection=true";

        public HomeScreen() {
            InitializeComponent();

            Contact.activeContactsList = CheckActiveContacts();
            Contact.inactiveContactsList = CheckInactiveContacts();
            Contact.currentContact = Contact.activeContactsList[1];
            UpdateContactScreen(Contact.currentContact);
            ContactsListBox.ItemsSource = Contact.activeContactsList;
        }

        #region Buttons

        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            CC.Content = new AddContact();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e) {
            CC.Content = new EditContact();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {

            var dialogResult = (MessageBox.Show($"Are you sure you want to delete {Contact.currentContact.ToString()}?", "Delete Contact", MessageBoxButton.YesNo));
            if (dialogResult == MessageBoxResult.Yes) {
                Contact.currentContact.IsActive = false;

                var connection = new SqlConnection(connectionString);
                using (connection) {
                    connection.Query<Contact>($"UPDATE tblContact SET isActive = '0' WHERE id = '{Contact.currentContact.Id}'");
                }

                Contact.activeContactsList = CheckActiveContacts();
                Contact.inactiveContactsList = CheckInactiveContacts();
                ContactsListBox.ItemsSource = Contact.activeContactsList;
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e) {
            string searchTerm = SearchId.Text;      
        }

        private void SortAZBtn_Click(object sender, RoutedEventArgs e) {
            Contact.activeContactsList = SortContactsAZ();
            ContactsListBox.ItemsSource = Contact.activeContactsList;
        }

        private void SortZABtn_Click(object sender, RoutedEventArgs e) {
            Contact.activeContactsList = SortContactsZA();
            ContactsListBox.ItemsSource = Contact.activeContactsList;
        }

        private void EmptyBtn_Click(object sender, RoutedEventArgs e) {
            var dialogResult = (MessageBox.Show($"Are you sure you want to delete all inactive/removed contacts?", "Empty inactive contacts", MessageBoxButton.YesNo));
            if (dialogResult == MessageBoxResult.Yes) {
                Contact.inactiveContactsList = EmptyInactiveContacts();
            }
        }

        #endregion

        #region XAML controls

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

        private List<Contact> CheckActiveContacts() {
            Contact.activeContactsList = new List<Contact>();

            for (int i = 0; i < Contact.contactsList.Count; i++) {
                if (Contact.contactsList[i].IsActive) {
                    Contact.activeContactsList.Add(Contact.contactsList[i]);
                }
            }
            return Contact.activeContactsList;
        }

        private List<Contact> CheckInactiveContacts() {
            Contact.inactiveContactsList = new List<Contact>();

            for (int i = 0; i < Contact.contactsList.Count; i++) {
                if (Contact.contactsList[i].IsActive == false) {
                    Contact.inactiveContactsList.Add(Contact.contactsList[i]);
                }
            }
            return Contact.inactiveContactsList;
        }

        private List<Contact> SortContactsAZ() {
            var connection = new SqlConnection(connectionString);
            using (connection) {
                return connection.Query<Contact>($"SELECT * FROM tblContact ORDER BY firstName ASC").ToList();
            }

        }

        private List<Contact> SortContactsZA() {
            var connection = new SqlConnection(connectionString);
            using (connection) {
                return connection.Query<Contact>($"SELECT * FROM tblContact ORDER BY firstName DESC").ToList();
            }

        }

        private List<Contact> EmptyInactiveContacts() {
            Contact.inactiveContactsList = CheckInactiveContacts();
            Contact.inactiveContactsList.Clear();

            var connection = new SqlConnection(connectionString);
            using (connection) {
                connection.Query<Contact>($"DELETE FROM tblContact WHERE isActive = '0'");
            }

            return Contact.inactiveContactsList;
        }

        private void ListContacts_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // When listbox item is clicked
            var selectedItem = (ListBox)sender;
            var clickedItem = (Contact)selectedItem.SelectedItem;

            Contact.currentContact = clickedItem;
            UpdateContactScreen(Contact.currentContact);

        }// End function

        private void UpdateContactScreen(Contact currentContact) {
            if (currentContact == null) {
                currentContact = Contact.activeContactsList[0];
                Contact.currentContact = currentContact;
            }
            if (currentContact.Nickname != null && currentContact.Nickname != "") {
                NameId.Content = currentContact.Nickname + " " + $"({currentContact.FirstName})" + " " + currentContact.MiddleName + " " + currentContact.LastName;
            } else {
                NameId.Content = currentContact.FirstName + " " + currentContact.MiddleName + " " + currentContact.LastName;
            }

            StreetId.Content = currentContact.Street;
            CityId.Content = currentContact.City;
            StateId.Content = currentContact.State;
            ZipId.Content = currentContact.ZipCode;
            EmailId.Content = currentContact.Email;
            PhoneId.Content = currentContact.Phone;
            WebId.Content = currentContact.Website;
            NotesId.Content = currentContact.Notes;

            if (currentContact.Picture != null && currentContact.Picture != "") {
                LoadImage(currentContact.Picture);
            }
        }

        private void LoadImage(string path) {
            //CREATE BITMAP TO HOLD IMAGE DATA
            BitmapImage bmpImage = new BitmapImage();

            //CREATE URI TO REFERENCE PATH TO IMAGE
            Uri uriImage = new Uri(path);

            //INIT BITMAP TO LOAD DATA
            bmpImage.BeginInit();
            bmpImage.UriSource = uriImage; //TELL BITMAP WHERE TO FIND IMAGE VIA URI
            bmpImage.EndInit();

            //SET IMAGE CONTROL TO DISPLAY THE IMAGE
            ImageId.Source = bmpImage;
        }

        #endregion
    }
}