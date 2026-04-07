# Sample CRUD Window Form

## Project Name

Create new project Window Form App - `WinFormsAppDemo`

## Project Structure

```
/Data
    ConnectionHelper.cs
/Forms
    /Lists
        /Categories
            CategoryForm.cs
            CreateCategoryForm.cs
            UpdateCategoryForm.cs
/Models
    Category.cs
/Services
    CategoryService.cs

MainForm.cs

```

---

## Database

Create database name - `DemoDB`

### Tables

Create table name - `TbCategories`

```sql

CREATE TABLE [dbo].[TbCategories] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName] NVARCHAR (100) NOT NULL,
    [Description]  NVARCHAR (200) NULL,
    [IsActive]     BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

```

---

## Connection Service

Create new class in Data folder - `ConnectionHelper.cs`

write code below

![alt text](Images/image1.png)

---

## Models

Create new model class - `Category.cs` and write code below

![alt text](Images/image2.png)

---

## Services

Create service `CategoryService.cs` class inside folder Services

![alt text](Images/image3.png)

### Add method GetCategories() in CategoryService.cs

In GetCategories() method, write code like below

![alt text](Images/image4.png)

### Add method CreateCategory() in CategoryService.cs

In CreateCategory() method, write code like below

![alt text](Images/image5.png)

### Add method GetCategory() in CategoryService.cs

In GetCategory() method, write code like below

![alt text](Images/image6.png)

### Add method UpdateCategory() in CategoryService.cs

In UpdateCategory() method, write code like below

![alt text](Images/image7.png)

### Add method DeleteCategory() in CategoryService.cs

In DeleteCategory() method, write code like below

![alt text](Images/image8.png)

---

## Design MainForm

![alt text](Images/image12.png)

```csharp
namespace WinFormsAppDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoryForm form = new CategoryForm();
            form.MdiParent = this;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Show();
        }
    }
}
```

## Design Category form

![alt text](Images/image9.png)

```csharp
namespace WinFormsAppDemo.Forms.Lists.Categories
{
    public partial class CategoryForm : Form
    {
        private readonly CategoryService categoryService = new CategoryService();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Id { get; set; }
        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            GetCategories();
        }
        protected void GetCategories(string? q = "")
        {
            var cates = categoryService.GetCategories(q);
            dataGridView1.DataSource = cates;
            dataGridView1.AutoGenerateColumns = true;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            CreateCategoryForm form = new CreateCategoryForm();
            form.WindowState = FormWindowState.Normal;
            form.StartPosition = FormStartPosition.WindowsDefaultLocation;
            form.ActionStatus = CommandOptions.Create;
            if (form.ShowDialog() == DialogResult.OK)
            {
                GetCategories();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            var category = (Category)dataGridView1.CurrentRow.DataBoundItem!;
            var cate = categoryService.GetCategory(category.Id);
            Id = cate.Id;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            UpdateCategoryForm form = new UpdateCategoryForm();
            form.WindowState = FormWindowState.Normal;
            form.StartPosition = FormStartPosition.WindowsDefaultLocation;
            form.Id = Id;
            form.ActionStatus = CommandOptions.Update;
            if (form.ShowDialog() == DialogResult.OK)
            {
                GetCategories();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var delete = MessageBox.Show(
                "Delete category?",
                "Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes;
            try
            {
                if (delete)
                {
                    var result = categoryService.DeleteCategory(Id);
                    if (result)
                    {
                        GetCategories();
                        MessageBox.Show("Category deleted succeed");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var filter = txtSearch.Text;
            GetCategories(filter);
        }
    }
}
```

### Create Category form

![alt text](Images/image10.png)

```csharp
namespace WinFormsAppDemo.Forms.Lists.Categories
{
    public partial class CreateCategoryForm : Form
    {
        public CreateCategoryForm()
        {
            InitializeComponent();
        }
        public CommandOptions ActionStatus;
        private readonly CategoryService categoryService = new CategoryService();
        private void CreateCategoryForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var category = new Category();
            category.CategoryName = txtCategoryName.Text;
            category.Description = richTextBoxDesc.Text;
            category.IsActive = checkBoxIsActive.Checked;

            try
            {
                var Id = categoryService.CreateCategory(category);
                if (Id > 0)
                {
                    MessageBox.Show("Category created succeed");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                else
                    MessageBox.Show("Creating failed");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
```

### Update category form

![alt text](Images/image11.png)

```csharp
namespace WinFormsAppDemo.Forms.Lists.Categories
{
    public partial class UpdateCategoryForm : Form
    {
        public UpdateCategoryForm()
        {
            InitializeComponent();
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Id { get; set; }
        public CommandOptions ActionStatus;
        private Category category = new();
        private readonly CategoryService categoryService = new CategoryService();
        private void UpdateCategoryForm_Load(object sender, EventArgs e)
        {
            if (Id > 0)
            {
                category = categoryService.GetCategory(Id);
                txtCategoryName.Text = category.CategoryName;
                richTextBoxDesc.Text = category.Description;
                checkBoxIsActive.Checked = category.IsActive;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            category.CategoryName = txtCategoryName.Text;
            category.Description = richTextBoxDesc.Text;
            category.IsActive = checkBoxIsActive.Checked;

            try
            {
                var result = categoryService.UpdateCategory(category);
                if (result)
                {
                    MessageBox.Show("Category updated succeed");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                else
                    MessageBox.Show("Updating failed");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

```
