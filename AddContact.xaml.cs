using Microsoft.Win32;
using System;
using System.IO;
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

        static string connectionString = Contact.ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);

        static string newContactImagePath = "";

        public AddContact() {
            InitializeComponent();
        }

        private Contact CreateContact() {
            Contact newContact = new Contact();

            string[] fields = {
                NewFirstName.Text,
                NewMidName.Text,
                NewLastName.Text,
                NewNickname.Text,
                NewTitle.Text,
                NewEmail.Text,
                NewPhone.Text,
                NewStreet.Text,
                NewCity.Text,
                NewState.Text,
                NewZip.Text,
                NewCountry.Text,
                NewWebsite.Text,
                NewNotes.Text
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

            newContact.FirstName = fields[0];
            newContact.MiddleName = fields[1];
            newContact.LastName = fields[2];
            newContact.Nickname = fields[3];
            newContact.Title = fields[4];
            newContact.Email = fields[5];
            newContact.Phone = fields[6];
            newContact.Street = fields[7];
            newContact.City = fields[8];
            newContact.State = fields[9];
            newContact.ZipCode = fields[10];
            newContact.Country = fields[11];
            newContact.Website = fields[12];
            newContact.Notes = fields[13];

            string favString;

            if (NewDay.Text != "00" && NewMonth.Text != "00" && NewYear.Text != "0000") {

                string birthDateString = $"'{NewYear.Text}-{NewMonth.Text}-{NewDay.Text}'";
                newContact.BirthDate = birthDateString;
            } else {
                string birthDateString = "null";
                newContact.BirthDate = "null";
            }

            if (newContactImagePath != null) {
                newContact.Picture = newContactImagePath;
            }

            bool favorite = (bool)btnFavorite.IsChecked;

            if (favorite) {
                newContact.IsFavorite = true;
                favString = "1";
            } else {
                newContact.IsFavorite = false;
                favString = "0";
            }

            newContact.IsActive = true;

            return newContact;

        }

        private void AddContactToDatabase(Contact newContact) {
            string favString;

            if (newContact.IsFavorite) {
                favString = "1";
            } else {
                favString = "0";
            }

            var newConnection = new SqlConnection(connectionString);
            using (newConnection) {
                newConnection.Query<Contact>("INSERT INTO tblContact (firstName, middleName, lastName, nickname, title, birthDate, email, phone, street, city, state, zipCode, country, website, notes, picture, isFavorite, isActive) " +
                    $"VALUES ('{newContact.FirstName}', '{newContact.MiddleName}', '{newContact.LastName}', '{newContact.Nickname}', '{newContact.Title}', " +
                    $"{newContact.BirthDate}, '{newContact.Email}', '{newContact.Phone}', '{newContact.Street}', '{newContact.City}', '{newContact.State}', '{newContact.ZipCode}', '{newContact.Country}', " +
                    $"'{newContact.Website}', '{newContact.Notes}', '{newContactImagePath}', '{favString}', '1')");
            }
            Contact.contactsList.Add(newContact);
            CC.Content = new HomeScreen();
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
                var workingDirectory = Environment.CurrentDirectory;
                string destinationFolder = $"{workingDirectory}\\";
                string destinationFileName = System.IO.Path.GetFileName(selectedFile);
                string destinationFilePath = System.IO.Path.Combine(destinationFolder, destinationFileName);

                File.Copy(selectedFile, destinationFilePath, true);
                newContactImagePath = destinationFolder + destinationFileName;
            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e) {

            if (NewFirstName.Text == "" && NewNickname.Text == "") {
                //MessageBox.Show("Contact requires first name or nickname");
                NewFirstName.BorderBrush = Brushes.Red;
                return;
            } else {
                NewFirstName.BorderBrush = Brushes.Black;
            }

            if (NewYear.Text.Length == 0 && NewMonth.Text.Length == 0 && NewDay.Text.Length == 0) {
                NewYear.Text = "0000";
                NewMonth.Text = "00";
                NewDay.Text = "00";

                Contact newContact = CreateContact();
                AddContactToDatabase(newContact);
                CC.Content = new HomeScreen();
            } else if (NewMonth.Text.Length != 2 || NewDay.Text.Length != 2 || NewYear.Text.Length != 4) {
                //MessageBox.Show("Incorrect date format (MM/DD/YYYY)", "Incorrect Date");
                NewMonth.BorderBrush = Brushes.Red;
                NewDay.BorderBrush = Brushes.Red;
                NewYear.BorderBrush = Brushes.Red;
            } else {
                Contact newContact = CreateContact();
                AddContactToDatabase(newContact);
                CC.Content = new HomeScreen();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e) {
            CC.Content = new HomeScreen();
        }
    }
}