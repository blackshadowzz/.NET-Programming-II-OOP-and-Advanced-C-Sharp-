# Windows Forms + Dapper + SQL Server (CRUD)

ឯកសារនេះសម្រាប់សិស្សរៀនពីរបៀបបង្កើត Windows Forms Application ដោយប្រើ Dapper និង SQL Server ដើម្បីធ្វើ CRUD (Create, Read, Update, Delete)។

ចំណាំ: Technical terms និង keywords ត្រូវរក្សាទុកជា English ដើម្បីឲ្យសិស្សស្គាល់ពាក្យពិតក្នុង programming។

---

## 1) ត្រូវមានអ្វីខ្លះមុនចាប់ផ្តើម

សូមដំឡើងឧបករណ៍ខាងក្រោមជាមុន៖

- .NET SDK
- Visual Studio 2022 (Workload: .NET Desktop Development)
- SQL Server (Express ឬ Developer Edition)
- SQL Server Management Studio (SSMS)

### Package ដែលត្រូវប្រើ

- Dapper
- Microsoft.Data.SqlClient

---

## 2) បង្កើត Windows Forms Project ក្នុង Visual Studio

បើក Visual Studio ហើយធ្វើតាមជំហាន៖

1. Click `Create a new project`
2. ជ្រើស `Windows Forms App` (C#)
3. Click `Next`
4. Project name: `StudentCrudWinForms`
5. Location: ជ្រើស folder ដែលចង់រក្សាទុក
6. Framework: ជ្រើស `.NET 8` ឬ `.NET 9`
7. Click `Create`

បន្ទាប់ពី create រួច អាចចុច `F5` ដើម្បី Run សាកល្បង project ដំបូង។

---

## 3) ដំឡើង Package (NuGet) ក្នុង Visual Studio

ជំហានដំឡើង package៖

1. Right-click លើ project `StudentCrudWinForms`
2. ជ្រើស `Manage NuGet Packages...`
3. Tab `Browse` ស្វែងរក `Dapper` ហើយចុច `Install`
4. ស្វែងរក `Microsoft.Data.SqlClient` ហើយចុច `Install`

Dapper ជួយឲ្យ query ទៅ SQL Server បានងាយ និងលឿន។

---

## 4) បង្កើត Database និង Table

បើក SSMS ហើយ run SQL ខាងក្រោម៖

```sql
CREATE DATABASE SchoolDB;
GO

USE SchoolDB;
GO

CREATE TABLE Students
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	Age INT NOT NULL,
	Email NVARCHAR(120) NULL
);
GO
```

---

## 5) កំណត់ Connection String

ប្តូរ `YOUR_SERVER_NAME` ទៅ server name របស់អ្នក (ឧ: `DESKTOP-ABC123\\SQLEXPRESS`)។

### Option A: Windows Authentication

```csharp
string connectionString =
	"Server=YOUR_SERVER_NAME;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True;";
```

### Option B: SQL Server Authentication

```csharp
string connectionString =
	"Server=YOUR_SERVER_NAME;Database=SchoolDB;User ID=YOUR_SQL_USER;Password=YOUR_SQL_PASSWORD;TrustServerCertificate=True;";
```

---

## 6) Project Structure

សម្រាប់ students ឲ្យងាយយល់ សូមបែងចែកជា folders ដូចនេះ៖

```text
StudentCrudWinForms/
├─ Models/
│  └─ Student.cs
├─ Services/
│  ├─ IStudentService.cs
│  └─ StudentService.cs
├─ Infrastructure/
│  ├─ IDbConnectionService.cs
│  └─ SqlConnectionService.cs
├─ Forms/
│  └─ StudentForm.cs
└─ Program.cs
```

---

## 7) បង្កើត Model Class

បង្កើត file `Models/Student.cs`៖

```csharp
namespace StudentCrudWinForms.Models;

public class Student
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int Age { get; set; }
	public string Email { get; set; } = string.Empty;
}
```

Class នេះប្រើសម្រាប់ផ្ទុក data របស់ Student ម្នាក់។

---

## 8) បង្កើត Connection Service

Connection Service ជួយយើងគ្រប់គ្រង connection string និងបង្កើត connection object ម្តងមួយកន្លែង។

### 8.1) Interface

បង្កើត file `Infrastructure/IDbConnectionService.cs`៖

```csharp
using System.Data;

namespace StudentCrudWinForms.Infrastructure;

public interface IDbConnectionService
{
	IDbConnection CreateConnection();
}
```

### 8.2) Implementation

បង្កើត file `Infrastructure/SqlConnectionService.cs`៖

```csharp
using System.Data;
using Microsoft.Data.SqlClient;

namespace StudentCrudWinForms.Infrastructure;

public class SqlConnectionService : IDbConnectionService
{
	private readonly string _connectionString;

	public SqlConnectionService(string connectionString)
	{
		_connectionString = connectionString;
	}

	public IDbConnection CreateConnection()
	{
		return new SqlConnection(_connectionString);
	}
}
```

---

## 9) បង្កើត Service សម្រាប់ CRUD

### 9.1) Interface

បង្កើត file `Services/IStudentService.cs`៖

```csharp
using StudentCrudWinForms.Models;

namespace StudentCrudWinForms.Services;

public interface IStudentService
{
	List<Student> GetAll();
	Student? GetById(int id);
	int Create(Student student);
	int Update(Student student);
	int Delete(int id);
}
```

### 9.2) StudentService

បង្កើត file `Services/StudentService.cs`៖

```csharp
using Dapper;
using StudentCrudWinForms.Infrastructure;
using StudentCrudWinForms.Models;

namespace StudentCrudWinForms.Services;

public class StudentService : IStudentService
{
	private readonly IDbConnectionService _connectionService;

	public StudentService(IDbConnectionService connectionService)
	{
		_connectionService = connectionService;
	}

	public List<Student> GetAll()
	{
		using var connection = _connectionService.CreateConnection();
		const string sql = "SELECT Id, Name, Age, Email FROM Students ORDER BY Id";
		return connection.Query<Student>(sql).ToList();
	}

	public Student? GetById(int id)
	{
		using var connection = _connectionService.CreateConnection();
		const string sql = "SELECT Id, Name, Age, Email FROM Students WHERE Id = @Id";
		return connection.QueryFirstOrDefault<Student>(sql, new { Id = id });
	}

	public int Create(Student student)
	{
		using var connection = _connectionService.CreateConnection();
		const string sql = @"
			INSERT INTO Students (Name, Age, Email)
			VALUES (@Name, @Age, @Email)";

		return connection.Execute(sql, student);
	}

	public int Update(Student student)
	{
		using var connection = _connectionService.CreateConnection();
		const string sql = @"
			UPDATE Students
			SET Name = @Name,
				Age = @Age,
				Email = @Email
			WHERE Id = @Id";

		return connection.Execute(sql, student);
	}

	public int Delete(int id)
	{
		using var connection = _connectionService.CreateConnection();
		const string sql = "DELETE FROM Students WHERE Id = @Id";
		return connection.Execute(sql, new { Id = id });
	}
}
```

Dapper នឹង map column names ទៅ property names ដោយស្វ័យប្រវត្តិ។

---

## 10) បង្កើត Form សម្រាប់ CRUD

សម្រាប់ students ងាយយល់ គួរមាន controls សំខាន់ៗដូចជា៖

- TextBox សម្រាប់ Name
- TextBox សម្រាប់ Age
- TextBox សម្រាប់ Email
- TextBox សម្រាប់ Id
- Button សម្រាប់ Create, Update, Delete, Load
- DataGridView សម្រាប់បង្ហាញ data

### ជំហាន Design Form ក្នុង Visual Studio Designer

1. បើក `Form1.cs [Design]`
2. ទាញ controls ពី `Toolbox` ដាក់លើ Form
3. កែ `(Name)` property តាមឈ្មោះខាងក្រោម
4. Double-click លើ Button នីមួយៗ ដើម្បី generate `Click` event
5. ចុចលើ `DataGridView` ហើយបង្កើត `CellClick` event
6. ចុចលើ Form background ហើយបង្កើត `Load` event

### ឧទាហរណ៍ឈ្មោះ controls

- `txtId`
- `txtName`
- `txtAge`
- `txtEmail`
- `btnCreate`
- `btnUpdate`
- `btnDelete`
- `btnLoad`
- `dgvStudents`

---

## 11) StudentForm Code

សម្រាប់ Visual Studio អ្នកអាចប្រើ `Form1.cs` តែម្តង (ឬ rename ទៅ `StudentForm.cs`) ហើយដាក់ code ខាងក្រោម៖

```csharp
using StudentCrudWinForms.Infrastructure;
using StudentCrudWinForms.Models;
using StudentCrudWinForms.Services;

namespace StudentCrudWinForms.Forms;

public partial class StudentForm : Form
{
	private readonly IStudentService _studentService;

	public StudentForm(IStudentService studentService)
	{
		InitializeComponent();
		_studentService = studentService;
	}

	private void StudentForm_Load(object sender, EventArgs e)
	{
		LoadStudents();
	}

	private void LoadStudents()
	{
		dgvStudents.DataSource = _studentService.GetAll();
	}

	private void btnCreate_Click(object sender, EventArgs e)
	{
		Student student = new()
		{
			Name = txtName.Text,
			Age = int.Parse(txtAge.Text),
			Email = txtEmail.Text
		};

		_studentService.Create(student);
		LoadStudents();
		ClearFields();
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		Student student = new()
		{
			Id = int.Parse(txtId.Text),
			Name = txtName.Text,
			Age = int.Parse(txtAge.Text),
			Email = txtEmail.Text
		};

		_studentService.Update(student);
		LoadStudents();
		ClearFields();
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		int id = int.Parse(txtId.Text);
		_studentService.Delete(id);
		LoadStudents();
		ClearFields();
	}

	private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex < 0) return;

		DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
		txtId.Text = row.Cells["Id"].Value?.ToString();
		txtName.Text = row.Cells["Name"].Value?.ToString();
		txtAge.Text = row.Cells["Age"].Value?.ToString();
		txtEmail.Text = row.Cells["Email"].Value?.ToString();
	}

	private void ClearFields()
	{
		txtId.Clear();
		txtName.Clear();
		txtAge.Clear();
		txtEmail.Clear();
	}
}
```

---

## 12) Program.cs

កំណត់ startup form និង inject services៖

```csharp
using StudentCrudWinForms.Forms;
using StudentCrudWinForms.Infrastructure;
using StudentCrudWinForms.Services;

namespace StudentCrudWinForms;

internal static class Program
{
	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize();

		string connectionString =
			"Server=YOUR_SERVER_NAME;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True;";

		IDbConnectionService connectionService = new SqlConnectionService(connectionString);
		IStudentService studentService = new StudentService(connectionService);

		Application.Run(new StudentForm(studentService));
	}
}
```

Run project ក្នុង Visual Studio៖

1. ចុច `Set as Startup Project` លើ `StudentCrudWinForms` (បើមានច្រើន projects)
2. ចុច `F5` (Debug) ឬ `Ctrl + F5` (Run without Debug)

---

## 13) លំដាប់ការរៀនសម្រាប់សិស្ស

1. សិក្សា model class មុន
2. សិក្សា connection service បន្ទាប់
3. សិក្សា CRUD service បន្ទាប់
4. ចុងក្រោយភ្ជាប់ service ទៅ form

វិធីនេះជួយឲ្យសិស្សយល់ថា UI, database logic, និង connection logic ត្រូវបែងចែកដាច់ពីគ្នា។

---

## 14) បញ្ហាដែលជួបញឹកញាប់

### Error: Cannot open server

- ពិនិត្យ server name
- ពិនិត្យ SQL Server service
- ពិនិត្យ authentication mode

### Error: Invalid object name 'Students'

- ពិនិត្យថា database ត្រឹមត្រូវ
- ពិនិត្យថា table `Students` មានពិត

### Error: Object reference not set to an instance of an object

- ពិនិត្យ `TextBox` name
- ពិនិត្យ event handler connection

---

## 15) សង្ខេប

មេរៀននេះបង្ហាញពី៖

- បង្កើត Windows Forms project
- ប្រើ Dapper ជាមួយ SQL Server
- បង្កើត `Student` class
- បង្កើត `Connection Service`
- បង្កើត `StudentService` សម្រាប់ CRUD
- ភ្ជាប់ service ទៅ `Form`

បន្ទាប់ពីសិស្សយល់ច្បាស់ពី guide នេះ អាចបន្តទៅ validation, search, repository pattern, ឬ async CRUD បាន។
