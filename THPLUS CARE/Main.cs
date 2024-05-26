using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using THPLUS_CARE.Controller;

namespace THPLUS_CARE
{
    public partial class Main : Form
    {
       private string connectionString = "server=34.142.182.120;user=fblike;password=dACo8SuD29d3UNZJ3;database=fblike";
       private MySqlConnection connection;
        List<FacebookAccount> accounts = new List<FacebookAccount>();

        public Main()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string profilePath = @"C:\THCARE\Profile";

            // Create ChromeOptions

            // Set the path to the new profile
            // Create a new instance of the ChromeDriver
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddArgument("user-data-dir=" + profilePath);

            chromeOptions.AddArgument("start-maximized"); // Maximize the browser window
            IWebDriver driver = new ChromeDriver(chromeOptions);

            
            // Navigate to a webpage
            driver.Navigate().GoToUrl("https://www.facebook.com/");
            /*
            // Find the input field by its ID (replace "inputFieldId" with the actual ID of your input field)
            IWebElement email = driver.FindElement(By.Name("email"));

            // Clear the input field (optional)
            email.Clear();

            // Set the text in the input field
            email.SendKeys("61560248636385");
           

            IWebElement password = driver.FindElement(By.Id("pass"));

            // Clear the input field (optional)
            password.Clear();

            // Set the text in the input field
            password.SendKeys("8DWbYp736717");


            IWebElement button = driver.FindElement(By.Name("login"));

            // Click the button
            button.Click();
            // Get and print the page title
            Console.WriteLine("Page title: " + driver.Title);

            // Close the browser
        //    driver.Quit();
            */
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Create a MySqlConnection object
            this.connection = new MySqlConnection(this.connectionString);
            
                try
                {
                    // Open the connection
                    this.connection.Open();
                    if (this.connection != null && this.connection.State == ConnectionState.Open)
                    {
                        Console.WriteLine("CONNECT DB");

                    }
                    // SQL query
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            
        }

        private void loinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected row
            DataGridViewRow selectedRow = dataGridView1.SelectedRows.Count > 0 ? dataGridView1.SelectedRows[0] : null;
            Console.WriteLine("CHECK row data:", dataGridView1.SelectedRows.Count);

            if (selectedRow != null)
            {
                // Retrieve data from the selected row
                // Assuming the DataGridView is bound to a DataSource and each row represents an object of a custom class

                FacebookSE.Login(selectedRow.Cells["UID"].Value.ToString(), selectedRow.Cells["PASSWORD"].Value.ToString());

                // Now you have the data from the selected row, you can use it as needed
                Console.WriteLine("Selected row data:");
                Console.WriteLine("Column1: " + selectedRow.Cells["UID"].Value.ToString());
                Console.WriteLine("Column2: " + selectedRow.Cells["PASSWORD"].Value.ToString());
                // Add more properties as needed
            }
            else
            {
                // No row is selected, cancel the opening of the ContextMenuStrip
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadData();
        }
        private void loadData()
        {


            // SQL query
            string query = "SELECT * FROM facebook_accounts ORDER BY id desc";

            // Create a MySqlCommand object
            using (MySqlCommand command = new MySqlCommand(query, this.connection))
            {
                // Execute the query and get a data reader
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Check if the reader has any rows
                    if (reader.HasRows)
                    {
                        dataGridView1.Rows.Clear();
                        // Read each row
                        while (reader.Read())
                        {
                            // Example: Retrieve data from columns "column1" and "column2"

                            dataGridView1.Rows.Add(reader.GetInt64("id"), reader.GetString("fbuid"), reader.GetString("password"), reader.GetString("firstname"), reader.GetString("lastname"), reader.GetBoolean("lived"), reader.GetInt64("farm"));

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;

            // SQL query
            string query = "SELECT * FROM facebook_accounts WHERE farm=0  ORDER BY id desc";

            // Create a MySqlCommand object
            using (MySqlCommand command = new MySqlCommand(query, this.connection))
            {
                // Execute the query and get a data reader
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Check if the reader has any rows
                    if (reader.HasRows)
                    {
                        dataGridView1.Rows.Clear();
                        // Read each row
                        while (reader.Read())
                        {

                            FacebookSE.Login(reader.GetString("fbuid"), reader.GetString("password"));

                            FacebookAccount account = new FacebookAccount
                            {
                                Id = reader.GetInt32("id"),
                                FbUid = reader.GetString("fbuid"),
                                Password = reader.GetString("password"),
                                FirstName = reader.GetString("firstname"),
                                LastName = reader.GetString("lastname"),
                                Lived = reader.GetBoolean("lived")
                            };

                            // Add the account object to the list
                            accounts.Add(account);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }

            }
            // Output the retrieved data
            foreach (var account in accounts)
            {
                UpdateData(account.Id);
                Console.WriteLine($"ID: {account.Id}, FBUID: {account.FbUid}, Password: {account.Password}, Firstname: {account.FirstName}, Lastname: {account.LastName}, Lived: {account.Lived}");
            }

        }
        private void UpdateData(int idToUpdate)
        {
            // SQL query to update data
            string updateQuery = "UPDATE facebook_accounts SET farm = @value1 WHERE id = @id";

            // Create a MySqlCommand object for updating data
            using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, this.connection))
            {
                // Add parameters to the command
                updateCommand.Parameters.AddWithValue("@value1", 1);
                updateCommand.Parameters.AddWithValue("@id", idToUpdate);

                // Execute the UPDATE query
                int rowsAffected = updateCommand.ExecuteNonQuery();

                // Output the number of rows affected
                Console.WriteLine($"Rows affected by update: {rowsAffected}");
            }
        }
        class FacebookAccount
        {
            public int Id { get; set; }
            public string FbUid { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public bool Lived { get; set; }
        }
    }
    }

