# មេរៀនអំពី ADO.NET + SQL Server

ADO.NET គឺជាផ្នែកមួយដ៏សំខាន់នៃ **.NET Framework** (និងបន្តទៅក្នុង **.NET** Core ) ដែលត្រូវបានបង្កើតឡើងដោយ Microsoft ដើម្បីអនុញ្ញាតឱ្យកម្មវិធី C# (ឬ VB.NET) ភ្ជាប់ទៅកាន់ **database** ផ្សេងៗ ដូចជា SQL Server, Oracle, MySQL ឬសូម្បីតែ XML files ដើម្បីអាន បញ្ចូល កែប្រែ និងលុបទិន្នន័យ។

វាជាបច្ចេកវិទ្យាដែលផ្តល់នូវ **ការចូលប្រើទិន្នន័យ** (data access) ដែលមានស្ថេរភាព និងអាចប្រើបានទូទាំងប្រភេទ data source ផ្សេងៗ។

---

## គោលបំណងសំខាន់ៗរបស់ ADO.NET

- ភ្ជាប់ទៅ database (connected mode)
- អនុវត្តពាក្យបញ្ជា SQL (SELECT, INSERT, UPDATE, DELETE)
- ទាញទិន្នន័យមកបង្ហាញ ឬគ្រប់គ្រងក្នុងកម្មវិធី (ដូចជា DataGridView ក្នុង Windows Forms)
- គាំទ្រ **disconnected mode** (ប្រើ DataSet ដើម្បីរក្សាទិន្នន័យក្នុងកម្មវិធីដោយមិនចាំបាច់ភ្ជាប់ជាប់ជានិច្ច)

## សមាសធាតុសំខាន់ៗ (Key Components) របស់ ADO.NET

1. **Connection** → ប្រើដើម្បីបើកភ្ជាប់ទៅ database (ដូចជា SqlConnection)
2. **Command** → ប្រើសម្រាប់រក្សាពាក្យបញ្ជា SQL (ដូចជា SqlCommand)
3. **DataReader** → អានទិន្នន័យពី database តាមលំដាប់ (forward-only, read-only) → លឿនបំផុតសម្រាប់អានច្រើន
4. **DataAdapter** → ប្រើសម្រាប់បំពេញទិន្នន័យទៅក្នុង DataSet/DataTable និងធ្វើបច្ចុប្បន្នភាព database (Fill, Update)
5. **DataSet / DataTable** → រក្សាទិន្នន័យក្នុងកម្មវិធីដោយឯករាជ្យពី database (disconnected)

---

### ឧទាហរណ៍សាមញ្ញ៖ ភ្ជាប់ទៅ SQL Server (Console Application)

ដំបូង ត្រូវបន្ថែម **using** statements ទាំងនេះ៖

```csharp
using System;
using System.Data.SqlClient;
```

បន្ទាប់មក កូដគំរូ (ក្នុង Main method)៖

```csharp
class Program
{
    static void Main(string[] args)
    {
        // ខ្សែភ្ជាប់ទៅ SQL Server (កែ Connection String តាមរបស់អ្នក)
        string connectionString = "Server=localhost;Database=YourDatabaseName;Trusted_Connection=True;";

        // បើក Connection
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("ភ្ជាប់ទៅ database ជោគជ័យ!");

                // បង្កើត Command ដើម្បីសួរទិន្នន័យ
                string query = "SELECT CustomerName FROM Customers"; // សន្មត់ថាមានតារាង Customers
                SqlCommand command = new SqlCommand(query, connection);

                // ប្រើ DataReader ដើម្បីអានទិន្នន័យ
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("\nបញ្ជីឈ្មោះអតិថិជន៖");
                while (reader.Read())
                {
                    Console.WriteLine(reader["CustomerName"].ToString());
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("មានបញ្ហា៖ " + ex.Message);
            }
        }

        Console.WriteLine("\nចុចគ្រាប់ណាមួយដើម្បីចាកចេញ...");
        Console.ReadKey();
    }
}
```

### ពន្យល់ជាជំហានៗ

