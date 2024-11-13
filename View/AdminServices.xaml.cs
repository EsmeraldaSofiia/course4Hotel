using course4Hotel.ViewModels;
using course4Hotel.Models;
using Firebase.Database;
namespace course4Hotel.View;


public partial class AdminServices : ContentPage
{
    private readonly OfServicesViewModel _viewModel;
    private readonly FirebaseClient _firebaseClient;

    // Прапорець для відстеження стану видимості форми
    private bool isServiceFormVisible = false;

    public AdminServices(OfServicesViewModel viewModel)
    {
        InitializeComponent();

        // Прив'язка контексту сторінки до ViewModel
        BindingContext = viewModel;
        _viewModel = viewModel;

        // Початковий стан форми — прихована
        UpdateServiceFormVisibility();
    }

    // Метод, який викликається, коли сторінка з'являється на екрані
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        // Завантаження сервісів із бази даних
        await _viewModel.LoadOfServicesAsync();
    }

    // Подія для показу/приховування форми додавання/редагування сервісу
    private void UnfoldFormClicked(object sender, EventArgs e)
    {
        // Зміна стану видимості форми
        isServiceFormVisible = !isServiceFormVisible;

        // Оновлення вигляду форми на основі нового стану
        UpdateServiceFormVisibility();
    }

    // Оновлення видимості форми та кнопки
    private void UpdateServiceFormVisibility()
    {
        serviceBlock.IsVisible = isServiceFormVisible;
        ServiceFormButton.Text = isServiceFormVisible ? "Згорнути" : "Додати сервіс";
    }

    // Подія для додавання сервісу
    private async void AddServiceButton_Clicked(object sender, EventArgs e)
    {
        // Якщо поточний сервіс не існує, створити новий
        if (_viewModel.OperatingOfService == null)
        {
            _viewModel.OperatingOfService = new OfService();
        }

        // Заповнення властивостей сервісу
        if (!string.IsNullOrEmpty(ServiceNameEntry.Text) &&
            !string.IsNullOrEmpty(ServicePriceEntry.Text) &&
            !string.IsNullOrEmpty(ServiceDescriptionEntry.Text))
        {
            _viewModel.OperatingOfService.Name = ServiceNameEntry.Text;
            _viewModel.OperatingOfService.Price = float.Parse(ServicePriceEntry.Text);
            _viewModel.OperatingOfService.Description = ServiceDescriptionEntry.Text;

            // Збереження сервісу
            await _viewModel.SaveOfServiceAsync(_viewModel.OperatingOfService);
            UpdateServiceFormVisibility();

            // Очищення полів вводу
            ServiceNameEntry.Text = string.Empty;
            ServicePriceEntry.Text = string.Empty;
            ServiceDescriptionEntry.Text = string.Empty;

            // Приховування форми
            isServiceFormVisible = false;
            UpdateServiceFormVisibility();
        }
        else
        {
            await DisplayAlert("Помилка", "Будь ласка, заповніть всі поля", "ОК");
        }
    }

    //Подія для редагування сервісу
    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.OperatingOfService != null)
        {
            ServiceNameEntry.Text = _viewModel.OperatingOfService.Name;
            ServicePriceEntry.Text = _viewModel.OperatingOfService.Price.ToString();
            ServiceDescriptionEntry.Text = _viewModel.OperatingOfService.Description;

            // Sпоказати форму, якщо вона прихована
            if (!isServiceFormVisible)
            {
                isServiceFormVisible = true;
                UpdateServiceFormVisibility();
            }
        }
        else
        {
            await DisplayAlert("Помилка", "Будь ласка, виберіть сервіс для оновлення", "ОК");
        }
    }
    // Подія для натиснення кнопки опцій акаунту
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }
    //Подія для виходу з акаунту
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false);
    }
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Про автора", "цей застосунок був створений у ході виконання курсового проєкту \n\nстуденткою 45 групи спеціальності 121\nВСП 'ППФК НТУ 'ХПІ''\n\nЖаботинською Софією", "Чудово!");
    }
}