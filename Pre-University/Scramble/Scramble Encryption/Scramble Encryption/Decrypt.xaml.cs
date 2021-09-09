using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Scramble_Encryption
{
    /// <summary>
    /// Interaction logic for Decrypt.xaml
    /// </summary>
    public partial class Decrypt : Window
    {
        public Decrypt()
        {
            InitializeComponent();
        }// Initializes

        bool ToolTipsEnabled = false;

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            if (ToolTipsEnabled == false)
            {
                btnBack.ToolTip = "Returns to the selection menu";
                txtDropFilesHere.ToolTip = "Click to add files by selecting a directory";
                btnHelp.ToolTip = "Disables tooltips";
                txtDestination.ToolTip = "Opens a directory for selecting a location to save the encrypted file into";
                txbPassword.ToolTip = "The password that is used to encrypt the file(s)";
                txbBruteKey.ToolTip = "Provides extra security by expanding file size with fake bytes";
                btnDecrypt.ToolTip = "Begins the encryption process";
                ToolTipsEnabled = true;
                MessageBox.Show("Tooltips have been enabled.", "Tooltips Enabled");
            }
            else
            {
                btnBack.ToolTip = null;
                txtDropFilesHere.ToolTip = null;
                btnHelp.ToolTip = "Enables tooltips";
                txtDestination.ToolTip = null;
                txbPassword.ToolTip = null;
                txbBruteKey.ToolTip = null;
                btnDecrypt.ToolTip = null;
                ToolTipsEnabled = false;
                MessageBox.Show("Tooltips have been disabled", "Tooltips Disabled");
            }
        }// Click Help

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }// Click Back

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txbPassword.Text == "")
            {
                txbPassword.Text = "[Enter Password]";
                txbPassword.TextAlignment = TextAlignment.Center;
                txbPassword.FontWeight = FontWeights.UltraLight;
            }
            if (txbBruteKey.Text == "")
            {
                txbBruteKey.Text = "[Anti-Brute Force Key]";
                txbBruteKey.TextAlignment = TextAlignment.Center;
                txbBruteKey.FontWeight = FontWeights.UltraLight;
            }
            btnBack.Focus();
        }// Clicks Away

        //Drag and Drop Functionality
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool DroptextExists = true;
        bool btnDecryptIsEnabled = false;

        private void lsvDnD_MouseEnter(object sender, MouseEventArgs e)
        {
            lsvDnD.Background = Brushes.PaleGreen;
            lsvDnD.Cursor = Cursors.Hand;
        }// Mouse Enter

        private void lsvDnD_MouseLeave(object sender, MouseEventArgs e)
        {
            lsvDnD.Background = Brushes.White;
            lsvDnD.Cursor = Cursors.Arrow;
        }// Mouse Leave

        private void lsvDnD_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (var path in droppedFilePaths)
                {
                    FileInfo fi = new FileInfo(path);
                    FilesToDecrypt newfile = new FilesToDecrypt();
                    newfile.FileName = fi.Name;
                    FileStream SizeChecker = fi.OpenRead();
                    newfile.Size = (SizeChecker.Length / 1000).ToString() + "KB";
                    newfile.FilePath = fi.FullName;
                    newfile.Extention = fi.Extension;
                    lsvDnD.Items.Add(newfile);
                    SizeChecker.Close();
                }
                txbPassword.IsEnabled = true;
                txbBruteKey.IsEnabled = true;
                txtDestination.IsEnabled = true;
                txtDestination.Background = Brushes.White;
                txtDestination.Foreground = Brushes.Green;
                btnDecryptIsEnabled = true;
                btnDecrypt.IsHitTestVisible = true;
                btnDecrypt.Background = Brushes.IndianRed;
                btnDecrypt.Foreground = Brushes.White;
                btnDecrypt.BorderBrush = Brushes.White;
            }
            Panel.SetZIndex(imgPlusFile, 0);
            Panel.SetZIndex(lsvDnD, 1);
            DroptextExists = false;
        }// Drop File

        private void lsvDnD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.ShowDialog();
                FilesToDecrypt newfile = new FilesToDecrypt();
                newfile.FileName = OpenFile.SafeFileName;
                FileStream SizeChecker = File.OpenRead(OpenFile.FileName);
                newfile.Size = (SizeChecker.Length / 4).ToString() + "KB";
                newfile.FilePath = OpenFile.FileName;
                newfile.Extention += OpenFile.AddExtension;
                lsvDnD.Items.Add(newfile);
                Panel.SetZIndex(txtDropFilesHere, 0);
                DroptextExists = false;
                SizeChecker.Close();
                txbPassword.IsEnabled = true;
                txbBruteKey.IsEnabled = true;
                txtDestination.IsEnabled = true;
                txtDestination.Background = Brushes.White;
                txtDestination.Foreground = Brushes.Green;
                btnDecryptIsEnabled = true;
                btnDecrypt.IsHitTestVisible = true;
                btnDecrypt.Background = Brushes.IndianRed;
                btnDecrypt.Foreground = Brushes.White;
                btnDecrypt.BorderBrush = Brushes.White;
            }
            catch
            {

            }
        }// Click

        private void lsvDnD_DragEnter(object sender, DragEventArgs e)
        {
            lsvDnD.Background = Brushes.PaleGreen;
            Panel.SetZIndex(imgPlusFile, 2);
            Panel.SetZIndex(txtDropFilesHere, 0);
            Panel.SetZIndex(lsvDnD, 1);
        }// Enter w/ File

        private void lsvDnD_DragLeave(object sender, DragEventArgs e)// Leave w/ File
        {
            lsvDnD.Background = Brushes.White;
            Panel.SetZIndex(imgPlusFile, 0);
            if (DroptextExists == true)
                Panel.SetZIndex(txtDropFilesHere, 1);
            Panel.SetZIndex(lsvDnD, 1);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Destination Functionality
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void txtDestination_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtDestination.Text = "";
            txtDestination.TextAlignment = TextAlignment.Left;
            txtDestination.FontWeight = FontWeights.Normal;

            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.ShowDialog();
            txtDestination.Text = SaveFile.FileName;
            if (txtDestination.Text == "")
            {
                txtDestination.Text = "[Select Save File Path]";
            }
        }// Click Destination

        private void txtDestination_MouseEnter(object sender, MouseEventArgs e)
        {
            txtDestination.Background = Brushes.PaleGreen;
            Cursor = Cursors.Hand;
        }// Mouse Enters Destination

        private void txtDestination_MouseLeave(object sender, MouseEventArgs e)
        {
            txtDestination.Background = Brushes.White;
            Cursor = Cursors.Arrow;
        }// Mouse Leaves Destination
         ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Brute-Force Functionality
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void txbBruteKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "\\d+") && e.Key != Key.Back)
                e.Handled = true;
        }

        private void txbBruteKey_GotFocus(object sender, RoutedEventArgs e)
        {
            txbBruteKey.Text = "";
            txbBruteKey.TextAlignment = TextAlignment.Left;
            txbBruteKey.FontWeight = FontWeights.Normal;
        }// Click Brute-Force

        private void txbBruteKey_MouseEnter(object sender, MouseEventArgs e)
        {
            txbBruteKey.Background = Brushes.PaleGreen;
        }// Enter Brute-Force

        private void txbBruteKey_MouseLeave(object sender, MouseEventArgs e)
        {
            txbBruteKey.Background = Brushes.White;
        }// Leave Brute-Force
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Password Functionality
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void txbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txbPassword.Text = "";
            txbPassword.TextAlignment = TextAlignment.Left;
            txbPassword.FontWeight = FontWeights.Normal;
        }// Click Password

        private void txbPassword_MouseEnter(object sender, MouseEventArgs e)// Enter Password
        {
            txbPassword.Background = Brushes.PaleGreen;
        }

        private void txbPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            txbPassword.Background = Brushes.White;
        }// Leave Password
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            bool Error = false;
            if (btnDecryptIsEnabled == false)
                return;
            bool noDestination = false;
            string destination = txtDestination.Text;
            string password = txbPassword.Text;
            double key = 0;

            try
            {
                key = double.Parse(txbBruteKey.Text);
            }
            catch
            {
            }


            if(destination == "[Select Save File Path]")
            {
                noDestination = true;
                MessageBoxResult msg = MessageBox.Show("You have not selected a save file path, do you want to replace the existing files?", "File Replacement", MessageBoxButton.OKCancel);
                if (msg != MessageBoxResult.OK)
                {
                    return;
                }
            }

            else if(lsvDnD.Items.Count >= 2)
            {
                Directory.CreateDirectory(destination);
            }

            if (password == "[Enter Password]")
            {
                password = "MyKey123";
            }

            foreach(FilesToDecrypt i in lsvDnD.Items)
            {

                if (noDestination == true)
                {
                    destination = i.FilePath;
                }

                FileStream OpenFile = File.OpenRead(i.FilePath);
                byte[] file = new byte[OpenFile.Length];
                OpenFile.Read(file, 0, file.Length);

                byte[] efile = AntiBruteForce(file, key);
                byte[] sfile = DecryptMain(efile, password);
                byte[] rfile = Split(sfile, password);

                OpenFile.Close();

                if (noDestination == true)
                {
                    try
                    {
                        File.Delete(destination);
                        FileStream SaveFile = File.OpenWrite(destination );
                        SaveFile.Write(rfile, 0, rfile.Length);
                        SaveFile.Close();
                    }
                    catch
                    {
                        MessageBox.Show(i.FileName + " could not be encrypted because the program does not have administrator privileges.", "Encryption Failed");
                        Error = true;
                    }

                }

                else if(lsvDnD.Items.Count >= 2)
                {
                    try
                    {
                        FileStream SaveFile = File.OpenWrite(destination + @"\" + i.FileName + i.Extention);
                        SaveFile.Write(rfile, 0, rfile.Length);
                        SaveFile.Close();
                    }
                    catch
                    {
                        MessageBox.Show(i.FileName + " could not be encrypted because the program does not have administrator privileges.", "Encryption Failed");
                        Error = true;
                    }
                }
                else
                {
                    try
                    {
                        FileStream SaveFile = File.OpenWrite(destination + i.Extention);
                        SaveFile.Write(rfile, 0, rfile.Length);
                        SaveFile.Close();
                    }
                    catch
                    {
                        MessageBox.Show(i.FileName + " could not be encrypted because the program does not have administrator privileges.", "Encryption Failed");
                        Error = true;
                    }
                }
            }
            if(Error == false)
                MessageBox.Show("Decryption Completed Successfully");
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }// Click Decrypt

        //Decryption
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private byte[] AntiBruteForce(byte[] file, double key)
        {

            char[] passarray = key.ToString().ToCharArray();
            int remainder = passarray.Length % 2;

            char[] keychar1 = new char[(passarray.Length + remainder) / 2];
            char[] keychar2 = new char[(passarray.Length - remainder) / 2];

            for(int i = 0; i < keychar2.Length; i++)
            {
                keychar1[i] = passarray[i];
                keychar2[i] = passarray[keychar1.Length + i];
            }

            for (int i = 0; i < remainder; i++)
            {
                keychar1[keychar2.Length + i] = passarray[keychar2.Length + i];
            }

            string keystring1 = new string(keychar1);
            string keystring2 = new string(keychar2);

            if (keystring2 == "")
            {
                keystring2 = "0";
            }

            int key1 = int.Parse(keystring1);
            int key2 = int.Parse(keystring2);

            byte[] bfile = new byte[file.Length - (key1 + key2)];

            for (int i = 0; i < bfile.Length; i++)
            {
                bfile[i] = file[key1 + i];
            }

            return bfile;
        }// Undo Stage 3: Take Off Fake Anti-Brute Force Bytes

        private byte[] DecryptMain(byte[] file, string password)
        {

            int keypos = 0;
            int pastkeys = 0;

            byte[] keyarray = PassConverter(password);

            byte[] originalfile = new byte[file.Length];
            for (int i = 0; i < file.Length; i++)
            {
                originalfile[i] = file[i];
            }

            for (int i = 0; i < file.Length; i++)
            {
                if (i != 0)
                    pastkeys += i;
                file[i] -= (byte)((keyarray[keypos] * (i + 1)) - pastkeys);
                keypos++;
                if (keypos == keyarray.Length)
                {
                    keypos = 0;
                }
            }

            return file;
        }// Undo Stage 2: Decrypt Whole File

        private byte[] Split(byte[] file, string password)
        {

            int remainder = file.Length % 4;

            byte[] file1 = new byte[(file.Length - remainder) / 4];
            byte[] file2 = new byte[file1.Length];
            byte[] file3 = new byte[file1.Length];
            byte[] file4 = new byte[file1.Length + remainder];

            for (int i = 0; i < file1.Length; i++)
            {
                file1[i] = file[i];
                file2[i] = file[(file1.Length) + i];
                file3[i] = file[2 * (file1.Length) + i];
                file4[i] = file[3 * (file1.Length) + i];
            }

            for (int i = 0; i < remainder; i++)
            {
                file4[file1.Length + i] = file[4 * (file1.Length) + i];
            }

            byte[] efile1 = DecryptMain(file1, password);
            byte[] efile2 = DecryptMain(file2, password);
            byte[] efile3 = DecryptMain(file3, password);
            byte[] efile4 = DecryptMain(file4, password);
            byte[] efile = new byte[file.Length];

            for (int i = 0; i < file1.Length; i++)
            {
                efile[i] = efile1[i];
                efile[file1.Length + i] = efile2[i];
                efile[2 * (file1.Length) + i] = efile3[i];
                efile[3 * (file1.Length) + i] = efile4[i];
            }

            for (int i = 0; i < remainder; i++)
            {
                efile[4 * (file1.Length) + i] = efile4[file1.Length + i];
            }

            return efile;
        }// Undo Stage 1: Split Files, Decrypt, and Put Back together

        public byte[] PassConverter(string password)
        {
            Char[] passarray = password.ToCharArray();
            byte[] keyarray = new byte[passarray.Length];

            for (int i = 0; i < passarray.Length; i++)
            {
                if (passarray[i] == 'a')
                    keyarray[i] = 0;
                else if (passarray[i] == 'b')
                    keyarray[i] = 1;
                else if (passarray[i] == 'c')
                    keyarray[i] = 2;
                else if (passarray[i] == 'd')
                    keyarray[i] = 3;
                else if (passarray[i] == 'e')
                    keyarray[i] = 4;
                else if (passarray[i] == 'f')
                    keyarray[i] = 5;
                else if (passarray[i] == 'g')
                    keyarray[i] = 6;
                else if (passarray[i] == 'h')
                    keyarray[i] = 7;
                else if (passarray[i] == 'i')
                    keyarray[i] = 8;
                else if (passarray[i] == 'j')
                    keyarray[i] = 9;
                else if (passarray[i] == 'k')
                    keyarray[i] = 10;
                else if (passarray[i] == 'l')
                    keyarray[i] = 11;
                else if (passarray[i] == 'm')
                    keyarray[i] = 12;
                else if (passarray[i] == 'n')
                    keyarray[i] = 13;
                else if (passarray[i] == 'o')
                    keyarray[i] = 14;
                else if (passarray[i] == 'p')
                    keyarray[i] = 15;
                else if (passarray[i] == 'q')
                    keyarray[i] = 16;
                else if (passarray[i] == 'r')
                    keyarray[i] = 17;
                else if (passarray[i] == 's')
                    keyarray[i] = 18;
                else if (passarray[i] == 't')
                    keyarray[i] = 19;
                else if (passarray[i] == 'u')
                    keyarray[i] = 20;
                else if (passarray[i] == 'v')
                    keyarray[i] = 21;
                else if (passarray[i] == 'w')
                    keyarray[i] = 22;
                else if (passarray[i] == 'x')
                    keyarray[i] = 23;
                else if (passarray[i] == 'y')
                    keyarray[i] = 24;
                else if (passarray[i] == 'z')
                    keyarray[i] = 25;
                else if (passarray[i] == 'A')
                    keyarray[i] = 26;
                else if (passarray[i] == 'B')
                    keyarray[i] = 27;
                else if (passarray[i] == 'C')
                    keyarray[i] = 28;
                else if (passarray[i] == 'D')
                    keyarray[i] = 29;
                else if (passarray[i] == 'E')
                    keyarray[i] = 30;
                else if (passarray[i] == 'F')
                    keyarray[i] = 31;
                else if (passarray[i] == 'G')
                    keyarray[i] = 32;
                else if (passarray[i] == 'H')
                    keyarray[i] = 33;
                else if (passarray[i] == 'I')
                    keyarray[i] = 34;
                else if (passarray[i] == 'J')
                    keyarray[i] = 35;
                else if (passarray[i] == 'K')
                    keyarray[i] = 36;
                else if (passarray[i] == 'L')
                    keyarray[i] = 37;
                else if (passarray[i] == 'M')
                    keyarray[i] = 38;
                else if (passarray[i] == 'N')
                    keyarray[i] = 39;
                else if (passarray[i] == 'O')
                    keyarray[i] = 40;
                else if (passarray[i] == 'P')
                    keyarray[i] = 41;
                else if (passarray[i] == 'Q')
                    keyarray[i] = 42;
                else if (passarray[i] == 'R')
                    keyarray[i] = 43;
                else if (passarray[i] == 'S')
                    keyarray[i] = 44;
                else if (passarray[i] == 'T')
                    keyarray[i] = 45;
                else if (passarray[i] == 'U')
                    keyarray[i] = 46;
                else if (passarray[i] == 'V')
                    keyarray[i] = 47;
                else if (passarray[i] == 'W')
                    keyarray[i] = 48;
                else if (passarray[i] == 'X')
                    keyarray[i] = 49;
                else if (passarray[i] == 'Y')
                    keyarray[i] = 50;
                else if (passarray[i] == 'Z')
                    keyarray[i] = 51;
                else if (passarray[i] == '0')
                    keyarray[i] = 52;
                else if (passarray[i] == '1')
                    keyarray[i] = 53;
                else if (passarray[i] == '2')
                    keyarray[i] = 54;
                else if (passarray[i] == '3')
                    keyarray[i] = 55;
                else if (passarray[i] == '4')
                    keyarray[i] = 56;
                else if (passarray[i] == '5')
                    keyarray[i] = 57;
                else if (passarray[i] == '6')
                    keyarray[i] = 58;
                else if (passarray[i] == '7')
                    keyarray[i] = 59;
                else if (passarray[i] == '8')
                    keyarray[i] = 60;
                else if (passarray[i] == '9')
                    keyarray[i] = 61;
            }
            return keyarray;
        }// Index of keys for each letter

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
    class FilesToDecrypt
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Size { get; set; }
        public string Extention { set; get; }
    }
}
