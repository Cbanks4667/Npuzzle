using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CbanksAssignment3
{/// <summary>
/// Main Game Form  class inherits from windows forms
/// </summary>
    public partial class PuzzleForm : Form
    {
        int BoardWidth;
        private bool boardEven;
        int RowCount;
        private bool rowEven;
        int RowEmptyStart;
        private bool inversionEven;
        private int sumInversionCount;
        bool isSolved = true;
        bool isSolvable = false;
        
        /// <summary>
        /// Gets and Sets Whether the number of Inversions is Odd or Even
        /// </summary>
        public bool InversionEven
        {
            get
            {
                return inversionEven;
            }

            set
            {
                inversionEven = value;
            }
        }
        /// <summary>
        /// Gets and Sets whether the board width is even or odd
        /// </summary>
        public bool BoardEven
        {
            get
            {
                return boardEven;
            }

            set
            {
                boardEven = value;
            }
        }
        /// <summary>
        /// gets and sets whether the empty tile is on an even row or and odd row counting from the bottom up
        /// </summary>
        public bool RowEven
        {
            get
            {
                return rowEven;
            }

            set
            {
                rowEven = value;
            }
        }
        /// <summary>
        /// gets and sets the sum of all inversions
        /// </summary>
        public int SumInversionCount
        {
            get
            {
                return sumInversionCount;
            }

            set
            {
                sumInversionCount = value;
            }
        }
        /// <summary>
        /// gets and sets whether the puzzle is solvable
        /// </summary>
        public bool IsSolvable
        {
            get
            {
                return isSolvable;
            }

            set
            {
                isSolvable = value;
            }
        }
        /// <summary>
        /// generates an array of random integers
        /// </summary>
        /// <param name="min">The lowest possible random int</param>
        /// <param name="max">The Highest possible random int</param>
        /// <returns>An Array of Random integers</returns>
        private int[] generateUniqueRandom(int min, int max)
        {
            int n = max - min + 1;
            int[] result = new int[n];
            List<int> candidates = new List<int>();
            for (int i = min; i <= max; i++)
            {
                candidates.Add(i);
            }
            Random r = new Random();
            int j = 0;
            while (candidates.Count > 0)
            {
                int index = r.Next(candidates.Count); //r.next(max) returns a random +ve value from 0-(max-1)
                result[j++] = candidates[index];
                candidates.RemoveAt(index);
            }
            return result;

        }
       
        /// <summary>
        /// Contructs the puzzle form class and initializes it
        /// </summary>
        public PuzzleForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// loads the puzzle form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PuzzleForm_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Generates the Tile Game 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            ClearBoard();

            try
            {
                int rows = int.Parse(txtRows.Text);
                int columns = int.Parse(txtColumns.Text);
                RowCount = rows;
                BoardWidth = columns;
                if ((rows >= 2 && rows <= 10) && (columns >=2 && columns <=10))
                {
                    if (BoardWidth % 2 == 0)
                    {
                        BoardEven = true;
                    }
                    else
                    {
                        BoardEven = false;
                    }

                    int[] Board = new int[rows * columns];


                    do
                    {
                        Board = generateUniqueRandom(0, rows * columns - 1);
                        this.isSolvable = CheckSolvable(rows, columns, Board, this);
                    } while (isSolvable == false);



                    CreateBoard(rows, columns, Board); 
                }
                else
                {
                    MessageBox.Show("Your rows and columns must be greater or equal to 2 and less than 10");
                    txtRows.Focus();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("You must have rows and columns entered as numeric");
                txtRows.Focus();
            }

        }
        /// <summary>
        /// Generates all tiles and assigns the locations of all tiles
        /// </summary>
        /// <param name="rows">The number of rows in the board</param>
        /// <param name="columns">The number of columns in the board</param>
        /// <param name="Board">An array of all tile numbers and their locations in the board</param>
        private void CreateBoard(int rows, int columns, int[] Board)
        {
            ClearBoard();
            int leftBound = pnlLeft.Right;
            int rightBound = pnlRight.Left;
            int topBound = pnlLeft.Top;
            int bottomBound = pnlLeft.Bottom;
            int startX = leftBound;
            int startY = topBound;
            int buttonHeight = (bottomBound - topBound) / rows;
            int buttonWidth = (rightBound - leftBound) / columns;
            RowCount = rows;
            BoardWidth = columns;

            Tile[] boardArray = new Tile[rows * columns];
            for (int i = 0; i < Board.Length; i++)
            {

                if (i != 0 && i % columns == 0)
                {
                    RowCount--;
                    startY += buttonHeight;
                    startX = leftBound;
                }

                if (Board[i] != 0)
                {

                    boardArray[i] = new Tile();
                    boardArray[i].Left = startX;
                    boardArray[i].Left = startX;
                    boardArray[i].Top = startY;
                    boardArray[i].Height = buttonHeight;
                    boardArray[i].Width = buttonWidth;
                    boardArray[i].Text = Board[i].ToString();
                    boardArray[i].Click += B_Click;
                    boardArray[i].StartingIndex = i;
                    boardArray[i].CurrentIndex = i;
                    boardArray[i].DestinationIndex = int.Parse(boardArray[i].Text) - 1;
                    this.Controls.Add(boardArray[i]);

                }
                else
                {

                    boardArray[i] = new Tile();
                    boardArray[i].Left = startX;
                    boardArray[i].Top = startY;
                    boardArray[i].Height = buttonHeight;
                    boardArray[i].Width = buttonWidth;
                    boardArray[i].Name = "pnlHole";
                    boardArray[i].BackColor = Color.White;
                    boardArray[i].BringToFront();
                    boardArray[i].IsEmptySpace = true;
                    boardArray[i].StartingIndex = i;
                    boardArray[i].CurrentIndex = i;
                    boardArray[i].DestinationIndex = rows * columns - 1;
                    this.Controls.Add(boardArray[i]);


                }

                startX += buttonWidth;

            }
        }

        /// <summary>
        /// Checks whether the array of random numbers is a solvable tile game
        /// </summary>
        /// <param name="rows">Height of the tile game board in number of rows</param>
        /// <param name="columns">Width of the tile board in number of columns</param>
        /// <param name="Board">Array of Random Numbers</param>
        /// <param name="F">PuzzleForm Class</param>
        /// <returns>Whether the board is solvable</returns>
        private static bool CheckSolvable(int rows, int columns, int[] Board, PuzzleForm F)
        {
            F.RowCount = rows;
            int InversionCounter = 0;
            int[] Inversions = new int[rows * columns];
            bool isSolvable = false;
            for (int i = 0; i < Board.Length; i++)
            {
                if(i != 0 && i % columns == 0)
                {
                    F.RowCount--;
                }

                if (Board[i] == 0)
                {
                    F.RowEmptyStart = F.RowCount;
                }
                for (int j = i + 1; j < Board.Length; j++)
                {
                    
                    if ((Board[i] > Board[j]) && (Board[j] != 0))
                    {
                        InversionCounter++;
                    }
                }
                Inversions[i] = InversionCounter;
                InversionCounter = 0;

            }

            int SumInversions = Inversions.Sum();
            F.SumInversionCount = SumInversions;
            if (SumInversions % 2 == 0)
            {
                F.inversionEven = true;
            }
            else
            {
                F.inversionEven = false;
            }
            if (F.RowEmptyStart % 2 == 0)
            {
                F.RowEven = true;
            }
            else if(F.RowEmptyStart % 2 == 1)
            {
                F.RowEven = false;
            }
            if (F.BoardEven == true && F.RowEven == true && F.InversionEven == false)
            {
                isSolvable = true;
            }
            else if (F.BoardEven == true && F.RowEven == false && F.InversionEven == true)
            {
                isSolvable = true;
            }
            else if (F.BoardEven == false && F.InversionEven == true)
            {
                isSolvable = true;
            }
            else
            {
                isSolvable = false;
            }
            return isSolvable;
        }
        /// <summary>
        /// Click event for all the tiles
        /// Checks Win Condition
        ///
        /// </summary>
        /// <param name="sender">the button that was clicked</param>
        /// <param name="e"></param>
        private void B_Click(object sender, EventArgs e)
        {
            Tile btn = (Tile)sender;
            
            foreach (var ctl in Controls.OfType<Tile>())
            {


                if (ctl.Text == "")
                {
                    if ((btn.Left == ctl.Right) && (btn.Top == ctl.Top))
                    {//move left
                        SwapTiles(btn, ctl);
                    }
                    else if ((btn.Right == ctl.Left) && (btn.Top == ctl.Top))
                    {
                        //  MessageBox.Show("Move Right");
                        SwapTiles(btn, ctl);
                    }
                    else if ((btn.Top == ctl.Bottom) && (btn.Right == ctl.Right))
                    {
                        // MessageBox.Show("Move Up");
                        SwapTiles(btn, ctl);
                    }
                    else if ((btn.Bottom == ctl.Top) && (btn.Right == ctl.Right))
                    {
                        //move down
                        SwapTiles(btn, ctl);
                    }
                    else
                    {
                        
                    }

                }
            }
            isSolved = true;
            foreach (var ctl in Controls.OfType<Tile>())
            {

                if (ctl.CurrentIndex != ctl.DestinationIndex)
                {
                    isSolved = false;
                }

            }
            if (isSolved == true)
            {
                MessageBox.Show("You Win");
                ClearBoard();
                isSolved = true;
            }
        }
        /// <summary>
        /// Swaps the tile selected with the empty square
        /// </summary>
        /// <param name="btn">Tile You Clicked on</param>
        /// <param name="ctl">Empty Tile</param>
        private static void SwapTiles(Tile btn, Tile ctl)
        {

            int TempTileLeft = btn.Left;
            int TempTileRight = btn.Right;
            int TempTileTop = btn.Top;
            int TempTileBottom = btn.Bottom;
            int TempTileHeight = btn.Height;
            int TempTileWidth = btn.Width;
            int TempTileIndex = btn.CurrentIndex;

            int TempEmptyLeft = ctl.Left;
            int TempEmptyRight = ctl.Right;
            int TempEmptyTop = ctl.Top;
            int TempEmptyBottom = ctl.Bottom;
            int TempEmptyHeight = ctl.Height;
            int TempEmptyWidth = ctl.Width;
            int TempEmptyIndex = ctl.CurrentIndex;


            btn.Left = TempEmptyLeft;
            btn.Top = TempEmptyTop;
            btn.Width = TempEmptyWidth;
            btn.Height = TempEmptyHeight;
            btn.CurrentIndex = TempEmptyIndex;
            ctl.Left = TempTileLeft;
            ctl.Top = TempTileTop;
            ctl.Width = TempTileWidth;
            ctl.Height = TempTileHeight;
            ctl.CurrentIndex = TempTileIndex;

        }

        /// <summary>
        /// Clears the board of all tiles
        /// </summary>
        private void ClearBoard()
        {
            foreach (Tile ctl in Controls.OfType<Tile>().ToArray())
            {
                ctl.Dispose();
                Controls.Remove(ctl);
            }
        }
      /// <summary>
      /// Save the file selected
      /// </summary>
      /// <param name="filename">The File name to be saved</param>
        private void doSave(string filename)
        {
            try
            {
                int rows = int.Parse(txtRows.Text);
                int columns = int.Parse(txtColumns.Text);
                int[] Board = new int[rows * columns];
                Board = getBoard();
                //  lstList.Items.Clear();

                //foreach (var item in Board)
                //{
                //    lstList.Items.Add(item);
                //}



                StreamWriter writer = new StreamWriter(filename);

                writer.WriteLine(rows);
                writer.WriteLine(columns);

                for (int i = 0; i < Board.Length; i++)
                {
                    writer.WriteLine(Board[i]);
                }

                writer.Close();
                ;


            }
            catch (Exception)
            {

                MessageBox.Show("Your Rows and columns must be entered correctly");
                txtColumns.Focus();
            }
        }
        /// <summary>
        /// Loads the filename selected and generates the board
        /// </summary>
        /// <param name="filename">The File to be loaded</param>
        private void doLoad(string filename)
        {
           // lstList.Items.Clear();
            StreamReader reader = new StreamReader(filename);
            int rows = int.Parse(reader.ReadLine());
            int columns = int.Parse(reader.ReadLine());

            txtColumns.Text = columns.ToString();
            txtRows.Text = rows.ToString();

            int[] Board = new int[rows * columns];
           
            for (int i = 0; i < Board.Length; i++)
            {
                Board[i] = int.Parse(reader.ReadLine());
               // lstList.Items.Add(Board[i]);
            }
            reader.Close();

            CreateBoard(rows, columns, Board);
        }


        
        /// <summary>
        /// Generates the Save File dialog
        /// </summary>
        /// <param name="sender">button selected</param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int.Parse(txtRows.Text) >= 2 && int.Parse(txtRows.Text) <= 10) && (int.Parse(txtColumns.Text)>= 2 && int.Parse(txtColumns.Text) <= 10))
                {
                    DialogResult result = dlgSave.ShowDialog();
                    switch (result)
                    {
                        case DialogResult.Abort:
                            break;
                        case DialogResult.Cancel:
                            break;
                        case DialogResult.Ignore:
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.None:
                            break;
                        case DialogResult.OK:
                            try
                            {
                                string filename = dlgSave.FileName;
                                doSave(filename);
                            }
                            catch (Exception)
                            {

                                MessageBox.Show("Error in file save");
                            }

                            break;
                        case DialogResult.Retry:
                            break;
                        case DialogResult.Yes:
                            break;
                        default:
                            break;
                    } 
                }
                else
                {
                    MessageBox.Show("You Must have the size of the board correctly entered as numeric");
                    txtColumns.Focus();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Generates the Load file dialog
        /// </summary>
        /// <param name="sender">button selected</param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpen.ShowDialog();
            switch (result)
            {
                case DialogResult.Abort:
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    try
                    {
                        string filename = dlgOpen.FileName;
                        doLoad(filename);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error in file load");
                    }
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Yes:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Gets the current status of the board in play
        /// </summary>
        /// <returns>An array of tiles current locations</returns>
        private int[] getBoard()
        {
            int[] Board = new int[int.Parse(txtColumns.Text) * int.Parse(txtRows.Text)];
            
            foreach (Tile ctl in Controls.OfType<Tile>())
            {
                if (ctl.Text != "")
                {
                    Board[ctl.CurrentIndex] = int.Parse(ctl.Text);

                }
                else
                {
                    Board[ctl.CurrentIndex] = 0;
                }
                
            }
            foreach (var item in Board)
            {
                Console.WriteLine(item);
            }

            return Board;
        }
    }
}
