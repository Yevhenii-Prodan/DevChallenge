﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sc.DevChallenge.Application.Common;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Domain.Entities;

namespace Sc.DevChallenge.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(ApplicationSettings applicationSettings)
        {
            _connectionString = applicationSettings.SQLiteConnectionString;
        }

        public DbSet<PriceEntity> Prices { get; set; }
        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = _connectionString };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriceEntity>();
            base.OnModelCreating(modelBuilder);
        }
    }
}