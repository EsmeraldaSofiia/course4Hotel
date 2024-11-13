using course4Hotel.Models;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using course4Hotel.ViewModels;


namespace course4Hotel.DataServices
{
    public class BookingService
    {
        private readonly FirebaseClient _firebaseClient;

        // Конструктор для ініціалізації клієнта Firebase
        public BookingService(FirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
        }
        // Метод для отримання доступних кімнат на основі дат заїзду та виїзду
        public async Task<IEnumerable<Room>> GetRoomsByDateAsync(DateTime checkInDate, DateTime checkOutDate)
        {
            // Отримуємо список кімнат з Firebase
            var rooms = await _firebaseClient.Child("Rooms").OnceAsync<Room>();
            // Отримуємо всі бронювання з Firebase
            var bookings = await _firebaseClient.Child("Bookings").OnceAsync<Booking>();

            // Фільтрація кімнат, щоб отримати лише доступні для вказаних дат
            var filteredRooms = rooms
                .Where(r => !bookings.Any(b =>
                    b.Object.RoomId == r.Key &&
                    ((checkInDate >= b.Object.CheckInDate && checkInDate < b.Object.CheckOutDate) ||
                     (checkOutDate > b.Object.CheckInDate && checkOutDate <= b.Object.CheckOutDate) ||
                     (checkInDate <= b.Object.CheckInDate && checkOutDate >= b.Object.CheckOutDate))))
                .Select(r => new Room
                {
                    Id = r.Key,
                    Number = r.Object.Number,
                    Name = r.Object.Name,
                    Price = r.Object.Price,
                    floor = r.Object.floor,
                    Description = r.Object.Description
                });

            return filteredRooms;
        }

        // Метод для отримання бронювань конкретного користувача
        public async Task<IEnumerable<Booking>> GetBookingsAsync(string userId)
        {
            var bookings = await _firebaseClient.Child("Bookings").OnceAsync<Booking>();
            // Фільтрування бронювання за користувачем
            return bookings.Where(b => b.Object.CustomerId == userId)
                           .Select(b => new Booking
                           {
                               Id = b.Key,
                               RoomId = b.Object.RoomId,
                               RoomNumber = b.Object.RoomNumber,
                               BookingDate = b.Object.BookingDate,
                               CheckInDate = b.Object.CheckInDate,
                               CheckOutDate = b.Object.CheckOutDate,
                               CustomerId = b.Object.CustomerId
                           });
        }

        // Метод для отримання номера кімнати за її ідентифікатором
        public async Task<int?> GetRoomNumberById(string roomId)
        {
            var rooms = await _firebaseClient.Child("Rooms").OnceAsync<Room>();
            var room = rooms.FirstOrDefault(r => r.Key == roomId);
            return room?.Object.Number;
        }

        // Метод для отримання ціни кімнати за її ідентифікатором
        public async Task<float?> GetRoomPriceById(string roomId)
        {
            var rooms = await _firebaseClient.Child("Rooms").OnceAsync<Room>();
            var room = rooms.FirstOrDefault(r => r.Key == roomId);
            return room?.Object.Price;
        }

        // Метод для збереження бронювання в базі даних
        public async Task SaveBooking(Booking booking)
        {
            await _firebaseClient.Child("Bookings").PostAsync(booking);
        }
    }
}
