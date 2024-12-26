using System;
using System.Collections.Generic;
using System.Linq;
using CulturalEventsApp.Models;

namespace CulturalEventsApp
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
        }

        public List<Venue> VenueList { get; set; }

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

            // Picker locație
            var selectedVenue = VenuePicker.SelectedItem as Venue;
            if (selectedVenue != null)
            {
                ev.VenueId = selectedVenue.ID;
                try
                {
                    await App.Database.SaveEventListAsync(ev);
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Eroare", $"A apărut o eroare la salvarea evenimentului: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Eroare", "Vă rugăm să selectați o locație.", "OK");
            }
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var venues = await App.Database.GetVenuesAsync();
                VenueList = venues.ToList();
                VenuePicker.ItemsSource = VenueList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"A apărut o eroare la încărcarea locațiilor: {ex.Message}", "OK");
            }
        }

        private void OnVenueSelected(object sender, EventArgs e)
        {
            var selectedVenue = VenuePicker.SelectedItem as Venue;
            if (selectedVenue != null)
            {
                Console.WriteLine($"Locația selectată: {selectedVenue.VenueName}");
            }
        }
    }
}
