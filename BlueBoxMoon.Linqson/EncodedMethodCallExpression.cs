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
using System.Linq.Expressions;
using System.Reflection;

namespace BlueBoxMoon.Linqson
{
    /// <summary>
    /// A method call expression that can be serialized.
    /// </summary>
    public class EncodedMethodCallExpression : EncodedExpression
    {
        #region Properties

        /// <summary>
        /// The expressions that will be resolved to the the arguments passed
        /// to the method.
        /// </summary>
        public List<EncodedExpression> Arguments { get; set; }

        /// <summary>
        /// The object instance whose method will be called; or <c>null</c>
        /// to indicate a static method.
        /// </summary>
        public EncodedExpression Object { get; set; }

        /// <summary>
        /// The class type that declares the method.
        /// </summary>
        public string MethodType { get; set; }

        /// <summary>
        /// The name of the method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// The argument types of the method to be called.
        /// </summary>
        public List<string> MethodArgumentTypes { get; set; }

        /// <summary>
        /// The generic types defined on the method.
        /// </summary>
        public List<string> MethodGenericTypes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Encodes a <see cref="Expression"/> into an object that can be
        /// serialized.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="state">The state tracker for the encode operation.</param>
        /// <param name="options">The options that will be used during encoding.</param>
        /// <returns>An object that can be serialized.</returns>
        internal static EncodedMethodCallExpression EncodeExpression( MethodCallExpression expression, EncodeState state, EncodeOptions options )
        {
            // Store the signature, that is the `.ToString()` of the method (or it's generic definition
            var method = expression.Method;
            if ( method.IsGenericMethod )
            {
                method = method.GetGenericMethodDefinition();
            }
            return new EncodedMethodCallExpression
            {
                Arguments = expression.Arguments.Select( a => EncodeExpression( a, state, options ) ).ToList(),
                Object = expression.Object != null ? EncodeExpression( expression.Object, state, options ) : null,
                MethodType = method.DeclaringType.AssemblyQualifiedName,
                Method = method.ToString(),
                MethodArgumentTypes = expression.Method.GetParameters().Select( a => a.ParameterType.AssemblyQualifiedName ).ToList(),
                MethodGenericTypes = expression.Method.IsGenericMethod ? expression.Method.GetGenericArguments().Select( a => a.AssemblyQualifiedName ).ToList() : new List<string>()
            };
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <param name="state">The state tracker for the decode operation.</param>
        /// <param name="options">The options that will be used during decoding.</param>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        internal protected override Expression DecodeExpression( DecodeState state, DecodeOptions options )
        {
            var type = Type.GetType( MethodType );
            var parameterTypes = MethodArgumentTypes.Select( a => Type.GetType( a ) ).ToArray();

            var methodInfo = type.GetRuntimeMethods()
                .Where( a => a.ToString() == Method )
                .Single();

            if ( methodInfo.IsGenericMethodDefinition )
            {
                methodInfo = methodInfo.MakeGenericMethod( MethodGenericTypes.Select( a => Type.GetType( a ) ).ToArray() );
            }

            if ( Object == null )
            {
                return Expression.Call( methodInfo, Arguments.Select( a => a.DecodeExpression( state, options ) ) );
            }
            else
            {
                return Expression.Call( Object.DecodeExpression( state, options ), methodInfo, Arguments.Select( a => a.DecodeExpression( state, options ) ) );
            }
        }

        #endregion
    }
}
