using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WinMine
{
    public partial class WinMine : Form
    {
        #region Global Variables
        Options optionsDialog = new Options();
        int _buttonSize = 20;
        bool _newBoard = true;
        bool _timing = false;
        int _Tmin = 0;
        int _Tsec = 0;
        Pen _penThick = new Pen(Color.Black, 3);
        Font _Font = new Font("Arial", 12, FontStyle.Regular);
        Font _PFont = new Font("Arial", 7, FontStyle.Regular);
        Rectangle _board = new Rectangle(0, 0, 0, 0);
        Rectangle[,] _boardArray = new Rectangle[0, 0];
        int _selectedRow = -1;
        int _selectedCol = -1;
        Rectangle _selectedRect;
        string[,] _Hgrid = null;
        string[,] _Sgrid = null;
        bool _play = false;
        Random _rand = new Random((int)System.DateTime.Now.Ticks);
        string _cheat = "";
        bool qwe = false;
        bool _z = true;
        CaptureScreen _desktop = new CaptureScreen();
        int xx = -1;
        int yy = -1;
        #endregion
        public WinMine()
        {
            InitializeComponent();
            InitializeBoardArray();
            CreateGrid();
            saveToolStripMenuItem.Visible = false;
            openToolStripMenuItem.Visible = false;
        }
        private void WinMine_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
        }
        private void WinMine_MouseDown(object sender, MouseEventArgs e)
        {
            if (label1.Text == "Enter")
            {
                Rectangle rect = _selectedRect;
                _selectedRect = TranslateToRectBounds(new Point(e.X, e.Y), out _selectedRow, out _selectedCol);
                Invalidate(rect);
                Invalidate(_selectedRect);
            }
            else
            {
                _selectedRect = TranslateToRectBounds(new Point(e.X, e.Y), out _selectedRow, out _selectedCol);
                MousClick(_selectedRow, _selectedCol, e.Button, true);
            }
        }
        private void WinMine_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'e')
            {
                
                if (_cheat == "solv")
                {
                    _cheat = "";
                    label1.Text = "Solve";
                    label1.Left = (this.ClientSize.Width / 2) - (label1.Width / 2);
                    ShowPercentages();
                    //EventArgs ev = null;
                    //startToolStripMenuItem_Click(sender, ev);
                }
                else if(label1.Text == "Enter")
                {
                    _Sgrid[_selectedRow, _selectedCol] = e.KeyChar.ToString();
                    Invalidate(_selectedRect);
                    for (int r = 0; r < optionsDialog._rows; r++)
                    {
                        for (int c = 0; c < optionsDialog._cols; c++)
                        {
                            if (_Sgrid[r, c].StartsWith("e") || _Sgrid[r, c] == "l" || _Sgrid[r, c] == "g")
                            {
                                _Sgrid[r, c] = "e";
                            }
                        }
                    }
                    ShowPercentages();
                }
                else if (_cheat == "ent")
                {
                    _cheat = "ente";
                }
                else
                {
                    if (_cheat != "")
                        System.Console.Beep();
                    _cheat = "e";
                }
            }
            else if (e.KeyChar == 'n')
            {
                if (_cheat == "e")
                {
                    _cheat = "en";
                }
                else if (_cheat != "")
                {
                    System.Console.Beep();
                    _cheat = "";
                }
            }
            else if (e.KeyChar == 't')
            {
                if (_cheat == "en")
                {
                    _cheat = "ent";
                }
                else if (_cheat != "")
                {
                    System.Console.Beep();
                    _cheat = "";
                }
            }
            else if (e.KeyChar == 'r')
            {
                if (_cheat == "ente")
                {
                    _cheat = "";
                    label1.Text = "Enter";
                    label1.Left = (this.ClientSize.Width / 2) - (label1.Width / 2);
                    EventArgs ev = null;
                    startToolStripMenuItem_Click(sender, ev);
                    _desktop = new CaptureScreen();
                }
                else if (_cheat != "")
                {
                    System.Console.Beep();
                    _cheat = "";
                }
            }
            else if (e.KeyChar == 's')
            {
                if (_cheat != "")
                {
                    System.Console.Beep();
                }
                _cheat = "s";
            }
            else if (e.KeyChar == 'o')
            {
                if (_cheat == "s")
                {
                    _cheat = "so";
                }
                else if (_cheat != "")
                {
                    System.Console.Beep();
                    _cheat = "";
                }
            }
            else if (e.KeyChar == 'l')
            {
                if (_cheat == "so")
                {
                    _cheat = "sol";
                }
                else if (_cheat != "")
                {
                    System.Console.Beep();
                    _cheat = "";
                }
            }
            else if (e.KeyChar == 'v')
            {
                if (_cheat == "sol")
                {
                    _cheat = "solv";
                }
                else if (_cheat != "")
                {
                    System.Console.Beep();
                    _cheat = "";
                }
            }
            else if (e.KeyChar == 'x')
            {
                _cheat = "";
                label1.Text = "";
                EventArgs ev = null;
                startToolStripMenuItem_Click(sender, ev);
            }
            else if (e.KeyChar == 'z' && (label1.Text == "Solve" || label1.Text == "Solvez"))
            {
                _z = !_z;
                if (label1.Text == "Solve")
                {
                    label1.Text = "Solvez";
                }
                else
                {
                    label1.Text = "Solve";
                }
                if (!_z)
                {
                    for (int r = 0; r < optionsDialog._rows; r++)
                    {
                        for (int c = 0; c < optionsDialog._cols; c++)
                        {
                            if (_Sgrid[r, c] == "l")
                            {
                                _Sgrid[r, c] = "e";
                            }
                        }
                    }
                }
                ShowPercentages();
                //EventArgs ev = null;
                //startToolStripMenuItem_Click(sender, ev);
            }
            else if (_cheat != "")
            {
                System.Console.Beep();
                _cheat = "";
            }
            else if (label1.Text == "Enter" && _selectedCol != -1)
            {
                if (e.KeyChar == ' ')
                {
                    _Sgrid[_selectedRow, _selectedCol] = "g";
                    Invalidate(_selectedRect);
                    for (int r = 0; r < optionsDialog._rows; r++)
                    {
                        for (int c = 0; c < optionsDialog._cols; c++)
                        {
                            if (_Sgrid[r, c].StartsWith("e") || _Sgrid[r, c] == "l")// || _Sgrid[r, c] == "g")
                            {
                                _Sgrid[r, c] = "e";
                            }
                        }
                    }
                    ShowPercentages();
                }
                else if (e.KeyChar >= '0' && e.KeyChar <= '8')
                {
                    _Sgrid[_selectedRow, _selectedCol] = e.KeyChar.ToString();
                    Invalidate(_selectedRect);
                    for (int r = 0; r < optionsDialog._rows; r++)
                    {
                        for (int c = 0; c < optionsDialog._cols; c++)
                        {
                            if (_Sgrid[r, c].StartsWith("e") || _Sgrid[r, c] == "l" || _Sgrid[r, c] == "g")
                            {
                                _Sgrid[r, c] = "e";
                            }
                        }
                    }
                    ShowPercentages();
                }
            }
        }
        private void WinMine_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (label1.Text == "Enter")
            {
                if (_selectedRow >= 0 && _selectedCol >= 0)
                {
                    int row = _selectedRow;
                    int col = _selectedCol;
                    if (e.KeyValue == 39)
                    {
                        _selectedCol++;
                        if (_selectedCol == optionsDialog._cols)
                            _selectedCol = 0;
                    }
                    else if (e.KeyValue == 37)
                    {
                        _selectedCol--;
                        if (_selectedCol == -1)
                            _selectedCol = optionsDialog._cols - 1;
                    }
                    else if (e.KeyValue == 40)
                    {
                        _selectedRow++;
                        if (_selectedRow == optionsDialog._rows)
                            _selectedRow = 0;
                    }
                    else if (e.KeyValue == 38)
                    {
                        _selectedRow--;
                        if (_selectedRow == -1)
                            _selectedRow = optionsDialog._rows - 1;
                    }
                    _selectedRect = _boardArray[_selectedRow, _selectedCol];
                    Invalidate(_boardArray[row, col]);
                    Invalidate(_selectedRect);
                }
                else
                {
                    if (e.KeyValue == 39)
                    {
                        for (int r = 0; r < optionsDialog._rows; r++)
                        {
                            for (int c = optionsDialog._cols - 1; c >= 0; c--)
                            {
                                if (c == 0)
                                {
                                    _Sgrid[r, c] = "e";
                                }
                                else
                                {
                                    _Sgrid[r, c] = _Sgrid[r, c - 1];
                                }
                            }
                        }
                    }
                    else if (e.KeyValue == 37)
                    {
                        for (int r = 0; r < optionsDialog._rows; r++)
                        {
                            for (int c = 0; c < optionsDialog._cols; c++)
                            {
                                if (c == optionsDialog._cols - 1)
                                {
                                    _Sgrid[r, c] = "e";
                                }
                                else
                                {
                                    _Sgrid[r, c] = _Sgrid[r, c + 1];
                                }
                            }
                        }
                    }
                    else if (e.KeyValue == 40)
                    {
                        for (int r = optionsDialog._rows - 1; r >= 0; r--)
                        {
                            for (int c = 0; c < optionsDialog._cols; c++)
                            {
                                if (r == 0)
                                {
                                    _Sgrid[r, c] = "e";
                                }
                                else
                                {
                                    _Sgrid[r, c] = _Sgrid[r - 1, c];
                                }
                            }
                        }
                    }
                    else if (e.KeyValue == 38)
                    {
                        for (int r = 0; r < optionsDialog._rows; r++)
                        {
                            for (int c = 0; c < optionsDialog._cols; c++)
                            {
                                if (r == optionsDialog._rows - 1)
                                {
                                    _Sgrid[r, c] = "e";
                                }
                                else
                                {
                                    _Sgrid[r, c] = _Sgrid[r + 1, c];
                                }
                            }
                        }
                    }
                    ShowPercentages();
                    Invalidate();
                }
            }
        }
        private void MousClick(int sRow, int sCol, MouseButtons b, bool SP)
        {
            if (_play && sCol > -1)
            {
                if (b == MouseButtons.Left && _Sgrid[sRow, sCol] != _Hgrid[sRow, sCol])
                {
                    int num = 0;
                    while (_newBoard && (_Hgrid[sRow, sCol] == "☻"|| (_Hgrid[sRow,sCol] != "0" && num < 100)))
                    {
                        num++;
                        CreateGrid();
                    }
                    _timing = true;
                    _Sgrid[sRow, sCol] = _Hgrid[sRow, sCol];
                    _newBoard = false;
                    startToolStripMenuItem.Text = "Restart";
                    Invalidate(_selectedRect);
                    if (_Sgrid[sRow, sCol] == "☻")
                    {
                        ShowBombs();
                    }
                    else if (_Sgrid[sRow, sCol] == "0")
                    {
                        NoBombs(sRow, sCol);
                    }
                }
                else if (b == MouseButtons.Right)
                {
                    if (_Sgrid[sRow, sCol].StartsWith("e") || _Sgrid[sRow, sCol] == "l")
                    {
                        _Sgrid[sRow, sCol] = "g";
                        Invalidate(_selectedRect);
                        for (int r = 0; r < optionsDialog._rows; r++)
                        {
                            for (int c = 0; c < optionsDialog._cols; c++)
                            {
                                if (_Sgrid[r, c].StartsWith("e"))
                                {
                                    _Sgrid[r, c] = "e";
                                }
                            }
                        }
                    }
                    else if (_Sgrid[sRow, sCol] == "g")
                    {
                        _Sgrid[sRow, sCol] = "e";
                        Invalidate(_selectedRect);
                    }
                    else
                    {
                        int bombs = 0;
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (sRow + i > -1 && sRow + i < optionsDialog._rows && sCol + j > -1 && sCol + j < optionsDialog._cols && _Sgrid[sRow + i, sCol + j] == "g")
                                {
                                    bombs++;
                                }
                            }
                        }
                        if (Convert.ToInt32(_Sgrid[sRow, sCol].Substring(0,1)) == bombs)
                        {
                            for (int i = -1; i < 2; i++)
                            {
                                for (int j = -1; j < 2; j++)
                                {
                                    if (sRow + i > -1 && sRow + i < optionsDialog._rows && sCol + j > -1 && sCol + j < optionsDialog._cols && _Sgrid[sRow + i, sCol + j] != "g")
                                    {
                                        if (_Sgrid[sRow + i, sCol + j] != _Hgrid[sRow + i, sCol + j])
                                        {
                                            _Sgrid[sRow + i, sCol + j] = _Hgrid[sRow + i, sCol + j];
                                            Invalidate(_boardArray[sRow + i, sCol + j]);
                                        }
                                        if (_Sgrid[sRow + i, sCol + j] == "☻")
                                        {
                                            if (qwe)
                                            {
                                                _selectedRow = sRow;
                                                _selectedCol = sCol;
                                                _selectedRect = _boardArray[_selectedRow, _selectedCol];
                                                Invalidate();
                                                MessageBox.Show("Oops! " + sRow.ToString() + " " + sCol.ToString());
                                            }
                                            ShowBombs();
                                        }
                                        if (_Sgrid[sRow + i, sCol + j] == "0")
                                        {
                                            NoBombs(sRow + i, sCol + j);
                                        }
                                    }
                                }
                            }
                            qwe = false;
                        }
                    }
                }
                else if (b == MouseButtons.Middle)
                {
                    MessageBox.Show(_selectedRow.ToString() + ", " + _selectedCol.ToString());
                }
                if (_play && CheckDone())
                {
                    _timing = false;
                    Invalidate();
                    System.Windows.Forms.MessageBox.Show("Congratulations!!! You won in " + timeToolStripMenuItem1.Text);
                    _play = false;
                    startToolStripMenuItem.Text = "Start";
                }
                else if(label1.Text != "" && SP)
                {
                    ShowPercentages();
                }
            }
        }
        private void InitializeBoardArray()
        {
            _board = new Rectangle(7, 7 + menuStrip1.Height, optionsDialog._cols * _buttonSize, optionsDialog._rows * _buttonSize);
            _boardArray = new Rectangle[optionsDialog._rows, optionsDialog._cols];
            int spacingX = _board.Width / optionsDialog._cols;
            int spacingY = _board.Height / optionsDialog._rows;
            for (int col = 0; col < optionsDialog._cols; col++)
            {
                for (int row = 0; row < optionsDialog._rows; row++)
                {
                    _boardArray[row, col] = new Rectangle(_board.Left + col * spacingX + 1, _board.Top + row * spacingY + 1, spacingX - 1, spacingY - 1);
                }
            }
        }
        private Rectangle TranslateToRectBounds(Point p, out int selectedRow, out int selectedCol)
        {
            for (int col = 0; col < optionsDialog._cols; col++)
            {
                for (int row = 0; row < optionsDialog._rows; row++)
                {
                    if (_boardArray[row, col].Contains(p))
                    {
                        selectedRow = row;
                        selectedCol = col;
                        return _boardArray[row, col];
                    }
                }
            }

            selectedRow = -1;
            selectedCol = -1;
            return new Rectangle(0, 0, 0, 0);
        }
        private void DrawGrid(Graphics g)
        {
            this.Height = 50 + menuStrip1.Height + optionsDialog._rows * _buttonSize;
            this.Width = 31 + optionsDialog._cols * _buttonSize;
            _board = new Rectangle(7, 7 + menuStrip1.Height, optionsDialog._cols * _buttonSize, optionsDialog._rows * _buttonSize);
            g.DrawRectangle(_penThick, _board);
            label1.Left = (this.ClientSize.Width / 2) - (label1.Width / 2);

            int mines = 0;
            int spacingX = _board.Width / optionsDialog._cols;
            int spacingY = _board.Height / optionsDialog._rows;
            for (int i = 0; i < ((optionsDialog._cols > optionsDialog._rows) ? optionsDialog._cols : optionsDialog._rows); i++)
            {
                if (i < optionsDialog._rows)
                {
                    g.DrawLine(Pens.Black, _board.Left, _board.Top + spacingY * i, _board.Right, _board.Top + spacingY * i);
                }
                if (i < optionsDialog._cols)
                {
                    g.DrawLine(Pens.Black, _board.Left + spacingX * i, _board.Top, _board.Left + spacingX * i, _board.Bottom);
                }
            }
            for (int row = 0; row < optionsDialog._rows; row++)
            {
                for (int col = 0; col < optionsDialog._cols; col++)
                {
                    if (_Sgrid[row, col].StartsWith("e"))
                    {
                        Brush b = Brushes.White;
                        if (_selectedRow == row && _selectedCol == col && (label1.Text == "Enter" || qwe))
                        {
                            g.FillRectangle(Brushes.Yellow, _selectedRect);
                            b = Brushes.Black;
                        }
                        else
                        {
                            g.FillRectangle(Brushes.Teal, _boardArray[row, col]);
                        }
                        if (_Sgrid[row, col] != "e")
                        {
                            g.DrawString(_Sgrid[row, col].Substring(3, 2) + "%", _PFont, b, _board.Left + col * spacingX, _board.Top + row * spacingY + 4, new StringFormat());
                        }
                    }
                    else
                    {
                        if (_Sgrid[row, col] == "l")
                        {
                            if (_selectedRow == row && _selectedCol == col && (label1.Text == "Enter" || qwe))
                            {
                                g.FillRectangle(Brushes.Yellow, _selectedRect);
                            }
                            else
                            {
                                g.FillRectangle(Brushes.Green, _boardArray[row, col]);
                            }
                        }
                        else if (_Sgrid[row, col] == "g")
                        {
                            mines++;
                            if (_selectedRow == row && _selectedCol == col && (label1.Text == "Enter" || qwe))
                            {
                                g.FillRectangle(Brushes.Yellow, _selectedRect);
                                g.DrawString("☻", _Font, Brushes.Black, _board.Left + col * spacingX - 1, _board.Top + row * spacingY - 1, new StringFormat());
                            }
                            else
                            {
                                g.FillRectangle(Brushes.Red, _boardArray[row, col]);
                                g.DrawString("☻", _Font, Brushes.White, _board.Left + col * spacingX - 1, _board.Top + row * spacingY - 1, new StringFormat());
                            }
                        }
                        else
                        {
                            if (_selectedRow == row && _selectedCol == col && (label1.Text == "Enter" || qwe))
                            {
                                g.FillRectangle(Brushes.Yellow, _selectedRect);
                            }
                            else
                            {
                                g.FillRectangle(Brushes.Azure, _boardArray[row, col]);
                            }
                            if (_Sgrid[row, col] != "0")
                            {
                                if (_Sgrid[row, col] == "☻")
                                {
                                    g.DrawString(_Sgrid[row, col], _Font, Brushes.Black, _board.Left + col * spacingX - 1, _board.Top + row * spacingY - 1, new StringFormat());
                                }
                                else
                                {
                                    g.DrawString(_Sgrid[row, col].Substring(0,1), _Font, Brushes.Black, _board.Left + col * spacingX + 3, _board.Top + row * spacingY + 2, new StringFormat());
                                }
                            }
                        }
                    }
                }
            }
            minesToolStripMenuItem.Text = "Mines: " + (optionsDialog._mines - mines).ToString();
            
            for (int row = 0; row < optionsDialog._rows; row++)
            {
                for (int col = 0; col < optionsDialog._cols ; col++)
                {
                    g.FillRectangle(Brushes.Black, _boardArray[row, col].Right - 1, _boardArray[row, col].Bottom - 1, 3, 3);
                }
            }
            for (int row = 0; row < optionsDialog._rows; row++)
            {
                for (int col = 0; col < optionsDialog._cols; col++)
                {
                    g.FillRectangle(Brushes.Black, _boardArray[row, col].Left - 2, _boardArray[row, col].Top - 2, 3, 3);
                }
            }
        }
        private void CreateGrid()
        {
            _newBoard = true;
            _Hgrid = new string[optionsDialog._rows, optionsDialog._cols];
            _Sgrid = new string[optionsDialog._rows, optionsDialog._cols];
            ResetGrid();
            int mines = 0;
            if (label1.Text == "Enter")
            {
                return;
            }
            while (mines < optionsDialog._mines)
            {
                if (_Hgrid[_rand.Next() % optionsDialog._rows, _rand.Next() % optionsDialog._cols] != "☻")
                {
                    _Hgrid[_rand.Next() % optionsDialog._rows, _rand.Next() % optionsDialog._cols] = "☻";
                    mines++;
                }
                if (mines == optionsDialog._mines)
                {
                    mines = CheckMines();
                }
            }
            for (int row = 0; row < optionsDialog._rows; row++)
            {
                for (int col = 0; col < optionsDialog._cols; col++)
                {
                    if (_Hgrid[row, col] != "☻")
                    {
                        int bombs = 0;
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (row + i > -1 && row + i < optionsDialog._rows && col + j > -1 && col + j < optionsDialog._cols && _Hgrid[row + i, col + j] == "☻")
                                {
                                    bombs++;
                                }
                            }
                        }
                        _Hgrid[row, col] = bombs.ToString();
                    }
                }
            }
            _play = true;
        }
        private void ResetGrid()
        {
            for (int i = 0; i < optionsDialog._rows; i++)
                for (int j = 0; j < optionsDialog._cols; j++)
                {
                    _Hgrid[i, j] = "e";
                    _Sgrid[i, j] = "e";
                }
        }
        private void ShowBombs()
        {
            _play = false;
            _timing = false;
            for (int i = 0; i < optionsDialog._rows; i++)
            {
                for (int j = 0; j < optionsDialog._cols; j++)
                {
                    _Sgrid[i, j] = _Hgrid[i, j];
                }
            }
            Invalidate();
        }
        private void NoBombs(int row, int col)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (row + i > -1 && row + i < optionsDialog._rows && col + j > -1 && col + j < optionsDialog._cols && (_Sgrid[row + i, col + j].StartsWith("e") || _Sgrid[row + i, col + j].StartsWith("l")))
                    {
                        _Sgrid[row + i, col + j] = _Hgrid[row + i, col + j];
                        Invalidate(_boardArray[row + i, col + j]);
                        if (_Sgrid[row + i, col + j] == "0")
                        {
                            NoBombs(row + i, col + j);
                        }
                    }
                }
            }
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateGrid();
            Invalidate();
            _timing = false;
            _Tmin = 0;
            _Tsec = 0;
            timeToolStripMenuItem1.Text = "0:00";
            _selectedCol = -1;
            _selectedRow = -1;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label1.Text == "Enter")
            {
                getGrid();
            }
            else if (_timing)
            {
                _Tsec++;
                if (_Tsec == 60)
                {
                    _Tsec = 0;
                    _Tmin++;
                }
                timeToolStripMenuItem1.Text = _Tmin + ":";
                if (_Tsec < 10)
                {
                    timeToolStripMenuItem1.Text += "0" + _Tsec;
                }
                else
                {
                    timeToolStripMenuItem1.Text += _Tsec;
                }
            }
        }
        private bool CheckDone()
        {
            for (int row = 0; row < optionsDialog._rows; row++)
            {
                for (int col = 0; col < optionsDialog._cols; col++)
                {
                    if (_Sgrid[row, col].Substring(0,1) != _Hgrid[row, col] && (_Sgrid[row, col] != "g" || _Hgrid[row, col] != "☻"))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private int CheckMines()
        {
            int i = 0;
            for (int row = 0; row < optionsDialog._rows; row++)
            {
                for (int col = 0; col < optionsDialog._cols; col++)
                {
                    if (_Hgrid[row, col] == "☻")
                        i++;
                }
            }
            return i;
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            optionsDialog.ShowDialog();
            InitializeBoardArray();
            CreateGrid();
            Invalidate();
            _timing = false;
            _Tmin = 0;
            _Tsec = 0;
            timeToolStripMenuItem1.Text = "0:00";
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.ShowDialog();
        }
        private void WinMine_Resize(object sender, EventArgs e)
        {
            this.Height = 50 + menuStrip1.Height + optionsDialog._rows * _buttonSize;
            this.Width = 31 + optionsDialog._cols * _buttonSize;
        }
        private void ShowPercentages()
        {
            bool changed = false;
            for (int row = 0; row < optionsDialog._rows; row++)
            {
                for (int col = 0; col < optionsDialog._cols; col++)
                {
                    if (row == 5 && col == 26)
                        row = 5;
                    if (_Sgrid[row, col] != "g" && _Sgrid[row, col] != "0" && _Sgrid[row, col] != "☻" && !_Sgrid[row, col].StartsWith("e") && _Sgrid[row, col] != "l")
                    {
                        int num = Convert.ToInt32(_Sgrid[row, col].Substring(0,1));
                        int num50 = 0;
                        int num250 = 0;
                        double empty = 0;
                        string s50 = "";
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (row + i > -1 && row + i < optionsDialog._rows && col + j > -1 && col + j < optionsDialog._cols)
                                {
                                    if (_Sgrid[row + i, col + j] == "g")
                                    {
                                        num--;
                                    }
                                    else if (_Sgrid[row + i, col + j].StartsWith("e"))
                                    {
                                        if (_Sgrid[row + i, col + j].StartsWith("e0.51"))
                                        {
                                            num250++;
                                            for (int k = -1; k < 2; k++)
                                            {
                                                for (int l = -1; l < 2; l++)
                                                {
                                                    if ((k != i || l != j) && row + k > -1 && row + k < optionsDialog._rows && col + l > -1 && col + l < optionsDialog._cols && _Sgrid[row + k, col + l].StartsWith("e0.51"))
                                                    {
                                                        foreach (string a in _Sgrid[row + i, col + j].Split(';'))
                                                        {
                                                            if (_Sgrid[row + k, col + l].Contains(";" + a) && !s50.Contains(a))
                                                            {
                                                                if (s50 == "")
                                                                {
                                                                    s50 = a;
                                                                }
                                                                else
                                                                {
                                                                    s50 += ";" + a;
                                                                }
                                                                num50++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        empty++;
                                    }
                                }
                            }
                        }
                        num50 = (num50 > num250 / 2) ? num250 / 2 : num50;
                        num -= num50;
                        _Sgrid[row, col] = _Sgrid[row, col].Substring(0, 1) + num.ToString();
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (row + i > -1 && row + i < optionsDialog._rows && col + j > -1 && col + j < optionsDialog._cols && _Sgrid[row + i, col + j].StartsWith("e"))
                                {
                                    if (num == 0)
                                    {
                                        if (label1.Text == "Enter")
                                        {
                                            if (num50 < 1)
                                            {
                                                _Sgrid[row + i, col + j] = "l";
                                                Invalidate(_boardArray[row + i, col + j]);
                                            }
                                            else
                                            {
                                                for (int r = -1; r < 2; r++)
                                                {
                                                    for (int c = -1; c < 2; c++)
                                                    {
                                                        if (row + r > -1 && row + r < optionsDialog._rows && col + c > -1 && col + c < optionsDialog._cols && _Sgrid[row + r, col + c].StartsWith("e") && !_Sgrid[row + r, col + c].StartsWith("e0.51"))
                                                        {
                                                            _Sgrid[row + r, col + c] = "l";
                                                            Invalidate(_boardArray[row + r, col + c]);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (num50 < 1)
                                            {
                                                if (_z)
                                                {
                                                    for (int r = -1; r < 2; r++)
                                                    {
                                                        for (int c = -1; c < 2; c++)
                                                        {
                                                            if (row + r > -1 && row + r < optionsDialog._rows && col + c > -1 && col + c < optionsDialog._cols && _Sgrid[row + r, col + c].StartsWith("e"))
                                                            {
                                                                _Sgrid[row + r, col + c] = "l";
                                                                Invalidate(_boardArray[row + r, col + c]);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    qwe = true;
                                                    MousClick(row, col, MouseButtons.Right, false);
                                                    changed = true;
                                                }
                                            }
                                            else
                                            {
                                                for (int r = -1; r < 2; r++)
                                                {
                                                    for (int c = -1; c < 2; c++)
                                                    {
                                                        if (row + r > -1 && row + r < optionsDialog._rows && col + c > -1 && col + c < optionsDialog._cols && _Sgrid[row + r, col + c].StartsWith("e") && !_Sgrid[row + r, col + c].StartsWith("e0.51"))
                                                        {
                                                            if (_Hgrid[row + r, col + c] == "☻")
                                                            {
                                                                _selectedCol = col + c;
                                                                _selectedRow = row + r;
                                                                _selectedRect = _boardArray[_selectedRow, _selectedCol];
                                                                qwe = true;
                                                                Invalidate();
                                                                MessageBox.Show("Oops! " + (row + r).ToString() + " " + (col + c).ToString());
                                                                qwe = false;
                                                            }
                                                            if (_z)
                                                            {
                                                                _Sgrid[row + r, col + c] = "l";
                                                                Invalidate(_boardArray[row + r, col + c]);
                                                            }
                                                            else
                                                            {
                                                                MousClick(row + r, col + c, MouseButtons.Left, false);
                                                                Invalidate();
                                                                changed = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        double temp = -1.0;
                                        if (_Sgrid[row + i, col + j].Length > 1)
                                        {
                                            temp = Convert.ToDouble(_Sgrid[row + i, col + j].Substring(1,4));
                                        }
                                        if ((num == 1 && empty == 2 && temp != 1) || (temp == 0.51 && num != empty))
                                        {
                                            if (num == 1 && empty == 2)                                               
                                            {
                                                if (!_Sgrid[row + i, col + j].StartsWith("e0.51"))
                                                {
                                                    _Sgrid[row + i, col + j] = "e0.51;" + row.ToString() + "," + col.ToString();
                                                    Invalidate(_boardArray[row + i, col + j]);
                                                }
                                                else if (!_Sgrid[row + i, col + j].Contains(";" + row.ToString() + "," + col.ToString()))
                                                {
                                                    _Sgrid[row + i, col + j] += ";" + row.ToString() + "," + col.ToString();
                                                }
                                            }
                                        }
                                        else if (num / (empty - (num50 * 2)) > temp)
                                        {
                                            if (_Sgrid[row + i, col + j] != "e" + (num / (empty - (num50 * 2))).ToString("N2"))
                                            {
                                                _Sgrid[row + i, col + j] = "e" + (num / (empty - (num50 * 2))).ToString("N2");
                                                Invalidate(_boardArray[row + i, col + j]);
                                            }
                                        }
                                        if (_Sgrid[row + i, col + j] == "e1.00")
                                        {
                                            //if (label1.Text == "Enter")
                                            //{
                                                _Sgrid[row + i, col + j] = "g";
                                                Invalidate(_selectedRect);
                                                for (int r = 0; r < optionsDialog._rows; r++)
                                                {
                                                    for (int c = 0; c < optionsDialog._cols; c++)
                                                    {
                                                        if (_Sgrid[r, c].StartsWith("e"))
                                                        {
                                                            _Sgrid[r, c] = "e";
                                                        }
                                                    }
                                                }
                                                ShowPercentages();
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    MousClick(row + i, col + j, MouseButtons.Right, false);
                                            //    changed = true;
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (changed)
            {
                ShowPercentages();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey board = Registry.CurrentUser;
            board.CreateSubKey(@"Software\WinMineU");
            board = board.OpenSubKey(@"Software\WinMineU", true);
            string puzs = optionsDialog._cols.ToString() + "|" + optionsDialog._rows.ToString() + "|" + optionsDialog._mines.ToString() + "|" + label1.Text + "|";
            string puzh = "";
            foreach (string a in _Sgrid)
            {
                puzs = puzs + a + "|";
            }
            foreach (string a in _Hgrid)
            {
                puzh = puzh + a + "|";
            }
            board.SetValue("Puzs", puzs);
            board.SetValue("Puzh", puzh);
            board.Close();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey board = Registry.CurrentUser;
            board.CreateSubKey(@"Software\WinMineU");
            board = board.OpenSubKey(@"Software\WinMineU", true);
            string puzs = board.GetValue("Puzs","").ToString();
            string puzh = board.GetValue("Puzh", "").ToString();
            string l1 = "";
            int c = 0, r = 0, m = 0;
            foreach (string a in puzs.Split('|'))
            {
                if (c == 0)
                {
                    c = Convert.ToInt32(a);
                }
                else if (r == 0)
                {
                    r = Convert.ToInt32(a);
                }
                else if (m == 0)
                {
                    m = Convert.ToInt32(a);
                }
                else
                {
                    l1 = a;
                    break;
                }
            }
            optionsDialog._cols = c;
            optionsDialog._mines = m;
            optionsDialog._rows = r;
            label1.Text = l1;
            if (label1.Text.EndsWith("z"))
            {
                _z = false;
            }
            else
            {
                _z = true;
            }
            _Sgrid = new string[r, c];
            _Hgrid = new string[r, c];
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    _Sgrid[i, j] = "|";
                    _Hgrid[i, j] = "|";
                }
            }
            int count = 0;
            foreach (string a in puzs.Split('|'))
            {
                if (count < 4)
                {
                    count++;
                }
                else
                {
                    for (int i = 0; i < r; i++)
                    {
                        int j;
                        for (j = 0; j < c; j++)
                        {
                            if (_Sgrid[i, j] == "|")
                            {
                                _Sgrid[i, j] = a;
                                break;
                            }
                        }
                        if (j != c)
                        {
                            break;
                        }
                    }
                }
            }
            foreach (string a in puzh.Split('|'))
            {
                for (int i = 0; i < r; i++)
                {
                    int j;
                    for (j = 0; j < c; j++)
                    {
                        if (_Hgrid[i, j] == "|")
                        {
                            _Hgrid[i, j] = a;
                            break;
                        }
                    }
                    if (j != c)
                    {
                        break;
                    }
                }
            }
            board.Close();
            InitializeBoardArray();
            Invalidate();
            _timing = false;
            _newBoard = false;
            _play = true;
            _Tmin = 0;
            _Tsec = 0;
            timeToolStripMenuItem1.Text = "0:00";
        }
        private void getGrid()
        {
            if (xx == -1 && yy == -1)
            {
                for (xx = 0; xx < _desktop.pic.Width; xx++)
                {
                    for (yy = 0; yy < _desktop.pic.Height; yy++)
                    {
                        if (_desktop.pic.GetPixel(xx, yy).Name == "ff747d8e")
                        {
                            xx++;
                            yy++;                            
                            goto a;
                        }
                    }
                }
                label1.Text = "";
                MessageBox.Show("Can't Find Board");
                return;
            }
        a:
            for (int r = 0; r < optionsDialog._rows; r++)
            {
                for (int c = 0; c < optionsDialog._cols; c++)
                {
                    if (_Sgrid[r, c].StartsWith("e") || _Sgrid[r,c] == "l")
                    {
                        int x = xx + c * 18;
                        int y = yy + r * 18;
                        if (_desktop.pic.GetPixel(x + 9, y + 4).Name == "ff3b4fb9")
                        {
                            _Sgrid[r, c] = "1";
                        }
                        else if (_desktop.pic.GetPixel(x + 9, y + 3).Name == "ff1c6901")
                        {
                            _Sgrid[r, c] = "2";
                        }
                        else if (_desktop.pic.GetPixel(x + 8, y + 4).Name == "ffaf0409")
                        {
                            _Sgrid[r, c] = "3";
                        }
                        else if (_desktop.pic.GetPixel(x + 11, y + 3).Name == "ff030380")
                        {
                            _Sgrid[r, c] = "4";
                        }
                        else if (_desktop.pic.GetPixel(x + 7, y + 3).Name == "ff7a0000")
                        {
                            _Sgrid[r, c] = "5";
                        }
                        else if (_desktop.pic.GetPixel(x + 11, y + 3).Name == "ff067d7f")
                        {
                            _Sgrid[r, c] = "6";
                        }
                        else if (_desktop.pic.GetPixel(x + 6, y + 3).Name == "ffa70603")
                        {
                            _Sgrid[r, c] = "7";
                        }
                        else if (_desktop.pic.GetPixel(x + 6, y+ 6).Name == "ffab201d")
                        {
                            _Sgrid[r, c] = "8";
                        }
                        else if (_desktop.pic.GetPixel(x + 1, y + 1).R < 10 && _desktop.pic.GetPixel(x + 1, y + 1).G < 10 && _desktop.pic.GetPixel(x + 1, y + 1).B < 10)
                        {
                            _Sgrid[r, c] = "0";
                        }
                    }
                }
            }
            Invalidate();
        }
    }
}
