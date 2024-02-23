using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WireEngine
{
    public class InputKeys : EventArgs
    {
        public ConsoleKey key;
        public char keyChar;
    }

    public class Input
    {

        const string cppUtilsDllPath = "F:\\code\\wiremole\\Scriptures\\x64\\Debug\\ConsoleCppUtils.dll";
        //cpp lib
        [DllImport(cppUtilsDllPath)]
        private static extern IntPtr GetStandartHandle(int kind);

        [DllImport(cppUtilsDllPath)]
        private static extern int TestWithInputs();

        public void testAgain()
        {
            TestWithInputs();
        }

        //Old code, works without the real stuff (no cpp lib)
        public delegate void KeyboardInputHandler(InputKeys e);
        public event KeyboardInputHandler KeyboardInput;

        //Keyboard inputs
        public void CheckForInputs(object? o, EventArgs e)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                InputKeys ik = new InputKeys();
                ik.keyChar = key.KeyChar;
                ik.key = key.Key;
                OnKeyboardInput(ik);
            }
        }

        public bool GetKeyPressed(ConsoleKey key)
        {
            if(Console.KeyAvailable)
            {
                ConsoleKeyInfo k = Console.ReadKey(true);
                return k.Key == key;
            }
            return false;
        }

        public void OnKeyboardInput(InputKeys ik)
        {
            if(KeyboardInput != null)
            {
                KeyboardInput.Invoke(ik);
            }
        }
    }
}
