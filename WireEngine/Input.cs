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
        [DllImport(cppUtilsDllPath)]
        static extern void dll_getPressedKeys(IntPtr p, IntPtr hdl);
        [DllImport(cppUtilsDllPath)]
        static extern void dll_getKeyboardAndMouse(IntPtr hdl, [MarshalAs(UnmanagedType.Struct)] ref mousePosStruct mp, IntPtr kpi);
        [DllImport(cppUtilsDllPath)]
        static extern void dll_setConsoleReadable(IntPtr hdl);
        [DllImport(cppUtilsDllPath)]
        static extern void dll_getOldConsoleMode(IntPtr hdl, ref int fdwSaveOldMode);
        [DllImport(cppUtilsDllPath)]
        private static extern void dll_resetConsoleMode(IntPtr hdl, int fdwOldMode);

        IntPtr InputHandle;

        int oldConsoleMode;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct mousePosStruct
        {
            public int x;
            public int y;
        }

        byte[] pressedKeys;
        mousePosStruct mps;

        public Vector2Int getMousePosition()
        {
            mousePosStruct p = new mousePosStruct();
            dll_getMousePos(ref p, InputHandle);
            return new Vector2Int(p.x, p.y);
        }

        void updatePressedKeys()
        {
            pressedKeys = new byte[32];
            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)) * 32);
            Marshal.Copy(pressedKeys, 0, p, pressedKeys.Length);
            dll_getPressedKeys(p, InputHandle);
            Marshal.Copy(p, pressedKeys, 0, 32);
            Marshal.FreeHGlobal(p);
        }

        public Input()
        {
            InputHandle = dll_GetStandartHandle(0);
            pressedKeys = new byte[32];
            mps = new mousePosStruct();
            dll_getOldConsoleMode(InputHandle, ref oldConsoleMode);
            dll_setConsoleReadable(InputHandle);
        }

        public void __updateInput(object? o, EventArgs e)
        {
            pressedKeys = new byte[32];
            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)) * 32);
            Marshal.Copy(pressedKeys, 0, p, pressedKeys.Length);
            dll_getKeyboardAndMouse(InputHandle, ref mps, p);
            Marshal.Copy(p, pressedKeys, 0, 32);
            Marshal.FreeHGlobal(p);
        }

        public bool GetKeyDown(KeyCode key)
        {
            int ui = (int)key;
            int bIndex = ui % 8;
            int byteIndex = (ui - bIndex) / 8;

            byte r = pressedKeys[byteIndex];
            byte mask = (byte)(0x01 << bIndex);
            r &= mask;
            return !(r == 0x00);
        }

        ~Input()
        {
            dll_resetConsoleMode(InputHandle, oldConsoleMode);
        }

        //Old code, works without the real stuff (no cpp lib)
        //public delegate void KeyboardInputHandler(InputKeys e);
        //public event KeyboardInputHandler KeyboardInput;

        //Keyboard inputs
        /*
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
        */
    }
}
