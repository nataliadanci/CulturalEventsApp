using System;
using CulturalEventsApp.Data;
using System.IO;

namespace CulturalEventsApp
{
    public partial class App : Application
    {
        static EventListDatabase database;
        public static EventListDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new
                   EventListDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.
                   LocalApplicationData), "EventList.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
