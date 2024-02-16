using System;
using System.Runtime.InteropServices;

namespace WireEngine
{
    public class GameWindow
    {
        const string cppUtilsDllPath = "D:\\Documents\\GitHub\\Scriptures\\x64\\Debug\\ConsoleCppUtils.dll";

        private int WINDOW_HEIGHT;
        private int WINDOW_WIDTH;

        public bool isGameRunning;
        public bool isSizeLocked = true;

        public string ApplicationName = "New Scripture Application";

        public List<Tablet> tablets;

        public delegate void UpdatingEventHandler(object? o, EventArgs e);
        public event UpdatingEventHandler Updating;

        public Input inputSystem;

        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;//resize

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(cppUtilsDllPath)]
        private static extern int SetConsoleFontSize(int a);

        private enum StdHandle
        {
            OutputHandle = -11
        }

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(StdHandle index);

        

        public GameWindow()
        {

            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            WINDOW_HEIGHT = 20;
            WINDOW_WIDTH = 20;
            isGameRunning = false;
            tablets = new List<Tablet>();
            inputSystem = new Input();

            if(isSizeLocked)
            {
                if (handle != IntPtr.Zero)
                {
                    //DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                    DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                    DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                    DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);//resize
                }
            }
        }

        public void Start()
        {
            Console.CursorVisible = false;
            isGameRunning = true;
            Console.Title = ApplicationName;
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            Updating += inputSystem.CheckForInputs;
            while(isGameRunning)
            {
                GameUpdate();
            }
        }

        void GameUpdate()
        {
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            OnUpdating();
        }

        public GameWindow SetWindowSize(int w, int h)
        {
            WINDOW_HEIGHT = h;
            WINDOW_WIDTH = w;
            return this;
        }

        public GameWindow SetCharSize(int w)
        {
            int b = SetConsoleFontSize(w);
            return this;
        }

        public void Stop()
        {
            isGameRunning = false;
        }

        public bool canTabletFit(Tablet t)
        {
            //Window checks
            if(t.transform.scale.x > WINDOW_WIDTH)
                return false;
            if(t.transform.scale.y > WINDOW_HEIGHT)
                return false;
            if(t.transform.position.x >= WINDOW_WIDTH)
                return false;
            if(t.transform.position.y >= WINDOW_HEIGHT)
                return false;
            if (t.transform.position.x + t.transform.scale.x > WINDOW_WIDTH)
                return false;
            if (t.transform.position.y + t.transform.scale.y > WINDOW_HEIGHT)
                return false;

            Vector2Int tr = t.transform.position;
            Vector2Int tl = t.transform.position;
            tl.x += t.transform.scale.x;
            Vector2Int dl = t.transform.position + t.transform.scale;
            Vector2Int dr = t.transform.position + t.transform.scale;
            dr.x -= t.transform.scale.x;
            for (int i = 0; i < tablets.Count; i++)
            {
                if (Math.pointInRect(tr, tablets[i].transform.position, tablets[i].transform.scale))
                    return false;
                if (Math.pointInRect(tl, tablets[i].transform.position, tablets[i].transform.scale))
                    return false;
                if (Math.pointInRect(dr, tablets[i].transform.position, tablets[i].transform.scale))
                    return false;
                if (Math.pointInRect(dl, tablets[i].transform.position, tablets[i].transform.scale))
                    return false;
            }
            return true;
        }

        public bool addTablet(Tablet nTablet)
        {
            if(canTabletFit(nTablet))
            {
                tablets.Add(nTablet);
            }
            else
            {
                return false;
            }
            return true;
        }

        public void OnUpdating()
        {
            if(Updating != null)
            {
                Updating.Invoke(this, EventArgs.Empty);
            }
        }
    }
}