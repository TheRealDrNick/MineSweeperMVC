using MineSweeperMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeperMVC.Services
{
    public interface IGameBoard
    {
        public Field GetFieldById(int fieldId);
        public void Initialize(int width, int height, int mines);
        public List<Field> GetAdjacentFields(int fieldId);
        public void ChangeGameStatus(GameStatus gameStatus);
        public void ChangeFlagCount(int flagCountAdd);
        public void CheckIfGameIsWon();
    }
}