1. **Connection String** → នេះគឺជាព័ត៌មានសំខាន់ដែលប្រាប់កម្មវិធីថាត្រូវភ្ជាប់ទៅ database ណា (Server, Database, User, Password)។ ប្រើ **Trusted_Connection=True** បើប្រើ Windows Authentication។
2. **using (SqlConnection ...)** → ធានាថា connection ត្រូវបិទដោយស្វ័យប្រវត្តិ ដើម្បីសន្សំធនធាន។
3. **connection.Open()** → បើកការភ្ជាប់។
4. **SqlCommand** → រក្សាពាក្យ SQL និង connection។
5. **ExecuteReader()** → សម្រាប់ SELECT (អានទិន្នន័យ)។ ប្រើ ExecuteNonQuery() សម្រាប់ INSERT/UPDATE/DELETE។
6. **reader.Read()** → អានជួរនីមួយៗ។

---

### ឧទាហរណ៍បន្ថែម៖ បញ្ចូលទិន្នន័យថ្មី (INSERT)

```csharp
string insertQuery = "INSERT INTO Customers (CustomerName, Age) VALUES (@Name, @Age)";
SqlCommand cmd = new SqlCommand(insertQuery, connection);

cmd.Parameters.AddWithValue("@Name", "សុខា");
cmd.Parameters.AddWithValue("@Age", 25);

int rowsAffected = cmd.ExecuteNonQuery();
Console.WriteLine("បញ្ចូលជោគជ័យ " + rowsAffected + " ជួរ!");
```

ប្រើ **Parameters** ដើម្បីការពារ **SQL Injection** (សុវត្ថិភាព)។

### ការប្រៀបធៀបសង្ខេប

- **ADO.NET** → គ្រប់គ្រងដោយដៃ លឿន គ្រប់គ្រងបានច្រើន (ស័ក្តិសមសម្រាប់កម្មវិធីតូច ឬ performance ខ្ពស់)
- **Entity Framework** (EF Core) → ងាយស្រួលជាង ប្រើ LINQ មិនចាំបាច់សរសេរ SQL ច្រើន (ស័ក្តិសមសម្រាប់កម្មវិធីធំ)

---

## Key Components

សមាសធាតុសំខាន់ៗ (Key Components) របស់ **ADO.NET** គឺជាផ្នែកស្នូលដែលធ្វើឱ្យវាអាចភ្ជាប់ អាន កែប្រែ និងគ្រប់គ្រងទិន្នន័យពី database បានយ៉ាងមានប្រសិទ្ធភាព។

#### ADO.NET មាន **ពីរផ្នែកធំ**៖

- **.NET Data Provider** (ផ្នែកភ្ជាប់ជាមួយ database ជាក់លាក់ ដូចជា SQL Server)
- **DataSet** (ផ្នែក disconnected ដែលរក្សាទិន្នន័យក្នុងកម្មវិធីដោយឯករាជ្យ)

ខាងក្រោមនេះគឺជាសមាសធាតុសំខាន់ៗទាំងអស់ ដែលត្រូវបានបង្រៀនជាជំហានៗ រួមទាំងការពន្យល់ មុខងារ និងឧទាហរណ៍សាមញ្ញ។

---

### ១. Connection (ការភ្ជាប់)

- **ឈ្មោះ class សំខាន់**៖ `SqlConnection` (សម្រាប់ SQL Server), `OleDbConnection`, `OracleConnection` ។ល។
- **មុខងារ**៖ បង្កើត និងគ្រប់គ្រងការភ្ជាប់ទៅ database។ វាគ្រប់គ្រង **connection pooling** (សន្សំការភ្ជាប់ដើម្បីប្រើឡើងវិញ សន្សំល្បឿន)។
- **គន្លឹះ**៖ តែងតែប្រើ `using` statement ដើម្បីបិទ connection ដោយស្វ័យប្រវត្តិ។
- **ឧទាហរណ៍**៖
  ```csharp
  using (SqlConnection con = new SqlConnection("Server=localhost;Database=MyDB;Trusted_Connection=True;"))
  {
      con.Open();  // បើកការភ្ជាប់
      // ធ្វើការជាមួយ database
  }  // បិទដោយស្វ័យប្រវត្តិ
  ```

---

### ២. Command (ពាក្យបញ្ជា)

- **ឈ្មោះ class សំខាន់**៖ `SqlCommand`
- **មុខងារ**៖ រក្សា និងអនុវត្តពាក្យ SQL (SELECT, INSERT, UPDATE, DELETE) ឬ stored procedure។ អាចប្រើ **parameters** ដើម្បីការពារ SQL Injection។
- **វិធីសំខាន់**៖
  - `ExecuteReader()` → សម្រាប់ SELECT (ត្រឡប់ DataReader)
  - `ExecuteNonQuery()` → សម្រាប់ INSERT/UPDATE/DELETE (ត្រឡប់ចំនួនជួរដែលប៉ះពាល់)
  - `ExecuteScalar()` → សម្រាប់តម្លៃតែមួយ (ដូចជា COUNT)
