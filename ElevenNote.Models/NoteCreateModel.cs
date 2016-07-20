using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    class NoteCreateModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public override string ToString()
        {
            return $"[New] {Title}";
        }
    }
}
