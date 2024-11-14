using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course4Hotel.Models
{
    public class Booking
    {
        [PrimaryKey, AutoIncrement]
        public string Id { get; set; }
        public string RoomId { get; set; }

        public int RoomNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CustomerId { get; set; }

        [Ignore]
        public Room Room { get; set; }


        public (bool IsValid, string? ErrorMessage) Validate()
        {
            if (CheckInDate > CheckOutDate)
            {
                return (false, $"Дата {nameof(CheckOutDate)} має бути пізніше за {nameof(CheckInDate)}.");
            }
            return (true, null);
        }
    }
}
