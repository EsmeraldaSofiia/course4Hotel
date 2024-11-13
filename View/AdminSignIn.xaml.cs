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
        var enteredAdminCode = AdminCodeEntry.Text; // Код, введений користувачем

        try
        {
            // Отримуємо всіх користувачів з бази даних Firebase
            var users = await _firebaseClient
                .Child("Users")
                .OnceAsync<UserInform>();

            // Знаходимо адміністратора за його AdminCode
            var adminUser = users.FirstOrDefault(u => u.Object.IsAdmin && u.Object.AdminCode == enteredAdminCode);

            if (adminUser != null)
            {
                // Якщо AdminCode правильний, створюємо екземпляр RoomsViewModel та переходимо на сторінку AdminRooms
                /*var roomsViewModel = new RoomsViewModel(_firebaseClient); // Створюємо ViewModel
                await Navigation.PushAsync(new AdminRooms(roomsViewModel));*/
                //await Shell.Current.GoToAsync("//AppShell");
                await Navigation.PushAsync(new AppShell());

            }
            else
            {
                // Якщо AdminCode неправильний, показуємо повідомлення про помилку
                await DisplayAlert("Помилка", "Невірний код адміністратора", "ОК");
            }
        }
        catch (Exception ex)
        {
            // Обробка помилки (наприклад, проблеми з мережею)
            await DisplayAlert("Помилка", "Сталася помилка при вході: " + ex.Message, "ОК");
        }
    }
} 