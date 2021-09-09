using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace NargesLogs_Server
{

    class DatabaseFunctions
    {

        //OleDbConnection used in multiple functions.
        OleDbConnection Connection;

        public void Connect()
        {

            //Opens the connection with the Database.
            Connection = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source =NargesLogs_Database.accdb");
            Connection.Open();

        }

        public void Disconnect()
        {

            //Closes the connection with the database.
            Connection.Close();

        }

        public List<string> RetrievePatientDetails(string RequiredInfo)
        {

            //Displays the correct feedback on console based on the nature of the command.
            if (RequiredInfo == "ID")
                Console.WriteLine(DateTime.Now + ": Retreiving IDs...");

            else if (RequiredInfo == "Name")
                Console.WriteLine(DateTime.Now + ": Retrieving Names...");

            else if (RequiredInfo == "Family_Name")
                Console.WriteLine(DateTime.Now + ": Retrieving Family Names...");

            else if (RequiredInfo == "Occupation")
                Console.WriteLine(DateTime.Now + ": Retrieving Occupations...");

            else if (RequiredInfo == "Sex")
                Console.WriteLine(DateTime.Now + ": Retrieving Sexes...");

            else if (RequiredInfo == "DoB")
                Console.WriteLine(DateTime.Now + ": Retrieving Dates of Birth...");

            else if (RequiredInfo == "Aboriginal_or_TSI")
                Console.WriteLine(DateTime.Now + ": Retrieving Aboriginal or Torres Strait Islander Information...");

            else if (RequiredInfo == "Home_Phone")
                Console.WriteLine(DateTime.Now + ": Retrieving Home Phones...");

            else if (RequiredInfo == "Mobile_Number")
                Console.WriteLine(DateTime.Now + ": Retrieving Mobile Numbers...");

            else if (RequiredInfo == "Work_Phone")
                Console.WriteLine(DateTime.Now + ": Retrieving Work Phones...");

            else if (RequiredInfo == "Emergency_Contact")
                Console.WriteLine(DateTime.Now + ": Retrieving Emergency Contacts...");

            else if (RequiredInfo == "Medicare_Number")
                Console.WriteLine(DateTime.Now + ": Retrieving Medicare Numbers...");

            else if (RequiredInfo == "Healthcare_Number")
                Console.WriteLine(DateTime.Now + ": Retrieving Healthcare Numbers...");

            else if (RequiredInfo == "Residential_Address")
                Console.WriteLine(DateTime.Now + ": Retrieving Residential Addresses...");

            else if (RequiredInfo == "Postal_Address")
                Console.WriteLine(DateTime.Now + ": Retrieving Postal Addresses...");

            else if (RequiredInfo == "Image_Path")
                Console.WriteLine(DateTime.Now + ": Retrieving Image Paths...");

            //Connects with the database and executes a selection command.
            Connect();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Patient_Details", Connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            //Reads the retrieved info for the appropriate information.
            List<string> Information = new List<string>();

            while (reader.Read())
            {

                Information.Add(reader[RequiredInfo].ToString());

            }

            //Disconnects and returns information.
            Console.WriteLine(DateTime.Now + ": Retrieved.");
            Disconnect();

            return Information;

        }

        public List<string> RetrieveUsernames()
        {

            //Connects and retrieves info from database.
            Console.WriteLine(DateTime.Now + ": Retrieving Usernames...");
            Connect();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Users", Connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            //Reads the usernames fron the retrieved info.
            List<string> usernames = new List<string>();

            while (reader.Read())
            {

                usernames.Add(reader["Username"].ToString());

            }

            //Disconnects and returns info.
            Console.WriteLine(DateTime.Now + ": Usernames retrieved.");
            Disconnect();

            return usernames;

        }

        public List<string> RetrievePasswords()
        {

            //Connects and retrieves info from the database.
            Console.WriteLine(DateTime.Now + ": Retrieving Passwords...");
            Connect();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Users", Connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            //Reads the passwords from the retrieved info.
            List<string> passwords = new List<string>();

            while (reader.Read())
            {

                passwords.Add(reader["Password"].ToString());

            }

            //Disconnects and returns the information.
            Console.WriteLine(DateTime.Now + ": Passwords Retrieved.");
            Disconnect();

            return passwords;

        }

        public List<string> RetrieveVisitInfo(string RequiredInfo)
        {

            //Displays the correct feedback on the console based on nature of the command.
            if (RequiredInfo == "Patient_ID")
                Console.WriteLine(DateTime.Now + ": Retrieving Patient IDs...");

            else if (RequiredInfo == "Image_Paths")
                Console.WriteLine(DateTime.Now + ": Retrieving Visit Image Paths...");

            else if (RequiredInfo == "Date")
                Console.WriteLine(DateTime.Now + ": Retrieving Visit Dates...");

            else if (RequiredInfo == "Visit")
                Console.WriteLine(DateTime.Now + ": Retrieving Visit Reason...");

            else if (RequiredInfo == "Notes")
                Console.WriteLine(DateTime.Now + ": Retrieving Visit Notes...");

            //Connects and retrieves information from the database.
            Connect();

            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Visits", Connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            //Reads the appropriate field from retrieved data.
            List<string> VisitInfo = new List<string>();

            while (reader.Read())
            {

                VisitInfo.Add(reader[RequiredInfo].ToString());

            }

            //Disconnects and returns the info.
            Console.WriteLine(DateTime.Now + ": Retrieved.");
            Disconnect();

            return VisitInfo;

        }

        public void UpdatePatient(string name, string familyname, string occupation, string sex, string dob, string aotsi, string homephone, string mobilephone, string workphone, string emergencycontact, string medicare, string healthcare, string address, string paddress, string newpatient)
        {

            //Connects to database.
            Console.WriteLine(DateTime.Now + ": Updating patient...");
            Connect();

            //Filters parameters to get the correct formats for each.
            name = name.Remove(name.IndexOf("\0"));
            familyname = familyname.Remove(familyname.IndexOf("\0"));
            occupation = occupation.Remove(occupation.IndexOf("\0"));
            sex = sex.Remove(sex.IndexOf("\0"));
            dob = dob.Remove(dob.IndexOf("\0"));
            aotsi = aotsi.Remove(aotsi.IndexOf("\0"));
            homephone = homephone.Remove(homephone.IndexOf("\0"));
            mobilephone = mobilephone.Remove(mobilephone.IndexOf("\0"));
            workphone = workphone.Remove(workphone.IndexOf("\0"));
            emergencycontact = emergencycontact.Remove(emergencycontact.IndexOf("\0"));
            medicare = medicare.Remove(medicare.IndexOf("\0"));
            healthcare = healthcare.Remove(healthcare.IndexOf("\0"));
            address = address.Remove(address.IndexOf("\0"));
            paddress = paddress.Remove(paddress.IndexOf("\0"));
            newpatient = newpatient.Remove(newpatient.IndexOf("\0"));

            //Either updates an existing patient or creates a new one based on final parameter.
            if (newpatient.ToLower() == "true")
            {

                //Inserts new entry if new patient.
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Patient_Details(Name, Family_Name, Occupation, Sex, DoB, Aboriginal_or_TSI, Home_Phone, Mobile_Number, Work_Phone, Emergency_Contact, Medicare_Number, Healthcare_Number, Residential_Address, Postal_Address) VALUES('" + name + "', '" + familyname + "', '" + occupation + "', '" + sex + "', '" + dob + "', '" + aotsi + "', '" + homephone + "', '" + mobilephone + "', '" + workphone + "', '" + emergencycontact + "', '" + medicare + "', '" + healthcare + "', '" + address + "', '" + paddress + "')", Connection);
                OleDbDataReader reader = cmd.ExecuteReader();

            }

            else
            {

                //Updates information if not new patient.
                string ID = newpatient.Substring(5);
                OleDbCommand cmd = new OleDbCommand("UPDATE Patient_Details SET Name = '" + name + "', Family_Name = '" + familyname + "', Occupation = '" + occupation + "', Sex = '" + sex + "', DoB = '" + dob + "', Aboriginal_or_TSI = '" + aotsi + "', Home_Phone = '" + homephone + "', Mobile_Number = '" + mobilephone + "', Work_Phone = '" + workphone + "', Emergency_Contact = '" + emergencycontact + "', Medicare_Number = '" + mobilephone + "', Healthcare_Number = '" + healthcare + "', Residential_Address = '" + address + "', Postal_Address = '" + paddress + "' WHERE ID = " + ID, Connection);
                OleDbDataReader reader = cmd.ExecuteReader();

            }

            //Disconnects.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Patient updated.");

        }

        public void RemovePatient(string ID)
        {

            //Connects to database.
            Console.WriteLine(DateTime.Now + ": Removing patient...");
            Connect();

            //Deletes all patient entries with the specified ID.
            OleDbCommand cmd = new OleDbCommand("DELETE FROM Patient_Details WHERE [ID] = " + ID, Connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            //Deletes all Visit entries with the specified owner patient ID.
            cmd = new OleDbCommand("DELETE FROM Visits WHERE [Patient_ID] = '" + ID + "'", Connection);
            reader = cmd.ExecuteReader();

            //Disconnects.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Patient removed.");

        }

        public void UpdateClientImage(string imagepath, string firstname, string lastname)
        {

            //Connects to database.
            Console.WriteLine(DateTime.Now + ": Updating patient image...");
            Connect();

            //Updates the correct enry's imagepath field with parameter.
            OleDbCommand cmd = new OleDbCommand("UPDATE Patient_Details SET Image_Path = '" + imagepath + "' WHERE [Name] = '" + firstname + "'", Connection);
            OleDbDataReader reader = cmd.ExecuteReader();
            
            //Disconnect.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Patient image updated.");

        }

        public string CreateVisit(string Date, string Reason, string PatientID)
        {

            //Connects to database.
            Console.WriteLine(DateTime.Now + ": Creating visit...");
            Connect();

            //Creates a new entry using parameter information.
            OleDbCommand cmd = new OleDbCommand("INSERT INTO Visits([Patient_ID], [Date], [Reason]) VALUES('" + PatientID + "', '" + Date + "', '" + Reason + "')", Connection);
            OleDbDataReader reader = cmd.ExecuteReader();
            cmd = new OleDbCommand("SELECT * FROM Visits", Connection);
            reader = cmd.ExecuteReader();

            //Retireves the allocated ID.
            string id;
            reader.Read();

            while (true)
            {

                id = reader["ID"].ToString();
                if (reader.Read() == false)
                    break;

            }

            //Disconnects and returns the new ID.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Visit created.");
            return id;

        }

        public void UpdaterVisitInfo(string ID, string Date, string Reason)
        {

            //Connects to the database.
            Console.WriteLine(DateTime.Now + ": Updating visit information...");
            Connect();

            //Updates the information using parameters.
            OleDbCommand cmd = new OleDbCommand("UPDATE Visits SET [Date] = '" + Date + "', [Reason] = '" + Reason + "' WHERE [ID] = " + ID, Connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            //Disconnects.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Visit information updated.");

        }
        
        public void UpdateVisitImages(string imagepath, string ID)
        {

            //Connects to the database and retrieves the correct entry.
            Console.WriteLine(DateTime.Now + ": Updating visit image...");
            Connect();

            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Visits WHERE [ID] = " + ID, Connection);
            OleDbDataReader reader = cmd.ExecuteReader();
            reader.Read();

            //Reads the owner ID and the imagepath of the retrieved entry
            string UserID = reader["Patient_ID"].ToString();
            string Imagepaths = reader["Image_Paths"].ToString();

            //Creates the correct prefix using this information.
            //Adds the imagepath with the prefix to the imagepath string.
            Imagepaths += "~~U" + UserID + "V" + ID + imagepath + "~~NEXT_IMAGE_PATH";
            
            //Updates the imagepath field with the new string.
            cmd = new OleDbCommand("UPDATE Visits SET Image_Paths = '" + Imagepaths + "' WHERE [ID] = " + ID, Connection);
            reader = cmd.ExecuteReader();

            //Disconnects.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Visit image updated.");

        }

        public void RemoveVisitImage(string imagepath, string ID)
        {

            //Connects to the database.
            Console.WriteLine(DateTime.Now + ": Removeing visit image...");
            Connect();

            //Retrieves the correct visit.
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Visits WHERE [ID] = " + ID, Connection);
            OleDbDataReader reader = cmd.ExecuteReader();
            reader.Read();

            //Sets the correct visit's imagepath as the function's parameter.
            cmd = new OleDbCommand("UPDATE Visits SET Image_Paths = '" + imagepath + "' WHERE [ID] = " + ID, Connection);
            reader = cmd.ExecuteReader();

            //Disconnects.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Visit image removed.");

        }

        public void UpdateVisitNotes(string path, string ID)
        {

            //Connects to the database.
            Console.WriteLine(DateTime.Now + ": Updating visit notes...");
            Connect();

            //Updates the correct entry's notes to the parameter of the function.
            OleDbCommand cmd = new OleDbCommand("UPDATE Visits SET Notes = '" + path + "' WHERE [ID] = " + ID, Connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            //Disconnects.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Visit notes updated.");

        }

        public void RemoveVisit(string id)
        {

            //Connects to the database.
            Console.WriteLine(DateTime.Now + ": Removing visit...");
            Connect();

            //Deletes entry with the correct ID.
            OleDbCommand cmd = new OleDbCommand("DELETE FROM Visits WHERE [ID] = " + id, Connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            //Disconnects.
            Disconnect();
            Console.WriteLine(DateTime.Now + ": Visit removed.");

        }

    }

}
