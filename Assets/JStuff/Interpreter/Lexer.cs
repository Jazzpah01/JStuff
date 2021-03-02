using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;


namespace JStuff.Interpreter
{
    public abstract class Lexer
    {
        protected List<TokenDefinition> tokenDefs = new List<TokenDefinition>();
        protected string signs;
        protected string divider;

        public struct TokenStringTuple // tuple for return value
        {
            public Token token;
            public string str;
        }

        protected void CreateTokenDifinition(TokenType token, char sign)
        {
            tokenDefs.Add(new TokenDefinition(token, sign, divider));
        }

        protected void CreateTokenDifinition(TokenType token, string regex, bool hasValue = false)
        {
            tokenDefs.Add(new TokenDefinition(token, regex, divider, hasValue));
        }

        protected class TokenDefinition
        {
            public TokenType tokenType;
            public string regex;
            public bool hasValue;

            public TokenDefinition(TokenType token, char sign, string divider)
            {
                this.tokenType = token;
                string str = sign.ToString();
                this.regex = "^" + "[" + str + "]" + divider;
            }
            public TokenDefinition(TokenType token, string regex, string divider, bool hasValue = false)
            {
                this.tokenType = token;
                this.regex = "^" + regex + divider;
                this.hasValue = hasValue;
            }
        }

        public virtual List<Token> Lex(string str)
        {
            List<Token> tokens = new List<Token>();
            tokens.Add(new Token(TokenType.START));

            str = Filter(str);

            int i = 0;
            while (str != divider && str != "" && i < 1000)
            {
                (Token token, string str) h = Inc(str);
                str = h.str;
                tokens.Add(h.token);
                i++;
            }

            tokens.Add(new Token(TokenType.END));

            return tokens;
        }

        protected virtual string Filter(string str)
        {
            string retval = str + divider;
            retval = retval.Replace(" ", divider);
            retval = retval.Replace("\n", divider);
            retval = retval.Replace("\r", divider);

            foreach (char c in signs)
            {
                retval = retval.Replace(c.ToString(), divider + c + divider);
            }

            bool repeat = true;
            int i = 0;
            while (repeat && i < 1000)
            {
                int count = retval.Length;
                retval = retval.Replace(divider+divider, divider);
                if (count == retval.Length)
                    repeat = false;
                i++;
            }

            if (retval[0] == divider[0])
                return retval.Substring(1);
            return retval;
        }

        protected virtual (Token t, string s) Inc(string str)
        {
            if (str == "")
                throw new Exception("Cannot take an empty string!");

            int i;
            (Token token, string str) tuple;

            foreach (TokenDefinition t in tokenDefs)
            {
                Regex regex = new Regex(t.regex);
                if (regex.IsMatch(str))
                {
                    i = Next(str);

                    if (t.hasValue)
                    {
                        tuple.token = new Token(t.tokenType, str.Substring(0, i));
                    }
                    else
                    {
                        tuple.token = new Token(t.tokenType);
                    }

                    tuple.str = str.Substring(i + 1);
                    return tuple;
                }
            }
            throw new Exception("String s does not match definition type: " + str);
        }

        protected int Next(string str)
        {
            if (str.Length < 1)
                throw new Exception("Can't iterate an empty string!");

            int i = 0;
            char count = str[0];
            while (count != divider[0] && i < 1000)
            {
                i++;
                count = str[i];
            }
            //Debug.Log(str);
            //Debug.Log(i);
            return i;
        }
    }
}
