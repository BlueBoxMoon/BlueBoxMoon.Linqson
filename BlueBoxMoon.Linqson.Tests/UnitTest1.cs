using System.Collections.Generic;
using System.Linq;
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

        [Test]
        public void TestAllEnumerable()
        {
            var type = typeof( Enumerable );

            foreach ( var expectedMethodInfo in type.GetMethods() )
            {

                var helper = new RealTypeSignatureHelper();
                var signature = helper.GetSignatureFromMethodInfo( expectedMethodInfo );
                var actualMethodInfo = helper.GetMethodInfoFromSignature( signature );

                Assert.NotNull( actualMethodInfo, signature );
                Assert.AreEqual( expectedMethodInfo.ToString(), actualMethodInfo.ToString(), signature );
            }
        }

        [Test]
        public void Test3()
        {
            var type = typeof( Enumerable );
            var expectedMethodInfo = type.GetMethods()
                .Where( a => a.Name == "SelectMany" )
                .Skip( 0 )
                .First();
            var expectedType = expectedMethodInfo.GetParameters()[2].ParameterType;

            var helper = new RealTypeSignatureHelper();
            var signature = helper.GetSignatureFromType( expectedType );
            var actualType = helper.GetTypeFromSignature( signature );

            Assert.True( helper.AreTypesEqual( expectedType, actualType ) );
        }
    }

    public static class TestMethods
    {
        public static void TestMethod1( IEnumerable<string> _ )
        {
        }
    }
}