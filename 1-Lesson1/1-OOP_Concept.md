# មេរៀនទី១៖ Class និង Object

Object-Oriented Programming - OOP ក្នុងភាសា C# ដោយផ្ដោតសំខាន់លើការបង្កើត Class, Object និងសមាជិកផ្សេងៗរបស់វា។

## ១. គោលការណ៍គ្រឹះនៃ OOP

OOP គឺជាវិធីសាស្ត្រក្នុងការរចនាកម្មវិធី (Software applications design) ដែលពឹងផ្អែកលើ Class និង Object រួមជាមួយគោលការណ៍សំខាន់ៗចំនួន ៣៖

- **Encapsulation (ការវេចខ្ចប់)**: ជាការបញ្ចូលគ្នារវាង Property, អថេរ (Fields), អនុគមន៍ (Methods) និង Event ឱ្យស្ថិតនៅក្នុងកញ្ចប់តែមួយ។

- **Inheritance (ការបន្តពូជ)**: ជាសមត្ថភាពដែល Object មួយអាចទទួលយកនូវលក្ខណៈផ្សេងៗពី Object ដ៏ទៃទៀត។

- **Polymorphism (ពហុរូបភាព)**: ជាលក្ខណៈដែល Object ពីរផ្សេងគ្នា មាន Property និង Method ដូចគ្នា ប៉ុន្តែការអនុវត្ត (Implementation) មានភាពខុសគ្នា។


---


## ២. និយមន័យ Class និង Object

### ២.១ Class

Class គឺជាពុម្ពគំរូ ឬប្លង់ (Blueprint or Template) សម្រាប់ការបង្កើត Object។ វាកំណត់ទម្រង់ដោយការប្រមូលផ្តុំនូវអថេរ Method និង Event ជាក្រុមជាមួយគ្នា។

### ២.២ Object

Object គឺជាវត្ថុជាក់ស្តែងដែលកើតចេញពី Class (ហៅថា Instance)។ វារួមមាន៖

* Properties: សម្រាប់សម្គាល់ស្ថានភាព (State) និងធ្វើសុពលកម្ម (Validate) ទិន្នន័យ។
* Methods: សម្រាប់សម្គាល់នូវបុគ្គលិកលក្ខណៈ និងមុខងារ (Behavior)។
* Events: សម្រាប់បរិយាយពីស្ថានភាពរបស់ Object។

## Real Example

សាលាមួយចង់រក្សាទុកព័ត៌មានសិស្ស។

* **Class** = Blueprint “Student”
* **Object** = សិស្សជាក់ស្តែងម្នាក់ៗ

---

### Code Example

```csharp
public class Student
{
    // Field
    private string _name;

    // Property
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Age { get; set; }

    // Method
    public void Study()
    {
        Console.WriteLine($"{Name} is studying...");
    }
}
```

### Create Object

```csharp
Student s1 = new Student();
s1.Name = "Dara";
s1.Age = 20;

s1.Study();
```

### Real Meaning

| OOP Concept | Real World Meaning |
| ----------- | ------------------ |
| Class       | Blueprint សិស្ស    |
| Object      | Dara (សិស្សពិត)    |
| Property    | Name, Age          |
| Method      | Study()            |

---

ក្រុមហ៊ុនលក់ឡានចង់គ្រប់គ្រងព័ត៌មានឡាន។

### Code Example

```csharp
public class Car
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }

    public void StartEngine()
    {
        Console.WriteLine("Engine started...");
    }
}
```

### Create Object

```csharp
Car car1 = new Car();
car1.Brand = "Toyota";
car1.Model = "Corolla";
car1.Year = 2022;

car1.StartEngine();
```

### Real Meaning

* Class = Blueprint របស់ឡាន
* car1 = ឡាន Toyota Corolla មួយគ្រឿង
* StartEngine() = Behavior របស់ឡាន


Class = ប្លង់ផ្ទះ
Object = ផ្ទះដែលសាងសង់រួច

Class = ម៉ូឌែលទូរស័ព្ទ
Object = iPhone 15 មួយគ្រឿង

---


## ៣. ការបង្កើត Class និង Object

### ៣.១ រូបមន្តនៃការបង្កើត Class

ដើម្បីបង្កើត Class គេត្រូវប្រើ Access specifier រួមជាមួយពាក្យគន្លឹះ class និងឈ្មោះរបស់វា៖

```csharp
<access specifier> class ClassName 
{  
    // Class members ស្ថិតនៅទីនេះ
}
```
#### Example

```csharp
public class Car
{
    public int Id{get;set;}
    public string Model{get;set;}
    ...
}
```

 -   សម្គាល់៖ ឈ្មោះ Class មិនត្រូវចាប់ផ្តើមដោយលេខ ឬសញ្ញាពិសេសឡើយ។

