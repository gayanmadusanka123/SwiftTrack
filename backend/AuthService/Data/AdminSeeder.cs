using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Data
{
    public class AdminSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var configuration = scope.ServiceProvider
                .GetRequiredService<IConfiguration>();

            var email = configuration["AdminBootstrap:Email"];
            var password = configuration["AdminBootstrap:Password"];
            var firstName = configuration["AdminBootstrap:FirstName"];
            var lastName = configuration["AdminBootstrap:LastName"];
            var nic = configuration["AdminBootstrap:NIC"];
            var address = configuration["AdminBootstrap:Address"];

            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Admin bootstrap credentials are missing.");
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                throw new InvalidOperationException(
                    "Admin role does not exist. Run RoleSeeder first.");
            }

            var admin = await userManager.FindByEmailAsync(email);

            if(admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName ?? "admin",
                    LastName = lastName ?? "admin",
                    NIC = nic ?? "ADMIN-BOOTSTRAP",
                    Address = address ?? "Not specified",
                    IsActive = true
                };

                var createResult = await userManager.CreateAsync(admin, password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to create Admin account: {errors}");
                }
            }

            //Ensure the user has the Admin role
            if(!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var roleResult = await userManager.AddToRoleAsync(admin, "Admin");

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to assign Admin role: {errors}");
                }
            }

            Console.WriteLine($"Admin account verified: {admin.Email}");
        }
    }
}
