using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverterTests
{
    [TestFixture]
    public class UnitTest1
    {
        public UnitTest1()
        {
        }
        [Test]
        public void FizzBizzPass()
        {
            var ExpectedResult = "Fizz";
            var originalResult = string.Empty;
            //originalResult = fbObj.checkFizzBuzz(6);
            Assert.AreEqual(ExpectedResult, originalResult);

        }
        [Test]
        public void FizzBizzFail()
        {
            var ExpectedResult = "FizzBizz";
            var originalResult = string.Empty;
            //originalResult = fbObj.checkFizzBuzz(6);
            Assert.AreEqual(ExpectedResult, originalResult);

        }
        [Test]
        public void DecimalToBinaryPass()
        {
            var ExpectedResult = "1000";
            var originalResult = string.Empty;
            //originalResult = obj.getDecimalNumber(8);
            Assert.AreEqual(ExpectedResult, originalResult);

        }
        [Test]
        public void DecimalToBinaryFail()
        {
            var ExpectedResult = "1001";
            var originalResult = string.Empty;
            //originalResult = obj.getDecimalNumber(8);
            Assert.That(ExpectedResult, Is.EqualTo(originalResult));

        }
        [Test]
        public void FibonacciPass()
        {
            List<int> ExpectedResult = new List<int>();
            //var result = fObj.GetFibonacci(1);
            ExpectedResult.Add(0);
            ExpectedResult.Add(1);
            ExpectedResult.Add(1);
            //CollectionAssert.AreEqual(ExpectedResult, result);
        }
        [Test]
        public void FibonacciFail()
        {
            List<int> ExpectedResult = new List<int>();
            //var result = fObj.GetFibonacci(1);
            ExpectedResult.Add(0);
            ExpectedResult.Add(1);
            ExpectedResult.Add(1);
            ExpectedResult.Add(2);
           // CollectionAssert.AreEqual(ExpectedResult, result);
        }
    }
}
