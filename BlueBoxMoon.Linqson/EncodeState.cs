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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlueBoxMoon.Linqson
{
    /// <summary>
    /// Defines the current state of an operation to encode LINQ Expressions
    /// into JSON Expressions.
    /// </summary>
    public class EncodeState
    {
        #region Fields

        /// <summary>
        /// The current Lambda parameters in effect.
        /// </summary>
        private Dictionary<ParameterExpression, EncodedParameterExpression> _parameters { get; } = new Dictionary<ParameterExpression, EncodedParameterExpression>();

        #endregion

        #region Methods

        /// <summary>
        /// Gets the <see cref="EncodedParameterExpression"/> associated with the name.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>A <see cref="EncodedParameterExpression"/> instance.</returns>
        public EncodedParameterExpression GetOrAddParameter( ParameterExpression parameter )
        {
            if ( !_parameters.ContainsKey( parameter ) )
            {
                var p = new EncodedParameterExpression( parameter );
                _parameters.Add( parameter, p );
            }

            return _parameters[parameter];
        }

        #endregion
    }
}
