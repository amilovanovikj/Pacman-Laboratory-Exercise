using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public class Pacman
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }
        public static int Radius = 20;
        public int Speed { get; set; }
        public bool IsMouthOpen { get; set; }

        public Pacman()
        {
            X = 7;
            Y = 5;
            Speed = Radius;
            Direction = Direction.RIGHT;
            IsMouthOpen = false;
        }

        public void ChangeDirection(Direction direction)
        {
            this.Direction = direction;
        }

        public void Move(int width, int height)
        {
            switch (this.Direction)
            {
                case Direction.DOWN:
                    Y = Mod(Y + 1, height); break;
                case Direction.UP:
                    Y = Mod(Y - 1, height); break;
                case Direction.RIGHT:
                    X = Mod(X + 1, width); break;
                case Direction.LEFT:
                    X = Mod(X - 1, width); break;
            }
        }

        public void Draw(Graphics g)
        {
            Brush b = new SolidBrush(Color.Yellow);
            if (!IsMouthOpen)
            {
                g.FillEllipse(b, Radius * 2 * X, Radius * 2 * Y, 2 * Radius, 2 * Radius);
            }
            else
            {
                switch (this.Direction)
                {
                    case Direction.RIGHT:
                        g.FillPie(b, Radius * 2 * X, Radius * 2 * Y, 2 * Radius, 2 * Radius, 45, 270);
                        break;
                    case Direction.LEFT:
                        g.FillPie(b, Radius * 2 * X, Radius * 2 * Y, 2 * Radius, 2 * Radius, 225, 270);
                        break;
                    case Direction.UP:
                        g.FillPie(b, Radius * 2 * X, Radius * 2 * Y, 2 * Radius, 2 * Radius, 315, 270);
                        break;
                    case Direction.DOWN:
                        g.FillPie(b, Radius * 2 * X, Radius * 2 * Y, 2 * Radius, 2 * Radius, 135, 270);
                        break;
                }
                
            }
            IsMouthOpen = !IsMouthOpen;
        }

        public static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
