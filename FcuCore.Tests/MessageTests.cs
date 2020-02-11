using System.Web;
using FcuCore.Communications;
using FcuCore.Communications.Opcodes;
using NUnit.Framework;

namespace Tests
{
    public class MessageTests
    {
        [Test]
        public void IsCanIdStoredAndRetrievedCorrectly()
        {
            var c = new CbusMessage(CbusOpCodes.AcOn) {
                CanId = 125
            };
            Assert.AreEqual(125, c.CanId);
        }

        [Test]
        public void IsMajorPriorityStoredAndRetrievedCorrectly()
        {
            var c = new CbusMessage(CbusOpCodes.AcOn) {MajorPriority = MajorPriority.Low};
            Assert.AreEqual(MajorPriority.Low, c.MajorPriority);
            c.MajorPriority = MajorPriority.Medium;
            Assert.AreEqual(MajorPriority.Medium, c.MajorPriority);
            c.MajorPriority = MajorPriority.High;
            Assert.AreEqual(MajorPriority.High, c.MajorPriority);
        }

        [Test]
        public void IsMinorPriorityStoredAndRetrievedCorrectly()
        {
            var c = new CbusMessage(CbusOpCodes.AcOn) {
                MinorPriority = MinorPriority.Low
            };
            Assert.AreEqual(MinorPriority.Low, c.MinorPriority);
            c.MinorPriority = MinorPriority.Normal;
            Assert.AreEqual(MinorPriority.Normal, c.MinorPriority);
            c.MinorPriority = MinorPriority.AboveNormal;
            Assert.AreEqual(MinorPriority.AboveNormal, c.MinorPriority);
            c.MinorPriority = MinorPriority.High;
            Assert.AreEqual(MinorPriority.High, c.MinorPriority);
        }

        [Test]
        public void EnsurePriorityAndCanIdPlayNicely()
        {
            var c = new CbusMessage(CbusOpCodes.AcOn) {
                CanId = 125,
                MajorPriority = MajorPriority.Medium,
                MinorPriority = MinorPriority.AboveNormal
            };
            Assert.AreEqual(125, c.CanId);
            Assert.AreEqual(MajorPriority.Medium, c.MajorPriority);
            Assert.AreEqual(MinorPriority.AboveNormal, c.MinorPriority);
            c.CanId = 99;
            Assert.AreEqual(99, c.CanId);
            Assert.AreEqual(MajorPriority.Medium, c.MajorPriority);
            Assert.AreEqual(MinorPriority.AboveNormal, c.MinorPriority);
            c.MajorPriority = MajorPriority.High;
            Assert.AreEqual(99, c.CanId);
            Assert.AreEqual(MajorPriority.High, c.MajorPriority);
            Assert.AreEqual(MinorPriority.AboveNormal, c.MinorPriority);
            c.MinorPriority = MinorPriority.Low;
            Assert.AreEqual(99, c.CanId);
            Assert.AreEqual(MajorPriority.High, c.MajorPriority);
            Assert.AreEqual(MinorPriority.Low, c.MinorPriority);
        }

        [Test]
        public void EnsureTransportStringIsRoundTrippedCorrectly()
        {
            var c = CbusMessage.FromTransportString(":SB020N9101000005;");
            Assert.AreEqual(":SB020N9101000005;", c.TransportString);

            c = CbusMessage.FromTransportString(":SB040NB60101A5050F;");
            Assert.AreEqual(":SB040NB60101A5050F;", c.TransportString);
        }
    }
}