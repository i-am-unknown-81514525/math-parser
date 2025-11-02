using System.Collections.Generic;
using System.Text;
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

        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{ParseResultExtensions.Indent(indent)}EqResult:");
            sb.AppendLine($"{ParseResultExtensions.Indent(indent + 1)}Comparison: {comparsionAtom.literal}");
            sb.AppendLine($"{ParseResultExtensions.Indent(indent + 1)}Expression (LHS - RHS):");
            sb.Append(exprs.ToString(indent + 2));
            return sb.ToString();
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