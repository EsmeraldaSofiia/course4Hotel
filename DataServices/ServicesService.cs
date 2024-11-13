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
    public class ServicesService
    {
        private readonly FirebaseClient _firebaseClient; // Клієнт для роботи з Firebase

        // Конструктор для ініціалізації клієнта Firebase
        public ServicesService(FirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
        }

        // Метод для отримання всіх послуг
        public async Task<IEnumerable<OfService>> GetAllServicesAsync()
        {
            // Отримуємо всі послуги з Firebase
            var services = await _firebaseClient.Child("Services").OnceAsync<OfService>();

            // Повертаємо список послуг з їхніми властивостями (назва, ціна, опис)
            return services.Select(s => new OfService
            {
                Id = s.Key, // Ідентифікатор послуги
                Name = s.Object.Name, // Назва послуги
                Price = s.Object.Price, // Ціна послуги
                Description = s.Object.Description // Опис послуги
            });
        }

        // Метод для збереження послуги (створення або оновлення)
        public async Task SaveOfService(OfService service)
        {
            if (string.IsNullOrEmpty(service.Id)) // Якщо послуга нова (без ідентифікатора)
            {
                await AddServiceAsync(service); // Додаємо нову послугу
            }
            else
            {
                await UpdateServiceAsync(service); // Оновлюємо існуючу послугу
            }
        }

        // Метод для додавання нової послуги
        public async Task AddServiceAsync(OfService service)
        {
            // Додаємо нову послугу до Firebase
            await _firebaseClient.Child("Services").PostAsync(service);
        }

        // Метод для оновлення існуючої послуги
        public async Task UpdateServiceAsync(OfService service)
        {
            // Оновлюємо послугу в Firebase за її ідентифікатором
            await _firebaseClient.Child("Services").Child(service.Id.ToString()).PutAsync(service);
        }

        // Метод для видалення послуги
        public async Task DeleteServiceAsync(string serviceId)
        {
            // Видаляємо послугу з Firebase за її ідентифікатором
            await _firebaseClient.Child("Services").Child(serviceId.ToString()).DeleteAsync();
        }
    }
}
