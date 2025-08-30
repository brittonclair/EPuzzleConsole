# :cup_with_straw::zebra:

Using constraint solvers to solve the Zebra Puzzle (sometimes called Einstein's Puzzle) as described at "Zebra Puzzle" on Wikipedia at https://en.wikipedia.org/wiki/Zebra_Puzzle.

## Solvers used:
* [Google OR-Tools](https://developers.google.com/optimization): CP-SAT for .Net
* [Decider](https://github.com/lifebeyondfife/Decider): open source .Net Constraint Programming Solver
* [Microsoft Z3](https://github.com/z3prover/z3?tab=readme-ov-file#z3-bindings): satisfiability modulo theories solver from Microsoft Research

## Requirements:
* .Net 9
* For the EPuzzleConsole project:
  * Google.OrTools .Net wrapper nuget package
  * Decider nuget package
  * Microsoft.Z3 for .Net nuget package
* For the EPuzzleConsoleTests project (to run tests):
  * NUnit nuget package

 
