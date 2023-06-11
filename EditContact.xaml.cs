using Microsoft.Win32;
using System;
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
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Emit;

namespace ContactsAttempt {
    /// <summary>
    /// Interaction logic for EditContact.xaml
    /// </summary>
    public partial class EditContact : UserControl {

        static string connectionString = "Server=localhost;Database=Contacts;Trusted_Connection=true";
        static string ContactImagePath = "";
        static string firstName = Contact.currentContact.FirstName;
        static string lastName = Contact.currentContact.LastName;

        public EditContact() {
            InitializeComponent();
            PopulateEditBoxes(Contact.currentContact);
        }

        private void PopulateEditBoxes(Contact selectedContact) {
            EditFirstName.Text = selectedContact.FirstName;
            EditMidName.Text = selectedContact.MiddleName;
            EditLastName.Text = selectedContact.LastName;
            EditNickname.Text = selectedContact.Nickname;
            EditTitle.Text = selectedContact.Title;
            //EditBirthdate.Text = selectedContact.BirthDate;
            EditEmail.Text = selectedContact.Email;
            EditPhone.Text = selectedContact.Phone;
            EditStreet.Text = selectedContact.Street;
            EditCity.Text = selectedContact.City;
            EditState.Text = selectedContact.State;
            EditZip.Text = selectedContact.ZipCode;
            EditCountry.Text = selectedContact.Country;
            EditWebsite.Text = selectedContact.Website;
            EditNotes.Text = selectedContact.Notes;
        }

        private Contact UpdateContact(Contact selectedContact) {

            selectedContact.FirstName = EditFirstName.Text;
            selectedContact.MiddleName = EditMidName.Text;
            selectedContact.LastName = EditLastName.Text;
            selectedContact.Nickname = EditNickname.Text;
            selectedContact.Title = EditTitle.Text;
            //selectedContact.BirthDate = EditBirthdate.Text;
            selectedContact.Email = EditEmail.Text;
            selectedContact.Phone = EditPhone.Text;
            selectedContact.Street = EditStreet.Text;
            selectedContact.City = EditCity.Text;
            selectedContact.State = EditState.Text;
            selectedContact.ZipCode = EditZip.Text;
            selectedContact.Country = EditCountry.Text;
            selectedContact.Website = EditWebsite.Text;
            selectedContact.Notes = EditNotes.Text;

            if (ContactImagePath != null) {
                selectedContact.Picture = ContactImagePath;
            }

            var connection = new SqlConnection(connectionString);

            using (connection) {
                connection.Query<Contact>($"UPDATE tblContact " +
                    $"SET firstName = '{selectedContact.FirstName}', middleName = '{selectedContact.MiddleName}', lastName = '{selectedContact.LastName}', nickname = '{selectedContact.Nickname}', title = '{selectedContact.Title}', " +
                    $"email = '{selectedContact.Email}', phone = '{selectedContact.Phone}', street = '{selectedContact.Street}', city = '{selectedContact.City}', state = '{selectedContact.State}', zipCode = '{selectedContact.ZipCode}', country = '{selectedContact.Country}', " +
                    $"website = '{selectedContact.Website}', notes = '{selectedContact.Notes}' " +
                    $"WHERE firstName = '{firstName}'");
            }

            return selectedContact;
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
                ContactImagePath = selectedFile;
            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e) {
            Contact.currentContact = UpdateContact(Contact.currentContact);
            //CC.Content = new MainWindow();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e) {
            //CC.Content = new MainWindow();
        }
    }
}