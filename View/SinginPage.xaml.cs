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
            await DisplayAlert("Помилка", "Будь ласка, введіть електронну пошту та пароль", "ОК");
            return;
        }
        else
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            try
            {
                // Отримати всіх користувачів з Firebase
                var users = await _firebaseClient
                    .Child("Users")
                    .OnceAsync<UserInform>();

                // Перевірити, чи існує користувач з наданими електронною поштою та паролем
                var user = users.FirstOrDefault(u => u.Object.Email == email && u.Object.Password == password);

                if (user != null)
                {
                    UserId = user.Key; // Use 'Key' instead of 'Id'

                    // Перевірка на isAdmin
                    if (user.Object.IsAdmin)
                    {
                        // Якщо користувач - адміністратор, перенаправити на сторінку входу для адміністраторів з FirebaseClient
                        Shell.SetTabBarIsVisible(this, false);
                        await Navigation.PushAsync(new AdminSignIn(_firebaseClient));
                    }
                    else
                    {
                        // Якщо користувач - не адміністратор, перенаправити на BookingReview
                        Shell.SetTabBarIsVisible(this, false);
                        var bookingService = new BookingService(_firebaseClient); 
                        await Navigation.PushAsync(new UserNavigation(UserId, bookingService));
                    }
                }
                else
                {
                    await DisplayAlert("Помилка", "Невірна електронна пошта або пароль", "ОК");
                }
            }
            catch (Exception ex)
            {
                // Обробка помилки (наприклад, проблеми з мережею)
                await DisplayAlert("Помилка", "Сталася помилка при вході: " + ex.Message, "ОК");
            }
        }
    }
}
