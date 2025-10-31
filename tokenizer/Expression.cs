using System;
using System.Linq.Expressions;
using math_parser.math;

namespace math_parser.tokenizer
{
    public struct Term
    {
        public readonly Fraction coefficient;
        public readonly string term_name; // "" mean just the literal value, no algebra term

        public Term(Fraction coefficient, string term_name)
        {
            this.coefficient = coefficient;
            this.term_name = term_name;
        }

    }

    public class ExprResult : MathAtomResult
    {
        public readonly Term[] terms;
    }

    public class Expression : Group<ParseResult, ExprResult>
    {
        public Expression() : base(
            new TokenSequence<ParseResult>(
                new MathPotentialSpace(),
                new OrNoBacktrack<ParseResult>(
                    new Bracketed<ExprResult>(new LazyExpression()),
                    new TokenSequence<ParseResult>(
                        new Number(),
                        new Maybe<MathAtomResult>(
                           new OrNoBacktrack<MathAtomResult>(
                                new VariableAtom(),
                                new Bracketed<ExprResult>(new LazyExpression())
                            )
                        )
                    ),
                    new TokenSequence<ParseResult>(
                        new VariableAtom(),
                        new Maybe<MathAtomResult>(
                           new Bracketed<ExprResult>(new LazyExpression())
                        )
                    )
                ),
                new Repeat<ParseResult>(
                    new TokenSequence<ParseResult>(
                        new MathPotentialSpace(),
                        new ArithmeticSymbolAtom(),
                        new MathPotentialSpace(),
                        new OrNoBacktrack<ParseResult>(
                            new Bracketed<ExprResult>(new LazyExpression()),
                            new TokenSequence<ParseResult>(
                                new Number(),
                                new Maybe<MathAtomResult>(
                                    new OrNoBacktrack<MathAtomResult>(
                                        new VariableAtom(),
                                        new Bracketed<ExprResult>(new LazyExpression())
                                    )
                                )
                            ),
                            new TokenSequence<ParseResult>(
                                new VariableAtom(),
                                new Maybe<MathAtomResult>(
                                    new Bracketed<ExprResult>(new LazyExpression())
                                )
                            )
                        )
                    ),
                    0,
                    Amount.Unbound
                ),
                new MathPotentialSpace()
            )
        )
        { }

        public override ExprResult Parse(CharacterStream stream)
        {
            throw new NotImplementedException();
        }
    }

    public class LazyExpression : Group<SyntaxDiscardResult, ExprResult>
    {
        public LazyExpression() : base(new Literal("LAZY_EVAL_LazyExpression")) { } // Placeholder, the token is dynamically provided to restrict recurrsion

        public override ExprResult Parse(CharacterStream stream)
        {
            Expression inner_token = new Expression();
            return inner_token.Parse(stream);
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            Expression inner_token = new Expression();
            return inner_token.PartialParse(stream);
        }

        public override bool CanParse(CharacterStream stream)
        {
            Expression inner_token = new Expression();
            return inner_token.CanParse(stream);
        }

        public override bool CanPartialParse(CharacterStream stream)
        {
            Expression inner_token = new Expression();
            return inner_token.CanPartialParse(stream);
        }
    }
}