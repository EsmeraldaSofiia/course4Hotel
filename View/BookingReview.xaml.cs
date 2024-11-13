using course4Hotel.DataServices;
using course4Hotel.Models;
using course4Hotel.ViewModels;
using Firebase.Database;
using Microsoft.Exchange.WebServices.Data;

namespace course4Hotel.View;

public partial class BookingReview : ContentPage
{
    // Ідентифікатор користувача
    private string userId;
    // Клієнт Firebase для роботи з базою даних
    private readonly FirebaseClient _firebaseClient;
    // ViewModel для управління бронюваннями
    private readonly BookingViewModel _bookingViewModel;
    // Сервіс для роботи з бронюваннями
    private readonly BookingService _bookingService;

    // Конструктор за замовчуванням
    public BookingReview()
    {
        InitializeComponent();

        // Ініціалізація UserId, FirebaseClient і BookingService
        userId = UserSession.UserId;
        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _bookingService = new BookingService(_firebaseClient);

        // Ініціалізація BookingViewModel і прив'язка до контексту
        var bookingService = new BookingService(_firebaseClient);
        _bookingViewModel = new BookingViewModel(bookingService);
        BindingContext = _bookingViewModel;
        LoadAllBookingsAsync(userId);
    }

    // Метод для завантаження всіх бронювань для вказаного користувача
    public async void LoadAllBookingsAsync(string userId)
    {
        await _bookingViewModel.LoadBookingsAsync(userId);
    }

    // Конструктор, що приймає BookingViewModel
    public BookingReview(BookingViewModel bookingViewModel)
    {
        userId = UserSession.UserId;
        InitializeComponent();
        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _bookingService = new BookingService(_firebaseClient);
        _bookingViewModel = bookingViewModel;
        BindingContext = _bookingViewModel;
    }

    // Обробник події для перемикання видимості блоку облікового запису
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }

    // Обробник події для виходу з облікового запису
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false);
    }

    // Метод, що викликається при появі сторінки на екрані
    protected override async void OnAppearing()
    {
    base.OnAppearing();
    LoadAllBookingsAsync(userId);
    }

    // Обробник події для показу інформації про автора
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Про автора", "цей застосунок був створений у ході виконання курсового проєкту \n\nстуденткою 45 групи спеціальності 121\nВСП 'ППФК НТУ 'ХПІ''\n\nЖаботинською Софією", "Чудово!");
    }
}
