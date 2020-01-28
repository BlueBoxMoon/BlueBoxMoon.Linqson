using System;
using System.Linq.Expressions;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class LambdaExpressionTests
    {
        [Test]
        public void Lambda()
        {
            Expression<Func<int>> expected = () => 3 + 3;
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            Assert.AreEqual( expected.Compile().Invoke(), actual.Compile().Invoke() );
            Assert.AreEqual( 6, actual.Compile().Invoke() );
        }
    }
}
