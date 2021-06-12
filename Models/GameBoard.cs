using MineSweeperMVC.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeperMVC.Models
{
    public class GameBoard : IGameBoard
    {
        [Required(ErrorMessage = "Please enter the game board width!")]
        [Display(Name = "Gameboard width")]
        [RegularExpression("^[0-9]+$",
            ErrorMessage = "The gameboard width has to be an integer!")]
        public int Width { get; set; }
        [Required(ErrorMessage = "Please enter the game board height!")]
        [Display(Name = "Gameboard height")]
        [RegularExpression("^[0-9]+$",
            ErrorMessage = "The gameboard height has to be an integer!")]
        public int Height { get; set; }
        [Required(ErrorMessage = "Please enter the total mine count!")]
        [Display(Name = "Total mines")]
        [RegularExpression("^[0-9]+$",
            ErrorMessage = "The total mines count has to be an integer!")]
        public int MineCount { get; set; }
        public int FlagCount { get; set; }
        public List<Field> Fields { get; set; }
        public GameStatus Status { get; set; }

        
        public void Initialize(int width, int height, int mines)
        {
            Width = width;
            Height = height;
            MineCount = mines;
            FlagCount = 0;
            Fields = CreateGameBoard(width, height);
            Status = GameStatus.InProgress;
            SetMines(mines);
        }

        private List<Field> CreateGameBoard(int width, int height)
        {
            var fields = new List<Field>();
            int id = 1;
            for (int i = 1; i <= width; i++)
            {
                for (int j = 1; j <= height; j++)
                {
                    fields.Add(new Field(id, i, j));
                    id++;
                }
            }
            return fields;
        }

        private void SetMines(int mines)
        {
            for (int i = 0; i < mines; i++)
            {
                var mineSet = true;
                do
                {
                    Random rnd = new Random();
                    var minePosition = rnd.Next(1, Fields.Count);
                    var field = Fields.First(f => f.Id == minePosition);
                    if (field.HasMine)
                    {
                        continue;
                    }
                    else
                    {
                        field.HasMine = true;
                        mineSet = false;
                    }
                } while (mineSet);
                

            }
            foreach (var field in Fields)
            {
                Console.WriteLine($"{field.Id} {field.HasMine}");
            }

        }

        public Field GetFieldById(int fieldId)
        {
            return Fields.SingleOrDefault(field => field.Id == fieldId);
            
        }

        public List<Field> GetAdjacentFields(int fieldId)
        {
            var field = GetFieldById(fieldId);
            var adjacentFields = new List<Field>();
            

            return Fields.Where(f => f.X == field.X - 1 && f.Y == field.Y + 1
                                || f.X == field.X && f.Y == field.Y + 1
                                || f.X == field.X + 1 && f.Y == field.Y + 1
                                || f.X == field.X - 1 && f.Y == field.Y
                                || f.X == field.X + 1 && f.Y == field.Y
                                || f.X == field.X - 1 && f.Y == field.Y - 1
                                || f.X == field.X && f.Y == field.Y - 1
                                || f.X == field.X + 1 && f.Y == field.Y - 1
                                )
                            .ToList();
        }

        public void ChangeGameStatus(GameStatus gameStatus)
        {
            Status = gameStatus;
        }

        public void ChangeFlagCount(int flagCountAdd)
        {
            FlagCount += flagCountAdd;
        }

        public void CheckIfGameIsWon()
        {
            var itemCount = Fields.Where(f => f.IsRevealed
                                        || (f.IsFlagged && f.HasMine)).Count();
            if(itemCount == Fields.Count)
            {
                ChangeGameStatus(GameStatus.Completed);
            }
        }
    }

    public enum GameStatus
    {
        InProgress,
        Failed,
        Completed
    }
}
