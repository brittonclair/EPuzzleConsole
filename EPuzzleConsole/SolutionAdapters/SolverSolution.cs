namespace EPuzzleConsole.Adapters
{
    internal class SolverSolution
    {
        /* Generic solution model to hold variable/value pairs
         * extracted from a solver-specific solution.
         * 
         * The SolverLabel property identifies the solver
         * that produced the solution.
         */
        internal readonly Dictionary<string, int> SolutionValues = [];
        internal string SolverLabel { get; init; }

        internal SolverSolution(string solverLabel)
        {
            SolverLabel = solverLabel;
        }
        internal void AddEntry(string variable, int value)
        {
            if (SolutionValues.TryGetValue(variable, out int currentVal))
            {
                if (currentVal != value)
                {
                    throw new Exception($"Variable {variable} already has a value assigned.");
                }
                return;
            }
            SolutionValues[variable] = value;
        }
    }
}
