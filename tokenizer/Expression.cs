using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
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

        public ExprResult(Term[] terms)
        {
            this.terms = terms.ToArray();
        }

        public bool isIntegerOnly() => terms.Where(x => x.term_name != "").Count() == 0;

        public static ExprResult operator +(ExprResult left, ExprResult right)
        {
            List<Term> ret = left.terms.ToList();
            foreach (Term term in right.terms)
            {
                int idx = ret.Select(x => x.term_name).ToList().IndexOf(term.term_name);
                if (idx == -1)
                {
                    ret.Add(term);
                }
                ret[idx] = new Term(ret[idx].coefficient + term.coefficient, term.term_name);
            }
            return new ExprResult(ret.ToArray());
        }

        public static ExprResult operator +(ExprResult curr) => curr.Clone();
        public static ExprResult operator -(ExprResult curr) => -1 * curr;
        public static ExprResult operator -(ExprResult left, ExprResult right) => left + (-right);

        public static ExprResult operator *(Fraction scalar, ExprResult right) => new ExprResult(right.terms.Select(x => new Term(scalar*x.coefficient, x.term_name)).ToArray());

        public static ExprResult operator *(ExprResult left, ExprResult right)
        {
            if (!(left.isIntegerOnly() || right.isIntegerOnly()))
            {
                throw new InvalidOperationException("This would cause multiplication between algebric term");
            }
            ExprResult curr = new ExprResult(new Term[] { });
            if (left.isIntegerOnly())
            {
                foreach (Term tLeft in left.terms)
                {
                    curr += tLeft.coefficient * right;
                }
            }
            else if (right.isIntegerOnly())
            {
                foreach (Term tRight in right.terms)
                {
                    curr += tRight.coefficient * left;
                }
            }
            return curr;
        }

        public static ExprResult operator /(ExprResult left, ExprResult right)
        {
            // This would ignore that (100x)/(50x) is possible to reduce complexity as out of scope
            if (!right.isIntegerOnly())
            {
                throw new InvalidOperationException("Only divide by value is allowed");
            }
            Fraction total = right.terms.Select(x => x.coefficient).Sum();

            if (total == 0)
            {
                throw new InvalidOperationException("Expression divided by 0");
            }
            return (1 / total) * left;
        }
        

        public ExprResult Clone() => new ExprResult(terms.ToArray());
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