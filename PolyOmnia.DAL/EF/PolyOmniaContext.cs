using Microsoft.EntityFrameworkCore;
using PolyOmnia.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolyOmnia.DAL.EF
{
    public class PolyOmniaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Video> Videos { get; set; }

        public PolyOmniaContext(DbContextOptions<PolyOmniaContext> options) : base(options)
        {
        }
    }
}
