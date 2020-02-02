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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlueBoxMoon.Linqson
{
    /// <summary>
    /// A helper for dealing with <see cref="Type"/> and signatures that allow
    /// them to be re-created later.
    /// </summary>
    public partial class TypeSignatureHelper
    {
        /// <summary>
        /// Gets a type from the signature definition.
        /// </summary>
        /// <param name="signatureDefinition">The signature that identifies this type.</param>
        /// <returns>A <see cref="Type"/> or <c>null</c> if not found.</returns>
        public Type GetTypeFromNameAndAssembly( string typeName, string assemblyName )
        {
            if ( string.IsNullOrWhiteSpace( typeName ) )
            {
                return null;
            }

            //
            // Check if this is a generic method argument.
            //
            if ( typeName.StartsWith( "!!" ) )
            {
                return MakeGenericMethodParameter( int.Parse( typeName.Substring( 2 ) ) );
            }

            //
            // If we have an assembly name, then try to load from the assembly.
            //
            if ( !string.IsNullOrWhiteSpace( assemblyName ) )
            {
                Type type = null;

                //
                // If this isn't a system assembly, then first try to load from
                // an assembly that matches the name.
                //
                if ( assemblyName != "mscorlib" && assemblyName != "System.Private.CoreLib" )
                {
                    var assembly = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .FirstOrDefault( a => a.GetName().Name == assemblyName );

                    if ( assembly != null )
                    {
                        type = assembly.GetTypes()
                            .FirstOrDefault( a => typeName == $"{a.Namespace}.{a.Name}" );
                    }
                }

                //
                // Okay, no luck there. Try standard system libraries.
                //
                if ( type == null )
                {
                    type = Type.GetType( typeName );
                }

                //
                // Still nothing? Come on man, work with me! Brute force search all
                // assemblies. It's possible to have conflicts, but hope for the best.
                // We do this because there are some differences between frameworks in
                // .NET and types can exist in different assemblies on different platforms.
                //
                if ( type == null )
                {
                    type = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany( a => a.GetTypes() )
                        .FirstOrDefault( a => typeName == $"{a.Namespace}.{a.Name}" );
                }

                return type;
            }
            else
            {
                return Type.GetType( typeName );
            }
        }

        /// <summary>
        /// Get a single type from a signature.
        /// </summary>
        /// <param name="signature">The signature of the type.</param>
        /// <returns>A <see cref="Type"/>.</returns>
        public Type GetTypeFromSignature( string signature )
        {
            return GetTypeFromSignature( new StringReader( signature ) );
        }

        /// <summary>
        /// Gets one or more types from a signature.
        /// </summary>
        /// <param name="signature">The signatures of the types.</param>
        /// <returns>A collection of <see cref="Type"/>.</returns>
        public Type[] GetTypesFromSignature( string signature )
        {
            return GetTypesFromSignature( new StringReader( signature ) );
        }

        /// <summary>
        /// Gets a method from a signature.
        /// </summary>
        /// <param name="signature">The signature of the method.</param>
        /// <returns>A <see cref="MethodInfo"/> instance or <c>null</c> if not found.</returns>
        public MethodInfo GetMethodInfoFromSignature( string signature )
        {
            return GetMethodInfoFromSignature( new StringReader( signature ) );
        }

        /// <summary>
        /// Gets a signature that can be later used to find this type again.
        /// </summary>
        /// <param name="type">The type to build a signature for.</param>
        /// <returns>A string the represents the type.</returns>
        public string GetSignatureFromType( Type type )
        {
            var typeInfo = type.GetTypeInfo();

            //
            // Check and return the signature for a generic method parameter.
            //
            if ( typeInfo.IsGenericParameter /*IsGenericMethodParameter*/ )
            {
                return $"{{!!{type.GenericParameterPosition}}}";
            }

            var sb = new StringBuilder( $"{{{type.Namespace}.{type.Name}" );

            //
            // If this type is a generic type, add in the generic type arguments.
            //
            if ( typeInfo.IsGenericType )
            {
                sb.Append( "<" );
                sb.Append( GetSignatureFromTypes( typeInfo.GenericTypeArguments/*.GetGenericArguments()*/ ) );
                sb.Append( ">" );
            }

            //
            // Add in the assembly name.
            //
            sb.Append( $", {typeInfo.Assembly.GetName().Name}}}" );

            return sb.ToString();
        }

        /// <summary>
        /// Get the signature for multiple types.
        /// </summary>
        /// <param name="types">The types whose signature is to be generated.</param>
        /// <returns>A string that represents the types.</returns>
        public string GetSignatureFromTypes( IEnumerable<Type> types )
        {
            return string.Join( ",", types.Select( a => GetSignatureFromType( a ) ) );
        }

        /// <summary>
        /// Get the signature for the given <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The method whose signature is to be built.</param>
        /// <returns>A string containing the signature of the method.</returns>
        public string GetSignatureFromMethodInfo( MethodInfo methodInfo )
        {
            var sb = new StringBuilder();

            sb.Append( methodInfo.Name );

            //
            // If this method is a generic, then note the number of generic
            // arguments.
            //
            if ( methodInfo.IsGenericMethodDefinition )
            {
                sb.Append( $"<{methodInfo.GetGenericArguments().Length}>" );
            }

            //
            // Add in the declaring type so we know what type this method
            // came from later.
            //
            sb.Append( $"@{GetSignatureFromType( methodInfo.DeclaringType )}" );

            //
            // Add in any method parameters.
            //
            sb.Append( "(" );
            sb.Append( GetSignatureFromTypes( methodInfo.GetParameters().Select( a => a.ParameterType ) ) );
            sb.Append( ")" );

            return sb.ToString();
        }

        /// <summary>
        /// Get a single type from the signature reader.
        /// </summary>
        /// <param name="reader">The reader containing the signature.</param>
        /// <returns>
        /// A <see cref="Type" />.
        /// </returns>
        /// <exception cref="BlueBoxMoon.Linqson.SignatureInvalidException">
        /// Missing opening token.
        /// or
        /// Missing closing generic types token.
        /// or
        /// Invalid type signature.
        /// </exception>
        /// <exception cref="TypeNotFoundException">The type specified in the signature was not found.</exception>
        private Type GetTypeFromSignature( StringReader reader )
        {
            if ( reader.Read() != '{' )
            {
                throw new SignatureInvalidException( "Missing opening token." );
            }

            Type[] typeArguments = null;
            string typeName = GetNameIdentifier( reader );
            string assemblyName = string.Empty;

            while ( true )
            {
                var c = reader.Read();

                //
                // If this is the end of our signature then process all the
                // values we've parsed.
                //
                if ( c == '}' )
                {
                    var type = GetTypeFromNameAndAssembly( typeName, assemblyName );

                    if ( type != null && typeArguments != null )
                    {
                        type = type.MakeGenericType( typeArguments.ToArray() );
                    }

                    if ( type == null )
                    {
                        throw new TypeNotFoundException( "The type specified in the signature was not found." );
                    }

                    return type;
                }

                //
                // Check for the existence of any generic type arguments.
                //
                else if ( c == '<' )
                {
                    typeArguments = GetTypesFromSignature( reader );

                    if ( reader.Read() != '>' )
                    {
                        throw new SignatureInvalidException( "Missing closing generic types token." );
                    }
                }

                //
                // Check for an assembly name.
                //
                else if ( c == ',' )
                {
                    assemblyName = GetNameIdentifier( reader ).Trim();
                }

                //
                // Shouldn't get here, but check just in case.
                //
                else
                {
                    throw new SignatureInvalidException( "Invalid type signature." );
                }
            }
        }

        /// <summary>
        /// Gets one or more types from the signature.
        /// </summary>
        /// <param name="reader">The reader containing the signature.</param>
        /// <returns>A collection of <see cref="Type"/>.</returns>
        private Type[] GetTypesFromSignature( StringReader reader )
        {
            var types = new List<Type>();

            do
            {
                if ( reader.Peek() == ',' )
                {
                    reader.Read();
                }

                types.Add( GetTypeFromSignature( reader ) );
            } while ( reader.Peek() == ',' );

            return types.ToArray();
        }

        /// <summary>
        /// Gets a method from the signature.
        /// </summary>
        /// <param name="reader">The reader containing the signature.</param>
        /// <returns>A <see cref="MethodInfo"/> instance or <c>null</c> if not found.</returns>
        private MethodInfo GetMethodInfoFromSignature( StringReader reader )
        {
            string methodName = GetNameIdentifier( reader );
            int genericParameterCount = 0;
            Type declaringType = null;

            while ( reader.Peek() != -1 )
            {
                var c = reader.Read();

                //
                // Check for the declaring type token.
                //
                if ( c == '@' )
                {
                    declaringType = GetTypeFromSignature( reader );
                }

                //
                // Check for any generic arguments.
                //
                else if ( c == '<' )
                {
                    string s = GetNameIdentifier( reader );

                    if ( reader.Read() != '>' )
                    {
                        throw new SignatureInvalidException( "Missing closing generic type token." );
                    }

                    genericParameterCount = int.Parse( s );
                }

                //
                // Check for any parameters, this also marks the end of our
                // signature.
                //
                else if ( c == '(' )
                {
                    Type[] parameterTypes;

                    if ( reader.Peek() != ')' )
                    {
                        parameterTypes = GetTypesFromSignature( reader );
                    }
                    else
                    {
                        parameterTypes = new Type[0];
                    }

                    if ( reader.Read() != ')' )
                    {
                        throw new SignatureInvalidException( "Missing closing method parameter token." );
                    }

                    return GetMethod( declaringType, methodName, genericParameterCount, parameterTypes );
                }

                //
                // Shouldn't ever get here.
                //
                else
                {
                    throw new SignatureInvalidException( "Invalid method signature." );
                }
            }

            throw new SignatureInvalidException( "Invalid method signature." );
        }

        /// <summary>
        /// Determines if the two types are equal to each other.
        /// </summary>
        /// <param name="type">The first type.</param>
        /// <param name="otherType">The second type.</param>
        /// <returns><c>true</c> if they are deemed equal; otherwise <c>false</c>.</returns>
        internal bool AreTypesEqual( Type type, Type otherType )
        {
            if ( type.IsGenericParameter != otherType.IsGenericParameter )
            {
                return false;
            }

            if ( type.GenericTypeArguments.Length != otherType.GenericTypeArguments.Length )
            {
                return false;
            }

            if ( type.IsGenericParameter )
            {
                if ( type.GenericParameterPosition != otherType.GenericParameterPosition )
                {
                    return false;
                }
            }
            else if ( type.GenericTypeArguments.Length > 0 )
            {
                var genericType = type.GetGenericTypeDefinition();
                var otherGenericType = otherType.GetGenericTypeDefinition();

                if ( genericType != otherGenericType )
                {
                    return false;
                }

                var genericArguments = type.GenericTypeArguments;
                var otherGenericArguments = otherType.GenericTypeArguments;

                for ( int i = 0; i < genericArguments.Length; i++ )
                {
                    if ( !AreTypesEqual( genericArguments[i], otherGenericArguments[i] ) )
                    {
                        return false;
                    }
                }
            }
            else if ( type != otherType )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reads a single name identifier from the reader.
        /// </summary>
        /// <param name="reader">The reader containing the contents.</param>
        /// <returns>A string containing the name.</returns>
        private string GetNameIdentifier( StringReader reader )
        {
            var sb = new StringBuilder( 100 );

            var c = reader.Peek();
            while ( c != -1 && c != ',' && c != '<' && c != '>' && c != '(' && c != ')' && c != '{' && c != '}' && c != '@' )
            {
                reader.Read();
                sb.Append( ( char ) c );
                c = reader.Peek();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the method matching the criteria.
        /// </summary>
        /// <param name="type">The type that declares the method.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="genericParameterCount">The generic parameter count.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <returns>A <see cref="MethodInfo"/> instance or <c>null</c> if no match was found.</returns>
        private MethodInfo GetMethod( Type type, string name, int genericParameterCount, Type[] parameterTypes )
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            var methods = type.GetMethods( bindingFlags )
                .Where( a => a.Name == name );

            if ( genericParameterCount == 0 )
            {
                methods = methods.Where( a => !a.ContainsGenericParameters );
            }
            else
            {
                methods = methods.Where( a => a.GetGenericArguments().Length == genericParameterCount );
            }

            methods = methods.Where( a => a.GetParameters().Length == parameterTypes.Length )
                .Where( a =>
                {
                    var methodParameters = a.GetParameters().Select( b => b.ParameterType ).ToList();
                    for ( int i = 0; i < methodParameters.Count; i++ )
                    {
                        if ( !AreTypesEqual( methodParameters[i], parameterTypes[i] ) )
                        {
                            return false;
                        }
                    }

                    return true;
                } );

            var method = methods.SingleOrDefault();

            return method;
        }
    }

#if NETCOREAPP
    public partial class TypeSignatureHelper
    {
        protected Type MakeGenericMethodParameter( int position )
        {
            return Type.MakeGenericMethodParameter( position );
        }
    }
#elif NETSTANDARD || NETFULL
    public partial class TypeSignatureHelper
    {
        protected Type MakeGenericMethodParameter( int position )
        {
            return new GenericMethodParameterType( position );
        }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class GenericMethodParameterType : Type
    {
        public override int GenericParameterPosition => _position;

        public override bool IsGenericParameter => true;

        private readonly int _position;

        public GenericMethodParameterType( int position )
        {
            _position = position;
        }

    #region Not Implemented

        public override Assembly Assembly => throw new NotImplementedException();

        public override string AssemblyQualifiedName => throw new NotImplementedException();

        public override Type BaseType => throw new NotImplementedException();

        public override string FullName => throw new NotImplementedException();

        public override Guid GUID => throw new NotImplementedException();

        public override Module Module => throw new NotImplementedException();

        public override string Namespace => throw new NotImplementedException();

        public override Type UnderlyingSystemType => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public override ConstructorInfo[] GetConstructors( BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes( bool inherit )
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes( Type attributeType, bool inherit )
        {
            throw new NotImplementedException();
        }

        public override Type GetElementType()
        {
            throw new NotImplementedException();
        }

        public override EventInfo GetEvent( string name, BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override EventInfo[] GetEvents( BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override FieldInfo GetField( string name, BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override FieldInfo[] GetFields( BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override Type GetInterface( string name, bool ignoreCase )
        {
            throw new NotImplementedException();
        }

        public override Type[] GetInterfaces()
        {
            throw new NotImplementedException();
        }

        public override MemberInfo[] GetMembers( BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override MethodInfo[] GetMethods( BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override Type GetNestedType( string name, BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override Type[] GetNestedTypes( BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override PropertyInfo[] GetProperties( BindingFlags bindingAttr )
        {
            throw new NotImplementedException();
        }

        public override object InvokeMember( string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters )
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined( Type attributeType, bool inherit )
        {
            throw new NotImplementedException();
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            throw new NotImplementedException();
        }

        protected override ConstructorInfo GetConstructorImpl( BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers )
        {
            throw new NotImplementedException();
        }

        protected override MethodInfo GetMethodImpl( string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers )
        {
            throw new NotImplementedException();
        }

        protected override PropertyInfo GetPropertyImpl( string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers )
        {
            throw new NotImplementedException();
        }

        protected override bool HasElementTypeImpl()
        {
            return false;
            throw new NotImplementedException();
        }

        protected override bool IsArrayImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsByRefImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsCOMObjectImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPointerImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPrimitiveImpl()
        {
            throw new NotImplementedException();
        }

    #endregion
    }
#else
#error Unsupported SDK framework.
#endif
}
