using System;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace BikeShowroomApplication
{
    class Program
    {
        private const string connectionString = "server=localhost;uid=Sanket;pwd=Mh15dg0525;database=bikeshowroom";

        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Header
            Console.WriteLine("         ╔══════════════════════════════════════════╗");
            Console.WriteLine("         ║              Welcome to Our              ║");
            Console.WriteLine("         ║       * Shri Sai Bike ShowRoom *         ║");
            Console.WriteLine("         ╚══════════════════════════════════════════╝");
            Console.WriteLine();

            // Content
            Console.WriteLine("       ----- We offer a Diffrent High End Bike Category ----- ");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("            ╭───────────────╮     ╭───────────────╮  ");
            Console.WriteLine("            │   Economical  │  &  │  High End CC  │  ");
            Console.WriteLine("            ╰───────────────╯     ╰───────────────╯  ");
            Console.WriteLine();

            Console.WriteLine("              Visit our Showroom and Select        ");      
            Console.WriteLine("                the perfect Bike for your          ");
            Console.WriteLine("                      Daily commute .              ");
            Console.WriteLine();

            Console.WriteLine("                      Special Diwali Offers :                           ");
            Console.WriteLine("   10% cashBack on Any High End Bikes & Ecomonical Bike Also .");
           
                       
            Console.WriteLine();

            // Footer
            Console.WriteLine("          Contact Us:                        ");
            Console.WriteLine("          ☎ Phone: +91 8999116815              ");
            Console.WriteLine("          ✉ Email: info@Shri_Sai_Bike_ShowRoom.com");
            Console.WriteLine("          🏠 Address: Near Temple, Shirdi , Maharashtra.");
            Console.WriteLine();
            Console.WriteLine();

            Console.ResetColor();

            try
            {
                CreateTables();

                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine();
                    Console.WriteLine("---------- Bike ShowRoom ----------");
                    Console.WriteLine();
                    Console.WriteLine("1. Register");
                    Console.WriteLine("2. User Login");
                    Console.WriteLine("3. Admin Login");
                    Console.WriteLine("4. Exit");
                    Console.Write("Enter your choice: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            RegisterUser();
                            break;
                        case 2:
                            UserLogin();
                            break;
                        case 3:
                            AdminLogin();
                            break;
                        case 4:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        // ... (Rest of the code remains the same)

        private static void CreateTables()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string createUsersTableQuery = "CREATE TABLE IF NOT EXISTS Users (Id INT AUTO_INCREMENT PRIMARY KEY, " +
                        "FirstName VARCHAR(50) NOT NULL, LastName VARCHAR(50) NOT NULL, MobileNumber VARCHAR(10) NOT NULL, " +
                        "EmailAddress VARCHAR(100) NOT NULL, Password VARCHAR(100) NOT NULL, Gender CHAR(1) NOT NULL, IsAdmin BOOLEAN)";
                    using (MySqlCommand command = new MySqlCommand(createUsersTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string createUserCartQuery = "CREATE TABLE IF NOT EXISTS UserCart (ProductId INT PRIMARY KEY, ProductName VARCHAR(100) NOT NULL, Quantity INT NOT NULL, Price DECIMAL(10, 2) NOT NULL)";

                    using (MySqlCommand command = new MySqlCommand(createUserCartQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while creating tables: " + ex.Message);
            }
        }

        private static void RegisterUser()
        {
            try
            {
                Console.Write("Enter first name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter last name: ");
                string lastName = Console.ReadLine();

                // Validate and get a valid mobile number
                string mobileNumber;
                do
                {
                    Console.Write("Enter mobile number (10 digits and starts with 7, 8, or 9): ");
                    mobileNumber = Console.ReadLine();
                } while (!IsValidMobileNumber(mobileNumber));

                // Validate and get a valid email address
                string emailAddress;
                do
                {
                    Console.Write("Enter email address: ");
                    emailAddress = Console.ReadLine();
                } while (!IsValidEmail(emailAddress));

                // Validate and get a strong password
                string password;
                do
                {
                    Console.Write("Enter password (e.g. User@123): ");
                    password = Console.ReadLine();
                } while (!IsValidPassword(password));

                // Validate and get a valid gender
                char gender;
                while (true)
                {
                    Console.Write("Enter gender (M/F): ");
                    if (char.TryParse(Console.ReadLine(), out gender) && (gender == 'M' || gender == 'm' || gender == 'F' || gender == 'f'))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input. Enter gender as M/m or F/f.");
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertUserQuery = "INSERT INTO Users (FirstName, LastName, MobileNumber, EmailAddress, Password, Gender, IsAdmin) " +
                                             "VALUES (@firstName, @lastName, @mobileNumber, @emailAddress, @password, @gender, @isAdmin)";
                    using (MySqlCommand command = new MySqlCommand(insertUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@lastName", lastName);
                        command.Parameters.AddWithValue("@mobileNumber", mobileNumber);
                        command.Parameters.AddWithValue("@emailAddress", emailAddress);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@gender", gender);
                        command.Parameters.AddWithValue("@isAdmin", false); // Regular users are not admins

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("User registered successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to register user. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while registering the user: " + ex.Message);
            }
        }

        // Validate email address using a simple regular expression
        private static bool IsValidEmail(string email)
        {
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, emailPattern);
        }

        // Validate the password using a regular expression
        private static bool IsValidPassword(string password)
        {
            // Password must contain at least one uppercase letter, one lowercase letter, one numeric value, and one special character
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$";
            return Regex.IsMatch(password, passwordPattern);
        }

        // Validate mobile number using a simple regular expression
        private static bool IsValidMobileNumber(string mobileNumber)
        {
            // Check if the mobile number matches the pattern: 7, 8, or 9 followed by 9 digits
            string mobilePattern = @"^[789]\d{9}$";
            return Regex.IsMatch(mobileNumber, mobilePattern);
        }


        private static void UserLogin()
        {
            try
            {
                Console.Write("Enter email address: ");
                string emailAddress = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the user exists in the 'Users' table
                    string checkUserQuery = "SELECT IsAdmin FROM Users WHERE EmailAddress = @email";
                    using (MySqlCommand command = new MySqlCommand(checkUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@email", emailAddress);

                        object isAdminObject = command.ExecuteScalar();
                        bool userExists = (isAdminObject != null);

                        if (!userExists)
                        {
                            Console.WriteLine("User does not exist. Please register first.");
                            return; // Exit the method, as the user cannot proceed without registering first
                        }
                    }

                    // If the user exists, proceed with authentication
                    string getUserQuery = "SELECT IsAdmin FROM Users WHERE EmailAddress = @email AND Password = @password";
                    using (MySqlCommand command = new MySqlCommand(getUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@email", emailAddress);
                        command.Parameters.AddWithValue("@password", password);

                        object isAdminObject = command.ExecuteScalar();
                        bool isAdmin = (isAdminObject != null) ? Convert.ToBoolean(isAdminObject) : false;

                        if (isAdmin)
                        {
                            Console.WriteLine("Admin login successful!");
                            ShowAdminMenu();
                        }
                        else
                        {
                            Console.WriteLine("User login successful!");
                            ShowUserMenu();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while logging in: " + ex.Message);
            }
        }

        // ... (Rest of the code remains the same)

        private static void ShowUserMenu()
        {
            try
            {
                bool logout = false;
                while (!logout)
                {
                    Console.WriteLine();
                    Console.WriteLine("---------- User Menu ----------");
                    Console.WriteLine();
                    Console.WriteLine("1. View All Bikes");              
                    Console.WriteLine("2. Book Your Bike Now ");
                    Console.WriteLine("3. View Your Bookings");
                    Console.WriteLine("4. Get Your Bike Delivery");
                    Console.WriteLine("5. Logout");
                    Console.Write("Enter your choice: ");
                    int userChoice = Convert.ToInt32(Console.ReadLine());

                    switch (userChoice)
                    {
                        case 1:
                            ViewProducts();
                            break;
                        case 2:
                            AddToCart();
                            break;
                        case 3:
                            ViewCart();
                            break;
                        case 4:
                            Checkout();
                            break;
                        case 5:
                            logout = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private static void AddToCart()
        {
            Console.Write("Enter the Bike ID you want to Book Your Bike: ");
            int productId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter the quantity you want to add: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {   
                connection.Open();

                // Check if the product exists and get its current available quantity
                string getProductQuery = "SELECT Name, Price, Quantity FROM Products WHERE Id = @productId";
                using (MySqlCommand command = new MySqlCommand(getProductQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string productName = reader.GetString("Name");
                            decimal productPrice = reader.GetDecimal("Price");
                            int availableQuantity = reader.GetInt32("Quantity");

                            if (availableQuantity >= quantity)
                            {
                                // Calculate the total price for the given quantity
                                decimal totalPrice = productPrice * quantity;

                                // Close the data reader before executing the next query
                                reader.Close();

                                // Add the product to the user's cart in the database
                                string addToCartQuery = "INSERT INTO UserCart (ProductId, ProductName, Quantity, Price) " +
                                                        "VALUES (@productId, @productName, @quantity, @totalPrice)";
                                using (MySqlCommand addToCartCommand = new MySqlCommand(addToCartQuery, connection))
                                {
                                    addToCartCommand.Parameters.AddWithValue("@productId", productId);
                                    addToCartCommand.Parameters.AddWithValue("@productName", productName);
                                    addToCartCommand.Parameters.AddWithValue("@quantity", quantity);
                                    addToCartCommand.Parameters.AddWithValue("@totalPrice", totalPrice);

                                    int rowsAffected = addToCartCommand.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        Console.WriteLine("Your Bike Booking process complete successfully...!!!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to Book Your Bike. Please try again.");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Insufficient quantity available. Please enter a lower quantity.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                    }
                }
            }
        }


        private static void ViewCart()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve the items in the user's cart from the database
                string viewCartQuery = "SELECT ProductId, ProductName, Quantity, Price FROM UserCart";
                using (MySqlCommand command = new MySqlCommand(viewCartQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Your Cart:");
                            Console.WriteLine("--------------------------------------------------------------------------------------");
                            Console.WriteLine("| Bike ID |               Bike Name               |  Quantity  |      Price    |");
                            Console.WriteLine("--------------------------------------------------------------------------------------");
                            decimal totalCartPrice = 0;

                            while (reader.Read())
                            {
                                int productId = reader.GetInt32("ProductId");
                                string productName = reader.GetString("ProductName");
                                int quantity = reader.GetInt32("Quantity");
                                decimal totalPrice = reader.GetDecimal("Price") * quantity;

                                Console.WriteLine($"| {productId,-10} | {productName,-40} | {quantity,-10} | Rs.{totalPrice,-10} |");
                                totalCartPrice += totalPrice;
                            }

                            Console.WriteLine("--------------------------------------------------------------------------------------");
                            Console.WriteLine($"Total Cart Price: Rs.{totalCartPrice}");
                        }
                        else
                        {
                            Console.WriteLine("Your cart is empty.");
                        }
                    }
                }
            }
        }


        private static void Checkout()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve the items in the user's cart from the database
                string viewCartQuery = "SELECT ProductId, ProductName, Quantity, Price FROM UserCart";
                List<CartItem> cartItems = new List<CartItem>();
                decimal totalCartPrice = 0;

                using (MySqlCommand command = new MySqlCommand(viewCartQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = reader.GetInt32("ProductId");
                            string productName = reader.GetString("ProductName");
                            int quantity = reader.GetInt32("Quantity");
                            decimal totalPrice = reader.GetDecimal("Price");

                            cartItems.Add(new CartItem(productId, productName, quantity, totalPrice));
                            totalCartPrice += totalPrice * quantity; // Calculate total cart price for each item
                        }
                    }
                }

                // Display cart items and total cart price
                if (cartItems.Count > 0)
                {
                    Console.WriteLine("Your Cart:");
                    Console.WriteLine("--------------------------------------------------------------------------------------");
                    Console.WriteLine("| Bike ID |               Bike Name               |   Quantity |      Price    |");
                    Console.WriteLine("--------------------------------------------------------------------------------------");

                    foreach (var item in cartItems)
                    {
                        Console.WriteLine($"| {item.ProductId,-10} | {item.ProductName,-40} | {item.Quantity,-10} | Rs.{item.TotalPrice,-10} |");
                    }

                    Console.WriteLine("--------------------------------------------------------------------------------------");
                    Console.WriteLine($"Total Cart Price: Rs.{totalCartPrice}");

                    // Clear the user's cart after the checkout
                    string clearCartQuery = "DELETE FROM UserCart";
                    using (MySqlCommand clearCartCommand = new MySqlCommand(clearCartQuery, connection))
                    {
                        int rowsAffected = clearCartCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Checkout completed successfully. Your cart has been cleared.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to complete the checkout process. Please try again.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Your cart is empty. Nothing to checkout.");
                }
            }
        }


        // Helper class to store cart item details
        public class CartItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }

            public CartItem(int productId, string productName, int quantity, decimal totalPrice)
            {
                ProductId = productId;
                ProductName = productName;
                Quantity = quantity;
                TotalPrice = totalPrice;
            }
        }


        private static void AdminLogin()
        {
            try
            {
                Console.Write("Enter admin email address: ");
                string adminUsername = Console.ReadLine();

                Console.Write("Enter admin password: ");
                string adminPassword = Console.ReadLine();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getAdminQuery = "SELECT IsAdmin FROM Users WHERE emailaddress = @emailaddress AND Password = @Password AND IsAdmin = 1";
                    using (MySqlCommand command = new MySqlCommand(getAdminQuery, connection))
                    {
                        command.Parameters.AddWithValue("@emailaddress", adminUsername);
                        command.Parameters.AddWithValue("@Password", adminPassword);

                        object isAdminObject = command.ExecuteScalar();
                        bool isAdmin = (isAdminObject != null) ? Convert.ToBoolean(isAdminObject) : false;

                        if (isAdmin)
                        {
                            Console.WriteLine("Admin login successful!");
                            ShowAdminMenu();
                        }
                        else
                        {
                            Console.WriteLine("Invalid admin credentials. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private static void ShowAdminMenu()
        {
            bool logout = false;
            while (!logout)
            {
                Console.WriteLine();
                Console.WriteLine("---------- Admin Menu ----------");
                Console.WriteLine();
                Console.WriteLine("1. View Bikes");
                Console.WriteLine("2. Add Bike");
                Console.WriteLine("3. Delete Bike");
                Console.WriteLine("4. Logout");
                Console.Write("Enter your choice: ");
                int adminChoice = Convert.ToInt32(Console.ReadLine());

                switch (adminChoice)
                {
                    case 1:
                        ViewProducts();
                        break;
                    case 2:
                        AddProduct();
                        break;
                    case 3:
                        DeleteProduct();
                        break;
                    case 4:
                        logout = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void ViewProducts()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve all products from the 'Products' table
                    string viewProductsQuery = "SELECT Id, Name, Price, Quantity FROM Products";
                    using (MySqlCommand command = new MySqlCommand(viewProductsQuery, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Available Products:");
                                Console.WriteLine("--------------------------------------------------------------------------------------");
                                Console.WriteLine("| Bike ID |               Bike Name               |     Price     |  Quantity  |");
                                Console.WriteLine("--------------------------------------------------------------------------------------");

                                while (reader.Read())
                                {
                                    int productId = reader.GetInt32("Id");
                                    string productName = reader.GetString("Name");
                                    decimal productPrice = reader.GetDecimal("Price");
                                    int productQuantity = reader.GetInt32("Quantity");

                                    Console.WriteLine($"| {productId,-10} | {productName,-40} | Rs.{productPrice,-10} | {productQuantity,-10} |");
                                }

                                Console.WriteLine("--------------------------------------------------------------------------------------");
                            }
                            else
                            {
                                Console.WriteLine("No Bike found in the database.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while viewing Bikes: " + ex.Message);
            }
        }



        private static void AddProduct()
        {
            try
            {
                Console.Write("Enter Bike name: ");
                string productName = Console.ReadLine();

                Console.Write("Enter Bike price: ");
                decimal productPrice = Convert.ToDecimal(Console.ReadLine());

                Console.Write("Enter Bike quantity: ");
                int productQuantity = Convert.ToInt32(Console.ReadLine());

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert the new product into the 'Products' table
                    string addProductQuery = "INSERT INTO Products (Name, Price, Quantity) VALUES (@name, @price, @quantity)";
                    using (MySqlCommand command = new MySqlCommand(addProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", productName);
                        command.Parameters.AddWithValue("@price", productPrice);
                        command.Parameters.AddWithValue("@quantity", productQuantity);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Bike added successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add Bike. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the Bike: " + ex.Message);
            }
        }


        private static void DeleteProduct()
        {
            try
            {
                Console.Write("Enter the Bike ID you want to delete: ");
                int productIdToDelete = Convert.ToInt32(Console.ReadLine());

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the product with the given ID exists in the 'Products' table
                    string checkProductQuery = "SELECT COUNT(*) FROM Products WHERE Id = @productId";
                    using (MySqlCommand checkProductCommand = new MySqlCommand(checkProductQuery, connection))
                    {
                        checkProductCommand.Parameters.AddWithValue("@productId", productIdToDelete);

                        long productCount = (long)checkProductCommand.ExecuteScalar();

                        if (productCount == 0)
                        {
                            Console.WriteLine("Bike not found. Cannot delete.");
                            return;
                        }
                    }

                    // If the product exists, proceed to delete it from the 'Products' table
                    string deleteProductQuery = "DELETE FROM Products WHERE Id = @productId";
                    using (MySqlCommand deleteProductCommand = new MySqlCommand(deleteProductQuery, connection))
                    {
                        deleteProductCommand.Parameters.AddWithValue("@productId", productIdToDelete);

                        int rowsAffected = deleteProductCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Bike deleted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to delete Bike. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the Bike: " + ex.Message);
            }
        }


    }
}
