using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace NargesLogs
{

    public partial class Patient_Details : Window
    {

        public Patient_Details()
        {

            InitializeComponent();

        }

        private void bdrImage_MouseEnter(object sender, MouseEventArgs e)
        {

            //Highlights the imagebox for UI feedback.
            BrushConverter bc = new BrushConverter();
            bdrImage.Background = (Brush)bc.ConvertFrom("#FFACE5FF");
            imgAddImage2.Visibility = Visibility.Visible;
            imgAddImage.Visibility = Visibility.Hidden;

        }

        private void bdrImage_MouseLeave(object sender, MouseEventArgs e)
        {

            //Unhighlights the imagebox for UI feedback.
            BrushConverter bc = new BrushConverter();
            bdrImage.Background = (Brush)bc.ConvertFrom("#FF056591");
            imgAddImage.Visibility = Visibility.Visible;
            imgAddImage2.Visibility = Visibility.Hidden;

        }

        private void bdrImage_MouseDown(object sender, MouseButtonEventArgs e)
        {

            //Ensures that the patient has been saved before the user tries to upload an image.
            if (CurrentPatient.NewPatient)
            {

                MessageBox.Show("You must save the patient before uploading a profile image", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            //Allows the user to select an image to upload as the profile image, and cancels if the user selects nothing.
            OpenFileDialog imageselector = new OpenFileDialog();
            imageselector.ShowDialog();

            if (imageselector.FileName == "")
                return;

            //Ensures that the file is an image.
            if (imageselector.FileName.Substring(imageselector.FileName.LastIndexOf(".")) != ".jpg")
            {

                MessageBox.Show("The file selected was not in the correct format. Please select a .jpg file.", "Incorrect File Format", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }

            Misc.ProfileImageIsDone = false;
            grdImage.Children.Clear();

            //Sets the cursor to 'wait' for UI feedback.
            Mouse.SetCursor(Cursors.Wait);

            //Adds the image using a bitmap to avoid locking the image file.
            Image clientimage = new Image();

            BitmapImage bmp = new BitmapImage();
            FileStream readstream = File.OpenRead(imageselector.FileName);
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.StreamSource = readstream;
            bmp.EndInit();
            readstream.Close();
            readstream.Dispose();

            clientimage.Source = bmp;
            clientimage.Height = imgAddImage.Height;
            clientimage.Width = imgAddImage.Width;
            grdImage.Children.Add(clientimage);

            //Sends the image to the server.
            ConnectionFunctions CFunctions = new ConnectionFunctions();
            CFunctions.SendClientImage(imageselector.FileName, "~CLIENTNAME" + CurrentPatient.Name + " " + CurrentPatient.FamilyName);

            Misc.ProfileImageIsDone = true;
            Mouse.SetCursor(Cursors.Arrow);

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            //Opens the visit screen and sets new visit to true.
            CurrentVisit.NewVisit = true;
            Visit_Details vd = new Visit_Details();
            vd.Owner = this;
            IsEnabled = false;
            vd.Show();

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

            //Returns to the patient list screen.
            Patients_List pl = new Patients_List();
            pl.Owner = this;
            pl.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            pl.Show();
            pl.Owner = null;
            Misc.WindowSwitch = true;
            Close();

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

            //Sets all of the editable objects to edit mode.
            PublicFunctions.Functionality.EditClicked(txtName, txbName);
            PublicFunctions.Functionality.EditClicked(txtFamilyName, txbFamilyName);
            PublicFunctions.Functionality.EditClicked(txtOccupation, txbOccupation);
            PublicFunctions.Functionality.DropDownEditClicked(cmbSex, txbSex);
            PublicFunctions.Functionality.EditClicked(txtDoB, txbDoB);
            PublicFunctions.Functionality.DropDownEditClicked(cmbAoTSI, txbAoTSI);
            PublicFunctions.Functionality.EditClicked(txtHomePhone, txbHomePhone);
            PublicFunctions.Functionality.EditClicked(txtMedicareNum, txbMedicareNum);
            PublicFunctions.Functionality.EditClicked(txtMobileNumber, txbMobileNumber);
            PublicFunctions.Functionality.EditClicked(txtHealthcareNum, txbHealthcareNum);
            PublicFunctions.Functionality.EditClicked(txtWorkPhone, txbWorkPhone);
            PublicFunctions.Functionality.EditClicked(txtAddress, txbAddress);
            PublicFunctions.Functionality.EditClicked(txtEmergencyContact, txbEmergencyContact);
            PublicFunctions.Functionality.EditClicked(txtPostalAddress, txbPostalAddress);

            //Disables the Edit button as edit mode is already enabled, and enables the save button so the user can save their edits.
            PublicFunctions.Functionality.DisableButton(btnEdit);
            PublicFunctions.Functionality.EnableButton(btnSave);

        }

        public bool IsValidDate(string date)
        {

            //Ensures that the date format is correct.
            if (txtDoB.Text.ToCharArray().Count() < 2 || txtDoB.Text.ToCharArray()[2] != '/' || txtDoB.Text.ToCharArray()[5] != '/' || txtDoB.Text.ToCharArray().Count() != 10)
                return false;

            //Isolates each section of the date.
            int day = int.Parse(date.Remove(date.IndexOf("/")));
            int month = int.Parse(date.Remove(date.LastIndexOf("/")).Substring(date.IndexOf("/") + 1));
            int year = int.Parse(date.Substring(date.LastIndexOf("/") + 1));

            //Ensures that the year, month, and day are all valid.
            if (year > DateTime.Now.Year || year < 1850)
                return false;

            if (month > 12 || month < 1)
                return false;

            if (day < 1 || day > 31)
                return false;

            //Ensures that the day is valid with respect to the leap year status.
            if((!DateTime.IsLeapYear(year) && month == 2 && day > 28) || (DateTime.IsLeapYear(year) && month == 2 && day > 29) || (month == 4 || month == 6 || month == 9 || month == 11) && day > 30)
                return false;

            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            //Checks the date of birth validity.
            if (!IsValidDate(txtDoB.Text))
            {

                MessageBox.Show("The date of birth entered was not valid. Please enter a valid date of birth in the format 'dd/mm/yyyy'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            //Checks if any fields are left empty.
            if(txtName.Text == "" || txtFamilyName.Text == "" || txtOccupation.Text == "" || txtDoB.Text == "" || txtHomePhone.Text == "" || txtMedicareNum.Text == "" || txtMobileNumber.Text == "" || txtHealthcareNum.Text == "" || txtWorkPhone.Text == "" || txtAddress.Text == "" || txtEmergencyContact.Text == "" || txtPostalAddress.Text == "" || cmbSex.Text == "" || cmbAoTSI.Text == "")
            {

                MessageBox.Show("One or more fields have not been filled in. If some information is unavailable, please enter 'N/A' into the field instead.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            //Checks if any fields have spaces on the end, removes the spaces, and informs the user.
            List<string> removedspaces = new List<string>();

            if(txtName.Text.ToCharArray().Last() == ' ')
            {

                txtName.Text = txtName.Text.Remove(txtName.Text.Length - 1);
                removedspaces.Add("Name");

            }

            if(txtFamilyName.Text.ToCharArray().Last() == ' ')
            {

                txtFamilyName.Text = txtFamilyName.Text.Remove(txtFamilyName.Text.Count() - 1);
                removedspaces.Add("Family Name");

            }

            if (txtOccupation.Text.ToCharArray().Last() == ' ')
            {

                txtOccupation.Text = txtOccupation.Text.Remove(txtOccupation.Text.Count() - 1);
                removedspaces.Add("Occupation");

            }

            if (txtHomePhone.Text.ToCharArray().Last() == ' ')
            {

                txtHomePhone.Text = txtHomePhone.Text.Remove(txtHomePhone.Text.Count() - 1);
                removedspaces.Add("Home Phone");

            }

            if (txtMobileNumber.Text.ToCharArray().Last() == ' ')
            {

                txtMobileNumber.Text = txtMobileNumber.Text.Remove(txtMobileNumber.Text.Count() - 1);
                removedspaces.Add("Mobile Number");

            }

            if (txtWorkPhone.Text.ToCharArray().Last() == ' ')
            {

                txbWorkPhone.Text = txbWorkPhone.Text.Remove(txbWorkPhone.Text.Count() - 1);
                removedspaces.Add("Work Phone");

            }

            if (txtEmergencyContact.Text.ToCharArray().Last() == ' ')
            {

                txbEmergencyContact.Text = txbEmergencyContact.Text.Remove(txbEmergencyContact.Text.Count() - 1);
                removedspaces.Add("Emergency Contact");

            }

            if (txtMedicareNum.Text.ToCharArray().Last() == ' ')
            {

                txtMedicareNum.Text = txtMedicareNum.Text.Remove(txtMedicareNum.Text.Count() - 1);
                removedspaces.Add("Medicare Number");

            }

            if (txtHealthcareNum.Text.ToCharArray().Last() == ' ')
            {

                txtHealthcareNum.Text = txtHealthcareNum.Text.Remove(txtHealthcareNum.Text.Count() - 1);
                removedspaces.Add("Healthcare Number");

            }

            if (txtAddress.Text.ToCharArray().Last() == ' ')
            {

                txtAddress.Text = txtAddress.Text.Remove(txtAddress.Text.Count() - 1);
                removedspaces.Add("Residential Address");

            }

            if (txtPostalAddress.Text.ToCharArray().Last() == ' ')
            {

                txtPostalAddress.Text = txtPostalAddress.Text.Remove(txtPostalAddress.Text.Count() - 1);
                removedspaces.Add("Postal Address");

            }

            if(removedspaces.Count > 0)
            {

                string fields = "";
                fields += removedspaces[0];

                for (int i = 1; i < removedspaces.Count; i++)
                    fields += ", " + removedspaces[i];

                MessageBox.Show("The following fields had a space removed from the end: " + fields, "Space Removal", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //Once all checks are completed successfully, all editable objects are switched back to save mode.
            PublicFunctions.Functionality.SaveClicked(txtName, txbName);
            PublicFunctions.Functionality.SaveClicked(txtFamilyName, txbFamilyName);
            PublicFunctions.Functionality.SaveClicked(txtOccupation, txbOccupation);
            PublicFunctions.Functionality.DropDownSaveClicked(cmbSex, txbSex);
            PublicFunctions.Functionality.SaveClicked(txtDoB, txbDoB);
            PublicFunctions.Functionality.DropDownSaveClicked(cmbAoTSI, txbAoTSI);
            PublicFunctions.Functionality.SaveClicked(txtHomePhone, txbHomePhone);
            PublicFunctions.Functionality.SaveClicked(txtMedicareNum, txbMedicareNum);
            PublicFunctions.Functionality.SaveClicked(txtMobileNumber, txbMobileNumber);
            PublicFunctions.Functionality.SaveClicked(txtHealthcareNum, txbHealthcareNum);
            PublicFunctions.Functionality.SaveClicked(txtWorkPhone, txbWorkPhone);
            PublicFunctions.Functionality.SaveClicked(txtAddress, txbAddress);
            PublicFunctions.Functionality.SaveClicked(txtEmergencyContact, txbEmergencyContact);
            PublicFunctions.Functionality.SaveClicked(txtPostalAddress, txbPostalAddress);

            //The cursor is set to 'wait' for UI feedback.
            Mouse.SetCursor(Cursors.Wait);

            //Sets the saved information to the current patient information.
            ConnectionFunctions CFunctions = new ConnectionFunctions();

            CurrentPatient.Name = txtName.Text;
            CurrentPatient.FamilyName = txtFamilyName.Text;
            CurrentPatient.Occupation = txtOccupation.Text;
            CurrentPatient.Sex = cmbSex.Text;
            CurrentPatient.DoB = txtDoB.Text;

            if (cmbAoTSI.Text == "Yes")
                CurrentPatient.AoTSI = true;

            else
                CurrentPatient.AoTSI = false;

            CurrentPatient.HomePhone = txtHomePhone.Text;
            CurrentPatient.MobilePhone = txtMobileNumber.Text;
            CurrentPatient.WorkPhone = txtWorkPhone.Text;
            CurrentPatient.EmergencyContact = txtEmergencyContact.Text;
            CurrentPatient.Medicare = txtMedicareNum.Text;
            CurrentPatient.Healthcare = txtHealthcareNum.Text;
            CurrentPatient.ResidentialAddress = txtAddress.Text;
            CurrentPatient.PostalAddress = txtPostalAddress.Text;

            //Sends the saved patient information to the server.
            CFunctions.SendPatientInfo();

            //Figures out the allocated ID of the new patient.
            RetrievedInfo.IDs = CFunctions.SendGenericCommand("3214~COMMAND#3");
            CurrentPatient.ID = RetrievedInfo.IDs.Last();
            for (int i = 0; i < RetrievedInfo.IDs.Count(); i++)
            {

                if (RetrievedInfo.IDs[i] == CurrentPatient.ID)
                    CurrentPatient.DbasePosition = i;

            }

            //Sets new patient to false, effectively making this a normal patient.
            CurrentPatient.NewPatient = false;

            //Enables and disables the appropriate buttons to make this a normal patient details screen.
            PublicFunctions.Functionality.EnableButton(btnEdit);
            PublicFunctions.Functionality.DisableButton(btnSave);
            PublicFunctions.Functionality.EnableButton(btnAdd);

            Mouse.SetCursor(Cursors.Arrow);

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

            //Sends the visit information to the server so that it can be deleted from the database.
            VisitEntry vEntry = lsvVisits.SelectedItem as VisitEntry;
            ConnectionFunctions CFunctions = new ConnectionFunctions();
            CFunctions.RemoveVisit(vEntry.ID.ToString());

            //Removes the visit from the list.
            lsvVisits.Items.Remove(lsvVisits.SelectedItem);

            //Ensures that the remove button is disabled if there are no more visits left to avoid crash.
            if (lsvVisits.Items.Count < 1)
                PublicFunctions.Functionality.DisableButton(btnRemove);

        }

        public void LoadVisitsList()
        {

            //Waits until the profile image is done loading before attempting to access the server.
            while (Misc.ProfileImageIsDone == false)
                Thread.Sleep(1);

            //Clears the Visits list so that they can be filled back up avoiding duplicates.
            ConnectionFunctions CFunctions = new ConnectionFunctions();
            Dispatcher.Invoke(() => lsvVisits.Items.Clear());
            CurrentPatient_Visits.Dates.Clear();
            CurrentPatient_Visits.IDs.Clear();
            CurrentPatient_Visits.Reasons.Clear();
            CurrentPatient_Visits.Count = 0;

            //Retrieves the visits info again.
            RetrievedInfo.Visit_PatientIDs = CFunctions.SendGenericCommand("3214~COMMAND#19");
            RetrievedInfo.Visit_Dates = CFunctions.SendGenericCommand("3214~COMMAND#20");
            RetrievedInfo.Visit_Reasons = CFunctions.SendGenericCommand("3214~COMMAND#21");
            RetrievedInfo.Visit_IDs = CFunctions.SendGenericCommand("3214~COMMAND#23");

            //Retrieves visit images.
            CFunctions.RequestVisitImages(CurrentPatient.Name + "_" + CurrentPatient.FamilyName, int.Parse(CurrentPatient.ID));

            //Transforms the retrieved information into a visits list.
            List<int> CurrentPatientsEntries = new List<int>();

            for (int i = 0; i < RetrievedInfo.Visit_PatientIDs.Length; i++)
            {

                if (RetrievedInfo.Visit_PatientIDs[i] == CurrentPatient.ID)
                {

                    CurrentPatient_Visits.Count++;
                    CurrentPatientsEntries.Add(i);

                }

            }

            foreach (int Entry in CurrentPatientsEntries)
            {

                CurrentPatient_Visits.Dates.Add(RetrievedInfo.Visit_Dates[Entry]);
                CurrentPatient_Visits.Reasons.Add(RetrievedInfo.Visit_Reasons[Entry]);
                CurrentPatient_Visits.IDs.Add(int.Parse(RetrievedInfo.Visit_IDs[Entry]));

            }

            for (int i = 0; i < CurrentPatient_Visits.Count; i++)
            {

                VisitEntry vEntry = new VisitEntry();
                vEntry.Date = CurrentPatient_Visits.Dates[i];
                vEntry.Reason = CurrentPatient_Visits.Reasons[i];
                vEntry.ID = CurrentPatient_Visits.IDs[i];
                vEntry.Visit_ImageCount = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\tempvisitimages\" + CurrentPatient.Name + "_" + CurrentPatient.FamilyName + @"\Visit" + vEntry.ID).Count().ToString();
                Dispatcher.Invoke(() => lsvVisits.Items.Add(vEntry));

            }

            Dispatcher.Invoke(() => lsvVisits.Cursor = Cursors.Arrow);
            Thread.CurrentThread.Abort();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Disables the remove button until a visit is selected and sets the cursor to 'wait' while the patient information loads for UI feedback.
            PublicFunctions.Functionality.DisableButton(btnRemove);
            Mouse.SetCursor(Cursors.Wait);

            //If the window is loading for a new patient, no information is loaded and all objects are set to edit mode instead.
            if (CurrentPatient.NewPatient == true)
            {

                btnEdit_Click(this, null);
                lsvVisits.Cursor = Cursors.Arrow;
                PublicFunctions.Functionality.DisableButton(btnEdit);
                PublicFunctions.Functionality.DisableButton(btnAdd);

            }

            //Otherwise, the patient information is loaded normally.
            else
            {

                //loads patient information into the text blocks.
                PublicFunctions.Functionality.DisableButton(btnSave);
                txbName.Text = CurrentPatient.Name;
                txtName.Text = CurrentPatient.Name;
                txbFamilyName.Text = CurrentPatient.FamilyName;
                txtFamilyName.Text = CurrentPatient.FamilyName;
                txbDoB.Text = CurrentPatient.DoB.Remove(CurrentPatient.DoB.IndexOf(" "));
                txtDoB.Text = CurrentPatient.DoB.Remove(CurrentPatient.DoB.IndexOf(" "));
                txbOccupation.Text = CurrentPatient.Occupation;
                txtOccupation.Text = CurrentPatient.Occupation;
                txbSex.Text = CurrentPatient.Sex;

                if (CurrentPatient.Sex == "Male")
                    cmbSex.SelectedIndex = 0;

                else if (CurrentPatient.Sex == "Female")
                    cmbSex.SelectedIndex = 1;

                else
                    cmbSex.SelectedIndex = 2;

                if (CurrentPatient.AoTSI == true)
                {

                    txbAoTSI.Text = "Yes";
                    cmbAoTSI.SelectedIndex = 0;

                }

                else
                {

                    txbAoTSI.Text = "No";
                    cmbAoTSI.SelectedIndex = 1;

                }

                txbHomePhone.Text = CurrentPatient.HomePhone;
                txtHomePhone.Text = CurrentPatient.HomePhone;
                txbMedicareNum.Text = CurrentPatient.Medicare;
                txtMedicareNum.Text = CurrentPatient.Medicare;
                txbMobileNumber.Text = CurrentPatient.MobilePhone;
                txtMobileNumber.Text = CurrentPatient.MobilePhone;
                txbHealthcareNum.Text = CurrentPatient.Healthcare;
                txtHealthcareNum.Text = CurrentPatient.Healthcare;
                txbWorkPhone.Text = CurrentPatient.WorkPhone;
                txtWorkPhone.Text = CurrentPatient.WorkPhone;
                txbAddress.Text = CurrentPatient.ResidentialAddress;
                txtAddress.Text = CurrentPatient.ResidentialAddress;
                txbEmergencyContact.Text = CurrentPatient.EmergencyContact;
                txtEmergencyContact.Text = CurrentPatient.EmergencyContact;
                txbPostalAddress.Text = CurrentPatient.PostalAddress;
                txtPostalAddress.Text = CurrentPatient.PostalAddress;

                //loads the profile image.
                Thread profileimage = new Thread(handleimages);
                profileimage.Start();
                
                //loads visits.
                Thread visitloader = new Thread(LoadVisitsList);
                visitloader.Start();

            }

        }

        public void handleimages()
        {
            //prevents other funtions from accessing the server while profile image is retrieved.
            Misc.ProfileImageIsDone = false;

            //Retrieves the client image.
            ConnectionFunctions CFunctions = new ConnectionFunctions();
            CFunctions.RequestClientImages();

            //Reads the client image from the temporary folder.
            Dispatcher.Invoke(() =>
            {

                try
                {

                    Image image = new Image();

                    BitmapImage bmp = new BitmapImage();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.BeginInit();
                    bmp.UriSource = new Uri(Directory.GetCurrentDirectory() + @"/tempclientimages/img_" + CurrentPatient.ID + ".jpg");
                    bmp.EndInit();

                    image.Source = bmp;
                    image.Width = imgAddImage.Width;
                    image.Height = imgAddImage.Height;
                    grdImage.Children.Clear();
                    grdImage.Children.Add(image);

                }

                catch { }

            });

            Misc.ProfileImageIsDone = true;
            Thread.CurrentThread.Abort();
        }

        public void VisitEntryPressed(object sender)
        {

            //Loads the selected visit information into the current visit information.
            VisitEntry vEntry = (VisitEntry)sender;

            try
            {

                CurrentVisit.ID = vEntry.ID;

            }

            catch
            {

                return;

            }

            CurrentVisit.Date = vEntry.Date;
            CurrentVisit.Reason = vEntry.Reason;
            CurrentVisit.NewVisit = false;
            Visit_Details vd = new Visit_Details();

            //Opens the visit window, with the current window disabled in the meantime to prevent crashes.
            IsEnabled = false;
            vd.Owner = this;
            vd.Show();

        }

        private void lsvVisits_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //Hooks mouse click on the visits list to the Visit entry pressed event.
            VisitEntry vEntry = lsvVisits.SelectedItem as VisitEntry;
            VisitEntryPressed(vEntry);

        }

        private void lsvVisits_MouseEnter(object sender, MouseEventArgs e)
        {

            //For while the list is loading.
            Cursor = Cursors.Wait;

        }

        private void lsvVisits_MouseLeave(object sender, MouseEventArgs e)
        {

            //As above.
            Cursor = Cursors.Arrow;

        }

        private void Window_Closed(object sender, EventArgs e)
        {

            //Clears the temporary folders and closes connection properly with the server if the window is closed.
            PublicFunctions.Functionality.ClearTempFiles();

            if (Misc.WindowSwitch == false)
            {

                try
                {

                    ConnectionFunctions CFunctions = new ConnectionFunctions();
                    CFunctions.CloseConnection();

                }

                catch { }

            }

            else
                Misc.WindowSwitch = false;

        }

        private void bdrImage_Drop(object sender, DragEventArgs e)
        {

            //Checks that a file was drag and dropped.
            if(e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {

                //The file information is retrieved from the event's e.
                Mouse.SetCursor(Cursors.Wait);
                Misc.ProfileImageIsDone = false;
                grdImage.Children.Clear();

                string filepath = (e.Data.GetData(DataFormats.FileDrop, true) as string[])[0];

                //Loads the image using bitmap image to avoid locking the file.
                Image clientimage = new Image();
                BitmapImage bmp = new BitmapImage();
                FileStream readstream = File.OpenRead(filepath);
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = readstream;
                bmp.EndInit();
                readstream.Close();
                readstream.Dispose();

                clientimage.Source = bmp;
                clientimage.Height = imgAddImage.Height;
                clientimage.Width = imgAddImage.Width;
                grdImage.Children.Add(clientimage);

                //Sends the image to the server.
                ConnectionFunctions CFunctions = new ConnectionFunctions();
                CFunctions.SendClientImage(filepath, "~CLIENTNAME" + CurrentPatient.Name + " " + CurrentPatient.FamilyName);
                Misc.ProfileImageIsDone = true;
                Mouse.SetCursor(Cursors.Arrow);
            }
        }

        private void txtName_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtName);

        }

        private void txtName_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtName);

        }

        private void txtFamilyName_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtFamilyName);

        }

        private void txtFamilyName_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtFamilyName);

        }

        private void txtOccupation_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtOccupation);

        }

        private void txtOccupation_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtOccupation);

        }

        private void txtDoB_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtDoB);

        }

        private void txtDoB_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtDoB);

        }

        private void txtHomePhone_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtHomePhone);

        }

        private void txtHomePhone_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtHomePhone);

        }

        private void txtMedicareNum_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtMedicareNum);

        }

        private void txtMedicareNum_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtMedicareNum);

        }

        private void txtMobileNumber_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtMobileNumber);

        }

        private void txtMobileNumber_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtMobileNumber);

        }

        private void txtHealthcareNum_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtHealthcareNum);

        }

        private void txtHealthcareNum_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtHealthcareNum);

        }

        private void txtWorkPhone_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtWorkPhone);

        }

        private void txtWorkPhone_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtWorkPhone);

        }

        private void txtAddress_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtAddress);

        }

        private void txtAddress_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtAddress);

        }

        private void txtEmergencyContact_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtEmergencyContact);

        }

        private void txtEmergencyContact_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtEmergencyContact);

        }

        private void txtPostalAddress_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.HighlightTextBox(txtPostalAddress);

        }

        private void txtPostalAddress_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description
            PublicFunctions.Responsivity.UnhighlightTextBox(txtPostalAddress);

        }

        private void lsvVisits_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //Selects the listview item as a 'PatientEntry' so that it may be addressed as one later.
            try
            {

                PatientEntry patient = lsvVisits.SelectedItem as PatientEntry;
                PublicFunctions.Functionality.EnableButton(btnRemove);

            }

            catch { }

        }

        private void txtFamilyName_KeyDown(object sender, KeyEventArgs e)
        {

            //Prevents digits from being entered into the family name field.
            if (PublicFunctions.Responsivity.IsDigitInput(e.Key.ToString()))
                e.Handled = false;

            else
                e.Handled = true;

        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {

            //Prevents digits from being entered into the name field.
            if (PublicFunctions.Responsivity.IsDigitInput(e.Key.ToString()))
                e.Handled = false;

            else
                e.Handled = true;

        }

    }

    class VisitEntry
    {

        //The class used to represent Visits in listviews.
        public string Date { get; set; }
        public string Reason { get; set; }
        public string Visit_ImageCount { set; get; }
        public int ID { get; set; }

    }

}
