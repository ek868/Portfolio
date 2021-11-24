using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NargesLogs
{

    public partial class Settings : Window
    {

        public Settings()
        {

            InitializeComponent();

        }

        private void txtIP_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.HighlightTextBox(txtIP);

        }

        private void txtIP_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.UnhighlightTextBox(txtIP);

        }

        private void txtPort_MouseEnter(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.HighlightTextBox(txtPort);

        }

        private void txtPort_MouseLeave(object sender, MouseEventArgs e)
        {

            //See function description.
            PublicFunctions.Responsivity.UnhighlightTextBox(txtPort);

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            //Simply closes the window without making any changes.
            Close();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            //Sets the global variables used to connect the TCP client to the text in the appropriate fields.
            if(btnSave.IsHitTestVisible == true)
            {

                if (txtIP.Text.ToCharArray().Count() != 0)
                    ConnectionInfo.IPAddress = txtIP.Text;

                if (txtPort.Text.ToCharArray().Count() != 0)
                    ConnectionInfo.Port = int.Parse(txtPort.Text);

                Close();

            }

        }

        private void txtIP_KeyDown(object sender, KeyEventArgs e)
        {

            //Ensures that save can only be clicked if information exists within the fields.
            PublicFunctions.Functionality.EnableButton(btnSave);

        }

        private void txtPort_KeyDown(object sender, KeyEventArgs e)
        {

            //Ensures that only digits may be entered into the port field to avoid errors.
            e.Handled = PublicFunctions.Responsivity.IsDigitInput(e.Key.ToString());
            PublicFunctions.Functionality.EnableButton(btnSave);

        }

        private void txtIP_KeyUp(object sender, KeyEventArgs e)
        {

            //Ensures that the info cant be saved if no information is in the fields.
            if (txtIP.Text == "" && txtPort.Text == "")
                PublicFunctions.Functionality.DisableButton(btnSave);

        }

        private void txtPort_KeyUp(object sender, KeyEventArgs e)
        {

            //As above.
            if (txtIP.Text == "" && txtPort.Text == "")
                PublicFunctions.Functionality.DisableButton(btnSave);
            
        }

    }

}
