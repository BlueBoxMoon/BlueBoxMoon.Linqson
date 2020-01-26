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
    /// Definition for the most basic type of expression that can be encoded
    /// into JSON format.
    /// </summary>
    public abstract class EncodedExpression
    {
        #region Properties

        /// <summary>
        /// The node type represented by this encoded expression.
        /// </summary>
        public ExpressionType NodeType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Encodes a <see cref="Expression"/> into an object that can be
        /// serialized.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <returns>An object that can be serialized.</returns>
        public static EncodedExpression EncodeExpression( Expression expression )
        {
            return EncodeExpression( expression, new EncodeState(), new EncodeOptions() );
        }

        /// <summary>
        /// Encodes a <see cref="Expression"/> into an object that can be
        /// serialized.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="options">The options that will be used during encoding.</param>
        /// <returns>An object that can be serialized.</returns>
        public static EncodedExpression EncodeExpression( Expression expression, EncodeOptions options )
        {
            return EncodeExpression( expression, new EncodeState(), options );
        }

        /// <summary>
        /// Encodes a <see cref="Expression"/> into an object that can be
        /// serialized.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="state">The state tracker for the encode operation.</param>
        /// <param name="options">The options that will be used during encoding.</param>
        /// <returns>An object that can be serialized.</returns>
        internal static EncodedExpression EncodeExpression( Expression expression, EncodeState state, EncodeOptions options )
        {
            switch ( expression.NodeType )
            {
                case ExpressionType.Equal:
                case ExpressionType.AndAlso:
                    return EncodedBinaryExpression.EncodeExpression( ( BinaryExpression ) expression, state, options );

                case ExpressionType.Convert:
                    return EncodedUnaryExpression.EncodeExpression( ( UnaryExpression ) expression, state, options );

                case ExpressionType.MemberAccess:
                    return EncodedMemberExpression.EncodeExpression( ( MemberExpression ) expression, state, options );

                case ExpressionType.Parameter:
                    return EncodedParameterExpression.EncodeExpression( ( ParameterExpression ) expression, state, options );

                case ExpressionType.Constant:
                    return EncodedConstantExpression.EncodeExpression( ( ConstantExpression ) expression, state, options );

                case ExpressionType.Call:
                    return EncodedMethodCallExpression.EncodeExpression( ( MethodCallExpression ) expression, state, options );

                case ExpressionType.Lambda:
                    return EncodedLambdaExpression.EncodeExpression( ( LambdaExpression ) expression, state, options );

                default:
                    throw new ArgumentException( $"Unknown node type {expression.NodeType} when serializing", nameof( expression ) );
            }
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        public Expression DecodeExpression()
        {
            return DecodeExpression( new DecodeState(), new DecodeOptions() );
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <param name="options">The options that will be used during decoding.</param>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        public Expression DecodeExpression( DecodeOptions options )
        {
            return DecodeExpression( new DecodeState(), options );
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <param name="state">The state tracker for the decode operation.</param>
        /// <param name="options">The options that will be used during decoding.</param>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        internal protected abstract Expression DecodeExpression( DecodeState state, DecodeOptions options );

        #endregion
    }
}
