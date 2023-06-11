using Microsoft.Win32;
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
using Dapper;

namespace ContactsAttempt {
    /// <summary>
    /// Interaction logic for AddContact.xaml
    /// </summary>
    public partial class AddContact : UserControl {

        static string connectionString = "Server=localhost;Database=Contacts;Trusted_Connection=true";
        static string newContactImagePath = "";
        public AddContact() {
            InitializeComponent();
        }

        private Contact CreateContact() {
            Contact newContact = new Contact();

            if (NewFirstName.Text != null) {
                newContact.FirstName = NewFirstName.Text;
            }
            if (NewMidName.Text != null) {
                newContact.MiddleName = NewMidName.Text;
            }
            if (NewLastName.Text != null) {
                newContact.LastName = NewLastName.Text;
            }
            if (NewNickname.Text != null) {
                newContact.Nickname = NewNickname.Text;
            }
            if (NewTitle.Text != null) {
                newContact.Title = NewTitle.Text;
            }
            if (NewBirthdate.Text != null) {
                //newContact.BirthDate = NewBirthdate.Text;
            }
            if (NewEmail.Text != null) {
                newContact.Email = NewEmail.Text;
            }
            if (NewPhone.Text != null) {
                newContact.Phone = NewPhone.Text;
            }
            if (NewStreet.Text != null) {
                newContact.Street = NewStreet.Text;
            }
            if (NewCity.Text != null) {
                newContact.City = NewCity.Text;
            }
            if (NewState.Text != null) {
                newContact.State = NewState.Text;
            }
            if (NewZip.Text != null) {
                newContact.ZipCode = NewZip.Text;
            }
            if (NewCountry.Text != null) {
                newContact.Country = NewCountry.Text;
            }
            if (NewWebsite.Text != null) {
                newContact.Website = NewWebsite.Text;
            }
            if (NewNotes.Text != null) {
                newContact.Notes = NewNotes.Text;
            }
            if (newContactImagePath != null) {
                newContact.Picture = newContactImagePath;
            }
            return newContact;
        }

        private void AddContact() {
            Contact addedContact = new Contact();
            var connection = new SqlConnection(connectionString);

            using (connection) {
                connection.Query<Contact>("INSERT INTO tblContact");
            }
            Contact.contactsList.Add(addedContact);;
        }

        private void UploadBtn_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "JPG Files (.jpg)|*.jpg";

            //SHOW FILE DIALOG
            bool? result = openFileDialog.ShowDialog();

            //PROCESS DIALOG RESULTS / DETERMINE IF FILE WAS OPENED
            if (result == true) {
                //STORE FILE PATH
                string selectedFile = openFileDialog.FileName;
                newContactImagePath = selectedFile;
            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e) {
            Contact newContact = CreateContact();
            Contact.contactsList.Add(newContact);
            //CC.Content = new MainWindow();

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e) {
            //CC.Content = new MainWindow();
        }
    }
}