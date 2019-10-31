using Ace.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application
{
    public interface IAppServiceTest : IAppService
    {
        string Console(string s);
    }
}
