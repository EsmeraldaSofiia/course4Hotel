using course4Hotel.Models;
using Firebase.Database;
using Firebase.Database.Query;


namespace course4Hotel.View;

public partial class LoginPage : ContentPage
{

    // ���� ��� �볺��� Firebase
    private readonly FirebaseClient _firebaseClient;

    // �����������, �� ������ ��'��� FirebaseClient
    public LoginPage(FirebaseClient firebaseClient)
    {
        InitializeComponent();
        _firebaseClient = firebaseClient;
    }

    // �������� �������� ��� ��������� ���������� ��� ������������
    private List<UserInform> _userCollection = new List<UserInform>();

    // �������� ��䳿 ��� �������� �� ������� ���������
    private async void OnRegisterLabelTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SinginPage(_firebaseClient));
    }

    // �������� ��䳿 ��� ������ �����/���������
    private async void Button_Clicked(object sender, EventArgs e)
    {
        // �������� �� ���������� ��� ����
        if (string.IsNullOrEmpty(FullNameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text) || string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PhoneNumberEntry.Text))
        {
            await DisplayAlert("�������", "���� �����, �������� �� ����", "��");
            return;
        }
        else
        {
            // �������� �� ��������� ���������� �����
            var userExists = await CheckIfEmailExists(EmailEntry.Text);
            if (userExists)
            {
                await DisplayAlert("�������", "���������� � ����� ����������� ������ ��� ����", "��");
                return;
            }

            // �������� ������� ���������� �����
            if (!EmailEntry.Text.Contains("@") || !EmailEntry.Text.Contains("."))
            {
                await DisplayAlert("�������", "������ �������� ���������� �����", "��");
                return;
            }

            // ��������� ������ ���������� �����������
            var newUser = new UserInform
            {
                FullName = FullNameEntry.Text,
                Password = PasswordEntry.Text,
                Email = EmailEntry.Text,
                PhoneNumber = PhoneNumberEntry.Text,
                IsAdmin = false,
                AdminCode = "0"
            };

            // ��������� ������ ����������� �� ��������
            _userCollection.Add(newUser);

            await _firebaseClient.Child("Users").PostAsync(newUser);

            // �������� ���� �����
            FullNameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PhoneNumberEntry.Text = string.Empty;

            // ������� �� ������� �����
            await Navigation.PushAsync(new SinginPage(_firebaseClient));
        }
    }

    // ����� ��� ��������, �� ���� ���������� �� ������� ����������� ������
    private async Task<bool> CheckIfEmailExists(string email)
    {
        // �������� �� �����������, �� ���������� ����� ������� ������
        var users = await _firebaseClient
            .Child("Users")
            .OnceAsync<UserInform>(); // Change User to UserInform

        // ����������, �� ���� ���������� � ��������� ����������� ������
        return users.Any(u => u.Object.Email == email);
    }
}