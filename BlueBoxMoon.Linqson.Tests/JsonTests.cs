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
    public class JsonTests
    {
        [Test]
        public void NewtonsoftEncodeDecode()
        {
            Expression<Func<int>> expected = () => 3 + 3;
            var encoded = EncodedExpression.EncodeExpression( expected );

            var json = Newtonsoft.Json.JsonConvert.SerializeObject( encoded );
            var jsonEncoded = Newtonsoft.Json.JsonConvert.DeserializeObject<EncodedExpression>( json );

            var actual = ( Expression<Func<int>> ) EncodedExpression.DecodeExpression( jsonEncoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            Assert.AreEqual( expected.Compile().Invoke(), actual.Compile().Invoke() );
            Assert.AreEqual( 6, actual.Compile().Invoke() );
        }

#if NETCOREAPP
        [Test]
        public void SystemJsonEncodeDecode()
        {
            Expression<Func<int>> expected = () => 3 + 3;
            var encoded = EncodedExpression.EncodeExpression( expected );

            var json = System.Text.Json.JsonSerializer.Serialize( encoded );
            var jsonEncoded = System.Text.Json.JsonSerializer.Deserialize<EncodedExpression>( json );

            var actual = ( Expression<Func<int>> ) EncodedExpression.DecodeExpression( jsonEncoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            Assert.AreEqual( expected.Compile().Invoke(), actual.Compile().Invoke() );
            Assert.AreEqual( 6, actual.Compile().Invoke() );
        }

        [Test]
        public void SystemJsonEncodeNewtonsoftDecode()
        {
            Expression<Func<int>> expected = () => 3 + 3;
            var encoded = EncodedExpression.EncodeExpression( expected );

            var json = System.Text.Json.JsonSerializer.Serialize( encoded );
            var jsonEncoded = Newtonsoft.Json.JsonConvert.DeserializeObject<EncodedExpression>( json );

            var actual = ( Expression<Func<int>> ) EncodedExpression.DecodeExpression( jsonEncoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            Assert.AreEqual( expected.Compile().Invoke(), actual.Compile().Invoke() );
            Assert.AreEqual( 6, actual.Compile().Invoke() );
        }

        [Test]
        public void NewtonsoftEncodeSystemJsonDecode()
        {
            Expression<Func<int>> expected = () => 3 + 3;
            var encoded = EncodedExpression.EncodeExpression( expected );

            var json = Newtonsoft.Json.JsonConvert.SerializeObject( encoded );
            var jsonEncoded = System.Text.Json.JsonSerializer.Deserialize<EncodedExpression>( json );

            var actual = ( Expression<Func<int>> ) EncodedExpression.DecodeExpression( jsonEncoded );

            Assert.AreEqual( expected.ToString(), actual.ToString() );

            Assert.AreEqual( expected.Compile().Invoke(), actual.Compile().Invoke() );
            Assert.AreEqual( 6, actual.Compile().Invoke() );
        }

#endif
    }
}
