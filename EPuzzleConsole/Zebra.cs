using Decider.Csp.Integer;
using EPuzzleConsole.Adapters;
using Google.OrTools.Sat;
using Microsoft.Z3;
using System.Collections.Immutable;

namespace EPuzzleConsole
{
    public class Zebra
    {
        private readonly ZebraSolutionMapper zebraSolutionMapper;

        public Zebra()
        {
            zebraSolutionMapper = new ZebraSolutionMapper();
        }

        public void Solve()
        {
            ZebraSolution? result;
            result = SolveUsingCpSolver();
            if (result != null) PresentSolution(result);

            result = SolveUsingDecider();
            if(result != null) PresentSolution(result);

            result =SolveUsingZ3();
            if(result != null) PresentSolution(result);
        }
        public ZebraSolution? SolveUsingCpSolver()
        {
            ZebraSolution? zebraSolution = null;

            (CpModel model, ImmutableArray<Google.OrTools.Sat.IntVar> variablesOfInterest) = ZebraModelBuilder_CpModel.BuildModel();
            
            CpSolver solver = new();
            var status = solver.Solve(model);

            // PROCESS SOLUTION

            /* CpSolver status can be one of these:
             *   Optimal, Feasible, Infeasible, ModelInvalid or Unknown
             * A status of Optimal or Feasible will have a solution.
             */
            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                SolverSolution solverSolution = CpSatAdapter.ExtractSolution(variablesOfInterest, solver);
                zebraSolution = zebraSolutionMapper.FromSolverSolution(solverSolution);
            }
            else
            {
                Console.WriteLine("No solution found.");
            }
            return zebraSolution;
        }

        public ZebraSolution? SolveUsingDecider()
        {
            ZebraSolution? zebraSolution = null;

            StateInteger state = ZebraModelBuilder_Decider.BuildModel();
            var searchResult = state.SearchAllSolutions();

            /* Decider returns a list of solutions, which
             * may be empty if no solution found. 
             */
            if(state.Solutions.Count == 0)
            {
                Console.WriteLine("No solution found.");
            }
            else
            {
                // Use the first solution that was found
                SolverSolution solverSolution = DeciderAdapter.ExtractSolution(state.Solutions[0]);
                zebraSolution = zebraSolutionMapper.FromSolverSolution(solverSolution);

            }
            return zebraSolution;
        }

        public ZebraSolution? SolveUsingZ3()
        {
            ZebraSolution? zebraSolution = null;

            Microsoft.Z3.Solver z3Solver = ZebraModelBuilder_Z3.BuildModel();
            var z3Result = z3Solver.Check();
            /* Z3 status can be one of these:
             *   SATISFIABLE, UNSATISFIABLE, UNKNOWN
             * A status of SATISFIABLE will have a solution.
             */
            if (z3Result == Status.SATISFIABLE)
            {
                SolverSolution solverSolution = Z3Adapter.ExtractSolution(z3Solver);
                zebraSolution = zebraSolutionMapper.FromSolverSolution(solverSolution);
            }
            else
            {
                Console.WriteLine("No solution found.");
            }
            return zebraSolution;
        }

        private static void PresentSolution(ZebraSolution zebraSolution)
        {
            zebraSolution.Print($"Solution from {zebraSolution.SolverLabel}");

            // Extract answers for the two specific questions posed by the puzzle
            House? drinks_water = zebraSolution.GetHouseByAttribute("drinks", "water");
            House? owns_zebra = zebraSolution.GetHouseByAttribute("pet", "zebra");
            Console.WriteLine($"Who drinks water? House {drinks_water?.Id} - the {drinks_water?.Nationality}");
            Console.WriteLine($"Who owns the zebra? House {owns_zebra?.Id} - the {owns_zebra?.Nationality}");

        }

    }
}