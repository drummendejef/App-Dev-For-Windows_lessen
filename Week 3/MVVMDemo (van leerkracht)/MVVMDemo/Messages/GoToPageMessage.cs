using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDemo.Messages
{
    public class GoToPageMessage
    {
        public int Page { get; internal set; }
    }
}
