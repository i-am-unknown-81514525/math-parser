using math_parser.tokenizer;
using System;
using System.Collections.Generic;
using System.IO;

namespace test{
    public class Program
    {
        public static void Main(string[] args)
        {
            Keyword.PushKeyword("MAX");
            Keyword.PushKeyword("ST");
            Keyword.PushKeyword("END");
            
            TokenSequence<ParseResult> tree = new TokenSequence<ParseResult>(
                new Literal("MAX"),
                new PotentialSpace(),
                new Expression(),
                new PotentialNewLine(),
                new Literal("ST"),
                new PotentialNewLine(),
                new Repeat<TokenSequenceResult<ParseResult>>(new TokenSequence<ParseResult>(new Equation(), new PotentialNewLine()), 1, 8),
                new Literal("END")
            );

            if (args.Length != 1)
            {
                Console.WriteLine("Please put a filename as a argument");
            }

            string content = File.ReadAllText(args[0]);

            CharacterStream stream = new CharacterStream(content);

            TokenSequenceResult<ParseResult> result = tree.Parse(stream);

            if (!stream.IsEof)
            {
                Console.WriteLine("Warning: Not End of line");
            }
            ExprResult objective = (ExprResult)result.parseResult[1];

            RepeatListResult<TokenSequenceResult<ParseResult>> eqs_token = (RepeatListResult<TokenSequenceResult<ParseResult>>)result.parseResult[6];

            List<EqResult> eqs = new List<EqResult>();

            foreach (TokenSequenceResult<ParseResult> res in eqs_token)
            {
                eqs.Add((EqResult)res.parseResult[0]);
            }

            Console.Write("Objective: ");
            foreach (Term term in objective.terms)
            {
                string sign = term.coefficient >= 0 ? "+" : "";
                Console.Write($"{sign}{term.coefficient}{term.term_name} ");
            }
            Console.Write("Constraint: ");
            foreach (EqResult eq in eqs)
            {
                foreach (Term term in eq.exprs.terms)
                {
                    string sign = term.coefficient >= 0 ? "+" : "";
                    Console.Write($"{sign}{term.coefficient}{term.term_name} ");
                }
                Console.Write($"{eq.comparsionAtom.literal} 0");
            }
        }
    }
}