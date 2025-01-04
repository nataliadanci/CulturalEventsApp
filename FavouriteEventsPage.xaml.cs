using CulturalEventsApp.Models;

namespace CulturalEventsApp
{
    public partial class FavouriteEventsPage : ContentPage
    {
        // Declare FavoriteEvents as a field
        public List<EventList> FavoriteEvents;

        public FavouriteEventsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadFavoriteEventsAsync();
        }

        private async Task LoadFavoriteEventsAsync()
        {
            try
            {
                // Get favorite events from the database
                FavoriteEvents = await App.Database.GetFavoriteEventsAsync();
                // Set the list view's item source to the favorite events
                FavoriteEventsListView.ItemsSource = FavoriteEvents;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"A apărut o eroare la încărcarea evenimentelor favorite: {ex.Message}", "OK");
            }
        }

        async void OnFavoriteEventSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var action = await DisplayActionSheet("Opțiuni", "Anulează", null, "Șterge din favorite");

                var selectedEvent = e.SelectedItem as EventList;

                switch (action)
                {

                    case "Șterge din favorite":
                        bool confirmDelete = await DisplayAlert("Confirmare", "Sigur vrei să elimini această locație din favorite?", "Da", "Nu");
                        if (confirmDelete)
                        {
                            selectedEvent.IsFavorite = false; // Mark as not favorite
                            await App.Database.SaveEventListAsync(selectedEvent); // Save the change to the database
                            await LoadFavoriteEventsAsync(); // Reload the list
                            await DisplayAlert("Succes", "Locația a fost eliminată din favorite!", "OK");
                        }
                        break;
                }

                FavoriteEventsListView.SelectedItem = null; // Deselect the item
            }
        }
    }
}
