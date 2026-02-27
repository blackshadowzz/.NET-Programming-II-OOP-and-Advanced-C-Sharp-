មគ្គុទ្ទេសក៍សិក្សា មេរៀនទី១៖ Class និង Object

មគ្គុទ្ទេសក៍សិក្សានេះត្រូវបានរៀបចំឡើងដើម្បីផ្ដល់នូវការយល់ដឹងស៊ីជម្រៅអំពីមូលដ្ឋានគ្រឹះនៃ Object-Oriented Programming - OOP ក្នុងភាសា C# ដោយផ្ដោតសំខាន់លើការបង្កើត Class, Object និងសមាជិកផ្សេងៗរបស់វា។

១. គោលការណ៍គ្រឹះនៃ OOP

OOP គឺជាវិធីសាស្ត្រក្នុងការរចនាកម្មវិធី (Software applications design) ដែលពឹងផ្អែកលើ Class និង Object រួមជាមួយគោលការណ៍សំខាន់ៗចំនួន ៣៖

* Encapsulation (ការវេចខ្ចប់): ជាការបញ្ចូលគ្នារវាង Property, អថេរ (Fields), អនុគមន៍ (Methods) និង Event ឱ្យស្ថិតនៅក្នុងកញ្ចប់តែមួយ។
* Inheritance (ការបន្តពូជ): ជាសមត្ថភាពដែល Object មួយអាចទទួលយកនូវលក្ខណៈផ្សេងៗពី Object ដ៏ទៃទៀត។
* Polymorphism (ពហុរូបភាព): ជាលក្ខណៈដែល Object ពីរផ្សេងគ្នា មាន Property និង Method ដូចគ្នា ប៉ុន្តែការអនុវត្ត (Implementation) មានភាពខុសគ្នា។


--------------------------------------------------------------------------------


២. និយមន័យ Class និង Object

២.១ Class

Class គឺជាពុម្ពគំរូ ឬប្លង់ (Blueprint or Template) សម្រាប់ការបង្កើត Object។ វាកំណត់ទម្រង់ដោយការប្រមូលផ្តុំនូវអថេរ Method និង Event ជាក្រុមជាមួយគ្នា។

២.២ Object

Object គឺជាវត្ថុជាក់ស្តែងដែលកើតចេញពី Class (ហៅថា Instance)។ វារួមមាន៖

* Properties: សម្រាប់សម្គាល់ស្ថានភាព (State) និងធ្វើសុពលកម្ម (Validate) ទិន្នន័យ។
* Methods: សម្រាប់សម្គាល់នូវបុគ្គលិកលក្ខណៈ និងមុខងារ (Behavior)។
* Events: សម្រាប់បរិយាយពីស្ថានភាពរបស់ Object។


--------------------------------------------------------------------------------


៣. ការបង្កើត Class និង Object

៣.១ រូបមន្តនៃការបង្កើត Class

ដើម្បីបង្កើត Class គេត្រូវប្រើ Access specifier រួមជាមួយពាក្យគន្លឹះ class និងឈ្មោះរបស់វា៖

<access specifier> class ClassName 
{  
    // Class members ស្ថិតនៅទីនេះ
}


សម្គាល់៖ ឈ្មោះ Class មិនត្រូវចាប់ផ្តើមដោយលេខ ឬសញ្ញាពិសេសឡើយ។

៣.២ រូបមន្តនៃការបង្កើត Object

ការបង្កើត Object ចេញពី Class ត្រូវប្រើពាក្យគន្លឹះ new ដើម្បីឱ្យ Compiler រក្សាទុកទីតាំងក្នុង Memory (Allocate memory)៖

class_name object_name = new class_name();


ដើម្បីចូលទៅប្រើប្រាស់សមាជិករបស់ Object (Properties, Methods) គេត្រូវប្រើសញ្ញាចុច (Dot operator .)។


--------------------------------------------------------------------------------


