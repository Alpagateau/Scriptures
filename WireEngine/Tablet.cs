using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace WireEngine
{
    public enum TabletRenderType
    {
        Direct,
        Bufferd
    }

    public class Tablet
    {
        public Transform2D transform;
        TabletRenderType renderType;

        public Tablet()
        {
            transform = new Transform2D();
            renderType = TabletRenderType.Direct;
        }

        public void setRenderType(TabletRenderType t)
        {
            this.renderType = t;
        }

        public Tablet(Vector2Int position, Vector2Int dimensions)
        {
            transform = new Transform2D();
            transform.position = position;
            transform.scale = dimensions;
        }

        public Tablet(int x, int y, int w, int h)
        {
            transform = new Transform2D();
            transform.position.x = x;
            transform.position.y = y;
            transform.scale.x = w;
            transform.scale.y = h;
        }

        public void setCursorPosition(int x, int y)
        {
            if (x < 0 || y < 0)
                return;
            if (x > transform.scale.x-1 || y > transform.scale.y-1)
                return;
            Console.SetCursorPosition(x + transform.position.x, y + transform.position.y);
        }

        public void setCursorPosition(Vector2Int v)
        {
            int x = v.x;
            int y = v.y;
            if (x < 0 || y < 0)
                return;
            if (x > transform.scale.x || y > transform.scale.y)
                return;
            Console.SetCursorPosition(x + transform.position.x, y + transform.position.y);
        }

        public void fillWith(char c)
        {
            for(int i = 0; i < transform.scale.x; i++)
            {
                for(int j = 0; j < transform.scale.y; j++)
                {
                    setCursorPosition(i, j);
                    Console.Write(c);
                }
            }
        }

        public void Write(string txt)
        {
            for(int i = 0; i < txt.Length; i++)
            {
                setCursorPosition(i%transform.scale.x,i/transform.scale.x);
                if (i / transform.scale.x > transform.scale.y)
                    return;
                Console.Write(txt[i]);
            }
        }
        public void WriteLine(string txt)
        {
            for (int i = 0; i < txt.Length; i++)
            {
                setCursorPosition(i % transform.scale.x, i / transform.scale.x);
                if (i / transform.scale.x > transform.scale.y)
                    return;
                Console.Write(txt[i]);
            }
            Console.Write("\n");
        }
    }
}
