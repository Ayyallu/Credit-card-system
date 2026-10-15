using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Tests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void CheckRangeOverlaps_RangesOverlap_ReturnsTrue()
        {
            bool result = Utils.CheckRangeOverlaps(10, 20, 15, 25);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRangeOverlaps_RangesAdjacentNotOverlapping_ReturnsFalse()
        {
            bool result = Utils.CheckRangeOverlaps(10, 20, 20, 30);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRangeOverlaps_RangesDisjoint_ReturnsFalse()
        {
            bool result = Utils.CheckRangeOverlaps(10, 20, 30, 40);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRangeOverlaps_SecondRangeFullyInsideFirst_ReturnsTrue()
        {
            bool result = Utils.CheckRangeOverlaps(0, 100, 40, 60);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetRecordStatus_UnknownCode_ReturnsEmptyString()
        {
            string result = Utils.GetRecordStatus("NOT_A_REAL_CODE");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void GetRecordStatus_KnownCode_ReturnsNonEmptyString()
        {
            string result = Utils.GetRecordStatus("A");
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}
