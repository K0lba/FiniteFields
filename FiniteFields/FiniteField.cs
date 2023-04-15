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
        public FiniteFieldElements Create0()
        {
            return new FiniteFieldElements(new int[1] { 0 }, this);
        }
        public FiniteFieldElements Create1()
        {
            return new FiniteFieldElements(new int[1] { 1 }, this);
        }


        public FiniteFieldElements CreateElementFromBinary(byte[] bytes)
        {
            if (p != 2) throw new Exception("Поле должно быть характеристики 2");
            
            int n = BitConverter.ToInt32(bytes);
            int[] res = new int[n];
            int counter = 0;
            while (n > 0)
            {
                res[counter] = n % 2;
                n = n / 2;
                counter++;
            }
            return new FiniteFieldElements(FiniteFieldElements.KillZeros(res), this);
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
    public class FiniteFieldElements
    {
        public int[] coef { get; set; }
        public FiniteField f { get; set; }
        public FiniteFieldElements(int[] coef, FiniteField f)
        {
            if (coef.Length - 1 > f.n) 
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
        public static FiniteFieldElements operator -(FiniteFieldElements a)
        {
            int deg = a.coef.Length;
            int[] result = new int[deg];
            for(int i=0; i<deg; i++)
            {
                result[i] = Mod(-a.coef[i],a.f.p);
            }
            return new FiniteFieldElements(KillZeros(result), a.f);
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
            return new FiniteFieldElements(KillZeros(result), a.f);
        }

        public static FiniteFieldElements operator -(FiniteFieldElements a, FiniteFieldElements b) => a + (-b);

        public static FiniteFieldElements operator %(int[] a, FiniteFieldElements b)
        {
            if(a.Length < b.coef.Length) { return new FiniteFieldElements(a,b.f); }
            int n = (int)a.Length - 1;
            int m = (int)b.coef.Length - 1;
            int[] result = a;
            int[] Q = new int[n - m + 1];
            for (int i = n; i >= m; i--)
            {
                Q[i - m] = Mod(result[i] * (int)Math.Pow(b.coef[m],b.f.p-2), b.f.p);
                for (int j = m; j >= 0; j--) 
                {
                    result[i - m + j] = Mod(result[i - m + j] - b.coef[j] * Q[i - m], b.f.p);
                }
            }
            Array.Resize(ref result, m);
            return new FiniteFieldElements(KillZeros(result), b.f);

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
            FiniteFieldElements res = result % new FiniteFieldElements(a.f.q,a.f);
            return res;
        }

        public FiniteFieldElements Power(int power)
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
        public FiniteFieldElements Inverse()
        {
            return this.Power((int)Math.Pow(f.p, f.n) - 2);
        }
        public static FiniteFieldElements operator /(FiniteFieldElements a, FiniteFieldElements b) => a * b.Inverse();

        public byte[] GetToBinary()
        {
            if (f.p != 2) throw new Exception("Поле должно быть характеристики 2");
            return BitConverter.GetBytes(Gorner(2,coef)); 
        }
        private int Gorner(int x, int[] a, int i = 0)
        {
            if (i >= a.Length)
                return 0;
            return a[i] + x * Gorner(x, a, i + 1);
        }
    }

    public class Program
    {
        public static void Main(string[] args) { }
    }

}
