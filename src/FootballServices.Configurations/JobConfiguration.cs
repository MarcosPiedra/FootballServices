using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServices.Configurations
{
    public class JobConfiguration
    {
        public int MinutesBetweenExecution { get; set; }
        public string IncorrectAligmentEndPoint { get; set; }
        public int MinutesBeforeStartMatch { get; set; }
    }
}
