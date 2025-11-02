# MathParser

A parser to parse specific language, particularly math expression related, and extracted specifically the sorted useful data with Pratt parsing

## Example:
The parsing tree:
```cs
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
```
Custom language:
```
MAX 1 + 2x + 2y + 5 + 2 * -4x + 2 * -(5 + 3x)
ST 
    5 + x <= 12
    22 + y <= 164
    254 - 72y +16x <= 5x + 74
END
```
=> Output:
```
...(Not the entire output shown, only the important bit)
Objective: -4-12x+2y
Constraint: -7 +1x <= 0
-142 +1y <= 0
+180 -72y +11x <= 0
```

## Scope
It need to handle expression where there is a combination of coefficient multiply by only the first order algebra (such as 2x), x*y, x*x, x**x is out of scope of this library

## How to use this
Go to GitHub Release and download a binary for windows x64, and you copy `test.model` in the repo, or other custom model that sastified this
Do `./test.exe test.model` where `test.exe` is the binary downloaded and `test.model` is the model (you can freely change the filename for either of this)
Make sure the executable and `math_parser.dll` is in the same directory and not renamed however.