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
using System.Linq.Expressions;
using System.Text.Json;

namespace BlueBoxMoon.Linqson.SystemJson
{
    /// <summary>
    /// Handles serializing and deserializing LINQ expressions to JSON using
    /// the System.Text.Json package.
    /// </summary>
    public class SystemJsonExpressionSerializer
    {
        /// <summary>
        /// Serializes a LINQ expression into a JSON string.
        /// </summary>
        /// <param name="expression">The expression to be serialized.</param>
        /// <returns>A string.</returns>
        public string Serialize( Expression expression )
        {
            var encoded = EncodedExpression.EncodeExpression( expression );

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add( new EncodedExpressionConverterFactory() );
            serializerOptions.WriteIndented = true;

            return JsonSerializer.Serialize( encoded, serializerOptions );
        }

        /// <summary>
        /// Deserializes a LINQ expression from a JSON string.
        /// </summary>
        /// <param name="json">A string that containts the JSON data.</param>
        /// <returns>A LINQ expression object.</returns>
        public Expression Deserialize( string json )
        {
            var serializerOptions = new JsonSerializerOptions();

            serializerOptions.Converters.Add( new EncodedExpressionConverterFactory() );
            serializerOptions.WriteIndented = true;

            var encoded = JsonSerializer.Deserialize<EncodedExpression>( json, serializerOptions );

            return EncodedExpression.DecodeExpression( encoded );
        }
    }
}
