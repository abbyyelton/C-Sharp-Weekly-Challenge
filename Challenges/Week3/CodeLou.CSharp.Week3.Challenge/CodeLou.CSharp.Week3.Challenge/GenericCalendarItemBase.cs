using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLou.CSharp.Week3.Challenge
{
    public abstract class GenericCalendarItemBase : CalendarItemBase
    {
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
    }
}
