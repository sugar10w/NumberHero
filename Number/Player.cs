using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Number
{
    public class Player:MapMover
    {
        public MapMover Spark;

        public string name;

        public int HPMax = 100, HPStart = 64, DeltaD = 16;
        public int HP;
        public bool Alive = true, canAttack = false;

        public int PlayerID;       

        public void ShowSpark(Player e,string st,bool ak=true)
        {
            if (!e.Equals(this))
            {
                Spark.Appear(st, X, Y, 150, 255, colorA, 50);
                Spark.MoveTo(e.X, e.Y);
            }
            else if (ak)
            {
                Spark.Appear(st, X, Y + 1, 150, 255, colorA, 50);
                Spark.MoveTo(e.X, e.Y);
            }
            else
            {
                Spark.Appear(st, X, Y + 1, 0, 150, colorO, 50);
                Spark.MoveTo(e.X, e.Y);
            }
        }

        public void AddHealth(int n)
        {
            HP += n;
            if (HP > HPMax) HP = HPMax;
            game.map.Resize(100+PlayerID);
            ShowSpark(this,n.ToString());
            game.help.l2 = this.str + " didn't attack anyone\nbut added its HP to "+HP+" ...";
        }
        public void Beaten(int n)
        {
            HP -= n;
            ShowSpark(this,n.ToString(),false);
            if (HP <= 0)
            {
                Alive = false;
                canAttack = false;
          //      X = -1; Y = -1;
                game.map.Resize(100 + PlayerID);
                Disappear();
                game.help.l2 = " "+this.str+" has just been killed\nby "+game.nowPlayer.str+" ~~~~~";
            }
        }

        public void Revive()
        {
            game.map.ReviveAction();
            HPStart -= DeltaD;
            HP = HPStart;
        }

        public int Atn=0; 
        public bool AttackAction(Stnum stnum,bool[,] bo)
        {
            Atn = stnum.n;

            int a = 0, b = 0, c = 0, d = 0;
            string st = stnum.st;
            for (int i = 0; i < st.Length; ++i)
            {
                switch (st[i])
                {
                    case '+': ++a; break;
                    case '-': ++b; break;
                    case '*': ++c; break;
                    case '/': ++d; break;
                }
            }

     //       if (b != 0) ++a;
     //       if (d != 0) ++c;
            a += b;
            c += d;
            if (a < c) a = c;

            bool flag = false;

            for (int i = 0; i < game.cntPlayer; ++i)
                if (i != PlayerID)
                {
                    Player e = game.player[i];
                    if (!e.canAttack) continue;
                    if (e.X == X && Math.Abs(e.Y - Y) <= a)
                    { flag = true; bo[e.X, e.Y] = true; }
                    if (e.Y == Y && Math.Abs(e.X - X) <= a) 
                    { flag = true; bo[e.X, e.Y] = true; }
                    if (Math.Abs(e.X - X) <= c && Math.Abs(e.X - X) == Math.Abs(e.Y - Y)) 
                    { flag = true; bo[e.X, e.Y] = true; }
                }

        //    bo[X, Y] = true;

            return flag;
        }

        public void Attack(Player e)
        {
            if (e.Equals(this))
            {
                AddHealth(Atn);
                return;
            }

            if (e.X == X || e.Y == Y)
            {
                e.Beaten(Atn);
                ShowSpark(e, "+");
                return;
            }

                e.Beaten(Atn);
                ShowSpark(e, "*");
                return;

        }

        public Player(int id,string nm,string st,int x0,int y0)
        {
            name = nm; str = st; loc = 100 + 10 * x0 + y0;
            X = x0; Y = y0;
            if (game != null) ToPT = PT = game.map.GetPoint(X, Y);
            HP = -1;
            Alive = false;
            PlayerID = id;

            Spark = new MapMover("",x0,y0);

            color = colorO; 
            ff = new Font("Georgia", Map.W / 10, FontStyle.Bold);
        }


    }
}
