using System.Collections.Generic;
using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var type = typeof( TestMethods );
            var expectedMethodInfo = type.GetMethod( "TestMethod1" );

            var helper = new RealTypeSignatureHelper();
            var signature = helper.GetSignatureFromMethodInfo( expectedMethodInfo );
            var actualMethodInfo = helper.GetMethodInfoFromSignature( signature );

            Assert.AreEqual( expectedMethodInfo.ToString(), actualMethodInfo.ToString() );
        }
    }

    public static class TestMethods
    {
        public static void TestMethod1( IEnumerable<string> _ )
        {
        }
    }
}