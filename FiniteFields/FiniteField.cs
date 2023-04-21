using System.Runtime.CompilerServices;

namespace FiniteFields
{ 
    public class FiniteField
    {
        public int p { get; set; }
        public int n { get; set; }
        public int[] q { get; set; }

        public FiniteField(int p, int n, int[] q)
        {
            if (q.Length - 1 != n) throw new Exception("Передан некорректный неприводимый многочлен");
            this.n = n;
            this.p = p;
            this.q = q;          
        }
        public FiniteFieldElement Create0()
        {
            return new FiniteFieldElement(new int[1] { 0 }, this);
        }
        public FiniteFieldElement Create1()
        {
            return new FiniteFieldElement(new int[1] { 1 }, this);
        }


        public FiniteFieldElement CreateElementFromBinary(byte Byte)
        {
            if (p != 2) throw new Exception("Поле должно быть характеристики 2");

            int n = Byte;
            int[] res = new int[8];
            int counter = 0;
            while (n > 0)
            {
                res[counter] = n % 2;
                n = n / 2;
                counter++;
            }
            return new FiniteFieldElement(FiniteFieldElement.KillZeros(res), this);
        }

        public override bool Equals(object? obj)
        {
            var field = (obj as FiniteField)!;
            if (field.p == p && field.n == n && field.q == q) return true;

            return false;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public class FiniteFieldElement
    {
        public int[] coef { get; set; }
        public FiniteField f { get; set; }
        public FiniteFieldElement(int[] coef, FiniteField f)
        {
            if (coef.Length > f.n) 
                throw new Exception("Размерность элемента не совпадает с размерностью заданного поля");
            this.coef = coef;
            this.f = f;
        }

        public static int[] KillZeros(int[] a)
        {
            if(a.Length == 0) return a;
            while (a[a.Length - 1] == 0)
            {
                if (a.Length - 1 == 0) break;
                a = a.SkipLast(1).ToArray();
            }
            return a;
        }
        private static int Mod(int n, int charac)
        {
            if ((n % charac) < 0)
            {
                return (n % charac) + charac;
            }
            else { return (n % charac); }
        }
        public static FiniteFieldElement operator -(FiniteFieldElement a)
        {
            int deg = a.coef.Length;
            int[] result = new int[deg];
            for(int i=0; i<deg; i++)
            {
                result[i] = Mod(-a.coef[i],a.f.p);
            }
            return new FiniteFieldElement(KillZeros(result), a.f);
        }

        public static FiniteFieldElement operator +(FiniteFieldElement a, FiniteFieldElement b)
        {
            if (!a.f.Equals(b.f))
                throw new InvalidOperationException();
            int deg = Math.Max(a.coef.Length, b.coef.Length);
            int lim = Math.Min(a.coef.Length, b.coef.Length);
            int[] result = new int[deg];
            if (a.coef.Length == deg)
                a.coef.CopyTo(result,0);
            else
                b.coef.CopyTo(result,0);
            for (int i = 0; i < lim; i++)
            {
                result[i] = Mod(a.coef[i] + b.coef[i], a.f.p);
            }
            return new FiniteFieldElement(KillZeros(result), a.f);
        }

        public static FiniteFieldElement operator -(FiniteFieldElement a, FiniteFieldElement b) => a + (-b);

        private static FiniteFieldElement PolynomDiv(int[] a, int[] b, FiniteField f)
        {
            if(a.Length < b.Length) { return new FiniteFieldElement(a, f); }
            int n = (int)a.Length - 1;
            int m = (int)b.Length - 1;
            int[] result = a;
            int[] Q = new int[n - m + 1];
            for (int i = n; i >= m; i--)
            {
                Q[i - m] = Mod(result[i] * (int)Math.Pow(b[m],f.p-2), f.p);
                for (int j = m; j >= 0; j--) 
                {
                    result[i - m + j] = Mod(result[i - m + j] - b[j] * Q[i - m], f.p);
                }
            }
            Array.Resize(ref result, m);
            return new FiniteFieldElement(KillZeros(result), f);

        }
        public static FiniteFieldElement operator *(FiniteFieldElement a, FiniteFieldElement b)
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
            return PolynomDiv(result, a.f.q, a.f);
        }

        public FiniteFieldElement Power(int power)
        {
            int deg = power % ((int)Math.Pow(f.p, f.n) - 1);
            if (deg == 0) return f.Create1();
            if(deg % 2 == 0) 
            {
                var temp = Power(deg / 2);
                return (temp * temp);
            }
            else
            {
                return this * Power(deg - 1);
            }
        }
        public FiniteFieldElement Inverse()
        {
            return this.Power((int)Math.Pow(f.p, f.n) - 2);
        }
        public static FiniteFieldElement operator /(FiniteFieldElement a, FiniteFieldElement b) => a * b.Inverse();

        public byte GetToBinary()
        {
            if (f.p != 2) throw new Exception("Поле должно быть характеристики 2");
            return (byte)Horner(2,coef); 
        }
        private int Horner(int x, int[] a, int i = 0)
        {
            if (i >= a.Length)
                return 0;
            return a[i] + x * Horner(x, a, i + 1);
        }
    }

    public class Program
    {
        public static void Main(string[] args) {}
    }

}

