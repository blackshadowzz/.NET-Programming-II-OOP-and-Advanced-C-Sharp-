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
}
