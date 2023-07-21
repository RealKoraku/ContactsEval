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
using System.Diagnostics.Eventing.Reader;

namespace ContactsAttempt {
    /// <summary>
    /// Interaction logic for EditContact.xaml
    /// </summary>
    public partial class EditContact : UserControl {

        static string connectionString = $"Server=localhost;Database=Contacts;Trusted_Connection=true";
        static string ContactImagePath;
        static string firstName = Contact.currentContact.FirstName;
        static string lastName = Contact.currentContact.LastName;

        public EditContact() {
            ContactImagePath = Contact.currentContact.Picture;
            InitializeComponent();
            PopulateEditBoxes(Contact.currentContact);
        }

        private void PopulateEditBoxes(Contact selectedContact) {
            EditFirstName.Text = selectedContact.FirstName;
            EditMidName.Text = selectedContact.MiddleName;
            EditLastName.Text = selectedContact.LastName;
            EditNickname.Text = selectedContact.Nickname;
            EditTitle.Text = selectedContact.Title;
            EditEmail.Text = selectedContact.Email;
            EditPhone.Text = selectedContact.Phone;
            EditStreet.Text = selectedContact.Street;
            EditCity.Text = selectedContact.City;
            EditState.Text = selectedContact.State;
            EditZip.Text = selectedContact.ZipCode;
            EditCountry.Text = selectedContact.Country;
            EditWebsite.Text = selectedContact.Website;
            EditNotes.Text = selectedContact.Notes;

            if (selectedContact.IsFavorite) {
                btnFavorite.IsChecked = true;
            }

            BuildBirthDate(selectedContact);

            if (EditYear.Text == "null") {
                EditYear.Text = "";
            }
        }

        private void BuildBirthDate(Contact selectedContact) {
            string birthDate = selectedContact.BirthDate;

            string month = "";
            string day = "";
            string year = "";

            if (birthDate != null) {
                string dateSub = birthDate.Substring(0, 4);

                if (dateSub.Contains('-') || dateSub.Contains('/')) {

                    for (int i = 0; i < birthDate.Length; i++) {
                        if (i < 2) {
                            month += birthDate[i];
                        }
                        if (i > 2 && i < 5) {
                            day += birthDate[i];
                        }
                        if (i > 5 && i < 10) {
                            year += birthDate[i];
                        }
                    }
                } else {
                    for (int i = 0; i < birthDate.Length; i++) {
                        if (i < 4) {
                            year += birthDate[i];
                        }
                        if (i > 4 && i < 7) {
                            month += birthDate[i];
                        }
                        if (i > 7 && i < 10) {
                            day += birthDate[i];
                        }
                    }
                }
            }

            EditMonth.Text = month;
            EditDay.Text = day;
            EditYear.Text = year;
        }

        private Contact UpdateContact(Contact selectedContact) {

            string[] fields = {
                EditFirstName.Text, 
                EditMidName.Text, 
                EditLastName.Text, 
                EditNickname.Text, 
                EditTitle.Text, 
                EditEmail.Text, 
                EditPhone.Text, 
                EditStreet.Text, 
                EditCity.Text, 
                EditState.Text, 
                EditZip.Text, 
                EditCountry.Text, 
                EditWebsite.Text, 
                EditNotes.Text
                };

            for (int field = 0; field < fields.Length; field++) {
                string newField = "";
                for (int chars = 0; chars < fields[field].Length; chars++) {
                    if (fields[field][chars] == '\'') {
                        newField += "";
                    } else {
                        newField += fields[field][chars];
                    }
                }
                fields[field] = newField;
            }

            string birthDateString = "null";
            if (EditYear.Text == "0000") {
                birthDateString = "null";
            } else {
                birthDateString = $"'{EditYear.Text}-{EditMonth.Text}-{EditDay.Text}'";
            }

            string favString;

            selectedContact.FirstName = fields[0];
            selectedContact.MiddleName = fields[1];
            selectedContact.LastName = fields[2];
            selectedContact.Nickname = fields[3];
            selectedContact.Title = fields[4];
            selectedContact.Email = fields[5];
            selectedContact.Phone = fields[6];
            selectedContact.Street = fields[7];
            selectedContact.City = fields[8];
            selectedContact.State = fields[9];
            selectedContact.ZipCode = fields[10];
            selectedContact.Country = fields[11];
            selectedContact.Website = fields[12];
            selectedContact.Notes = fields[13];

            bool favorite = (bool)btnFavorite.IsChecked;

            if (favorite) {
                selectedContact.IsFavorite = true;
                favString = "1";
            } else {
                selectedContact.IsFavorite = false;
                favString = "0";
            }

            if (ContactImagePath != null) {
                selectedContact.Picture = ContactImagePath;
            }

            var connection = new SqlConnection(connectionString);

            using (connection) {
                 connection.Query<Contact>($"UPDATE tblContact " +
                    $"SET firstName = '{selectedContact.FirstName}', middleName = '{selectedContact.MiddleName}', lastName = '{selectedContact.LastName}', nickname = '{selectedContact.Nickname}', title = '{selectedContact.Title}', " +
                    $"birthDate = {birthDateString}, " +
                    $"email = '{selectedContact.Email}', phone = '{selectedContact.Phone}', street = '{selectedContact.Street}', city = '{selectedContact.City}', state = '{selectedContact.State}', zipCode = '{selectedContact.ZipCode}', country = '{selectedContact.Country}', " +
                    $"website = '{selectedContact.Website}', notes = '{selectedContact.Notes}', picture = '{ContactImagePath}', isFavorite = '{favString}', isActive = '1'" +
                    $"WHERE id = '{selectedContact.Id}'");
            }
            ContactImagePath = null;
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

            if (EditFirstName.Text == "" && EditNickname.Text == "") {
                //MessageBox.Show("Contact requires first name or nickname", "Required fields");
                EditFirstName.BorderBrush = Brushes.Red;
                return;
            }

            if (EditYear.Text.Length == 0 && EditMonth.Text.Length == 0 && EditDay.Text.Length == 0) {
                EditYear.Text = "0000";
                EditMonth.Text = "00";
                EditDay.Text = "00";
                Contact.currentContact = UpdateContact(Contact.currentContact);
                CC.Content = new HomeScreen();
                
            } else if (EditMonth.Text.Length != 2 || EditDay.Text.Length != 2 || EditYear.Text.Length != 4) {
                //MessageBox.Show("Incorrect date format (MM/DD/YYYY)", "Incorrect Date");
                EditMonth.BorderBrush = Brushes.Red;
                EditDay.BorderBrush = Brushes.Red;
                EditYear.BorderBrush = Brushes.Red;
                //MessageBox.Show("Incorrect date format (MM/DD/YYYY)", "Incorrect Date");
            } else { 
                Contact.currentContact = UpdateContact(Contact.currentContact);
                CC.Content = new HomeScreen();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e) {
            CC.Content = new HomeScreen();
        }
    }
}