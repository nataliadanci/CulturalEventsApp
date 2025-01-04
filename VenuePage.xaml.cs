using CulturalEventsApp.Models;
using Plugin.LocalNotification;

namespace CulturalEventsApp
{
    public partial class VenuePage : ContentPage
    {
        public VenuePage()
        {
            InitializeComponent();
        }

        // Salvează locația
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var venue = (Venue)BindingContext;
            try
            {
                await App.Database.SaveVenueAsync(venue);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"A apărut o eroare la salvarea locației: {ex.Message}", "OK");
            }
        }

        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Deschide locația pe hartă și trimite notificare
        async void OnShowMapButtonClicked(object sender, EventArgs e)
        {
            var venue = (Venue)BindingContext;
            var address = venue.Adress;

            var options = new MapLaunchOptions
            {
                Name = venue.VenueName
            };

            var venuelocation = new Location(46.7492379, 23.5745597);  // Exemple de coordonate
            var myLocation = new Location(46.7731796289, 23.6213886738); // Exemplu locație curentă
            var distance = myLocation.CalculateDistance(venuelocation, DistanceUnits.Kilometers);

            if (distance < 5)
            {
                var request = new NotificationRequest
                {
                    Title = "Aproape ai ajuns! Nu uita să te distrezi",
                    Description = address,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(1)
                    }
                };
                LocalNotificationCenter.Current.Show(request);
            }

            await Map.OpenAsync(venuelocation, options);
        }
    }
}