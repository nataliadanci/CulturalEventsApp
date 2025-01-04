using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions; // Necesită pentru metodele async
using SQLiteNetExtensions.Attributes;
using CulturalEventsApp.Models;

namespace CulturalEventsApp.Data
{
    public class EventListDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public EventListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<EventList>().Wait();
            _database.CreateTableAsync<Venue>().Wait();
        }
        // Obține toate locațiile
        public async Task<List<Venue>> GetVenuesAsync()
        {
            var venues = await _database.Table<Venue>().ToListAsync();
            foreach (var venue in venues)
            {
                await _database.GetChildrenAsync(venue, true);
            }
            return venues;
        }

        // Obține locație după ID
        public Task<Venue> GetVenueAsync(int venueId)
        {
            return _database.GetWithChildrenAsync<Venue>(venueId);
        }

        
        // Obține lista completă de evenimente cu detalii despre locatii
        public async Task<List<EventList>> GetEventListAsync()
        {
            var events = await _database.Table<EventList>().ToListAsync();
            foreach (var evt in events)
            {
                await _database.GetChildrenAsync(evt, true); // Adaugă detalii despre locație
            }
            return events;
        }


        // Obține evenimente asociate unei locații
        public async Task<List<EventList>> GetEventsByVenueIdAsync(int venueId)
        {
            var events = await _database.Table<EventList>()
                                        .Where(e => e.VenueId == venueId)
                                        .ToListAsync();

            foreach (var evt in events)
            {
                await _database.GetChildrenAsync(evt, true);
            }

            return events;
        }

        // Obține un eveniment specificat după ID
        public Task<EventList> GetVenueListAsync(int id)
        {
            return _database.Table<EventList>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        // Salvează sau actualizează un eveniment
        public async Task<int> SaveEventListAsync(EventList slist)
        {
            try
            {
                if (slist.ID != 0)
                {
                    // Dacă evenimentul există deja, îl actualizez
                    var existingEvent = await _database.Table<EventList>()
                                                        .Where(e => e.ID == slist.ID)
                                                        .FirstOrDefaultAsync();
                    if (existingEvent != null)
                    {
                        return await _database.UpdateAsync(slist);
                    }
                    else
                    {
                        return await _database.InsertAsync(slist);
                    }
                }
                else
                {
                    // Dacă este un eveniment nou, îl inserez
                    return await _database.InsertAsync(slist);
                }
            }
            catch (Exception ex)
            {
                // Gestionare erori
                Console.WriteLine($"Eroare la salvarea evenimentului: {ex.Message}");
                throw;
            }
        }

        // Șterge un eveniment
        public Task<int> DeleteEventListAsync(EventList slist)
        {
            return _database.DeleteAsync(slist);
        }

        // Salvează sau actualizează o locație
        public async Task<int> SaveVenueAsync(Venue venue)
        {
            try
            {
                if (venue.ID != 0)
                {
                    // Verifică dacă locația există deja
                    var existingVenue = await _database.Table<Venue>()
                                                        .Where(v => v.ID == venue.ID)
                                                        .FirstOrDefaultAsync();
                    if (existingVenue != null)
                    {
                        // Actualizează locația
                        return await _database.UpdateAsync(venue);
                    }
                    else
                    {
                        // Adaugă o locație nouă
                        return await _database.InsertAsync(venue);
                    }
                }
                else
                {
                    // Adaugă locația nouă
                    return await _database.InsertAsync(venue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la salvarea locației: {ex.Message}");
                throw;
            }
        }
        public async Task SaveFavoriteEventAsync(EventList eventItem)
        {
            var existingFavorite = await _database.Table<EventList>().FirstOrDefaultAsync(e => e.ID == eventItem.ID && e.IsFavorite);
            if (existingFavorite == null)
            {
                eventItem.IsFavorite = true; // Marks the event as favorite
                await _database.UpdateAsync(eventItem);  // Updates the event in the database
            }
        }
        public Task<List<EventList>> GetFavoriteEventsAsync()
        {
            // Fetch all events that are marked as favorites
            return _database.Table<EventList>().Where(e => e.IsFavorite).ToListAsync();
        }

        public async Task DeleteEventAsync(EventList eventItem)
        {
            await _database.DeleteAsync(eventItem);  // Deletes the event from the database
        }
        public async Task DeleteVenueAsync(Venue venue)
        {
            await _database.DeleteAsync(venue);  // Deletes the venue from the database
        }
    }
}
