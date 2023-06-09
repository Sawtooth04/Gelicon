﻿using GeliconProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.ApplicationContexts
{
    public interface IAppContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Room> Rooms { get; }
        public DbSet<Color> Colors { get; }

        public int SaveChanges();

        public void DetachAll();
    }
}
