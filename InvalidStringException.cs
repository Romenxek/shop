using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    internal class InvalidStringException : Exception
    {
        public InvalidStringException(string message) : base(message) { }

        internal Program Program
        {
            get => default;
            set
            {
            }
        }
    }
}
