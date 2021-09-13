using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;

namespace NargesLogs
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();
            
        }

        private void txtPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            
            //See function description.
            PublicFunctions.Responsivity.HighlightTextBox(txtPassword);

        }

        private void txtPassword_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.UnhighlightTextBox(txtPassword);

        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {

            //Ensures that the password is not displayed on screen.
            pwdPassword.Visibility = Visibility.Visible;
            pwdPassword.Focus();
            txtPassword.IsEnabled = false;

            Panel.SetZIndex(txtPassword, 0);
            Panel.SetZIndex(pwdPassword, 1);

        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Resets the objects to their default forms.
            if(pwdPassword.Password == "")
            {

                pwdPassword.Visibility = Visibility.Hidden;
                txtPassword.IsEnabled = true;

                Panel.SetZIndex(txtPassword, 1);
                Panel.SetZIndex(pwdPassword, 0);

            }

            PublicFunctions.Responsivity.DeselectTextBox(txtUserName, "[Enter User Name]", btnLogin);
            btnLogin.Focus();

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            //Attempts to connect to the server based on specified connection settings.
            ConnectionFunctions CFunctions = new ConnectionFunctions();

            try
            {

                Custom_Classes.Tcp.Connect();

            }

            catch
            {

                MessageBox.Show("Failed to establish a connection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            //Comapares Usernames and Passwords with the entered information to ensure that the user used the correct login credentials.
            RetrievedInfo.usernames = CFunctions.SendGenericCommand("3214~COMMAND#1");
            
            RetrievedInfo.passwords = CFunctions.SendGenericCommand("3214~COMMAND#2");
            try
            {

                int usernameused = Array.IndexOf(RetrievedInfo.usernames, txtUserName.Text);
                if (pwdPassword.Password == RetrievedInfo.passwords[usernameused]) { }

                else
                {

                    MessageBox.Show("Incorrect Username or Password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    CFunctions.CloseConnection();
                    return;

                }

            }

            catch
            {

                MessageBox.Show("Incorrect Username or Password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CFunctions.CloseConnection();
                return;

            }

            //If the process is successful, the home page is openeed on the same position as the current page and this page is closed.
            Home_Page hp = new Home_Page();
            hp.Owner = this;
            hp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            hp.Show();
            hp.Owner = null;
            Close();

        }

        private void txtUserName_GotFocus(object sender, RoutedEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.SelectTextBox(txtUserName, "[Enter User Name]");

        }

        private void txtUserName_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtUserName);

        }

        private void txtUserName_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtUserName);

        }

        private void Window_Closed(object sender, EventArgs e)
        {

            //Clears any remaining temporary files.
            PublicFunctions.Functionality.ClearTempFiles();

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

            //Opens the settings screen.
            Settings sw = new Settings();
            sw.Show();

        }

        private void txtUserName_KeyUp(object sender, KeyEventArgs e)
        {

            //Checks the input to see if something has been enetered into both the username and password box, and whether the login button should be enabled.
            if (txtUserName.Text == "" || txtUserName.Text == "[Enter User Name]" || pwdPassword.Password == "")
                PublicFunctions.Functionality.DisableButton(btnLogin);

            if (txtUserName.Text != "" && txtUserName.Text != "[Enter User Name]" && pwdPassword.Password != "")
                PublicFunctions.Functionality.EnableButton(btnLogin);

        }

        private void pwdPassword_KeyUp(object sender, KeyEventArgs e)
        {

            //Checks the input of the password box to ensure that something has been entered into both the username and password box, and whether the login button should be enabled.
            if (txtUserName.Text == "" || txtUserName.Text == "[Enter User Name]" || pwdPassword.Password == "")
                PublicFunctions.Functionality.DisableButton(btnLogin);

            if (txtUserName.Text != "" && txtUserName.Text != "[Enter User Name]" && pwdPassword.Password != "")
            {

                PublicFunctions.Functionality.EnableButton(btnLogin);
                if (e.Key.ToString() == "Return")
                    btnLogin_Click(this, null);

            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Ensures that the login button cannot be pressed if no details have been entered.
            PublicFunctions.Functionality.DisableButton(btnLogin);

        }
    }

}
