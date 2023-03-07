using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteFields
{ 
    public class FiniteField
    {
        public int n { get; set; }
        public int p { get; set; }
        public FiniteFieldElements q { get; set; }

        public FiniteField(int n, int p, FiniteFieldElements q)
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
    }
    public class FiniteFieldElements
    {
        public int[] coef { get; set; }
        public FiniteField f { get; set; }

        public FiniteFieldElements(int[] coef, FiniteField f)
        {
            this.coef = coef;
            this.f = f;
        }
        
        public static FiniteFieldElements operator -(FiniteFieldElements a)
        {
            int deg = a.f.n;
            int[] result = new int[deg];
            for(int i=0; i<=deg; i++)
            {
                result[i] = -a.coef[i];
            }
            return new FiniteFieldElements(result, a.f);
        }

        public static FiniteFieldElements operator +(FiniteFieldElements a, FiniteFieldElements b)
        {
            int deg = Math.Max(a.f.n, b.f.n);
            int[] result = new int[deg];
            for (int i = 0; i <= deg; i++)
            {
                result[i] = a.coef[i] + b.coef[i];
            }
            return new FiniteFieldElements(result, a.f);
        }

        public static FiniteFieldElements operator -(FiniteFieldElements a, FiniteFieldElements b) => a + (-b);

        public static FiniteFieldElements operator %(FiniteFieldElements a, FiniteFieldElements b)
        {

            return new FiniteFieldElements(a.coef, a.f);
        }
        public static FiniteFieldElements operator *(FiniteFieldElements a, FiniteFieldElements b)
        {
            int deg = a.f.n + b.f.n;
            int[] result = new int[deg];
            for (int i = 0; i <= deg; i++)
            {
                for (int j = 0; j <= Math.Min(i, a.f.n); j++)
                {
                    result[i] += a.coef[j] * b.coef[i-j];
                }
            }
            FiniteFieldElements res = new FiniteFieldElements(result,a.f) % a.f.q;    
            return res;
        }

        public FiniteFieldElements Power(int power)
        {
            int deg = power%((int)Math.Pow(f.p,f.n) - 1);
            if(deg == 0) return f.Create1();
            if (deg == 1) return this;
            else
            {
                return (Power(deg/2) * Power(deg/2));
            }
        }
        public FiniteFieldElements Reversed()
        {
            return this.Power((int)Math.Pow(f.p,f.n)-2);
        }
        public static FiniteFieldElements operator /(FiniteFieldElements a, FiniteFieldElements b) => a * b.Reversed();
    }
}