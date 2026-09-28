using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperMaster
{
    internal class Cell
    {
        public Cell(int row, int column) 
        {
            Row = row;
            Column = column;

            IsFlagged = false;
            IsMine = false;
            IsRevealed = false;
            AdjacentMines = 0;

        }

        public int Row {get;} public int Column {get;}
        public int AdjacentMines { get; set;}
        public bool IsMine { get; private set; }
        public bool IsRevealed { get; private set; }
        public bool IsFlagged { get; private set; }


        public void PlaceMine()
        {
            IsMine = true;
        }

        public bool Reveal()
        {
            IsRevealed = true;
            if(IsMine) {return true;}
            else {return false;}
        }

        public void PlaceFlag()
        {
            IsFlagged = true;
        }
        public void Unflag()
        {
            IsFlagged = false;
        }

    }
}
