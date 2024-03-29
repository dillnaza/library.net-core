﻿using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using web.db;
using web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace web.Controllers
{
    public class LibraryController : Controller
    {
        private readonly Database _db;

        public LibraryController(Database db)
        {
            _db = db;
        }

        [HttpPost]

        public IEnumerable<Book> GetBook(int cat)
        {
            var books = from b in _db.Book.Where(b => b.Category == cat)
                        select b;
            return books.ToList();
        }

        private int GetCount(int cat)
        {
            var bookcount = (from c in _db.Book.Where(c => c.Category == cat)
                             select c).Count();
            return bookcount;
        }

        public IActionResult Index(int cat)
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Books = GetBook(cat);
            mymodel.Count = GetCount(cat);
            mymodel.Cat = cat;
            return View(mymodel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book obj)
        {
            if (ModelState.IsValid)
            {
                _db.Book.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Информация занесена в базу данных";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0) { return NotFound(); }
            var book = _db.Book.Find(id);
            if (book == null) { return NotFound(); }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book obj)
        {
            if (ModelState.IsValid)
            {
                _db.Book.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Информация изменена";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0) { return NotFound(); }
            var book = _db.Book.Find(id);
            if (book == null) { return NotFound(); }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Book.Find(id);
            if (obj == null) { return NotFound(); }

            _db.Book.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Информация удалена из базы данных";
            return RedirectToAction("Index");
        }
    }
}
