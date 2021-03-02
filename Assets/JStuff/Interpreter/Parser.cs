using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.Interpreter
{
    public enum TokenType
    {
        START,
        END,
        VAR,
        RPAR,
        LPAR,
        AND,
        OR,
        EQ,
        NEQ,
        SUBTRACT,
        ADD,
        MULTIPLY,
        DIVIDE,
        VAL,
        DAMAGE,
        HEAL,
        INSTANT,
        PROJECTILE,
        SEPERATOR,
        STR,
        APPLY,
        CARD,
        SPAWN,
        HASTAG,
        HOMING,
        NOT,
        HASFLAG,
        ISALIVE,
        HEALTH,
        MANA,
        CUSTOMVAR,
        DOT,
        SOURCE,
        TARGETTYPE,
        MULTIEFFECT,
        MULTIAPPLY,
        SPECIALEFFECT,
        BOOLEAN,
        FORALLUNITS,
        ELEMENT,
        POWER,
        PROTECTION,
        INCREASE,
        DECREASE,
        DRAW,
        ASSIGN,
        BONUS,
        AOE,
        TREE,
        TRIGGER
    }

    public class Token
    {
        public object value;
        public TokenType tokenType;
        public Token(TokenType type)
        {
            this.tokenType = type;
        }
        public Token(TokenType type, string value)
        {
            this.tokenType = type;
            this.value = value;
        }
        public override string ToString()
        {
            string retval = tokenType.ToString() + "; " + base.ToString();

            if (value != null)
                retval += "; " + value;

            return retval;
        }
    }

    public abstract class Parser
    {
        public virtual AbSynTree.Element Parse(List<Token> tokens)
        {
            if (tokens[0].tokenType != TokenType.START || tokens[tokens.Count - 1].tokenType != TokenType.END)
                throw new Exception();

            return CreateAbsynTree(tokens, 1, tokens.Count - 2);
        }

        protected abstract AbSynTree.Element CreateAbsynTree(List<Token> token, int start, int end);

        protected virtual int LowestPrecedence(List<Token> token, int start, int end)
        {
            int index = start;
            int depth = 0;
            for (int i = start; i <= end; i++)
            {
                if (depth < 0)
                    throw new Exception("Cannot have negative depth. Depth: " + depth + ". Token: " + token[i] + ". (start;end;i): " + (start,end,i));
                if (depth == 0 && Precedence(token[i].tokenType) <= Precedence(token[index].tokenType))
                    index = i;
                if (token[i].tokenType == TokenType.LPAR)
                    depth++;
                if (token[i].tokenType == TokenType.RPAR)
                    depth--;
            }
            if (depth > 0)
                throw new Exception("Cannot have positive depth at the end of token list. Token: " + token[start]);

            return index;
        }

        protected abstract int Precedence(TokenType type);
    }
}