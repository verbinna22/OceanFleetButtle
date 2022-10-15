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
                MyAdding(i);
                ComputerAdding(i);
            }
            
            StartPlaying();

        }

        List<Rectangle> shipList = new List<Rectangle>();
        bool dragging = false;
        int[,] shipArray = new int[10, 10];
        int[,] shipComputerArray = new int[10, 10];
        int allShips = 0;
        bool playing = false;
        int allFields = 35;
        public void ComputerAdding(int i)
        {
            ComputerAddingHorisontal(i);
            ComputerAddingVertical(i);
        }

        public void ComputerAddingHorisontal(int i)
        {
            var line = new Line();
            line.X1 = 25;
            line.Y1 = 25 + i * 25;
            line.X2 = 275;
            line.Y2 = 25 + i * 25;
            line.Stroke = new SolidColorBrush(Colors.Red);
            canvy.Children.Add(line);

        }

        public void ComputerAddingVertical(int i)
        {
            var line2 = new Line();
            line2.X1 = 25 + i * 25;
            line2.Y1 = 25;
            line2.X2 = 25 + i * 25;
            line2.Y2 = 275;
            line2.Stroke = new SolidColorBrush(Colors.Red);
            canvy.Children.Add(line2);
        }

        public void MyAdding(int i)
        {
            MyAddingHorisontal(i);
            MyAddingVertical(i);
        }

        public void MyAddingHorisontal(int i)
        {
            var line3 = new Line();
            line3.X1 = 300;
            line3.Y1 = 25 + i * 25;
            line3.X2 = 550;
            line3.Y2 = 25 + i * 25;
            line3.Stroke = new SolidColorBrush(Colors.Red);
            canvy.Children.Add(line3);
        }

        public void MyAddingVertical(int i)
        {
            var line4 = new Line();
            line4.X1 = 300 + i * 25;
            line4.Y1 = 25;
            line4.X2 = 300 + i * 25;
            line4.Y2 = 275;
            line4.Stroke = new SolidColorBrush(Colors.Red);
            canvy.Children.Add(line4);
        }
        public void StartPlaying()
        {
            for (int i = 1; i <= 5; i++)
            {
                for (int j = i; j <= 5; j++)
                {
                    CreatingRectangle(i);
                }
            }
        }
        public void Playing(double x, double y)
        {
            var IntCoords = IntCoordinates2(x, y);
            var IntX = IntCoords.Item1;
            var IntY = IntCoords.Item2;
            if ((IntX >= 0) && (IntY >= 0) && (IntY < 10) && (IntX < 10))
            {
                if (shipComputerArray[IntX, IntY] == 1)
                {
                    shipComputerArray[IntX, IntY] = 0;
                    allFields--;
                    MessageBox.Show("Ранил!");
                    if (allFields == 0)
                    {
                        FinishPlaying();
                    }
                }
                else
                {
                    MessageBox.Show("Мимо!");
                    ComputerStickBack();
                }
            }
        }

        public void ComputerStickBack()
        {

        }

        public void FinishPlaying()
        {

        }

        public void CreatingRectangle(int width)
        {
            var ship = new Rectangle();
            ship.Width = width * 25;
            ship.Height = 25;
            ship.Fill = new SolidColorBrush(Colors.Red);
            canvy.Children.Add(ship);
            Canvas.SetTop(ship, 325);
            Canvas.SetLeft(ship, 25);
            ship.AddHandler(MouseDownEvent, new RoutedEventHandler(EvMouseDown));
            ship.AddHandler(MouseUpEvent, new RoutedEventHandler(EvMouseUp));
            ship.AddHandler(MouseMoveEvent, new RoutedEventHandler(EvMouseMove));
            shipList.Add(ship);

        }

        public void EvMouseDown(object sender, RoutedEventArgs e)
        {
            var curShip = (Rectangle)e.Source;
            if (curShip.Fill.ToString() == "#FFFF0000")
            { 
                dragging = true;
                Mouse.Capture(curShip);
            }
            if (playing)
            {
                var position = Mouse.GetPosition(this);
                Playing(position.X, position.Y);
            }
        }

        public void EvMouseUp(object sender, RoutedEventArgs e)
        {
            var curShip = (Rectangle)e.Source;
            dragging = false;
            Mouse.Capture(null);
            CorrectingPosition(curShip);
        }

        public void EvMouseMove(object sender, RoutedEventArgs e)
        {
            if (dragging)
            {
                var curShip = (Rectangle)e.Source;
                var position = Mouse.GetPosition(this);
                Canvas.SetLeft(curShip, position.X);
                Canvas.SetTop(curShip, position.Y);
            }
        }

        public Tuple<int, int> IntCoordinates(double x, double y)
        {
            int intX = Convert.ToInt32(x) / 25 - 12;
            int intY = Convert.ToInt32(y) / 25 - 1;
            return Tuple.Create(intX, intY);
        }

        public Tuple<int, int> IntCoordinates2(double x, double y)
        {
            int intX = Convert.ToInt32(x) / 25 - 1;
            int intY = Convert.ToInt32(y) / 25 - 1;
            return Tuple.Create(intX, intY);
        }
        public void FillShipArray(Rectangle ship, int x, int y)
        {
            var width = Convert.ToInt16(ship.Width) / 25;
            var height = Convert.ToInt16(ship.Height) / 25;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int kx = -1; kx<2; kx++)
                    {
                        for (int ky = -1; ky < 2; ky++)
                        {
                            try
                            {
                                if (shipArray[i + x + kx, j + y + ky] == 1)
                                    throw new DivideByZeroException();
                            }
                            catch(IndexOutOfRangeException)
                            {
                                
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    shipArray[i + x, j + y] = 1;
                    
                }
            }
        }
        public void CorrectingPosition(Rectangle uncorrect)
        {
            double x = Canvas.GetLeft(uncorrect);
            double y = Canvas.GetTop(uncorrect);
            var intCoords = IntCoordinates(x, y);
            var intX = intCoords.Item1;
            var intY = intCoords.Item2;
            allShips++;
            if ((intX < 0) || (intY < 0) ||
                (intX > 10 - Convert.ToInt64(uncorrect.Width)/25) || (intY > 10 - uncorrect.Height/ 25))
            {
                Canvas.SetTop(uncorrect, 325);
                Canvas.SetLeft(uncorrect, 25);
                allShips--;
                
            }
            else
            {
                Canvas.SetLeft(uncorrect, intX * 25 + 300);
                Canvas.SetTop(uncorrect, intY * 25 + 25);
                try
                {
                    FillShipArray(uncorrect, intX, intY);
                    uncorrect.Fill = new SolidColorBrush(Colors.Green);
                    
                }
                catch (DivideByZeroException)
                {
                    Canvas.SetTop(uncorrect, 325);
                    Canvas.SetLeft(uncorrect, 25);
                    allShips--;
                }
            }
            if (allShips == 15)
            {
                playing = true;
                GenerateComputerField();
            }
        }

        private void WinKeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.Key.ToString());
            if (e.Key == Key.E) MessageBox.Show("a");//this.Close();
        }
        public void GenerateComputerField()
        {
            var rand = new Random();
            var composition = rand.Next(4);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (composition == 0)
                    {
                        shipComputerArray[i,j] = shipArray[i,j];
                    }
                    else if (composition == 1)
                    {
                        shipComputerArray[10 - i, j] = shipArray[i, j];
                    }
                    else if (composition == 2)
                    {
                        shipComputerArray[i, 10 - j] = shipArray[i, j];
                    }
                    else
                    {
                        shipComputerArray[10 - i, 10 - j] = shipArray[i, j];
                    }
                }
            }
        }
    }
}
