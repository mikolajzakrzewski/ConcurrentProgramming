using Data;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void CreateAPITest()
        {
            DataAPI dataAPI = DataAPI.Instance(new System.Numerics.Vector2(6, 9), 10, 10);
            Assert.IsNotNull(dataAPI);
        }
    }
}