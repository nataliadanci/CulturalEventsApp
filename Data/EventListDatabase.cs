using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        }
        public Task<List<EventList>> GetEventListAsync()
        {
            return _database.Table<EventList>().ToListAsync();
        }
        public Task<EventList> GetShopListAsync(int id)
        {
            return _database.Table<EventList>()
            .Where(i => i.ID == id)
           .FirstOrDefaultAsync();
        }
        public Task<int> SaveEventListAsync(EventList slist)
        {
            if (slist.ID != 0)
            {
                return _database.UpdateAsync(slist);
            }
            else
            {
                return _database.InsertAsync(slist);
            }
        }
        public Task<int> DeleteEventListAsync(EventList slist)
        {
            return _database.DeleteAsync(slist);
        }
    }
}
