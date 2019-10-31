using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application
{
    public interface IAceSessionAppService
    {
        IAceSession AceSession { get; set; }
    }
}
