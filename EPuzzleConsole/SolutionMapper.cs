using Google.OrTools.Sat;
using System.Collections.Immutable;

namespace EPuzzleConsole
{
    internal class SolutionMapper
    {
        public static Solution ToSolution(
            CpSolver solver, 
            ImmutableArray<IntVar> colors, 
            ImmutableArray<IntVar> nationalities,
            ImmutableArray<IntVar> pets,
            ImmutableArray<IntVar> drinks,
            ImmutableArray<IntVar> smokes)
        {
            Solution solution = new Solution();
            ImmutableArray<IntVar> allVars = colors.AddRange(nationalities).AddRange(pets).AddRange(drinks).AddRange(smokes);

            foreach (IntVar modelIntVar in allVars)
            {
                int value = (int)solver.Value(modelIntVar);
                if(colors.Contains(modelIntVar))
                    solution.Houses[value].Color = modelIntVar.Name();
                else if(nationalities.Contains(modelIntVar))
                    solution.Houses[value].Nationality = modelIntVar.Name();
                else if(pets.Contains(modelIntVar))
                    solution.Houses[value].Pet = modelIntVar.Name();
                else if(drinks.Contains(modelIntVar))
                    solution.Houses[value].Drinks = modelIntVar.Name();
                else if(smokes.Contains(modelIntVar))
                    solution.Houses[value].Smokes = modelIntVar.Name();
            }
            return solution;
        }
    }
}
