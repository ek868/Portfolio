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
using System.Windows.Shapes;

namespace NargesLogs
{

    public partial class Home_Page : Window
    {

        public Home_Page()
        {

            InitializeComponent();

        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {

            //Opens the Patient details window on the current position, closes the current window, and sets new patient to true.
            CurrentPatient.NewPatient = true;
            Patient_Details pd = new Patient_Details();
            pd.Owner = this;
            pd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            pd.Show();
            pd.Owner = null;
            Misc.WindowSwitch = true;
            Close();

        }

        private void btnPatientList_Click(object sender, RoutedEventArgs e)
        {

            //Opens the patients list window on the same postition as the current window and closes this window.
            Patients_List pl = new Patients_List();
            pl.Owner = this;
            pl.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            pl.Show();
            pl.Owner = null;
            Misc.WindowSwitch = true;
            Close();

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

            //Opens the login screen on the same position as this screen and closes this screen.
            MainWindow mw = new MainWindow();
            mw.Owner = this;
            mw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mw.Show();
            mw.Owner = null;
            Close();

        }

        private void Window_Closed(object sender, EventArgs e)
        {

            //Clears the temporary folders and properly disconnects from the server if the window is closed.
            PublicFunctions.Functionality.ClearTempFiles();

            if(Misc.WindowSwitch == false)
            {

                ConnectionFunctions CFunctions = new ConnectionFunctions();
                CFunctions.CloseConnection();

            }

            else
                Misc.WindowSwitch = false;

        }

    }
    
}
