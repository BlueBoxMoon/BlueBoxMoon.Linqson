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
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlueBoxMoon.Linqson.SystemJson
{
    /// <summary>
    /// A converter for all <see cref="EncodedExpression"/> objects.
    /// </summary>
    public class EncodedExpressionConverter : JsonConverter<EncodedExpression>
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
        public override EncodedExpression Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
        {
            if ( reader.TokenType != JsonTokenType.StartObject )
            {
                throw new JsonException();
            }

            EncodedExpression expr = null;

            while ( reader.Read() )
            {
                //
                // We have read the entire object.
                //
                if ( reader.TokenType == JsonTokenType.EndObject )
                {
                    return expr;
                }

                //
                // Should always start the loop with a property name.
                //
                if ( reader.TokenType != JsonTokenType.PropertyName )
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();

                if ( propertyName == "$T" )
                {
                    //
                    // Class type definition.
                    //
                    reader.Read();
                    var typeName = reader.GetString();
                    expr = ( EncodedExpression ) Activator.CreateInstance( Type.GetType( typeName ) );
                }
                else
                {
                    //
                    // If we never found a $T object then something is invalid in the stream.
                    //
                    if ( expr == null )
                    {
                        throw new JsonException();
                    }

                    //
                    // Read the property from the object.
                    //
                    var propertyInfo = expr.GetType().GetProperty( propertyName );
                    if ( propertyInfo == null )
                    {
                        throw new JsonException();
                    }

                    //
                    // Deserialize and store value.
                    //
                    var v = JsonSerializer.Deserialize( ref reader, propertyInfo.PropertyType, options );

                    propertyInfo.SetValue( expr, v );
                }
            }

            throw new JsonException();
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write( Utf8JsonWriter writer, EncodedExpression value, JsonSerializerOptions options )
        {
            writer.WriteStartObject();

            writer.WriteString( "$T", value.GetType().AssemblyQualifiedName );

            foreach ( var property in value.GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public ) )
            {
                var propertyValue = property.GetValue( value );
                writer.WritePropertyName( property.Name );
                JsonSerializer.Serialize( writer, propertyValue, propertyValue != null ? propertyValue.GetType() : property.PropertyType, options );
            }

            writer.WriteEndObject();
        }
    }
}
