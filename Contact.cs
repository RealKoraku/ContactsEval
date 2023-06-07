using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ContactsAttempt {
    internal class Contact {

        public string dataId;
        public string firstName;
        public string middleName;
        public string lastName;
        public string nickname;
        public string title;
        public string birthDate;
        public string email;
        public string phone;
        public string street;
        public string city;
        public string state;
        public string zipCode;
        public string country;
        public string website;
        public string notes;
        public string picture;
        public bool isFavorite;
        public bool isActive;

        public string DataId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Title { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string Notes { get; set; }
        public string Picture { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsActive { get; set; }

        public Contact(string dataId) {

        }

      //private Contact SetContact() {
      //    string connectionString = "Server=localhost;Database=Contacts;Trusted_Connection=true";


      //}
    }
}