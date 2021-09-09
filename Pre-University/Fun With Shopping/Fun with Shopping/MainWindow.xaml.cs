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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fun_with_Shopping
{
    public partial class MainWindow : Window
    {
        // setting the items and variables up for later use
        Items apple;
        Items banana;
        Items carrot;
        Items dingle;
        Items elephant;
        Items frangapanie;
        Items goldfish;
        Items horse;
        Items insignia;
        Items jackarse;

        double TotalCost;
        int Totalcount;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //setting sell button to disabled so that the user can't crash the program by clicking remove at the beggining of the program
            btnSELL.IsEnabled = false;

            switch (Window1.GlobalVAR.Language)
            {
                // checking what language was selected and changing all text to said language
                //{
                case "ch":
                    txtHeading.Text = "我的购物清单";
                    txtAvailable.Text = "可用项目";
                    txtCurrent.Text = "当前项目";
                    txtCategories.Text = "类别";

                    ((GridView)lsvTO.View).Columns[0].Header = "名称";
                    ((GridView)lsvTO.View).Columns[1].Header = "计数";
                    ((GridView)lsvTO.View).Columns[2].Header = "成本";

                    btnApple.Content = "苹果";
                    btnBanana.Content = "香蕉";
                    btnCarrot.Content = "胡萝卜";
                    btnDingle.Content = "丁格尔";
                    btnElephant.Content = "象";
                    btnFrangapanie.Content = "鸡蛋花";
                    btnGoldfish.Content = "金鱼";
                    btnHorse.Content = "马";
                    btnInsignia.Content = "徽章";
                    btnJackarse.Content = "杰克斯";
                    btnSELL.Content = "去掉";

                    rdbAll.Content = "所有";
                    rdbFruit.Content = "水果";
                    rdbGroceries.Content = "杂货";
                    rdbMeat.Content = "肉";
                    rdbMisc.Content = "杂项";

                    txtTotalCOST.Text = "总成本";
                    txtTotalITEMS.Text = "总项目";

                    txtTotalCOST.Width -= 20;

                    apple = new Items { name = "Apple", cost = 0.50, count = 0, categories = "Fruit", cost2 = 0 };
                    banana = new Items { name = "Banana", cost = 0.65, count = 0, categories = "Fruit", cost2 = 0 };
                    carrot = new Items { name = "Carrot", cost = 0.30, count = 0, categories = "Fruit", cost2 = 0 };
                    dingle = new Items { name = "Dingle", cost = 1.86, count = 0, categories = "Meat", cost2 = 0 };
                    elephant = new Items { name = "Elephant", cost = 2.50, count = 0, categories = "Meat", cost2 = 0 };
                    frangapanie = new Items { name = "Frangapanie", cost = 1.00, count = 0, categories = "Groceries", cost2 = 0 };
                    goldfish = new Items { name = "Goldfish", cost = 23.00, count = 0, categories = "Groceries", cost2 = 0 };
                    horse = new Items { name = "Horse", cost = 9.12, count = 0, categories = "Misc", cost2 = 0 };
                    insignia = new Items { name = "Insignia", cost = 1.80, count = 0, categories = "Misc", cost2 = 0 };
                    jackarse = new Items { name = "Jackarse", cost = 2.00, count = 0, categories = "Misc", cost2 = 0 };

                    break;
                case "ar":
                    txtHeading.Text = "قائمة التسوق";
                    txtAvailable.Text = "العناصر المتوفرة";
                    txtCurrent.Text = "العناصر الحالية";
                    txtCategories.Text = "الاقسام";

                    ((GridView)lsvTO.View).Columns[0].Header = "الاسم";
                    ((GridView)lsvTO.View).Columns[1].Header = "العد";
                    ((GridView)lsvTO.View).Columns[2].Header = "التكلفة";

                    btnApple.Content = "تفاحة";
                    btnBanana.Content = "موز";
                    btnCarrot.Content = "جزرة";
                    btnDingle.Content = "دينجل";
                    btnElephant.Content = "الفيل";
                    btnFrangapanie.Content = "فرانغاباني";
                    btnGoldfish.Content = "الأسماك الذهبية";
                    btnHorse.Content = "حصان";
                    btnInsignia.Content = "شارة";
                    btnJackarse.Content = "جاكاس";
                    btnSELL.Content = "إزالة";

                    rdbAll.Content = "الكل";
                    rdbFruit.Content = "فاكهة";
                    rdbGroceries.Content = "البقالة";
                    rdbMeat.Content = "لحم";
                    rdbMisc.Content = "متفرقات";

                    txtTotalITEMS.Text = "العناصر:";
                    txtTotalCOST.Text = "التكاليف:";

                    txtTotalCOST.Width -= 20;

                    apple = new Items { name = "Apple", cost = 0.50, count = 0, categories = "Fruit", cost2 = 0 };
                    banana = new Items { name = "Banana", cost = 0.65, count = 0, categories = "Fruit", cost2 = 0 };
                    carrot = new Items { name = "Carrot", cost = 0.30, count = 0, categories = "Fruit", cost2 = 0 };
                    dingle = new Items { name = "Dingle", cost = 1.86, count = 0, categories = "Meat", cost2 = 0 };
                    elephant = new Items { name = "Elephant", cost = 2.50, count = 0, categories = "Meat", cost2 = 0 };
                    frangapanie = new Items { name = "Frangapanie", cost = 1.00, count = 0, categories = "Groceries", cost2 = 0 };
                    goldfish = new Items { name = "Goldfish", cost = 23.00, count = 0, categories = "Groceries", cost2 = 0 };
                    horse = new Items { name = "Horse", cost = 9.12, count = 0, categories = "Misc", cost2 = 0 };
                    insignia = new Items { name = "Insignia", cost = 1.80, count = 0, categories = "Misc", cost2 = 0 };
                    jackarse = new Items { name = "Jackarse", cost = 2.00, count = 0, categories = "Misc", cost2 = 0 };


                    break;
                case "en":

                    apple = new Items { name = "Apple", cost = 0.50, count = 0, categories = "Fruit", cost2=0 };
                    banana = new Items { name = "Banana", cost = 0.65, count = 0, categories = "Fruit", cost2=0 };
                    carrot = new Items { name = "Carrot", cost = 0.30, count = 0, categories = "Fruit", cost2=0 };
                    dingle = new Items { name = "Dingle", cost = 1.86, count = 0, categories = "Meat", cost2 = 0 };       
                    elephant = new Items { name = "Elephant", cost = 2.50, count = 0, categories = "Meat", cost2 = 0 };
                    frangapanie = new Items { name = "Frangapanie", cost = 1.00, count = 0, categories = "Groceries", cost2 = 0 };
                    goldfish = new Items { name = "Goldfish", cost = 23.00, count = 0, categories = "Groceries", cost2 = 0 };
                    horse = new Items { name = "Horse", cost = 9.12, count = 0, categories = "Misc", cost2 = 0 };
                    insignia = new Items { name = "Insignia", cost = 1.80, count = 0, categories = "Misc", cost2 = 0 };
                    jackarse = new Items { name = "Jackarse", cost = 2.00, count = 0, categories = "Misc", cost2 = 0 };

                    break;
                    //}
            }
        }


        

        private void btnSELL_Click(object sender, RoutedEventArgs e)
        {
            //checking if this is the first time the user is adding the item
           if ((((Items)lsvTO.SelectedItem).count) == 1)
            {
                ((Items)lsvTO.SelectedItem).count--;
                TotalCost = TotalCost - ((Items)lsvTO.SelectedItem).cost;
                lsvTO.Items.RemoveAt(lsvTO.SelectedIndex);
                Totalcount--;
                txtCountITEMS.Text = Totalcount.ToString();
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                btnSELL.IsEnabled = false;
                
            }
            else
            // if it isnt, then you do the same thing but without adding a seperate item
            {
                ((Items)lsvTO.SelectedItem).cost2 -= ((Items)lsvTO.SelectedItem).cost;
                TotalCost = TotalCost - ((Items)lsvTO.SelectedItem).cost;
                ((Items)lsvTO.SelectedItem).count--;
                Totalcount--;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
            }
           
        }

        private void btnApple_Click(object sender, RoutedEventArgs e)
        {
            if(apple.count == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    //checking language again
                    case "en":
                        lsvTO.Items.Add(apple = new Items() { name = "Apple", cost = 0.50, categories = "fruitandveg", count = 0, cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(apple = new Items() { name = "تفاحة", cost = 0.50, categories = "fruitandveg", count = 0, cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(apple = new Items() { name = "苹果", cost = 0.50, categories = "fruitandveg", count = 0, cost2 = 0 });
                        break;
                }
                //reverse of sell button
                apple.cost2 += apple.cost;
                apple.count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (apple).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();

                btnSELL.IsEnabled = true;
            }
            else
            {
                //reverse of sell button
                apple.cost2 += apple.cost;
                (apple).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (apple).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                btnSELL.IsEnabled = true;


            }
            btnSELL.IsEnabled = false;
        }

        private void btnBanana_Click(object sender, RoutedEventArgs e)
        {
            //same as apple
            if (banana.count == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(banana = new Items { name = "Banana", cost = 0.65, count = 0, categories = "Fruit", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(banana = new Items { name = "موز", cost = 0.65, count = 0, categories = "Fruit", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(banana = new Items { name = "香蕉", cost = 0.65, count = 0, categories = "Fruit", cost2 = 0 });
                        break;
                }
                banana.cost2 += banana.cost;
                banana.count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (banana).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();
                btnSELL.IsEnabled = true;

            }
            else
            {
                banana.cost2 += banana.cost;
                (banana).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (banana).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                btnSELL.IsEnabled = true;


            }
            btnSELL.IsEnabled = false;
        }

        private void btnDingle_Click(object sender, RoutedEventArgs e)
        {
            //same as apple

            if (((dingle).count) == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(dingle = new Items { name = "Dingle", cost = 1.86, count = 0, categories = "Meat", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(dingle = new Items { name = "دينجل", cost = 1.86, count = 0, categories = "Meat", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(dingle = new Items { name = "丁格尔", cost = 1.86, count = 0, categories = "Meat", cost2 = 0 });
                        break;
                }
                dingle.cost2 += dingle.cost;
                (dingle).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (dingle).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();
                btnSELL.IsEnabled = true;

            }
            else
            {
                dingle.cost2 += dingle.cost;
                (dingle).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (dingle).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                btnSELL.IsEnabled = true;


            }
            btnSELL.IsEnabled = false;
        }

        private void btnCarrot_Click(object sender, RoutedEventArgs e)
        {
            //same as apple

            if (((carrot).count) == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(carrot = new Items { name = "Carrot", cost = 0.30, count = 0, categories = "Fruit", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(carrot = new Items { name = "جزرة", cost = 0.30, count = 0, categories = "Fruit", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(carrot = new Items { name = "胡萝卜", cost = 0.30, count = 0, categories = "Fruit", cost2 = 0 });
                        break;
                }
                carrot.cost2 += carrot.cost;
                (carrot).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (carrot).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh(); btnSELL.IsEnabled = true;

            }
            else
            {
                carrot.cost2 += carrot.cost;
                (carrot).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (carrot).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                btnSELL.IsEnabled = true;


            }
            btnSELL.IsEnabled = false;
        }

        private void btnElephant_Click(object sender, RoutedEventArgs e)
        {
            //same as apple

            if (((elephant).count) == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(elephant = new Items { name = "Elephant", cost = 2.50, count = 0, categories = "Meat", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(elephant = new Items { name = "الفيل", cost = 2.50, count = 0, categories = "Meat", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(elephant = new Items { name = "象", cost = 2.50, count = 0, categories = "Meat", cost2 = 0 });
                        break;
                }
                elephant.cost2 += elephant.cost;
                (elephant).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (elephant).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();
            }
            else
            {
                elephant.cost2 += elephant.cost;
                (elephant).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (elephant).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);

            }
            btnSELL.IsEnabled = false;

        }

        private void btnFrangapanie_Click(object sender, RoutedEventArgs e)
        {
            //same as apple

            if (((frangapanie).count) == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(frangapanie = new Items { name = "Frangapanie", cost = 1.00, count = 0, categories = "Groceries", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(frangapanie = new Items { name = "فرانغاباني", cost = 1.00, count = 0, categories = "Groceries", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(frangapanie = new Items { name = "鸡蛋花", cost = 1.00, count = 0, categories = "Groceries", cost2 = 0 });
                        break;
                }
                frangapanie.cost2 += frangapanie.cost;
                (frangapanie).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (frangapanie).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();
            }
            else
            {
                frangapanie.cost2 += frangapanie.cost;
                (frangapanie).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (frangapanie).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);

            }
            btnSELL.IsEnabled = false;

        }

        private void btnGoldfish_Click(object sender, RoutedEventArgs e)
        {
            //same as apple

            if (((goldfish).count) == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(goldfish = new Items { name = "Goldfish", cost = 23.00, count = 0, categories = "Groceries", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(goldfish = new Items { name = "الأسماك الذهبية", cost = 23.00, count = 0, categories = "Groceries", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(goldfish = new Items { name = "金鱼", cost = 23.00, count = 0, categories = "Groceries", cost2 = 0 });
                        break;
                }
                goldfish.cost2 += goldfish.cost;
                (goldfish).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (goldfish).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();
            }
            else
            {
                goldfish.cost2 += goldfish.cost;
                (goldfish).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (goldfish).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);

            }
            btnSELL.IsEnabled = false;

        }

        private void btnHorse_Click(object sender, RoutedEventArgs e)
        {
            //same as apple

            if (((horse).count) == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(horse = new Items { name = "Horse", cost = 9.12, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(horse = new Items { name = "حصان", cost = 9.12, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(horse = new Items { name = "马", cost = 9.12, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                }
                horse.cost2 += horse.cost;
                (horse).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (horse).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();
            }
            else
            {
                horse.cost2 += horse.cost;
                (horse).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (horse).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);

            }
            btnSELL.IsEnabled = false;

        }

        private void btnInsignia_Click(object sender, RoutedEventArgs e)
        {
            //same as apple

            if (((insignia).count) == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(insignia = new Items { name = "Insignia", cost = 1.80, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(insignia = new Items { name = "شارة", cost = 1.80, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(insignia = new Items { name = "徽章", cost = 1.80, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                }
                insignia.cost2 += insignia.cost;
                (insignia).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (insignia).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();
            }
            else
            {
                insignia.cost2 += insignia.cost;
                (insignia).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (insignia).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);

            }
            btnSELL.IsEnabled = false;

        }

        private void btnJackarse_Click(object sender, RoutedEventArgs e)
        {
            //same as apple

            if (((jackarse).count) == 0)
            {
                switch (Window1.GlobalVAR.Language)
                {
                    case "en":
                        lsvTO.Items.Add(jackarse = new Items { name = "Jackarse", cost = 2.00, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                    case "ar":
                        lsvTO.Items.Add(jackarse = new Items { name = "جاكاس", cost = 2.00, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                    case "ch":
                        lsvTO.Items.Add(jackarse = new Items { name = "杰克斯", cost = 2.00, count = 0, categories = "Misc", cost2 = 0 });
                        break;
                }
                jackarse.cost2 += jackarse.cost;
                (jackarse).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                TotalCost = TotalCost + (jackarse).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);
                lsvTO.Items.Refresh();
            }
            else
            {
                jackarse.cost2 += jackarse.cost;
                (jackarse).count++;
                Totalcount++;
                txtCountITEMS.Text = Totalcount.ToString();
                lsvTO.Items.Refresh();
                TotalCost = TotalCost + (jackarse).cost;
                txtCountCOST.Text = "$" + TotalCost.ToString();
                txtCountCOST.Text = String.Format("{0:C2}", TotalCost);

            }
            btnSELL.IsEnabled = false;

        }

        private void rdbAll_Click(object sender, RoutedEventArgs e)
        {
            //because it's all, set all the buttons to enabled
            btnApple.IsEnabled = true;
            btnBanana.IsEnabled = true;
            btnCarrot.IsEnabled = true;
            btnDingle.IsEnabled = true;
            btnElephant.IsEnabled = true;
            btnFrangapanie.IsEnabled = true;
            btnGoldfish.IsEnabled = true;
            btnHorse.IsEnabled = true;
            btnInsignia.IsEnabled = true;
            btnJackarse.IsEnabled = true;
        }

        private void rdbFruit_Click(object sender, RoutedEventArgs e)
        {
            //reset the other ones to enable so it undoes the effect if the last category selection
            btnApple.IsEnabled = true;
            btnBanana.IsEnabled = true;
            btnCarrot.IsEnabled = true;
            btnDingle.IsEnabled = false;
            btnElephant.IsEnabled = false;
            btnFrangapanie.IsEnabled = false;
            btnGoldfish.IsEnabled = false;
            btnHorse.IsEnabled = false;
            btnInsignia.IsEnabled = false;
            btnJackarse.IsEnabled = false;
        }

        private void rdbMeat_Click(object sender, RoutedEventArgs e)
        {
            //same as fruit
            btnApple.IsEnabled = false;
            btnBanana.IsEnabled = false;
            btnCarrot.IsEnabled = false;
            btnDingle.IsEnabled = true;
            btnElephant.IsEnabled = true;
            btnFrangapanie.IsEnabled = false;
            btnGoldfish.IsEnabled = false;
            btnHorse.IsEnabled = false;
            btnInsignia.IsEnabled = false;
            btnJackarse.IsEnabled = false;
        }

        private void rdbGroceries_Click(object sender, RoutedEventArgs e)
        {
            //same as fruit

            btnApple.IsEnabled = false;
            btnBanana.IsEnabled = false;
            btnCarrot.IsEnabled = false;
            btnDingle.IsEnabled = false;
            btnElephant.IsEnabled = false;
            btnFrangapanie.IsEnabled = true;
            btnGoldfish.IsEnabled = true;
            btnHorse.IsEnabled = false;
            btnInsignia.IsEnabled = false;
            btnJackarse.IsEnabled = false;
        }

        private void rdbMisc_Click(object sender, RoutedEventArgs e)
        {
            //same as fruit

            btnApple.IsEnabled = false;
            btnBanana.IsEnabled = false;
            btnCarrot.IsEnabled = false;
            btnDingle.IsEnabled = false;
            btnElephant.IsEnabled = false;
            btnFrangapanie.IsEnabled = false;
            btnGoldfish.IsEnabled = false;
            btnHorse.IsEnabled = true;
            btnInsignia.IsEnabled = true;
            btnJackarse.IsEnabled = true;
        }

        private void lsvTO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //so that they can remove an item after selecting it
            btnSELL.IsEnabled = true;
        }
    }
    class Items
    {
        //cost1 is the actual original cost of the items eg apple's 0.30
        // cost 2 is the cost of all the apples currently in the list eg 5 apples 1.50
        public string name { set; get; }
        public double cost { set; get; }
        public string categories;
        public int count { set; get; }
        public double cost2 { set; get; }


    }
}
