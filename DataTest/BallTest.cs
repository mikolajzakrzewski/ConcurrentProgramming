using Data;

namespace DataTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void CreateAPITest()
        {
            DataAPI dataAPI = DataAPI.Instance(10, 10, 10, 10);
            Assert.IsNotNull(dataAPI);
        }
    }
}