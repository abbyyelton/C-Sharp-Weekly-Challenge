﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLou.CSharp.Week3.Challenge
{
    public class Meeting : GenericCalendarItemBase
    {
        // Meetings need to be assigned a start date and time, an end date and time, a location, and attendees. You can decide what data you need for attendees.
        public string[] Attendees { get; set; }
    }
}
