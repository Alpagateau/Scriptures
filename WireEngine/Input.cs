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
        public delegate void KeyboardInputHandler(InputKeys e);
        public event KeyboardInputHandler KeyboardInput;

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Creator / input init
        public Input()
        {
        }

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
