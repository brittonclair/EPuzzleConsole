using EPuzzleConsole.Adapters;
using Google.OrTools.Sat;
using System.Collections.Immutable;

namespace EPuzzleConsole.SolutionAdapters
{
    internal class SolutionAdapter_CpSat
    {
        public static SolverSolution ExtractSolution(ImmutableArray<IntVar> variablesOfInterest, CpSolver solver)
        {
            SolverSolution solution = new(solverLabel: "CpSat");
            foreach (IntVar modelIntVar in variablesOfInterest)
            {
                string solutionVar = modelIntVar.Name();
                int solutionVal = (int)solver.Value(modelIntVar);
                solution.AddEntry(solutionVar, solutionVal);
            }
            return solution;
        }

    }
}
