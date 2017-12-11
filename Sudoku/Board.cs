using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sudoku
{
    public class Board
    {
        public ICommand ButtonCommand { get; set; }
        public delegate void lockedDelegate(int n, bool locked);
        public event lockedDelegate lockedChange = null;

        public List<Cell> cells = new List<Cell>();

        int selectedID = 0;

        public Board()
        {
            ButtonCommand = new RelayCommand(
                new Action<object>(delegate (object obj)
                {
                    string val = obj as string;
                    cells[selectedID].Content = val;
                    Check();
                    if (Win())
                        MessageBox.Show("Congratulations! You won!");
                }));
            for (int i = 0; i < 9*9; i++)
            {
                Cell cell = new Cell(i);
                cell.Clicked += Clicked;
                cells.Add(cell);
            }
            SetBoard();
        }

        public void Clicked(int cellID)
        {
            if (selectedID == cellID)
                return;
            if (cells[cellID].Locked)
                return;
            cells[selectedID].Selected = false;
            selectedID = cellID;
            cells[selectedID].Selected = true;

            Check();
        }
        public void Check()
        {
            int col = selectedID % 9;
            int row = selectedID / 9;
            int sCol = col / 3;
            int sRow = row / 3;
            if (lockedChange == null)
                return;
            for (int i = 1; i < 10; i++)
                lockedChange(i, false);

            for (int i = 0; i < 9 * 9; i++)
            {
                if (i % 9 == col)
                    lockedChange(cells[i].content, true);
                if (i / 9 == row)
                    lockedChange(cells[i].content, true);
                if(i % 9 / 3 == sCol && i / 9 / 3 == sRow)
                    lockedChange(cells[i].content, true);
            }
        }
        public bool Win()
        {
            for (int i = 0; i < 9 * 9; i++)
            {
                if (cells[i].content == 0)
                    return false;
            }
            return true;
        }
        public void SetBoard()
        {
            string[] arr=
            {
                "", "", "", "", "1", "", "8", "9", "",
                "", "9", "8", "", "", "2", "7", "", "",
                "7", "", "5", "3", "", "", "", "", "6",
                "5", "4", "1", "2", "", "", "", "", "",
                "", "", "", "", "", "", "", "", "",
                "", "", "", "", "", "8", "6", "1", "4",
                "4", "", "", "", "", "3", "1", "", "2",
                "", "", "6", "9", "", "", "3", "5", "",
                "", "7", "3", "", "2", "", "", "", ""
            };
            for (int i = 0; i < 9 * 9; i++)
            {
                cells[i].Content = arr[i];
                cells[i].Locked = !(arr[i] == "");
                cells[i].Selected = false;
            }
            selectedID = 0;
            cells[0].Selected = true;
            Check();
        }
    }

    public class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        public delegate void ClickedDelegate(int cellID);
        public event ClickedDelegate Clicked = null;

        int ID;
        public int content = 0;
        private bool locked;
        private bool selected;

        public Cell(int ID)
        {
            this.ID = ID;
        }

        public void Click(object sender, EventArgs e)
        {
            if (Clicked != null)
                Clicked(ID);
        }

        public string Content
        {
            get
            {
                if (content != 0)
                    return content.ToString();
                else
                    return "";
            }
            set
            {
                try
                {
                    content = int.Parse(value);
                }
                catch
                {
                    content = 0;
                }
                OnPropertyChanged("Content");
            }
        }

        public bool Locked
        {
            get { return locked; }
            set
            {
                locked = value;
                OnPropertyChanged("Locked");
            }
        }
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }
        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
