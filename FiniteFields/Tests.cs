using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteFields
{
    internal class Tests
    {
        [Test]
        public void Sum1()
        {
            var GF4 = new FiniteField(2, 2, new int[] { 1, 1, 1 });
            var element1 = new FiniteFieldElements(new int[] { 1, 1 }, GF4);
            var element2 = new FiniteFieldElements(new int[] { 0, 1 }, GF4);
            var sum = element1 + element2;
            Assert.That(sum.coef, Is.EqualTo(new int[] { 1, 0 }));
        }
        
    }   
}
