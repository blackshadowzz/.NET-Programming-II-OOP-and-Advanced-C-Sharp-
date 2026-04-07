using StudentCrudApp.Models;
using StudentCrudApp.Services;

//Windows Authentication
// string connectionString = @"Server=BLACKK;Database=SchoolDBTest;
//    Trusted_Connection=True;TrustServerCertificate=True;";

//SQL Server Authentication
string connectionString = @"Server=.;Database=SchoolDBTest;
    User Id=mango;Password=1234;TrustServerCertificate=True;";


StudentService studentService = new(connectionString);

List<Student> students = studentService.GetAll();
foreach (Student student in students)
{
    Console.WriteLine($"{student.Id}: {student.Name} ({student.Age}) - {student.Email}");
}


Console.ReadLine();