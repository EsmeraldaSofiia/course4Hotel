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
    public partial class RoomsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;

        public RoomsViewModel(DatabaseContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<Room> _rooms = new();

        [ObservableProperty]
        private Room _operatingRoom = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        [RelayCommand]
        public async Task LoadRoomsAsync()
        {
            await ExecuteAsync(async () =>
            {
                Rooms.Clear();
                var rooms = await _context.GetAllAsync<Room>();
                if (rooms is not null && rooms.Any())
                {
                    Rooms ??= new ObservableCollection<Room>();

                    foreach (var room in rooms)
                    {
                        Rooms.Add(room);
                    }
                }
            }, "Fetching rooms...");
        }

        [RelayCommand]
        private void SetOperatingRoom(Room? room) => OperatingRoom = room ?? new();

        [RelayCommand]
        private async Task SaveRoomAsync()
        {
            if (OperatingRoom is null)
                return;

            var (isValid, errorMessage) = OperatingRoom.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "Ok");
                return;
            }

            var busyText = OperatingRoom.Id == 0 ? "Creating Room..." : "Updating Room...";
            await ExecuteAsync(async () =>
            {
                if (OperatingRoom.Id == 0)
                {
                    // Create room
                    await _context.AddItemAsync<Room>(OperatingRoom);
                    Rooms.Add(OperatingRoom);
                }
                else
                {
                    // Update room
                    if (await _context.UpdateItemAsync<Room>(OperatingRoom))
                    {
                        var roomCopy = OperatingRoom.Clone;

                        var index = Rooms.IndexOf(OperatingRoom);
                        Rooms.RemoveAt(index);

                        Rooms.Insert(index, roomCopy);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Room updation error", "Ok");
                        return;
                    }
                }
                SetOperatingRoomCommand.Execute(new());
            }, busyText);
        }

        [RelayCommand]
        private async Task DeleteRoomAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeleteItemByKeyAsync<Room>(id))
                {
                    var room = Rooms.FirstOrDefault(p => p.Id == id);
                    Rooms.Remove(room);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Room was not deleted", "Ok");
                }
            }, "Deleting room...");
        }
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
            }

            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }
    }
}

