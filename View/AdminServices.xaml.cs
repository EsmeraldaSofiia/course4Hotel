using course4Hotel.ViewModels;
using course4Hotel.Models;
using Firebase.Database;
namespace course4Hotel.View;


public partial class AdminServices : ContentPage
{
    private readonly OfServicesViewModel _viewModel;
    private readonly FirebaseClient _firebaseClient;

    // ��������� ��� ���������� ����� �������� �����
    private bool isServiceFormVisible = false;

    public AdminServices(OfServicesViewModel viewModel)
    {
        InitializeComponent();

        // ����'���� ��������� ������� �� ViewModel
        BindingContext = viewModel;
        _viewModel = viewModel;

        // ���������� ���� ����� � ���������
        UpdateServiceFormVisibility();
    }

    // �����, ���� �����������, ���� ������� �'��������� �� �����
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        // ������������ ������ �� ���� �����
        await _viewModel.LoadOfServicesAsync();
    }

    // ���� ��� ������/������������ ����� ���������/����������� ������
    private void UnfoldFormClicked(object sender, EventArgs e)
    {
        // ���� ����� �������� �����
        isServiceFormVisible = !isServiceFormVisible;

        // ��������� ������� ����� �� ����� ������ �����
        UpdateServiceFormVisibility();
    }

    // ��������� �������� ����� �� ������
    private void UpdateServiceFormVisibility()
    {
        serviceBlock.IsVisible = isServiceFormVisible;
        ServiceFormButton.Text = isServiceFormVisible ? "��������" : "������ �����";
    }

    // ���� ��� ��������� ������
    private async void AddServiceButton_Clicked(object sender, EventArgs e)
    {
        // ���� �������� ����� �� ����, �������� �����
        if (_viewModel.OperatingOfService == null)
        {
            _viewModel.OperatingOfService = new OfService();
        }

        // ���������� ������������ ������
        if (!string.IsNullOrEmpty(ServiceNameEntry.Text) &&
            !string.IsNullOrEmpty(ServicePriceEntry.Text) &&
            !string.IsNullOrEmpty(ServiceDescriptionEntry.Text))
        {
            _viewModel.OperatingOfService.Name = ServiceNameEntry.Text;
            _viewModel.OperatingOfService.Price = float.Parse(ServicePriceEntry.Text);
            _viewModel.OperatingOfService.Description = ServiceDescriptionEntry.Text;

            // ���������� ������
            await _viewModel.SaveOfServiceAsync(_viewModel.OperatingOfService);
            UpdateServiceFormVisibility();

            // �������� ���� �����
            ServiceNameEntry.Text = string.Empty;
            ServicePriceEntry.Text = string.Empty;
            ServiceDescriptionEntry.Text = string.Empty;

            // ������������ �����
            isServiceFormVisible = false;
            UpdateServiceFormVisibility();
        }
        else
        {
            await DisplayAlert("�������", "���� �����, �������� �� ����", "��");
        }
    }

    //���� ��� ����������� ������
    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        if (_viewModel.OperatingOfService != null)
        {
            ServiceNameEntry.Text = _viewModel.OperatingOfService.Name;
            ServicePriceEntry.Text = _viewModel.OperatingOfService.Price.ToString();
            ServiceDescriptionEntry.Text = _viewModel.OperatingOfService.Description;

            // S�������� �����, ���� ���� ���������
            if (!isServiceFormVisible)
            {
                isServiceFormVisible = true;
                UpdateServiceFormVisibility();
            }
        }
        else
        {
            await DisplayAlert("�������", "���� �����, ������� ����� ��� ���������", "��");
        }
    }
    // ���� ��� ���������� ������ ����� �������
    public void BlueButton_Clicked(object sender, EventArgs e)
    {
        Account_frame.IsVisible = !Account_frame.IsVisible;
    }
    //���� ��� ������ � �������
    public async void OnLogOutLabelTapped(object sender, TappedEventArgs e)
    {
        Account_frame.IsVisible = false;
        UserSession.UserId = null;
        Application.Current.MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
        Shell.SetTabBarIsVisible(this, false);
    }
    public async void ShowAuthor_Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("��� ������", "��� ���������� ��� ��������� � ��� ��������� ��������� ������ \n\n���������� 45 ����� ������������ 121\n��� '���� ��� '�ϲ''\n\n������������ ������", "������!");
    }
}