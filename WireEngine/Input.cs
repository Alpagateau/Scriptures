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

        [DllImport(cppUtilsDllPath)]
        static extern void dll_getMousePos([MarshalAs(UnmanagedType.Struct)] ref mousePosStruct mp, IntPtr hdl);
        [DllImport(cppUtilsDllPath)]
        static extern IntPtr dll_GetStandartHandle(int kind);

        IntPtr InputHandle;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct mousePosStruct
        {
            public int x;
            public int y;
        }

        public Vector2Int getMousePosition()
        {
            mousePosStruct p = new mousePosStruct();
            dll_getMousePos(ref p, InputHandle);
            return new Vector2Int(p.x, p.y);
        }

        public Input()
        {
            InputHandle = dll_GetStandartHandle(0);
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
