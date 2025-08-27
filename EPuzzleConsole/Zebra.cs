using Google.OrTools.Sat;
using System.Collections.Immutable;

namespace EPuzzleConsole
{
    internal class Zebra
    {
        public void Solve()
        {
            int numberOfHouses = 5;

            /* We will have the solver assign an integer as
             * the 'house number' (1 to 5) to the attributes
             * (the 5 colors, 5 pets, etc.).
             * For modeling spatial constraints, the  
             * leftmost house will be considered number 1.
             */

            // One constraint refers to the 'middle house' so
            // establish that house number here.
            int middleHouse = 3;

            // Step 1: CREATE MODEL
            CpModel model = new();

            // Step 2: ADD MODEL VARIABLES
            int lowerBound = 1;
            int upperBound = numberOfHouses;

            // Color variables
            IntVar blue   = model.NewIntVar(lowerBound, upperBound, "blue");
            IntVar green  = model.NewIntVar(lowerBound, upperBound, "green");
            IntVar ivory  = model.NewIntVar(lowerBound, upperBound, "ivory");
            IntVar red    = model.NewIntVar(lowerBound, upperBound, "red");
            IntVar yellow = model.NewIntVar(lowerBound, upperBound, "yellow");

            // Pet variables
            IntVar dog    = model.NewIntVar(lowerBound, upperBound, "dog");
            IntVar fox    = model.NewIntVar(lowerBound, upperBound, "fox");
            IntVar horse  = model.NewIntVar(lowerBound, upperBound, "horse");
            IntVar snails = model.NewIntVar(lowerBound, upperBound, "snails");
            IntVar zebra  = model.NewIntVar(lowerBound, upperBound, "zebra");

            // Nationality variables
            IntVar english   = model.NewIntVar(lowerBound, upperBound, "english");
            IntVar japanese  = model.NewIntVar(lowerBound, upperBound, "japanese");
            IntVar norwegian = model.NewIntVar(lowerBound, upperBound, "norwegian");
            IntVar spanish   = model.NewIntVar(lowerBound, upperBound, "spanish");
            IntVar ukrainian = model.NewIntVar(lowerBound, upperBound, "ukrainian");

            // Drink variables
            IntVar coffee       = model.NewIntVar(lowerBound, upperBound, "coffee");
            IntVar milk         = model.NewIntVar(lowerBound, upperBound, "milk");
            IntVar orange_juice = model.NewIntVar(lowerBound, upperBound, "orange_juice");
            IntVar tea          = model.NewIntVar(lowerBound, upperBound, "tea");
            IntVar water        = model.NewIntVar(lowerBound, upperBound, "water");

            // Smokes variables
            IntVar chesterfields = model.NewIntVar(lowerBound, upperBound, "chesterfields");
            IntVar kools         = model.NewIntVar(lowerBound, upperBound, "kools");
            IntVar luckystrikes  = model.NewIntVar(lowerBound, upperBound, "luckystrikes");
            IntVar oldgolds      = model.NewIntVar(lowerBound, upperBound, "oldgolds");
            IntVar parliaments   = model.NewIntVar(lowerBound, upperBound, "parliaments");

            // Make lists of the variables by category, to make some operations a bit easier later
            ImmutableArray<IntVar> colors        = [blue, green, ivory, red, yellow];
            ImmutableArray<IntVar> nationalities = [english, japanese, norwegian, spanish, ukrainian];
            ImmutableArray<IntVar> pets          = [dog, fox, horse, snails, zebra];
            ImmutableArray<IntVar> drinks        = [coffee, milk, orange_juice, tea, water];
            ImmutableArray<IntVar> smokes        = [chesterfields, kools, luckystrikes, oldgolds, parliaments];


            // Step 3: ADD MODEL CONSTRAINTS
             
            /* Add Uniqueness constraints across the variables in each
             * category. The same value (house number) should not be 
             * assigned to more than one color variable, more than one
             * pet variable, etc.
             */
            model.AddAllDifferent(colors);
            model.AddAllDifferent(pets);
            model.AddAllDifferent(nationalities);
            model.AddAllDifferent(drinks);
            model.AddAllDifferent(smokes);

            /* Define a model Constant = 1: 
             * Useful for creating the "next-to" constraints
             * where we want to constrain the absolute-value of the 
             * distance between two houses to exactly 1 (i.e. the 
             * houses must be next to each other, but they can be 
             * in either order). The specific syntax of the 
             * AddAbsEquality() method doesn't accept a literal.
             */
            var const1 = model.NewConstant(1);

            // Logic constraints from the puzzle statement

            // 1. The Englishman lives in the red house
            model.Add(red == english);

            // 2. The Spaniard owns the dog
            model.Add(dog == spanish);

            // 3. Cofffee is drunk in the green house
            model.Add(green == coffee);

            // 4. The Ukrainian drinks tea
            model.Add(tea == ukrainian);

            // 5. The green house is immediately to the right of the ivory house
            model.Add(green == ivory + 1);

            // 5b. ALTERNATE PUZZLE VERSION:
            // The green house is immediately to the LEFT of the ivory house
            // model.Add(ivory == green + 1);

            // 6. The Old Gold smoker owns snails
            model.Add(snails == oldgolds);

            // 7. Kools are smoked in the yellow house
            model.Add(yellow == kools);

            // 8. Milk is drunk in the middle house
            model.Add(milk == middleHouse);

            // 9. The Norwegian lives in the first house
            model.Add(norwegian == 1);

            // 10. The man who smokes Chesterfields lives in the house next to the man with the fox
            model.AddAbsEquality(const1, fox - chesterfields);

            // 11. Kools are smoked in the house next to the house where the horse is kept
            model.AddAbsEquality(const1, horse - kools);

            // 12. The Lucky Strike smoker drinks orange juice
            model.Add(luckystrikes == orange_juice);

            // 13. The Japanese smokes Parliaments
            model.Add(japanese == parliaments);

            // 14. The Norwegian lives next to the blue house
            model.AddAbsEquality(const1, blue - norwegian);
            /* Since in #9 above norwegian == 1, this
             * constraint could be reduced to blue == 2, as 2
             * will be the only feasible value for any attribute
             * next to the Norwegian's house.
             * But to avoid problems if we experiment with 
             * changing other constraints later, make each
             * constraint independent.
             */

            // Step 4: SOLVE MODEL
            CpSolver solver = new();
            var status = solver.Solve(model);
            Console.WriteLine($"Solve Status: {status}");


            // Step 5: PROCESS SOLUTION

            /* CpSolver status can be one of these:
             *   Optimal, Feasible, Infeasible, ModelInvalid or Unknown
             * A status of Optimal or Feasible will have a solution.
             */
            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                Solution solution = SolutionMapper.ToSolution(solver, colors, nationalities, pets, drinks, smokes);
                solution.Print();

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
    }
}