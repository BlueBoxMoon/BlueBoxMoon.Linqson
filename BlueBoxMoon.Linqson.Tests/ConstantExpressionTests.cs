using System.Linq.Expressions;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class ConstantExpressionTests
    {
        private static readonly object[] PrimitiveValues =
        {
            true,           // bool
            ( byte ) 2,
            ( sbyte ) -3,
            'a',            // char
            5.837M,         // decimal
            6.837d,         // double
            7.837f,         // float
            -8,             // int
            ( uint ) 9,
            ( long ) -10,
            ( ulong ) 11,
            ( short ) -12,
            ( ushort ) 13,
            "b"             // string
        };

        [Test]
        public void EncodeDecodePrimitives( [ValueSource( nameof( PrimitiveValues ) )] object expectedValue )
        {
            var expected = Expression.Constant( expectedValue, expectedValue.GetType() );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( ConstantExpression ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedValue, actual.Value );
        }
    }
}
