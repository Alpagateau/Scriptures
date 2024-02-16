using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireEngine
{
    public class Transform2D
    {
        public Vector2Int position;
        public Vector2Int scale;

        public Transform2D()
        {
            position = new Vector2Int();
            scale = new Vector2Int();
        }
    }
}
