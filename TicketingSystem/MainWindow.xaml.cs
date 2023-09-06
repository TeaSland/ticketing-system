using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TicketingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MinSummary = 50;
        private const int MaxSummary = 200;
        private const int MaxDescription = 750;
        private const string Teacher = "teacher";
        private const string Student = "student";
        private const string Admin = "admin";
        private readonly MySqlConnection _connection;

        public MainWindow()
        {
            InitializeComponent();
            string connectString = string.Format("Server=nimbus.rangitoto.school.nz;" + "Port=3307;" + "database=student2021130434;" +
                "UID=2021130434;" + "password=TL2003;" + "sslmode=none;" + "charset=utf8mb4;");
            _connection = new MySqlConnection(connectString);
            FillComboBox();
        }

        private void FillComboBox()
        {
            // This fills the comboboxes in the view ticket tab and report ticket tab with the usernames
            var cmd = new MySqlCommand("SELECT username FROM users WHERE occupation='admin'", _connection);
            _connection.Open();
            MySqlDataReader dataRow = cmd.ExecuteReader();
            while (dataRow.Read())
            {
                filterViewComboBox.Items.Add(dataRow[0]).ToString();
                changeStaffComboBox.Items.Add(dataRow[0]).ToString();
            }
            changeStaffComboBox.Text = "";
            changeCategoryComboBox.Text = "";
            viewDescriptionTextBox.Clear();
            _connection.Close();
        }

        private void ViewPasswordCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            // Changes between passwordbox and textbox depending on whether the user wants to see the password
            if (viewPasswordCheckBox.IsChecked == true)
            {
                passwordTextBox.Text = passwordPasswordBox.Password;
                passwordTextBox.Visibility = Visibility.Visible;
                passwordPasswordBox.Visibility = Visibility.Hidden;
            }
            else
            {
                passwordPasswordBox.Password = passwordTextBox.Text;
                passwordTextBox.Visibility = Visibility.Hidden;
                passwordPasswordBox.Visibility = Visibility.Visible;
            }
        }

        private void FillDataGrid()
        {
            string filterString = "";
            if (filterViewComboBox.Text != (string)allViewComboBoxItem.Content)
            {
                // If admin wants to filter a view it will add the statement below.
                filterString = $"WHERE `assigned` = '{filterViewComboBox.Text}'";
            }
            // This is used to fill the datatable with the needed columns, using INNER JOIN and WHERE statements, to get the right data. 
            string mySqlJoin = "SELECT tickets.id_ticket, users.username, users.occupation, details.category," +
                " details.summary, tickets.assigned FROM tickets INNER JOIN users ON tickets.id_user = users.id_user " +
                "INNER JOIN details ON tickets.id_detail = details.id_detail " + filterString;
            var cmd = new MySqlCommand(mySqlJoin, _connection);
            _connection.Open();
            // Fills the data table
            DataTable dataTable = new DataTable();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            dataAdapter.Fill(dataTable);
            ticketDataGrid.DataContext = dataTable;
            // Resets the description textbox, and the 2 comboboxes
            changeStaffComboBox.Text = "";
            changeCategoryComboBox.Text = "";
            viewDescriptionTextBox.Clear();
            _connection.Close();
        }
        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            // Counts the number of users selected, along with the user id and their occupation.
            viewPasswordCheckBox.IsChecked = false;
            string username = usernameTextBox.Text.Replace("'", "\\'").Replace("\"", "\\\"");
            string password = passwordPasswordBox.Password.Replace("'", "\\'").Replace("\"", "\\\"");
            var cmd = new MySqlCommand($"SELECT COUNT(*) AS affected_rows, id_user, occupation FROM users WHERE " +
                $"username='{username}' AND hashed_password=UNHEX(sha2('{password}', 256))", _connection);

            _connection.Open();
            long rowsAffected = (long)cmd.ExecuteScalar();
            // If only 1 row is selected it runs.
            if (rowsAffected == 1)
            {
                MySqlDataReader dataRow = cmd.ExecuteReader();
                dataRow.Read(); // This is fine as there's only one column so we can use this.
                int userId = (int)dataRow[1];
                string occupation = dataRow[2].ToString();
                _connection.Close();

                // If the occupation is admin they are redirected to the viewticket tab item and the datagrid is updated.
                if (occupation == Admin)
                {
                    controlTab.SelectedIndex = 2;
                    viewTicketTabItem.IsEnabled = true;
                    loginTabItem.IsEnabled = false;
                    FillDataGrid();
                }
                // If the occupation is student or teacher, then they are redirected to the report issue item and id_user is stored in the label.
                else if (occupation == Teacher || occupation == Student)
                {
                    controlTab.SelectedIndex = 1;
                    reportIssueTabItem.IsEnabled = true;
                    loginTabItem.IsEnabled = false;
                    userIdLabel.Content = userId;
                }
                else
                {
                    MessageBox.Show("Your occupation isn't specified in the database. Please ask IT staff to update this.");
                }
               
            }
            else
            {
                // If the username/password is not recognized than it will let the user now, and resets the boxes
                MessageBox.Show("Wrong username/password. Please try again.");
                passwordPasswordBox.Password = "";
                passwordTextBox.Text = "";
            }
            _connection.Close();
        }

        private void ReportIssueButtonClick(object sender, RoutedEventArgs e)
        {
            // If the summary is too short/long or description is too long, it will comeup with an error message.
            bool correctNumberCharacters = true;
            if (summaryTextBox.Text.EnumerateRunes().Count() < MinSummary)
            {
                MessageBox.Show($"Please enter a summary of decent length ({MinSummary} characters minimum).");
                correctNumberCharacters = false;
            }
            else if (summaryTextBox.Text.EnumerateRunes().Count() > MaxSummary)
            {
                MessageBox.Show($"Please enter a smaller summary. If you need more space use the description box ({MaxSummary} characters maximum).");
                correctNumberCharacters = false;
            }
            if (enterDescriptionTextBox.Text.EnumerateRunes().Count() > MaxDescription)
            {
                MessageBox.Show($"Please enter a smaller description. You can probably remove some unneseccary words. ({MaxDescription} characters maximum).");
                correctNumberCharacters = false;
            }
            if (correctNumberCharacters)
            {
                // This reports the ticket. Inserts the necessary values into details. Then inserts the values into the ticket table.
                // Then resets the category, summary and description, and gives a message of ticket reported.
                _connection.Open();

                string summary = summaryTextBox.Text.Replace("'", "\\'").Replace("\"", "\\\"");
                string description = enterDescriptionTextBox.Text.Replace("'", "\\'").Replace("\"", "\\\"");
                var cmdInsertDetail = new MySqlCommand($"INSERT INTO `details` (`category`, `summary`, `description`) " +
                    $"VALUES ('{enterCategoryComboBox.Text}', '{summary}', '{description}')", _connection);
                cmdInsertDetail.ExecuteNonQuery();
                if (string.IsNullOrEmpty(description))
                {
                    description = "A description was not entered";
                }
                var cmdGetDetailRow = new MySqlCommand($"SELECT id_detail FROM `details` WHERE category='{enterCategoryComboBox.Text}' AND summary='{summary}' " +
                    $"AND description='{description}'", _connection);
                int detailRow = (int)cmdGetDetailRow.ExecuteScalar();

                var cmdInsertTicket = new MySqlCommand($"INSERT INTO `tickets` (`id_user`, `id_detail`) " +
                    $"VALUES ('{userIdLabel.Content}', {detailRow})", _connection);
                cmdInsertTicket.ExecuteNonQuery();
                _connection.Close();

                MessageBox.Show("Ticket succesfully reported!");
                enterDescriptionTextBox.Text = "";
                summaryTextBox.Text = "";
                enterCategoryComboBox.SelectedIndex = 0;
            }
           
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            // Resets everything is that the new user is introduced to a empty program.
            usernameTextBox.Clear();
            passwordPasswordBox.Clear();
            passwordTextBox.Clear();

            summaryTextBox.Clear();
            enterDescriptionTextBox.Clear();
            changeCategoryComboBox.SelectedIndex = 0;
            filterViewComboBox.SelectedItem = allViewComboBoxItem;

            controlTab.SelectedIndex = 0;
            loginTabItem.IsEnabled = true;
            viewTicketTabItem.IsEnabled = false;
            reportIssueTabItem.IsEnabled = false;
        }

        private void TicketDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ticketDataGrid.SelectedItems.Count > 0)
            {
                // If the user selects a row, it changes the text in the comboboxes and the description in the textbox
                changeCategoryComboBox.Text = (string)((DataRowView)ticketDataGrid.SelectedItems[0])["category"];
                changeStaffComboBox.Text = (string)((DataRowView)ticketDataGrid.SelectedItems[0])["assigned"];
                int ticketId = (int)((DataRowView)ticketDataGrid.SelectedItems[0])["id_ticket"];
                MySqlCommand cmd = new MySqlCommand($"SELECT details.description FROM details, tickets WHERE tickets.id_ticket = {ticketId} AND " +
                    $"tickets.id_detail = details.id_detail", _connection);
                _connection.Open();
                viewDescriptionTextBox.Text = (string)cmd.ExecuteScalar();
                _connection.Close();
            }
        }

        private void FilterViewComboBoxDropDownClosed(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        private void CloseTicketButtonClick(object sender, RoutedEventArgs e)
        {
            if (ticketDataGrid.SelectedItems.Count > 0)
            {
                // If the user presses ticket closed, it deletes the corresponding ticket and detail row.
                int ticketId = (int)((DataRowView)ticketDataGrid.SelectedItems[0])["id_ticket"];
                MySqlCommand cmd = new MySqlCommand($"DELETE tickets, details FROM tickets, details WHERE " +
                    $"tickets.id_ticket = {ticketId} AND tickets.id_detail = details.id_detail", _connection);
                _connection.Open();
                cmd.ExecuteNonQuery();
                _connection.Close();
                FillDataGrid();
                MessageBox.Show("Ticket Closed!");
            }
        }

        private void ApplyChangesButtonClick(object sender, RoutedEventArgs e)
        {
            if (ticketDataGrid.SelectedItems.Count > 0)
            {
                // This changes the category and assigned column to what is in the comboboxes in the selected row.
                int ticketId = (int)((DataRowView)ticketDataGrid.SelectedItems[0])["id_ticket"];
                MySqlCommand cmd = new MySqlCommand($"UPDATE `details`, `tickets` SET details.category = '{changeCategoryComboBox.Text}'," +
                    $" tickets.assigned = '{changeStaffComboBox.Text}' WHERE tickets.id_ticket = {ticketId} AND tickets.id_detail = details.id_detail;", _connection);
                _connection.Open();
                cmd.ExecuteNonQuery();
                _connection.Close();
                FillDataGrid();
            }
        }
    }
}