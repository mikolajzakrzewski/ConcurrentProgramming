using System.Numerics;
using Data;

namespace DataTest;

[TestClass]
public class BallTest
{
    [TestMethod]
    public void CreateApiTest()
    {
        var dataApi = DataApi.Instance(new Vector2(1, 1), 1, 1, new Random());
        Assert.IsNotNull(dataApi);
    }
}