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
using Newtonsoft.Json;

namespace BlueBoxMoon.Linqson.JsonNet
{
    /// <summary>
    /// Handles dynamic type conversion when serializing or deserializing
    /// with the Newtonsoft JSON library.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class EncodedExpressionConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert( Type objectType )
        {
            return typeof( EncodedExpression ).IsAssignableFrom( objectType );
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="JsonException">
        /// </exception>
        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            if ( reader.TokenType != JsonToken.StartObject )
            {
                throw new JsonException();
            }

            EncodedExpression expr = null;

            while ( reader.Read() )
            {
                //
                // We have read the entire object.
                //
                if ( reader.TokenType == JsonToken.EndObject )
                {
                    return expr;
                }

                //
                // Should always start the loop with a property name.
                //
                if ( reader.TokenType != JsonToken.PropertyName )
                {
                    throw new JsonException();
                }

                var propertyName = ( string ) reader.Value;

                if ( propertyName == "$T" )
                {
                    //
                    // Class type definition.
                    //
                    reader.Read();
                    var typeName = ( string ) reader.Value;
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
                    reader.Read();
                    var v = serializer.Deserialize( reader, propertyInfo.PropertyType );

                    propertyInfo.SetValue( expr, v );
                }
            }

            throw new JsonException();
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            writer.WriteStartObject();

            writer.WritePropertyName( "$T" );
            writer.WriteValue( value.GetType().AssemblyQualifiedName );

            foreach ( var property in value.GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public ) )
            {
                var propertyValue = property.GetValue( value );
                writer.WritePropertyName( property.Name );

                serializer.Serialize( writer, propertyValue, propertyValue != null ? propertyValue.GetType() : property.PropertyType );
            }

            writer.WriteEndObject();
        }
    }
}
