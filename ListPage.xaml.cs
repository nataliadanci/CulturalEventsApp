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

            // Asigură-te că locația este selectată
            var selectedVenue = VenuePicker.SelectedItem as Venue;
            if (selectedVenue != null)
            {
                ev.VenueId = selectedVenue.ID; // Setează VenueId
                ev.VenueName = selectedVenue.VenueName; // Opțional, pentru afisare rapidă
            }
            else
            {
                await DisplayAlert("Eroare", "Vă rugăm să selectați o locație.", "OK");
                return;
            }

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
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var venues = await App.Database.GetVenuesAsync();
                VenueList = venues.ToList();
                VenuePicker.ItemsSource = VenueList;

                // Selectează locația existentă dacă editezi un eveniment
                var ev = (EventList)BindingContext;
                if (ev != null && ev.VenueId != 0)
                {
                    VenuePicker.SelectedItem = VenueList.FirstOrDefault(v => v.ID == ev.VenueId);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"A apărut o eroare la încărcarea locațiilor: {ex.Message}", "OK");
            }
        }
        private void OnVenueSelected(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedVenue = picker.SelectedItem as Venue;

            if (selectedVenue != null)
            {
                Console.WriteLine($"Locația selectată: {selectedVenue.VenueName}");
            }
        }
        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
