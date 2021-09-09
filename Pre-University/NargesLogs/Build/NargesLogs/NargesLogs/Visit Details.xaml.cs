using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace NargesLogs
{

    public partial class Visit_Details : Window
    {

        //Declaring variables that will be used later in the application.
        public bool imagesloaded = false;

        public Visit_Details()
        {

            InitializeComponent();

        }

        private void brdImage_MouseEnter(object sender, MouseEventArgs e)
        {

            if (imagesloaded == false)
            {

                //Clicking on the images box when its empty allows the user to add an image. This is UI feedback for that feature.
                BrushConverter bc = new BrushConverter();
                brdImage.Background = (Brush)bc.ConvertFrom("#FFACE5FF");
                imgDandDImage.Visibility = Visibility.Hidden;
                imgDandDImage2.Visibility = Visibility.Visible;
                Cursor = Cursors.Hand;

            }

        }

        private void brdImage_MouseLeave(object sender, MouseEventArgs e)
        {

            //As above.
            if (imagesloaded == false)
            {
                BrushConverter bc = new BrushConverter();
                brdImage.Background = (Brush)bc.ConvertFrom("#FF056591");
                imgDandDImage.Visibility = Visibility.Visible;
                imgDandDImage2.Visibility = Visibility.Hidden;
                Cursor = Cursors.Arrow;
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            
            //Simply closes the window and returns to the Patient details without making any changes.
            Close();

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

            //Switches textblocks with editable textboxes and disables/enables the appropriate buttons for edit mode.
            PublicFunctions.Functionality.DisableButton(btnEdit);
            PublicFunctions.Functionality.EnableButton(btnSave);
            PublicFunctions.Functionality.EditClicked(txtDate, txbDate);
            PublicFunctions.Functionality.EditClicked(txtReason, txbReason);

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            //Ensures that fields are not left empty to avoid errors.
            if(txtDate.Text == "" || txtReason.Text == "")
            {

                MessageBox.Show("The 'Reason' or 'Date' fields have been left empty. Please enter today's date and the reason for the visit into the appropriate fields.", "Fields left empty", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }

            if (!IsValidDate(txtDate.Text))
            {

                MessageBox.Show("The date of birth entered was not valid. Please enter a valid date of birth in the format 'dd/mm/yyyy'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            //Switches textboxes with textblocks and enables/disables the appropriate nuttons to return to viewing mode.
            PublicFunctions.Functionality.DisableButton(btnSave);
            PublicFunctions.Functionality.EnableButton(btnEdit);
            PublicFunctions.Functionality.SaveClicked(txtDate, txbDate);
            PublicFunctions.Functionality.SaveClicked(txtReason, txbReason);

            //Sends the editted/created visit to the server.
            ConnectionFunctions CFunctions = new ConnectionFunctions();

            if (CurrentVisit.NewVisit)
            {

                CFunctions.AddVisit(txtDate.Text, txtReason.Text, CurrentPatient.ID);
                PublicFunctions.Functionality.EnableButton(btnAdd);
                CurrentVisit.NewVisit = false;

            }

            else
            {

                CFunctions.UpdateVisitInfo(CurrentVisit.ID.ToString(), txbDate.Text, txtReason.Text);

            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Disables remove button as obviously no image has been selected.
            PublicFunctions.Functionality.DisableButton(btnRemove);

            //Enters edit mode if this is a new visit.
            if (CurrentVisit.NewVisit)
            {

                btnEdit_Click(this, null);
                txtDate.Text = DateTime.Today.ToShortDateString();
                PublicFunctions.Functionality.DisableButton(btnAdd);
                PublicFunctions.Functionality.DisableButton(btnEdit);

            }

            //Otherwise, continues as normal.
            else
            {

                //loads into the textboxes the values from the visit.
                PublicFunctions.Functionality.DisableButton(btnSave);
                txtDate.Text = CurrentVisit.Date;
                txtReason.Text = CurrentVisit.Reason;
                txbDate.Text = CurrentVisit.Date;
                txbReason.Text = CurrentVisit.Reason;

                //Loads in the images from the temporary folder.
                for (int i = 0; i < Directory.GetFiles(Directory.GetCurrentDirectory() + @"\tempvisitimages\" + CurrentPatient.Name + "_" + CurrentPatient.FamilyName + @"\" + "Visit" + CurrentVisit.ID.ToString()).Count(); i++)
                {

                    Image img = new Image();
                    byte[] data;
                    using (FileStream bytestream = new FileStream(Directory.GetCurrentDirectory() + @"\tempvisitimages\" + CurrentPatient.Name + "_" + CurrentPatient.FamilyName + @"\" + "Visit" + CurrentVisit.ID.ToString() + @"\img_" + i.ToString() + ".jpg", FileMode.Open))
                    {
                        data = new byte[bytestream.Length];
                        bytestream.Read(data, 0, (int)bytestream.Length);
                    }

                    BitmapImage bitmap = new BitmapImage();
                    FileStream readstream = File.OpenRead(Directory.GetCurrentDirectory() + @"\tempvisitimages\" + CurrentPatient.Name + "_" + CurrentPatient.FamilyName + @"\" + "Visit" + CurrentVisit.ID.ToString() + @"\img_" + i.ToString() + ".jpg");
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = readstream;
                    bitmap.EndInit();
                    readstream.Close();
                    readstream.Dispose();

                    img.Source = bitmap;
                    img.Tag = data;
                    img.Effect = new BlurEffect();
                    img.Margin = new Thickness(20);
                    img.Cursor = Cursors.Hand;
                    img.PreviewMouseDown += new MouseButtonEventHandler(img_Clicked);
                    stkVisitImages.Children.Remove(grdVisitImages);
                    stkVisitImages.Children.Add(img);
                    imagesloaded = true;

                }

                //Downloads in the visit notes from the server.
                ConnectionFunctions CFunctions = new ConnectionFunctions();
                CFunctions.RequestVisitNotes();
                string directory = @"\tempnotefiles\" + "U" + CurrentPatient.ID + "_V" + CurrentVisit.ID + ".txt";
                rtxtNotes.Document.Blocks.Clear();

                //This code loads in the visit notes from the downloaded string to the richtextbox.

                //code from: https://social.msdn.microsoft.com/Forums/sqlserver/en-US/149deaab-122d-4385-aa24-e44a94cd281d/how-can-get-rtf-text-from-wpf-rich-text-box-control?forum=wpf
                FileStream filestream = new FileStream(Directory.GetCurrentDirectory() + directory, FileMode.Open);
                byte[] file = new byte[filestream.Length];
                filestream.Read(file, 0, file.Length);
                string rtf = Encoding.ASCII.GetString(file);
                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rtf));
                TextRange range = new TextRange(rtxtNotes.Document.ContentStart, rtxtNotes.Document.ContentEnd);
                range.Load(stream, DataFormats.Rtf);
                stream.Close();
                filestream.Close();

                //Configures the richtextbox.
                Paragraph p = rtxtNotes.Document.Blocks.FirstBlock as Paragraph;
                p.LineHeight = 0.4;

            }

        }

        private void img_Clicked(object sender, EventArgs e)
        {

            //Loads in the sender object as an image.
            Image img = (Image)sender;

            //Checks if the image is being clicked for a second time. If so, goes through the process of saving the file onto the pc.
            if (img.Tag.Equals(SelectedImage.selectedimage))
            {

                //Reads the file and saves it into specified location.
                MessageBoxResult AskSaveDialog = MessageBox.Show("Would you like to download the selected file on your local machine?", "Save File Dialog", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (AskSaveDialog == MessageBoxResult.Yes)
                {

                    SaveFileDialog SaveDialog = new SaveFileDialog();
                    SaveDialog.DefaultExt = ".jpg";
                    SaveDialog.ShowDialog();

                    byte[] image = (byte[])(img.Tag);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    using (FileStream writestream = new FileStream(SaveDialog.FileName, FileMode.Create))
                    {

                        writestream.Write(image, 0, image.Length);

                    }

                }

            }

            else
            {

                //Removes the blur effect and selects the image when the user clicks on it for the first time in a row.
                PublicFunctions.Functionality.EnableButton(btnRemove);
                SelectedImage.selectedimage = (byte[])(img).Tag;

                foreach(Image i in stkVisitImages.Children)
                {

                    i.Effect = new BlurEffect();

                }

                img.Effect = null;

            }

        }

        private void scrVisitImages_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Ensures that the visit is saved before the user attempts to add an image.
            if (CurrentVisit.NewVisit)
            {

                MessageBox.Show("You must save the visit before uploading images for it.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            //If statement ensures that the images are not duplicated.
            if (imagesloaded == false)
            {

                //User selects and image.
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();

                if (ofd.FileName == "")
                    return;

                //Ensures that the file is an image.
                if (ofd.FileName.Substring(ofd.FileName.LastIndexOf(".")) != ".jpg")
                {

                    MessageBox.Show("The file selected was not in the correct format. Please select a .jpg file.", "Incorrect File Format", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;

                }

                stkVisitImages.Children.Remove(grdVisitImages);

                //Image is sent to the server where it is saved to the visit.
                Image img = new Image();

                ConnectionFunctions CFunctions = new ConnectionFunctions();
                CFunctions.SendVisitImages(ofd.FileName, CurrentPatient.Name + " " + CurrentPatient.FamilyName, CurrentVisit.ID.ToString(), stkVisitImages.Children.Count + 1, this);

                //reads the file and loads it in using a bitmap image to avoid locking the file.
                byte[] data;
                using (FileStream bytestream = new FileStream(ofd.FileName, FileMode.Open))
                {
                    data = new byte[bytestream.Length];
                    bytestream.Read(data, 0, (int)bytestream.Length);
                }

                BitmapImage bitmap = new BitmapImage();
                FileStream readstream = File.OpenRead(ofd.FileName);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = readstream;
                bitmap.EndInit();
                readstream.Close();
                readstream.Dispose();

                img.Source = bitmap;
                img.Tag = data;
                img.Effect = new BlurEffect();
                img.Margin = new Thickness(20);
                img.Cursor = Cursors.Hand;
                img.PreviewMouseDown += new MouseButtonEventHandler(img_Clicked);
                stkVisitImages.Children.Add(img);
                imagesloaded = true;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.SetCursor(Cursors.Arrow);

                });

            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            //Go to 'scrVisitImages_PreviewMouseDown' for full description.
            //The same as the 'scrVisitImages_PreviewMouseDown' Event but uses button and works for more than the first time. (Does not have the mechanisms to ensure that it does not)
            if (CurrentVisit.NewVisit)
            {

                MessageBox.Show("You must save the visit before uploading images for it.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (ofd.FileName == "")
                return;

            //Ensures that the file is an image.
            if (ofd.FileName.Substring(ofd.FileName.LastIndexOf(".")) != ".jpg")
            {

                MessageBox.Show("The file selected was not in the correct format. Please select a .jpg file.", "Incorrect File Format", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }

            stkVisitImages.Children.Remove(grdVisitImages);

            Image img = new Image();

            ConnectionFunctions CFunctions = new ConnectionFunctions();
            CFunctions.SendVisitImages(ofd.FileName, CurrentPatient.Name + " " + CurrentPatient.FamilyName, CurrentVisit.ID.ToString(), stkVisitImages.Children.Count + 1, this);

            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(new Uri(ofd.FileName)));

            using (MemoryStream ms = new MemoryStream())
            {

                encoder.Save(ms);
                data = ms.ToArray();

            }

            BitmapImage bitmap = new BitmapImage();
            FileStream readstream = File.OpenRead(ofd.FileName);
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = readstream;
            bitmap.EndInit();
            readstream.Close();
            readstream.Dispose();

            img.Source = bitmap;
            img.Tag = data;
            img.Effect = new BlurEffect();
            img.Margin = new Thickness(20);
            img.Cursor = Cursors.Hand;
            img.PreviewMouseDown += new MouseButtonEventHandler(img_Clicked);
            stkVisitImages.Children.Add(img);
            imagesloaded = true;

            Application.Current.Dispatcher.Invoke(() =>
            {

                Mouse.SetCursor(Cursors.Arrow);

            });

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

            //Uses loop to go through each image to determine their image number.
            for(int i = 0; i < stkVisitImages.Children.Count; i++)
                if(((Image)(stkVisitImages.Children[i])).Tag == SelectedImage.selectedimage)
                {

                    stkVisitImages.Children.RemoveAt(i);
                    SelectedImage.imagenum = i;

                }

            //Collects info and sends to server for deletion.
            string filename = (SelectedImage.imagenum + 1).ToString() + ".jpg";
            string patient = CurrentPatient.Name + " " + CurrentPatient.FamilyName;

            ConnectionFunctions CFunctions = new ConnectionFunctions();
            CFunctions.RemoveImage(filename, CurrentPatient.ID, CurrentVisit.ID.ToString(), patient);
            PublicFunctions.Functionality.DisableButton(btnRemove);

        }

        private void scrVisitImages_Drop(object sender, DragEventArgs e)
        {

            //The same as the 'btnAdd_Click' event, but works with drag and drop. These features are described but go to similar events for full description.
            if (CurrentVisit.NewVisit)
            {

                MessageBox.Show("You must save the visit before uploading images for it.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            //Check's the event information - 'e', to determine if a file was dropped.
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                stkVisitImages.Children.Remove(grdVisitImages);

                //Retrieves 'e' filedrop information and reads it as a string array which contains all dropped file paths.
                string[] filepaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                //Ensures that the files dropped are images.
                foreach(string filepath in filepaths)
                {

                    if (filepath.Substring(filepath.LastIndexOf(".")) != ".jpg")
                    {

                        MessageBox.Show("The file selected was not in the correct format. Please select a .jpg file.", "Incorrect File Format", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;

                    }

                }


                for (int i = 0; i < filepaths.Count(); i++)
                {

                    Image img = new Image();

                    ConnectionFunctions CFunctions = new ConnectionFunctions();
                    CFunctions.SendVisitImages(filepaths[i], CurrentPatient.Name + " " + CurrentPatient.FamilyName, CurrentVisit.ID.ToString(), stkVisitImages.Children.Count + 1, this);

                    byte[] data;
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(new Uri(filepaths[i])));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }

                    BitmapImage bitmap = new BitmapImage();
                    FileStream readstream = File.OpenRead(filepaths[i]);
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = readstream;
                    bitmap.EndInit();
                    readstream.Close();
                    readstream.Dispose();

                    img.Source = bitmap;
                    img.Tag = data;
                    img.Effect = new BlurEffect();
                    img.Margin = new Thickness(20);
                    img.Cursor = Cursors.Hand;
                    img.PreviewMouseDown += new MouseButtonEventHandler(img_Clicked);
                    stkVisitImages.Children.Add(img);
                    imagesloaded = true;

                }

                Application.Current.Dispatcher.Invoke(() =>
                {

                    Mouse.SetCursor(Cursors.Arrow);

                });

            }

        }

        private void txtDate_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.HighlightTextBox(txtDate);

        }

        private void txtDate_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.UnhighlightTextBox(txtDate);

        }

        private void txtReason_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.HighlightTextBox(txtReason);

        }

        private void txtReason_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.UnhighlightTextBox(txtReason);

        }

        public bool IsValidDate(string date)
        {
            
            //Algorithm for checking if the visit date is valid.

            //Checks that the date is in the correct format.
            if (txtDate.Text.ToCharArray().Count() < 2 || txtDate.Text.ToCharArray()[2] != '/' || txtDate.Text.ToCharArray()[5] != '/' || txtDate.Text.ToCharArray().Count() != 10)
                return false;

            //Isolates each part of the date to later check.
            int day = int.Parse(date.Remove(date.IndexOf("/")));
            int month = int.Parse(date.Remove(date.LastIndexOf("/")).Substring(date.IndexOf("/") + 1));
            int year = int.Parse(date.Substring(date.LastIndexOf("/") + 1));

            //Checks if the year is valid.
            if (year > DateTime.Now.Year || year < 1850)
                return false;

            //Checks if the month is valid.
            if (month > 12 || month < 1)
                return false;

            //Checks if the day is valid.
            if (day < 1 || day > 31)
                return false;

            //Checks if the day is valid with respect to leap year status.
            if ((!DateTime.IsLeapYear(year) && month == 2 && day > 28) || (DateTime.IsLeapYear(year) && month == 2 && day > 29) || (month == 4 || month == 6 || month == 9 || month == 11) && day > 30)
                return false;

            return true;

        }

        private void Window_Closed(object sender, EventArgs e)
        {

            //Sends notes to the server, and properly closes the window before returning to the Patient Details window.
            stkVisitImages.Children.Clear();
            PublicFunctions.Functionality.ClearTempFiles();
            Mouse.SetCursor(Cursors.Wait);
            ConnectionFunctions CFunctions = new ConnectionFunctions();
            CFunctions.SendVisitNotes(rtxtNotes);
            Patient_Details pd = (Patient_Details)Owner;
            pd.lsvVisits.Cursor = Cursors.Wait;
            Thread visitloader = new Thread(pd.LoadVisitsList);
            visitloader.Start();
            Mouse.SetCursor(Cursors.Arrow);
            Owner.IsEnabled = true;
            pd.Focus();

        }

    }

    static class SelectedImage
    {

        //Class used to store visit image information.
        public static byte[] selectedimage;
        public static int imagenum;

    }

}
