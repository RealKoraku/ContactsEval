﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ContactsAttempt {

    public class Contact {

        public static string DatabaseName { get; set; }
        public static Contact currentContact { get; set; }
        public static List<Contact> contactsList { get; set; }

        public int DataId { get; set; }
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

        public override string ToString() {
            return $"{FirstName} {LastName}";
        }
    }
}