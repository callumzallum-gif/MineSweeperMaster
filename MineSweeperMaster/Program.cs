using System.Data;

namespace MineSweeperMaster
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cell[,] Grid; Board board = new Board();
            bool Check;
            var dimensions = board.ValidateBoard();
            int width = dimensions.width;
            int height = dimensions.height;
            int mineCount = dimensions.mineCount;
            board.InitialiseGrid(width, height);
            StartGame(board, width, height, mineCount);

        }



        static void StartGame(Board board, int width, int height, int mineCount)
        {
                Console.WriteLine("\n Enter game type:\n 1. Play Game \n 2. Solver");
                string inp = Console.ReadLine();
                if (inp == ("1"))
                {
                    board.GetFirstInput(board, mineCount);
                    HumanLoop(board, width, height);
                }
                else if (inp == "2")
                {

                }
                else { Console.WriteLine("Invalid input please enter again, in form '1' or '2' "); }
            
        }

        static void HumanLoop(Board board, int width, int height)
        {

            while (!board.GameOver)
            {
                Console.Clear();
                board.DisplayBoard();
                Console.WriteLine("\nPlease enter move type:\n 1. Reveal Tile \n 2.Flag Tile");
                int choice = board.TryParse();
                if(choice == 1)
                {
                    var (row, col) = board.GetInput();
                    board.RevealTile(row, col);
                    if (board.GameOver) { board.DisplayBoard(); Console.WriteLine("YOU HIT A MINE \n GAME OVER");  break; }
                }
                
            }
        }

        


    }
}
