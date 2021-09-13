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

namespace Scramble_Encryption
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();

        }//Initialized

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {

            //Goes to the Encryption Window
            Encrypt EncryptWindow = new Encrypt();
            EncryptWindow.Show();
            Close();

        }//Clicked Encrypt

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {

            //Goes to the Decryption Window
            Decrypt DecryptWindow = new Decrypt();
            DecryptWindow.Show();
            Close();

        }//Clicked Decrypt

    }

}
