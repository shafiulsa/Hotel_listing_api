using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace hotellisting.api.data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions<HotelListingDbContext> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
    }
}