# Using Dapper with SQL Server

Dapper គឺជា **Micro ORM** (Micro Object-Relational Mapper) ដ៏ពេញនិយមបំផុតមួយសម្រាប់ .NET។ វាមិនមែនជា ORM ពេញលេញដូច Entity Framework ទេ (គ្មាន Change Tracking, Migration ស្វ័យប្រវត្តិ, LINQ ជាដើម) ប៉ុន្តែវាលឿនខ្លាំង ស្រាល និងផ្តល់សិទ្ធិគ្រប់គ្រង SQL ទាំងស្រុងដល់ developer។

Dapper បន្ថែម **extension methods** លើ `IDbConnection` (ពី ADO.NET) ដើម្បីធ្វើការ execute SQL និង map លទ្ធផលទៅជា object ឬ list របស់ C# យ៉ាងងាយស្រួល។

វាត្រូវបានបង្កើតឡើងដោយក្រុម `Stack Overflow` ដើម្បីដោះស្រាយបញ្ហា performance នៅពេលប្រើ ORM ធម្មតា។

---


### លក្ខណៈសំខាន់ៗរបស់ Dapper

- លឿនជាង Entity Framework ច្រើន (ជិតដូច ADO.NET សុទ្ធ)
- សរសេរ SQL ដោយផ្ទាល់ → គ្មាន "surprise query" ដូច EF
- គាំទ្រ multi-mapping, stored procedure, transaction, multiple result sets
- គាំទ្រ SQL Server, PostgreSQL, MySQL, SQLite, Oracle ជាដើម

### ចាប់ផ្តើមពី Zero ដល់ CRUD ជាមួយ SQL Server

#### 1. រៀបចំ Project

បង្កើត project ថ្មី (Console App or Window Form)

```bash
dotnet new winforms -n DapperWinFormsDemo
cd DapperWinFormsDemo
```

#### 2. ដំឡើង Package ចាំបាច់

```bash
dotnet add package Dapper
dotnet add package Microsoft.Data.SqlClient   # សម្រាប់ SQL Server
```

---


#### 3. បង្កើត Table នៅ SQL Server (ឧទាហរណ៍តារាង `Product`)

```sql
CREATE DATABASE DapperDemo;
GO

USE DapperDemo;
GO

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
```

#### 4. បង្កើត Model (C# class)

```csharp
// Models/Product.cs
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedDate { get; set; }
}

```

---

## Connection management class
the most common and clean approaches in WinForms are:

1. Hard-coded connection string (quick for development/testing)
2. From App.config (classic, still works perfectly)
3. From appsettings.json (modern style, requires a few extra lines)

### Option 1: Very Simple (Connection string inside the class – good for learning/small apps)

Create new class `DapperConnection.cs`  inside folder Data

```csharp
using Microsoft.Data.SqlClient;
using System.Data;

namespace YourWinFormsApp.Data
{
    public static class DapperConnection
    {
        // Change this string according to your SQL Server setup
        private const string ConnectionString = 
            "Server=(localdb)\\MSSQLLocalDB;" +
            "Database=DapperDB;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";

        // Returns an **opened** connection (most common pattern with Dapper)
        public static IDbConnection GetOpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        // Returns a connection that is **not yet opened** (useful for transactions)
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}

```
Usage example in your Form or Repository:

```csharp
using var conn = DatabaseConnection.GetOpenConnection();
var employees = conn.Query<Employee>("SELECT * FROM Employees");
```

### Option 2: Read  connectionStrings from App.config

Step 1: Right-click project → Add → New Item → Application Configuration File → name it App.config
Step 2: Edit App.config (or app.config):

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <connectionStrings>
    <add name="DefaultConnection"
         connectionString="Server=(localdb)\MSSQLLocalDB;Database=DapperDB;Trusted_Connection=True;TrustServerCertificate=True;"
         providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>

```

Step 3: Connection class

```csharp
using Microsoft.Data.SqlClient;
using System.Configuration;           // Important: add this
using System.Data;

