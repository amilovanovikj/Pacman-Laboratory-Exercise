using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    class Prepreka
    {
        public Point P { get; set; }

        public bool isColided(Point p)
        {
            return p == P || p == new Point(P.X, P.Y + 1) || p == new Point(P.X, P.Y + 2) || p == new Point(P.X + 1, P.Y) || p == new Point(P.X, P.Y + 3) || p == new Point(P.X, P.Y -1) || p == new Point(P.X-1, P.Y) || p == new Point(P.X, P.Y - 2) || p == new Point(P.X, P.Y - 3) || p == new Point(P.X-1, P.Y -2) || p == new Point(P.X-1, P.Y -1) || p == new Point(P.X -1, P.Y + 1) || p == new Point(P.X -1, P.Y + 2) || p == new Point(P.X + 1, P.Y - 2) || p == new Point(P.X + 1, P.Y -1) || p == new Point(P.X + 1, P.Y + 1) || p == new Point(P.X + 1, P.Y +2);
        }
    }
}
