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
        public string Id { get; set; }
        public int Number { get; set; }
        public int floor { get; set; }
        public string Name { get; set; }
        
        public float Price { get; set; }

        public string Description { get; set; }

        public string ImageSource
        {
            get
            {
                return Name switch
                {
                    "Делюкс" => "https://assets.bwbx.io/images/users/iqjWHBFdfxIU/iVBgilJb2vwA/v3/-1x-1.webp",
                    "Стандарт" => "https://i.pinimg.com/originals/19/d2/a4/19d2a41216579687b8273cd043a30050.jpg",
                    "Студія" => "https://images.squarespace-cdn.com/content/v1/57c9a72c15d5dbf908c5bfb3/716812ed-c46d-42d3-857e-d04b8a73be59/elmBNAEW.811012.jpg?format=1000w",
                    _ => "https://www.hotel.de/de/media/image/9f/74/7d/President-Prag-Junior-Suite-1-14891.jpg"
                };
            }
        }

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
            else if (floor <= 0 && floor>80)
            {
                return (false, $"{nameof(floor)} має бути реальним числом.");
            }
            if (string.IsNullOrWhiteSpace(Description))
            {
                return (false, $"Поле {nameof(Description)} обов'язкове.");
            }
            return (true, null);
        }
    }
}
