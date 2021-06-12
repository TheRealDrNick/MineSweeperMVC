using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MineSweeperMVC.Models;

namespace MineSweeperMVC.Controllers
{
    public class CreateGameController : Controller
    {
        // GET: CreateGameController
        public ActionResult Index()
        {
            return View();
        }


        // POST: CreateGameController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            int width = int.Parse(Request.Form["Width"]);
            int height = int.Parse(Request.Form["Height"]);
            int mines = int.Parse(Request.Form["MineCount"]);
            //var gameBoard = new GameBoard(width, height, mines);
            try
            {
                return RedirectToAction("Index", "GameBoard", new {width = width, height = height, mines = mines });
            }
            catch
            {
                return View();
            }
        }

    }
}
