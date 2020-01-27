﻿// MIT License
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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlueBoxMoon.Linqson.SystemJson
{
    /// <summary>
    /// A converter for all <see cref="List{T}"/> collections.
    /// </summary>
    /// <typeparam name="T">A subclass type of <see cref="EncodedExpression"/>.</typeparam>
    public class EncodedExpressionListConverter<T> : JsonConverter<List<T>>
        where T : EncodedExpression
    {
        /// <summary>
        /// When overridden in a derived class, determines whether the converter
        /// instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
        /// <returns><c>true</c> if the instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert( Type typeToConvert )
        {
            return true;
        }

        /// <summary>
        /// Reads and converts the JSON to type <typeparamref name="T" />.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override List<T> Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
        {
            if ( reader.TokenType != JsonTokenType.StartArray )
            {
                throw new JsonException();
            }

            var expressions = new List<T>();
            while ( reader.Read() )
            {
                if ( reader.TokenType == JsonTokenType.EndArray )
                {
                    return expressions;
                }

                var expr = ( T ) JsonSerializer.Deserialize<EncodedExpression>( ref reader, options );
                expressions.Add( expr );
            }

            throw new JsonException();
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write( Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options )
        {
            writer.WriteStartArray();

            foreach ( var expr in value )
            {
                JsonSerializer.Serialize( writer, ( EncodedExpression ) expr, options );
            }

            writer.WriteEndArray();
        }
    }
}