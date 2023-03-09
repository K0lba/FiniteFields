using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FiniteFields
{ 
    public class FiniteField
    {
        public int p { get; set; }
        public int n { get; set; }
        public int[] q { get; set; }

        public FiniteField(int p, int n, int[] q)
        {
            this.n = n;
            this.p = p;
            this.q = q;

        }
        public FiniteFieldElements Create0()
        {
            return new FiniteFieldElements(new int[1] { 0 }, this);
        }
        public FiniteFieldElements Create1()
        {
            return new FiniteFieldElements(new int[1] { 1 }, this);
        }
        public FiniteFieldElements GetBynaryRepres(int n)
        {
            if (p == 2)
            {
                string conv = Convert.ToString(n, 2);
                int[] res = conv.Split().Select(x => Convert.ToInt32(x)).ToArray();
                return new FiniteFieldElements(res, this);
            }
            else
                throw new Exception("Поле должно быть характеристики 2");
        }
        public int GetDecimalRepres(FiniteFieldElements a)
        {
            if (p == 2)
            {
                return Convert.ToInt32(Convert.ToString(Convert.ToInt32(string.Join("", a.coef), 2), 10));
            }
            else
                throw new Exception("Поле должно быть характеристики 2");
        }
        public FiniteFieldElements GetFromBinary(byte[] bytes)
        {
            if (p == 2)
            {
                int n = BitConverter.ToInt32(bytes, 0);
                return GetBynaryRepres(n);
            }
            else throw new Exception("Поле должно быть характеристики 2");
        }

        public override bool Equals(object? obj)
        {
            var field = obj as FiniteField;
            if (field.p == p && field.n == n && field.q == q) return true;
            return false;
        }
    }
    public class FiniteFieldElements
    {
        public int[] coef { get; set; }
        public FiniteField f { get; set; }
        public FiniteFieldElements(int[] coef, FiniteField f)
        {
            //if (coef.Length != f.n) throw new Exception("Размерность элемента не совпадает с размерностью заданного поля");
            this.coef = coef;
            this.f = f;
        }

        public static int Mod(int n, int charac)
        {
            if ((n %= charac) < 0)
            {
                return n + charac;
            }
            else { return n; }
        }
        public static FiniteFieldElements operator -(FiniteFieldElements a)
        {
            int deg = a.coef.Length;
            int[] result = new int[deg];
            for(int i=0; i<deg; i++)
            {
                result[i] = -a.coef[i];
            }
            return new FiniteFieldElements(result, a.f);
        }

        public static FiniteFieldElements operator +(FiniteFieldElements a, FiniteFieldElements b)
        {
            if (!a.f.Equals(b.f))
                throw new InvalidOperationException();
            int deg = Math.Max(a.coef.Length, b.coef.Length);
            int lim = Math.Min(a.coef.Length, b.coef.Length);
            int[] result = new int[deg];
            _ = (a.coef.Length == deg) ? result = a.coef : result = b.coef;
            for (int i = 0; i < lim; i++)
            {
                result[i] = Mod(a.coef[i] + b.coef[i], a.f.p);
            }
            return new FiniteFieldElements(result, a.f);
        }

        public static FiniteFieldElements operator -(FiniteFieldElements a, FiniteFieldElements b) => a + (-b);

        public static FiniteFieldElements operator %(FiniteFieldElements a, FiniteFieldElements b)
        {
            if (!a.f.Equals(b.f))
                throw new InvalidOperationException();
            int n = (int)a.coef.Length - 1;
            int m = (int)b.coef.Length - 1;
            int[] result = a.coef;
            int[] Q = new int[n - m + 1];
            for (int i = n; i >= m; i--)
            {
                Q[i - m] = Mod(result[i] * (int)Math.Pow(b.coef[m],a.f.p-2), a.f.p);
                for (int j = m; j >= 0; j--) 
                {
                    result[i - m + j] -= b.coef[j] * Q[i - m];
                    result[i - m + j] = Mod(result[i - m + j], a.f.p);
                }
            }
            Array.Resize(ref result, m);
            while ( result[result.Length - 1] == 0 )
            {
                if (result.Length - 1 == 0) break;
                result = result.SkipLast(1).ToArray();
            }
            return new FiniteFieldElements(result, a.f);

        }
        public static FiniteFieldElements operator *(FiniteFieldElements a, FiniteFieldElements b)
        {
            if (!a.f.Equals(b.f))
                throw new InvalidOperationException();
            int deg = a.coef.Length + b.coef.Length;
            int[] result = new int[a.coef.Length + b.coef.Length - 1];
            for (var i = a.coef.Length - 1; i >= 0; i--)
            {
                for (var j = b.coef.Length - 1; j >= 0; j--)
                {
                    result[i + j] = Mod(result[i+j] + a.coef[i] * b.coef[j],a.f.p);
                }
            }
            FiniteFieldElements res = new FiniteFieldElements(result, a.f) % new FiniteFieldElements(a.f.q,a.f);
            return res;
        }

        public FiniteFieldElements Power(int power)
        {
            int deg = power % ((int)Math.Pow(f.p, f.n) - 1);
            if (deg == 0) return f.Create1();
            if (deg == 1) return this;
            else
            {
                return (Power(deg / 2) * Power(deg / 2));
            }
        }
        public FiniteFieldElements Reversed()
        {
            return this.Power((int)Math.Pow(f.p, f.n) - 2);
        }
        public static FiniteFieldElements operator /(FiniteFieldElements a, FiniteFieldElements b) => a * b.Reversed();

        public byte[] GetToBinary()
        { 
            int deg = 0;
            int el = 0;
            for (int i = coef.Length - 1; i >= 0; i--)
                el += coef[i] * (int)Math.Pow(f.p, deg++);
            if (f.p == 2)
            {
                return BitConverter.GetBytes(el);
            }
            else
                throw new Exception("Поле должно быть характеристики 2");
        }
    }
}
