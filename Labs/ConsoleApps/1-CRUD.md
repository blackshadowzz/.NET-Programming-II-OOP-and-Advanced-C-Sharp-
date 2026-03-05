# Sample CRUD Console App

នៅក្នុងមេរៀននេះ យើងនឹងរៀនពីរបៀបធ្វើ **CRUD operations** (Create, Read, Update, Delete) ទាំងអស់ដោយប្រើ **ADO.NET** ប៉ុន្តែ **មិនប្រើ Stored Procedures** ទេ។ យើងនឹងសរសេរ **SQL statements** ដោយផ្ទាល់នៅក្នុងកូដ C# (direct SQL queries)។

វិធីនេះងាយស្រួលសម្រាប់ការរៀន និងសាកល្បង ប៉ុន្តែនៅក្នុង production code គេណែនាំឱ្យប្រើ Stored Procedures ឬ parameterized queries ដើម្បីការពារ **SQL Injection**។ យើងនឹងប្រើ **parameters** ដើម្បីធ្វើឱ្យវាសុវត្ថិភាព។

---

### ជំហានទី១៖ រៀបចំ Database (SQL Server)

ប្រើតារាង **Students** ដដែលពីមេរៀនមុន៖

```sql
CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    ClassName NVARCHAR(50)
);
```

### ជំហានទី២៖ រៀបចំ C# Console Application

- បង្កើត Console App (.NET Framework ឬ .NET 8)
- បន្ថែម using statements៖

```csharp
using System;
using System.Data.SqlClient;
```

- Connection string (កែតាមរបស់អ្នក)៖

```csharp
private static string connectionString = "Server=localhost;Database=YourSchoolDB;Trusted_Connection=True;";
```

### ជំហានទី៣៖ កូដ CRUD ទាំងអស់ (Direct SQL ដោយមិនប្រើ Stored Proc)

#### 1. Create (Insert Student) – បញ្ចូលសិស្សថ្មី

```csharp
static void InsertStudent(string name, int age, string className)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();

            string sql = "INSERT INTO Students (Name, Age, ClassName) VALUES (@Name, @Age, @ClassName); " +
                         "SELECT SCOPE_IDENTITY();";  // ទទួល ID ថ្មី

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@ClassName", className);

                // ExecuteScalar() ទទួលតម្លៃតែមួយ (ID ថ្មី)
                object newIdObj = cmd.ExecuteScalar();
                int newId = Convert.ToInt32(newIdObj);

                Console.WriteLine($"បញ្ចូលជោគជ័យ! ID ថ្មី = {newId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("បញ្ហាក្នុងការបញ្ចូល: " + ex.Message);
        }
    }
}
```

---

#### 2. Read (Get All Students) – បង្ហាញសិស្សទាំងអស់

```csharp
static void GetAllStudents()
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();

            string sql = "SELECT Id, Name, Age, ClassName FROM Students";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
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
        catch (Exception ex)
        {
            Console.WriteLine("បញ្ហាក្នុងការអាន: " + ex.Message);
        }
    }
}
```

---

#### 3. Read (Get Student By Id) – ស្វែងរកសិស្សតាម ID

```csharp
static void GetStudentById(int id)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();

            string sql = "SELECT Id, Name, Age, ClassName FROM Students WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
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
        catch (Exception ex)
        {
            Console.WriteLine("បញ្ហាក្នុងការស្វែងរក: " + ex.Message);
        }
    }
}
```

---

#### 4. Update Student – កែប្រែព័ត៌មានសិស្ស

```csharp
static void UpdateStudent(int id, string name, int age, string className)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();

            string sql = "UPDATE Students SET Name = @Name, Age = @Age, ClassName = @ClassName WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@ClassName", className);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Console.WriteLine($"កែប្រែជោគជ័យ! ប៉ះពាល់ {rowsAffected} ជួរ");
                else
                    Console.WriteLine("មិនមានសិស្សត្រូវកែ (ID មិនត្រឹមត្រូវ)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("បញ្ហាក្នុងការកែ: " + ex.Message);
        }
    }
}
```

---

#### 5. Delete Student – លុបសិស្ស

```csharp
static void DeleteStudent(int id)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();

            string sql = "DELETE FROM Students WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Console.WriteLine($"លុបជោគជ័យ! លុប {rowsAffected} ជួរ");
                else
                    Console.WriteLine("មិនមានសិស្សត្រូវលុប (ID មិនត្រឹមត្រូវ)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("បញ្ហាក្នុងការលុប: " + ex.Message);
        }
    }
}
```

---

### ជំហានទី៤៖ សាកល្បងនៅក្នុង Main()

```csharp
static void Main(string[] args)
{
    // សាកល្បង CRUD ដោយផ្ទាល់ SQL
    InsertStudent("សុខា", 20, "ថ្នាក់ទី១២");
    InsertStudent("វណ្ណា", 19, "ថ្នាក់ទី១១");

    GetAllStudents();

    GetStudentById(1);

    UpdateStudent(1, "សុខា សុភា", 21, "ថ្នាក់ទី១២ A");

    DeleteStudent(2);

    GetAllStudents();

    Console.WriteLine("\nចុចគ្រាប់ណាមួយដើម្បីចាកចេញ...");
    Console.ReadKey();
}
```

### គន្លឹះសំខាន់ៗ

- **Parameterized queries** (`@Name`, `@Id` ជាដើម) → ការពារ **SQL Injection** ទោះបីមិនប្រើ Stored Proc ក៏ដោយ។ កុំ concatenate string ដូចជា `"WHERE Id = " + id` ដែលគ្រោះថ្នាក់!
- ប្រើ `ExecuteNonQuery()` សម្រាប់ INSERT/UPDATE/DELETE (ត្រឡប់ចំនួនជួរដែលប៉ះពាល់)
- ប្រើ `ExecuteReader()` សម្រាប់ SELECT ច្រើនជួរ
- ប្រើ `ExecuteScalar()` សម្រាប់ SELECT តែមួយតម្លៃ (ដូចជា ID ថ្មីពី `SCOPE_IDENTITY()`)
- តែងតែប្រើ `using` ដើម្បីបិទ connection/command ដោយស្វ័យប្រវត្តិ
- នៅក្នុង **OOP** (ពីសៀវភៅ Angkor University ដែលអ្នកកំពុងរៀន) → អ្នកអាចបង្កើត class `StudentRepository` ដើម្បីដាក់ methods ទាំងនេះ ដើម្បី encapsulate data access (ធ្វើឱ្យកូដស្អាត និងងាយថែទាំ)
