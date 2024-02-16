using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireEngine
{
    //Vector 2D

    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(Vector2Int a)
        {
            x = a.x;
            y = a.y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, b.y + a.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, b.y - a.y);
        }

        public static Vector2 operator *(float a, Vector2 b)
        {
            return new Vector2(a * b.x, b.y * a);
        }

        public static Vector2 operator *(Vector2 b, float a)
        {
            return new Vector2(a * b.x, b.y * a);
        }

        public static Vector2 operator -(Vector2 a) => (a * -1);
    }

    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2Int operator -(Vector2Int a) => (a * -1);

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, b.y + a.y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, b.y - a.y);
        }

        public static Vector2Int operator *(int a, Vector2Int b)
        {
            return new Vector2Int(a * b.x, b.y*a);
        }

        public static Vector2Int operator *(Vector2Int b,int a)
        {
            return new Vector2Int(a * b.x, b.y * a);
        }
        
    }

    public static class Math
    {
        public static bool pointInRect(Vector2Int point, Vector2Int rectPos, Vector2Int rectSize) => pointInRect(new Vector2(point), new Vector2(rectPos), new Vector2(rectSize));

        public static bool pointInRect(Vector2 point, Vector2 rectPos, Vector2 rectSize)
        {
            if (point.x > rectPos.x && point.x < rectPos.x + rectSize.x)
            {
                if (point.y > rectPos.y && point.y < rectPos.y + rectSize.y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
