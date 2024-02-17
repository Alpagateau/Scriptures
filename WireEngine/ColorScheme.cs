using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireEngine
{
    public struct ColorScheme
    {
        public ConsoleColor backgroundColor;
        public ConsoleColor foregroundColor;

        public ColorScheme(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            this.backgroundColor = backgroundColor;
            this.foregroundColor = foregroundColor;
        }
    }
}
