using System;
using CloudCustomers.API.Models;

namespace CloudCustomers.UnitTests.Fixtures;

public static class UsersFixture
{
    public static List<User> GetTestUsers() => new() {
            new User
            {
                Id = 1,
                Name = "Jane",
                Address = new Address()
                {
                    Street = "123 Main Street",
                    City = "Madison",
                    ZipCode = "123"
                },
                Email = "jane@example.com"
            },
            new User
            {
                Id = 2,
                Name = "Cristian",
                Address = new Address()
                {
                    Street = "456 Main Street",
                    City = "Square",
                    ZipCode = "654"
                },
                Email = "cristian@example.com"
            },
            new User
            {
                Id = 3,
                Name = "Natali",
                Address = new Address()
                {
                    Street = "789 Main Street",
                    City = "Madison",
                    ZipCode = "987"
                },
                Email = "natali@example.com"
            }
    };
}