namespace YourWinFormsApp.Data
{
    public static class DatabaseConnection
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in App.config");
        }

        public static IDbConnection GetOpenConnection()
        {
            var conn = new SqlConnection(GetConnectionString());
            conn.Open();
            return conn;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
```

---

### Option 3 – Modern: Read from appsettings.json

**Step 1:** Add **appsettings.json** to project root

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=DapperWinForms;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Step 2:** Set file properties: **Copy to Output Directory** = **Copy if newer** or **Copy always**

**Step 3:** Install package (if not already):

```bash
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.Binder   // optional but useful
```

**Step 4:** Connection class with lazy configuration

```csharp
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace YourWinFormsApp.Data
{
    public class DatabaseConnection
    {
        private static IConfiguration? _configuration;
        private static readonly object _lock = new();

        private static string GetConnectionString()
        {
            if (_configuration == null)
            {
                lock (_lock)
                {
                    if (_configuration == null)
                    {
                        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                        _configuration = builder.Build();
                    }
                }
            }

            var connStr = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");

            return connStr;
        }

        public static IDbConnection GetOpenConnection()
        {
            var conn = new SqlConnection(GetConnectionString());
            conn.Open();
            return conn;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
```

## ProductRepository - using Dapper pass ConnectionStrings directly

```csharp

using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace YourAppName.Data
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString 
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        // ────────────────────────────────────────────────
        // READ - Get All Products
        // ────────────────────────────────────────────────
        public IEnumerable<Product> GetAll()
        {
            using var conn = CreateConnection();
            return conn.Query<Product>(
                @"SELECT Id, Name, Price, Stock, CreatedDate 
                  FROM Products 
                  ORDER BY Id DESC");
        }

        // ────────────────────────────────────────────────
        // READ - Get Single Product by ID
        // ────────────────────────────────────────────────
        public Product? GetById(int id)
        {
            using var conn = CreateConnection();
            return conn.QuerySingleOrDefault<Product>(
                @"SELECT Id, Name, Price, Stock, CreatedDate 
                  FROM Products 
                  WHERE Id = @Id",
                new { Id = id });
        }

        // ────────────────────────────────────────────────
        // CREATE - Insert new Product
        // ────────────────────────────────────────────────
        public int Insert(Product product)
        {
            using var conn = CreateConnection();

            const string sql = @"
                INSERT INTO Products (Name, Price, Stock, CreatedDate)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Price, @Stock, @CreatedDate);";

            return conn.ExecuteScalar<int>(sql, product);
        }

        // ────────────────────────────────────────────────
        // UPDATE
        // ────────────────────────────────────────────────
        public bool Update(Product product)
        {
            using var conn = CreateConnection();

            const string sql = @"
                UPDATE Products
                SET Name = @Name,
                    Price = @Price,
                    Stock = @Stock
                WHERE Id = @Id";

            int rows = conn.Execute(sql, product);
            return rows > 0;
        }

        // ────────────────────────────────────────────────
        // DELETE
        // ────────────────────────────────────────────────
        public bool Delete(int id)
        {
            using var conn = CreateConnection();
            int rows = conn.Execute("DELETE FROM Products WHERE Id = @Id", new { Id = id });
            return rows > 0;
        }
    }
}

```

---


## ProductRepository - using DatabaseConnection static

```csharp

// Data/ProductRepository.cs
using Dapper;
using System.Data;

namespace YourWinFormsApp.Data
{
    public class ProductRepository
    {
        // No more _connectionString field or constructor parameter

        private IDbConnection CreateConnection()
        {
            // Uses the static helper class directly
            return DatabaseConnection.GetOpenConnection();
            // or: return DatabaseConnection.GetConnection();  ← if you prefer not auto-open
        }

        public IEnumerable<Product> GetAll()
        {
            using var conn = CreateConnection();
            return conn.Query<Product>(
                @"SELECT Id, Name, Price, Stock, CreatedDate
                  FROM Products
                  ORDER BY Id DESC");
        }

        public Product? GetById(int id)
        {
            using var conn = CreateConnection();
            return conn.QuerySingleOrDefault<Product>(
                @"SELECT Id, Name, Price, Stock, CreatedDate
                  FROM Products
                  WHERE Id = @Id",
                new { Id = id });
        }

        public int Insert(Product product)
        {
            using var conn = CreateConnection();

            const string sql = @"
                INSERT INTO Products (Name, Price, Stock, CreatedDate)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Price, @Stock, @CreatedDate);";

            return conn.ExecuteScalar<int>(sql, product);
        }

        public bool Update(Product product)
        {
            using var conn = CreateConnection();

            const string sql = @"
                UPDATE Products
                SET Name = @Name, Price = @Price, Stock = @Stock
                WHERE Id = @Id";

            return conn.Execute(sql, product) > 0;
        }

        public bool Delete(int id)
        {
            using var conn = CreateConnection();
            return conn.Execute(
                "DELETE FROM Products WHERE Id = @Id",
                new { Id = id }) > 0;
        }
    }
}

```

---

## 3. ProductForm (Windows Forms – sample CRUD)

```csharp

using System;
using System.Windows.Forms;
using YourAppName.Data;  // adjust namespace

namespace YourAppName.Forms
{
    public partial class ProductForm : Form
    {
        private readonly ProductRepository _repo;
        private int? _editingId = null;

        public ProductForm()
        {
            InitializeComponent();
            // ────────────────────────────────────────────────
            // Change connection string according to your environment
            // ────────────────────────────────────────────────
            string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=YourDatabaseName;Trusted_Connection=True;TrustServerCertificate=True;";
            _repo = new ProductRepository(connStr);

            // Form setup
            SetupControls();
            LoadProducts();
        }

        private void SetupControls()
        {
            // Assume you already dragged these controls in Designer:
            // dgvProducts    → DataGridView
            // txtName        → TextBox
            // nudPrice       → NumericUpDown (for Price)
            // nudStock       → NumericUpDown (for Stock)
            // dtpCreated     → DateTimePicker (optional – usually read-only)
            // btnAdd         → Button
            // btnUpdate      → Button
            // btnDelete      → Button
            // btnClear       → Button
            // lblId          → Label (shows current ID)

            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            nudPrice.DecimalPlaces = 2;
            nudPrice.Minimum = 0;
            nudPrice.Maximum = 9999999;

            nudStock.Minimum = 0;
            nudStock.Maximum = 999999;

            dtpCreated.Enabled = false; // usually auto-set by DB
        }

        private void LoadProducts()
        {
            try
            {
                var products = _repo.GetAll().ToList();
                dgvProducts.DataSource = products;

                // Optional: hide or reorder columns
                if (dgvProducts.Columns["Id"] != null)
                    dgvProducts.Columns["Id"].Visible = false;

                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            var product = (Product)dgvProducts.CurrentRow.DataBoundItem;

            _editingId = product.Id;
            txtName.Text = product.Name;
            nudPrice.Value = product.Price;
            nudStock.Value = product.Stock;
            dtpCreated.Value = product.CreatedDate;

            lblId.Text = $"ID: {product.Id}";
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            var product = new Product
            {
                Name = txtName.Text.Trim(),
                Price = nudPrice.Value,
                Stock = (int)nudStock.Value,
                CreatedDate = DateTime.Now   // or let DB handle with DEFAULT
            };

            try
            {
                int newId = _repo.Insert(product);
                MessageBox.Show($"Product added successfully! ID = {newId}", "Success");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!_editingId.HasValue)
            {
                MessageBox.Show("Please select a product to update.", "Warning");
                return;
            }

            if (!ValidateInput()) return;

            var product = new Product
            {
                Id = _editingId.Value,
                Name = txtName.Text.Trim(),
                Price = nudPrice.Value,
                Stock = (int)nudStock.Value,
                // CreatedDate usually not updated
            };

            try
            {
                bool success = _repo.Update(product);
                if (success)
                {
                    MessageBox.Show("Product updated successfully!", "Success");
                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product:\n{ex.Message}", "Error");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_editingId.HasValue)
            {
                MessageBox.Show("Please select a product to delete.", "Warning");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                bool success = _repo.Delete(_editingId.Value);
                if (success)
                {
                    MessageBox.Show("Product deleted successfully!", "Success");
                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product:\n{ex.Message}", "Error");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            _editingId = null;
            txtName.Clear();
            nudPrice.Value = 0;
            nudStock.Value = 0;
            dtpCreated.Value = DateTime.Now;
            lblId.Text = "ID: —";
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Product name is required.", "Validation");
                txtName.Focus();
                return false;
            }

            if (nudPrice.Value <= 0)
            {
                MessageBox.Show("Price must be greater than 0.", "Validation");
                nudPrice.Focus();
                return false;
            }

            return true;
        }
    }
}

```