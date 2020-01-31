using System;
using System.Linq.Expressions;

using NUnit.Framework;

namespace BlueBoxMoon.Linqson.Tests
{
    public class EncodedExpressionTests
    {
        [Test]
        public void EncodeExpression_UnsupportedNodeType()
        {
            var sdi = Expression.SymbolDocument( "test.xml" );
            var debugInfo = Expression.DebugInfo( sdi, 1, 1, 2, 2 );

            Assert.Throws<ArgumentException>( () => EncodedExpression.EncodeExpression( debugInfo ) );
        }

        [Test]
        public void DecodeExpression_UnsupportedNodeType()
        {
            var encoded = new EncodedExpression() { NodeType = ExpressionType.DebugInfo };

            Assert.Throws<ArgumentException>( () => EncodedExpression.DecodeExpression( encoded ) );
        }
    }
}
