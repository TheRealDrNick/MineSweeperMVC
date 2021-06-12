using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeperMVC.Models
{
    public class Field
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasMine { get; set; }
        public int AdjacentMines { get; set; } = 0;
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }

        public Field(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }
}
