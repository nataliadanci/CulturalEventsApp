using CulturalEventsApp.Models;
using System;
using System;

namespace CulturalEventsApp;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var ev = (EventList)BindingContext;

        // Validare titlu și dată
        if (string.IsNullOrWhiteSpace(ev.Title))
        {
            await DisplayAlert("Eroare", "Titlul este obligatoriu.", "OK");
            return;
        }

        if (ev.Date < DateTime.Now.Date)
        {
            await DisplayAlert("Eroare", "Data evenimentului nu poate fi în trecut.", "OK");
            return;
        }

        await App.Database.SaveEventListAsync(ev);
        await Navigation.PopAsync();
    }


    async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (EventList)BindingContext;
        await App.Database.DeleteEventListAsync(slist);
        await Navigation.PopAsync();
    }
}