using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.Web.Controllers.WebApi
{
    [Authorize]
    [RoutePrefix("api/Notes")]
    public class NotesController : ApiController
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

        [Route]
        public IEnumerable<NoteListItemModel> Get()
        {
            return _svc.Value.GetNotes();
        }

        [Route("{id}")]
        public NoteDetailModel Get(int id)
        {
            return _svc.Value.GetNoteById(id);
        }

        private bool SetStarState(int noteId, bool state)
        {
            var detail =
                _svc.Value.GetNoteById(noteId);
            var note =
                new NoteEditModel
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content,
                    IsStarred = state
                };

            return _svc.Value.UpdateNote(note);
        }

        [Route("{id}/Star")]
        [HttpPost]
        public bool ToggleStarOn(int id)
        {
            return SetStarState(id, true);
        }

        [Route("{id}/Star")]
        [HttpDelete]
        public bool ToggleStarOff(int id)
        {
            return SetStarState(id, false);
        }
    }
}
