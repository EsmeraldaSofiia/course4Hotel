using course4Hotel.DataServices;
using course4Hotel.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace course4Hotel.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        private readonly BookingService _bookingService; // Сервіс для роботи з бронюваннями
        public ObservableCollection<Room> RoomList { get; set; } // Список кімнат для відображення в інтерфейсі користувача
        public ObservableCollection<Booking> BookingList { get; } // Список бронювань для відображення в інтерфейсі користувача
        public ICommand SaveBookingCommand { get; } // Команда для збереження бронювання
        public ICommand CancelBookingCommand { get; } // Команда для скасування бронювання (якщо потрібно)

        // Конструктор для ініціалізації сервісу і списків
        public BookingViewModel(BookingService bookingService)
        {
            _bookingService = bookingService; // Ініціалізація сервісу для роботи з бронюваннями
            RoomList = new ObservableCollection<Room>(); // Створення порожнього списку для кімнат
            BookingList = new ObservableCollection<Booking>(); // Створення порожнього списку для бронювань

            // Ініціалізація команди для збереження бронювання
            SaveBookingCommand = new RelayCommand<Booking>(async (booking) => await SaveBookingAsync(booking));
        }

        // Завантаження кімнат для конкретного періоду (під час бронювання)
        public async Task LoadRoomsAsync(DateTime checkInDate, DateTime checkOutDate)
        {
            RoomList.Clear(); // Очищення списку кімнат
            var rooms = await _bookingService.GetRoomsByDateAsync(checkInDate, checkOutDate); // Отримання доступних кімнат за датами
            foreach (var room in rooms)
            {
                RoomList.Add(room); // Додавання кожної доступної кімнати до списку
            }
        }

        // Завантаження бронювань користувача
        public async Task LoadBookingsAsync(string userId)
        {
            var bookings = await _bookingService.GetBookingsAsync(userId); // Отримання бронювань для користувача
            BookingList.Clear(); // Очищення списку бронювань
            foreach (var booking in bookings)
            {
                BookingList.Add(booking); // Додавання кожного бронювання до списку
            }
        }

        // Збереження бронювання
        public async Task SaveBookingAsync(Booking booking)
        {
            await _bookingService.SaveBooking(booking); // Викликається метод для збереження бронювання в базі даних
        }
    }
}
