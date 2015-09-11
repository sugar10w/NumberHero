using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Number
{
    public class Map
    {
        public static int W = 360;
        public static int MT = W / 18;

        public int MapX = 5, MapY = 5;

        Game game;
        public Rectangle[,] MapR ;
        public MapMover[,] cmd ;
        bool[,] bo , bo1 ;

        static SolidBrush brush0 = new SolidBrush(Color.FromArgb(255, 181, 224, 242));
        static SolidBrush brush1 = new SolidBrush(Color.FromArgb(255, 199, 232, 245));
        static SolidBrush brush2 = new SolidBrush(Color.FromArgb(255, 233, 246, 251));

        public void cmdDisappear()
        {
            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    cmd[i, j].Disappear();
        }
        public void ReviveAction()
        {
            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j) bo[i, j] = true;

            for (int i = 0; i < game.cntPlayer; ++i)
                if (game.player[i].Alive)
                    bo[game.player[i].X, game.player[i].Y] = false;

            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    if (bo[i,j])
                        cmd[i, j].Appear("#", i, j);
        }

        public void MoveAction(int n)
        {
            Player pl=game.nowPlayer;

            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j) bo[i, j] = false;

            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j) bo1[i, j] = true;
            for (int i = 0; i < game.cntPlayer; ++i) 
                if (game.player[i].Alive)
                    bo1[game.player[i].X, game.player[i].Y] = false;


            if (n < 0) n = -n;
            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    if (bo1[i,j])
                        if (Math.Abs(pl.X - i) + Math.Abs(pl.Y - j) <= n)
                        {
                            cmd[i, j].Appear("#", i, j);
                            bo[i, j] = true;
                        }

            bo[pl.X, pl.Y] = true;
            cmd[pl.X, pl.Y].Appear("[  ]", pl.X, pl.Y, 0, 200, Mover.colorA, 50);

            Resize(2);
        }
        public void AttackAction(Stnum stnum)
        {
            Player pl = game.nowPlayer;

            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j) bo[i, j] = false;

            if (!pl.AttackAction(stnum, bo))
            {
                pl.AddHealth(stnum.n);
                game.alg.ClearAlg();
                game.WaitAttacking = false;
                return;
            }

            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    if (bo[i, j])
                        for (int k=0;k<game.cntPlayer;++k)
                            if (game.player[k].X==i&&game.player[k].Y==j&&game.player[k].Alive)
                                cmd[i, j].Appear(game.player[k].HP.ToString(), i, j);
            bo[pl.X, pl.Y] = true;
            cmd[pl.X, pl.Y].Appear("["+stnum.n.ToString()+"]", pl.X, pl.Y, 0, 200, Mover.colorA, 50);

            Resize(2);
        }

        public void GetClick(Point p)
        {
            if (!game.Waiting)
            {

                game.nowPlayer.ShowSpark(game.nowPlayer, "["+game.nowPlayer.HP.ToString()+"]");
                for (int i = 0; i < game.cntPlayer; ++i)
                    if (!game.player[i].Equals(game.nowPlayer))
                        if (game.player[i].Alive)
                            game.player[i].ShowSpark(game.player[i], game.player[i].HP.ToString(), false);

            }


            if (game.WaitReviving)
            {
                for (int i = 0; i < MapX; ++i)
                    for (int j = 0; j < MapY; ++j)
                        if (MapR[i, j].Contains(p))
                            if (bo[i, j])
                            {
                                game.nowPlayer.Appear(game.nowPlayer.str, i, j);
                                game.nowPlayer.Alive = true;
                                cmdDisappear();
                                game.WaitReviving = false;
                                game.NewRound();
                                return;
                            }
            }

            if (game.WaitMoving)
            {
                for (int i = 0; i < MapX; ++i)
                    for (int j = 0; j < MapY; ++j)
                        if (MapR[i, j].Contains(p))
                            if (bo[i, j])
                            {
                                game.nowPlayer.MoveTo(i, j);
                                cmdDisappear();
                                game.alg.ClearOpr();
                                game.WaitMoving = false;
                                if (!game.alg.GoAttack()) game.alg.ClearAlg();
                                return;
                            }
            }

            if (game.WaitAttacking)
            {
                for (int i = 0; i < MapX; ++i)
                    for (int j = 0; j < MapY; ++j)
                        if (MapR[i, j].Contains(p))
                            if (bo[i, j])
                            {
                                for (int k=0;k<game.cntPlayer;++k)
                                {
                                    Player pl=game.player[k];
                                    if (pl.X == i && pl.Y == j && pl.Alive)
                                    {                                      
                                        game.nowPlayer.Attack(pl);
                                        cmdDisappear();
                                        game.WaitAttacking = false;
                                        game.alg.ClearAlg();
                                        return;
                                    }
                                }

                            }
            }


        }
        public Rectangle ShowRect()
        {
            return new Rectangle(0, 0, Form1.W, Form1.W + Form1.MT);
        }

        Point center;
        AnimPoint LT, RB;
        int MW2 = 0, MH2 = 0;

        public Point GetPointW(int x, int y)
        {
            Point p = new Point();
            p.X = W / MapX * x + center.X - W / 2 - MW2 * W / 2/ MapX;
            p.Y = W / MapY * y + center.Y - W / 2 - MH2 * W / 2/ MapY;
            return p;
        }
        public Point GetPoint(int x, int y)
        {
            Point p = new Point();
            if (LT != null) p.X = (LT.X * (MapX - x) + RB.X * x) / MapX;
            if (LT != null) p.Y = (LT.Y * (MapY - y) + RB.Y * y) / MapY;
            return p;
        }
        public Rectangle GetRectangle(int l,int t,int w,int h)
        {
            Point p1 = GetPoint(l, t);
            Point p2 = GetPoint(l + w , t + h );
            return new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
        }
        public Rectangle RectXY(int x, int y)
        {
            return GetRectangle(x, y, 1, 1);
        }

        public int SizeID = 0;
        public void Resize()
        {
            int l = MapX, r = 0, t = MapY, b = 0;
            int ww, hh;

            if (game.WaitReviving) SizeID = 0;

            if (SizeID >= 100 && SizeID < 200)
            {
                Player pl = game.player[SizeID-100];

                ww = (int)(Form1.W / 2);
                hh = (int)(Form1.H / 2);
                if (ww < hh) W = ww * MapX; else W = hh * MapY;
                MW2 = 2 * pl.X - (MapX-1);
                MH2 = 2 * pl.Y - (MapY-1);
                LT.SetTo(GetPointW(0, 0));
                RB.SetTo(GetPointW(MapX, MapY));
                return;
            }
            switch (SizeID)
            {
                case 0:
                    SetCenter();
                    MW2 = 0;
                    MH2 = 0;
                    if (center.X < center.Y) W = center.X * 2; else W = center.Y * 2;
                    LT.SetTo(GetPointW(0, 0));
                    RB.SetTo(GetPointW(MapX, MapY));
                    break;
                case 1:
                    for (int i = 0; i < game.cntPlayer; ++i)
                        if (game.player[i].Alive)
                        {
                            if (game.player[i].X < l) l = game.player[i].X;
                            if (game.player[i].X > r) r = game.player[i].X;
                            if (game.player[i].Y < t) t = game.player[i].Y;
                            if (game.player[i].Y > b) b = game.player[i].Y;
                        }
                    ww = (int)(center.X * 2 / (r - l + 1.6));
                    hh = (int)(center.Y * 2 / (b - t + 1.6));
                    if (ww < hh) W = ww * MapX; else W = hh * MapY;
                    MW2 = l + r - (MapX-1);
                    MH2 = t + b - (MapY - 1);
                    LT.SetTo(GetPointW(0, 0));
                    RB.SetTo(GetPointW(MapX,MapY));
                    break;   
                case 2:
                    for (int i=0;i<MapX;++i)
                        for (int j=0;j<MapY;++j)
                            if (bo[i, j])
                            {
                                if (i < l) l = i;
                                if (i > r) r = i;
                                if (j < t) t = j;
                                if (j > b) b = j;
                            }
                    ww = (int)(center.X * 2 / (r - l + 1.6));
                    hh = (int)(center.Y * 2 / (b - t + 1.6));
                    if (ww < hh) W = ww * MapX; else W = hh * MapY;
                    MW2 = l + r - (MapX - 1);
                    MH2 = t + b - (MapY - 1);
                    LT.SetTo(GetPointW(0, 0));
                    RB.SetTo(GetPointW(MapX, MapY));
                    break;   
            }

        }
        public void Resize(int id)
        {
            SizeID = id;
            Resize();
        }

        void SetCenter()
        {
            if (Form1.H - 260 > Form1.W - 360)
                center = new Point(Form1.W/2,(Form1.H-260)/2);
            else center = new Point((Form1.W-360)/2,Form1.H/2);
        }

        public void Refresh()
        {
            SetCenter();
            RB.Refresh();
            LT.Refresh();
            W = RB.X - LT.X;
            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    MapR[i, j] = RectXY(i, j);
            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    cmd[i, j].Refresh();            
        }
        public void Draw(Graphics g)
        {

            g.FillRectangle(brush0, GetRectangle(0,0,MapX,MapY));

            if (MapX>2&&MapY>2) g.FillRectangle(brush1, GetRectangle(1,1,MapX-2,MapY-2));

            if (MapX>4&&MapY>4) g.FillRectangle(brush2, GetRectangle(2,2,MapX-4,MapY-4));

            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    cmd[i, j].Draw(g);           
        }

        public Map(Game g,int x=5,int y=5)
        {
            game = g;

            MapX = x; MapY = y;
            if (MapX < 1 || MapX > 9) MapX = 5;
            if (MapY < 1 || MapY > 9) MapY = 5;

            MapR = new Rectangle[MapX, MapY];
            MapR = new Rectangle[MapX,MapY];
            cmd = new MapMover[MapX, MapY];
            bo = new bool[MapX, MapY];
            bo1 = new bool[MapX, MapY];

            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    MapR[i, j] = RectXY(i,j);
            for (int i = 0; i < MapX; ++i)
                for (int j = 0; j < MapY; ++j)
                    cmd[i, j] = new MapMover("", i, j);

            SetCenter();

            MW2 = 0;
            MH2 = 0;
            W = 0;
            LT = new AnimPoint(GetPointW(0, 0));
            RB = new AnimPoint(GetPointW(5, 5));

            Resize(0);
          

        }
    }
}
