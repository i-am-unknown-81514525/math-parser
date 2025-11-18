using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using math_parser.ast;
using System.Text;
using math_parser.math;
using math_parser.atom;

namespace math_parser.tokenizer
{
    public struct Term
    {
        public readonly Fraction Coefficient;
        public readonly string TermName; // "" mean just the literal value, no algebra term

        public Term(Fraction coefficient, string termName)
        {
            Coefficient = coefficient;
            TermName = termName;
        }

        public static implicit operator Term(Fraction v) => new Term(v, "");
        public static implicit operator Term((Fraction frac, string term) v) => new Term(v.frac, v.term);
    }

    public class ExprResult : MathAtomResult, IAdd<ExprResult, ExprResult>, ISub<ExprResult, ExprResult>, ISelfAdd<ExprResult>, ISelfSub<ExprResult>, IMul<ExprResult, ExprResult>, IDiv<ExprResult, ExprResult>
    {
        public readonly Term[] Terms;

        public ExprResult(Term[] terms)
        {
            Terms = terms.ToArray();
        }

        public bool isIntegerOnly() => Terms.Where(x => x.TermName != "").Count() == 0;

        public static ExprResult operator +(ExprResult left, ExprResult right)
        {
            List<Term> ret = left.Terms.ToList();
            foreach (Term term in right.Terms)
            {
                int idx = ret.Select(x => x.TermName).ToList().IndexOf(term.TermName);
                if (idx == -1)
                {
                    ret.Add(term);
                }
                else
                {
                    ret[idx] = new Term(ret[idx].Coefficient + term.Coefficient, term.TermName);
                }
            }
            return new ExprResult(ret.ToArray());
        }

        public static ExprResult operator +(ExprResult curr) => curr.Clone();
        public static ExprResult operator -(ExprResult curr) => -1 * curr;
        public static ExprResult operator -(ExprResult left, ExprResult right) => left + (-right);

        public static ExprResult operator *(Fraction scalar, ExprResult right) => new ExprResult(right.Terms.Select(x => new Term(scalar * x.Coefficient, x.TermName)).ToArray());

