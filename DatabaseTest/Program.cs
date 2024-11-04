using System;
using MySql.Data.MySqlClient;


namespace DatabaseTest
{
    internal class Program
    {
        public class Account
        {
            private MySqlConnection _connection;
            private MySqlTransaction _transaction;
            public Account(string DataBaseName, String UserName, String Password) {
                string connectionString = $"Server=localhost;Database={DataBaseName};User ID={UserName};Password={Password};";
                _connection = new MySqlConnection(connectionString);
            }

            public void RegisterThis(String FirstName, String LastName, String Email, String Password, String PhoneNumber)
            {
                _connection.Open();
                _transaction = _connection.BeginTransaction();
                try
                {
                    string query = $"INSERT INTO accounts(FirstName, LastName, Email, Password, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @Password, @PhoneNumber)";
                    MySqlCommand command = new MySqlCommand(query, _connection, _transaction);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Registered Successfully");
                    _transaction.Commit();
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    Console.WriteLine($"An error has occured: {ex.Message}");
                }
                finally
                {
                    _connection.Close();
                }
            }
            public bool Login(string email, string password)
            {
                try
                {
                    _connection.Open();
                    string query = $"SELECT * FROM accounts WHERE Email=@Email AND Password=@Password";
                    MySqlCommand command = new MySqlCommand(query, _connection);

                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    MySqlDataReader reader = command.ExecuteReader();
                    return reader.HasRows;

                }
                catch(Exception ex)
                {
                    Console.WriteLine($"An Error has Occured{ex.Message}");
                    return false;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        static void Main(string[] args)
        {
            Account accounts = new Account("airplane test", "root", "");



            while (true)
            {
                Console.Write("Choose Transaction [1] Register [2] Login [3] Exit: ");
                String choose = Console.ReadLine();

                if (choose == "1")
                {
                    Console.Write("Enter First Name: ");
                    string FirstName = Console.ReadLine();
                    Console.Write("Enter Last Name: ");
                    string LastName = Console.ReadLine();
                    Console.Write("Enter Email: ");
                    string Email = Console.ReadLine();
                    Console.Write("Enter Password: ");
                    string Password = Console.ReadLine();
                    Console.Write("Enter Phone Number: ");
                    string PhoneNumber = Console.ReadLine();
                    accounts.RegisterThis(FirstName, LastName, Email, Password, PhoneNumber);
                }
                else if (choose == "2")
                {
                    string email = "";
                    string password = "";

                    do
                    {
                        Console.Write("Enter Email: ");
                        email = Console.ReadLine();
                        Console.Write("Enter Password: ");
                        password = Console.ReadLine();
                    } while (!accounts.Login(email, password));
                    Console.WriteLine("Login Successful");
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            
        }
    }
}
