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
        public void CallUnsafeMethod()
        {
            Expression<Action<MethodCallTest>> expected = ( a ) => a.AddOne();

            var encoded = EncodedExpression.EncodeExpression( expected );

            Assert.Throws( typeof( UnsafeMethodCallException ), () => EncodedExpression.DecodeExpression( encoded ) );
        }

        #region Test Classes

        private class MethodCallTest
        {
            public int Value { get; set; }

            public void AddOne()
            {
                Value += 1;
            }
        }

        #endregion
    }
}
