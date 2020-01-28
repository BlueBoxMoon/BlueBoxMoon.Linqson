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
    /// A parameter expression that can be serialized.
    /// </summary>
    public class EncodedParameterExpression : EncodedExpression
    {
        #region Properties

        /// <summary>
        /// The type that this parameter represents.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The name of the parameter used by other expressions to reference
        /// the value of the parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the <see cref="EncodedParameterExpression"/> class.
        /// </summary>
        public EncodedParameterExpression()
        {
        }

        /// <summary>
        /// Instantiates a new instance of the <see cref="EncodedParameterExpression"/> class.
        /// </summary>
        /// <param name="expression">The original expression.</param>
        internal EncodedParameterExpression( ParameterExpression expression )
        {
            NodeType = expression.NodeType;
            Type = expression.Type.AssemblyQualifiedName;
            Name = expression.Name;
        }

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
        internal static EncodedParameterExpression EncodeExpression( ParameterExpression expression, EncodeState state, EncodeOptions options )
        {
            return state.GetOrAddParameter( expression );
        }

        /// <summary>
        /// Decodes the expression back into a LINQ expression.
        /// </summary>
        /// <param name="state">The state tracker for the decode operation.</param>
        /// <param name="options">The options that will be used during decoding.</param>
        /// <returns>A LINQ <see cref="Expression"/> instance.</returns>
        internal protected override Expression DecodeExpression( DecodeState state, DecodeOptions options )
        {
            return state.GetOrAddParameter( this );
        }

        #endregion
    }
}
