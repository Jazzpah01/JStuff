using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.Interpreter
{
    public class Interpreter
    {
        private Lexer lexer;
        private Parser parser;

        public Interpreter(Lexer lexer, Parser parser)
        {
            this.lexer = lexer;
            this.parser = parser;
        }

        public AbSynTree.Element Interpret(string s)
        {
            return parser.Parse(lexer.Lex(s));
        }

        public AbSynTree.Element Convert(string s)
        {
            string p = "";
            foreach(Token token in lexer.Lex(s))
            {
                p += token.tokenType.ToString() + ": " + token.value + ";";
            }
            return parser.Parse(lexer.Lex(s));
        }
    }
}