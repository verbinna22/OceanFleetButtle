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
            var gridMy = new Grid();
            gridMy.Background = Brushes.BlueViolet;
            for (int i = 0; i < 10; i++)
            {
                var colDef1 = new ColumnDefinition();
                gridMy.ColumnDefinitions.Add(colDef1);
            }
            for (int i = 0; i < 10; i++)
            {
                var colDef1 = new RowDefinition();
                gridMy.RowDefinitions.Add(colDef1);
            }
            gridMy.Width = 500;
            gridMy.Height = 500;
            
            gridMy.ShowGridLines = true;

            InitializeComponent();
            canvy.Children.Add(gridMy);
        }
    }
}
