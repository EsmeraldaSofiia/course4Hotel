using course4Hotel.DataServices;
using course4Hotel.Models;
using course4Hotel.ViewModels;
using Firebase.Database;
using Microsoft.Exchange.WebServices.Data;

namespace course4Hotel.View;

public partial class BookingReview : ContentPage
{
    // ������������� �����������
    private string userId;
    // �볺�� Firebase ��� ������ � ����� �����
    private readonly FirebaseClient _firebaseClient;
    // ViewModel ��� ��������� ������������
    private readonly BookingViewModel _bookingViewModel;
    // ����� ��� ������ � ������������
    private readonly BookingService _bookingService;

    // ����������� �� �������������
    public BookingReview()
    {
        InitializeComponent();

        // ����������� UserId, FirebaseClient � BookingService
        userId = UserSession.UserId;
        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _bookingService = new BookingService(_firebaseClient);

        // ����������� BookingViewModel � ����'���� �� ���������
        var bookingService = new BookingService(_firebaseClient);
        _bookingViewModel = new BookingViewModel(bookingService);
        BindingContext = _bookingViewModel;
        LoadAllBookingsAsync(userId);
    }

    // ����� ��� ������������ ��� ��������� ��� ��������� �����������
    public async void LoadAllBookingsAsync(string userId)
    {
        await _bookingViewModel.LoadBookingsAsync(userId);
    }

    // �����������, �� ������ BookingViewModel
    public BookingReview(BookingViewModel bookingViewModel)
    {
        userId = UserSession.UserId;
        InitializeComponent();
        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _bookingService = new BookingService(_firebaseClient);
        _bookingViewModel = bookingViewModel;
        BindingContext = _bookingViewModel;
    }

    // �������� ��䳿 ��� ����������� �������� ����� ��������� ������
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }

    // �������� ��䳿 ��� ������ � ��������� ������
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false);
    }

    // �����, �� ����������� ��� ���� ������� �� �����
    protected override async void OnAppearing()
    {
    base.OnAppearing();
    LoadAllBookingsAsync(userId);
    }

    // �������� ��䳿 ��� ������ ���������� ��� ������
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("��� ������", "��� ���������� ��� ��������� � ��� ��������� ��������� ������ \n\n���������� 45 ����� ������������ 121\n��� '���� ��� '�ϲ''\n\n������������ ������", "������!");
    }
}
