using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CulturalEventsApp.Models;

namespace CulturalEventsApp.Models
{
    public class Venue
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string VenueName { get; set; }

        public string Adress { get; set; }
        public string VenueDetails
        {
            get
            {
                return VenueName + " - " + Adress;
            }
        }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<EventList> EventLists { get; set; }

    }
}
