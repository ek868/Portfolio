using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading;

namespace NargesLogs
{

    public partial class Patients_List : Window
    {

        public Patients_List()
        {

            InitializeComponent();

        }

        //-------------------------------------------------txtName------------------------------------------------------------------------------------------------------------//

        private void txtName_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.HighlightTextBox(txtName);

        }

        private void txtName_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.UnhighlightTextBox(txtName);

        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.SelectTextBox(txtName, "[Enter Patient Name]");

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------Window-------------------------------------------------------------------------------------------------------------//

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.DeselectTextBox(txtName, "[Enter Patient Name]", btnSearch);

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //------------------------------------------------btnAdd--------------------------------------------------------------------------------------------------------------//

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            //Ensures that the Patient Details window is able to determine that this is a new patient, and can handle it correctly.
            CurrentPatient.NewPatient = true;

            //Opens the Patient Details window on the correct spot.
            Patient_Details pd = new Patient_Details();
            pd.Owner = this;
            pd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            pd.Show();
            pd.Owner = null;
            Misc.WindowSwitch = true;
            Close();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //-----------------------------------------------ListView-------------------------------------------------------------------------------------------------------------//

        private void ListView_MouseEnter(object sender, MouseEventArgs e)
        {

            //Ensures that the user gets approriate feedback when the list is loading.
            Cursor = Cursors.Wait;

        }

        private void ListView_MouseLeave(object sender, MouseEventArgs e)
        {

            //As above.
            Cursor = Cursors.Arrow;

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

            //Closes this window and opens the home page on the same position of the screen.
            Home_Page hp = new Home_Page();
            hp.Owner = this;
            hp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            hp.Show();
            hp.Owner = null;
            Misc.WindowSwitch = true;
            Close();

        }

