using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using SQLite;

namespace course4Hotel.Models
{
    public class UserInform
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        public bool IsAdmin {  get; set; }
        public string AdminCode {  get; set; }

        //public UserInform Clone => MemberwiseClone() as UserInform;

        public (bool IsValid, string? ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                return (false, $"Поле {nameof(FullName)} обов'язкове.");
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                return (false, $"Поле {nameof(Password)} обов'язкове.");
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                return (false, $"Поле {nameof(Email)} обов'язкове.");
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                return (false, $"Поле {nameof(PhoneNumber)} обов'язкове.");
            }
            return (true, null);
        }
    }
}
