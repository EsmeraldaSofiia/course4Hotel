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

    // ������������� �����������, ��������� �� ���
    public string userId;
    // ������������� ������� ������
    private string currentRoomId;

    // ����������� �� �������������
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
    //����������� �� ���������� ViewModel
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

    // �������� ��䳿 ��� ������ ������ �����
    public async void FindRoomsButton_Clicked(object sender, EventArgs e)
    {
        DateTime checkInDate = CheckInDate.Date;
        DateTime checkOutDate = CheckOutDate.Date;

        // �������� ���������� ��� (���� ����� ������� ���� ������ ���� �'����)
        if (checkOutDate <= checkInDate)
        {
            await DisplayAlert("������� ���� ����", "���� ����� �� ������ �'����", "OK");
            return;
        }
        await _bookingViewModel.LoadRoomsAsync(checkInDate, checkOutDate);
    }

    // �������� ��䳿 ��� ���������� ������ ������ � ��������� ������
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }

    // ����� ��� ������������ ID ����������� ������
    public async void SetRoomId(string roomId)
    {
        currentRoomId = roomId;
       

        var room = (await _bookingService.GetRoomsByDateAsync(CheckInDate.Date, CheckOutDate.Date))
              .FirstOrDefault(r => r.Id == roomId);
        if (room != null)
        {
            NumberLabel.Text = "����� �����������: " + (await _bookingService.GetRoomNumberById(roomId)).ToString();
        DatesLabel.Text = "�� ����� " + CheckInDate.Date.ToString("dd.MM.yyyy") + " - " + CheckOutDate.Date.ToString("dd.MM.yyyy");
        SummaryPriceLabel.Text = "�� ������: " + ((CheckOutDate.Date - CheckInDate.Date).Days * (await _bookingService.GetRoomPriceById(roomId))).ToString() + " UAH";
            RoomImage.Source = room.ImageSource; 
        }
        bookingBlock.IsVisible = true;
    }

    // �������� ��䳿 ��� ���������� ����������
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

    // �������� ��䳿 ��� ���������� ������� ����������
    public async void CancelBookingProcessButton_Clicked(object sender, EventArgs e)
    {
        bookingBlock.IsVisible = false;
    }

    // �������� ��䳿 ��� ������ � ��������� ������
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false); 
    }

    // �������� ��䳿 ��� ������ ���������� ��� ������ ����������
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("��� ������", "��� ���������� ��� ��������� � ��� ��������� ��������� ������ \n\n���������� 45 ����� ������������ 121\n��� '���� ��� '�ϲ''\n\n������������ ������", "������!");
    }
}
