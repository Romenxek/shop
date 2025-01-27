using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab7
{
    internal class ConsoleLoger : ILogOut
    {
        public async Task LogOut(string message) 
        {
            Console.WriteLine(message);
        }
    }

    internal class FileLoger : ILogOut
    {
        string filePath = AppDomain.CurrentDomain.BaseDirectory;
        
        public FileLoger(string fileName) 
        {
            filePath = Path.Combine(filePath,fileName); 
        }

        public async Task LogOut(string message) 
        {
            using (var sw = new StreamWriter(filePath, append: true)) 
            {
                await Task.Delay(1000);
                await sw.WriteLineAsync(message + "\n");
            }
        }
    }

    internal class Logger<T>
    {
        private ILogOut _logOutput;

        public Logger(ILogOut logOutput)
        {
            _logOutput = logOutput;
        }

        public async Task LogAsync(T message)
        {
            await _logOutput.LogOut($"[{DateTime.Now}] {message}");
        }
    }
}