        private void imgLogo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Does the same thing as the 'Back' button.
            Home_Page hp = new Home_Page();
            hp.Owner = this;
            hp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            hp.Show();
            hp.Owner = null;
            Misc.WindowSwitch = true;
            Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Has a different thread load the list so that the UI doesn't hang while this happens.
            PublicFunctions.Functionality.DisableButton(btnRemove);
            Thread LoadListThread = new Thread(LoadList);
            LoadListThread.Start();

        }

        public void LoadList()
        {

            //Ensures that the server is not accessed while information is being retrieved by this thread using a boolean variable.
            Misc.SearchConducted = false;

            ConnectionFunctions CFunctions = new ConnectionFunctions();

            //Collects each piece of information regarding the current date to use later to determine the age of each patient.
            int currentyear = int.Parse(DateTime.Today.ToString().Substring(DateTime.Today.ToString().LastIndexOf("/")).Substring(1).Remove(4));
            int currentmonth = int.Parse(DateTime.Today.ToString().Substring(DateTime.Today.ToString().IndexOf("/")).Substring(1).Remove(DateTime.Today.ToString().Substring(DateTime.Today.ToString().IndexOf("/")).Substring(1).IndexOf("/")));
            int currentday = int.Parse(DateTime.Today.ToString().Remove(DateTime.Today.ToString().IndexOf("/")));

            //Collects and stores all patient information.
            RetrievedInfo.IDs = CFunctions.SendGenericCommand("3214~COMMAND#3");
            RetrievedInfo.names = CFunctions.SendGenericCommand("3214~COMMAND#4");
            RetrievedInfo.familynames = CFunctions.SendGenericCommand("3214~COMMAND#5");
            RetrievedInfo.occupations = CFunctions.SendGenericCommand("3214~COMMAND#6");
            RetrievedInfo.sexes = CFunctions.SendGenericCommand("3214~COMMAND#7");
            RetrievedInfo.DoBs = CFunctions.SendGenericCommand("3214~COMMAND#8");
            RetrievedInfo.AoTSI = CFunctions.SendGenericCommand("3214~COMMAND#9");
            RetrievedInfo.HomePhone = CFunctions.SendGenericCommand("3214~COMMAND#10");
            RetrievedInfo.MobilePhone = CFunctions.SendGenericCommand("3214~COMMAND#11");
            RetrievedInfo.WorkPhone = CFunctions.SendGenericCommand("3214~COMMAND#12");
            RetrievedInfo.EmergencyContact = CFunctions.SendGenericCommand("3214~COMMAND#13");
            RetrievedInfo.Medicare = CFunctions.SendGenericCommand("3214~COMMAND#14");
            RetrievedInfo.Healthcare = CFunctions.SendGenericCommand("3214~COMMAND#15");
            RetrievedInfo.ResidentialAddress = CFunctions.SendGenericCommand("3214~COMMAND#16");
            RetrievedInfo.PostalAddress = CFunctions.SendGenericCommand("3214~COMMAND#17");

            //Collects and stores all basic visit information.
            RetrievedInfo.Visit_PatientIDs = CFunctions.SendGenericCommand("3214~COMMAND#19");
            RetrievedInfo.Visit_Dates = CFunctions.SendGenericCommand("3214~COMMAND#20");
            RetrievedInfo.Visit_Reasons = CFunctions.SendGenericCommand("3214~COMMAND#21");
            RetrievedInfo.Visit_IDs = CFunctions.SendGenericCommand("3214~COMMAND#23");

            //Goes through each patient to conduct the process of loading them in each as a list item.
            for (int i = 0; i < RetrievedInfo.IDs.Count(); i++)
            {

                try
                {
                    
                    //'PatientEntry' class is used to create listview item, and loads the information of the next patient into the properties of this class object.
                    PatientEntry patient = new PatientEntry();
                    patient.ID = RetrievedInfo.IDs[i];
                    patient.Name = RetrievedInfo.names[i] + " " + RetrievedInfo.familynames[i];
                    patient.Sex = RetrievedInfo.sexes[i];

                    //List for containing all current patient's visits.
                    List<int> collectedvisits = new List<int>();

                    //Adds all relevant visits using a loop.
                    for (int s = 0; s < RetrievedInfo.Visit_PatientIDs.Count(); s++)
                    {

                        if (RetrievedInfo.Visit_PatientIDs[s] == RetrievedInfo.IDs[i])
                        {

                            patient.Visits++;
                            collectedvisits.Add(s);

                        }

                    }

                    //If the patient has had any visits, the reason for the last one is displayed as the latest condition.
                    if (collectedvisits.Count > 0)
                        patient.Condition = RetrievedInfo.Visit_Reasons[collectedvisits.Last()];

                    //Otherwise, nothing is displayed as the latest condition.
                    else
                        patient.Condition = "";

                    //The birthday is extracted.
                    int birthday = int.Parse(RetrievedInfo.DoBs[i].Remove(RetrievedInfo.DoBs[i].IndexOf("/")));
                    int birthmonth = int.Parse(RetrievedInfo.DoBs[i].Substring(RetrievedInfo.DoBs[i].IndexOf("/")).Substring(1).Remove(RetrievedInfo.DoBs[i].Substring(RetrievedInfo.DoBs[i].IndexOf("/")).Substring(1).IndexOf("/")));
                    int birthyear = int.Parse(RetrievedInfo.DoBs[i].Substring(RetrievedInfo.DoBs[i].LastIndexOf("/")).Substring(1).Remove(4));

                    //The age is calculated.
                    patient.Age = (currentyear - birthyear) - 1;

                    if (birthmonth < currentmonth || (birthmonth == currentmonth && birthday >= currentday))
                    {

                        patient.Age++;

                    }

                    //The item is added to the list.
                    Dispatcher.Invoke(() =>
                    {

                        lsvPatients.Items.Add(patient);

                    });

                }

                catch { }

            }

            Dispatcher.Invoke(() =>
            {

                lsvPatients.Cursor = Cursors.Arrow;

            });

            Thread.CurrentThread.Abort();

        }

        public void EntryPressed (object sender)
        {

            //Loads the selected listview item as a 'PatientEntry'.
            Mouse.SetCursor(Cursors.Wait);
            PatientEntry selectedpatient = new PatientEntry();
            selectedpatient = (PatientEntry)sender;

            try
            {

                //Tries to see if this is a valid patient, as the user may have double clicked blank space in the listview.
                CurrentPatient.ID = selectedpatient.ID;

            }

            catch
            {

                return;

            }

            //Finds the position of the patient within the database.
            for (int i = 0; i < RetrievedInfo.IDs.Count(); i++)
            {

                if (RetrievedInfo.IDs[i] == CurrentPatient.ID)
                    CurrentPatient.DbasePosition = i;

            }

            //Using this information, it saves the correct information into 'CurrentPatient'.
            CurrentPatient.Name = RetrievedInfo.names[CurrentPatient.DbasePosition];
            CurrentPatient.FamilyName = RetrievedInfo.familynames[CurrentPatient.DbasePosition];
            CurrentPatient.Occupation = RetrievedInfo.occupations[CurrentPatient.DbasePosition];
            CurrentPatient.Sex = RetrievedInfo.sexes[CurrentPatient.DbasePosition];
            CurrentPatient.DoB = RetrievedInfo.DoBs[CurrentPatient.DbasePosition];

            if (RetrievedInfo.AoTSI[CurrentPatient.DbasePosition] == "Yes")
                CurrentPatient.AoTSI = true;
            else
                CurrentPatient.AoTSI = false;

            CurrentPatient.HomePhone = RetrievedInfo.HomePhone[CurrentPatient.DbasePosition];
            CurrentPatient.MobilePhone = RetrievedInfo.MobilePhone[CurrentPatient.DbasePosition];
            CurrentPatient.WorkPhone = RetrievedInfo.WorkPhone[CurrentPatient.DbasePosition];
            CurrentPatient.EmergencyContact = RetrievedInfo.EmergencyContact[CurrentPatient.DbasePosition];
            CurrentPatient.Medicare = RetrievedInfo.Medicare[CurrentPatient.DbasePosition];
            CurrentPatient.Healthcare = RetrievedInfo.Healthcare[CurrentPatient.DbasePosition];
            CurrentPatient.ResidentialAddress = RetrievedInfo.ResidentialAddress[CurrentPatient.DbasePosition];
            CurrentPatient.PostalAddress = RetrievedInfo.PostalAddress[CurrentPatient.DbasePosition];
            CurrentPatient.NewPatient = false;

            Mouse.SetCursor(Cursors.Arrow);

            //With this data now stored and accessable via the patient details window, the window is closed and the patient details window is opened.
            Patient_Details pd = new Patient_Details();

            pd.Owner = this;
            pd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            pd.Show();
            pd.Owner = null;
            Misc.WindowSwitch = true;
            Close();

        }

        private void lsvPatients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //Reads the selected list view item as a 'PatientEntry'.
            PatientEntry patient = lsvPatients.SelectedItem as PatientEntry;
            EntryPressed(patient);

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

            //Reads the selected item as a 'PatientEntry' and sends its information to the server to be deleted.
            ConnectionFunctions CFunctions = new ConnectionFunctions();
            PatientEntry patient = lsvPatients.SelectedItem as PatientEntry;
            CFunctions.RemovePatient(patient.ID);

            //Removes the item from the list.
            lsvPatients.Items.Remove(lsvPatients.SelectedItem);

            //If there are no more items left, it disables the remove button to prevent it being pressed with no entry selected.
            if (lsvPatients.Items.Count < 1)
                PublicFunctions.Functionality.DisableButton(btnRemove);

        }

        private void Window_Closed(object sender, EventArgs e)
        {

            //Clears temporary folders and properly closes the connection with the server.
            PublicFunctions.Functionality.ClearTempFiles();

            if (Misc.WindowSwitch == false)
            {

                ConnectionFunctions CFunctions = new ConnectionFunctions();
                CFunctions.CloseConnection();

            }

            else
                Misc.WindowSwitch = false;

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            //The initial search term is a space so that all entries will be loaded if nothing is entered into the search tab.
            string searchterm = " ";

            //Sets the search term to the text in the search bar if the text is not the default display text.
            if (txtName.Text != "" && txtName.Text != "[Enter Patient Name]")
                searchterm = txtName.Text;

            //If the search is being conducted for the first time, goes through each list entry and removes them if they do not include the search term.
            if(Misc.SearchConducted == false)
            {

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    Misc.PreSearchEntries.Add((PatientEntry)lsvPatients.Items[i]);

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    if (((PatientEntry)lsvPatients.Items[i]).Name.Contains(searchterm) == false)
                    {

                        lsvPatients.Items.RemoveAt(i);
                        i = -1;

                    }

                Misc.SearchConducted = true;

            }

            //If the search is being conducted for a second time, clears the list, and goes through a list of all patient entries, adding the entries that include the search term.
            else
            {

                lsvPatients.Items.Clear();

                for(int i = 0; i < Misc.PreSearchEntries.Count; i++)
                {

                    if(Misc.PreSearchEntries[i].Name.Contains(searchterm))
                        lsvPatients.Items.Add(Misc.PreSearchEntries[i]);

                }

            }

        }

        //Variables used for determining if the sort has already been conducted once, in which case it will need to be conducted in reverse.
        public bool IDsorted = false;
        public bool AgeSorted = false;
        public bool VisitsSorted = false;
        public bool NamesSorted = false;
        public bool SexSorted = false;
        public bool ConditionSorted;

        private void hdrID_Click(object sender, RoutedEventArgs e)
        {

            //If the list is not already sorted by ID, 
            if(IDsorted == false)
            {

                List<string> IDs = new List<string>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                //Adds all items to 'PatientEntries',
                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                //Keeps a copy of the list.
                PatientEntry[] OriginalEntries = new PatientEntry[PatientEntries.Count];
                PatientEntries.CopyTo(OriginalEntries);

                //Clears the listview.
                lsvPatients.Items.Clear();

                //Uses Selection sort to sort throught the IDs and add them back to the listview.
                int highestID = 0;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (int.Parse(PatientEntries[i].ID) > highestID)
                            highestID = int.Parse(PatientEntries[i].ID);

                    IDs.Add(highestID.ToString());

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].ID == highestID.ToString())
                            PatientEntries.RemoveAt(i);
                    highestID = 0;

                }

                for (int i = 0; i < IDs.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (IDs[i] == OriginalEntries[x].ID)
                            lsvPatients.Items.Add(OriginalEntries[x]);

                IDsorted = true;

            }

            //If the sort is being conducted a second time, inverses the sort.
            else
            {
                
                //Algorithm is the same but reverse.
                List<string> IDs = new List<string>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                PatientEntry[] OriginalEntries = new PatientEntry[PatientEntries.Count];
                PatientEntries.CopyTo(OriginalEntries);

                lsvPatients.Items.Clear();

                int lowestID = 9999999;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (int.Parse(PatientEntries[i].ID) < lowestID)
                            lowestID = int.Parse(PatientEntries[i].ID);

                    IDs.Add(lowestID.ToString());

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].ID == lowestID.ToString())
                            PatientEntries.RemoveAt(i);

                    lowestID = 9999999;

                }

                for (int i = 0; i < IDs.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (IDs[i] == OriginalEntries[x].ID)
                            lsvPatients.Items.Add(OriginalEntries[x]);

                IDsorted = false;

            }

        }

        private void hdrAge_Click(object sender, RoutedEventArgs e)
        {

            //Age is sorted the same way as ID, see above for description of algorithm - Selection Sort.
            if (AgeSorted == false)
            {

                List<int> Ages = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int highestAge = -9999;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].Age > highestAge)
                            highestAge = PatientEntries[i].Age;

                    Ages.Add(highestAge);

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].Age == highestAge)
                        {

                            PatientEntries.RemoveAt(i);
                            break;

                        }

                    highestAge = 0;

                }

                for (int i = 0; i < Ages.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (Ages[i] == OriginalEntries[x].Age)
                        {

                            lsvPatients.Items.Add(OriginalEntries[x]);
                            OriginalEntries.RemoveAt(x);
                            x--;
                            break;

                        }

                AgeSorted = true;

            }

            else
            {

                List<int> Ages = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int lowestAge = 9999;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].Age < lowestAge)
                            lowestAge = PatientEntries[i].Age;

                    Ages.Add(lowestAge);

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].Age == lowestAge)
                        {

                            PatientEntries.RemoveAt(i);
                            break;

                        }

                    lowestAge = 9999;

                }

                for (int i = 0; i < Ages.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (Ages[i] == OriginalEntries[x].Age)
                        {

                            lsvPatients.Items.Add(OriginalEntries[x]);
                            OriginalEntries.RemoveAt(x);
                            x--;
                            break;

                        }

                AgeSorted = false;

            }
        }
        
        private void hdrVisit_Click(object sender, RoutedEventArgs e)
        {

            //The number of visits sort is conducted the same way as the ID, please see above for a description of the algorithm - Selection Sort.
            if (VisitsSorted == false)
            {

                List<int> Visits = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int highestVisits = -1;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].Visits > highestVisits)
                            highestVisits = PatientEntries[i].Visits;

                    Visits.Add(highestVisits);

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].Visits == highestVisits)
                        { 
                            PatientEntries.RemoveAt(i);
                            break;
                        }

                    highestVisits = -1;

                }

                for (int i = 0; i < Visits.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (Visits[i] == OriginalEntries[x].Visits)
                        {

                            lsvPatients.Items.Add(OriginalEntries[x]);
                            OriginalEntries.RemoveAt(x);
                            x--;
                            break;

                        }

                VisitsSorted = true;

            }

            else
            {

                List<int> Visits = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int lowestVisits = 999999;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].Visits < lowestVisits)
                            lowestVisits = PatientEntries[i].Visits;

                    Visits.Add(lowestVisits);

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PatientEntries[i].Visits == lowestVisits)
                        {

                            PatientEntries.RemoveAt(i);
                            break;

                        }

                    lowestVisits = 999999;

                }

                for (int i = 0; i < Visits.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (Visits[i] == OriginalEntries[x].Visits)
                        {

                            lsvPatients.Items.Add(OriginalEntries[x]);
                            OriginalEntries.RemoveAt(x);
                            x--;
                            break;

                        }

                VisitsSorted = false;

            }

        }
        
        private void hdrName_Click(object sender, RoutedEventArgs e)
        {

            //The first letter of the name is assigned a numeric alphabetic value, which is then used to conduct the sort in the same way as the ID sort - Selection Sort.
            if (NamesSorted == false)
            {

                List<int> Names = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int lowest = 999999;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Name.Substring(PatientEntries[i].Name.IndexOf(" ") + 1).ToCharArray()[0].ToString()) < lowest)
                            lowest = PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Name.Substring(PatientEntries[i].Name.IndexOf(" ") + 1).ToCharArray()[0].ToString());

                    Names.Add(lowest);

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Name.Substring(PatientEntries[i].Name.IndexOf(" ") + 1).ToCharArray()[0].ToString()) == lowest)
                        {

                            PatientEntries.RemoveAt(i);
                            break;

                        }

                    lowest = 999999;

                }

                for (int i = 0; i < Names.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (Names[i] == PublicFunctions.Functionality.LetterToNumber(OriginalEntries[x].Name.Substring(OriginalEntries[x].Name.IndexOf(" ") + 1).ToCharArray()[0].ToString()))
                        {

                            lsvPatients.Items.Add(OriginalEntries[x]);
                            OriginalEntries.RemoveAt(x);
                            x--;
                            break;

                        }

                NamesSorted = true;

            }

            else
            {

                List<int> Names = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int highest = -1;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Name.Substring(PatientEntries[i].Name.IndexOf(" ") + 1).ToCharArray()[0].ToString()) > highest)
                            highest = PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Name.Substring(PatientEntries[i].Name.IndexOf(" ") + 1).ToCharArray()[0].ToString());

                    Names.Add(highest);

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Name.Substring(PatientEntries[i].Name.IndexOf(" ") + 1).ToCharArray()[0].ToString()) == highest)
                        {

                            PatientEntries.RemoveAt(i);
                            break;

                        }

                    highest = -1;

                }

                for (int i = 0; i < Names.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (Names[i] == PublicFunctions.Functionality.LetterToNumber(OriginalEntries[x].Name.Substring(OriginalEntries[x].Name.IndexOf(" ") + 1).ToCharArray()[0].ToString()))
                        {

                            lsvPatients.Items.Add(OriginalEntries[x]);
                            OriginalEntries.RemoveAt(x);
                            x--;
                            break;

                        }

                NamesSorted = false;

            }

        }
        
        private void hdrSex_Click(object sender, RoutedEventArgs e)
        {

            //This information is sorted alphabetically the same way as the Name algorithm, please view the ID algorithm for a description of the algorithm, and the Name algorithm
            //for a description of how the ID algorithm is adapted for letter based information.
            if (SexSorted == false)
            {

                List<int> Sexes = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int lowest = 999999;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Sex.ToCharArray()[0].ToString()) < lowest)
                            lowest = PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Sex.ToCharArray()[0].ToString());

                    Sexes.Add(lowest);

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Sex.ToCharArray()[0].ToString()) == lowest)
                        {

                            PatientEntries.RemoveAt(i);
                            break;

                        }

                    lowest = 999999;

                }

                for (int i = 0; i < Sexes.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (Sexes[i] == PublicFunctions.Functionality.LetterToNumber(OriginalEntries[x].Sex.ToCharArray()[0].ToString()))
                        {

                            lsvPatients.Items.Add(OriginalEntries[x]);
                            OriginalEntries.RemoveAt(x);
                            x--;
                            break;

                        }

                SexSorted = true;

            }

            else
            {

                List<int> Names = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int highest = -1;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Sex.ToCharArray()[0].ToString()) > highest)
                            highest = PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Sex.ToCharArray()[0].ToString());

                    Names.Add(highest);

                    for (int i = 0; i < PatientEntries.Count; i++)
                        if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Sex.ToCharArray()[0].ToString()) == highest)
                        {

                            PatientEntries.RemoveAt(i);
                            break;

                        }

                    highest = -1;

                }

                for (int i = 0; i < Names.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                        if (Names[i] == PublicFunctions.Functionality.LetterToNumber(OriginalEntries[x].Sex.ToCharArray()[0].ToString()))
                        {

                            lsvPatients.Items.Add(OriginalEntries[x]);
                            OriginalEntries.RemoveAt(x);
                            x--;
                            break;

                        }

                SexSorted = false;

            }

        }
        
        private void hdrCondition_Click(object sender, RoutedEventArgs e)
        {

            //This information is sorted alphabetically the same way as the Name algorithm, please view the ID algorithm for a description of the algorithm, and the Name algorithm
            //for a description of how the ID algorithm is adapted for letter based information. (Note that some minor adjustments are outlined in this version.)
            if (ConditionSorted == false)
            {

                List<int> Conditions = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int lowest = 999999;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                    {

                        if (PatientEntries[i].Condition.ToCharArray().Count() > 0)
                        {

                            if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Condition.ToCharArray()[0].ToString()) < lowest)
                                lowest = PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Condition.ToCharArray()[0].ToString());

                        }

                        //Ensures that patient entries with null latest condition are considered before 'A'.
                        else
                            lowest = 0;

                    }


                    Conditions.Add(lowest);

                    for (int i = 0; i < PatientEntries.Count; i++)
                    {

                        if (PatientEntries[i].Condition.ToCharArray().Count() != 0)
                        {

                            if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Condition.ToCharArray()[0].ToString()) == lowest)
                            {

                                PatientEntries.RemoveAt(i);
                                break;

                            }

                        }

                        else
                        {

                            if (lowest == 0)
                            {

                                PatientEntries.RemoveAt(i);
                                break;

                            }

                        }

                    }

                    lowest = 999999;

                }

                for (int i = 0; i < Conditions.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                    {

                        if(OriginalEntries[x].Condition.ToCharArray().Count() == 0)
                        {

                            if(Conditions[i] == 0)
                            {

                                lsvPatients.Items.Add(OriginalEntries[x]);
                                OriginalEntries.RemoveAt(x);
                                x--;
                                break;

                            }

                        }

                        else
                        {

                            if (Conditions[i] == PublicFunctions.Functionality.LetterToNumber(OriginalEntries[x].Condition.ToCharArray()[0].ToString()))
                            {

                                lsvPatients.Items.Add(OriginalEntries[x]);
                                OriginalEntries.RemoveAt(x);
                                x--;
                                break;

                            }

                        }

                    }

                ConditionSorted = true;

            }

            else
            {

                List<int> Conditions = new List<int>();
                List<PatientEntry> PatientEntries = new List<PatientEntry>();

                for (int i = 0; i < lsvPatients.Items.Count; i++)
                    PatientEntries.Add((PatientEntry)lsvPatients.Items[i]);

                List<PatientEntry> OriginalEntries = new List<PatientEntry>();

                for (int i = 0; i < PatientEntries.Count; i++)
                    OriginalEntries.Add(PatientEntries[i]);

                lsvPatients.Items.Clear();

                int highest = -1;

                while (PatientEntries.Count != 0)
                {

                    for (int i = 0; i < PatientEntries.Count; i++)
                    {

                        if(PatientEntries[i].Condition.ToCharArray().Count() > 0)
                        {

                            if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Condition.ToCharArray()[0].ToString()) > highest)
                            {

                                highest = PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Condition.ToCharArray()[0].ToString());

                            }

                        }

                        else
                        {

                            if (0 > highest)
                                highest = 0;

                        }

                    }

                    Conditions.Add(highest);

                    for (int i = 0; i < PatientEntries.Count; i++)
                    {

                        if (PatientEntries[i].Condition.ToCharArray().Count() != 0)
                        {

                            if (PublicFunctions.Functionality.LetterToNumber(PatientEntries[i].Condition.ToCharArray()[0].ToString()) == highest)
                            {

                                PatientEntries.RemoveAt(i);
                                break;

                            }

                        }

                        else
                        {

                            if (highest == 0)
                            {

                                PatientEntries.RemoveAt(i);
                                break;

                            }

                        }

                    }

                    highest = -1;

                }

                for (int i = 0; i < Conditions.Count; i++)
                    for (int x = 0; x < OriginalEntries.Count(); x++)
                    {

                        if (OriginalEntries[x].Condition.ToCharArray().Count() == 0)
                        {

                            if(Conditions[i] == 0)
                            {

                                lsvPatients.Items.Add(OriginalEntries[x]);
                                OriginalEntries.RemoveAt(x);
                                x--;
                                break;

                            }

                        }

                        else
                        {

                            if (Conditions[i] == PublicFunctions.Functionality.LetterToNumber(OriginalEntries[x].Condition.ToCharArray()[0].ToString()))
                            {

                                lsvPatients.Items.Add(OriginalEntries[x]);
                                OriginalEntries.RemoveAt(x);
                                x--;
                                break;

                            }

                        }

                    }

                ConditionSorted = false;

            }

        }

        private void lsvPatients_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            try
            {

                //Tries to load the selected item as a 'PatientEntry' class object.
                PatientEntry patient = lsvPatients.SelectedItem as PatientEntry;
                PublicFunctions.Functionality.EnableButton(btnRemove);

            }

            catch { }

        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {

            //Ensures that the search button is pressed if the user presses 'Enter' when typing into the search bar - for UI fluidity.
            if (e.Key.ToString() == "Return")
                btnSearch_Click(this, null);

        }



        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------//
    }

    class PatientEntry
    {

        //The class used to represent patients and their information as listview items. Each property is a column in the listview.
        public string ID { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public int Visits { get; set; }
        public string Condition { get; set; }

    }

}