### ៣.២ រូបមន្តនៃការបង្កើត Object

ការបង្កើត Object ចេញពី Class ត្រូវប្រើពាក្យគន្លឹះ new ដើម្បីឱ្យ Compiler រក្សាទុកទីតាំងក្នុង Memory (Allocate memory)៖


```csharp
class_name object_name = new class_name();
```


- ដើម្បីចូលទៅប្រើប្រាស់សមាជិករបស់ Object (Properties, Methods) គេត្រូវប្រើសញ្ញាចុច (Dot operator .)។



---


## ៤. សមាជិកនៃ Class (Class Members)

ជាទូទៅ Class តែងមានសមាជិកដូចជា៖ 
- **Field**: អថេរនៅក្នុង Class (ទូទៅជា private/public)  
- **Property**: អាន/សរសេរ/គណនាតម្លៃ Field  
- **Method**: មុខងាររបស់ Object  
- **Event**: បង្ហាញស្ថានភាព  
- **Constructor/Destructor**: កំណត់ និងបំផ្លាញ Object  
- **Indexer**: ចូលប្រើទិន្នន័យតាម Index 


### ៤.១ Field (អថេរក្នុង Class)

- Field គឺជាអថេរ (Variable) ដែលប្រកាសដោយផ្ទាល់នៅក្នុង Class។ 
- វាអាចជាប្រភេទ private ឬ public។ 
- ជាទូទៅ គេប្រើវាជាលក្ខណៈ Data Hiding ឬ Data Abstraction ដោយមិនអនុញ្ញាតឱ្យចូលប្រើដោយផ្ទាល់ពីខាងក្រៅឡើយ។


### ៤.២ Properties

Property គឺជាសមាជិកដែលផ្តល់នូវយន្តការបត់បែនក្នុងការអាន សរសេរ ឬគណនាតម្លៃនៃ Private Field។ គេហៅវាថាជា Method ពិសេសម្យ៉ាងដែលមានឈ្មោះថា Accessor (ការរួមគ្នារវាង Field និង Method)។

#### ប្រភេទនៃ Property:

1. Read-Write Property: មានទាំង get (សម្រាប់ទាញយកតម្លៃ) និង set (សម្រាប់កំណត់តម្លៃថ្មី)។

```csharp
public string Name
{
    get { return _name; }
    set { _name = value; }
}
```

2. Read-Only Property: មានតែ get accessor ប៉ុណ្ណោះ មិនអាចផ្តល់តម្លៃឱ្យបានឡើយ។

```csharp
public string Name
{
    get { return _name; }
}
```

3. Write-Only Property: មានតែ set accessor (កម្រប្រើប្រាស់)។

```csharp
public string Password
{
    set { _password = value; }
}
```

4. Auto-implemented Property: ជាប្រភេទ Property ដែលមិនមានការសរសេរកូដក្នុង Body ឡើយ។

```csharp
public int Age { get; set; }
```

#### Full Example

```csharp
public class UserAccount
{
    // Private Fields
    private string _name;
    private string _password;
    private DateTime _createdDate = DateTime.Now;

    // 1️ Read-Write Property
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    // 2️ Read-Only Property
    public DateTime CreatedDate
    {
        get { return _createdDate; }
    }

    // 3️ Write-Only Property
    public string Password
    {
        set { _password = value; }
    }

    // 4️ Auto-Implemented Property
    public int Age { get; set; }

    // Method to display some info
    public void DisplayInfo()
    {
        Console.WriteLine("Name: " + Name);
        Console.WriteLine("Age: " + Age);
        Console.WriteLine("Account Created: " + CreatedDate);
    }

    // Method to validate password internally
    public bool CheckPassword(string input)
    {
        return _password == input;
    }
}

class Program
{
    static void Main()
    {
        UserAccount user = new UserAccount();

        //  Read-Write
        user.Name = "Dara";

        // Auto Property
        user.Age = 22;

        // Write-Only
        user.Password = "12345";

        user.DisplayInfo();

        Console.WriteLine("Password Correct? " + user.CheckPassword("12345"));

        // Not allowed (Compile Error)
        //Console.WriteLine(user.Password); 
        //user.CreatedDate = DateTime.Now;
    }
}
```

### តារាងសេចក្តីសង្ខេបអំពី Access Specifiers:

