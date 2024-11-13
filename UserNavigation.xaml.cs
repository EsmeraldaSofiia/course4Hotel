using course4Hotel.DataServices;
using course4Hotel.Models;
using course4Hotel.View;
using course4Hotel.ViewModels;

namespace course4Hotel;

public partial class UserNavigation : Shell
{
    public static readonly BindableProperty UserIdProperty = BindableProperty.Create(
            nameof(UserId), typeof(string), typeof(UserNavigation), string.Empty);

    public string UserId
    {
        get => (string)GetValue(UserIdProperty);
        set => SetValue(UserIdProperty, value);
    }
    private readonly BookingViewModel _bookingViewModel;

    public UserNavigation(string userId, BookingService bookingService)
    {
        InitializeComponent();
        _bookingViewModel = new BookingViewModel(bookingService);
        UserSession.UserId = userId;
    }
}
