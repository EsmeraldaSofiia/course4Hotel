using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using course4Hotel.Models;

namespace course4Hotel.Models
{
    public class Room
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Number { get; set; }
        public int floor { get; set; }
        public string Name { get; set; }
        
        public float Price { get; set; }

        public string Description { get; set; }

        public Room Clone => MemberwiseClone() as Room;

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
            else if (Number <= 0)
            {
                return (false, $"{nameof(Number)} має бути більше 0.");
            }
            else if (floor <= 0)
            {
                return (false, $"{nameof(floor)} має бути більше 0.");
            }
            return (true, null);
        }
    }
}
