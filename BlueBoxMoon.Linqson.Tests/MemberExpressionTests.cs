// MIT License
//
// Copyright( c) 2020 Blue Box Moon
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class MemberExpressionTests
    {
        [Test]
        public void IsAnonymousType_Fail()
        {
            var anonObject = new { Value = 4 };

            var objExpr = Expression.Parameter( anonObject.GetType(), "p" );
            var expected = Expression.Property( objExpr, "Value" );

            var ex = Assert.Throws<Exception>( () => EncodedExpression.EncodeExpression( expected ) );
            Assert.AreEqual( "Encoding member access of anonymous types is not supported.", ex.Message );
        }

        [Test]
        public void IsAnonymousType_GenericPass()
        {
            var anonObject = new List<string>();

            var objExpr = Expression.Parameter( anonObject.GetType(), "p" );
            var expected = Expression.Property( objExpr, "Count" );

            Assert.DoesNotThrow( () => EncodedExpression.EncodeExpression( expected ) );
        }

        [Test]
        public void MemberAccessField()
        {
            var testObject = new TestHelper
            {
                FieldValue = 6
            };
            var objExpr = Expression.Parameter( typeof( TestHelper ), "p" );
            var expected = Expression.Field( objExpr, "FieldValue" );
            var expectedLambda = Expression.Lambda<Func<TestHelper, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestHelper, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( testObject ), actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 6, actualLambda.Compile().Invoke( testObject ) );
        }

        [Test]
        public void MemberAccessProperty()
        {
            var testObject = new TestHelper
            {
                PropertyValue = 6
            };
            var objExpr = Expression.Parameter( typeof( TestHelper ), "p" );
            var expected = Expression.Property( objExpr, "PropertyValue" );
            var expectedLambda = Expression.Lambda<Func<TestHelper, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestHelper, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( testObject ), actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 6, actualLambda.Compile().Invoke( testObject ) );
        }

        #region Test Classes

        private class TestHelper
        {
            public int FieldValue;

            public int PropertyValue { get; set; }
        }

        #endregion
    }
}
