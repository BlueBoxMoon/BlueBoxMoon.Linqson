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
#if false
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlueBoxMoon.Linqson.SystemJson
{
    /// <summary>
    /// Factory to provide any <see cref="JsonConverter{T}"/> instances that
    /// will handle our custom types.
    /// </summary>
    public class EncodedExpressionConverterFactory : JsonConverterFactory
    {
        /// <summary>
        /// When overridden in a derived class, determines whether the converter
        /// instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
        /// <returns><c>true</c> if the instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert( Type typeToConvert )
        {
            if ( typeToConvert.IsGenericType )
            {
                var genericType = typeToConvert.GetGenericTypeDefinition();
                var genericArguments = typeToConvert.GetGenericArguments();

                return genericType == typeof( List<> ) && genericArguments.Length == 1 && typeof( EncodedExpression ).IsAssignableFrom( genericArguments[0] );
            }

            return typeof( EncodedExpression ).IsAssignableFrom( typeToConvert );
        }

        /// <summary>
        /// Creates a converter for a specified type.
        /// </summary>
        /// <param name="typeToConvert">The type handled by the converter.</param>
        /// <param name="options">The serialization options to use.</param>
        public override JsonConverter CreateConverter( Type typeToConvert, JsonSerializerOptions options )
        {
            if ( typeToConvert.IsGenericType )
            {
                var genericType = typeToConvert.GetGenericTypeDefinition();
                var genericArguments = typeToConvert.GetGenericArguments();

                if ( genericType == typeof( List<> ) )
                {
                    var t = typeof( EncodedExpressionListConverter<> ).MakeGenericType( genericArguments[0] );
                    return ( JsonConverter ) Activator.CreateInstance( t );
                }
            }
            else
            {
                return new EncodedExpressionConverter();
            }

            throw new JsonException();
        }
    }
}
#endif