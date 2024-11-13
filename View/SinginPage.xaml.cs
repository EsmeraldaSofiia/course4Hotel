using course4Hotel.DataServices;
using course4Hotel.Models;
using Firebase.Database;

namespace course4Hotel.View;

public partial class SinginPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private string UserId;
    public SinginPage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        _firebaseClient = firebaseClient;
       
    }
    private async void OnRegisterLabelTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_firebaseClient));
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
        {
            await DisplayAlert("�������", "���� �����, ������ ���������� ����� �� ������", "��");
            return;
        }
        else
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            try
            {
                // �������� ��� ������������ � Firebase
                var users = await _firebaseClient
                    .Child("Users")
                    .OnceAsync<UserInform>();

                // ���������, �� ���� ���������� � �������� ����������� ������ �� �������
                var user = users.FirstOrDefault(u => u.Object.Email == email && u.Object.Password == password);

                if (user != null)
                {
                    UserId = user.Key; // Use 'Key' instead of 'Id'

                    // �������� �� isAdmin
                    if (user.Object.IsAdmin)
                    {
                        // ���� ���������� - �����������, ������������� �� ������� ����� ��� ������������ � FirebaseClient
                        Shell.SetTabBarIsVisible(this, false);
                        await Navigation.PushAsync(new AdminSignIn(_firebaseClient));
                    }
                    else
                    {
                        // ���� ���������� - �� �����������, ������������� �� BookingReview
                        Shell.SetTabBarIsVisible(this, false);
                        var bookingService = new BookingService(_firebaseClient); 
                        await Navigation.PushAsync(new UserNavigation(UserId, bookingService));
                    }
                }
                else
                {
                    await DisplayAlert("�������", "������ ���������� ����� ��� ������", "��");
                }
            }
            catch (Exception ex)
            {
                // ������� ������� (���������, �������� � �������)
                await DisplayAlert("�������", "������� ������� ��� ����: " + ex.Message, "��");
            }
        }
    }
}