- **ឧទាហរណ៍**៖
  ```csharp
  SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE Id = @Id", con);
  cmd.Parameters.AddWithValue("@Id", 5);
  ```

---

### ៣. DataReader (អ្នកអានទិន្នន័យ)

- **ឈ្មោះ class សំខាន់**៖ `SqlDataReader`
- **មុខងារ**៖ អានទិន្នន័យពី database តាមលំដាប់ (forward-only, read-only) លឿនបំផុត ស័ក្តិសមសម្រាប់អានទិន្នន័យច្រើន មិនកែប្រែ។
- **គន្លឹះ**៖ មិនអាចត្រឡប់ក្រោយ ឬកែប្រែទិន្នន័យបានទេ។ ត្រូវបិទវាបន្ទាប់ពីប្រើ។
- **ឧទាហរណ៍**៖
  ```csharp
  SqlDataReader reader = cmd.ExecuteReader();
  while (reader.Read())
  {
      Console.WriteLine(reader["CustomerName"]);
  }
  reader.Close();
  ```

---

### ៤. DataAdapter (អ្នកសម្របទិន្នន័យ)

- **ឈ្មោះ class សំខាន់**៖ `SqlDataAdapter`
- **មុខងារ**៖ ជា "ស្ពាន" រវាង database និង DataSet/DataTable។ ប្រើដើម្បី៖
  - បំពេញទិន្នន័យទៅ DataSet (`Fill`)
  - ធ្វើបច្ចុប្បន្នភាព database ពីការផ្លាស់ប្តូរក្នុង DataSet (`Update`)
- **គន្លឹះ**៖ វាប្រើ Command ខាងក្នុង (SelectCommand, InsertCommand, UpdateCommand, DeleteCommand)។
- **ឧទាហរណ៍**៖
  ```csharp
  SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Customers", con);
  DataSet ds = new DataSet();
  adapter.Fill(ds, "Customers");  // បំពេញទិន្នន័យ
  ```

### ៥. DataSet (សំណុំទិន្នន័យ)

- **ឈ្មោះ class**៖ `DataSet` (នៅក្នុង namespace `System.Data`)
- **មុខងារ**៖ រក្សាទិន្នន័យក្នុងកម្មវិធីដោយឯករាជ្យពី database (disconnected mode)។ វាអាចមានច្រើន **DataTable**, ទំនាក់ទំនង (relations), constraints, primary key ជាដើម។ អាចប្រើជាមួយ XML ឬទិន្នន័យពីប្រភពផ្សេងៗ។
- **គន្លឹះ**៖ មិនចាំបាច់ភ្ជាប់ជាប់ database ទេ ស័ក្តិសមសម្រាប់កម្មវិធីដែលត្រូវការគ្រប់គ្រងទិន្នន័យច្រើន ឬ offline។
- **ឧទាហរណ៍**៖
  ```csharp
  DataTable dt = ds.Tables["Customers"];
  foreach (DataRow row in dt.Rows)
  {
      Console.WriteLine(row["CustomerName"]);
  }
  ```

### សង្ខេបតារាងប្រៀបធៀប

| សមាសធាតុ    | Mode         | ល្បឿន    | អាចកែប្រែទិន្នន័យ? | ស័ក្តិសមសម្រាប់                        |
| ----------- | ------------ | -------- | ------------------ | -------------------------------------- |
| Connection  | Connected    | -        | -                  | គ្រប់គ្រងការភ្ជាប់                     |
| Command     | Connected    | លឿន      | បាទ (via Execute)  | អនុវត្ត SQL                            |
| DataReader  | Connected    | លឿនបំផុត | ទេ (read-only)     | អានទិន្នន័យច្រើន លឿន                   |
| DataAdapter | Bridge       | មធ្យម    | បាទ (Fill/Update)  | ភ្ជាប់ DataSet និង DB                  |
| DataSet     | Disconnected | យឺតជាង   | បាទ                | គ្រប់គ្រងទិន្នន័យ offline, ច្រើន table |
