using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Board board;
        public MainWindow()
        {
            InitializeComponent();
            board = new Board();
            tilesGrid.DataContext = board;
            drawBoxes();
            NewGame.Click += (s, e) => { board.SetBoard(); };
            board.lockedChange += (n, locked) =>
            {
                switch (n)
                {
                    case 1: B1.IsEnabled = !locked; break;
                    case 2: B2.IsEnabled = !locked; break;
                    case 3: B3.IsEnabled = !locked; break;
                    case 4: B4.IsEnabled = !locked; break;
                    case 5: B5.IsEnabled = !locked; break;
                    case 6: B6.IsEnabled = !locked; break;
                    case 7: B7.IsEnabled = !locked; break;
                    case 8: B8.IsEnabled = !locked; break;
                    case 9: B9.IsEnabled = !locked; break;
                    default: break;
                }
            };
            board.Check();
        }

        public void drawBoxes()
        {
            for(int i = 0; i < 9; i++)
                for(int j = 0; j < 9; j++)
                {
                    int tT = i % 3 == 0 ? 2 : 1;
                    int bT = i % 3 == 2 ? 2 : 1;
                    int lT = j % 3 == 0 ? 2 : 1;
                    int rT = j % 3 == 2 ? 2 : 1;
                    Label lab = new Label()
                    {
                        BorderThickness = new Thickness(lT, tT, rT, bT),
                        BorderBrush = Brushes.Black,
                        Content = (i * 10 + j).ToString(),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontStretch = FontStretches.Normal,
                        FontSize = 16          
                };
                    Binding contentBbind = new Binding();
                    contentBbind.Mode = BindingMode.Default;
                    contentBbind.Source = board.cells;
                    contentBbind.Path = new PropertyPath("["+ (i * 9 + j).ToString() + "].Content");
                    lab.SetBinding(Label.ContentProperty, contentBbind);

                    Binding selectedBind = new Binding();
                    selectedBind.Converter = new BoolToBrushConverter();        
                    selectedBind.Mode = BindingMode.Default;
                    selectedBind.Source = board.cells;
                    selectedBind.Path = new PropertyPath("[" + (i * 9 + j).ToString() + "].Selected");
                    lab.SetBinding(Label.BackgroundProperty, selectedBind);

                    Binding lockedBind = new Binding();
                    lockedBind.Converter = new BoolToBrushConverter2();
                    lockedBind.Mode = BindingMode.Default;
                    lockedBind.Source = board.cells;
                    lockedBind.Path = new PropertyPath("[" + (i * 9 + j).ToString() + "].Locked");
                    lab.SetBinding(Label.ForegroundProperty, lockedBind);

                    tilesGrid.Children.Add(lab);
                    Grid.SetColumn(lab, j);
                    Grid.SetRow(lab, i);
                    lab.MouseDown += board.cells[i*9+j].Click;
                }
        }

        private void MyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Brushes.LightGray;
            return Brushes.Beige;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolToBrushConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Brushes.DarkGray;
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
