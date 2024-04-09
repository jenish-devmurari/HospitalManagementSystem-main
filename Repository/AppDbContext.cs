using Microsoft.EntityFrameworkCore;
using Repository.Modal;

namespace Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {  }
       
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }

        public virtual DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    UserId = 1,
                    FirstName = "Jenish",
                    LastName = "Devmurari",
                    Email = "jenishdev07@gmail.com",
                    PhoneNumber = "1234567890",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Sex = "Male",
                    Address = "123 Main Street",
                    PostalCode = "123456",
                    Role = "Doctor",
                    Password = "Y7vlZKpcCII1X23NbyJBtvjQLW85cllmrlZM0xDweFA="
                }
            );

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor
                {
                    DoctorId = 1,
                    UserId = 1,
                    Speciallization = "Brain Surgery"
                }
            );

            modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Patient)
               .WithMany(u => u.PatientAppointments)
               .HasForeignKey(a => a.PatientId)
               .IsRequired();

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Nurse)
                .WithMany(u => u.NurseAppointments)
                .HasForeignKey(a => a.NurseId)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
