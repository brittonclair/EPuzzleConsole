using Decider.Csp.Integer;
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
        public ZebraSolution? SolveUsingCpSolver()
        {
            ZebraSolution? zebraSolution = null;

            (CpModel model, ImmutableArray<Google.OrTools.Sat.IntVar> variablesOfInterest) = ZebraModelBuilder_CpModel.BuildModel();
            
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
                zebraSolution = zebraSolutionMapper.ToSolutionFromCpSolver(variablesOfInterest, solver);
                zebraSolution.Print("Solution from CpSolver");

                // Extract answers for the two specific questions posed by the puzzle
                House? drinks_water = zebraSolution.GetHouseByAttribute("drinks","water");
                House? owns_zebra = zebraSolution.GetHouseByAttribute("pet","zebra");
                Console.WriteLine($"Who drinks water? House {drinks_water?.Id} - the {drinks_water?.Nationality}");
                Console.WriteLine($"Who owns the zebra? House {owns_zebra?.Id} - the {owns_zebra?.Nationality}");
            }
            else
            {
                Console.WriteLine("No solution found.");
            }
            return zebraSolution;
        }

        public ZebraSolution SolveUsingDecider()
        {
            ZebraSolution? zebraSolution = null;

            StateInteger state = ZebraModelBuilder_Decider.BuildModel();
            var searchResult = state.SearchAllSolutions();

            Console.WriteLine();
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"Decider Solution Count: {state.Solutions.Count}");
            foreach (var stateSolution in state.Solutions)
            {
                zebraSolution = zebraSolutionMapper.ToSolutionFromDecider(stateSolution);
                zebraSolution.Print("Solution from Decider");

                // Extract answers for the two specific questions posed by the puzzle
                House? drinks_water = zebraSolution.GetHouseByAttribute("drinks", "water");
                House? owns_zebra = zebraSolution.GetHouseByAttribute("pet", "zebra");
                Console.WriteLine($"Who drinks water? House {drinks_water?.Id} - the {drinks_water?.Nationality}");
                Console.WriteLine($"Who owns the zebra? House {owns_zebra?.Id} - the {owns_zebra?.Nationality}");

            }
            return zebraSolution;
        }

        public ZebraSolution? SolveUsingZ3()
        {
            ZebraSolution? zebraSolution = null;

            Microsoft.Z3.Solver s = ZebraModelBuilder_Z3.BuildModel();
            var z3Result = s.Check();
            Console.WriteLine();
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"Z3 Check: {z3Result}");
            if(z3Result == Status.SATISFIABLE)
            {
                zebraSolution = zebraSolutionMapper.ToSolutionFromZ3(s);
                zebraSolution.Print("Solution from Z3");

                // Extract answers for the two specific questions posed by the puzzle
                House? drinks_water = zebraSolution.GetHouseByAttribute("drinks", "water");
                House? owns_zebra = zebraSolution.GetHouseByAttribute("pet", "zebra");
                Console.WriteLine($"Who drinks water? House {drinks_water?.Id} - the {drinks_water?.Nationality}");
                Console.WriteLine($"Who owns the zebra? House {owns_zebra?.Id} - the {owns_zebra?.Nationality}");
            }
            else
            {
                Console.WriteLine("No solution found.");
            }
            return zebraSolution;
        }
    }
}