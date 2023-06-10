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

namespace ContactsAttempt {
    /// <summary>
    /// Interaction logic for AddContact.xaml
    /// </summary>
    public partial class AddContact : UserControl {

        static string newContactImagePath = "";
        public AddContact() {
            InitializeComponent();
        }

        private Contact CreateContact() {
            Contact newContact = new Contact();

            newContact.FirstName = NewFirstName.Text;
            newContact.MiddleName = NewMidName.Text;
            newContact.LastName = NewLastName.Text;
            newContact.Nickname = NewNickname.Text;
            newContact.Title = NewTitle.Text;
            //newContact.BirthDate = NewBirthdate.Text;
            newContact.Email = NewEmail.Text;
            newContact.Phone = NewPhone.Text;
            newContact.Street = NewStreet.Text;
            newContact.City = NewCity.Text;
            newContact.State = NewState.Text;
            newContact.ZipCode = NewZip.Text;
            newContact.Country = NewCountry.Text;
            newContact.Website = NewWebsite.Text;
            newContact.Notes = NewNotes.Text;

            if (newContactImagePath != null) {
                newContact.Picture = newContactImagePath;
            }
            return newContact;
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