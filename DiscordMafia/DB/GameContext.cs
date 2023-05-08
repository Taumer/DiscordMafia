﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiscordMafia.DB
{
    // TODO Переделать на сервис
    public class GameContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameUser> GameUsers { get; set; }

        public GameContext()
            : base()
        {

        }

        public GameContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(Program.Connection.ConnectionString);
            }
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            optionsBuilder.UseLoggerFactory(loggerFactory);
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameUser>()
                .HasKey(gu => new { gu.GameId, gu.UserId });

            modelBuilder.Entity<GameUser>()
                .HasOne(u => u.Game)
                .WithMany(g => g.Users)
                .HasForeignKey(gu => gu.GameId);

            modelBuilder.Entity<GameUser>()
                .HasOne(u => u.User)
                .WithMany(u => u.Games)
                .HasForeignKey(gu => gu.UserId);
        }
    }
}