        public static ExprResult operator *(ExprResult left, ExprResult right)
        {
            if (!(left.isIntegerOnly() || right.isIntegerOnly()))
            {
                throw new InvalidOperationException("This would cause multiplication between algebric term");
            }
            ExprResult curr = new ExprResult(new Term[] { });
            if (left.isIntegerOnly())
            {
                foreach (Term tLeft in left.Terms)
                {
                    curr += tLeft.Coefficient * right;
                }
            }
            else if (right.isIntegerOnly())
            {
                foreach (Term tRight in right.Terms)
                {
                    curr += tRight.Coefficient * left;
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
            Fraction total = right.Terms.Select(x => x.Coefficient).Sum();

            if (total == 0)
            {
                throw new InvalidOperationException("Expression divided by 0");
            }
            return (1 / total) * left;
        }


        public ExprResult Clone() => new ExprResult(Terms.ToArray());

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

        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.Append($"{ParseResultExtensions.Indent(indent)}ExprResult: ");
            for (int i = 0; i < Terms.Length; i++)
            {
                sb.Append($"({Terms[i].Coefficient}, '{Terms[i].TermName}')");
                if (i < Terms.Length - 1) sb.Append(" + ");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }

    public class AstExprResult : AstValue1<ExprResult>
    {
        public AstExprResult(ExprResult result) : base(result) { }

    }

    public class PrattParseError : Exception
    {
        public PrattParseError(string info) : base(info) {}
    }

    public class Expression : Group<ParseResult, ExprResult>
    {
        public static readonly Dictionary<ArithematicSymbolAtom, (int, int)> BindingPower = new Dictionary<ArithematicSymbolAtom, (int, int)>()
        {
            {ArithematicSymbolAtom.Add, (1, 2) },
            {ArithematicSymbolAtom.Sub, (1, 2) },
            {ArithematicSymbolAtom.Mul, (3, 4) },
            {ArithematicSymbolAtom.Div, (3, 4) },
            { ArithematicSymbolAtom.StongMul, (5, 6)}
        };

        public Expression() : base( // 1 + (2x * 3) / 2 => [(5/2, ""), (1, "x")]
            new TokenSequence<ParseResult>(
                new PotentialSpace(),
                new Or<ParseResult>(
                    new TokenSequence<ParseResult>(
                        new Maybe<ParseResult>(new OppoSign()),
                        new Bracketed<ExprResult>(new LazyExpression())
                    ),
                    new TokenSequence<ParseResult>(
                        new Number(),
                        new OrNoBacktrack<ParseResult>(
                            new Bracketed<ExprResult>(new LazyExpression()),
                            new Maybe<ParseResult>(
                                new TokenSequence<ParseResult>(
                                    new VariableAtom(),
                                    new Maybe<ParseResult>(
                                        new Bracketed<ExprResult>(new LazyExpression())
                                    )
                                )
                            )
                        )
                    ),
                    new TokenSequence<ParseResult>(
                        new Maybe<ParseResult>(new OppoSign()),
                        new VariableAtom(),
                        new Maybe<ParseResult>(
                           new Bracketed<ExprResult>(new LazyExpression())
                        )
                    )
                ),
                new Repeat<ParseResult>(
                    new TokenSequence<ParseResult>(
                        new PotentialSpace(),
                        new ArithmeticSymbolAtom(),
                        new PotentialSpace(),
                        new Or<ParseResult>(
                            new TokenSequence<ParseResult>(
                                new Maybe<ParseResult>(new OppoSign()),
                                new Bracketed<ExprResult>(new LazyExpression())
                            ),
                            new TokenSequence<ParseResult>(
                                new Number(),
                                new Maybe<ParseResult>(
                                    new OrNoBacktrack<ParseResult>(
                                        new Bracketed<ExprResult>(new LazyExpression()),
                                        new Maybe<ParseResult>(
                                            new TokenSequence<ParseResult>(
                                                new VariableAtom(),
                                                new Maybe<ParseResult>(
                                                    new Bracketed<ExprResult>(new LazyExpression())
                                                )
                                            )
                                        )
                                    )
                                )
                            ),
                            new TokenSequence<ParseResult>(
                                new Maybe<ParseResult>(new OppoSign()),
                                new VariableAtom(),
                                new Maybe<ParseResult>(
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
                    foreach (ParseResult r in seqResult.ParseResult)
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
                    curr.Add(atom);
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

            var parseTree = InnerToken.Parse(stream);
            //Console.WriteLine("--- Parse Tree ---");
            //Console.WriteLine(parseTree.Print());
            //Console.WriteLine("------------------");
            List<MathAtomResult> linearParse = Recur(parseTree);

            bool lastIsValue = false;
            foreach (MathAtomResult result in linearParse)
            {
                if (result is OppoSignResult)
                {
                    atoms.Add(new Value((Term)(Fraction)(-1)));
                    atoms.Add(ArithematicSymbolAtom.StongMul);
                    lastIsValue = false;
                }
                else if (result is NumberResult num)
                {
                    if (lastIsValue)
                    {
                        atoms.Add(ArithematicSymbolAtom.Mul);
                    }
                    lastIsValue = true;
                    atoms.Add(new Value((Term)num.AsFraction()));
                }
                else if (result is MathLiteralResult literal)
                {
                    if (matcher.ContainsKey(literal.Literal))
                    {
                        lastIsValue = false;
                        atoms.Add(matcher[literal.Literal]);
                    }
                    else
                    {
                        if (lastIsValue)
                        {
                            atoms.Add(ArithematicSymbolAtom.Mul);
                        }
                        lastIsValue = true;
                        atoms.Add(new Value((Term)(1, literal.Literal)));
                    }
                }
                else if (result is ExprResult expr)
                {
                    if (lastIsValue)
                    {
                        atoms.Add(ArithematicSymbolAtom.Mul);
                    }
                    lastIsValue = true;
                    atoms.Add(new Value(expr));
                }
            }
            //foreach (Atom atom in atoms)
            //{
            //    Console.Write(atom.GetType().Name);
            //    if (atom is Value v)
            //    {
            //        Console.Write("(");
            //        for (int i = 0; i < v.inner.terms.Length; i++)
            //        {
            //            var term = v.inner.terms[i];
            //            Console.Write($"({term.coefficient}, '{term.term_name}')");
            //            if (i < v.inner.terms.Length - 1)
            //            {
            //                Console.Write(", ");
            //            }
            //        }
            //        Console.Write(")");
            //    } else if (atom is ArithematicSymbolAtom op)
            //    {
            //        Console.Write($"({op.literal})");
            //    }
            //    Console.Write(" ");
            //}
            //Console.Write("\n");
            return ParseExpr(new Queue<Atom>(atoms), 0).Calc();
        }
        // Core Dumped, ‘This Simple Algorithm Powers Real Interpreters: Pratt Parsing’, YouTube. Accessed: May 23, 2025. [Online]. Available: https://youtu.be/0c8b7YfsBKJs
        // 12:45
        public static IastNode<ExprResult> ParseExpr(Queue<Atom> tokens, int minBindingPower) //
        {
            Atom nextLhs = tokens.Dequeue();
            if (!(nextLhs is Value))
            {
                throw new PrattParseError("Failed lhs parse lhs");
            }
            IastNode<ExprResult> lhs = new AstValue1<ExprResult>(((Value)nextLhs).Inner);
            do
            {
                if (tokens.Count == 0)
                {
                    break;
                }
                Atom nextOp = tokens.Peek();
                if (!(nextOp is ArithematicSymbolAtom))
                {
                    throw new PrattParseError($"Failed lhs parse op, atom: {nextOp}");
                }
                ArithematicSymbolAtom opRaw = (ArithematicSymbolAtom)nextOp;
                (int leftBindingPower, int rightBindingPower) = BindingPower[(ArithematicSymbolAtom)nextOp];
                if (leftBindingPower < minBindingPower)
                {
                    break;
                }
                tokens.Dequeue(); // Consume the operator now.
                IastNode<ExprResult> rhs = ParseExpr(tokens, rightBindingPower);
                lhs = new Dictionary<ArithematicSymbolAtom, IastNode<ExprResult>>
                {
                    { ArithematicSymbolAtom.Add, new AstAdd<ExprResult>(lhs, rhs)},
                    { ArithematicSymbolAtom.Sub, new AstSub<ExprResult>(lhs, rhs)},
                    { ArithematicSymbolAtom.Mul, new AstMul<ExprResult>(lhs, rhs)},
                    { ArithematicSymbolAtom.Div, new AstDiv<ExprResult>(lhs, rhs)},
                    { ArithematicSymbolAtom.StongMul, new AstMul<ExprResult>(lhs, rhs)}
                }[opRaw];
            } while (true);
            return lhs;
        }
    }

    public class LazyExpression : Group<SyntaxDiscardResult, ExprResult>
    {
        public LazyExpression() : base(new Literal("LAZY_EVAL_LazyExpression")) { } // Placeholder, the token is dynamically provided to restrict recurrsion

        public override ExprResult Parse(CharacterStream stream)
        {
            Expression innerToken = new Expression();
            return innerToken.Parse(stream);
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            Expression innerToken = new Expression();
            return innerToken.PartialParse(stream);
        }

        public override bool CanParse(CharacterStream stream)
        {
            Expression innerToken = new Expression();
            return innerToken.CanParse(stream);
        }

        public override bool CanPartialParse(CharacterStream stream)
        {
            Expression innerToken = new Expression();
            return innerToken.CanPartialParse(stream);
        }
    }
}