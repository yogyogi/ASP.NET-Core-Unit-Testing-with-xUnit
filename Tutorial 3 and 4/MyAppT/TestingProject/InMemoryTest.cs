using Microsoft.EntityFrameworkCore;
using MyAppT.Models;

namespace TestingProject
{
    public class InMemoryTest : TestRegistration
    {
        public InMemoryTest()
            : base(
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("TestDatabase")
                    .Options)
        {
        }
    }
}
