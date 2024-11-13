using Firebase.Database;

namespace course4Hotel.View;

public partial class HeaderView : ContentPage
{
    private FirebaseClient firebaseClient;

    public HeaderView()
    {
        InitializeComponent();
    }

    public void OnCloseLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
    }
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {

        Account_frame.IsVisible = false;
        await Navigation.PushAsync(new SinginPage(firebaseClient));
    }
}
