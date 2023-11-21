using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Noested.Models;

namespace Noested.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customer { get; set; } = default!;
        public DbSet<ServiceOrder> ServiceOrder { get; set; } = default!;
        public DbSet<Checklist> Checklist { get; set; } = default!;
        public DbSet<WinchChecklist> WinchChecklist { get; set; } = default!;
    }
}