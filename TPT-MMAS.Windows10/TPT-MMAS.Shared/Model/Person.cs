using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{
    public enum NameType { Full, FirstLast, MiddleAbbreviated, FirstMiddleAbbreviated }

    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => BuildName(FirstName, MiddleName, LastName);
        public string FullNameAbbreviated => BuildName(FirstName, MiddleName, LastName, NameType.MiddleAbbreviated);
        public string ShortName => BuildName(FirstName, MiddleName, LastName, NameType.FirstMiddleAbbreviated);
        public string FirstLastName => BuildName(FirstName, MiddleName, LastName, NameType.FirstLast);
        public string Photo { get; set; }

        private static string BuildName(string firstName, string middleName = null, string lastName = null, NameType abbrev = NameType.Full)
        {
            string name = "";

            if (abbrev == NameType.FirstMiddleAbbreviated)
            {
                string first = "";
                string[] firsts = firstName.Split(' ');

                foreach (var n in firsts)
                {
                    first += n[0].ToString().ToUpper() + ". ";
                }
                name += first.Trim();
            }
            else
                name += firstName;
            
            if(!string.IsNullOrWhiteSpace(middleName) && abbrev != NameType.FirstLast)
                name += (abbrev == NameType.Full) ? " " + middleName : " " + middleName[0].ToString().ToUpper() + ".";

            name += (string.IsNullOrWhiteSpace(lastName)) ? "": " " + lastName.Trim();

            return name;
        }

    }
}
