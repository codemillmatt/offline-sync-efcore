using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Xamarin.Essentials;

namespace FitApp.Core
{
    public class EFCoreDataContext : DbContext
    {
        public DbSet<DataSyncInfo> DataSyncInfo { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }

        public EFCoreDataContext()
        {
            SQLitePCL.Batteries_V2.Init();

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "sessions.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }
    }
}
