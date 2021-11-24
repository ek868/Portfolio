using System;

namespace NargesLogs_Server
{

    class Encryption
    {
        public static byte[] Encrypt(byte[] file, string password, double key)
        {
            file = Split(file, password);
            file = ExtraEncryption(file, password);
            file = BruteForce(file, key);
            return file;
        }

        //The following code is taken from a previous project, also made by Eilia Keyhanee

        public static byte[] Split(byte[] file, string password)
        {

            //splits the file into four pieces, with them remaining bytes stored in the last part.
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

            //Encrypts each of the parts
            byte[] efile1 = ExtraEncryption(file1, password);
            byte[] efile2 = ExtraEncryption(file2, password);
            byte[] efile3 = ExtraEncryption(file3, password);
            byte[] efile4 = ExtraEncryption(file4, password);
            byte[] efile = new byte[file.Length];

            //places all parts back into one array
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

        }

        public static byte[] ExtraEncryption(byte[] file, string password)
        {

            int keypos = 0;
            int pastkeys = 0;

            //converts the password into ineger values, corresponding with the character (see PassConverter)
            byte[] keyarray = PassConverter(password);

            //The value of each of the password's characters are added to the byte values of the file. However, to create a more dynamic system that changes
            //depending on the rest of the password, the password value is multiplied by the position of the character in the password, and the value of all past
            //keys are taken away, the first value is not added into the pastkeys as that would always make the first value to be added 0. Once the last character
            //in the password is reached, the algorithm starts again at position 0.

            for (int i = 0; i < file.Length; i++)
            {

                if (i != 0)
                    pastkeys += i;

                file[i] += (byte)((keyarray[keypos] * (i + 1)) - pastkeys);
                keypos++;
                if (keypos == keyarray.Length)
                    keypos = 0;

            }

            return file;

        }

        public static byte[] BruteForce(byte[] file, double key)
        {

            //splits the key into two parts, with the remainder going to the first part.
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
                keychar1[keychar2.Length + i] = keychar[keychar2.Length + i];

            string keystring1 = new string(keychar1);
            string keystring2 = new string(keychar2);

            //if the second part is empty, turns it to 0 to prevent errors.
            if (keystring2 == "")
                keystring2 = "0";

            //gets the value of each part
            int key1 = int.Parse(keystring1);
            int key2 = int.Parse(keystring2);

            //creates two byte arrays, each with the size of the specified values, and fills them with random bytes.
            Random ByteCreator = new Random();
            byte[] bfile1 = new byte[key1];
            byte[] bfile2 = new byte[key2];

            ByteCreator.NextBytes(bfile1);
            ByteCreator.NextBytes(bfile2);

            //creates a new byte array in which the final file can be stored.
            byte[] bfile = new byte[file.Length + (key1 + key2)];

            //fills the final array with the first fake packet of bytes, the encrypted file, and finally, the second packet of fake bytes..
            for (int i = 0; i < key1; i++)
                bfile[i] = bfile1[i];

            for (int i = 0; i < file.Length; i++)
                bfile[key1 + i] = file[i];

            for (int i = 0; i < key2; i++)
                bfile[key1 + file.Length + i] = bfile2[i];

            return bfile;

        }

        public static byte[] PassConverter(string password)
        {

            //turns the password into a character array.
            Char[] passarray = password.ToCharArray();
            byte[] keyarray = new byte[passarray.Length];

            //goes through character array and retrieves the corresponding number.
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

        }

    }

    class Decryption
    {

        public static byte[] Decrypt(byte[] file, string password, double key)
        {
            file = AntiBruteForce(file, key);
            file = DecryptMain(file, password);
            file = Split(file, password);
            return file;
        }

        //The following code is taken from a previous project, also made by Eilia Keyhanee

        private static byte[] AntiBruteForce(byte[] file, double key)
        {

            //Splits the Anti-Brute Force Key into two parts
            char[] passarray = key.ToString().ToCharArray();
            int remainder = passarray.Length % 2;

            char[] keychar1 = new char[(passarray.Length + remainder) / 2];
            char[] keychar2 = new char[(passarray.Length - remainder) / 2];

            for (int i = 0; i < keychar2.Length; i++)
            {
                keychar1[i] = passarray[i];
                keychar2[i] = passarray[keychar1.Length + i];
            }

            for (int i = 0; i < remainder; i++)
                keychar1[keychar2.Length + i] = passarray[keychar2.Length + i];

            string keystring1 = new string(keychar1);
            string keystring2 = new string(keychar2);

            //If the second part is empty, turns it to 0 to prevent a crash.
            if (keystring2 == "")
                keystring2 = "0";

            //Gets the length of each part and removes it from either end of the file, leaving the original, encrypted file without the fake bytes.
            int key1 = int.Parse(keystring1);
            int key2 = int.Parse(keystring2);

            byte[] bfile = new byte[file.Length - (key1 + key2)];

            for (int i = 0; i < bfile.Length; i++)
                bfile[i] = file[key1 + i];

            return bfile;
        }// Undo Stage 3: Take Off Fake Anti-Brute Force Bytes

        private static byte[] DecryptMain(byte[] file, string password)
        {

            int keypos = 0;
            int pastkeys = 0;

            //truns the password's characters to their integer values corresponding to the character (See PassConverter)
            byte[] keyarray = PassConverter(password);

            //Does the reverse version of encryption, see the Encryption explanation.
            for (int i = 0; i < file.Length; i++)
            {

                if (i != 0)
                    pastkeys += i;

                file[i] -= (byte)((keyarray[keypos] * (i + 1)) - pastkeys);
                keypos++;

                if (keypos == keyarray.Length)
                    keypos = 0;

            }

            return file;
        }// Undo Stage 2: Decrypt Whole File

        private static byte[] Split(byte[] file, string password)
        {

            //Splits the file into four parts
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
                file4[file1.Length + i] = file[4 * (file1.Length) + i];

            //Decrypts each part
            byte[] efile1 = DecryptMain(file1, password);
            byte[] efile2 = DecryptMain(file2, password);
            byte[] efile3 = DecryptMain(file3, password);
            byte[] efile4 = DecryptMain(file4, password);
            byte[] efile = new byte[file.Length];

            //Puts them back into one part
            for (int i = 0; i < file1.Length; i++)
            {
                efile[i] = efile1[i];
                efile[file1.Length + i] = efile2[i];
                efile[2 * (file1.Length) + i] = efile3[i];
                efile[3 * (file1.Length) + i] = efile4[i];
            }

            for (int i = 0; i < remainder; i++)
                efile[4 * (file1.Length) + i] = efile4[file1.Length + i];


            return efile;

        }// Undo Stage 1: Split Files, Decrypt, and Put Back together

        public static byte[] PassConverter(string password)
        {

            //Turns the password into a character array
            Char[] passarray = password.ToCharArray();
            byte[] keyarray = new byte[passarray.Length];

            //Converts each character into their corresponding integer value.
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

    }

}
