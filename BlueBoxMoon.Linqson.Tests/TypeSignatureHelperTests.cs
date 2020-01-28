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
using System.Linq;
using System.Reflection;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class Tests
    {
        [Test]
        public void TestAllEnumerableMethods()
        {
            var type = typeof( Enumerable );

            foreach ( var expectedMethodInfo in type.GetMethods( BindingFlags.Static | BindingFlags.Public ) )
            {
                var helper = new TypeSignatureHelper();
                var signature = helper.GetSignatureFromMethodInfo( expectedMethodInfo );
                MethodInfo actualMethodInfo = null;

                Assert.DoesNotThrow( () =>
                {
                    actualMethodInfo = helper.GetMethodInfoFromSignature( signature );
                }, signature );

                Assert.NotNull( actualMethodInfo, signature );
                Assert.AreEqual( expectedMethodInfo.ToString(), actualMethodInfo.ToString(), signature );
            }
        }
    }
}