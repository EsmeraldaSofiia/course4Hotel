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
using Firebase.Database;
using Firebase.Database.Query;
using course4Hotel.DataServices;
using System.Windows.Input;

namespace course4Hotel.ViewModels
{
    public partial class RoomsViewModel : ObservableObject
    {
        private readonly RoomService _roomService; // Сервіс для роботи з базою даних
        public ObservableCollection<Room> RoomList { get; set; } // Список кімнат для відображення в UI

        private string _selectedName; // Назва обраної кімнати
        public string SelectedName
        {
            get => _selectedName;
            set => SetProperty(ref _selectedName, value); // Встановлення вибраної назви кімнати
        }

        [ObservableProperty]
        private bool _isBusy; // Прапорець, що вказує, чи виконується операція (завантаження, збереження тощо)

        [ObservableProperty]
        private string _busyText; // Текст, який відображається під час виконання операції

        private Room _operatingRoom; // Кімната, яку зараз обробляють (для редагування або створення нової)
        public Room OperatingRoom
        {
            get => _operatingRoom;
            set => SetProperty(ref _operatingRoom, value); // Встановлення операційної кімнати
        }

        public ICommand SaveRoomCommand { get; } // Команда для збереження кімнати
        public ICommand SetOperatingRoomCommand { get; } // Команда для вибору кімнати для редагування
        public ICommand DeleteRoomCommand { get; } // Команда для видалення кімнати

        // Конструктор для ініціалізації сервісу та списку кімнат
        public RoomsViewModel(FirebaseClient firebaseClient)
        {
            _roomService = new RoomService(firebaseClient); // Ініціалізація сервісу для роботи з Firebase
            RoomList = new ObservableCollection<Room>(); // Створення порожнього списку кімнат
            _ = LoadRoomsAsync(); // Завантаження кімнат з Firebase

            // Команди для збереження, вибору та видалення кімнат
            SaveRoomCommand = new Command(async () => await SaveRoomAsync(OperatingRoom));
            SetOperatingRoomCommand = new Command<Room>(async (room) => await SetOperatingRoomAsync(room));
            DeleteRoomCommand = new Command<Room>(async (room) => await DeleteRoomAsync(room));
        }

        // Завантаження кімнат з бази даних
        public async Task LoadRoomsAsync()
        {
            var rooms = await _roomService.GetAllRoomsAsync(); // Отримання списку кімнат з Firebase
            RoomList.Clear(); // Очищення наявного списку кімнат
            foreach (var room in rooms)
            {
                RoomList.Add(room); // Додавання отриманих кімнат до списку
            }
        }

        // Збереження кімнати в базі даних
        public async Task SaveRoomAsync(Room room)
        {
            await _roomService.SaveRoom(room); // Виклик методу збереження кімнати
            await LoadRoomsAsync(); // Завантаження оновленого списку кімнат
            OperatingRoom = new Room(); // Очищення операційної кімнати
            SelectedName = string.Empty; // Очищення вибраної назви кімнати
        }

        // Видалення кімнати з бази даних
        public async Task DeleteRoomAsync(Room room)
        {
            if (room == null || string.IsNullOrEmpty(room.Id)) // Перевірка на наявність кімнати та її ID
            {
                await Shell.Current.DisplayAlert("Error", "Room or Room ID is null", "OK"); // Повідомлення про помилку
                return;
            }

            await _roomService.DeleteRoomAsync(room.Id); // Виклик методу для видалення кімнати з Firebase
            RoomList.Remove(room); // Видалення кімнати зі списку
        }

        // Встановлення операційної кімнати для редагування або створення
        private async Task SetOperatingRoomAsync(Room? room)
        {
            OperatingRoom = room ?? new Room(); // Якщо кімната не знайдена, створюємо нову
            SelectedName = room?.Name ?? string.Empty; // Встановлення назви кімнати або порожнього рядка
        }
    }
}


