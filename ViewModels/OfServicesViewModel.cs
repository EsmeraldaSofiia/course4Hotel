using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using course4Hotel.Data;
using course4Hotel.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using course4Hotel.View;

namespace course4Hotel.ViewModels
{
    public partial class OfServicesViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;

        public OfServicesViewModel(DatabaseContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<OfService> _ofServices = new();

        [ObservableProperty]
        private OfService _operatingOfService = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        // Команда для завантаження усіх сервісів
        [RelayCommand]
        public async Task LoadOfServicesAsync()
        {
            await ExecuteAsync(async () =>
            {
                OfServices.Clear();
                var services = await _context.GetAllAsync<OfService>();
                if (services is not null && services.Any())
                {
                    OfServices ??= new ObservableCollection<OfService>();

                    foreach (var service in services)
                    {
                        OfServices.Add(service);
                    }
                }
            }, "Fetching services...");
        }

        // Команда для встановлення поточного редагованого сервісу
        [RelayCommand]
        private void SetOperatingOfService(OfService? ofService) => OperatingOfService = ofService ?? new();

        // Команда для збереження сервісу (створення або оновлення)
        [RelayCommand]
        private async Task SaveOfServiceAsync()
        {
            if (OperatingOfService is null)
                return;

            var (isValid, errorMessage) = OperatingOfService.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "Ok");
                return;
            }

            var busyText = OperatingOfService.Id == 0 ? "Creating Service..." : "Updating Service...";
            await ExecuteAsync(async () =>
            {
                if (OperatingOfService.Id == 0)
                {
                    // Створення сервісу
                    await _context.AddItemAsync<OfService>(OperatingOfService);
                    OfServices.Add(OperatingOfService);
                }
                else
                {
                    // Оновлення сервісу
                    if (await _context.UpdateItemAsync<OfService>(OperatingOfService))
                    {
                        var serviceCopy = OperatingOfService.Clone;

                        var index = OfServices.IndexOf(OperatingOfService);
                        OfServices.RemoveAt(index);

                        OfServices.Insert(index, serviceCopy);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Service updation error", "Ok");
                        return;
                    }
                }
                SetOperatingOfServiceCommand.Execute(new());
            }, busyText);
        }

        // Команда для видалення сервісу
        [RelayCommand]
        private async Task DeleteOfServiceAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeleteItemByKeyAsync<OfService>(id))
                {
                    var service = OfServices.FirstOrDefault(p => p.Id == id);
                    OfServices.Remove(service);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Service was not deleted", "Ok");
                }
            }, "Deleting service...");
        }

        // Допоміжний метод для обробки виконання команд із відображенням "busy state"
        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processing...";
            try
            {
                await operation?.Invoke();
            }
            catch (Exception ex)
            {
                // Логування або обробка помилки
            }
            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }
    }
}