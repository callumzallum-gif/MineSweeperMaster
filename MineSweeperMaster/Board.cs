using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperMaster
{
    internal class Board
    {
        private int x {  get; set; }
        private int y { get; set; }
        private int MineCount { get; set; }
        private int Width => Grid.GetLength(0);
        private int Height => Grid.GetLength(1);

        private Cell[,] Grid;

        public bool GameOver;

        Random random = new Random();
        


        public void GetFirstInput(Board board, int mineCount)
        {
            board.DisplayBoard();
            Console.WriteLine("Please enter first move ( row first then column )");
            int safeR = TryParse();
            int safeC = TryParse();
            Grid[safeC-1, safeR - 1].Reveal();
            board.PlaceMines(mineCount, safeR - 1, safeC - 1);
            CalculateAllAdjacent();
        }


        public int TryParse()
        {
            bool tryParse;
            int input;
            do
            {
                tryParse = int.TryParse(Console.ReadLine(), out input);
                if (!tryParse) { Console.WriteLine("Please enter again"); }
            } while (!tryParse);
            return input;
                
        }

        public (int row, int col) GetInput()
        {
            Console.Write("Please enter row \n");
            int row = TryParse();
            Console.Write("Please enter column \n");
            int col = TryParse();
            return (row - 1, col - 1);
        }

        public void RevealTile(int row, int col)
        {
            Cell cell = Grid[col, row];
            if (cell.IsFlagged) { cell.Unflag(); return; }
            else if(cell.IsRevealed) { return; }
            bool mine = cell.Reveal();
            if (mine) { GameOver = true; }
            else if(cell.AdjacentMines == 0) { RecursiveFloodFill(cell.Row , cell.Column); }
        }

        public (int width , int height, int mineCount) ValidateBoard()
        {
            bool validate;
            int x; int y; int mineCount; bool correctMineCount = false;
            do
            {   // Validate input by checking all of the requirements
                Console.WriteLine("Please enter grid dimensions ( x followed by y) - maximium size 50x50 , minimium size 3x3");
                bool TryX = int.TryParse(Console.ReadLine(), out x);bool TryY = int.TryParse(Console.ReadLine(), out y);
                int TotalCells = x * y;int MinTotalMines = (int)Math.Ceiling(TotalCells * 0.1);int MaxTotalMines = (int)(TotalCells * 0.8);
                Console.WriteLine($"Please enter mine count (maximium 80% of total tiles {MaxTotalMines}, minimum 10% {MinTotalMines})");
                bool TryMineCount = int.TryParse(Console.ReadLine(), out mineCount);
                if(mineCount >= MinTotalMines && mineCount <= MaxTotalMines) { correctMineCount = true; }
                else { correctMineCount = false; }
                    validate = TryMineCount && TryX && TryY && ValidateBoardSize(x, y) && correctMineCount;
                if (!validate) { Console.WriteLine("Wrong input please try again"); }
            } while (!validate);
              return (x  , y , mineCount);
        }

        static bool ValidateBoardSize(int x, int y)
        {
            // Check input is in correct range
            if (x < 3 || y < 3 || x > 50 || y > 50) { return false; }
            return true;
        }

        public void InitialiseGrid(int width, int height)
        {
            // Create a new cell for each tile,  a collection of tiles created makes the grid
            Grid = new Cell[width , height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Grid[x,y] = new Cell(y, x); 
                }
            }
        }

        static void WriteColoured(string text, ConsoleColor color)
        {
            // Function to change output colour, allows a more user freindly output with higher contrast 
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public void DisplayBoard()
        {
            // Prints board, uses limit set by user and checks on the state of that tile to distinguish what to print
            Console.Write("    ");
            for (int i = 1; i <= Width; i++) {WriteColoured($"({i , 2})" , ConsoleColor.Cyan); }
            Console.WriteLine();
            for(int j = 0; j < Height; j++)
            {
                WriteColoured($"({j+1, 2})" , ConsoleColor.Cyan);
                for (int k = 0; k < Width; k++)
                {

                    if (Grid[k, j].IsRevealed)
                    {
                        if (Grid[k, j].IsMine) { WriteColoured($"[ X]", ConsoleColor.Red); }
                        else { WriteColoured($"[ {Grid[k, j].AdjacentMines}]", ConsoleColor.Magenta); }
                    }
                    else { WriteColoured($"[  ]", ConsoleColor.DarkGray);} 

                    
                }
                Console.WriteLine();
                
            }
            
        }
        
        public void PlaceMines(int mineCount, int safeR, int safeC)
        {
            int placed = 0;
            while(placed < mineCount)
            {
                int r = random.Next(0, Height);
                int c = random.Next(0, Width);
                Cell cell = Grid[c, r];
                if(cell.IsMine || (r == safeR && c == safeC)) { continue; }
                cell.PlaceMine();
                placed++;
            }
        }


        private int AdjacentMineCount(int row , int column)
        {
            int MineCount = 0;
            for(int i = -1 ;i <= 1; i++)
            {
                for (int j = -1 ;j <= 1; j++)
                {
                    if(i  == j && j == 0) {  continue; }
                    else if(i+column < 0 || i+column >= Width || j + row < 0 || j + row >= Height) { continue; }
                    else
                    {
                        if (Grid[column + i, row + j].IsMine) { MineCount++; }
                    }
                }
            }
            return MineCount;
        }
        private void RecursiveFloodFill(int row, int column)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if(i == j && i ==0 ){ continue; }
                    if(column + i < 0 || row + j < 0 || j + row >= Height || i + column >= Width) { continue; }
                    if(Grid[column + i, row + j].IsMine || Grid[column + i, row + j].IsRevealed) { continue;}
                    if (Grid[column + i, row + j].AdjacentMines == 0)
                    {
                        Grid[column + i, row + j].Reveal(); Console.Clear(); DisplayBoard(); Thread.Sleep(50);
                        RecursiveFloodFill(row + j, column + i);
                    }
                    else { Grid[column + i, row + j].Reveal(); Console.Clear(); DisplayBoard();  Thread.Sleep(50); }
                }
            }
        }

        private void CalculateAllAdjacent()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (!Grid[col, row].IsMine)
                    {
                        Grid[col, row].AdjacentMines = AdjacentMineCount(row, col);
                    }
                }
            }
        }
    }
}
