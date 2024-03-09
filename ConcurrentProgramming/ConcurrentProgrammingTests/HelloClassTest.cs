using ConcurrentProgramming;

namespace ConcurrentProgrammingTests
{
    public class HelloClassTest
    {
        [Test]
        public void sumTest()
        {
            int a = 10;
            int b = 20;

            int result = HelloClass.sum(a, b);

            Assert.AreEqual(30, result);
        }
    }
}