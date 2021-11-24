using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Client
{
    public partial class JoinWindow : Window
    {
        public JoinWindow()
        {
            InitializeComponent();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Contains(" "))
            {
                MessageBox.Show("You cant have a spaced name.");
            }
            else
            {
                Information._IPAdress = txtIP.Text;
                Information._UserName = txtName.Text;
                MainWindow mw = new MainWindow();
                mw.Show();
            }
        }

        private void btnHost_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Server_code.exe");
        }
    }
    public static class Information
    {
        public static string _IPAdress = "";
        public static string _UserName = "";
    }
}
