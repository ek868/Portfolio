using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NargesLogs
{

    static class ConnectionInfo
    {

        //Connection settings.
        public static string IPAddress = "localhost";
        public static int Port = 3214;

    }

    class RetrievedInfo
    {

        //Downloaded information.
        public static string[] usernames;
        public static string[] passwords;
        public static string[] IDs;
        public static string[] names;
        public static string[] familynames;
        public static string[] occupations;
        public static string[] sexes;
        public static string[] DoBs;
        public static string[] AoTSI;
        public static string[] HomePhone;
        public static string[] MobilePhone;
        public static string[] WorkPhone;
        public static string[] EmergencyContact;
        public static string[] Medicare;
        public static string[] Healthcare;
        public static string[] ResidentialAddress;
        public static string[] PostalAddress;
        public static string[] Visit_Dates;
        public static string[] Visit_Reasons;
        public static string[] Visit_PatientIDs;
        public static string[] Visit_IDs;

    }

    static class CurrentPatient
    {

        //The information of the currently open patient.
        static public int DbasePosition;
        static public string ID;
        static public string Name;
        static public string FamilyName;
        static public string Sex;
        static public string DoB;
        static public string Occupation;
        static public bool AoTSI;
        static public string HomePhone;
        static public string MobilePhone;
        static public string WorkPhone;
        static public string EmergencyContact;
        static public string Medicare;
        static public string Healthcare;
        static public string ResidentialAddress;
        static public string PostalAddress;
        static public bool NewPatient;

    }

    static class CurrentPatient_Visits
    {

        //The visits information of the current patient.
        static public List<string> Dates = new List<string>();
        static public List<string> Reasons = new List<string>();
        static public List<int> IDs = new List<int>();
        static public int Count = 0;

    }

    static class CurrentVisit
    {

        //The information of the current visit.
        static public string Date;
        static public string Reason;
        static public int ID;
        static public bool NewVisit;

    }

    static class Misc
    {

        //Other useful variables that are accessed all throughout the application.
        static public bool ProfileImageIsDone = true;
        static public bool ImageChanged = true;
        static public bool SearchConducted = false;
        static public List<PatientEntry> PreSearchEntries = new List<PatientEntry>();
        static public bool WindowSwitch = false;

    }

}
