using Bogus;
using StudentManagement.WebApi.Data.Entities;
using System;

namespace StudentManagement.WebApi.Data
{
    public class DataSeeder
    {
        private static readonly string[] classList = new[] { "9-A", "9-B", "10-A", "10-C", "11-B", "12-A" };

        public static async Task SeedAsync(StudentManagementDbContext dbContext)
        {
            var createdStudents = new Faker<StudentEntity>("tr")
                .Ignore(s => s.Id)
                .RuleFor(s => s.No, f => f.IndexFaker + 1)
                 .RuleFor(s => s.Name, f => f.Name.FirstName())
                 .RuleFor(s => s.Surname, f => f.Name.LastName())
                  .RuleFor(s => s.Class, f => f.PickRandom(classList));

            List<StudentEntity> fakeStudents = createdStudents.Generate(20);

            await dbContext.Students.AddRangeAsync(fakeStudents);
            await dbContext.SaveChangesAsync();

        }
    }
}
