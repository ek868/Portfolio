using System;
using System.Linq;
using System.IO;

namespace NargesLogs_Server
{

    class Program
    {

        static void Main(string[] args)
        {

            ConnectionFunctions CFunctions = new ConnectionFunctions();

            Console.WriteLine("NargesLogs is property of Eilia Keyhanee - © Eilia Keyhanee 2018. All Rights Reserved.");
            Console.WriteLine("NargesLogs Server v1.5");

            //Saves the password entered.
            Console.Write("Password: ");
            Global_Information.password = Console.ReadLine();

            ClearCurrentConsoleLine();

            Console.WriteLine(" ");

            Console.WriteLine("Creating backup of database...");
            //Backs up database.
            BackupDataBase();
            Console.WriteLine("Backup of database Created.");

            Console.WriteLine(" ");

            //Decryptes database using password.
            Console.WriteLine("Decrypting database...");
            DecryptDataBase(Global_Information.password, 3214);
            Console.WriteLine("Decryption complete.");

            Console.WriteLine(" ");

            Console.WriteLine("Enter 'help' for a list of available commands.");

            Console.WriteLine(" ");

            //Launches server.
            Console.WriteLine("Server Report:");
            Console.WriteLine(DateTime.Now + ": Launching Server...");
            CFunctions.Launch();
            Console.ReadLine();

        }

        public static void ClearCurrentConsoleLine()
        {

            //Clears the line of the console.
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);

        }

        public static void BackupDataBase()
        {

            //Copies the emcrypted database to backup folder.
            using(FileStream openstream = new FileStream(Directory.GetCurrentDirectory() + @"\EncryptedDatabase", FileMode.Open))
            {

                byte[] filecopy = new byte[openstream.Length];
                openstream.Read(filecopy, 0, filecopy.Length);
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Backups");
                string date = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year;

                using (FileStream writestream = new FileStream(Directory.GetCurrentDirectory() + @"\Backups\Database_Backup_" + date, FileMode.Create))
                    writestream.Write(filecopy, 0, filecopy.Count());

            }

        }

        public static void EncryptDataBase(string password, double key)
        {

            //Reads the database.
            using(FileStream openstream = new FileStream(Directory.GetCurrentDirectory() + @"\NargesLogs_Database.accdb", FileMode.Open))
            {

                byte[] file = new byte[openstream.Length];
                openstream.Read(file, 0, file.Length);

                //Encrypts the database.
                byte[] encryptedfile = Encryption.Encrypt(file, password, key);

                //Saves it in the same folder.
                using (FileStream writestream = new FileStream(Directory.GetCurrentDirectory() + @"\EncryptedDatabase", FileMode.Create))
                    writestream.Write(encryptedfile, 0, encryptedfile.Length);

            }

            //Deletes decrypted database.
            File.Delete(Directory.GetCurrentDirectory() + @"\NargesLogs_Database.accdb");

            //Backs up the encrypted database.
            BackupDataBase();

        }

        public static void DecryptDataBase(string password, double key)
        {

            //Reads the database.
            using (FileStream openstream = new FileStream(Directory.GetCurrentDirectory() + @"\EncryptedDatabase", FileMode.Open))
            {
                byte[] file = new byte[openstream.Length];
                openstream.Read(file, 0, file.Length);

                //Decrypts the database.
                byte[] decryptedfile = Decryption.Decrypt(file, password, key);

                //Writes it into the same folder.
                using (FileStream writestream = new FileStream(Directory.GetCurrentDirectory() + @"\NargesLogs_Database.accdb", FileMode.Create))
                    writestream.Write(decryptedfile, 0, decryptedfile.Length);

            }

            //Deletes encrypted database.
            File.Delete(Directory.GetCurrentDirectory() + @"\EncryptedDatabase");

        }

    }

}
