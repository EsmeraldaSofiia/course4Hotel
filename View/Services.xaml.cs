
using course4Hotel.DataServices;
using course4Hotel.Models;
using course4Hotel.ViewModels;
using Firebase.Database;

namespace course4Hotel.View;

public partial class Services : ContentPage
{
    // Поля для зберігання інформації про користувача, FirebaseClient та ViewModel
    private string userId;
    private readonly FirebaseClient _firebaseClient;
    private readonly OfServicesViewModel _ofServicesViewModel;
    private readonly ServicesService _servicesService;

    // Конструктор за замовчуванням
    public Services()
    {
        InitializeComponent();
        userId = UserSession.UserId;
        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _servicesService = new ServicesService(_firebaseClient);
        _ofServicesViewModel = new OfServicesViewModel(_firebaseClient);
        BindingContext = _ofServicesViewModel;
        LoadServicesAsync();
    }

    // Конструктор, що приймає ViewModel
    public Services(OfServicesViewModel ofServicesViewModel)
    {
        userId = UserSession.UserId;
        InitializeComponent();
        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _servicesService = new ServicesService(_firebaseClient);
        _ofServicesViewModel = ofServicesViewModel;
        BindingContext = _ofServicesViewModel;
    }

    // Завантаження всіх послуг
    public async void LoadServicesAsync()
    {
        await _ofServicesViewModel.LoadOfServicesAsync();
    }

    // Обробник події для кнопки показу кнопки виходу з акаунту
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }

    // Обробник події для кнопки замовлення послуги
    public async void OrderButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Послуга була успішно замовлена,", "очікуйте замовлене найближчим часом", "OK");
    }

    // Обробник події для кнопки виходу з акаунту
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false); // Corrected this line
    }

    // Викликається при появі сторінки на екрані
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadServicesAsync();
    }

    // Обробник події для кнопки показу інформації про автора
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Про автора", "цей застосунок був створений у ході виконання курсового проєкту \n\nстуденткою 45 групи спеціальності 121\nВСП 'ППФК НТУ 'ХПІ''\n\nЖаботинською Софією", "Чудово!");
    }
}
