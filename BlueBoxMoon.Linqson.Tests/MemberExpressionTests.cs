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
