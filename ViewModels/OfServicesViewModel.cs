using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using course4Hotel.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using course4Hotel.View;
using course4Hotel.DataServices;
using System.Windows.Input;
using Firebase.Database;
using Syncfusion.Maui.Core.Converters;

namespace course4Hotel.ViewModels
{
    public partial class OfServicesViewModel : ObservableObject
    {
        private readonly ServicesService _servicesService; // Сервіс для роботи з базою даних

        public ObservableCollection<OfService> ServiceList { get; set; } // Список послуг для відображення в UI

        [ObservableProperty]
        private OfService _operatingOfService = new(); // Поточний сервіс, який редагується або створюється

        [ObservableProperty]
        private bool _isBusy; // Прапор, що вказує, чи виконується операція (завантаження, збереження тощо)

        [ObservableProperty]
        private string _busyText; // Текст, який відображається під час виконання операції

        public ICommand SetOperatingOfServiceCommand { get; } // Команда для вибору сервісу для редагування
        public ICommand SaveOfServiceCommand { get; } // Команда для збереження сервісу (створення або оновлення)
        public ICommand DeleteOfServiceCommand { get; } // Команда для видалення сервісу

        // Конструктор для ініціалізації сервісу та списку послуг
        public OfServicesViewModel(FirebaseClient firebaseClient)
        {
            _servicesService = new ServicesService(firebaseClient); // Ініціалізація сервісу для роботи з Firebase
            ServiceList = new ObservableCollection<OfService>(); // Створення порожнього списку послуг
            _ = LoadOfServicesAsync(); // Завантаження послуг з Firebase

            // Ініціалізація команд для вибору, збереження та видалення сервісів
            SetOperatingOfServiceCommand = new Command<OfService>(SetOperatingOfService);
            SaveOfServiceCommand = new Command(async () => await SaveOfServiceAsync(OperatingOfService));
            DeleteOfServiceCommand = new Command<OfService>(async (ofService) => await DeleteOfServiceAsync(ofService));
        }

        // Команда для завантаження усіх сервісів з бази даних
        public async Task LoadOfServicesAsync()
        {
            var ofServices = await _servicesService.GetAllServicesAsync(); // Отримання списку послуг з Firebase
            ServiceList.Clear(); // Очищення наявного списку послуг
            foreach (var ofService in ofServices)
            {
                ServiceList.Add(ofService); // Додавання послуг до списку
            }
        }

        // Команда для встановлення поточного редагованого сервісу
        private void SetOperatingOfService(OfService? ofService) => OperatingOfService = ofService ?? new(); // Якщо сервіс не знайдений, створюється новий

        // Команда для збереження сервісу (створення або оновлення)
        public async Task SaveOfServiceAsync(OfService ofService)
        {
            await _servicesService.SaveOfService(ofService); // Викликається метод для збереження сервісу в Firebase
            await LoadOfServicesAsync(); // Оновлення списку послуг після збереження
            OperatingOfService = new OfService(); // Очищення поточного сервісу
        }

        // Команда для видалення сервісу з бази даних
        public async Task DeleteOfServiceAsync(OfService ofService)
        {
            if (ofService == null || string.IsNullOrEmpty(ofService.Id)) // Перевірка на наявність сервісу та його ID
            {
                await Shell.Current.DisplayAlert("Помилка", "Не вдалося видалити сервіс", "ОК"); // Повідомлення про помилку
                return;
            }

            await _servicesService.DeleteServiceAsync(ofService.Id); // Видалення сервісу з Firebase
            ServiceList.Remove(ofService); // Видалення сервісу зі списку
        }
    }
}