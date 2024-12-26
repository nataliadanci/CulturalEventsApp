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
        listView.ItemsSource = await App.Database.GetVenuesAsync();
    }
    async void OnVenueAddedClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VenuePage
        {
            BindingContext = new Venue()
        });
    }
    async void OnListViewItemSelected(object sender,
   SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            await Navigation.PushAsync(new VenuePage
            {
                BindingContext = e.SelectedItem as Venue
            });
        }
    }
}