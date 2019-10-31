using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Entity.System
{
    [Table("Users_Log")]
    public class Users_Log
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public LogType Type { get; set; }
        public string IP { get; set; }
        public string IPAddress { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool? Result { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
