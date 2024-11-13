using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace course4Hotel.Models
{
    public class OfService
    {
        [PrimaryKey, AutoIncrement]
        public string Id { get; set; }

        public string Name { get; set; }  

        public float Price { get; set; }  

        public string Description { get; set; }  

        // Метод для валідації даних
        public (bool IsValid, string? ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return (false, $"Поле {nameof(Name)} обов'язкове.");
            }
            else if (Price <= 0)
            {
                return (false, $"{nameof(Price)} має бути більше 0.");
            }

            return (true, null);
        }
    }
}