using Decider.Csp.Integer;
using Google.OrTools.Sat;
using System.Collections.Immutable;

namespace EPuzzleConsole
{
    internal class Zebra
    {
        public static void SolveUsingCpSolver()
        {
            (CpModel model, ImmutableArray<IntVar> variablesOfInterest) = ZebraModelBuilder_CpModel.BuildModel();
            
            CpSolver solver = new();
            var status = solver.Solve(model);
            Console.WriteLine();
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"CpSolver Status: {status}");


            // PROCESS SOLUTION

            /* CpSolver status can be one of these:
             *   Optimal, Feasible, Infeasible, ModelInvalid or Unknown
             * A status of Optimal or Feasible will have a solution.
             */
            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                ZebraSolution solution = ZebraSolutionMapper.ToSolutionFromCpSolver(variablesOfInterest, solver);
                solution.Print("Solution from CpSolver");

                // Extract answers for the two specific questions posed by the puzzle
                House? drinks_water = solution.GetHouseByAttribute("drinks","water");
                House? owns_zebra = solution.GetHouseByAttribute("pet","zebra");
                Console.WriteLine($"Who drinks water? House {drinks_water?.Id} - the {drinks_water?.Nationality}");
                Console.WriteLine($"Who owns the zebra? House {owns_zebra?.Id} - the {owns_zebra?.Nationality}");
            }
            else
            {
                Console.WriteLine("No solution found.");
            }
        }

        public static void SolveUsingDecider()
        {
            StateInteger state = ZebraModelBuilder_Decider.BuildModel();
            var searchResult = state.SearchAllSolutions();

            Console.WriteLine();
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"Decider Solution Count: {state.Solutions.Count}");
            foreach (var stateSolution in state.Solutions)
            {
                ZebraSolution solution = ZebraSolutionMapper.ToSolutionFromDecider(stateSolution);
                solution.Print("Solution from Decider");

                // Extract answers for the two specific questions posed by the puzzle
                House? drinks_water = solution.GetHouseByAttribute("drinks", "water");
                House? owns_zebra = solution.GetHouseByAttribute("pet", "zebra");
                Console.WriteLine($"Who drinks water? House {drinks_water?.Id} - the {drinks_water?.Nationality}");
                Console.WriteLine($"Who owns the zebra? House {owns_zebra?.Id} - the {owns_zebra?.Nationality}");

            }
        }
    }
}