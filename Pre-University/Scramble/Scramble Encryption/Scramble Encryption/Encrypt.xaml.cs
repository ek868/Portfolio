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
    public partial class Encrypt : Window
    {
        bool ToolTipsEnabled = false;
        public Encrypt()
        {
            InitializeComponent();
        } //Initialized

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
        } //Clicked away

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        } //Clicked Back

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
                btnEncrypt.ToolTip = "Begins the encryption process";
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
                btnEncrypt.ToolTip = null;
                ToolTipsEnabled = false;
                MessageBox.Show("Tooltips have been enabled.", "Tooltips Disabled");
            }

        } //Clicked Help

        bool btnEncryptIsEnabled = false;

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            bool Error = false;

            if (btnEncryptIsEnabled == false)
            {
                return;
            }
            string destination = txtDestination.Text;
            string password = txbPassword.Text;
            double bruteKey = 0;
            try
            {
                bruteKey = double.Parse(txbBruteKey.Text);
            }
            catch
            {
            }

            bool noDestination = false;

            if (txtDestination.Text == "[Select Save File Path]")
            {
                noDestination = true;
            }
            else if (lsvDnD.Items.Count >= 2)
            {
                Directory.CreateDirectory(destination);
            }

            if (password == "[Enter Password]")
            {
                password = "MyKey123";
            }

            foreach (Files i in lsvDnD.Items)
            {
               
                if(noDestination == true)
                {
                    destination = i.FilePath;
                    MessageBoxResult msg = MessageBox.Show("You have not selected a save file path, do you want to replace the existing files?", "File Replacement", MessageBoxButton.OKCancel);
                    if (msg != MessageBoxResult.OK)
                    {
                        return;
                    }
                }

                FileStream OpenFile = File.OpenRead(i.FilePath);
                byte[] rfile = new byte[OpenFile.Length];
                OpenFile.Read(rfile, 0, rfile.Length);

                byte[] sfile = Split(rfile, password);
                byte[] efile = ExtraEncryption(sfile, password);
                byte[] file = BruteForce(efile, bruteKey);

                OpenFile.Close();

                if(noDestination == true)
                {
                    try
                    {
                        FileStream WriteFile = File.OpenWrite(destination);
                        WriteFile.Write(file, 0, file.Length);
                        WriteFile.Close();
                    }
                    catch
                    {
                        MessageBox.Show(i.FileName + " could not be encrypted because the program does not have administrator privileges.", "Encryption Failed");
                        Error = true;
                    }
                }
                else if (lsvDnD.Items.Count >= 2)
                {
                    try
                    {
                        FileStream WriteFile = File.OpenWrite(destination + @"\" + i.FileName + i.Extention);
                        WriteFile.Write(file, 0, file.Length);
                        WriteFile.Close();
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
                        FileStream WriteFile = File.OpenWrite(destination + i.Extention);
                        WriteFile.Write(file, 0, file.Length);
                        WriteFile.Close();
                    }
                    catch
                    {
                        MessageBox.Show(i.FileName + " could not be encrypted because the program does not have administrator privileges.", "Encryption Failed");
                        Error = true;
                    }
                }

            }
            if(Error == false)
                MessageBox.Show("Encryption was completed successfully.", "Ecryption Completed");
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        } //Clicked Encrypt

        

        //Encryption
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public byte[] Split(byte[] file, string password)
        {

            int remainder = file.Length % 4;

            byte[] file1 = new byte[(file.Length - remainder) / 4];
            byte[] file2 = new byte[file1.Length];
            byte[] file3 = new byte[file1.Length];
            byte[] file4 = new byte[file1.Length + remainder];

            for(int i = 0; i < file1.Length; i++)
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

            byte[] efile1 = ExtraEncryption(file1, password);
            byte[] efile2 = ExtraEncryption(file2, password);
            byte[] efile3 = ExtraEncryption(file3, password);
            byte[] efile4 = ExtraEncryption(file4, password);
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
        } // Stage 1: Split and Encrypt Individually

        public byte[] ExtraEncryption(byte[] file, string password)
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
                file[i] += (byte)((keyarray[keypos] * (i + 1)) - pastkeys);
                keypos++;
                if (keypos == keyarray.Length)
                {
                    keypos = 0;
                }
            }

            return file;
        } // Stage 2: Encrypt Entire File

        public byte[] BruteForce(byte[] file, double key)
        {

            char[] keychar = key.ToString().ToCharArray();
            int remainder = keychar.Length % 2;

            char[] keychar1 = new char[(keychar.Length + remainder) / 2]; 
            char[] keychar2 = new char[(keychar.Length - remainder) / 2];

            for (int i = 0; i < keychar2.Length; i++)
            {
                keychar1[i] = keychar[i];
                keychar2[i] = keychar[keychar1.Length + i];
            }

            for (int i = 0; i < remainder; i++)
            {
                keychar1[keychar2.Length + i] = keychar[keychar2.Length + i];
            }

            string keystring1 = new string(keychar1);
            string keystring2 = new string(keychar2);

            if (keystring2 == "")
            {
                keystring2 = "0";
            }

            int key1 = int.Parse(keystring1);
            int key2 = int.Parse(keystring2);

            Random ByteCreator = new Random();
            byte[] bfile1 = new byte[key1];
            byte[] bfile2 = new byte[key2];

            ByteCreator.NextBytes(bfile1);
            ByteCreator.NextBytes(bfile2);

            byte[] bfile = new byte[file.Length + (key1 + key2)];

            for (int i = 0; 
                                                                                                                                                      i < key1; i++)
            {
                bfile[i] = bfile1[i];
            }
            for (int i = 0; i < file.Length; i++)

            {
                bfile[key1 + i] = file[i];
            }
            for (int i = 0; i < key2; i++)
            {
                bfile[key1 + file.Length + i] = bfile2[i];
            }

            return bfile;
        }// Stage 3: Adds the Anti-Brute Force Fake Bytes

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
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Destination Functionality
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                txtDestination.TextAlignment = TextAlignment.Center;
                txtDestination.FontWeight = FontWeights.UltraLight;
            }

        } //Clicked Destination

        private void txtDestination_MouseEnter(object sender, MouseEventArgs e)
        {
            txtDestination.Background = Brushes.PaleGreen;
            Cursor = Cursors.Hand;
        } //Entered Destination

        private void txtDestination_MouseLeave(object sender, MouseEventArgs e)
        {
            txtDestination.Background = Brushes.White;
            Cursor = Cursors.Arrow;
        } //Left Destination
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Password Functionality
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void txbPassword_GotFocus(object sender, RoutedEventArgs e)
        {

            txbPassword.Text = "";
            txbPassword.TextAlignment = TextAlignment.Left;
            txbPassword.FontWeight = FontWeights.Normal;

        } //Clicked Password

        private void txbPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            txbPassword.Background = Brushes.PaleGreen;
        } //Enter Password

        private void txbPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            txbPassword.Background = Brushes.White;
        } //Leave Password
          ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Anti-Brute Force Functionality
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void txbBruteKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //No Idea what the following line means, ask dingle, stops input of text.
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "\\d+") && e.Key != Key.Back) 
                e.Handled = true;
        }// Key Pressed

        private void txbBruteKey_GotFocus(object sender, RoutedEventArgs e)
        {
            txbBruteKey.Text = "";
            txbBruteKey.TextAlignment = TextAlignment.Left;
            txbBruteKey.FontWeight = FontWeights.Normal;
        } //Clicked Brute Force

        private void txbBruteKey_MouseEnter(object sender, MouseEventArgs e)
        {
            txbBruteKey.Background = Brushes.PaleGreen;
        } //Entered Brute Force

        private void txbBruteKey_MouseLeave(object sender, MouseEventArgs e)
        {
            txbBruteKey.Background = Brushes.White;
        } //Left Brute Force
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Drag and Drop Functionality
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool DroptextExists = true;

        private void lsvDnD_MouseEnter(object sender, MouseEventArgs e)
        {
            lsvDnD.Background = Brushes.PaleGreen;
            lsvDnD.Cursor = Cursors.Hand;
        } //Mouse Enter

        private void lsvDnD_MouseLeave(object sender, MouseEventArgs e)
        {
            lsvDnD.Background = Brushes.White;
            lsvDnD.Cursor = Cursors.Arrow;
        } //Mouse Leave

        private void lsvDnD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.ShowDialog();
                Files newfile = new Files();
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
                btnEncryptIsEnabled = true;
                btnEncrypt.IsHitTestVisible = true;
                btnEncrypt.Background = Brushes.IndianRed;
                btnEncrypt.Foreground = Brushes.White;
                btnEncrypt.BorderBrush = Brushes.White;
            }
            catch
            {

            }
        } //Click

        private void lsvDnD_DragEnter(object sender, DragEventArgs e)
        {
            lsvDnD.Background = Brushes.PaleGreen;
            Panel.SetZIndex(imgPlusFile, 2);
            Panel.SetZIndex(txtDropFilesHere, 0);
            Panel.SetZIndex(lsvDnD, 1);
        } //Enter w/File

        private void lsvDnD_DragLeave(object sender, DragEventArgs e)
        {
            lsvDnD.Background = Brushes.White;
            Panel.SetZIndex(imgPlusFile, 0);
            if (DroptextExists == true)
                Panel.SetZIndex(txtDropFilesHere, 1);
            Panel.SetZIndex(lsvDnD, 1);
        } // Leave w/File

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (var path in droppedFilePaths)
                {
                    FileInfo fi = new FileInfo(path);
                    Files newfile = new Files();
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
                btnEncryptIsEnabled = true;
                btnEncrypt.IsHitTestVisible = true;
                btnEncrypt.Background = Brushes.IndianRed;
                btnEncrypt.Foreground = Brushes.White;
                btnEncrypt.BorderBrush = Brushes.White;
            }
            Panel.SetZIndex(imgPlusFile, 0);
            Panel.SetZIndex(lsvDnD, 1);
            DroptextExists = false;

        } //Drop File

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    class Files
    {
        public string FilePath { set; get; }
        public string FileName { set; get; }
        public string Size { set; get; }
        public string Extention { set; get; }
    }

}
