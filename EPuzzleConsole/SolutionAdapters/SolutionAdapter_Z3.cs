using EPuzzleConsole.Adapters;
using Microsoft.Z3;

namespace EPuzzleConsole.SolutionAdapters
{
    internal class SolutionAdapter_Z3
    {
        internal static SolverSolution ExtractSolution(Solver s)
        {
            SolverSolution solution = new(solverLabel: "Z3");
            IEnumerable<KeyValuePair<FuncDecl, Expr>> cs = s.Model.Consts;
            foreach (var c in cs)
            {
                string solutionVar = c.Key.Name.ToString();
                int solutionVal = ((IntNum)c.Value).Int;
                solution.AddEntry(solutionVar, solutionVal);
            }
            return solution;
        }

    }
}
