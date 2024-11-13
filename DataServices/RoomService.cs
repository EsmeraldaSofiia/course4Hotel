using course4Hotel.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course4Hotel.DataServices
{
    public class RoomService
    {
        private readonly FirebaseClient _firebaseClient; // Клієнт для роботи з Firebase

        // Конструктор для ініціалізації клієнта Firebase
        public RoomService(FirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
        }

        // Метод для отримання всіх кімнат
        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            // Отримуємо всі кімнати з Firebase
            var rooms = await _firebaseClient.Child("Rooms").OnceAsync<Room>();

            // Повертаємо список кімнат з відомостями про кімнату
            return rooms.Select(r => new Room
            {
                Id = r.Key, // Ідентифікатор кімнати
                Number = r.Object.Number, // Номер кімнати
                Name = r.Object.Name, // Назва кімнати
                Price = r.Object.Price, // Ціна кімнати
                floor = r.Object.floor, // Поверх
                Description = r.Object.Description // Опис кімнати
            });
        }

        // Метод для збереження кімнати (створення або оновлення)
        public async Task SaveRoom(Room room)
        {
            if (string.IsNullOrEmpty(room.Id)) // Якщо кімната нова (без ідентифікатора)
            {
                await AddRoomAsync(room); // Додаємо нову кімнату
            }
            else
            {
                await UpdateRoomAsync(room); // Оновлюємо існуючу кімнату
            }
        }

        // Метод для додавання нової кімнати
        public async Task AddRoomAsync(Room room)
        {
            // Додаємо кімнату до Firebase
            await _firebaseClient.Child("Rooms").PostAsync(room);
        }

        // Метод для оновлення існуючої кімнати
        public async Task UpdateRoomAsync(Room room)
        {
            // Оновлюємо кімнату за її ідентифікатором в Firebase
            await _firebaseClient.Child("Rooms").Child(room.Id.ToString()).PutAsync(room);
        }

        // Метод для видалення кімнати
        public async Task DeleteRoomAsync(string roomId)
        {
            // Видаляємо кімнату з Firebase за її ідентифікатором
            await _firebaseClient.Child("Rooms").Child(roomId.ToString()).DeleteAsync();
        }
    }
}
