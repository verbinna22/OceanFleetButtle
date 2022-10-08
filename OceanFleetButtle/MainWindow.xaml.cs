using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OceanFleetButtle
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            for (int i = 0; i <= 10; i++)
            {
                var line = new Line();
                line.X1 = 25;
                line.Y1 = 25 + i * 25;
                line.X2 = 275;
                line.Y2 = 25 + i * 25;
                line.Stroke = new SolidColorBrush(Colors.Red);
                canvy.Children.Add(line);

                var line2 = new Line();
                line2.X1 = 25 + i * 25;
                line2.Y1 = 25;
                line2.X2 = 25 + i * 25;
                line2.Y2 = 275;
                line2.Stroke = new SolidColorBrush(Colors.Red);
                canvy.Children.Add(line2);

                var line3 = new Line();
                line3.X1 = 300;
                line3.Y1 = 25 + i * 25;
                line3.X2 = 550;
                line3.Y2 = 25 + i * 25;
                line3.Stroke = new SolidColorBrush(Colors.Red);
                canvy.Children.Add(line3);

                var line4 = new Line();
                line4.X1 = 300 + i * 25;
                line4.Y1 = 25;
                line4.X2 = 300 + i * 25;
                line4.Y2 = 275;
                line4.Stroke = new SolidColorBrush(Colors.Red);
                canvy.Children.Add(line4);

            }
            
        }
    }
}
