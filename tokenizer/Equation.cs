using System.Collections.Generic;
using System.Text;
using math_parser;

namespace math_parser.tokenizer
{
    public class EqResult : ParseResult
    {
        public readonly ExprResult Exprs;
        public readonly atom.ComparsionSymbolAtom ComparsionAtom;

        public EqResult(ExprResult result, atom.ComparsionSymbolAtom atom)
        {
            Exprs = result;
            ComparsionAtom = atom;
        }

        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{ParseResultExtensions.Indent(indent)}EqResult:");
            sb.AppendLine($"{ParseResultExtensions.Indent(indent + 1)}Comparison: {ComparsionAtom.Literal}");
            sb.AppendLine($"{ParseResultExtensions.Indent(indent + 1)}Expression (LHS - RHS):");
            sb.Append(Exprs.ToString(indent + 2));
            return sb.ToString();
        }
    }

    public class Equation : Group<TokenSequenceResult<ParseResult>, EqResult>
    {
        public static Dictionary<string, atom.ComparsionSymbolAtom> Mapping = new Dictionary<string, atom.ComparsionSymbolAtom>()
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
            TokenSequenceResult<ParseResult> results = InnerToken.Parse(stream);
            ExprResult left = (ExprResult)results.ParseResult[1];
            ExprResult right = (ExprResult)results.ParseResult[5];
            MathLiteralResult mathLiteral = (MathLiteralResult)results.ParseResult[3];
            return new EqResult(left - right, Mapping[mathLiteral.Literal]);
        }
    }
}