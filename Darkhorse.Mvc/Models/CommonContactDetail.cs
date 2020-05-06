using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc
{
    public class CommonContactDetail
    {
        public IEnumerable<Contact> Contacts { get; set; }
        public Contact SelectedContact { get; set; }
        public IEnumerable<ContactNumber> PhoneNumbers { get; set; }
    }
}
