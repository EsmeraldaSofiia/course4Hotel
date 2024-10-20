using course4Hotel.ViewModels;
using course4Hotel.Models;
using course4Hotel.Data;
namespace course4Hotel.View;


public partial class AdminServices : ContentPage
{
    private readonly OfServicesViewModel _viewModel;

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
}