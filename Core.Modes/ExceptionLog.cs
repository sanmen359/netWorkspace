using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Modes
{
    public class ExceptionLog:IEnitity<int>
    {
        public int Id { get; set;}

        public DateTime Time { get; set; }

        public string Level { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; } }
    
}
