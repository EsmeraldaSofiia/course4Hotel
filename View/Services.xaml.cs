
using course4Hotel.DataServices;
using course4Hotel.Models;
using course4Hotel.ViewModels;
using Firebase.Database;

namespace course4Hotel.View;

public partial class Services : ContentPage
{
    // ���� ��� ��������� ���������� ��� �����������, FirebaseClient �� ViewModel
    private string userId;
    private readonly FirebaseClient _firebaseClient;
    private readonly OfServicesViewModel _ofServicesViewModel;
    private readonly ServicesService _servicesService;

    // ����������� �� �������������
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

    // �����������, �� ������ ViewModel
    public Services(OfServicesViewModel ofServicesViewModel)
    {
        userId = UserSession.UserId;
        InitializeComponent();
        _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
        _servicesService = new ServicesService(_firebaseClient);
        _ofServicesViewModel = ofServicesViewModel;
        BindingContext = _ofServicesViewModel;
    }

    // ������������ ��� ������
    public async void LoadServicesAsync()
    {
        await _ofServicesViewModel.LoadOfServicesAsync();
    }

    // �������� ��䳿 ��� ������ ������ ������ ������ � �������
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }

    // �������� ��䳿 ��� ������ ���������� �������
    public async void OrderButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("������� ���� ������ ���������,", "�������� ��������� ���������� �����", "OK");
    }

    // �������� ��䳿 ��� ������ ������ � �������
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false); // Corrected this line
    }

    // ����������� ��� ���� ������� �� �����
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadServicesAsync();
    }

    // �������� ��䳿 ��� ������ ������ ���������� ��� ������
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("��� ������", "��� ���������� ��� ��������� � ��� ��������� ��������� ������ \n\n���������� 45 ����� ������������ 121\n��� '���� ��� '�ϲ''\n\n������������ ������", "������!");
    }
}