| Access Modifier | ពិពណ៌នា                         |
| --------------- | ------------------------------- |
| `private`       | អាចប្រើបានតែជាមួយ Class Object ខ្លួនឯងប៉ុណ្ណោះ។    |
| `public`        | អាចប្រើបានគ្រប់ Object ទាំងអស់ក្នុង Project និងពី Project ផ្សេងទៀត។  |
| `protected`     | ប្រើបានក្នុង Class ខ្លួនឯង និង Sub-class ដែលបាន Inherit។ |
| `internal`      | ប្រើបានគ្រប់ Object ទាំងអស់ដែលមាននៅក្នុង Project ជាមួយគ្នា។  |

### Example 

```csharp
Person p = new Person();
p.Name = "Dara";
p.DisplayInfo();

// internal → accessible inside same project
Console.WriteLine("Phone from Main: " + p.PhoneNumber);

// Console.WriteLine(p.Address);

// Console.WriteLine(p._nationalId);

Console.WriteLine("------");

Student s = new Student();
s.Name = "Sokha";
s.ShowAddress();

// public class → អាចប្រើបានគ្រប់ទីកន្លែង
public class Person
{
    private string _nationalId = "ID12345";
    protected string Address = "Phnom Penh";
    internal string PhoneNumber = "012345678";
    public string Name { get; set; }

    private void ShowNationalId()
    {
        Console.WriteLine("National ID: " + _nationalId);
    }

    public void DisplayInfo()
    {
        Console.WriteLine("Name: " + Name);
        Console.WriteLine("Phone: " + PhoneNumber);

        // Allowed (private used inside class)
        ShowNationalId();
    }
}

// Subclass (Inheritance)
public class Student : Person
{
    public void ShowAddress()
    {
        // Allowed (protected accessible in subclass)
        Console.WriteLine("Address: " + Address);

        // Not allowed (private cannot access)
        // Console.WriteLine(_nationalId);
    }
}
```

---


## ៥. ការប្រើប្រាស់ពាក្យគន្លឹះ 'this'

`this` keyword សំដៅទៅលើ Instance បច្ចុប្បន្នរបស់ Class (The current instance of class)។ គេប្រើប្រាស់វាដើម្បី៖

* ចូលទៅប្រើប្រាស់ (Access) សមាជិករបស់ Class ពីក្នុង Constructor, Instance Method ឬ Instance Accessor។

* សម្គាល់រវាងអថេរដែលជា Parameter និង Field របស់ Class នៅពេលពួកវាមានឈ្មោះដូចគ្នា។

```csharp
Customer customer = new Customer("Koko", 25);
Console.WriteLine(customer.Name);
Console.WriteLine(customer.Age);
public class Customer
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    public Customer(string Name, int Age)
    {
        this.Name = Name; // this keyword
        this.Age = Age; // this keyword
    }
}
``` 

- ចំណាំ៖ `this` មិនអាចប្រើប្រាស់ជាមួយ Static Class ឬ Static Member បានឡើយ ព្រោះវាមិនអាចបង្កើត Instance បាន។


---


### Example

ខាងក្រោមនេះគឺជាឧទាហរណ៍នៃការបង្កើត Class Customer ដែលរួមមាន Field, Property និង Method៖
```csharp
using System;

namespace AppDemo 
{
    public class Customer
    {
        // 1. Field
        private string _firstName;
        private string _lastName;

        // 2. Read-Write Property ជាមួយ 'this' keyword
        public string FirstName
        {
            get { return _firstName; }
            set { this._firstName = value; }
        }

        // 3. Auto-implemented Property
        public int Age { get; set; }

        // 4. Constructor ប្រើ 'this' ដើម្បីកំណត់តម្លៃឱ្យ Field
        public Customer(string fname, string lname)
        {
            this._firstName = fname;
            this._lastName = lname;
        }

        // 5. Method
        public string GetFullName()
        {
            return $"FullName: {FirstName} {_lastName}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // ការបង្កើត Object
            Customer customer = new Customer("Keo", "Sokha");
            
            // ការបង្ហាញលទ្ធផល
            Console.WriteLine(customer.GetFullName());
        }
    }
}
```

#### តាមរយៈឧទាហរណ៍ខាងលើ៖

* _firstName និង _lastName គឺជា Field ដែលលាក់បាំងទិន្នន័យ។
* FirstName គឺជា Property សម្រាប់ទាញយក និងកំណត់តម្លៃ។
* GetFullName() គឺជា Method សម្រាប់បង្ហាញមុខងាររបស់ Object។
* new Customer(...) គឺជាការបង្កើត Object ចេញពី Class។


--- 

## Practices 
### Create class Book

```csharp
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}, Author: {Author}, Price: ${Price}");
    }
}
```
### Output
```csharp
Book b1 = new Book { Title = "C# Basics", Author = "John", Price = 25 };
Book b2 = new Book { Title = "OOP Master", Author = "David", Price = 30 };

b1.DisplayInfo();
b2.DisplayInfo();
```
