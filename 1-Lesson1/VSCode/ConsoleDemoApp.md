# Lesson Guide Line: VS Code + .NET CLI + SQL Server (Console CRUD)

ឯកសារនេះជាដំណាក់កាលពី `0` ដល់ `Run` សម្រាប់សិស្ស៖

1. បង្កើត Console App ដោយប្រើ .NET CLI
2. ភ្ជាប់ទៅ SQL Server
3. ធ្វើ CRUD (Create, Read, Update, Delete)

---

## 1) ត្រូវមានអ្វីខ្លះមុនចាប់ផ្តើម

សូមដំឡើងឧបករណ៍ខាងក្រោមជាមុន៖

- [.NET SDK (ណែនាំ .NET 9/10)](https://dotnet.microsoft.com/download)
- [Visual Studio Code](https://code.visualstudio.com/)
- SQL Server (Express ឬ Developer Edition)
- SQL Server Management Studio (SSMS) សម្រាប់បង្កើត Database/Table ងាយៗ

### Extensions ក្នុង VS Code (ណែនាំ)

- C# Dev Kit
- C#

---

## 2) បង្កើត Console Project ដោយ .NET CLI

បើក Terminal ក្នុង VS Code ហើយវាយ៖

```bash
mkdir StudentCrudDemo
cd StudentCrudDemo
dotnet new console -n StudentCrudApp
cd StudentCrudApp
code .
```

ពិនិត្យថា Project ដំណើរការបាន៖

```bash
dotnet run
```

បើឃើញ `Hello, World!` មានន័យថា setup ត្រឹមត្រូវ។

---

## 3) ដំឡើង Package សម្រាប់ភ្ជាប់ SQL Server

ក្នុង Terminal (នៅ folder project):

```bash
dotnet add package Microsoft.Data.SqlClient
```

Package នេះប្រើសម្រាប់ `SqlConnection`, `SqlCommand`, និងការធ្វើ query ទៅ SQL Server។

---

## 4) បង្កើត Database និង Table ក្នុង SQL Server

បើក SSMS ហើយ run SQL ខាងក្រោម៖

```sql
CREATE DATABASE SchoolDB;
GO

USE SchoolDB;
GO

CREATE TABLE Students (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	Age INT NOT NULL,
	Email NVARCHAR(120) NULL
);
GO
```

---

## 5) កំណត់ Connection String

ប្តូរ `YOUR_SERVER_NAME` ទៅ Server Name របស់អ្នក (ឧ: `DESKTOP-ABC123\\SQLEXPRESS`)។

### Option A: Windows Authentication

```csharp
string connectionString =
    "Server=YOUR_SERVER_NAME;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True;";
```

### Option B: SQL Server Authentication (User ID + Password)

```csharp
string connectionString =
    "Server=YOUR_SERVER_NAME;Database=SchoolDB;User ID=YOUR_SQL_USER;Password=YOUR_SQL_PASSWORD;TrustServerCertificate=True;";
```

ចំណាំ:

- `User ID` និង `Password` ត្រូវជាគណនី SQL Login (មិនមែន Windows Account)
- `TrustServerCertificate=True` ជួយសម្រាប់ local/dev environment

---

## 6) បង្កើត Model Class និង Service Class សម្រាប់ CRUD

ដើម្បីអោយសិស្សយល់ OOP structure កាន់តែច្បាស់ សូមបែងចែកជា 3 files:

1. `Models/Student.cs`
2. `Services/StudentService.cs`
3. `Program.cs`

### 6.1) បង្កើត Model: Student

បង្កើត file `Models/Student.cs`:

.NET CLI command:

```bash
dotnet new class -n MyClassName -o DirectoryName
dotnet new interface -n ICustomer
dotnet new enum -n Status
dotnet new struct -n Point
dotnet new record -n Person
```

```bash
dotnet new class -n Student -o Models
```

```csharp
namespace StudentCrudApp.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
}
```

### 6.2) បង្កើត Service: StudentService (CRUD)

បង្កើត file `Services/StudentService.cs`:

```csharp
using Microsoft.Data.SqlClient;
using StudentCrudApp.Models;

namespace StudentCrudApp.Services;

public class StudentService
{
    private readonly string _connectionString;

    public StudentService(string connectionString)
    {
        _connectionString = connectionString;
    }

    //Optional method to check connection
    public void CheckConnection()
    {
        using SqlConnection conn = new(_connectionString);
        try
        {
            conn.Open();
            Console.WriteLine("Connection successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }

    public int Create(Student student)
    {
        const string sql = "INSERT INTO Students (Name, Age, Email) VALUES (@Name, @Age, @Email)";

        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@Name", student.Name);
        cmd.Parameters.AddWithValue("@Age", student.Age);
        cmd.Parameters.AddWithValue("@Email", student.Email);

        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public List<Student> GetAll()
    {
        const string sql = "SELECT Id, Name, Age, Email FROM Students ORDER BY Id";
        List<Student> students = [];

        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new(sql, conn);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Age = Convert.ToInt32(reader["Age"]),
                Email = reader["Email"].ToString() ?? string.Empty
            });
        }

        return students;
    }

    public int Update(Student student)
    {
        const string sql = "UPDATE Students SET Name=@Name, Age=@Age, Email=@Email WHERE Id=@Id";

        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@Name", student.Name);
        cmd.Parameters.AddWithValue("@Age", student.Age);
        cmd.Parameters.AddWithValue("@Email", student.Email);
        cmd.Parameters.AddWithValue("@Id", student.Id);

        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public int Delete(int id)
    {
        const string sql = "DELETE FROM Students WHERE Id=@Id";

        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        conn.Open();
        return cmd.ExecuteNonQuery();
    }
}
```

### 6.3) Program.cs (Call StudentService)

ជំនួសកូដទាំងអស់ក្នុង `Program.cs` ដោយ៖

```csharp
using StudentCrudApp.Models;
using StudentCrudApp.Services;

string connectionString =
    "Server=YOUR_SERVER_NAME;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True;";

// If you use SQL Server Authentication, use this instead:
// string connectionString =
//     "Server=YOUR_SERVER_NAME;Database=SchoolDB;User ID=YOUR_SQL_USER;Password=YOUR_SQL_PASSWORD;TrustServerCertificate=True;";

StudentService studentService = new(connectionString);

while (true)
{
    Console.WriteLine("\n===== STUDENT CRUD MENU =====");
    Console.WriteLine("1. Create Student");
    Console.WriteLine("2. Read All Students");
    Console.WriteLine("3. Update Student");
    Console.WriteLine("4. Delete Student");
    Console.WriteLine("0. Exit");
    Console.Write("Choose option: ");
    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            CreateStudent(studentService);
            break;
        case "2":
            ReadStudents(studentService);
            break;
        case "3":
            UpdateStudent(studentService);
            break;
        case "4":
            DeleteStudent(studentService);
            break;
        case "0":
            Console.WriteLine("Good bye!");
            return;
        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }
}

static void CreateStudent(StudentService service)
{
    Console.Write("Name: ");
    string name = Console.ReadLine() ?? string.Empty;

    Console.Write("Age: ");
    int.TryParse(Console.ReadLine(), out int age);

    Console.Write("Email: ");
    string email = Console.ReadLine() ?? string.Empty;

    Student student = new()
    {
        Name = name,
        Age = age,
        Email = email
    };

    int rows = service.Create(student);
    Console.WriteLine($"Inserted {rows} row(s).");
}

static void ReadStudents(StudentService service)
{
    List<Student> students = service.GetAll();

    Console.WriteLine("\n----- STUDENT LIST -----");
    foreach (Student student in students)
    {
        Console.WriteLine($"Id: {student.Id}, Name: {student.Name}, Age: {student.Age}, Email: {student.Email}");
    }
}

static void UpdateStudent(StudentService service)
{
    Console.Write("Student Id to update: ");
    int.TryParse(Console.ReadLine(), out int id);

    Console.Write("New Name: ");
    string name = Console.ReadLine() ?? string.Empty;

    Console.Write("New Age: ");
    int.TryParse(Console.ReadLine(), out int age);

    Console.Write("New Email: ");
    string email = Console.ReadLine() ?? string.Empty;

    Student student = new()
    {
        Id = id,
        Name = name,
        Age = age,
        Email = email
    };

    int rows = service.Update(student);

    if (rows > 0)
        Console.WriteLine("Update successful.");
    else
        Console.WriteLine("No student found with that Id.");
}

static void DeleteStudent(StudentService service)
{
    Console.Write("Student Id to delete: ");
    int.TryParse(Console.ReadLine(), out int id);

    int rows = service.Delete(id);

    if (rows > 0)
        Console.WriteLine("Delete successful.");
    else
        Console.WriteLine("No student found with that Id.");
}
```

---

## 7) Run Application

```bash
dotnet run
```

បន្ទាប់មកសាកល្បងតាម Menu៖

1. Create (Insert new Student)
2. Read (Show all Students)
3. Update (by Id)
4. Delete (by Id)

---

## 8) បញ្ហាដែលសិស្សជួបញឹកញាប់ និងវិធីដោះស្រាយ

### Error: Cannot open server / login failed

- ពិនិត្យឈ្មោះ Server ក្នុង connection string
- ពិនិត្យថា SQL Server Service កំពុង Run
- បើប្រើ SQL Login ត្រូវប្តូរ connection string ទៅ Username/Password

### Error: A network-related or instance-specific error occurred

- ពិនិត្យថា instance name ត្រឹមត្រូវ (ឧ: `SQLEXPRESS`)
- បើម៉ាស៊ីនផ្សេង ត្រូវបើក TCP/IP និង firewall

### Error: Invalid object name 'Students'

- អាចមិនទាន់ `USE SchoolDB`
- ឬមិនទាន់បង្កើត table `Students`

---

## 9) កំណែបន្ទាប់សម្រាប់អភិវឌ្ឍបន្ថែម

បន្ទាប់ពីសិស្សយល់ CRUD មូលដ្ឋានហើយ អាចបន្ថែម៖

1. Validation (Name មិនទទេ, Age > 0)
2. Search by Name
3. Add Interface (`IStudentService`) + Dependency Injection
4. ប្រើ async/await (`ExecuteNonQueryAsync`, `ExecuteReaderAsync`)
5. បង្កើត UI ជា Windows Forms ឬ ASP.NET Core

---

## សង្ខេប

ឯកសារនេះបានបង្ហាញជាដំណាក់កាលច្បាស់ៗពី៖

- បង្កើត Console App ដោយ .NET CLI
- ភ្ជាប់ SQL Server តាម `Microsoft.Data.SqlClient`
- អនុវត្ត CRUD ពេញលេញលើ table `Students`

សិស្សអាចយកកូដនេះទៅអនុវត្តផ្ទាល់ និងបន្តអភិវឌ្ឍទៅ architecture ដែលល្អជាងមុន។
