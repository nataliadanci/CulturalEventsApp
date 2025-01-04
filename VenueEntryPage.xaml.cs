using CulturalEventsApp.Models;

namespace CulturalEventsApp;

public partial class VenueEntryPage : ContentPage
{
    public VenueEntryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadVenuesAsync();
    }

    private async Task LoadVenuesAsync()
    {
        listView.ItemsSource = await App.Database.GetVenuesAsync();
    }

    async void OnVenueAddedClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VenuePage
        {
            BindingContext = new Venue()
        });
    }

    async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var action = await DisplayActionSheet("Opțiuni", "Anulează", null, "Editează", "Șterge");

            var selectedVenue = e.SelectedItem as Venue;

            switch (action)
            {
                case "Editează":
                    await Navigation.PushAsync(new VenuePage
                    {
                        BindingContext = selectedVenue
                    });
                    break;

                case "Șterge":
                    bool confirmDelete = await DisplayAlert("Confirmare", "Sigur vrei să ștergi această locație?", "Da", "Nu");
                    if (confirmDelete)
                    {
                        await App.Database.DeleteVenueAsync(selectedVenue);
                        await LoadVenuesAsync();
                        await DisplayAlert("Succes", "Locația a fost ștearsă cu succes!", "OK");
                    }
                    break;
            }

            listView.SelectedItem = null; // Deselectează elementul
        }
    }
}
