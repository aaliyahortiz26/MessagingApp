using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApp.Models
{
    public class Preferences : DBObject
    {
        public string TxtColor { get; set; }

        public string Baccolor { get; set; }
    }
}