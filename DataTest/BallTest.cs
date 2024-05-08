using Data;
using System.Numerics;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void CreateAPITest()
        {
            DataAPI dataAPI = DataAPI.Instance(new Vector2(6, 9), 10, 10);
            Assert.IsNotNull(dataAPI);
        }
    }
}