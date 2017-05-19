using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Manager.Models
{
    public class NoticeModel
    {
        public int NoticeID
        {
            get;
            set;
        }
        public string Author
        {
            get;
            set;
        }
        [Required(ErrorMessage = "标题不能为空。")]
        public string Title
        {
            get;
            set;
        }
        [Required(ErrorMessage = "内容不能为空。")]
        public string Content
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
