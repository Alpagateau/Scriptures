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
        Buffered
    }

    public enum wrapParameter
    {
        warning,
        block,
        wrap
    }

    public class Tablet
    {
        public Transform2D transform;
        TabletRenderType renderType;

        public ConsoleColor stdforeground;

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


        public void setCursorPosition(int x, int y) => setCursorPosition(x, y, wrapParameter.wrap);
        public void setCursorPosition(int x, int y, wrapParameter parameter)
        {
            if (x < 0 || y < 0)
            {
                switch (parameter)
                {
                    case wrapParameter.warning:
                        throw new Exception("Cannot set position below zero : " + x + "x " + y + "y");
                        return;
                        break;
                    case wrapParameter.block:
                        if (x < 0)
                            x = 0;
                        if (y < 0)
                            y = 0;
                        break;
                    case wrapParameter.wrap:
                        if(x < 0)
                            x += (transform.scale.x);
                        if(y < 0)
                            y += (transform.scale.y);
                        break;
                    default:
                        break;
                }
            }
            if (x >= transform.scale.x || y >= transform.scale.y)
            {
                switch (parameter)
                {
                    case wrapParameter.warning:
                        throw new Exception("Cannot set position too high : " + x + "x " + y + "y");
                        return;
                        break;
                    case wrapParameter.block:
                        if (x >= transform.scale.x)
                            x = transform.scale.x;
                        if (y >= transform.scale.y)
                            y = transform.scale.y;
                        break;
                    case wrapParameter.wrap:
                        if(x >= transform.scale.x)
                            x -= (transform.scale.x);
                        if (y >= transform.scale.y)
                            y -= (transform.scale.y);
                        break;
                    default:
                        break;
                }
            }
            Console.SetCursorPosition(x + transform.position.x, y + transform.position.y);
        }

        public void setCursorPosition(Vector2Int v) => setCursorPosition(v.x, v.y);

        public void fillWith(char c)
        {
            for(int i = 0; i < transform.scale.x; i++)
            {
                for(int j = 0; j < transform.scale.y; j++)
                {
                    Write(c + "", new Vector2Int(i,j));
                }
            }
        }

        public void borderWith(char c, ConsoleColor? color)
        {
            for (int i = 0; i < transform.scale.x; i++)
            {
                if (color != null)
                {
                    Write(c + "", new Vector2Int(i, 0), (ConsoleColor)color);
                    Write(c + "", new Vector2Int(i, transform.scale.y - 1), (ConsoleColor)color);
                }
                else
                {
                    Write(c + "", new Vector2Int(i, 0));
                    Write(c + "", new Vector2Int(i, transform.scale.y - 1));
                }
            }
            for (int i = 0; i < transform.scale.y; i++)
            {
                if (color != null)
                {
                    Write(c + "", new Vector2Int(0, i), (ConsoleColor)color);
                    Write(c + "", new Vector2Int(transform.scale.x - 1, i), (ConsoleColor)color);
                }
                else
                {
                    Write(c + "", new Vector2Int(0, i));
                    Write(c + "", new Vector2Int(transform.scale.x - 1, i));
                }
            }
        }

        public void Write(string txt, Vector2Int pos)
        {
            for (int i = 0; i < txt.Length; i++)
            {
                setCursorPosition(pos);
                if (pos.x > transform.scale.x)
                {
                    pos.x = 0;
                    pos.y += 1;
                }
                if (pos.y > transform.scale.y)
                    throw new Exception("No more room to write in tablet");
                pos.x += 1;
                Console.Write(txt[i]);
            }
        }

        public void Write(string txt, Vector2Int pos, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Write(txt, pos);
            Console.ForegroundColor = stdforeground;
        }

        public void Write(string txt) => Write(txt, new Vector2Int(0, 0));

        public void Clear() => fillWith(' ');
    }
}
