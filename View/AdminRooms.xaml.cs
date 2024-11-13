using course4Hotel.Models;
using course4Hotel.ViewModels;
using Firebase.Database;
using Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific;
using Application = Microsoft.Maui.Controls.Application;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;

namespace course4Hotel.View;

public partial class AdminRooms : ContentPage
{
    private readonly RoomsViewModel _viewModel;
    private readonly FirebaseClient _firebaseClient;

    // ��������� ��� ���������� ����� �������� �����
    private bool isRoomFormVisible = false;

    public AdminRooms(RoomsViewModel viewModel, FirebaseClient firebaseClient) 
    {
        InitializeComponent();

        // ����'���� ��������� ������� �� ViewModel
        BindingContext = viewModel;
        _viewModel = viewModel;
        _firebaseClient = firebaseClient;
        // ���������� ���� ����� � ���������
        UpdateRoomFormVisibility();
    }

    // �����, ���� �����������, ���� ������� �'��������� �� �����
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        // ������������ ������ �� ���� �����
        await _viewModel.LoadRoomsAsync();
    }

    // �������� ��䳿 ��� ������ ����������� �����
    private void UnfoldFormClicked(object sender, EventArgs e)
    {
        if (isRoomFormVisible)
        {
            // �������� ���� ����� ��� �������
            RoomNumberEntry.Text = string.Empty;
            PriceEntry.Text = string.Empty;
            floorEntry.Text = string.Empty;
            DescriptionEntry.Text = string.Empty;
        }
        isRoomFormVisible = !isRoomFormVisible;
        UpdateRoomFormVisibility();
    }

    // ��������� �������� ����� � ��������� �� ��������
    private void UpdateRoomFormVisibility()
    {
        roomBlock.IsVisible = isRoomFormVisible;
        RoomFormButton.Text = isRoomFormVisible ? "��������" : "������ ������";
    }

    // �������� ��䳿 ��� ��������� ���� ������
    private async void AddRoomButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel == null)
        {
            await Shell.Current.DisplayAlert("Error", "ViewModel is not initialized.", "Ok");
            return;
        }

        // ���������� OperatingRoom, ���� ���� �������
        if (_viewModel.OperatingRoom == null)
        {
            _viewModel.OperatingRoom = new Room();
        }

        // ��������, �� �� ���� �������� ����� �����������
        if (string.IsNullOrWhiteSpace(RoomNumberEntry.Text) ||
            string.IsNullOrWhiteSpace(PriceEntry.Text) ||
            string.IsNullOrWhiteSpace(floorEntry.Text) ||
            string.IsNullOrWhiteSpace(DescriptionEntry.Text) ||
            string.IsNullOrEmpty(_viewModel.SelectedName))
        {
            await Shell.Current.DisplayAlert("�������", "���� �����, �������� �� ����", "��");
            return;
        }

        try
        {
            // ����������� ������� ��� ������
            _viewModel.OperatingRoom.Number = int.Parse(RoomNumberEntry.Text);
            _viewModel.OperatingRoom.Name = _viewModel.SelectedName;
            _viewModel.OperatingRoom.Price = float.Parse(PriceEntry.Text);
            _viewModel.OperatingRoom.floor = int.Parse(floorEntry.Text);
            _viewModel.OperatingRoom.Description = DescriptionEntry.Text;

            // �������� �� ����������� �����
            if (_viewModel.OperatingRoom.Number <= 0 ||
                _viewModel.OperatingRoom.Price < 0 ||
                _viewModel.OperatingRoom.floor < 0 ||
                string.IsNullOrEmpty(_viewModel.OperatingRoom.Description))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please fill all fields correctly.", "Ok");
                return;
            }
            // ���������� ������ � ���� �����
            await _viewModel.SaveRoomAsync(_viewModel.OperatingRoom);
            UpdateRoomFormVisibility();

            // �������� ����
            RoomNumberEntry.Text = string.Empty;
            PriceEntry.Text = string.Empty;
            floorEntry.Text = string.Empty;
            DescriptionEntry.Text = string.Empty;

            isRoomFormVisible = false;
            UpdateRoomFormVisibility();
        }
        catch (FormatException)
        {
            await Shell.Current.DisplayAlert("�������", "����������� ������ �������� ����", "��");
        }
    }

    // �������� ��䳿 ��� ��������� ���������� ��� ������
    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.OperatingRoom != null)
        {
            RoomNumberEntry.Text = _viewModel.OperatingRoom.Number.ToString();
            PriceEntry.Text = _viewModel.OperatingRoom.Price.ToString();
            floorEntry.Text = _viewModel.OperatingRoom.floor.ToString();
            DescriptionEntry.Text = _viewModel.OperatingRoom.Description;

            if (!isRoomFormVisible)
            {
                isRoomFormVisible = true;
                UpdateRoomFormVisibility();
            }
        }
        else
        {
            await DisplayAlert("�������", "���� �����, ������� ����� ��� ���������", "��");
        }
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
    // �������� ��䳿 ��� ������ ���������� ��� ������
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("��� ������", "��� ���������� ��� ��������� � ��� ��������� ��������� ������ \n\n���������� 45 ����� ������������ 121\n��� '���� ��� '�ϲ''\n\n������������ ������", "������!");
    }
}
