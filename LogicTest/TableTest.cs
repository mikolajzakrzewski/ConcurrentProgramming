using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace LogicTest
{
    [TestClass]
    public class TableTest
    {
        [TestMethod]
        public void TestCreateBalls()
        {
            LogicAPI table = LogicAPI.Instance(1000, 1000);
            table.CreateBalls(15, 20);
            Assert.AreEqual(15, table.Balls.Count);
        }

        [TestMethod]
        public void TestResetTable()
        {
            LogicAPI table = LogicAPI.Instance(1000, 1000);
            table.CreateBalls(15, 20);
            table.ResetTable();
            Assert.AreEqual(0, table.Balls.Count);
        }
    }
}
