using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PublicFunctions
{

    public class Responsivity
    {

        //------------------------------------------------------------------TextBox--------------------------------------------------------------------------------------------//

        public static void HighlightTextBox(TextBox element)
        {

            //Changes the background of the textbox that the mouse is over for UI feedback.
            BrushConverter bc = new BrushConverter();
            element.Background = (Brush)bc.ConvertFrom("#FFACE5FF");

        }

        public static void UnhighlightTextBox(TextBox element)
        {

            //Changes the background back to normal after mouse leaves.
            BrushConverter bc = new BrushConverter();
            element.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");

        }

        public static void SelectTextBox(TextBox element, string OriginalText)
        {

            //Changes textbox contents from the display content to the content the user enters.
            element.TextAlignment = TextAlignment.Left;
            element.FontWeight = FontWeights.Regular;
            if (element.Text == OriginalText)
            {
                element.Text = "";
            }

        }

        public static void DeselectTextBox(TextBox element, string OriginalText, UIElement FocusOn)
        {

            //Changes the contents of a textbox back to the display contents if the user does not add anything.
            if (element.Text == "")
            {
                element.TextAlignment = TextAlignment.Center;
                element.Text = OriginalText;
                element.FontWeight = FontWeights.UltraLight;
            }

            FocusOn.Focus();

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public static bool IsDigitInput(string input, string otheracceptedinput = null)
        {

            //Checks the input to ensure that it is either a digit, a '/' or a '.', to bloch any invalid inputs.
            if (otheracceptedinput == ".")
                otheracceptedinput = "OemPeriod";

            if (otheracceptedinput == "/")
                otheracceptedinput = "OemQuestion";

            if (int.TryParse(input.ToCharArray()[input.Length - 1].ToString(), out int i) || input == otheracceptedinput)
                return false;

            else
                return true;

        }

    }

    public class Functionality
    {

        //-----------------------------------------------------------------Edit---------------------------------------------------------------------------------------------//

        public static void EditClicked(TextBox EditText, TextBlock Text)
        {

            //Switches the textboxes so that the information is editable.
            EditText.Visibility = Visibility.Visible;
            EditText.IsEnabled = true;
            Text.Visibility = Visibility.Hidden;

        }

        public static void SaveClicked(TextBox EditText, TextBlock Text)
        {

            //Switches the textboxes so that the information is only visible.
            Text.Text = EditText.Text;
            EditText.Visibility = Visibility.Hidden;
            EditText.IsEnabled = false;
            Text.Visibility = Visibility.Visible;

        }

        public static void EnableButton(Button button)
        {

            //Enables a button.
            button.IsHitTestVisible = true;
            BrushConverter bc = new BrushConverter();
            button.Background = (Brush)bc.ConvertFrom("#FF0789C4");
            button.Foreground = (Brush)bc.ConvertFrom("#FF060E12");
            button.BorderBrush = (Brush)bc.ConvertFrom("#FF060E12");

        }

        public static void DisableButton(Button button)
        {

            //Disables a button.
            button.IsHitTestVisible = false;
            BrushConverter bc = new BrushConverter();
            button.Background = (Brush)bc.ConvertFrom("#7F0789C4");
            button.Foreground = (Brush)bc.ConvertFrom("#B2060E12");
            button.BorderBrush = (Brush)bc.ConvertFrom("#B2060E12");

        }

        public static void DropDownEditClicked(ComboBox ComboBox, TextBlock Text)
        {

            //Switches a textblock so that the contents can be editted through a drop down box.
            Text.Visibility = Visibility.Hidden;
            ComboBox.IsEnabled = true;
            ComboBox.Visibility = Visibility.Visible;

        }

        public static void DropDownSaveClicked(ComboBox ComboBox, TextBlock Text)
        {

            //Switches an editable drop down box with a visible text block.
            Text.Text = ComboBox.Text;
            ComboBox.Visibility = Visibility.Hidden;
            ComboBox.IsEnabled = false;
            Text.Visibility = Visibility.Visible;

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------//



        //-----------------------------------------------------------------Misc-------------------------------------------------------------------------------------//

        public static void ClearTempFiles()
        {

            //attempts to delete all temporary folders.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            try
            {

                Directory.Delete(Directory.GetCurrentDirectory() + @"\tempvisitimages", true);

            }

            catch { }

            try
            {

                Directory.Delete(Directory.GetCurrentDirectory() + @"\tempclientimages", true);

            }

            catch { }

            try
            {

                Directory.Delete(Directory.GetCurrentDirectory() + @"\tempnotefiles", true);

            }

            catch { }

        }

        public static int LetterToNumber(string input)
        {

            //Converts letters into reprasentative numbers.
            input = input.ToLower();
            char letter = input.ToCharArray()[0];

            if (letter == 'a')
                return 1;

            else if (letter == 'b')
                return 2;

            else if (letter == 'c')
                return 3;

            else if (letter == 'd')
                return 4;

            else if (letter == 'e')
                return 5;

            else if (letter == 'f')
                return 6;

            else if (letter == 'g')
                return 7;

            else if (letter == 'h')
                return 8;

            else if (letter == 'i')
                return 9;

            else if (letter == 'j')
                return 10;

            else if (letter == 'k')
                return 11;

            else if (letter == 'l')
                return 12;

            else if (letter == 'm')
                return 13;

            else if (letter == 'n')
                return 14;

            else if (letter == 'o')
                return 15;

            else if (letter == 'p')
                return 16;

            else if (letter == 'q')
                return 17;

            else if (letter == 'r')
                return 18;

            else if (letter == 's')
                return 19;

            else if (letter == 't')
                return 20;

            else if (letter == 'u')
                return 21;

            else if (letter == 'v')
                return 22;

            else if (letter == 'w')
                return 23;

            else if (letter == 'x')
                return 24;

            else if (letter == 'y')
                return 25;

            else if (letter == 'z')
                return 26;

            else
                return 0;

        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------//

    }

}
