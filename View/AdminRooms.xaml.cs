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

    // Прапорець для відстеження стану видимості форми
    private bool isRoomFormVisible = false;

    public AdminRooms(RoomsViewModel viewModel, FirebaseClient firebaseClient) 
    {
        InitializeComponent();

        // Прив'язка контексту сторінки до ViewModel
        BindingContext = viewModel;
        _viewModel = viewModel;
        _firebaseClient = firebaseClient;
        // Початковий стан форми — прихована
        UpdateRoomFormVisibility();
    }

    // Метод, який викликається, коли сторінка з'являється на екрані
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        // Завантаження номерів із бази даних
        await _viewModel.LoadRoomsAsync();
    }

    // Обробник події для кнопки розгортання форми
    private void UnfoldFormClicked(object sender, EventArgs e)
    {
        if (isRoomFormVisible)
        {
            // Очищення полів форми при закритті
            RoomNumberEntry.Text = string.Empty;
            PriceEntry.Text = string.Empty;
            floorEntry.Text = string.Empty;
            DescriptionEntry.Text = string.Empty;
        }
        isRoomFormVisible = !isRoomFormVisible;
        UpdateRoomFormVisibility();
    }

    // Оновлення видимості форми в залежності від прапорця
    private void UpdateRoomFormVisibility()
    {
        roomBlock.IsVisible = isRoomFormVisible;
        RoomFormButton.Text = isRoomFormVisible ? "Згорнути" : "Додати кімнату";
    }

    // Обробник події для додавання нової кімнати
    private async void AddRoomButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel == null)
        {
            await Shell.Current.DisplayAlert("Error", "ViewModel is not initialized.", "Ok");
            return;
        }

        // Ініціалізуємо OperatingRoom, якщо вона відсутня
        if (_viewModel.OperatingRoom == null)
        {
            _viewModel.OperatingRoom = new Room();
        }

        // Перевірка, що всі поля заповнені перед збереженням
        if (string.IsNullOrWhiteSpace(RoomNumberEntry.Text) ||
            string.IsNullOrWhiteSpace(PriceEntry.Text) ||
            string.IsNullOrWhiteSpace(floorEntry.Text) ||
            string.IsNullOrWhiteSpace(DescriptionEntry.Text) ||
            string.IsNullOrEmpty(_viewModel.SelectedName))
        {
            await Shell.Current.DisplayAlert("Помилка", "Будь ласка, заповніть всі поля", "ОК");
            return;
        }

        try
        {
            // Ініціалізація значень для кімнати
            _viewModel.OperatingRoom.Number = int.Parse(RoomNumberEntry.Text);
            _viewModel.OperatingRoom.Name = _viewModel.SelectedName;
            _viewModel.OperatingRoom.Price = float.Parse(PriceEntry.Text);
            _viewModel.OperatingRoom.floor = int.Parse(floorEntry.Text);
            _viewModel.OperatingRoom.Description = DescriptionEntry.Text;

            // Перевірка на правильність даних
            if (_viewModel.OperatingRoom.Number <= 0 ||
                _viewModel.OperatingRoom.Price < 0 ||
                _viewModel.OperatingRoom.floor < 0 ||
                string.IsNullOrEmpty(_viewModel.OperatingRoom.Description))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please fill all fields correctly.", "Ok");
                return;
            }
            // Збереження кімнати в базу даних
            await _viewModel.SaveRoomAsync(_viewModel.OperatingRoom);
            UpdateRoomFormVisibility();

            // Очищення полів
            RoomNumberEntry.Text = string.Empty;
            PriceEntry.Text = string.Empty;
            floorEntry.Text = string.Empty;
            DescriptionEntry.Text = string.Empty;

            isRoomFormVisible = false;
            UpdateRoomFormVisibility();
        }
        catch (FormatException)
        {
            await Shell.Current.DisplayAlert("Помилка", "Некоректний формат числових полів", "ОК");
        }
    }

    // Обробник події для оновлення інформації про кімнату
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
            await DisplayAlert("Помилка", "Будь ласка, виберіть номер для оновлення", "ОК");
        }
    }

    // Обробник події для перемикання видимості блоку облікового запису
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }

    // Обробник події для виходу з облікового запису
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false);
    }
    // Обробник події для показу інформації про автора
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Про автора", "цей застосунок був створений у ході виконання курсового проєкту \n\nстуденткою 45 групи спеціальності 121\nВСП 'ППФК НТУ 'ХПІ''\n\nЖаботинською Софією", "Чудово!");
    }
}
