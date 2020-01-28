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

namespace BlueBoxMoon.Linqson.Internals
{
    /// <summary>
    /// Contains information for the mapping table to use when determining out to encode
    /// or decode an expression.
    /// </summary>
    internal class ExpressionLookup
    {
        /// <summary>
        /// Gets the encode method to call.
        /// </summary>
        /// <value>
        /// The encode method to call.
        /// </value>
        public Func<Expression, EncodeState, EncodeOptions, EncodedExpression> Encode { get; }

        /// <summary>
        /// Gets the decode method to call.
        /// </summary>
        /// <value>
        /// The decode method to call.
        /// </value>
        public Func<EncodedExpression, DecodeState, DecodeOptions, Expression> Decode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionLookup"/> class.
        /// </summary>
        /// <param name="encode">The encode method.</param>
        /// <param name="decode">The decode method.</param>
        internal ExpressionLookup( Func<Expression, EncodeState, EncodeOptions, EncodedExpression> encode, Func<EncodedExpression, DecodeState, DecodeOptions, Expression> decode )
        {
            Encode = encode;
            Decode = decode;
        }
    }
}
