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
    public class BinaryExpressionTests
    {
        [Test]
        public void Add()
        {
            var expected = Expression.Add( Expression.Constant( 2 ), Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 5, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Add_Overflow()
        {
            var expected = Expression.Add( Expression.Constant( ( short ) 30000 ), Expression.Constant( ( short ) 10000 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<short>>( expected );
            var actualLambda = Expression.Lambda<Func<short>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( ( short ) -25536, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void AddAssign()
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
            var assignExpr = Expression.AddAssign( propExpr, Expression.Constant( 10 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 6;
            Assert.AreEqual( 16, actualFn( actualObject ) );
            Assert.AreEqual( 16, actualObject.PropertyValue );
        }

        [Test]
        public void AddAssign_Overflow()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 2000000000
            };
            var actualObject = new TestObject
            {
                PropertyValue = 2000000000
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.AddAssign( propExpr, Expression.Constant( 1000000000 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 2000000000;
            Assert.AreEqual( -1294967296, actualFn( actualObject ) );
            Assert.AreEqual( -1294967296, actualObject.PropertyValue );
        }

        [Test]
        public void AddAssignChecked()
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
            var assignExpr = Expression.AddAssignChecked( propExpr, Expression.Constant( 10 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 6;
            Assert.AreEqual( 16, actualFn( actualObject ) );
            Assert.AreEqual( 16, actualObject.PropertyValue );
        }

        [Test]
        public void AddAssignChecked_Overflow()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 2000000000
            };
            var actualObject = new TestObject
            {
                PropertyValue = 2000000000
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.AddAssignChecked( propExpr, Expression.Constant( 1000000000 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.Throws( typeof( OverflowException ), () => expectedFn.Invoke( expectedObject ) );
            Assert.Throws( typeof( OverflowException ), () => actualFn.Invoke( actualObject ) );
        }

        [Test]
        public void AddChecked()
        {
            var expected = Expression.AddChecked( Expression.Constant( 2 ), Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 5, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void AddChecked_Overflow()
        {
            var expected = Expression.AddChecked( Expression.Constant( ( short ) 30000 ), Expression.Constant( ( short ) 10000 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<short>>( expected );
            var actualLambda = Expression.Lambda<Func<short>>( actual );

            Assert.Throws( typeof( OverflowException ), () => expectedLambda.Compile().Invoke() );
            Assert.Throws( typeof( OverflowException ), () => actualLambda.Compile().Invoke() );
        }

        [Test]
        public void And()
        {
            var expected = Expression.And( Expression.Constant( 9 ), Expression.Constant( 8 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 8, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void AndAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 9
            };
            var actualObject = new TestObject
            {
                PropertyValue = 9
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.AndAssign( propExpr, Expression.Constant( 1 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 9;
            Assert.AreEqual( 1, actualFn( actualObject ) );
            Assert.AreEqual( 1, actualObject.PropertyValue );
        }

        [Test]
        public void AndAlso_IsTrue()
        {
            var expected = Expression.AndAlso( Expression.Constant( true ), Expression.Constant( true ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void AndAlso_IsFalse()
        {
            var expected = Expression.AndAlso( Expression.Constant( true ), Expression.Constant( false ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Assign()
        {
            var testObject = new TestObject
            {
                PropertyValue = 6
            };
            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var expected = Expression.Assign( propExpr, Expression.Constant( 10 ) );
            var expectedLambda = Expression.Lambda<Func<TestObject, int>>( expected, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expectedLambda );
            var actualLambda = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expectedLambda.ToString(), actualLambda.ToString() );

            Assert.AreEqual( expectedLambda.Compile().Invoke( testObject ), actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 10, actualLambda.Compile().Invoke( testObject ) );
            Assert.AreEqual( 10, testObject.PropertyValue );
        }

        [Test]
        public void Coalesce()
        {
            var left = Expression.Convert( Expression.Constant( 6 ), typeof( int? ) );
            var expected = Expression.Coalesce( left, Expression.Constant( 10 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 6, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Coalesce_WithNull()
        {
            var left = Expression.Convert( Expression.Constant( null ), typeof( int? ) );
            var expected = Expression.Coalesce( left, Expression.Constant( 10 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 10, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Coalesce_WithConversion()
        {
            Expression<Func<int, int>> fn = ( a ) => a + 3;
            var left = Expression.Convert( Expression.Constant( 6 ), typeof( int? ) );
            var expected = Expression.Coalesce( left, Expression.Constant( 10 ), fn );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 9, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Divide()
        {
            var expected = Expression.Divide( Expression.Constant( 6 ), Expression.Constant( 2 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 3, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void DivideAssign()
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
            var assignExpr = Expression.DivideAssign( propExpr, Expression.Constant( 2 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 6;
            Assert.AreEqual( 3, actualFn( actualObject ) );
            Assert.AreEqual( 3, actualObject.PropertyValue );
        }

        [Test]
        public void Equal_IsTrue()
        {
            var expected = Expression.Equal( Expression.Constant( 3 ), Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Equal_IsFalse()
        {
            var expected = Expression.Equal( Expression.Constant( 4 ), Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void ExclusiveOr()
        {
            var expected = Expression.ExclusiveOr( Expression.Constant( 9 ), Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 10, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void ExclusiveOrAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 9
            };
            var actualObject = new TestObject
            {
                PropertyValue = 9
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.ExclusiveOrAssign( propExpr, Expression.Constant( 3 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 9;
            Assert.AreEqual( 10, actualFn( actualObject ) );
            Assert.AreEqual( 10, actualObject.PropertyValue );
        }

        [Test]
        public void GreaterThan_IsGreater()
        {
            var expected = Expression.GreaterThan( Expression.Constant( 8 ), Expression.Constant( 1 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void GreaterThan_IsNotGreater()
        {
            var expected = Expression.GreaterThan( Expression.Constant( 1 ), Expression.Constant( 8 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void GreaterThanOrEqual_IsGreater()
        {
            var expected = Expression.GreaterThanOrEqual( Expression.Constant( 8 ), Expression.Constant( 1 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void GreaterThanOrEqual_IsNotGreater()
        {
            var expected = Expression.GreaterThanOrEqual( Expression.Constant( 1 ), Expression.Constant( 8 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void GreaterThanOrEqual_IsEqual()
        {
            var expected = Expression.GreaterThanOrEqual( Expression.Constant( 8 ), Expression.Constant( 8 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void LeftShift()
        {
            var expected = Expression.LeftShift( Expression.Constant( 1 ), Expression.Constant( 4 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 16, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void LeftShiftAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 1
            };
            var actualObject = new TestObject
            {
                PropertyValue = 1
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.LeftShiftAssign( propExpr, Expression.Constant( 4 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 1;
            Assert.AreEqual( 16, actualFn( actualObject ) );
            Assert.AreEqual( 16, actualObject.PropertyValue );
        }

        [Test]
        public void LessThan_IsLess()
        {
            var expected = Expression.LessThan( Expression.Constant( 1 ), Expression.Constant( 8 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void LessThan_IsNotLess()
        {
            var expected = Expression.LessThan( Expression.Constant( 8 ), Expression.Constant( 1 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void LessThanOrEqual_IsLess()
        {
            var expected = Expression.LessThanOrEqual( Expression.Constant( 1 ), Expression.Constant( 8 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void LessThanOrEqual_IsNotLess()
        {
            var expected = Expression.LessThanOrEqual( Expression.Constant( 8 ), Expression.Constant( 1 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void LessThanOrEqual_IsEqual()
        {
            var expected = Expression.LessThanOrEqual( Expression.Constant( 8 ), Expression.Constant( 8 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Modulo()
        {
            var expected = Expression.Modulo( Expression.Constant( 10 ), Expression.Constant( 4 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 2, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void ModuloAssign()
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
            var assignExpr = Expression.ModuloAssign( propExpr, Expression.Constant( 4 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 6;
            Assert.AreEqual( 2, actualFn( actualObject ) );
            Assert.AreEqual( 2, actualObject.PropertyValue );
        }

        [Test]
        public void Multiply()
        {
            var expected = Expression.Multiply( Expression.Constant( 3 ), Expression.Constant( 2 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 6, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Multiply_Overflow()
        {
            var expected = Expression.Multiply( Expression.Constant( ( short ) 10000 ), Expression.Constant( ( short ) 4 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<short>>( expected );
            var actualLambda = Expression.Lambda<Func<short>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( ( short ) -25536, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void MultiplyAssign()
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
            var assignExpr = Expression.MultiplyAssign( propExpr, Expression.Constant( 2 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 6;
            Assert.AreEqual( 12, actualFn( actualObject ) );
            Assert.AreEqual( 12, actualObject.PropertyValue );
        }

        [Test]
        public void MultiplyAssign_Overflow()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 2000000000
            };
            var actualObject = new TestObject
            {
                PropertyValue = 2000000000
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.MultiplyAssign( propExpr, Expression.Constant( 2 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 2000000000;
            Assert.AreEqual( -294967296, actualFn( actualObject ) );
            Assert.AreEqual( -294967296, actualObject.PropertyValue );
        }

        [Test]
        public void MultiplyAssignChecked()
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
            var assignExpr = Expression.MultiplyAssignChecked( propExpr, Expression.Constant( 2 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 6;
            Assert.AreEqual( 12, actualFn( actualObject ) );
            Assert.AreEqual( 12, actualObject.PropertyValue );
        }

        [Test]
        public void MultiplyAssignChecked_Overflow()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 2000000000
            };
            var actualObject = new TestObject
            {
                PropertyValue = 2000000000
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.MultiplyAssignChecked( propExpr, Expression.Constant( 2 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.Throws( typeof( OverflowException ), () => expectedFn.Invoke( expectedObject ) );
            Assert.Throws( typeof( OverflowException ), () => actualFn.Invoke( actualObject ) );
        }

        [Test]
        public void MultiplyChecked()
        {
            var expected = Expression.MultiplyChecked( Expression.Constant( 3 ), Expression.Constant( 2 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 6, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void MultiplyChecked_Overflow()
        {
            var expected = Expression.MultiplyChecked( Expression.Constant( ( short ) 10000 ), Expression.Constant( ( short ) 7 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<short>>( expected );
            var actualLambda = Expression.Lambda<Func<short>>( actual );

            Assert.Throws( typeof( OverflowException ), () => expectedLambda.Compile().Invoke() );
            Assert.Throws( typeof( OverflowException ), () => actualLambda.Compile().Invoke() );
        }

        [Test]
        public void NotEqual_IsTrue()
        {
            var expected = Expression.NotEqual( Expression.Constant( 4 ), Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void NotEqual_IsFalse()
        {
            var expected = Expression.NotEqual( Expression.Constant( 3 ), Expression.Constant( 3 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Or()
        {
            var expected = Expression.Or( Expression.Constant( 8 ), Expression.Constant( 1 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 9, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void OrAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 8
            };
            var actualObject = new TestObject
            {
                PropertyValue = 8
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.OrAssign( propExpr, Expression.Constant( 1 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 8;
            Assert.AreEqual( 9, actualFn( actualObject ) );
            Assert.AreEqual( 9, actualObject.PropertyValue );
        }

        [Test]
        public void OrElse_IsTrue()
        {
            var expected = Expression.OrElse( Expression.Constant( false ), Expression.Constant( true ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( true, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void OrElse_IsFalse()
        {
            var expected = Expression.OrElse( Expression.Constant( false ), Expression.Constant( false ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<bool>>( expected );
            var actualLambda = Expression.Lambda<Func<bool>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( false, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Power()
        {
            var expected = Expression.Power( Expression.Constant( 2d ), Expression.Constant( 3d ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<double>>( expected );
            var actualLambda = Expression.Lambda<Func<double>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 8d, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void PowerAssign()
        {
            var expectedObject = new TestObject
            {
                DoubleValue = 2d
            };
            var actualObject = new TestObject
            {
                DoubleValue = 2d
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "DoubleValue" );
            var assignExpr = Expression.PowerAssign( propExpr, Expression.Constant( 4d ) );
            var expected = Expression.Lambda<Func<TestObject, double>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, double>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.DoubleValue, actualObject.DoubleValue );

            actualObject.DoubleValue = 2d;
            Assert.AreEqual( 16d, actualFn( actualObject ) );
            Assert.AreEqual( 16d, actualObject.DoubleValue );
        }

        [Test]
        public void RightShift()
        {
            var expected = Expression.RightShift( Expression.Constant( 16 ), Expression.Constant( 4 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 1, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void RightShiftAssign()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = 16
            };
            var actualObject = new TestObject
            {
                PropertyValue = 16
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.RightShiftAssign( propExpr, Expression.Constant( 4 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 16;
            Assert.AreEqual( 1, actualFn( actualObject ) );
            Assert.AreEqual( 1, actualObject.PropertyValue );
        }

        [Test]
        public void Subtract()
        {
            var expected = Expression.Subtract( Expression.Constant( 3 ), Expression.Constant( 2 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 1, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void Subtract_Overflow()
        {
            var expected = Expression.Subtract( Expression.Constant( ( short ) -30000 ), Expression.Constant( ( short ) 10000 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<short>>( expected );
            var actualLambda = Expression.Lambda<Func<short>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( ( short ) 25536, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void SubtractAssign()
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
            var assignExpr = Expression.SubtractAssign( propExpr, Expression.Constant( 10 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 6;
            Assert.AreEqual( -4, actualFn( actualObject ) );
            Assert.AreEqual( -4, actualObject.PropertyValue );
        }

        [Test]
        public void SubtractAssign_Overflow()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = -2000000000
            };
            var actualObject = new TestObject
            {
                PropertyValue = -2000000000
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.SubtractAssign( propExpr, Expression.Constant( 1000000000 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = -2000000000;
            Assert.AreEqual( 1294967296, actualFn( actualObject ) );
            Assert.AreEqual( 1294967296, actualObject.PropertyValue );
        }

        [Test]
        public void SubtractAssignChecked()
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
            var assignExpr = Expression.SubtractAssignChecked( propExpr, Expression.Constant( 10 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.AreEqual( expectedFn( expectedObject ), actualFn( actualObject ) );
            Assert.AreEqual( expectedObject.PropertyValue, actualObject.PropertyValue );

            actualObject.PropertyValue = 6;
            Assert.AreEqual( -4, actualFn( actualObject ) );
            Assert.AreEqual( -4, actualObject.PropertyValue );
        }

        [Test]
        public void SubtractAssignChecked_Overflow()
        {
            var expectedObject = new TestObject
            {
                PropertyValue = -2000000000
            };
            var actualObject = new TestObject
            {
                PropertyValue = -2000000000
            };

            var objExpr = Expression.Parameter( typeof( TestObject ), "p" );
            var propExpr = Expression.Property( objExpr, "PropertyValue" );
            var assignExpr = Expression.SubtractAssignChecked( propExpr, Expression.Constant( 1000000000 ) );
            var expected = Expression.Lambda<Func<TestObject, int>>( assignExpr, objExpr );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = ( Expression<Func<TestObject, int>> ) EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedFn = expected.Compile();
            var actualFn = actual.Compile();

            Assert.Throws( typeof( OverflowException ), () => expectedFn.Invoke( expectedObject ) );
            Assert.Throws( typeof( OverflowException ), () => actualFn.Invoke( actualObject ) );
        }

        [Test]
        public void SubtractChecked()
        {
            var expected = Expression.SubtractChecked( Expression.Constant( 3 ), Expression.Constant( 2 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 1, actualLambda.Compile().Invoke() );
        }

        [Test]
        public void SubtractChecked_Overflow()
        {
            var expected = Expression.SubtractChecked( Expression.Constant( ( short ) -30000 ), Expression.Constant( ( short ) 10000 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<short>>( expected );
            var actualLambda = Expression.Lambda<Func<short>>( actual );

            Assert.Throws( typeof( OverflowException ), () => expectedLambda.Compile().Invoke() );
            Assert.Throws( typeof( OverflowException ), () => actualLambda.Compile().Invoke() );
        }
    }
}
