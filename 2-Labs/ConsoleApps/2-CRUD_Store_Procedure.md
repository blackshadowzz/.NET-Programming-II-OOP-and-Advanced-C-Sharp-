# Sample CRUD using Store Procedure

នៅក្នុងមេរៀននេះ យើងនឹងរៀនពីរបៀបធ្វើ **CRUD operations** (Create, Read, Update, Delete) ទាំងអស់ដោយប្រើ **ADO.NET** និង **Stored Procedures** ទាំងអស់។ យើងនឹងប្រើ **Console Application** សាមញ្ញ (ដើម្បីងាយស្រួលរៀន) ប៉ុន្តែគោលការណ៍ដដែលអាចប្រើក្នុង Windows Forms, ASP.NET MVC, ឬ .NET Core

---

### ជំហានទី១៖ រៀបចំ Database (SQL Server)

សន្មត់ថាយើងមានតារាង **Students** សាមញ្ញ៖

```sql
CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    ClassName NVARCHAR(50)
);
```

បន្ទាប់មក បង្កើត **Stored Procedures** សម្រាប់ CRUD ទាំង៤ (នៅ SQL Server Management Studio ឬ Visual Studio)៖

1. **Create (Insert)**

```sql
CREATE PROCEDURE spInsertStudent
    @Name NVARCHAR(100),
    @Age INT,
    @ClassName NVARCHAR(50),
    @NewId INT OUTPUT
AS
BEGIN
    INSERT INTO Students (Name, Age, ClassName)
    VALUES (@Name, @Age, @ClassName);

    SET @NewId = SCOPE_IDENTITY();
END
```

---

2. **Read (Select All & Select By Id)**

```sql
CREATE PROCEDURE spGetAllStudents
AS
BEGIN
    SELECT Id, Name, Age, ClassName FROM Students;
END

CREATE PROCEDURE spGetStudentById
    @Id INT
AS
BEGIN
    SELECT Id, Name, Age, ClassName FROM Students WHERE Id = @Id;
END
```

---

3. **Update**

```sql
CREATE PROCEDURE spUpdateStudent
    @Id INT,
    @Name NVARCHAR(100),
    @Age INT,
    @ClassName NVARCHAR(50)
AS
BEGIN
    UPDATE Students
    SET Name = @Name, Age = @Age, ClassName = @ClassName
    WHERE Id = @Id;
END
```

---

4. **Delete**

```sql
CREATE PROCEDURE spDeleteStudent
    @Id INT
AS
BEGIN
    DELETE FROM Students WHERE Id = @Id;
END
```

---

### ជំហានទី២៖ រៀបចំ C# Project (Console App)

- បើក **Visual Studio** → Create new project → Console App
- បន្ថែម **using** សំខាន់ៗ៖

```csharp
using System;
using System.Data;
using System.Data.SqlClient;
```

- កំណត់ **connection string** (កែតាមរបស់អ្នក)៖

```csharp
private static string connectionString = "Server=localhost;Database=YourSchoolDB;Trusted_Connection=True;";
```

### ជំហានទី៣៖ កូដ CRUD ទាំងអស់ (ជាមួយ Stored Procedures)

#### 1. Create (Insert Student)

```csharp
static void InsertStudent(string name, int age, string className)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        using (SqlCommand cmd = new SqlCommand("spInsertStudent", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@ClassName", className);

            // Output parameter ដើម្បីទទួល ID ថ្មី
            SqlParameter outParam = new SqlParameter("@NewId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            cmd.ExecuteNonQuery();

            int newId = (int)outParam.Value;
            Console.WriteLine($"បញ្ចូលជោគជ័យ! ID ថ្មី = {newId}");
        }
    }
}
```

---

#### 2. Read (Get All Students)

```csharp
static void GetAllStudents()
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        using (SqlCommand cmd = new SqlCommand("spGetAllStudents", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\nបញ្ជីសិស្សទាំងអស់៖");
                Console.WriteLine("ID\tឈ្មោះ\tអាយុ\tថ្នាក់");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]}\t{reader["Name"]}\t{reader["Age"]}\t{reader["ClassName"]}");
                }
            }
        }
    }
}
```

---

#### 3. Read (Get Student By Id)

```csharp
static void GetStudentById(int id)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        using (SqlCommand cmd = new SqlCommand("spGetStudentById", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"\nព័ត៌មានសិស្ស ID {id}៖");
                    Console.WriteLine($"ឈ្មោះ៖ {reader["Name"]}");
                    Console.WriteLine($"អាយុ៖ {reader["Age"]}");
                    Console.WriteLine($"ថ្នាក់៖ {reader["ClassName"]}");
                }
                else
                {
                    Console.WriteLine($"រកមិនឃើញសិស្ស ID {id}");
                }
            }
        }
    }
}
```

---

#### 4. Update Student

```csharp
static void UpdateStudent(int id, string name, int age, string className)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        using (SqlCommand cmd = new SqlCommand("spUpdateStudent", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@ClassName", className);

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
                Console.WriteLine($"កែប្រែជោគជ័យ! ប៉ះពាល់ {rows} ជួរ");
            else
                Console.WriteLine("មិនមានសិស្សត្រូវកែ");
        }
    }
}
```

---

#### 5. Delete Student

```csharp
static void DeleteStudent(int id)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        using (SqlCommand cmd = new SqlCommand("spDeleteStudent", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
                Console.WriteLine($"លុបជោគជ័យ! លុប {rows} ជួរ");
            else
                Console.WriteLine("មិនមានសិស្សត្រូវលុប");
        }
    }
}
```

---

### ជំហានទី៤៖ សាកល្បងនៅក្នុង Main()

```csharp
static void Main(string[] args)
{
    // សាកល្បង CRUD
    InsertStudent("សុខា", 20, "ថ្នាក់ទី១២");
    InsertStudent("វណ្ណា", 19, "ថ្នាក់ទី១១");

    GetAllStudents();

    GetStudentById(1);

    UpdateStudent(1, "សុខា សុភា", 21, "ថ្នាក់ទី១២ A");

    DeleteStudent(2);

    GetAllStudents();

    Console.ReadKey();
}
```

### គន្លឹះសំខាន់ៗ

- តែងតែប្រើ `using` សម្រាប់ **SqlConnection**, **SqlCommand**, **SqlDataReader** → បិទដោយស្វ័យប្រវត្តិ សន្សំធនធាន
- ប្រើ **Parameters** ជានិច្ច → ការពារ **SQL Injection**
- **CommandType = CommandType.StoredProcedure** → សំខាន់បំផុតពេលហៅ Stored Proc
- បើមាន **Output parameter** → ប្រើ `ParameterDirection.Output` និងអាន `.Value` បន្ទាប់ពី Execute
- ពិនិត្យ `ExecuteNonQuery()` return value → ដើម្បីដឹងថាប៉ះពាល់ជួរប៉ុន្មាន
- នៅក្នុង **OOP** (ពីមេរៀន Angkor University) → អ្នកអាចបង្កើត class ដូចជា `StudentRepository` ដើម្បីដាក់ methods CRUD ទាំងនេះ ធ្វើឱ្យកូដស្អាត និង reusable (Encapsulation)
