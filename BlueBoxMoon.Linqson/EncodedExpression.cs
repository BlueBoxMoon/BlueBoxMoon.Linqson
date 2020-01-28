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
using System.Linq.Expressions;

namespace BlueBoxMoon.Linqson
{
    /// <summary>
    /// Definition for the most basic type of expression that can be encoded
    /// into JSON format.
    /// </summary>
    public abstract class EncodedExpression
    {
        #region Static Properties

        /// <summary>
        /// Gets the expression type mapping table to help deserializers.
        /// </summary>
        /// <value>
        /// The expression type mapping to help deserializers.
        /// </value>
        public static IReadOnlyDictionary<ExpressionType, Type> ExpressionTypeMapping { get; }

        #endregion

        #region Properties

        /// <summary>
        /// The node type represented by this encoded expression.
        /// </summary>
        public ExpressionType NodeType { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="EncodedExpression"/> class.
        /// </summary>
        static EncodedExpression()
        {
            #region Expression Type Mapping

            ExpressionTypeMapping = new Dictionary<ExpressionType, Type>()
            {
                { ExpressionType.Add, typeof( EncodedBinaryExpression ) },
                { ExpressionType.AddChecked, typeof( EncodedBinaryExpression ) },
                { ExpressionType.And, typeof( EncodedBinaryExpression ) },
                { ExpressionType.AndAlso, typeof( EncodedBinaryExpression ) },
                //{ ExpressionType.ArrayLength, typeof() },
                //{ ExpressionType.ArrayIndex, typeof() },
                { ExpressionType.Call, typeof( EncodedMethodCallExpression ) },
                { ExpressionType.Coalesce, typeof( EncodedBinaryExpression ) },
                //{ ExpressionType.Conditional, typeof() },
                { ExpressionType.Constant, typeof( EncodedConstantExpression ) },
                { ExpressionType.Convert, typeof( EncodedUnaryExpression ) },
                { ExpressionType.ConvertChecked, typeof( EncodedUnaryExpression ) },
                { ExpressionType.Divide, typeof( EncodedBinaryExpression ) },
                { ExpressionType.Equal, typeof( EncodedBinaryExpression ) },
                { ExpressionType.ExclusiveOr, typeof( EncodedBinaryExpression ) },
                { ExpressionType.GreaterThan, typeof( EncodedBinaryExpression) },
                { ExpressionType.GreaterThanOrEqual, typeof( EncodedBinaryExpression ) },
                //{ ExpressionType.Invoke, typeof() },
                { ExpressionType.Lambda, typeof( EncodedLambdaExpression ) },
                { ExpressionType.LeftShift, typeof( EncodedBinaryExpression ) },
                { ExpressionType.LessThan, typeof( EncodedBinaryExpression ) },
                { ExpressionType.LessThanOrEqual, typeof( EncodedBinaryExpression ) },
                //{ ExpressionType.ListInit, typeof() },
                { ExpressionType.MemberAccess, typeof( EncodedMemberExpression ) },
                //{ ExpressionType.MemberInit, typeof() },
                { ExpressionType.Modulo, typeof( EncodedBinaryExpression ) },
                { ExpressionType.Multiply, typeof( EncodedBinaryExpression ) },
                { ExpressionType.MultiplyChecked, typeof( EncodedBinaryExpression ) },
                //{ ExpressionType.Negate, typeof() },
                //{ ExpressionType.UnaryPlus, typeof() },
                //{ ExpressionType.NegateChecked, typeof() },
                //{ ExpressionType.New, typeof() },
                //{ ExpressionType.NewArrayInit, typeof() },
                //{ ExpressionType.NewArrayBounds, typeof() },
                //{ ExpressionType.Not, typeof() },
                { ExpressionType.NotEqual, typeof( EncodedBinaryExpression ) },
                { ExpressionType.Or, typeof( EncodedBinaryExpression ) },
                { ExpressionType.OrElse, typeof( EncodedBinaryExpression ) },
                { ExpressionType.Parameter, typeof( EncodedParameterExpression ) },
                { ExpressionType.Power, typeof( EncodedBinaryExpression ) },
                //{ ExpressionType.Quote, typeof() },
                { ExpressionType.RightShift, typeof( EncodedBinaryExpression ) },
                { ExpressionType.Subtract, typeof( EncodedBinaryExpression ) },
                { ExpressionType.SubtractChecked, typeof( EncodedBinaryExpression ) },
                //{ ExpressionType.TypeAs, typeof() },
                //{ ExpressionType.TypeIs, typeof() },
                //{ ExpressionType.Assign, typeof() },
                //{ ExpressionType.Block, typeof() },
                //{ ExpressionType.DebugInfo, typeof() },
                //{ ExpressionType.Decrement, typeof() },
                //{ ExpressionType.Dynamic, typeof() },
                //{ ExpressionType.Default, typeof() },
                //{ ExpressionType.Extension, typeof() },
                //{ ExpressionType.Goto, typeof() },
                //{ ExpressionType.Increment, typeof() },
                //{ ExpressionType.Index, typeof() },
                //{ ExpressionType.Label, typeof() },
                //{ ExpressionType.RuntimeVariables, typeof() },
                //{ ExpressionType.Loop, typeof() },
                //{ ExpressionType.Switch, typeof() },
                //{ ExpressionType.Throw, typeof() },
                //{ ExpressionType.Try, typeof() },
                //{ ExpressionType.Unbox, typeof() },
                //{ ExpressionType.AddAssign, typeof() },
                //{ ExpressionType.AndAssign, typeof() },
                //{ ExpressionType.DivideAssign, typeof() },
                //{ ExpressionType.ExclusiveOrAssign, typeof() },
                //{ ExpressionType.LeftShiftAssign, typeof() },
                //{ ExpressionType.ModuloAssign, typeof() },
                //{ ExpressionType.MultiplyAssign, typeof() },
                //{ ExpressionType.OrAssign, typeof() },
                //{ ExpressionType.PowerAssign, typeof() },
                //{ ExpressionType.RightShiftAssign, typeof() },
                //{ ExpressionType.SubtractAssign, typeof() },
                //{ ExpressionType.AddAssignChecked, typeof() },
                //{ ExpressionType.MultiplyAssignChecked, typeof() },
                //{ ExpressionType.SubtractAssignChecked, typeof() },
                //{ ExpressionType.PreIncrementAssign, typeof() },
                //{ ExpressionType.PreDecrementAssign, typeof() },
                //{ ExpressionType.PostIncrementAssign, typeof() },
                //{ ExpressionType.PostDecrementAssign, typeof() },
                //{ ExpressionType.TypeEqual, typeof() },
                //{ ExpressionType.OnesComplement, typeof() },
                //{ ExpressionType.IsTrue, typeof() },
                //{ ExpressionType.IsFalse, typeof() }
            };

            #endregion
        }

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
            if ( !ExpressionTypeMapping.ContainsKey( expression.NodeType ) )
            {
                throw new ArgumentException( $"Unknown node type {expression.NodeType} when serializing", nameof( expression ) );
            }

            var type = ExpressionTypeMapping[expression.NodeType];
            var methodInfo = type.GetMethod( "EncodeExpression", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic );

            return ( EncodedExpression ) methodInfo.Invoke( null, new object[] { expression, state, options } );
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <param name="expression">The encoded expression to be decoded.</param>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        public static Expression DecodeExpression( EncodedExpression expression )
        {
            return DecodeExpression( expression, new DecodeOptions() );
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <param name="expression">The encoded expression to be decoded.</param>
        /// <param name="options">The options that will be used during decoding.</param>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        public static Expression DecodeExpression( EncodedExpression expression, DecodeOptions options )
        {
            return expression.DecodeExpression( new DecodeState(), options );
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
