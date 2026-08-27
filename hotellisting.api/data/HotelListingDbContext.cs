using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hotellisting.api.data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace hotellisting.api.data
{
    public class HotelListingDbContext : IdentityDbContext<ApplicationUser>
    {
        public HotelListingDbContext(DbContextOptions<HotelListingDbContext> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

    }
}