using System;
using System.Linq.Expressions;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class MethodCallExpressionTests
    {
        [Test]
        public void CallSafeMethod()
        {
            Expression<Action<MethodCallTest>> expected = ( a ) => a.AddOne();

            var encoded = EncodedExpression.EncodeExpression( expected );
            var decodeOptions = new DecodeOptions();
            decodeOptions.SafeMethods.Add( typeof( MethodCallTest ).GetMethod( nameof( MethodCallTest.AddOne ) ) );
            var actual = ( Expression<Action<MethodCallTest>> ) EncodedExpression.DecodeExpression( encoded, decodeOptions );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var testObject = new MethodCallTest();
            expected.Compile().Invoke( testObject );
            Assert.AreEqual( 1, testObject.Value );

            testObject = new MethodCallTest();
            actual.Compile().Invoke( testObject );
            Assert.AreEqual( 1, testObject.Value );
        }

        [Test]
        public void CallUnsafeMethod()
        {
            Expression<Action<MethodCallTest>> expected = ( a ) => a.AddOne();

            var encoded = EncodedExpression.EncodeExpression( expected );

            Assert.Throws( typeof( UnsafeMethodCallException ), () => EncodedExpression.DecodeExpression( encoded ) );
        }

        #region Test Classes

        private class MethodCallTest
        {
            public int Value { get; set; }

            public void AddOne()
            {
                Value += 1;
            }
        }

        #endregion
    }
}
