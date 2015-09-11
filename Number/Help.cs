using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Number
{
    public class Help
    {
        Game game;

        Font ff1 = new Font("Lucida Console",20,FontStyle.Bold);
        Font ff2 = new Font("Arial",20,FontStyle.Bold);
        Font ff3 = new Font("Georgia", 20, FontStyle.Bold);
        Brush brush = new SolidBrush(Color.FromArgb(180,SystemColors.ControlDarkDark));
        int ttt = 0, stop = 50;
        public string l = "", l2 = "";

        void L1(Graphics g)
        {
            g.DrawString("["+game.nowPlayer.str+"]", ff3, brush, Form1.W - 58, Form1.H - 240);
            g.DrawString(l, ff1, brush, Form1.W - 360, Form1.H - 280);
        }
        void L2(Graphics g)
        {
            g.DrawString(l2, ff2, brush, Form1.W - 360, Form1.H - 340);
        }

        public void Start()
        {
            ttt = 0;
            l = "";
        }

        public void Draw(Graphics g)
        {
            if (!game.Waiting)
            {
                ++ttt; ttt %= stop;
                if (Alg.Atop != 0)
                {
                    if (ttt == 0)
                    {
                        string st = game.alg.ALine();
                        l = "";
                        Stnum stnum;
                        stnum = Calc.CheckMove(st, false);
                        if (stnum.n != 0) l += stnum.st + " ";
                        stnum = Calc.CheckAttack(st, false);
                        if (stnum.n != -1) l += stnum.st;
                    }
                }
                else
                {
                    if (l2=="") l2 = "What would " + game.nowPlayer.str + " do with\nthese numbers?";
                    L2(g);
                    return;
                }
                L1(g);
            }
            else
            {
                if (game.WaitAttacking) l2 = game.nowPlayer.str + " is attacking ..<>...";
                else if (game.WaitMoving) l2 = game.nowPlayer.str + ": moves to ..#...";
                else if (game.WaitReviving) l2 = game.nowPlayer.str + ": Where to revive?";
                L2(g);
            }
        }

        public Help(Game g)
        {
            game = g;
        }
    }
}
