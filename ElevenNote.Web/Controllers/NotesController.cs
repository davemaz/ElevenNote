using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElevenNote.Models;

namespace ElevenNote.Web.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly Lazy<NoteService> _svc;

        public NotesController()
        {
            _svc =
                new Lazy<NoteService>(
                        () =>
                        {
                            var userId = Guid.Parse(User.Identity.GetUserId());
                            return new NoteService(userId);
                        }
                    );
        }

        //// GET: Notes
        //public ActionResult Index()
        //{

        //    var notes = _svc.Value.GetNotes();

        //    return View(notes);
        //}

        public ActionResult Index(string search)
        {
            var notes = _svc.Value.GetNotes();

            if(!String.IsNullOrEmpty(search))
            {
                notes = notes.Where(s => s.Content.ToLower().Contains(search.ToLower()) ||
                    s.Title.ToLower().Contains(search.ToLower()));

                ViewBag.Search = search;
            }
            else
            {
                ViewBag.Search = String.Empty;
            }

            return View(notes);
        }

        public ActionResult IsStarred()
        {
            var notes = _svc.Value.GetNotes();
            notes = notes.Where(s => s.IsStarred == true);
            return View(notes);
        }

        public ActionResult IsNotStarred()
        {
            var notes = _svc.Value.GetNotes();
            notes = notes.Where(s => s.IsStarred == false);
            return View(notes);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreateModel model)
        {
            if (!ModelState.IsValid) return View(model); // repopulates form with current data

            if (!_svc.Value.CreateNote(model))
            {
                ModelState.AddModelError("", "Unable to create note");
                return View(model);
            }

            TempData["SaveResult"] = "Your note was created";

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var note = _svc.Value.GetNoteById(id);

            return View(note);
        }

        public ActionResult Edit(int id)
        {
            var note = _svc.Value.GetNoteById(id);
            var model =
                new NoteEditModel
                {
                    NoteId = note.NoteId,
                    Title = note.Title,
                    Content = note.Content,
                    IsStarred = note.IsStarred
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoteEditModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!_svc.Value.UpdateNote(model))
            {
                ModelState.AddModelError("", "Unable to update note");
                return View(model);
            }

            TempData["SaveResult"] = "Your note was saved";

            return RedirectToAction("Index");
        }

        [ActionName("Delete")]
        public ActionResult DeleteGet(int id)
        {
            var detail = _svc.Value.GetNoteById(id);

            return View(detail);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            _svc.Value.DeleteNote(id);
            TempData["SaveResult"] = "Your note was deleted";
            return RedirectToAction("Index");
        }
    }
}