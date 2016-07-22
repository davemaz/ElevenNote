using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteDetailModel
    {
        [Display(Name = "Id")]
        public int NoteId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        [UIHint("Starred")]
        public bool IsStarred { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }

        [Display(Name = "Modified")]
        public DateTimeOffset? ModifiedUtc { get; set; }

        public override string ToString()
        {
            return $"[{NoteId}] {Title}";
        }
    }
}
