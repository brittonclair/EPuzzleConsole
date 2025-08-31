using Decider.Csp.BaseTypes;
using Google.OrTools.Sat;
using Microsoft.Z3;
using System.Collections.Immutable;

namespace EPuzzleConsole.Adapters
{
    /* Classes that extract the variable/value pairs 
     * from a specific solver's solution into a common  
     * data structure (a SolverSolution instance).
     * Avoids client code having to know the mechanics
     * of pulling variables with their assigned values  
     * out of solutions via each unique solver's API.
     */
    internal class DeciderAdapter
    {
        public static SolverSolution ExtractSolution(IDictionary<string, IVariable<int>> modelVariables)
        {
            SolverSolution solution = new("Decider");
            foreach (var modelVariable in modelVariables)
            {
                IVariable<int> solutionVariable = modelVariable.Value;
                string solutionVar = solutionVariable.Name;
                int solutionVal = solutionVariable.InstantiatedValue;
                solution.AddEntry(solutionVar, solutionVal);
            }
            return solution;
        }
    }

    internal class Z3Adapter()
    {
        public static SolverSolution ExtractSolution(Solver s)
        {
            SolverSolution solution = new("Z3");
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

    internal class CpSatAdapter()
    {
        public static SolverSolution ExtractSolution(ImmutableArray<IntVar> variablesOfInterest, CpSolver solver)
        {
            SolverSolution solution = new("CpSat");
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
