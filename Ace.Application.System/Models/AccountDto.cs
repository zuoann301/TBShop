using Ace.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.System
{
    public class ModifyAccountInfoInput : ValidationModel
    {
        public string UserId { get; set; }
        public string AccountName { get; set; }
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? Birthday { get; set; }

        public string WeChat { get; set; }
        public string Description { get; set; }
    }
}
