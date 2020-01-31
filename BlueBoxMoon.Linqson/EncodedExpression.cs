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
using System.Reflection;
using System.Runtime.CompilerServices;

using BlueBoxMoon.Linqson.Internals;

namespace BlueBoxMoon.Linqson
{

    /// <summary>
    /// Definition for the most basic type of expression that can be encoded
    /// into JSON format.
    /// </summary>
    public class EncodedExpression
    {
        #region Static Properties

        /// <summary>
        /// Gets the expression type mapping table.
        /// </summary>
        /// <value>
        /// The expression type mapping.
        /// </value>
        internal static IReadOnlyDictionary<ExpressionType, ExpressionLookup> ExpressionTypeMapping { get; }

        #endregion

        #region Properties

        /// <summary>
        /// The node type represented by this encoded expression.
        /// </summary>
        public ExpressionType NodeType { get; set; }

        public IDictionary<string, EncodedExpression> Expressions { get; set; }

        public IDictionary<string, string> Values { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="EncodedExpression"/> class.
        /// </summary>
        static EncodedExpression()
        {
            #region Expression Type Mapping

            ExpressionTypeMapping = new Dictionary<ExpressionType, ExpressionLookup>
            {
                { ExpressionType.Add, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.AddChecked, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.And, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.AndAlso, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                // ExpressionType.ArrayLength
                // ExpressionType.ArrayIndex
                { ExpressionType.Call, new ExpressionLookup ( EncodeMethodCallExpression, DecodeMethodCallExpression ) },
                { ExpressionType.Coalesce, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                // ExpressionType.Conditional
                { ExpressionType.Constant, new ExpressionLookup( EncodeConstantExpression, DecodeConstantExpression ) },
                { ExpressionType.Convert, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                { ExpressionType.ConvertChecked, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                { ExpressionType.Divide, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.Equal, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.ExclusiveOr, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.GreaterThan, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.GreaterThanOrEqual, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                // ExpressionType.Invoke
                { ExpressionType.Lambda, new ExpressionLookup( EncodeLambdaExpression, DecodeLambdaExpression ) },
                { ExpressionType.LeftShift, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.LessThan, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.LessThanOrEqual, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                // ExpressionType.ListInit
                { ExpressionType.MemberAccess, new ExpressionLookup( EncodeMemberExpression, DecodeMemberExpression ) },
                // ExpressionType.MemberInit
                { ExpressionType.Modulo, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.Multiply, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.MultiplyChecked, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.Negate, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                // ExpressionType.UnaryPlus
                { ExpressionType.NegateChecked, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                // ExpressionType.New
                // ExpressionType.NewArrayInit
                // ExpressionType.NewArrayBounds
                { ExpressionType.Not, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                { ExpressionType.NotEqual, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.Or, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.OrElse, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.Parameter, new ExpressionLookup( EncodeParameterExpression, DecodeParameterExpression ) },
                { ExpressionType.Power, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                // ExpressionType.Quote
                { ExpressionType.RightShift, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.Subtract, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.SubtractChecked, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                // ExpressionType.TypeAs
                // ExpressionType.TypeIs
                { ExpressionType.Assign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                // ExpressionType.Block
                // ExpressionType.DebugInfo
                { ExpressionType.Decrement, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                // ExpressionType.Dynamic
                // ExpressionType.Default
                // ExpressionType.Extension
                // ExpressionType.Goto
                { ExpressionType.Increment, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                // ExpressionType.Index
                // ExpressionType.Label
                // ExpressionType.RuntimeVariables
                // ExpressionType.Loop
                // ExpressionType.Switch
                // ExpressionType.Throw
                // ExpressionType.Try
                // ExpressionType.Unbox
                { ExpressionType.AddAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.AndAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.DivideAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.ExclusiveOrAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.LeftShiftAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.ModuloAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.MultiplyAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.OrAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.PowerAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.RightShiftAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.SubtractAssign, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.AddAssignChecked, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.MultiplyAssignChecked, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.SubtractAssignChecked, new ExpressionLookup( EncodeBinaryExpression, DecodeBinaryExpression ) },
                { ExpressionType.PreIncrementAssign, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                { ExpressionType.PreDecrementAssign, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                { ExpressionType.PostIncrementAssign, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                { ExpressionType.PostDecrementAssign, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                // ExpressionType.TypeEqual
                { ExpressionType.OnesComplement, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                { ExpressionType.IsTrue, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },
                { ExpressionType.IsFalse, new ExpressionLookup( EncodeUnaryExpression, DecodeUnaryExpression ) },

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
            return EncodeExpression( expression, new EncodeOptions() );
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
                throw new ArgumentException( $"Encountered unknown node type {expression.NodeType} when encoding expression.", nameof( expression ) );
            }

            return ExpressionTypeMapping[expression.NodeType].Encode( expression, state, options );
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
            return DecodeExpression( expression, new DecodeState(), options );
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <param name="expression">The encoded expression to be decoded.</param>
        /// <param name="state">The state of the decode operation.</param>
        /// <param name="options">The options that will be used during decoding.</param>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        internal static Expression DecodeExpression( EncodedExpression expression, DecodeState state, DecodeOptions options )
        {
            if ( !ExpressionTypeMapping.ContainsKey( expression.NodeType ) )
            {
                throw new ArgumentException( $"Encountered unknown node type {expression.NodeType} when decoding expression.", nameof( expression ) );
            }

            return ExpressionTypeMapping[expression.NodeType].Decode( expression, state, options );
        }

        /// <summary>
        /// Determines whether the type is an anonymous type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if type is anonymous; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsAnonymousType( Type type )
        {
            if ( type.IsGenericType )
            {
                var d = type.GetGenericTypeDefinition();
                if ( d.IsClass && d.IsSealed && d.Attributes.HasFlag( TypeAttributes.NotPublic ) )
                {
                    var attributes = d.GetCustomAttributes( typeof( CompilerGeneratedAttribute ), false );
                    if ( attributes != null && attributes.Length > 0 )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region BinaryExpression

        /// <summary>
        /// Encodes the binary expression.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="state">The current state of the encode operation.</param>
        /// <param name="options">The options for the encode operation.</param>
        /// <returns>An <see cref="EncodedExpression"/> object.</returns>
        internal static EncodedExpression EncodeBinaryExpression( Expression expression, EncodeState state, EncodeOptions options )
        {
            var binaryExpression = ( BinaryExpression ) expression;

            return new EncodedExpression
            {
                NodeType = binaryExpression.NodeType,
                Expressions = new Dictionary<string, EncodedExpression>
                {
                    { "Left", EncodeExpression( binaryExpression.Left, state, options ) },
                    { "Right", EncodeExpression( binaryExpression.Right, state, options ) },
                    { "Conversion", binaryExpression.Conversion != null ? EncodeExpression( binaryExpression.Conversion, state, options ) : null }
                }
            };
        }

        /// <summary>
        /// Decodes the binary expression.
        /// </summary>
        /// <param name="expression">The expression to be decoded.</param>
        /// <param name="state">The current state of the decode operation.</param>
        /// <param name="options">The options for the decode operation.</param>
        /// <returns>An <see cref="Expression"/> object.</returns>
        internal static Expression DecodeBinaryExpression( EncodedExpression expression, DecodeState state, DecodeOptions options )
        {
            var left = DecodeExpression( expression.Expressions["Left"], state, options );
            var right = DecodeExpression( expression.Expressions["Right"], state, options );

            if ( !expression.Expressions.ContainsKey( "Conversion" ) || expression.Expressions["Conversion"] == null )
            {
                return Expression.MakeBinary( expression.NodeType, left, right );
            }
            else
            {
                var conversion = ( LambdaExpression ) DecodeExpression( expression.Expressions["Conversion"], state, options );

                return Expression.MakeBinary( expression.NodeType, left, right, false, null, conversion );
            }
        }

        #endregion

        #region UnaryExpression

        /// <summary>
        /// Encodes the unary expression.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="state">The current state of the encode operation.</param>
        /// <param name="options">The options for the encode operation.</param>
        /// <returns>An <see cref="EncodedExpression"/> object.</returns>
        internal static EncodedExpression EncodeUnaryExpression( Expression expression, EncodeState state, EncodeOptions options )
        {
            var unaryExpression = ( UnaryExpression ) expression;

            return new EncodedExpression
            {
                NodeType = unaryExpression.NodeType,
                Expressions = new Dictionary<string, EncodedExpression>
                {
                    { "Operand", EncodeExpression( unaryExpression.Operand, state, options ) }
                },
                Values = new Dictionary<string, string>
                {
                    { "Type", state.SignatureHelper.GetSignatureFromType( unaryExpression.Type ) }
                }
            };
        }

        /// <summary>
        /// Decodes the unary expression.
        /// </summary>
        /// <param name="expression">The expression to be decoded.</param>
        /// <param name="state">The current state of the decode operation.</param>
        /// <param name="options">The options for the decode operation.</param>
        /// <returns>An <see cref="Expression"/> object.</returns>
        internal static Expression DecodeUnaryExpression( EncodedExpression expression, DecodeState state, DecodeOptions options )
        {
            var operand = DecodeExpression( expression.Expressions["Operand"], state, options );
            var type = state.SignatureHelper.GetTypeFromSignature( expression.Values["Type"] );

            return Expression.MakeUnary( expression.NodeType, operand, type );
        }

        #endregion

        #region ConstantExpression

        /// <summary>
        /// Decodes the binary expression.
        /// </summary>
        /// <param name="expression">The expression to be decoded.</param>
        /// <param name="state">The current state of the decode operation.</param>
        /// <param name="options">The options for the decode operation.</param>
        /// <returns>An <see cref="Expression"/> object.</returns>
        internal static EncodedExpression EncodeConstantExpression( Expression expression, EncodeState state, EncodeOptions options )
        {
            var constantExpression = ( ConstantExpression ) expression;

            return new EncodedExpression
            {
                NodeType = constantExpression.NodeType,
                Values = new Dictionary<string, string>
                {
                    { "Type", state.SignatureHelper.GetSignatureFromType( constantExpression.Type ) },
                    { "Value", constantExpression.Value?.ToString() }
                }
            };
        }

        /// <summary>
        /// Decodes the constant expression.
        /// </summary>
        /// <param name="expression">The expression to be decoded.</param>
        /// <param name="state">The current state of the decode operation.</param>
        /// <param name="options">The options for the decode operation.</param>
        /// <returns>An <see cref="Expression"/> object.</returns>
        internal static Expression DecodeConstantExpression( EncodedExpression expression, DecodeState state, DecodeOptions options )
        {
            var type = state.SignatureHelper.GetTypeFromSignature( expression.Values["Type"] );
            var Value = expression.Values["Value"];

            if ( type == typeof( bool ) )
            {
                return Expression.Constant( bool.Parse( Value ) );
            }
            else if ( type == typeof( char ) )
            {
                if ( !char.TryParse( Value, out var c ) )
                {
                    throw new Exception( "Invalid constant" );
                }

                return Expression.Constant( c );
            }
            else if ( type == typeof( short ) )
            {
                return Expression.Constant( short.Parse( Value ) );
            }
            else if ( type == typeof( int ) )
            {
                return Expression.Constant( int.Parse( Value ) );
            }
            else if ( type == typeof( long ) )
            {
                return Expression.Constant( long.Parse( Value ) );
            }
            else if ( type == typeof( float ) )
            {
                return Expression.Constant( float.Parse( Value ) );
            }
            else if ( type == typeof( double ) )
            {
                return Expression.Constant( double.Parse( Value ) );
            }
            else if ( type == typeof( string ) )
            {
                return Expression.Constant( Value );
            }
            else if ( type == typeof( object ) )
            {
                return Expression.Constant( Value );
            }

            throw new Exception( $"Unknown constant type {type}" );
        }

        #endregion

        #region LambdaExpression

        /// <summary>
        /// Encodes the lambda expression.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="state">The current state of the encode operation.</param>
        /// <param name="options">The options for the encode operation.</param>
        /// <returns>An <see cref="EncodedExpression"/> object.</returns>
        internal static EncodedExpression EncodeLambdaExpression( Expression expression, EncodeState state, EncodeOptions options )
        {
            var lambdaExpression = ( LambdaExpression ) expression;

            var expressions = new Dictionary<string, EncodedExpression>
            {
                { "Body", EncodeExpression( lambdaExpression.Body, state, options ) }
            };

            for ( int i = 0; i < lambdaExpression.Parameters.Count; i++ )
            {
                expressions.Add( $"p{i}", EncodeExpression( lambdaExpression.Parameters[i], state, options ) );
            }

            return new EncodedExpression
            {
                NodeType = lambdaExpression.NodeType,
                Expressions = expressions,
                Values = new Dictionary<string, string>
                {
                    { "ParameterCount", lambdaExpression.Parameters.Count.ToString() }
                }
            };
        }

        /// <summary>
        /// Decodes the lambda expression.
        /// </summary>
        /// <param name="expression">The expression to be decoded.</param>
        /// <param name="state">The current state of the decode operation.</param>
        /// <param name="options">The options for the decode operation.</param>
        /// <returns>An <see cref="Expression"/> object.</returns>
        internal static Expression DecodeLambdaExpression( EncodedExpression expression, DecodeState state, DecodeOptions options )
        {
            var body = DecodeExpression( expression.Expressions["Body"], state, options );
            var parameterCount = Convert.ToInt32( expression.Values["ParameterCount"] );
            var parameters = new List<ParameterExpression>();

            for ( int i = 0; i < parameterCount; i++ )
            {
                parameters.Add( ( ParameterExpression ) DecodeExpression( expression.Expressions[$"p{i}"], state, options ) );
            }

            return Expression.Lambda( body, parameters.ToArray() );
        }

        #endregion

        #region MemberExpression

        /// <summary>
        /// Encodes the member expression.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="state">The current state of the encode operation.</param>
        /// <param name="options">The options for the encode operation.</param>
        /// <returns>An <see cref="EncodedExpression"/> object.</returns>
        internal static EncodedExpression EncodeMemberExpression( Expression expression, EncodeState state, EncodeOptions options )
        {
            var memberExpression = ( MemberExpression ) expression;

            if ( IsAnonymousType( memberExpression.Member.ReflectedType ) )
            {
                throw new Exception( "Encoding member access of anonymous types is not supported." );
            }

            return new EncodedExpression
            {
                NodeType = memberExpression.NodeType,
                Expressions = new Dictionary<string, EncodedExpression>
                {
                    { "Expression", EncodeExpression( memberExpression.Expression, state, options ) }
                },
                Values = new Dictionary<string, string>
                {
                    { "Type", state.SignatureHelper.GetSignatureFromType( memberExpression.Member.ReflectedType ) },
                    { "Member", memberExpression.Member.Name },
                    { "IsProperty", ( memberExpression.Member.MemberType == MemberTypes.Property ).ToString() }
                }
            };
        }

        /// <summary>
        /// Decodes the member expression.
        /// </summary>
        /// <param name="expression">The expression to be decoded.</param>
        /// <param name="state">The current state of the decode operation.</param>
        /// <param name="options">The options for the decode operation.</param>
        /// <returns>An <see cref="Expression"/> object.</returns>
        internal static Expression DecodeMemberExpression( EncodedExpression expression, DecodeState state, DecodeOptions options )
        {
            MemberInfo memberInfo;
            Type type = state.SignatureHelper.GetTypeFromSignature( expression.Values["Type"] );

            if ( bool.Parse( expression.Values["IsProperty"] ) )
            {
                memberInfo = type.GetProperty( expression.Values["Member"] );
            }
            else
            {
                memberInfo = type.GetField( expression.Values["Member"] );
            }

            var expr = DecodeExpression( expression.Expressions["Expression"], state, options );

            return Expression.MakeMemberAccess( expr, memberInfo );
        }

        #endregion

        #region ParameterExpression

        /// <summary>
        /// Encodes the parameter expression.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="state">The current state of the encode operation.</param>
        /// <param name="options">The options for the encode operation.</param>
        /// <returns>An <see cref="EncodedExpression"/> object.</returns>
        internal static EncodedExpression EncodeParameterExpression( Expression expression, EncodeState state, EncodeOptions options )
        {
            return state.GetOrAddParameter( ( ParameterExpression ) expression );
        }

        /// <summary>
        /// Decodes the parameter expression.
        /// </summary>
        /// <param name="expression">The expression to be decoded.</param>
        /// <param name="state">The current state of the decode operation.</param>
        /// <param name="options">The options for the decode operation.</param>
        /// <returns>An <see cref="Expression"/> object.</returns>
        internal static Expression DecodeParameterExpression( EncodedExpression expression, DecodeState state, DecodeOptions options )
        {
            return state.GetOrAddParameter( expression );
        }

        #endregion

        #region MethodCallExpression

        /// <summary>
        /// Encodes the method call expression.
        /// </summary>
        /// <param name="expression">The expression to be encoded.</param>
        /// <param name="state">The current state of the encode operation.</param>
        /// <param name="options">The options for the encode operation.</param>
        /// <returns>An <see cref="EncodedExpression"/> object.</returns>
        internal static EncodedExpression EncodeMethodCallExpression( Expression expression, EncodeState state, EncodeOptions options )
        {
            var methodCallExpression = ( MethodCallExpression ) expression;

            var expressions = new Dictionary<string, EncodedExpression>();
            var methodSignature = state.SignatureHelper.GetSignatureFromMethodInfo( methodCallExpression.Method );

            for ( int i = 0; i < methodCallExpression.Arguments.Count; i++ )
            {
                expressions.Add( $"a{i}", EncodeExpression( methodCallExpression.Arguments[i], state, options ) );
            }

            if ( methodCallExpression.Object != null )
            {
                expressions.Add( "Object", EncodeExpression( methodCallExpression.Object, state, options ) );
            }
            else
            {
                expressions.Add( "Object", null );
            }

            return new EncodedExpression
            {
                NodeType = methodCallExpression.NodeType,
                Expressions = expressions,
                Values = new Dictionary<string, string>
                {
                    { "Method", methodSignature },
                    { "ArgumentCount", methodCallExpression.Arguments.Count.ToString() }
                }
            };
        }

        /// <summary>
        /// Decodes the method call expression.
        /// </summary>
        /// <param name="expression">The expression to be decoded.</param>
        /// <param name="state">The current state of the decode operation.</param>
        /// <param name="options">The options for the decode operation.</param>
        /// <returns>An <see cref="Expression"/> object.</returns>
        internal static Expression DecodeMethodCallExpression( EncodedExpression expression, DecodeState state, DecodeOptions options )
        {
            var arguments = new List<Expression>();
            Expression objectExpression = null;

            if ( expression.Expressions["Object"] != null )
            {
                objectExpression = DecodeExpression( expression.Expressions["Object"], state, options );
            }

            int argumentCount = Convert.ToInt32( expression.Values["ArgumentCount"] );
            for ( int i = 0; i < argumentCount; i++ )
            {
                arguments.Add( DecodeExpression( expression.Expressions[$"a{i}"], state, options ) );
            }

            var methodInfo = state.SignatureHelper.GetMethodInfoFromSignature( expression.Values["Method"] );

            if ( methodInfo == null )
            {
                throw new MethodNotFoundException( "A matching method was not found." );
            }

            if ( !options.IsMethodSafe( methodInfo ) )
            {
                throw new UnsafeMethodCallException( "Attempted to decode an unsafe method call.", methodInfo );
            }

            return Expression.Call( objectExpression, methodInfo, arguments );
        }

        #endregion
    }
}
