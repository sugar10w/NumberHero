using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Number
{
    public class AnimPoint
    {
        public Point PT, ToPT;
        public int X { set { PT.X = value; } get { return PT.X; } }
        public int Y { set { PT.Y = value; } get { return PT.Y; } }
        public double k = 0.6666;

        public virtual void Refresh()
        {
            if (PT.Equals(ToPT)) return;

            if (Math.Abs(PT.Y - ToPT.Y) <= 2)
                if (PT.X > ToPT.X) PT.X -= 1;
                else if (PT.X < ToPT.X) PT.X += 1;
            if (Math.Abs(PT.Y - ToPT.Y) <= 2)
                if (PT.Y > ToPT.Y) PT.Y -= 1;
                else if (PT.Y < ToPT.Y) PT.Y += 1;

            if (Math.Abs(PT.X - ToPT.X) <= 1 && Math.Abs(PT.Y - ToPT.Y) <= 1) return;


            PT.X = (int)(k * PT.X + (1 - k) * ToPT.X + 0.5);
            PT.Y = (int)(k * PT.Y + (1 - k) * ToPT.Y + 0.5);
        }

        public bool Arrived()
        {
            return PT.Equals(ToPT);
        }

        public void SetTo(int x, int y) { ToPT.X = x; ToPT.Y = y; }
        public void SetTo(Point p) { ToPT = p; }

        public AnimPoint() { }
        public AnimPoint(int x, int y, double kk = 0.6666) { PT.X = x; PT.Y = y; ToPT = PT; k = kk; }
        public AnimPoint(Point p, double kk = 0.6666) { ToPT = PT = p; k = kk; }
    }
}
