﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CulturalEventsApp.Models
{
    public class EventList
    {
        [PrimaryKey, AutoIncrement]

        public int ID { get; set; }
        [MaxLength(250), Unique]

        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
