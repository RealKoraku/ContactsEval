using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;
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

            Contact.contactsList = SQLCheck();
            Contact.activeContactsList = CheckActiveContacts();
            Contact.inactiveContactsList = CheckInactiveContacts();
            ContactsListBox.ItemsSource = Contact.activeContactsList;

            if (Contact.activeContactsList.Count != 0) {
                Contact.currentContact = Contact.activeContactsList[0];
                Contact.currentContact = ReadSavedContact();
                ButtonsCheck();
                SetFontSize(Contact.FontSize);
                UpdateContactScreen(Contact.currentContact);
            }
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

                SqlQuery($"UPDATE tblContact SET isActive = '0' WHERE id = '{Contact.currentContact.Id}'");

                Contact.activeContactsList = CheckActiveContacts();
                Contact.inactiveContactsList = CheckInactiveContacts();

                if (Contact.inactiveContactsList.Count >= 1) {
                    EmptyBtn.Visibility = Visibility.Visible;
                }

                ContactsListBox.ItemsSource = Contact.activeContactsList;
            }
        }

        private void SortAZBtn_Click(object sender, RoutedEventArgs e) {
            if (SortAzBtn.Content == "Sort A-Z") {
                Contact.activeContactsList = SqlQueryList($"SELECT * FROM tblContact WHERE isActive = '1' ORDER BY firstName ASC");
                ContactsListBox.ItemsSource = Contact.activeContactsList;

                SortAzBtn.Content = "Sort Z-A";
            } else {
                Contact.activeContactsList = SqlQueryList($"SELECT * FROM tblContact WHERE isActive = '1' ORDER BY firstName DESC");
                ContactsListBox.ItemsSource = Contact.activeContactsList;

                SortAzBtn.Content = "Sort A-Z";
            }
        }

        private void FontSizeBtn_Click(object sender, RoutedEventArgs e) {

            if (Contact.FontSize == "Large") {
                Contact.FontSize = "Normal";
            } else {
                Contact.FontSize = "Large";
            }

            SetFontSize(Contact.FontSize);
        }

        private void EmptyBtn_Click(object sender, RoutedEventArgs e) {
            var dialogResult = (MessageBox.Show($"Are you sure you want to delete all inactive/removed contacts?", "Empty inactive contacts", MessageBoxButton.YesNo));
            if (dialogResult == MessageBoxResult.Yes) {
                Contact.inactiveContactsList = EmptyInactiveContacts();
            }
        }

        private void ShowBtnTxt_Click(object sender, RoutedEventArgs e) {
            if (ShowBtnTxt.Text == "Show inactive") {
                Contact.inactiveContactsList = CheckInactiveContacts();
                ContactsListBox.ItemsSource = Contact.inactiveContactsList;

                ShowBtnTxt.Text = "Show active";
            } else {
                Contact.activeContactsList = CheckActiveContacts();
                ContactsListBox.ItemsSource = Contact.activeContactsList;

                ShowBtnTxt.Text = "Show inactive";
            }
        }

        private void RestoreBtn_Click(object sender, RoutedEventArgs e) {
            SqlQuery("UPDATE tblContact SET isActive ='1'");
            Contact.currentContact.IsActive = true;
            Contact.inactiveContactsList = CheckInactiveContacts();
            Contact.activeContactsList = CheckActiveContacts();
            ContactsListBox.ItemsSource = Contact.inactiveContactsList;

            if (Contact.inactiveContactsList.Count == 0) {
                EmptyBtn.Visibility = Visibility.Hidden;
            }
        }

            #endregion

            #region XAML controls

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

        private List<Contact> EmptyInactiveContacts() {

            SqlQuery("DELETE FROM tblContact WHERE isActive = '0'");

            List<Contact> refreshList = new();

            for (int i = 0; i < Contact.contactsList.Count; i++) {
                if (Contact.contactsList[i].IsActive == true) {
                    refreshList.Add(Contact.contactsList[i]);
                }
            }
            Contact.contactsList = refreshList;

            EmptyBtn.Visibility = Visibility.Hidden;
            Contact.inactiveContactsList.Clear();

            if (ShowBtnTxt.Text == "Show active") {
                ContactsListBox.ItemsSource = CheckActiveContacts().ToList();
                ShowBtnTxt.Text = "Show inactive";
            }

            return Contact.inactiveContactsList;
        }

        private List<Contact> SQLCheck() {
            var connection = new SqlConnection(connectionString);
            using (connection) {
                return connection.Query<Contact>("SELECT * FROM tblContact").ToList();
            }
        }

        private void ListContacts_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // When listbox item is clicked
            var selectedItem = (ListBox)sender;
            var clickedItem = (Contact)selectedItem.SelectedItem;

            Contact.currentContact = clickedItem;

            if (Contact.currentContact != null) {
                SaveContact(Contact.currentContact);
            }

            UpdateContactScreen(Contact.currentContact);

        }// End function

        private void SearchId_TextChanged(object sender, EventArgs e) {

            if (SearchId.Text == "") {
                ContactsListBox.ItemsSource = Contact.activeContactsList;
            } else {
                List<Contact> contactSearch = SqlQueryList($"SELECT * FROM tblContact WHERE isActive = '1' AND firstName LIKE '%{SearchId.Text}%' OR lastName LIKE '%{SearchId.Text}%'");
                ContactsListBox.ItemsSource = contactSearch;
            }
            ShowBtnTxt.Text = "Show inactive";
        }

        private void TxtBdayRange_TextChanged(object sender, EventArgs e) {

            bool parser = int.TryParse(TxtBdayRange.Text, out int range);

            if (parser) {

                if (range > 365) {
                    range = 365;
                }

                DateTime todaysDate = DateTime.Now;
                List<Contact> bdayList = new List<Contact>();

                for (int i = 0; i < range; i++) {

                    DateTime currentDate = todaysDate.AddDays(i);
                    DateTime inverseDate = todaysDate.AddDays(-i);

                    for (int contact = 0; contact < Contact.activeContactsList.Count; contact++) {
                        if (Contact.activeContactsList[contact].BirthDate != null) {
                            DateTime birthDate = DateTime.Parse(Contact.activeContactsList[contact].BirthDate);
                            if (birthDate.Month == currentDate.Month && birthDate.Day == currentDate.Day && (bdayList.Contains(Contact.activeContactsList[contact]) == false)) {
                                bdayList.Add(Contact.activeContactsList[contact]);
                            } else if (birthDate.Month == inverseDate.Month && birthDate.Day == inverseDate.Day && (bdayList.Contains(Contact.activeContactsList[contact]) == false)) {
                                bdayList.Add(Contact.activeContactsList[contact]);
                            }
                        }
                    }
                }
                ContactsListBox.ItemsSource = bdayList;
            } else {
                ContactsListBox.ItemsSource = Contact.activeContactsList;
            }
            ShowBtnTxt.Text = "Show inactive";
        }

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
            } else {
                ImageId.Source = null;
            }

            if (Contact.currentContact.IsActive == false) {
                RestoreBtn.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Hidden;
            } else {
                RestoreBtn.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Visible;
            }
        }

        private void ButtonsCheck() {
            if (Contact.activeContactsList.Count == 0) {
                btnEdit.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                SortAzBtn.Visibility = Visibility.Hidden;
            }

            if (Contact.inactiveContactsList.Count == 0) {
                EmptyBtn.Visibility = Visibility.Hidden;
            }

            if (Contact.currentContact != null && Contact.currentContact.IsActive) {
                RestoreBtn.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Visible;
            } else {
                RestoreBtn.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Hidden;
            }

            if (Contact.FontSize == null) {
                Contact.FontSize = "Normal";
            }
            SortAzBtn.Content = "Sort A-Z";
        }

        private void SetFontSize(string fontSetting) { 

            if (fontSetting == "Large") {
                NameId.FontSize = 26;
                StreetId.FontSize = 26;
                CityId.FontSize = 26;
                StateId.FontSize = 26;
                ZipId.FontSize = 26;
                EmailId.FontSize = 26;
                PhoneId.FontSize = 26;
                WebId.FontSize = 26;
                NotesId.FontSize = 26;
                WebBlock.FontSize = 22;
                notesBlock.FontSize = 22;
                ContactsListBox.FontSize = 24;

                btnAdd.FontSize = 26;
                btnDelete.FontSize = 26;
                btnEdit.FontSize = 26;
                EmptyBtn.FontSize = 16;
                RestoreBtn.FontSize = 26;
                SortAzBtn.FontSize = 16;
                FontSizeBtn.FontSize = 16;
                ShowBtnTxt.FontSize = 26;
                lblSearch.FontSize = 20;
                lblRange.FontSize = 20;

            } else if (fontSetting == "Normal") {
                NameId.FontSize = 18;
                StreetId.FontSize = 18;
                CityId.FontSize = 18;
                StateId.FontSize = 18;
                ZipId.FontSize = 18;
                EmailId.FontSize = 18;
                PhoneId.FontSize = 18;
                WebId.FontSize = 18;
                NotesId.FontSize = 18;
                WebBlock.FontSize = 16;
                notesBlock.FontSize = 16;
                ContactsListBox.FontSize = 16;

                btnAdd.FontSize = 16;
                btnDelete.FontSize = 16;
                btnEdit.FontSize = 16;
                EmptyBtn.FontSize = 16;
                RestoreBtn.FontSize = 16;
                ShowBtnTxt.FontSize = 16;
                SortAzBtn.FontSize = 16;
                FontSizeBtn.FontSize = 16;
                lblSearch.FontSize = 14;
                lblRange.FontSize = 14;
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

        private void SqlQuery(string query) {
            var connection = new SqlConnection(connectionString);
            using (connection) {
                connection.Query<Contact>($"{query}");
            }
        }

        private List<Contact> SqlQueryList(string query) {
            var connection = new SqlConnection(connectionString);
            using (connection) {
                return connection.Query<Contact>($"{query}").ToList();
            }
        }

        #endregion

        #region FileIO

        private Contact ReadSavedContact() {
            string path = "C:\\Users\\MCA Coder\\Desktop\\savedContact.txt";
            string saveContactId = $"{Contact.currentContact.Id}";

            if (File.Exists(path)) {
                string contactId = File.ReadAllText(path);

                List<Contact> saved = SqlQueryList($"SELECT * FROM tblContact WHERE id = '{contactId}'");
                if (saved.Count > 0) {
                    Contact.currentContact = saved[0];

                    if (Contact.currentContact.IsActive == false) {
                        Contact.currentContact = Contact.activeContactsList[0];
                    }
                    return Contact.currentContact;
                }
                return Contact.currentContact;
        
            } else {
                try {
                    File.WriteAllText(path, Contact.currentContact.Id.ToString());
                    return Contact.currentContact;
                } catch (Exception error) { }
                return Contact.currentContact;
            }
        }

        private void SaveContact(Contact currentContact) {
            string path = "C:\\Users\\MCA Coder\\Desktop\\savedContact.txt";

            try {
                File.WriteAllText(path, currentContact.Id.ToString());
            } catch (Exception error) { }
        }

        #endregion
    }
}