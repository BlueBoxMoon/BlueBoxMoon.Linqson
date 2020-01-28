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
using System.Reflection;

namespace BlueBoxMoon.Linqson
{
    /// <summary>
    /// The options associated with a decode operation.
    /// </summary>
    public class DecodeOptions
    {
        #region Properties

        /// <summary>
        /// If <c>true</c> then all method calls will be allowed; otherwise
        /// only those on the safe-list will be allowed.
        /// </summary>
        public bool AllowUnsafeCalls { get; set; }

        /// <summary>
        /// Gets or sets the static types that are considered safe. All
        /// static methods on these types will be allowed.
        /// </summary>
        /// <value>
        /// The static types that are considered safe. All static methods
        /// on these types will be allowed.
        /// </value>
        public List<Type> SafeStaticMethodTypes { get; set; }

        /// <summary>
        /// Gets or sets the instance types that are considered safe. All
        /// instance methods on these types will be allowed.
        /// </summary>
        /// <value>
        /// The instance types that are considered safe. All instance methods
        /// on these types will be allowed.
        /// </value>
        public List<Type> SafeInstanceMethodTypes { get; set; }

        /// <summary>
        /// Gets or sets the method declarations that are safe to call.
        /// </summary>
        /// <value>
        /// The method declarations that are safe to call.
        /// </value>
        public List<MethodInfo> SafeMethods { get; set; }

        #endregion

        #region Constructors

        public DecodeOptions()
        {
            SafeStaticMethodTypes = new List<Type>();
            SafeInstanceMethodTypes = new List<Type>();
            SafeMethods = new List<MethodInfo>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the method is safe to call.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>
        ///   <c>true</c> if the method is safe to call; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsMethodSafe( MethodInfo methodInfo )
        {
            if ( AllowUnsafeCalls )
            {
                return true;
            }
            else if ( methodInfo.IsStatic )
            {
                if ( SafeStaticMethodTypes?.Contains( methodInfo.DeclaringType ) ?? false )
                {
                    return true;
                }
            }
            else
            {
                if ( SafeInstanceMethodTypes?.Contains( methodInfo.DeclaringType ) ?? false )
                {
                    return true;
                }
            }

            return SafeMethods.Contains( methodInfo );
        }

        #endregion
    }
}
