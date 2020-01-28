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

namespace BlueBoxMoon.Linqson
{
    /// <summary>
    /// A lambda expression that can be serialized.
    /// </summary>
    public class EncodedLambdaExpression : EncodedExpression
    {
        #region Properties

        /// <summary>
        /// The parameters that are passed in and made available to descendent
        /// expressions.
        /// </summary>
        public List<EncodedExpression> Parameters { get; set; }

        /// <summary>
        /// The main body of this lambda.
        /// </summary>
        public EncodedExpression Body { get; set; }

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
        internal static EncodedLambdaExpression EncodeExpression( LambdaExpression expression, EncodeState state, EncodeOptions options )
        {
            var parameters = expression.Parameters
                .Select( a => EncodeExpression( a, state, options ) )
                .ToList();

            var jsonExpression = new EncodedLambdaExpression
            {
                NodeType = expression.NodeType,
                Parameters = parameters,
                Body = EncodeExpression( expression.Body, state, options )
            };

            return jsonExpression;
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <param name="state">The state tracker for the decode operation.</param>
        /// <param name="options">The options that will be used during decoding.</param>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        internal protected override Expression DecodeExpression( DecodeState state, DecodeOptions options )
        {
            var parameters = Parameters.Select( a => a.DecodeExpression( state, options ) )
                .Cast<ParameterExpression>()
                .ToList();

            var lambda = Expression.Lambda( Body.DecodeExpression( state, options ), parameters );

            return lambda;
        }

        #endregion
    }
}
