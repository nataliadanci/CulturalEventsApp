using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
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

        // Obține lista de evenimente
        public Task<List<EventList>> GetEventListAsync()
        {
            return _database.Table<EventList>().ToListAsync();
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

        // Obține lista de locații
        public Task<List<Venue>> GetVenuesAsync()
        {
            return _database.Table<Venue>().ToListAsync();
        }

        // Salvează sau actualizează o locație
        public async Task<int> SaveVenueAsync(Venue venue)
        {
            try
            {
                if (venue.ID != 0)
                {
                    var existingVenue = await _database.Table<Venue>()
                                                        .Where(v => v.ID == venue.ID)
                                                        .FirstOrDefaultAsync();
                    if (existingVenue != null)
                    {
                        return await _database.UpdateAsync(venue);
                    }
                    else
                    {
                        return await _database.InsertAsync(venue);
                    }
                }
                else
                {
                    return await _database.InsertAsync(venue);
                }
            }
            catch (Exception ex)
            {
                // Gestionare erori
                Console.WriteLine($"Eroare la salvarea locației: {ex.Message}");
                throw;
            }
        }
    }
}
