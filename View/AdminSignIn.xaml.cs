using course4Hotel.Models;
using course4Hotel.ViewModels;
using Firebase.Database;

namespace course4Hotel.View;

public partial class AdminSignIn : ContentPage
{
    private readonly FirebaseClient _firebaseClient;

    public AdminSignIn(FirebaseClient firebaseClient)
    {
        InitializeComponent();
        _firebaseClient = firebaseClient;
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var enteredAdminCode = AdminCodeEntry.Text; // ���, �������� ������������

        try
        {
            // �������� ��� ������������ � ���� ����� Firebase
            var users = await _firebaseClient
                .Child("Users")
                .OnceAsync<UserInform>();

            // ��������� ������������ �� ���� AdminCode
            var adminUser = users.FirstOrDefault(u => u.Object.IsAdmin && u.Object.AdminCode == enteredAdminCode);

            if (adminUser != null)
            {
                // ���� AdminCode ����������, ��������� ��������� RoomsViewModel �� ���������� �� ������� AdminRooms
                /*var roomsViewModel = new RoomsViewModel(_firebaseClient); // ��������� ViewModel
                await Navigation.PushAsync(new AdminRooms(roomsViewModel));*/
                //await Shell.Current.GoToAsync("//AppShell");
                await Navigation.PushAsync(new AppShell());

            }
            else
            {
                // ���� AdminCode ������������, �������� ����������� ��� �������
                await DisplayAlert("�������", "������� ��� ������������", "��");
            }
        }
        catch (Exception ex)
        {
            // ������� ������� (���������, �������� � �������)
            await DisplayAlert("�������", "������� ������� ��� ����: " + ex.Message, "��");
        }
    }
} 