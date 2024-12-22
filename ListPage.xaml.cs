using CulturalEventsApp.Models;
namespace CulturalEventsApp;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (EventList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveEventListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (EventList)BindingContext;
        await App.Database.DeleteEventListAsync(slist);
        await Navigation.PopAsync();
    }
}