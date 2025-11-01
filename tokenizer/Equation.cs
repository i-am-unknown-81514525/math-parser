using System.Collections.Generic;
using math_parser;

namespace math_parser.tokenizer
{
    public class EqResult : ParseResult
    {
        public readonly ExprResult exprs;
        public readonly atom.ComparsionSymbolAtom comparsionAtom;

        public EqResult(ExprResult result, atom.ComparsionSymbolAtom atom)
        {
            this.exprs = result;
            this.comparsionAtom = atom;
        }
    }

    public class Equation : Group<TokenSequenceResult<ParseResult>, EqResult>
    {
        public static Dictionary<string, atom.ComparsionSymbolAtom> mapping = new Dictionary<string, atom.ComparsionSymbolAtom>()
        {
            {"=", atom.ComparsionSymbolAtom.Eq},
            {">=", atom.ComparsionSymbolAtom.Ge},
            {"<=", atom.ComparsionSymbolAtom.Le}
        };

        public Equation() : base(new TokenSequence<ParseResult>(
            new PotentialSpace(),
            new Expression(),
            new PotentialSpace(),
            new ComparsionSymbolAtom(),
            new PotentialSpace(),
            new Expression(),
            new PotentialSpace()
        )) { }

        public override EqResult Parse(CharacterStream stream)
        {
            TokenSequenceResult<ParseResult> results = inner_token.Parse(stream);
            ExprResult left = (ExprResult)results.parseResult[1];
            ExprResult right = (ExprResult)results.parseResult[5];
            MathLiteralResult mathLiteral = (MathLiteralResult)results.parseResult[3];
            return new EqResult(left - right, mapping[mathLiteral.literal]);
        }
    }
}