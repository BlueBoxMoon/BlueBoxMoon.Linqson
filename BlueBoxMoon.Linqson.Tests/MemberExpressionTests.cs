using System;
using System.Linq.Expressions;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class MemberExpressionTests
    {
        [Test]
        public void MemberAccessProperty()
        {
            var testObject = new TestObject
            {
                PropertyValue = 6
            };
            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var expected = Expression.Property( objExpr, "PropertyValue" );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( testObject ), actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 6, actualLambda.Compile().Invoke( testObject ) );
        }

        [Test]
        public void MemberAccessField()
        {
            var testObject = new TestObject
            {
                FieldValue = 6
            };
            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var expected = Expression.Field( objExpr, "FieldValue" );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( testObject ), actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 6, actualLambda.Compile().Invoke( testObject ) );
        }
    }
}
