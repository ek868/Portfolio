using System;
using System.Collections.Generic;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public string trial = "";
        
        public Window1()
        {
            InitializeComponent();
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow mw = new MainWindow();
            //mw.Show();
            //mw.TxtInput.Text += Btn1.Content.ToString();
            //Extras.ExtraGlobal += Btn1.Content.ToString();
            trial += ( (Button)sender ).Content.ToString();
            this.Close();
        }
    }
}
