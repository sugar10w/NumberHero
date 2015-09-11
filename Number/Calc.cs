using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Number
{

    public class Calc
    {
        static int stop = 10000;

        static int GetId(char c)
        {
            switch (c)
            {
                case '+': return 11;
                case '-': return 12;
                case '*': return 21;
                case '/': return 22;
                case '=': return 0;
                default: return -1;
            }
        }

        static private Stnum CheckEqual(string s)
        {


            int[] num = new int[100], id = new int[100];
            int top = 0;

            for (int i = 0; i < s.Length; ++i)
            {
                if (s[i] == ' ')
                {
                    if (num[top] == 0) return new Stnum("0 to start a number", -10);  //use '0' start a number
                }
                else if (s[i] >= '0' && s[i] <= '9')
                {
                    num[top] = num[top] * 10 + s[i] - '0';
                }
                else
                {
                    id[top] = GetId(s[i]);
                    while (top > 0 && id[top] / 10 <= id[top - 1] / 10)
                    {
                        switch (id[top - 1])
                        {
                            case 11:
                                num[top - 1] += num[top];
                                break;
                            case 12:
                                num[top - 1] -= num[top];
                                break;
                            case 21:
                                num[top - 1] *= num[top];
                                break;
                            case 22:
                                if (num[top] == 0) return new Stnum("divide 0", -101); //divide 0
                                if (num[top - 1] % num[top] != 0) return new Stnum("Can't divide", -11); //can't divide
                                num[top - 1] /= num[top];
                                break;
                        }
                        id[top - 1] = id[top];
                        num[top] = 0;
                        --top;
                    }
                    ++top;



                }

                if (i == s.Length - 1) break;

            }

            if (num[0] != num[1]) return new Stnum("not Equal", -1); //not equal

            string ans = "";

            for (int i = 0; i < s.Length; ++i)
                if (s[i] != ' ')
                {
                    ans += s[i];
                }

            //	t*=num[0];
            return new Stnum(ans, num[0]);
        }

        static public Stnum CheckAttack(string s,bool full=true)
        {
            int ttt = 0;
            const string p = " +-*/";
            string st = "";
            int l = 0, pl = p.Length;
            int cnt = 0;
            double max = Math.Pow((double)pl, (int)(s.Length - 1));
            Stnum best = new Stnum("", -1);

            for (int x0 = 0; x0 < max; ++x0)
            {
                ++ttt; if (!full && ttt > stop) break;

                int x = x0;
                st = "";
                for (int j = 0; j < s.Length; ++j)
                {
                    st += s[j]; ++l;
                    if (j != s.Length - 1) st += p[x % pl]; ++l;
                    x /= pl;
                }

                int i;
                for (i = st.Length - 2; i > 0; i -= 2) if (st[i] != ' ') break;

                if (i < 1) i = 1;
                for (; i < st.Length; i += 2)
                {
                    if (st[i] == ' ')
                    {
                        string stt = "";
                        stt = st.Substring(0, i) + "=" + st.Substring(i + 1, st.Length - i - 1);

                        Stnum stn = CheckEqual(stt);

                        if (stn.n > 0)
                        {
                            if (stn.n > best.n) best = stn;
                            ++cnt;
                            if (cnt > 20) return best;
                        }
                    }
                }
            }

            return best;
        }

        static public Stnum CheckMove(string s,bool full=true)
        {
            int ttt = 0;
            const string p = " ,";
            string st = "";
            int pl = p.Length;
            double max = Math.Pow((double)pl, (int)(s.Length - 1));

            for (int x0 = (int)max; x0 >= 3; --x0)
            {
                ++ttt; if (!full && ttt > stop) break;

                int x = x0;
                st = "";
                for (int i = 0; i < s.Length; ++i)
                {
                    st += s[i];
                    if (i != s.Length - 1) st += p[x % pl];
                    x /= pl;
                }


                int top = 0;
                int[] num = new int[100];
                bool flag = true;
                for (int i = 0; i < st.Length; ++i)
                {
                    switch (st[i])
                    {
                        case ' ': if (num[top] == 0) flag = false; break;
                        case ',': ++top; break;
                        default: num[top] = num[top] * 10 + st[i] - '0'; break;
                    }
                }

                if (!flag) continue;
                if (top <= 1) continue;

                bool flag1 = true;
                for (int i = 1; i <= top - 1; ++i)
                    if (num[i] * 2 != num[i - 1] + num[i + 1]) { flag1 = false; break; }

                if (flag1)
                {
                    string ans="";
                    for (int i = 0; i < st.Length; ++i) if (st[i] != ' ') ans += st[i];
                    return new Stnum(ans, top);
                }

                bool flag2 = true;
                for (int i = 1; i <= top - 1; ++i)
                    if (num[i] == 0 || num[i] * num[i] != num[i - 1] * num[i + 1]) { flag2 = false; break; }


                if (flag2)
                {
                    string ans = "";
                    for (int i = 0; i < st.Length; ++i) if (st[i] != ' ') ans += st[i];
                    return new Stnum(ans, -top);
                }
            }
            return new Stnum("", 0);
        }

    }
}