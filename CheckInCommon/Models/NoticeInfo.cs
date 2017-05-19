using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CheckIn.Common.Models
{
    public class NoticeInfo
    {
        [Key]
        public int NoticeID
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
        public string Author
        {
            get;
            set;
        }
        public DateTime Time
        {
            get;
            set;
        }
    }
}
