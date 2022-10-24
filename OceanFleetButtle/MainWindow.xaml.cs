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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            for (int i = 0; i <= 10; i++)
            {
                MyAdding(i);
                ComputerAdding(i);
                for (int j = 0; j < 10; j++)
                {
                    if (i != 10) ComputerAddingButton(i, j);
                }
            }
            StartPlaying();
            MessageBox.Show("Вам необходимо разместить корабли" +
                            "(см. в левом нижнем углу). Щелчок по кораблю меняет ориентацию.",
                            "Игра Морской бой");
        }

        List<Rectangle> shipList = new List<Rectangle>();
        List<Tuple<int, int>> computerAttacker = new List<Tuple<int, int>>();
        bool dragging = false;
        int[,] shipArray = new int[10, 10];
        int[,] shipComputerArray = new int[10, 10];
        int allShips = 0;
        bool playing = false;
        int allFields = 35;
        int myAllFields = 35;
        int position = 0;
        public void Restruct(object sender, RoutedEventArgs e)
        {
            if (playing)
            {
                return;
            }

            allShips = 0;
            foreach (var ship in shipList)
            {
                ship.Fill = new SolidColorBrush(Colors.Red);
            }

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    shipArray[i, j] = 0;
        }
        public void ComputerAdding(int i)
        {
            ComputerAddingHorisontal(i, true);
            ComputerAddingHorisontal(i, false);
            Button RestructButton = new Button();
            position = canvy.Children.Count;
            canvy.Children.Add(RestructButton);
            RestructButton.Content = "Перестроить";
            RestructButton.Width = 100;
            RestructButton.Height = 30;
            Canvas.SetRight(RestructButton, 5);
            Canvas.SetBottom(RestructButton, 5);
            RestructButton.Click += new RoutedEventHandler(Restruct);
        }

        public void ComputerAddingHorisontal(int i, bool pos)
        {
            if (i != 0)
            {
                InitLine(i, pos);
            }
            var line = new Line();
            if (pos)
            {
                line.X1 = 25;
                line.Y1 = 25 + i * 25;
                line.X2 = 275;
                line.Y2 = 25 + i * 25;
            }
            else
            {
                line.X1 = 25 + i * 25;
                line.Y1 = 25;
                line.X2 = 25 + i * 25;
                line.Y2 = 275;
            }
            line.Stroke = new SolidColorBrush(Colors.Blue);
            canvy.Children.Add(line);
        }

        private void InitLine(int i, bool pos)
        {
            var label = new Label();
            if (pos) label.Content = (i.ToString());
            else label.Content = (((char)(i - 1 + 'A')).ToString());
            canvy.Children.Add(label);
            if (pos)
            {
                Canvas.SetLeft(label, 2);
                Canvas.SetTop(label, 25 * (i));
            }
            else
            {
                Canvas.SetLeft(label, 25 * (i));
                Canvas.SetTop(label, 2);
            }
        }

        public void ComputerAddingButton(int x, int y)
        {
            var microField = new Button();
            microField.Width = 25;
            microField.Height = 25;
            microField.Background = new SolidColorBrush(Colors.Blue);
            microField.Click += new RoutedEventHandler(MyAttack);
            canvy.Children.Add(microField);
            Canvas.SetLeft(microField, 25 + x * 25);
            Canvas.SetTop(microField, 25 + y * 25);
        }

        public void MyAdding(int i)
        {
            MyAddingHorisontal(i, true);
            MyAddingHorisontal(i, false);
        }
        public void MyAddingHorisontal(int i, bool pos)
        {
            if (i != 0)
            {
                InitMyLine(i, pos);
            }
            var line3 = new Line();
            if (pos)
            {
                line3.X1 = 300;
                line3.Y1 = 25 + i * 25;
                line3.X2 = 550;
                line3.Y2 = 25 + i * 25;
            }
            else
            {
                line3.X1 = 300 + i * 25;
                line3.Y1 = 25;
                line3.X2 = 300 + i * 25;
                line3.Y2 = 275;
            }
            line3.Stroke = new SolidColorBrush(Colors.Red);
            canvy.Children.Add(line3);
        }

        private void InitMyLine(int i, bool pos)
        {
            var label = new Label();
            if (pos) label.Content = (((char)(i + 'A' - 1)).ToString());
            else label.Content = (i.ToString());
            canvy.Children.Add(label);
            if (pos)
            {
                Canvas.SetLeft(label, 275 + 25 * (i));
                Canvas.SetTop(label, 2);
            }
            else
            {
                Canvas.SetLeft(label, 277);
                Canvas.SetTop(label, 25 * (i));
            }
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
        public void Playing(double x, double y, Button curField)
        {
            var IntCoords = IntCoordinates2(x, y);
            var IntX = IntCoords.Item1;
            var IntY = IntCoords.Item2;
            if ((IntX >= 0) && (IntY >= 0) && (IntY < 10) && (IntX < 10))
            {
                CorrectCoordinatesDo(curField, IntX, IntY);
            }
        }

        private void CorrectCoordinatesDo(Button curField, int IntX, int IntY)
        {
            if (shipComputerArray[IntX, IntY] == 1)
            {
                DoIfDamage(curField, IntX, IntY);
            }
            else
            {
                curField.Visibility = Visibility.Hidden;
                //MessageBox.Show("Мимо!");
                ComputerStickBack();
            }
        }

        private void DoIfDamage(Button curField, int IntX, int IntY)
        {
            shipComputerArray[IntX, IntY] = 0;
            curField.Background = new SolidColorBrush(Colors.Yellow);
            allFields--;
            //MessageBox.Show("Ранил!");
            if (allFields == 0)
            {
                FinishPlaying();
            }
        }

        public void AttackGeneration()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    computerAttacker.Add(Tuple.Create(i, j));
                }
            }
        }

        public void ComputerStickBack()
        {
            var rand = new Random();
            var el = rand.Next(computerAttacker.Count - 1);
            var x = computerAttacker[el].Item1;
            var y = computerAttacker[el].Item2;
            computerAttacker.RemoveAt(el);
            if (shipArray[x, y] == 1)
            {
                DamagingMe(x, y);
            }
            else
            {
                //MessageBox.Show("Противник не попал");
                ShowDamage(x, y, false);
            }
        }

        private void DamagingMe(int x, int y)
        {
            shipArray[x, y] = 0;
            ShowDamage(x, y, true);
            //MessageBox.Show("Противник попал");
            myAllFields--;
            if (myAllFields == 0)
            {
                FinishPlaying();
            }
            else
            {
                ComputerStickBack();
            }
        }

        private void ShowDamage(int x, int y, bool color)
        {
            var damage = new Rectangle();
            damage.Width = 25;
            damage.Height = 25;
            if (color) damage.Fill = new SolidColorBrush(Colors.Yellow);
            else damage.Fill = new SolidColorBrush(Colors.AliceBlue);
            canvy.Children.Add(damage);
            Canvas.SetLeft(damage, 300 + 25 * x);
            Canvas.SetTop(damage, 25 + 25 * y);
        }

        public void FinishPlaying()
        {
            if (allFields == 0)
            {
                MessageBox.Show("Мои поздравления! Вы победили!");
            }
            else
            {
                MessageBox.Show("К сожалению, Вы проиграли!");
            }
            window.Close();

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

        void MyAttack(object sender, RoutedEventArgs e)
        {
            if (playing)
            {
                var curField = (Button)e.Source;

                if (curField.Background.ToString() == "#FF0000FF")
                {
                    var x = Canvas.GetLeft(curField);
                    var y = Canvas.GetTop(curField);
                    Playing(x, y, curField);
                }
            }
        }
        string greenColor = "#FFFF0000";
        public void EvMouseDown(object sender, RoutedEventArgs e)
        {
            var curShip = (Rectangle)e.Source;
            if (curShip.Fill.ToString() == greenColor)
            {
                dragging = true;
                Mouse.Capture(curShip);
            }
        }

        public void EvMouseUp(object sender, RoutedEventArgs e)
        {
            if (dragging)
            {
                var curShip = (Rectangle)e.Source;
                dragging = false;
                Mouse.Capture(null);
                CorrectingPosition(curShip);
            }
        }

        public void RotateShips()
        {
            foreach (var ship in shipList)
            {
                if (ship.Fill.ToString() == greenColor)
                {
                    var heigth = ship.Height;
                    ship.Height = ship.Width;
                    ship.Width = heigth;
                }
            }
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
            int intX = (Convert.ToInt32(x) + 13) / 25 - 12;
            int intY = (Convert.ToInt32(y) + 13) / 25 - 1;
            return Tuple.Create(intX, intY);
        }

        public Tuple<int, int> IntCoordinates2(double x, double y)
        {
            int intX = (Convert.ToInt32(x) + 13) / 25 - 1;
            int intY = (Convert.ToInt32(y) + 13) / 25 - 1;
            return Tuple.Create(intX, intY);
        }
        public bool FillShipArray(Rectangle ship, int x, int y)
        {
            var width = Convert.ToInt16(ship.Width) / 25;
            var height = Convert.ToInt16(ship.Height) / 25;
            var isValid = true;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int kx = -1; kx < 2; kx++)
                    {
                        for (int ky = -1; ky < 2; ky++)
                        {
                            if (isValid) isValid = Filling(x, y, i, j, kx, ky);
                        }
                    }
                }
            }
            if (isValid) FillMyArray(x, y, width, height);
            return isValid;
        }

        private void FillMyArray(int x, int y, int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    shipArray[i + x, j + y] = 1;

                }
            }
        }

        private bool Filling(int x, int y, int i, int j, int kx, int ky)
        {
            try
            {
                if (shipArray[i + x + kx, j + y + ky] == 1) return false;
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return true;
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
            if (DoingCondicion(uncorrect, intX, intY))
            {
                DoIfErrorDragging(uncorrect);
            }
            else
            {
                FillRectangle(uncorrect, intX, intY);
            }
            if (allShips == 15)
            {
                playing = true;
                GenerateComputerField();
                MessageBox.Show("А теперь давайте поиграем!");
                //canvy.Children[position].Visibility = Visibility.Hidden;
                AttackGeneration();
            }
        }

        private void DoIfErrorDragging(Rectangle uncorrect)
        {
            Canvas.SetTop(uncorrect, 325);
            Canvas.SetLeft(uncorrect, 25);
            RotateShips();
            allShips--;
        }

        private void FillRectangle(Rectangle incorrect, int intX, int intY)
        {
            Canvas.SetLeft(incorrect, intX * 25 + 300);
            Canvas.SetTop(incorrect, intY * 25 + 25);
            var isValid = FillShipArray(incorrect, intX, intY);
            incorrect.Fill = new SolidColorBrush(Colors.Green);
            if (!isValid)
            {
                incorrect.Fill = new SolidColorBrush(Colors.Red);
                Canvas.SetTop(incorrect, 325);
                Canvas.SetLeft(incorrect, 25);
                allShips--;
            }
        }

        private static bool DoingCondicion(Rectangle uncorrect, int intX, int intY)
        {
            return (intX < 0) || (intY < 0) ||
                    (intX > 10 - Convert.ToInt64(uncorrect.Width) / 25) ||
                    (intY > 10 - uncorrect.Height / 25);
        }

        public void GenerateComputerField()
        {
            var rand = new Random();
            var composition = rand.Next(4);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    AutoGenerating(composition, i, j);
                }
            }
        }

        private void AutoGenerating(int composition, int i, int j)
        {
            if (composition == 0)
            {
                shipComputerArray[i, j] = shipArray[i, j];
            }
            else if (composition == 1)
            {
                shipComputerArray[9 - i, j] = shipArray[i, j];
            }
            else if (composition == 2)
            {
                shipComputerArray[i, 9 - j] = shipArray[i, j];
            }
            else
            {
                shipComputerArray[9 - i, 9 - j] = shipArray[i, j];
            }
        }
    }
}
