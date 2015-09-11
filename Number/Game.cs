using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Number
{
    public class Game
    {
        public Alg alg;
        public Map map;
        public Help help;
        public Player[] player;

        public bool WaitMoving = false;
        public bool WaitAttacking = false;
        public bool WaitReviving = false;

        public bool Waiting { get { return WaitMoving || WaitAttacking || WaitReviving; } }
        public Player nowPlayer { get { return player[nowPlayerID]; } }


        public int PlayingID;
        public int cntPlayer = 3;
        public int nowPlayerID;


        public void Refresh()
        {
            alg.Refresh();

            map.Refresh();

            for (int i = 0; i < cntPlayer; ++i)
            {
                if (player[i].Spark.Arrived()) player[i].Spark.Disappear();
                player[i].Spark.Refresh();
                player[i].Refresh();
            }
         
        }
        public void Draw(Graphics g)
        {
            SolidBrush brush;
            Font ff;

            //Backgroud
            brush = new SolidBrush(Color.AliceBlue);
            g.FillRectangle(brush, 0, 0, Form1.W, Form1.H);

            //M
            map.Draw(g);

            for (int i = 0; i < cntPlayer; ++i)
            {
                if (player[i].Alive) player[i].Draw(g);
                player[i].Spark.Draw(g);
            }

            g.FillRectangle( new SolidBrush(Color.FromArgb(100,Color.AliceBlue)),alg.ShowRect());

            alg.DrawL(g);
            alg.DrawG(g);
            alg.DrawA(g);

             help.Draw(g);



            //O
            ff = new Font("Arial", 12, FontStyle.Bold);
            string l = "";
            l += player[nowPlayerID].str + "'s playing.    ";
            for (int i = 0; i < cntPlayer; ++i)
                if (player[i].HP >= 0) l += player[i].str + player[i].HP + "    ";
            if (WaitAttacking) l += " Attacking ";
            if (WaitMoving) l += " Moving ";
            if (WaitReviving) l += " Reviving ";
            if (alg.CheckEmpty()) l += " Empty ";
            l +=" MapMode="+ map.SizeID.ToString();
            g.DrawString(l, ff, Brushes.CornflowerBlue, 0, 0);
        }

        public void GetClick(Point p)
        {

            if (alg.ShowRect().Contains(p))
            {
                map.Resize(1);
                if (Waiting) map.Resize();
                alg.GetClick(p);
                return;
            }
            else 
            {
                map.Resize(1);
                if (Waiting) map.Resize();
                map.GetClick(p);
                if (WaitReviving) nowPlayer.ShowSpark(nowPlayer, "[" + nowPlayer.str + "]", false);
                return;
            }
        }

        public void NewRound()
        {

            ++nowPlayerID;
            if (nowPlayerID >= cntPlayer) nowPlayerID = 0;

            if (nowPlayer.Alive)
            {
                map.Resize(1);
                nowPlayer.canAttack = true;
                if (nowPlayer.color.A != 255) nowPlayer.Appear(nowPlayer.str, nowPlayer.X, nowPlayer.Y, 75, 255);
                nowPlayer.ShowSpark(nowPlayer, "[  ]");
                alg.GetNum();
            }
            else if (nowPlayer.HPStart > 0)
            {
                map.Resize(0);
                WaitReviving = true;
                nowPlayer.Revive();
                nowPlayer.ShowSpark(nowPlayer, "[" + nowPlayer.str + "]", false);
            }
            else NewRound();

            alg.ClearAlg();
            if (help.l2!="" && help.l2[0]!=' ') help.l2 = "";

     //       MessageBox.Show("It's time for "+player[nowPlayerID].name);
        }

        public Game()
        {


            try
            {
                FileStream inFile = new FileStream("Setting.txt", FileMode.Open);
                StreamReader sr = new StreamReader(inFile);                
                string st = sr.ReadLine();
                map= new Map(this, st[0] - '0', st[1] - '0');
                
                st = sr.ReadLine();
                cntPlayer = st.Length;
                player = new Player[cntPlayer];
                for (int i = 0; i < cntPlayer; ++i)
                    player[i] = new Player(i, "", st[i].ToString(), map.MapX / 2, map.MapY / 2);
            }
            catch
            {
                map = new Map(this);
                cntPlayer = 4;
                player = new Player[cntPlayer];
                player[0] = new Player(0, "", "A", map.MapX / 2, map.MapY / 2);
                player[1] = new Player(1, "", "B", map.MapX / 2, map.MapY / 2);
                player[2] = new Player(2, "", "C", map.MapX / 2, map.MapY / 2);
                player[3] = new Player(3, "", "D", map.MapX / 2, map.MapY / 2);

            }

            //cntPlayer = 4;
            //player = new Player[cntPlayer];
            //player[0] = new Player(0, "", "A", map.MapX / 2, map.MapY / 2);
            //player[1] = new Player(1, "", "B", map.MapX / 2, map.MapY / 2);
            //player[2] = new Player(2, "", "C", map.MapX / 2, map.MapY / 2);
            //player[3] = new Player(3, "", "D", map.MapX / 2, map.MapY / 2);
            //nowPlayerID = -1;

            alg = new Alg(this);
            help = new Help(this);
            Mover.game = this;

            nowPlayerID = -1;

            WaitReviving = true;

            NewRound();
        }

    }

    

}
