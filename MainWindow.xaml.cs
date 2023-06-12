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

        static string connectionString = $"Server=localhost;Database={Contact.DatabaseName};Trusted_Connection=true";

        public MainWindow() {
            InitializeComponent();

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
            var connection = new SqlConnection(connectionString);
            using(connection) {
                return connection.Query<Contact>("SELECT * FROM tblContact").ToList();
            }
        }

        #endregion

        #region Buttons

        //private void btnAdd_Click(object sender, RoutedEventArgs e) {
        //
        //    if (Window.GetWindow(this) is MainWindow mainWindow) {
        //        mainWindow.ShowAddScreen();
        //    }
        //}
        //
        //private void btnEdit_Click(object sender, RoutedEventArgs e) {
        //
        //    if (Window.GetWindow(this) is MainWindow mainWindow) {
        //        mainWindow.ShowEditScreen();
        //    }
        //}
        //
        //private void SearchBtn_Click(object sender, RoutedEventArgs e) {
        //    string searchTerm = SearchId.Text;
        //}
        //
        //private void OptionsBtn_Click(object sender, RoutedEventArgs e) {
        //}

        #endregion

        #region XAML controls

        //private void ShowAddScreen() {
        //    CC.Content = new AddContact();
        //}

        //private void ShowEditScreen() {
        //    CC.Content = new EditContact();
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

        //private void UpdateContactScreen(Contact currentContact) {
        //    NameId.Content = currentContact.FirstName + " " + currentContact.MiddleName + " " + currentContact.LastName;
        //    StreetId.Content = currentContact.Street;
        //    CityId.Content = currentContact.City;
        //    StateId.Content = currentContact.State;
        //    ZipId.Content = currentContact.ZipCode;
        //    EmailId.Content = currentContact.Email;
        //    PhoneId.Content = currentContact.Phone;
        //    WebId.Content = currentContact.Website;
        //    NotesId.Content = currentContact.Notes;

        //    if (currentContact.Picture != null) {
        //        LoadImage(currentContact.Picture);
        //    }
        //}

        //private void LoadImage(string path) {
        //    //CREATE BITMAP TO HOLD IMAGE DATA
        //    BitmapImage bmpImage = new BitmapImage();

        //    //CREATE URI TO REFERENCE PATH TO IMAGE
        //    Uri uriImage = new Uri(path);

        //    //INIT BITMAP TO LOAD DATA
        //    bmpImage.BeginInit();
        //    bmpImage.UriSource = uriImage; //TELL BITMAP WHERE TO FIND IMAGE VIA URI
        //    bmpImage.EndInit();

        //    //SET IMAGE CONTROL TO DISPLAY THE IMAGE
        //    ImageId.Source = bmpImage;
        //}

        #endregion

        private void EnterBtn_Click(object sender, RoutedEventArgs e) {
            Contact.DatabaseName = DatabaseBox.Text;

            if (Contact.DatabaseName == "") {
                FailedLbl.Content = "Name cannot be null";
                FailedLbl.Opacity = 100;
                return;
            }

            connectionString = $"Server=localhost;Database={Contact.DatabaseName};Trusted_Connection=true";

            bool connected = TestConnection();

            if (connected) {

                FailedLbl.Opacity = 0;

                EnterBtn.Visibility = Visibility.Collapsed;

                Contact.contactsList = GetData();
                Contact.currentContact = Contact.contactsList[1];

                CC.Content = new HomeScreen();
            } else {
                FailedLbl.Content = "Connection Failed";
                FailedLbl.Opacity = 100;
            }
        }
    }
}