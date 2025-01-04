using SQLite;
using System;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CulturalEventsApp.Models
{
    public class EventList
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(250), Unique]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(250), Unique]
        public string Title { get; set; }

        [ForeignKey(typeof(Venue))]
        public int VenueId { get; set; }

        public bool IsFavorite { get; set; }
        public override string ToString()
        {
            return Title;
        }

        [Ignore]
        public ICommand DeleteCommand { get; set; }

    }
}
