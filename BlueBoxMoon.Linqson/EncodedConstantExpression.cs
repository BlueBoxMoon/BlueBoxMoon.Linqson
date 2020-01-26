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

namespace BlueBoxMoon.Linqson
{
    /// <summary>
    /// A constant value expression that can be serialized.
    /// </summary>
    public class EncodedConstantExpression : EncodedExpression
    {
        #region Properties

        /// <summary>
        /// The type of the constant value.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The value converted to a string.
        /// </summary>
        public string Value { get; set; }

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
        internal static EncodedConstantExpression EncodeExpression( ConstantExpression expression, EncodeState state, EncodeOptions options )
        {
            return new EncodedConstantExpression
            {
                NodeType = expression.NodeType,
                Type = expression.Type.AssemblyQualifiedName,
                Value = expression.Value.ToString()
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
            var type = System.Type.GetType( Type );

            if ( type == typeof( char ) )
            {
                if ( !char.TryParse( Value, out var c ) )
                {
                    throw new Exception( "Invalid constant" );
                }

                return Expression.Constant( c );
            }
            else if ( type == typeof( int ) )
            {
                return Expression.Constant( int.Parse( Value ) );
            }
            else if ( type == typeof( string ) )
            {
                return Expression.Constant( Value );
            }

            throw new Exception( $"Unknown constant type {type}" );
        }

        #endregion
    }
}
