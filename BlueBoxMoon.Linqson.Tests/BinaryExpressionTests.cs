using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    // TODO: Need to handle all the Assign variations.
    public class BinaryExpressionTests
    {
        [Test]
        public void Add()
        {
            var expected = Expression.AddAssign( Expression.Constant( 2 ), Expression.Constant( 3 ) );
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
            var expected = Expression.ExclusiveOr( Expression.Constant( 8 ), Expression.Constant( 1 ) );
            var encoded = EncodedExpression.EncodeExpression( expected );
            var actual = EncodedExpression.DecodeExpression( encoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var expectedLambda = Expression.Lambda<Func<int>>( expected );
            var actualLambda = Expression.Lambda<Func<int>>( actual );

            Assert.AreEqual( expectedLambda.Compile().Invoke(), actualLambda.Compile().Invoke() );
            Assert.AreEqual( 9, actualLambda.Compile().Invoke() );
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