៤. សមាជិកនៃ Class (Class Members)

ជាទូទៅ Class តែងមានសមាជិកដូចជា៖ Field, Property, Method, Event, Indexer, Constructor និង Destructor។

៤.១ Field (អថេរក្នុង Class)

Field គឺជាអថេរ (Variable) ដែលប្រកាសដោយផ្ទាល់នៅក្នុង Class។ វាអាចជាប្រភេទ private ឬ public។ ជាទូទៅ គេប្រើវាជាលក្ខណៈ Data Hiding ឬ Data Abstraction ដោយមិនអនុញ្ញាតឱ្យចូលប្រើដោយផ្ទាល់ពីខាងក្រៅឡើយ។

៤.២ Properties

Property គឺជាសមាជិកដែលផ្តល់នូវយន្តការបត់បែនក្នុងការអាន សរសេរ ឬគណនាតម្លៃនៃ Private Field។ គេហៅវាថាជា Method ពិសេសម្យ៉ាងដែលមានឈ្មោះថា Accessor (ការរួមគ្នារវាង Field និង Method)។

ប្រភេទនៃ Property:

1. Read-Write Property: មានទាំង get (សម្រាប់ទាញយកតម្លៃ) និង set (សម្រាប់កំណត់តម្លៃថ្មី)។
2. Read-Only Property: មានតែ get accessor ប៉ុណ្ណោះ មិនអាចផ្តល់តម្លៃឱ្យបានឡើយ។
3. Write-Only Property: មានតែ set accessor (កម្រប្រើប្រាស់)។
4. Auto-implemented Property: ជាប្រភេទ Property ដែលមិនមានការសរសេរកូដក្នុង Body ឡើយ។

តារាងសេចក្តីសង្ខេបអំពី Access Specifiers:

Access Specifier	ការបរិយាយ
private	អាចប្រើបានតែជាមួយ Class Object ខ្លួនឯងប៉ុណ្ណោះ។
public	អាចប្រើបានគ្រប់ Object ទាំងអស់ក្នុង Project និងពី Project ផ្សេងទៀត។
protected	ប្រើបានក្នុង Class ខ្លួនឯង និង Sub-class ដែលបាន Inherit។
internal	ប្រើបានគ្រប់ Object ទាំងអស់ដែលមាននៅក្នុង Project ជាមួយគ្នា។


--------------------------------------------------------------------------------


៥. ការប្រើប្រាស់ពាក្យគន្លឹះ 'this'

this keyword សំដៅទៅលើ Instance បច្ចុប្បន្នរបស់ Class (The current instance of class)។ គេប្រើប្រាស់វាដើម្បី៖

* ចូលទៅប្រើប្រាស់ (Access) សមាជិករបស់ Class ពីក្នុង Constructor, Instance Method ឬ Instance Accessor។
* សម្គាល់រវាងអថេរដែលជា Parameter និង Field របស់ Class នៅពេលពួកវាមានឈ្មោះដូចគ្នា។

ចំណាំ៖ this មិនអាចប្រើប្រាស់ជាមួយ Static Class ឬ Static Member បានឡើយ ព្រោះវាមិនអាចបង្កើត Instance បាន។


--------------------------------------------------------------------------------


៦. ឧទាហរណ៍នៃការអនុវត្ត (Code Example)

ខាងក្រោមនេះគឺជាឧទាហរណ៍នៃការបង្កើត Class Customer ដែលរួមមាន Field, Property និង Method៖

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


តាមរយៈឧទាហរណ៍ខាងលើ៖

* _firstName និង _lastName គឺជា Field ដែលលាក់បាំងទិន្នន័យ។
* FirstName គឺជា Property សម្រាប់ទាញយក និងកំណត់តម្លៃ។
* GetFullName() គឺជា Method សម្រាប់បង្ហាញមុខងាររបស់ Object។
* new Customer(...) គឺជាការបង្កើត Object ចេញពី Class។
