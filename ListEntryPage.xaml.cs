using CulturalEventsApp.Models;

namespace CulturalEventsApp;

public partial class ListEntryPage : ContentPage
{
    public ListEntryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetEventListAsync();
    }

    async void OnEventAddedClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListPage
        {
            BindingContext = new EventList()
        });
    }

    async void OnEventViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedEvent = e.SelectedItem as EventList;

            var action = await DisplayActionSheet("Opțiuni", "Anulează", null, "Editează", "Șterge");

            switch (action)
            {
                case "Editează":
                    await Navigation.PushAsync(new ListPage
                    {
                        BindingContext = selectedEvent
                    });
                    break;

                case "Șterge":
                    bool confirmDelete = await DisplayAlert("Confirmare", "Sigur vrei să ștergi acest eveniment?", "Da", "Nu");
                    if (confirmDelete)
                    {
                        await App.Database.DeleteEventAsync(selectedEvent); // Șterge evenimentul din baza de date
                        await RefreshEventListAsync(); // Reîncarcă lista de evenimente
                        await DisplayAlert("Succes", "Evenimentul a fost șters cu succes!", "OK");
                    }
                    break;
            }

            listView.SelectedItem = null; // Deselectez elementul
        }
    }

    private async Task RefreshEventListAsync()
    {
        try
        {
            listView.ItemsSource = await App.Database.GetEventListAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A apărut o eroare la reîncărcarea listei: {ex.Message}", "OK");
        }
    }

}
