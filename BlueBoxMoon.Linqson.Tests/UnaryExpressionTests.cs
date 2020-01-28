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

        [Test]
        public void Decrement()
        {
            var testObject = new TestObject
            {
                PropertyValue = 6
            };
            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var expected = Expression.Decrement( propExpr );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( testObject ), actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 5, actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 6, testObject.PropertyValue );
        }

        [Test]
        public void Increment()
        {
            var testObject = new TestObject
            {
                PropertyValue = 6
            };
            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var expected = Expression.Increment( propExpr );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( testObject ), actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 7, actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 6, testObject.PropertyValue );
        }

        [Test]
        public void IsFalse_IsTrue()
        {
            var expected = Expression.IsFalse( Expression.Constant( false ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void IsFalse_IsFalse()
        {
            var expected = Expression.IsFalse( Expression.Constant( true ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void IsTrue_IsTrue()
        {
            var expected = Expression.IsTrue( Expression.Constant( true ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void IsTrue_IsFalse()
        {
            var expected = Expression.IsTrue( Expression.Constant( false ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Negate()
        {
            var expected = Expression.Negate( Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( -3, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void NegateChecked()
        {
            var expected = Expression.NegateChecked( Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( -3, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Not()
        {
            var expected = Expression.Not( Expression.Constant( false ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void OnesComplement()
        {
            var expected = Expression.OnesComplement( Expression.Constant( 127 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( -128, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void PreDecrementAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 6
            };

            var actualObject = new TestObject
            {
                PropertyValue = 6
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var expected = Expression.PreDecrementAssign( propExpr );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( expectedObject ), actualLambda.Compile().Invoke( actualObject ) );

            actualObject.PropertyValue = 6; // Reset for next test.

            Assert.AreEqual( 5, actualLambda.Compile().Invoke( actualObject ) );
            Assert.AreEqual( 5, expectedObject.PropertyValue );
        }

        [Test]
        public void PreIncrementAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 6
            };

            var actualObject = new TestObject
            {
                PropertyValue = 6
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var expected = Expression.PreIncrementAssign( propExpr );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( expectedObject ), actualLambda.Compile().Invoke( actualObject ) );

            actualObject.PropertyValue = 6; // Reset for next test.

            Assert.AreEqual( 7, actualLambda.Compile().Invoke( actualObject ) );
            Assert.AreEqual( 7, expectedObject.PropertyValue );
        }

        [Test]
        public void PostDecrementAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 6
            };

            var actualObject = new TestObject
            {
                PropertyValue = 6
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var expected = Expression.PostDecrementAssign( propExpr );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( expectedObject ), actualLambda.Compile().Invoke( actualObject ) );

            actualObject.PropertyValue = 6; // Reset for next test.

            Assert.AreEqual( 6, actualLambda.Compile().Invoke( actualObject ) );
            Assert.AreEqual( 5, expectedObject.PropertyValue );
        }

        [Test]
        public void PostIncrementAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 6
            };

            var actualObject = new TestObject
            {
                PropertyValue = 6
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var expected = Expression.PostIncrementAssign( propExpr );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( expectedObject ), actualLambda.Compile().Invoke( actualObject ) );

            actualObject.PropertyValue = 6; // Reset for next test.

            Assert.AreEqual( 6, actualLambda.Compile().Invoke( actualObject ) );
            Assert.AreEqual( 7, expectedObject.PropertyValue );
        }

    }
}
