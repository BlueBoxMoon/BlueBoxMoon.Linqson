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
    public class MethodCallExpressionTests
    {
        [Test]
        public void CallAllowUnsafeMethod()
        {
            Expression<Action<MethodCallTest>> expected = ( a ) => a.AddOne();

            var encoded = EncodedExpression.EncodeExpression( expected );
            var decodeOptions = new DecodeOptions();
            decodeOptions.AllowUnsafeCalls = true;
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
        public void CallSafeInstanceMethod()
        {
            Expression<Action<MethodCallTest>> expected = ( a ) => a.AddOne();

            var encoded = EncodedExpression.EncodeExpression( expected );
            var decodeOptions = new DecodeOptions();
            decodeOptions.SafeInstanceMethodTypes.Add( typeof( MethodCallTest ) );
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
        public void CallSafeStaticMethod()
        {
            Expression<Func<int>> expected = () => MethodCallTest.GetOne();

            var encoded = EncodedExpression.EncodeExpression( expected );
            var decodeOptions = new DecodeOptions();
            decodeOptions.SafeStaticMethodTypes.Add( typeof( MethodCallTest ) );
            var actual = ( Expression<Func<int>> ) EncodedExpression.DecodeExpression( encoded, decodeOptions );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            Assert.AreEqual( 1, expected.Compile().Invoke() );
        }

        [Test]
        public void CallTwoParameterMethod()
        {
            Expression<Func<MethodCallTest, int>> expected = ( a ) => a.AddValues( 1, 2 );

            var encoded = EncodedExpression.EncodeExpression( expected );
            var decodeOptions = new DecodeOptions
            {
                AllowUnsafeCalls = true
            };
            var actual = ( Expression<Func<MethodCallTest, int>> ) EncodedExpression.DecodeExpression( encoded, decodeOptions );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            var testObject = new MethodCallTest();
            Assert.AreEqual( 3, expected.Compile().Invoke( testObject ) );

            testObject = new MethodCallTest();
            Assert.AreEqual( 3, actual.Compile().Invoke( testObject ) );
        }


        [Test]
        public void DecodeMissingMethodIsError()
        {
            var parameterExpr = Expression.Parameter( typeof( MethodCallTest ), "a" );
            var expected = Expression.Call( parameterExpr, typeof( MethodCallTest ).GetMethod( "AddOne" ) );

            var encoded = EncodedExpression.EncodeExpression( expected );
            encoded.Values["Method"] = encoded.Values["Method"].Replace( "AddOne", "MissingOne" );

            Assert.Throws<MethodNotFoundException>( () => EncodedExpression.DecodeExpression( encoded ) );
        }

        [Test]
        public void DecodeUnsafeInstanceMethodIsError()
        {
            var parameterExpr = Expression.Parameter( typeof( MethodCallTest ), "a" );
            var expected = Expression.Call( parameterExpr, typeof( MethodCallTest ).GetMethod( "AddOne" ) );

            var encoded = EncodedExpression.EncodeExpression( expected );

            var exception = Assert.Throws<UnsafeMethodCallException>( () => EncodedExpression.DecodeExpression( encoded ) );

            Assert.IsNotNull( exception.MethodInfo );

            Assert.AreEqual( nameof( MethodCallTest.AddOne ), exception.MethodInfo.Name );
        }

        [Test]
        public void DecodeUnsafeStaticMethodIsError()
        {
            var expected = Expression.Call( typeof( MethodCallTest ).GetMethod( "GetOne" ) );

            var encoded = EncodedExpression.EncodeExpression( expected );

            var exception = Assert.Throws<UnsafeMethodCallException>( () => EncodedExpression.DecodeExpression( encoded ) );

            Assert.IsNotNull( exception.MethodInfo );

            Assert.AreEqual( nameof( MethodCallTest.GetOne ), exception.MethodInfo.Name );
        }

        #region Test Classes

        private class MethodCallTest
        {
            public int Value { get; set; }

            public void AddOne()
            {
                Value += 1;
            }

            public static int GetOne()
            {
                return 1;
            }

            public int AddValues( int a, int b )
            {
                return a + b;
            }
        }

        #endregion
    }
}
