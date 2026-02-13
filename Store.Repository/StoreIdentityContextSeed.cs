using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public  class StoreIdentityContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "zeyad ahmed",
                    Email = "zeyadahmed20042020@gmail.com",
                    UserName = "zezoallord",
                    Address = new Address
                    {
                        FirstName = "zeyad",
                        LastName = "ahmed",
                        Street = "el tayar street",
                        City = "6 october",
                        State = "Giza",
                        PostalCode = "3220001"
                    }
                };
                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
