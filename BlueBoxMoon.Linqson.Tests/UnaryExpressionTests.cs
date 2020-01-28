using System;
using System.Linq.Expressions;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class UnaryExpressionTests
    {
        [Test]
        public void Convert()
        {
            var expected = Expression.Convert( Expression.Constant( 6 ), typeof( byte ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<byte>>( expected );
            var actualLambda = Expression.Lambda<Func<byte>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( ( byte ) 6, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void ConvertChecked()
        {
            var expected = Expression.ConvertChecked( Expression.Constant( 6 ), typeof( byte ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<byte>>( expected );
            var actualLambda = Expression.Lambda<Func<byte>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( ( byte ) 6, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void ConvertChecked_Overflow()
        {
            var expected = Expression.ConvertChecked( Expression.Constant( 600 ), typeof( byte ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<byte>>( expected );
            var actualLambda = Expression.Lambda<Func<byte>>( actual );

            Assert.Throws( typeof( OverflowException ), () => expectedLambda.Compile().Invoke() );
            Assert.Throws( typeof( OverflowException ), () => actualLambda.Compile().Invoke() );
        }
    }
}
