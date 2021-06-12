using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MineSweeperMVC.Models;
using MineSweeperMVC.Services;

namespace MineSweeperMVC.Controllers
{
    public class GameBoardController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameBoard _service;


        public GameBoardController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _service = (IGameBoard)_serviceProvider.GetService(typeof(IGameBoard));

        }
        public IActionResult Index(int width, int height, int mines)
        {
            _service.Initialize(width, height, mines);
                        
            return View(_service);
        }

        public IActionResult RefreshField()
        {
            _service.CheckIfGameIsWon();
            return Json(_service);
        }
                
        public IActionResult LeftMouseClick(int fieldId)
        {
            var field = _service.GetFieldById(fieldId);
            
            if (field.HasMine)
            {
                _service.ChangeGameStatus(GameStatus.Failed);
            }
            else if (!field.IsRevealed && !field.IsFlagged)
            {
                field.IsRevealed = true;
                CheckAllNeighborFields(field);
            }
            else if (field.IsFlagged)
            {
                field.IsFlagged = false;
            }
            return RedirectToAction("RefreshField");
        }

        public IActionResult RightMouseClick(int fieldId)
        {
            var field = _service.GetFieldById(fieldId);
            if (field.IsFlagged)
            {
                field.IsFlagged = false;
                if (field.HasMine)
                {
                    _service.ChangeFlagCount(-1);
                }
                    
            }
            else
            {
                field.IsFlagged = true;
                if (field.HasMine)
                {
                    _service.ChangeFlagCount(1);
                }
            }
            
            return RedirectToAction("RefreshField");
        }

        private IActionResult CheckAllNeighborFields(Field field)
        {
            
            var adjacentFields = new List<Field>();
            
            adjacentFields = _service.GetAdjacentFields(field.Id);

            if (adjacentFields.FindAll(f => f.HasMine == true).Count > 0)
            {
                field.AdjacentMines = adjacentFields.FindAll(f => f.HasMine == true).Count;
            }
            else
            {
                field.AdjacentMines = 0;
                IEnumerable<Field> query = adjacentFields.Where(f => f.IsFlagged == false && f.IsRevealed == false);
                foreach (var adjacentField in query)
                {
                    LeftMouseClick(adjacentField.Id);
                }

            };
            
            return RedirectToAction("RefreshField");
        }
        
    }
}

