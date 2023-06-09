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
        public AddContact() {
            InitializeComponent();
        }

        private Contact CreateContact(string imagePath) {
            Contact newContact = new Contact();

            newContact.FirstName = NewFirstName.Text;
            newContact.MiddleName = NewMidName.Text;
            newContact.LastName = NewLastName.Text;
            newContact.Nickname = NewNickname.Text;
            newContact.Title = NewTitle.Text;
            newContact.BirthDate = NewBirthdate.Text;
            newContact.Email = NewEmail.Text;
            newContact.Phone = NewPhone.Text;
            newContact.Street = NewStreet.Text;
            newContact.City = NewCity.Text;
            newContact.State = NewState.Text;
            newContact.ZipCode = NewZip.Text;
            newContact.Country = NewCountry.Text;
            newContact.Website = NewWebsite.Text;
            newContact.Notes = NewNotes.Text;
            newContact.Picture = imagePath;

            return newContact;
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
            //imgMain.Source = bmpImage;
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

                //CALL LOADIMAGE METHOD
                LoadImage(selectedFile);
            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e) {

        }
    }
}