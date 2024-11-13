using course4Hotel.DataServices;
using course4Hotel.Models;
using course4Hotel.ViewModels;
using Firebase.Database;
using Microsoft.Exchange.WebServices.Data;
using System.Windows.Input;

namespace course4Hotel.View;

public partial class AddBooking : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private readonly BookingViewModel _bookingViewModel;
    private readonly BookingService _bookingService; 
    public ICommand SetRoomIdCommand { get; private set; }

    // Ідентифікатор користувача, отриманий із сесії
    public string userId;
    // Ідентифікатор поточної кімнати
    private string currentRoomId;

    // Конструктор за замовчуванням
    public AddBooking()
    {
        InitializeComponent();
        userId = UserSession.UserId;
        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _bookingService = new BookingService(_firebaseClient);
        var bookingService = new BookingService(_firebaseClient);
        _bookingViewModel = new BookingViewModel(bookingService);
        BindingContext = _bookingViewModel;
        bookingBlock.IsVisible = false; 
        currentRoomId = string.Empty;
        SetRoomIdCommand = new Command<string>(SetRoomId);
    }
    //Конструктор із параметром ViewModel
    public AddBooking(BookingViewModel bookingViewModel)
    {
        InitializeComponent();

        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _bookingService = new BookingService(_firebaseClient);
        _bookingViewModel = bookingViewModel;
        BindingContext = _bookingViewModel;
        bookingBlock.IsVisible = false; 
        currentRoomId = string.Empty; 
    }

    // Обробник події для кнопки пошуку кімнат
    public async void FindRoomsButton_Clicked(object sender, EventArgs e)
    {
        DateTime checkInDate = CheckInDate.Date;
        DateTime checkOutDate = CheckOutDate.Date;

        // Перевірка коректності дат (дата виїзду повинна бути пізніше дати в'їзду)
        if (checkOutDate <= checkInDate)
        {
            await DisplayAlert("Виберіть інші дати", "Дата виїзду має пізніше в'їзду", "OK");
            return;
        }
        await _bookingViewModel.LoadRoomsAsync(checkInDate, checkOutDate);
    }

    // Обробник події для відкривання кнопки виходу з облікового запису
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }

    // Метод для встановлення ID оброблюваної кімнати
    public async void SetRoomId(string roomId)
    {
        currentRoomId = roomId;
       

        var room = (await _bookingService.GetRoomsByDateAsync(CheckInDate.Date, CheckOutDate.Date))
              .FirstOrDefault(r => r.Id == roomId);
        if (room != null)
        {
            NumberLabel.Text = "Номер апартаментів: " + (await _bookingService.GetRoomNumberById(roomId)).ToString();
        DatesLabel.Text = "на період " + CheckInDate.Date.ToString("dd.MM.yyyy") + " - " + CheckOutDate.Date.ToString("dd.MM.yyyy");
        SummaryPriceLabel.Text = "До сплати: " + ((CheckOutDate.Date - CheckInDate.Date).Days * (await _bookingService.GetRoomPriceById(roomId))).ToString() + " UAH";
            RoomImage.Source = room.ImageSource; 
        }
        bookingBlock.IsVisible = true;
    }

    // Обробник події для збереження бронювання
    public async void SaveBookingButton_Clicked(object sender, EventArgs e)
    {
        var booking = new Booking
        {
            RoomId = currentRoomId,
            RoomNumber = await _bookingService.GetRoomNumberById(currentRoomId) ?? 0,
            BookingDate = DateTime.Now,
            CheckInDate = CheckInDate.Date,
            CustomerId = userId, 
            CheckOutDate = CheckOutDate.Date
        };
        await _bookingViewModel.SaveBookingAsync(booking);
        bookingBlock.IsVisible = false;
        await _bookingViewModel.LoadRoomsAsync(CheckInDate.Date, CheckOutDate.Date);
    }

    // Обробник події для скасування процесу бронювання
    public async void CancelBookingProcessButton_Clicked(object sender, EventArgs e)
    {
        bookingBlock.IsVisible = false;
    }

    // Обробник події для виходу з облікового запису
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false); 
    }

    // Обробник події для показу інформації про автора застосунку
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Про автора", "цей застосунок був створений у ході виконання курсового проєкту \n\nстуденткою 45 групи спеціальності 121\nВСП 'ППФК НТУ 'ХПІ''\n\nЖаботинською Софією", "Чудово!");
    }
}
