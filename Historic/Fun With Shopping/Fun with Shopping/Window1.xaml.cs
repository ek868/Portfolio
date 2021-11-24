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

namespace Fun_with_Shopping
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        public static class GlobalVAR
        {
            public static string Language;
        }

        private void btnEnglish_Click(object sender, RoutedEventArgs e)
        {
            GlobalVAR.Language = "en";
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void btnChinese_Click(object sender, RoutedEventArgs e)
        {
            GlobalVAR.Language = "ch";
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void btnArabic_Click(object sender, RoutedEventArgs e)
        {
            GlobalVAR.Language = "ar";
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();

        }
    }
}
