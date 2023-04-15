using FiniteFields;

namespace TestForFiniteFields
{
    public class Tests
    {
        [Test]
        public void Sum1()
        {
            var GF4 = new FiniteField(2, 2, new int[] { 1, 1, 1 });
            var element1 = new FiniteFieldElements(new int[] { 1, 1 }, GF4);
            var element2 = new FiniteFieldElements(new int[] { 0, 1 }, GF4);
            var sum = element1 + element2;
            Assert.That(sum.coef, Is.EqualTo(new int[] { 1 }));
        }
        [Test]
        public void Pow1()
        {
            var GF9 = new FiniteField(3, 2, new int[] { 1, 2, 2 });
            var el1 = new FiniteFieldElements(new int[] { 2, 1 }, GF9);
            var pow = el1.Power(9);
            Assert.That(el1.coef, Is.EqualTo(pow.coef));
        }

        [Test]
        public void Multiplication1()
        {
            var GF4 = new FiniteField(2, 2, new int[] { 1, 1, 1 });
            var element1 = new FiniteFieldElements(new int[] { 1, 0, 1 }, GF4);
            var element2 = GF4.Create0();
            var mult = element1 * element2;
            mult = mult + element1;
            Assert.That(mult.coef, Is.EqualTo(element1.coef));
        }

        [Test]
        public void GetInverse1()
        {
            var GF4 = new FiniteField(2, 2, new int[] { 1, 1, 1 });
            var element1 = new FiniteFieldElements(new int[] { 1, 1 }, GF4);
            var inverse = element1.Inverse();
            Assert.That(inverse.coef, Is.EqualTo(new int[] { 0, 1 }));
        }

        [Test]
        public void Divide1()
        {
            var GF4 = new FiniteField(2, 2, new int[] { 1, 1, 1 });
            var element1 = new FiniteFieldElements(new int[] { 1, 1 }, GF4);
            var element2 = new FiniteFieldElements(new int[] { 1 }, GF4);
            var div = element1 / element2;
            Assert.That(div.coef, Is.EqualTo(new int[] { 1, 1 }));
        }

        [Test]
        public void Substr1()
        {
            var GF9 = new FiniteField(3, 2, new int[] { 1, 1, 2 });
            var element1 = new FiniteFieldElements(new int[] { 1, 2 }, GF9);
            var element2 = new FiniteFieldElements(new int[] { 2, 0 }, GF9);
            var substract = element1 - element2;
            Assert.That(substract.coef, Is.EqualTo(new int[] { 2, 2 }));
        }
        [Test]
        public void Divide2()
        {
            var GF8 = new FiniteField(2, 3, new int[] { 1, 1, 0, 1 });
            var element1 = new FiniteFieldElements(new int[] { 1, 0, 0, 1 }, GF8);
            var element2 = new FiniteFieldElements(new int[] { 1, 1 }, GF8);
            var divide = element1 / element2;
            Assert.That(divide.coef, Is.EqualTo(new int[] { 1, 1, 1 }));

        }
    }
}