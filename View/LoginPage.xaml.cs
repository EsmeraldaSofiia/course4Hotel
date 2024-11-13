using course4Hotel.Models;
using Firebase.Database;
using Firebase.Database.Query;


namespace course4Hotel.View;

public partial class LoginPage : ContentPage
{

    // Поле для клієнта Firebase
    private readonly FirebaseClient _firebaseClient;

    // Конструктор, що приймає об'єкт FirebaseClient
    public LoginPage(FirebaseClient firebaseClient)
    {
        InitializeComponent();
        _firebaseClient = firebaseClient;
    }

    // Локальна колекція для зберігання інформації про користувачів
    private List<UserInform> _userCollection = new List<UserInform>();

    // Обробник події для переходу до сторінки реєстрації
    private async void OnRegisterLabelTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SinginPage(_firebaseClient));
    }

    // Обробник події для кнопки входу/реєстрації
    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Перевірка на заповнення всіх полів
        if (string.IsNullOrEmpty(FullNameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text) || string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PhoneNumberEntry.Text))
        {
            await DisplayAlert("Помилка", "Будь ласка, заповніть всі поля", "ОК");
            return;
        }
        else
        {
            // Перевірка на існування електронної пошти
            var userExists = await CheckIfEmailExists(EmailEntry.Text);
            if (userExists)
            {
                await DisplayAlert("Помилка", "Користувач з такою електронною поштою вже існує", "ОК");
                return;
            }

            // Перевірка формату електронної пошти
            if (!EmailEntry.Text.Contains("@") || !EmailEntry.Text.Contains("."))
            {
                await DisplayAlert("Помилка", "Введіть коректну електронну пошту", "ОК");
                return;
            }

            // Створення нового екземпляру користувача
            var newUser = new UserInform
            {
                FullName = FullNameEntry.Text,
                Password = PasswordEntry.Text,
                Email = EmailEntry.Text,
                PhoneNumber = PhoneNumberEntry.Text,
                IsAdmin = false,
                AdminCode = "0"
            };

            // Додавання нового користувача до колекції
            _userCollection.Add(newUser);

            await _firebaseClient.Child("Users").PostAsync(newUser);

            // Очищення поля вводу
            FullNameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PhoneNumberEntry.Text = string.Empty;

            // Перехід до сторінки входу
            await Navigation.PushAsync(new SinginPage(_firebaseClient));
        }
    }

    // Метод для перевірки, чи існує користувач із заданою електронною поштою
    private async Task<bool> CheckIfEmailExists(string email)
    {
        // Отримуємо всі користувачі, де електронна пошта відповідає заданій
        var users = await _firebaseClient
            .Child("Users")
            .OnceAsync<UserInform>(); // Change User to UserInform

        // Перевіряємо, чи існує користувач з відповідною електронною поштою
        return users.Any(u => u.Object.Email == email);
    }
}