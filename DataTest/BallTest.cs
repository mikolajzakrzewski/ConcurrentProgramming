using System.Numerics;
using Data;

namespace DataTest;

[TestClass]
public class BallTest
{
    [TestMethod]
    public void CreateApiTest()
    {
        var dataApi = DataApi.Instance(new Vector2(6, 9), 10);
        Assert.IsNotNull(dataApi);
    }
}