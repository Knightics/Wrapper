using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            TripAdvisorWrapper wrapper = new TripAdvisorWrapper();
            wrapper.Execute();
            Console.Read();
        }
    }
}
