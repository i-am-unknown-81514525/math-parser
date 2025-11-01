using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using math_parser.ast;
using math_parser.math;
using math_parser.atom;

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

        public static implicit operator Term(Fraction v) => new Term(v, "");
        public static implicit operator Term((Fraction frac, string term) v) => new Term(v.frac, v.term);
    }

    public class ExprResult : MathAtomResult, IAdd<ExprResult, ExprResult>, ISub<ExprResult, ExprResult>, ISelfAdd<ExprResult>, ISelfSub<ExprResult>, IMul<ExprResult, ExprResult>, IDiv<ExprResult, ExprResult>
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

        public static ExprResult operator *(Fraction scalar, ExprResult right) => new ExprResult(right.terms.Select(x => new Term(scalar * x.coefficient, x.term_name)).ToArray());

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

        public ExprResult Add(ExprResult right)
        {
            return this + right;
        }

        public ExprResult Sub(ExprResult right)
        {
            return this - right;
        }

        public ExprResult Add()
        {
            return this;
        }

        public ExprResult Sub()
        {
            return -this;
        }

        public ExprResult Mul(ExprResult right)
        {
            return this * right;
        }

        public ExprResult Div(ExprResult right)
        {
            return this / right;
        }

        public static implicit operator ExprResult(Term v) => new ExprResult(new[] { v });
    }

    public class ASTExprResult : ASTValue1<ExprResult>
    {
        public ASTExprResult(ExprResult result) : base(result) { }

    }
    
    public class PrattParseError : Exception
    {
        public PrattParseError(string info) : base(info) {}
    }

    public class Expression : Group<ParseResult, ExprResult>
    {
        public static readonly Dictionary<ArithematicSymbolAtom, (int, int)> binding_power = new Dictionary<ArithematicSymbolAtom, (int, int)>()
        {
            {ArithematicSymbolAtom.Add, (1, 2) },
            {ArithematicSymbolAtom.Sub, (1, 2) },
            {ArithematicSymbolAtom.Mul, (3, 4) },
            {ArithematicSymbolAtom.Div, (3, 4) }
        };

        public Expression() : base( // 1 + (2x * 3) / 2 => [(5/2, ""), (1, "x")]
            new TokenSequence<ParseResult>(
                new PotentialSpace(),
                new OrNoBacktrack<ParseResult>(
                    new Bracketed<ExprResult>(new LazyExpression()),
                    new TokenSequence<ParseResult>(
                        new Number(),
                        new Maybe<ParseResult>(
                           new TokenSequence<ParseResult>(
                                new VariableAtom(),
                                new Maybe<MathAtomResult>(
                                    new Bracketed<ExprResult>(new LazyExpression())
                                )
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
                        new PotentialSpace(),
                        new ArithmeticSymbolAtom(),
                        new PotentialSpace(),
                        new OrNoBacktrack<ParseResult>(
                            new Bracketed<ExprResult>(new LazyExpression()),
                            new TokenSequence<ParseResult>(
                                new Number(),
                                new Maybe<ParseResult>(
                                new TokenSequence<ParseResult>(
                                        new VariableAtom(),
                                        new Maybe<MathAtomResult>(
                                            new Bracketed<ExprResult>(new LazyExpression())
                                        )
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
                new PotentialSpace()
            )
        )
        { }

        public override ExprResult Parse(CharacterStream stream)
        {
            // List<Atom> TakeValues()
            // {
            //     List<Atom> inner = new List<Atom>();
            //     int count = 0;
            //     if ((new Number()).CanParse(stream))
            //     {
            //         inner.Add(
            //             new Value(
            //                 (ExprResult)(Term)(new Number()).Parse(stream).AsFraction()
            //             )
            //         );
            //         count++;
            //     }
            //     if ((new VariableAtom().CanParse(stream)))
            //     {
            //         if (count > 0)
            //         {
            //             inner.Add(ArithematicSymbolAtom.Mul);
            //         }

            //         inner.Add(
            //             new Value(
            //                 (ExprResult)(Term)(1, (new VariableAtom()).Parse(stream).literal)
            //             )
            //         );
            //         count++;
            //     }
            //     if (new Bracketed<ExprResult>(new Expression()).CanParse(stream))
            //     {
            //         if (count > 0)
            //         {
            //             inner.Add(ArithematicSymbolAtom.Mul);
            //         }

            //         inner.Add(new Value(new Bracketed<ExprResult>(new Expression()).Parse(stream)));
            //         count++;
            //     }
            //     if (count == 0)
            //     {
            //         throw new TokenParseBacktrackException("no valid path");
            //     }
            //     return inner;
            // }

            List<MathAtomResult> Recur(ParseResult result)
            {
                List<MathAtomResult> curr = new List<MathAtomResult>();
                if (result is TokenSequenceResult<ParseResult> seqResult)
                {
                    foreach (ParseResult r in seqResult.parseResult)
                    {
                        curr.AddRange(Recur(r));
                    }
                }
                else if (result is RepeatListResult<ParseResult> repeatResult)
                {
                    foreach (ParseResult r in repeatResult)
                    {
                        curr.AddRange(Recur(r));
                    }
                }
                else if (result is MathAtomResult atom)
                {
                    curr.Add((MathAtomResult)atom);
                }
                return curr;
            }

            Dictionary<string, ArithematicSymbolAtom> matcher = new Dictionary<string, ArithematicSymbolAtom>()
            {
                {"+", ArithematicSymbolAtom.Add},
                {"-", ArithematicSymbolAtom.Sub},
                {"*", ArithematicSymbolAtom.Mul},
                {"/", ArithematicSymbolAtom.Div}
            };

            List<Atom> atoms = new List<Atom>();

            List<MathAtomResult> linear_parse = Recur(inner_token.Parse(stream));

            bool last_is_value = false;
            foreach (MathAtomResult result in linear_parse)
            {
                if (result is NumberResult num)
                {
                    if (last_is_value)
                    {
                        atoms.Add(ArithematicSymbolAtom.Mul);
                    }
                    last_is_value = true;
                    atoms.Add(new Value((Term)num.AsFraction()));
                }
                else if (result is MathLiteralResult literal)
                {
                    if (matcher.ContainsKey(literal.literal))
                    {
                        last_is_value = false;
                        atoms.Add(matcher[literal.literal]);
                    }
                    else
                    {
                        if (last_is_value)
                        {
                            atoms.Add(ArithematicSymbolAtom.Mul);
                        }
                        last_is_value = true;
                        atoms.Add(new Value((Term)(1, literal.literal)));
                    }
                }
                else if (result is ExprResult expr)
                {
                    if (last_is_value)
                    {
                        atoms.Add(ArithematicSymbolAtom.Mul);
                    }
                    last_is_value = true;
                    atoms.Add(new Value(expr));
                }
            }
            return ParseExpr(new Queue<Atom>(atoms), 0).Calc();
        }
        // Core Dumped, ‘This Simple Algorithm Powers Real Interpreters: Pratt Parsing’, YouTube. Accessed: May 23, 2025. [Online]. Available: https://youtu.be/0c8b7YfsBKJs
        // 12:45
        public static IASTNode<ExprResult> ParseExpr(Queue<Atom> tokens, int minBindingPower) // 
        {
            Atom next_lhs = tokens.Dequeue();
            if (!(next_lhs is Value))
            {
                throw new PrattParseError("Failed lhs parse lhs");
            }
            IASTNode<ExprResult> lhs = new ASTValue1<ExprResult>(((Value)next_lhs).inner);
            do
            {
                if (tokens.Count == 0)
                {
                    break;
                }
                Atom next_op = tokens.Dequeue();
                if (!(next_op is ArithematicSymbolAtom))
                {
                    throw new PrattParseError("Failed lhs parse op");
                }
                ArithematicSymbolAtom op_raw = (ArithematicSymbolAtom)next_op;
                (int leftBindingPower, int rightBindingPower) = binding_power[(ArithematicSymbolAtom)next_op];
                if (leftBindingPower < minBindingPower)
                {
                    break;
                }
                IASTNode<ExprResult> rhs = ParseExpr(tokens, rightBindingPower);
                lhs = new Dictionary<ArithematicSymbolAtom, IASTNode<ExprResult>>
                {
                    { ArithematicSymbolAtom.Add, new ASTAdd<ExprResult>(lhs, rhs)},
                    { ArithematicSymbolAtom.Sub, new ASTSub<ExprResult>(lhs, rhs)},
                    { ArithematicSymbolAtom.Mul, new ASTMul<ExprResult>(lhs, rhs)},
                    { ArithematicSymbolAtom.Div, new ASTDiv<ExprResult>(lhs, rhs)}
                }[op_raw];
            } while (true);
            return lhs;
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