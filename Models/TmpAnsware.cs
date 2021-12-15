using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models
{
    public class TmpAnsware
    {
        public TmpAnsware()
        {

        }

        public TmpAnsware(string answareText, bool wasChecked)
        {
            AnswareText = answareText;
            WasChecked = wasChecked;
        }

        public TmpAnsware(long iD, string answareText, bool wasChecked)
        {
            ID = iD;
            AnswareText = answareText;
            WasChecked = wasChecked;
        }

        public TmpAnsware(string answareText, bool wasChecked, long attemptID)
        {
            AnswareText = answareText;
            WasChecked = wasChecked;
            AttemptID = attemptID;
        }


        public TmpAnsware(long iD, string answareText, bool wasChecked, long answareID)
        {
            ID = iD;
            AnswareText = answareText;
            WasChecked = wasChecked;
            AnswareID = answareID;
        }

        public TmpAnsware(string answareText, bool wasChecked, long attemptID, long answareID)
        {
            AnswareText = answareText;
            WasChecked = wasChecked;
            AttemptID = attemptID;
            AnswareID = answareID;
        }
        [Key]
        public long ID { get; set; }
        public string AnswareText { get; set; }
        public bool WasChecked { get; set; }

        [ForeignKey("Answare")]
        public long AnswareID { get; set; }

        [ForeignKey("Attempt")]
        public long AttemptID { get; set; }
    }
}
