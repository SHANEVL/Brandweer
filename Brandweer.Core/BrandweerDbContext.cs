using Brandweer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Brandweer.Core
{
    public class BrandweerDbContext : IdentityDbContext<IdentityUser>
    {
        public BrandweerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee>  Employees => Set<Employee>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<VehicleEmployee> VehicleEmployees => Set<VehicleEmployee>();

        public void Seed()
        {
            AddDefaultRoles();
            AddDefaultUsers();
            SaveChanges();

            if (Employees.Any() || Vehicles.Any())
            {
                return;
            }

            AddEmployees();
            AddVehicles();
            SaveChanges();

            AssignVehiclesToEmployees();
            SaveChanges();
        }

        private void AddEmployees()
        {
            for (int i = 1; i <= 10; i++)
            {
                Employees.Add(new Employee
                {
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                });
            }
        }

        private void AddVehicles()
        {
            for (int i = 1; i <= 3; i++)
            {
                _ = Vehicles.Add(new Vehicle
                {
                    Description = $"Description{i}",
                    Capacity = i * 5
                });
            }
        }

        private void AssignVehiclesToEmployees()
        {
            var allVehicles = Vehicles.ToList();
            var allEmployees = Employees.ToList();

            for (int i = 0; i < allVehicles.Count; i++)
            {
                foreach (var employee in allEmployees)
                {
                    VehicleEmployees.Add(new VehicleEmployee
                    {
                        VehicleId = allVehicles[i].Id,
                        EmployeeId = employee.Id
                    });
                }
            }
        }

        private void AddDefaultUsers()
        {

            var supervisorRole = Roles.SingleOrDefault(r => r.Name == "Supervisor");
            if (supervisorRole is null)
            {
                return;
            }

            var username = "admin@test.be";

            var user = new IdentityUser
            {
                AccessFailedCount = 0,
                EmailConfirmed = false,
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                Email = username,
                NormalizedEmail = username.ToUpper(),
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = "AQAAAAIAAYagAAAAENoVftXbReMcFkOwzlfLFgFkx1tZ2PVoFwKaz/7UP6r5rlrymlHMFjMgowJCZl+6YA==" //Test123$
            };

            Users.Add(user);

            var normalUsername = "normal@test.be";
            var normalUser = new IdentityUser
            {
                AccessFailedCount = 0,
                EmailConfirmed = false,
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                Email = normalUsername,
                NormalizedEmail = normalUsername.ToUpper(),
                UserName = normalUsername,
                NormalizedUserName = normalUsername.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = "AQAAAAIAAYagAAAAENoVftXbReMcFkOwzlfLFgFkx1tZ2PVoFwKaz/7UP6r5rlrymlHMFjMgowJCZl+6YA==" //Test123$
            };

            Users.Add(normalUser);
            SaveChanges();

            UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = supervisorRole.Id,
                UserId = user.Id
            });
            SaveChanges();
        }

        private void AddDefaultRoles()
        {
            Roles.Add(new IdentityRole("Supervisor"));
            SaveChanges();
        }
    }
}
