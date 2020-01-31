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
using System.Linq;
using System.Reflection;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class TypeSignatureHelperTests
    {
        [Test]
        public void AreTypesEqual_MismatchedGenericFail()
        {
            var helper = new TypeSignatureHelper();

            Assert.IsFalse( helper.AreTypesEqual( typeof( string ), typeof( List<> ) ) );
        }

        [Test]
        public void AreTypesEqual_MismatchedGenericArgumentCountFail()
        {
            var helper = new TypeSignatureHelper();

            Assert.IsFalse( helper.AreTypesEqual( typeof( TestHelpOneGeneric<string> ), typeof( TestHelperTwoGenerics<string, string> ) ) );
        }

        [Test]
        public void AreTypesEqual_MismatchedParameterPositionFail()
        {
            var helper = new TypeSignatureHelper();

            var mi = typeof( TestHelper ).GetMethod( "TwoGenericMethod" );
            var parameters = mi.GetParameters();

            Assert.IsFalse( helper.AreTypesEqual( parameters[0].ParameterType, parameters[1].ParameterType ) );
        }

        [Test]
        public void DecodeFromNetCoreSignature()
        {
            var helper = new TypeSignatureHelper();
            var signature = "SelectMany<3>@{System.Linq.Enumerable, System.Linq}({System.Collections.Generic.IEnumerable`1<{!!0}>, System.Private.CoreLib},{System.Func`3<{!!0},{System.Int32, System.Private.CoreLib},{System.Collections.Generic.IEnumerable`1<{!!1}>, System.Private.CoreLib}>, System.Private.CoreLib},{System.Func`3<{!!0},{!!1},{!!2}>, System.Private.CoreLib})";

            Assert.DoesNotThrow( () => helper.GetMethodInfoFromSignature( signature ) );
        }

        [Test]
        public void DecodeFromNetFullSignature()
        {
            var helper = new TypeSignatureHelper();
            var signature = "SelectMany<3>@{System.Linq.Enumerable, System.Core}({System.Collections.Generic.IEnumerable`1<{!!0}>, mscorlib},{System.Func`2<{!!0},{System.Collections.Generic.IEnumerable`1<{!!1}>, mscorlib}>, mscorlib},{System.Func`3<{!!0},{!!1},{!!2}>, mscorlib})";

            Assert.DoesNotThrow( () => helper.GetMethodInfoFromSignature( signature ) );
        }

        [Test]
        public void EncodeAndDecodeAllEnumerableMethods()
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

        [Test]
        public void EncodeAndDecodeMultipleTypes()
        {
            var helper = new TypeSignatureHelper();
            var expectedTypes = new[] { typeof( string ), typeof( int ) };

            var signature = helper.GetSignatureFromTypes( expectedTypes );
            var actualTypes = helper.GetTypesFromSignature( signature );

            Assert.IsNotNull( actualTypes );
            Assert.AreEqual( expectedTypes.Length, actualTypes.Length );
            Assert.AreEqual( expectedTypes[0], actualTypes[0] );
            Assert.AreEqual( expectedTypes[1], actualTypes[1] );
        }

        [Test]
        public void EncodeAndDecodeStringType()
        {
            var helper = new TypeSignatureHelper();
            var expectedType = typeof( string );

            var signature = helper.GetSignatureFromType( expectedType );
            var actualType = helper.GetTypeFromSignature( signature );

            Assert.IsNotNull( actualType );
            Assert.AreEqual( expectedType, actualType );
        }

        [Test]
        public void GetTypeFromNameAndAssemblyEmptyNameReturnsNull()
        {
            var helper = new TypeSignatureHelper();

            Assert.IsNull( helper.GetTypeFromNameAndAssembly( string.Empty, null ) );
        }

        [Test]
        public void GetTypeFromNameAndAssemblyEmptyAssembly()
        {
            var helper = new TypeSignatureHelper();

            Assert.IsNotNull( helper.GetTypeFromNameAndAssembly( typeof( string ).FullName, null ) );
        }

        [Test]
        public void GetMethodInfoFromSignature_EmptyStringFails()
        {
            var helper = new TypeSignatureHelper();

            //SelectMany<3>@{System.Linq.Enumerable, System.Linq}({System.Collections.Generic.IEnumerable`1<{!!0}>, System.Private.CoreLib},{System.Func`3<{!!0},{System.Int32, System.Private.CoreLib},{System.Collections.Generic.IEnumerable`1<{!!1}>, System.Private.CoreLib}>, System.Private.CoreLib},{System.Func`3<{!!0},{!!1},{!!2}>, System.Private.CoreLib})
            Assert.Throws<SignatureInvalidException>( () => helper.GetMethodInfoFromSignature( "" ) );
        }

        [Test]
        public void GetMethodInfoFromSignature_MissingClosingGenericFails()
        {
            var helper = new TypeSignatureHelper();

            Assert.Throws<SignatureInvalidException>( () => helper.GetTypeFromSignature( "ToString<1" ) );
        }

        [Test]
        public void GetMethodInfoFromSignature_MissingClosingParenthesisFails()
        {
            var helper = new TypeSignatureHelper();

            Assert.Throws<SignatureInvalidException>( () => helper.GetTypeFromSignature( "ToString@{System.Object, mscorlib}({System.Object, mscorlib}" ) );
        }

        [Test]
        public void GetMethodInfoFromSignature_MissingOpeningParenthesisFails()
        {
            var helper = new TypeSignatureHelper();

            Assert.Throws<SignatureInvalidException>( () => helper.GetTypeFromSignature( "ToString@{System.Object, mscorlib}" ) );
        }

        [Test]
        public void GetTypeFromSignature_EmptyStringFails()
        {
            var helper = new TypeSignatureHelper();

            Assert.Throws<SignatureInvalidException>( () => helper.GetTypeFromSignature( "" ) );
        }

        [Test]
        public void GetTypeFromSignature_PartialStringFails()
        {
            var helper = new TypeSignatureHelper();

            Assert.Throws<SignatureInvalidException>( () => helper.GetTypeFromSignature( "System.Type" ) );
        }

        [Test]
        public void GetTypeFromSignature_MissingClosingGenericFails()
        {
            var helper = new TypeSignatureHelper();

            Assert.Throws<SignatureInvalidException>( () => helper.GetTypeFromSignature( "System.Type<2" ) );
        }

        #region Test Classes

        private class TestHelpOneGeneric<T1> : List<T1>
        {
        }

        private class TestHelperTwoGenerics<T1, T2> : List<T1>
        {
        }

        private class TestHelper
        {
            public void TwoGenericMethod<T1, T2>( T1 t1, T2 t2 )
            {
            }
        }

        #endregion
    }
}
