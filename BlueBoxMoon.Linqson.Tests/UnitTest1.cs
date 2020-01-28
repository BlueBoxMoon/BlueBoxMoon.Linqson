using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

            var helper = new TypeSignatureHelper();
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

                var helper = new TypeSignatureHelper();
                var signature = helper.GetSignatureFromMethodInfo( expectedMethodInfo );
                MethodInfo actualMethodInfo = null;

                Assert.DoesNotThrow( () =>
                {
                    actualMethodInfo = helper.GetMethodInfoFromSignature( signature );
                }, signature );

                Assert.NotNull( actualMethodInfo, signature );
                Assert.AreEqual( expectedMethodInfo.ToString(), actualMethodInfo.ToString(), signature );
            }
        }
    }

    public static class TestMethods
    {
        public static void TestMethod1( IEnumerable<string> _ )
        {
        }
    }

    public class TestObject
    {
        public int FieldValue;

        public int PropertyValue { get; set; }
    }
}