using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageLibrary
{
    public class Message
    {
        public string Source { get; set; }

        public string Target { get; set; }

        public string Operation { get; set; }

        public string Stamp { get; set; }

        public List<string> ListParams { get; set; }

        public Message(string source, string target, string operation,
            string stamp, List<string> listParams)
        {
            Source = source;
            Target = target;
            Operation =operation;
            Stamp = stamp;
            ListParams = listParams;
        }
    }
}
