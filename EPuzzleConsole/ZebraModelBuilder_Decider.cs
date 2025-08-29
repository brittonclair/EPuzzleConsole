using Decider.Csp.BaseTypes;
using Decider.Csp.Global;
using Decider.Csp.Integer;
using System.Collections.Immutable;

namespace EPuzzleConsole
{
    internal class ZebraModelBuilder_Decider
    {
        /* Decider solver doesn't really have a 'Model' class, 
         * so just return a list of the variables of interest
         * and the constraints.
         */
        public static StateInteger BuildModel()
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

            var c3 = new VariableInteger("c3", 0, 1);

            // Step 1: CREATE MODEL VARIABLES
            int lowerBound = 1;
            int upperBound = numberOfHouses;

            // Color variables
            var blue = new VariableInteger("blue", lowerBound, upperBound);
            var green = new VariableInteger("green", lowerBound, upperBound);
            var ivory = new VariableInteger("ivory", lowerBound, upperBound);
            var red = new VariableInteger("red", lowerBound, upperBound);
            var yellow = new VariableInteger("yellow", lowerBound, upperBound);

            // Pet variables
            var dog = new VariableInteger("dog", lowerBound, upperBound);
            var fox = new VariableInteger("fox", lowerBound, upperBound);
            var horse = new VariableInteger("horse", lowerBound, upperBound);
            var snails = new VariableInteger("snails", lowerBound, upperBound);
            var zebra = new VariableInteger("zebra", lowerBound, upperBound);

            // Nationality variables
            var english = new VariableInteger("english", lowerBound, upperBound);
            var japanese = new VariableInteger("japanese", lowerBound, upperBound);
            var norwegian = new VariableInteger("norwegian", lowerBound, upperBound);
            var spanish = new VariableInteger("spanish", lowerBound, upperBound);
            var ukrainian = new VariableInteger("ukrainian", lowerBound, upperBound);

            // Drink variables
            var coffee = new VariableInteger("coffee", lowerBound, upperBound);
            var milk = new VariableInteger("milk", lowerBound, upperBound);
            var orange_juice = new VariableInteger("orange_juice", lowerBound, upperBound);
            var tea = new VariableInteger("tea", lowerBound, upperBound);
            var water = new VariableInteger("water", lowerBound, upperBound);

            // Smokes variables
            var chesterfields = new VariableInteger("chesterfields", lowerBound, upperBound);
            var kools = new VariableInteger("kools", lowerBound, upperBound);
            var luckystrikes = new VariableInteger("luckystrikes", lowerBound, upperBound);
            var oldgolds = new VariableInteger("oldgolds", lowerBound, upperBound);
            var parliaments = new VariableInteger("parliaments", lowerBound, upperBound);

            // Make lists of the variables by category, to make some operations a bit easier later
            ImmutableArray<VariableInteger> colors = [blue, green, ivory, red, yellow];
            ImmutableArray<VariableInteger> nationalities = [english, japanese, norwegian, spanish, ukrainian];
            ImmutableArray<VariableInteger> pets = [dog, fox, horse, snails, zebra];
            ImmutableArray<VariableInteger> drinks = [coffee, milk, orange_juice, tea, water];
            ImmutableArray<VariableInteger> smokes = [chesterfields, kools, luckystrikes, oldgolds, parliaments];

            // Step 3: ADD MODEL CONSTRAINTS
            List<IConstraint> constraints = [];

            /* Add Uniqueness constraints across the variables in each
             * category. The same value (house number) should not be 
             * assigned to more than one color variable, more than one
             * pet variable, etc.
             */
            constraints.Add(new AllDifferentInteger(colors));
            constraints.Add(new AllDifferentInteger(pets));
            constraints.Add(new AllDifferentInteger(nationalities));
            constraints.Add(new AllDifferentInteger(drinks));
            constraints.Add(new AllDifferentInteger(smokes));

            // new ConstraintInteger(n + r + c0 == (10 * c1) + e),
            // Logic constraints from the puzzle statement

            // 1. The Englishman lives in the red house
            constraints.Add(new ConstraintInteger(red == english));

            // 2. The Spaniard owns the dog
            constraints.Add(new ConstraintInteger(dog == spanish));

            // 3. Cofffee is drunk in the green house
            constraints.Add(new ConstraintInteger(green == coffee));

            // 4. The Ukrainian drinks tea
            constraints.Add(new ConstraintInteger(tea == ukrainian));

            // 5. The green house is immediately to the right of the ivory house
            constraints.Add(new ConstraintInteger(green == ivory + 1));

            // 5b. ALTERNATE PUZZLE VERSION:
            // The green house is immediately to the LEFT of the ivory house
            // constraints.Add(new ConstraintInteger(ivory == green + 1));

            // 6. The Old Gold smoker owns snails
            constraints.Add(new ConstraintInteger(snails == oldgolds));

            // 7. Kools are smoked in the yellow house
            constraints.Add(new ConstraintInteger(yellow == kools));

            // 8. Milk is drunk in the middle house
            constraints.Add(new ConstraintInteger(milk == middleHouse));

            // 9. The Norwegian lives in the first house
            constraints.Add(new ConstraintInteger(norwegian == 1));

            // 10. The man who smokes Chesterfields lives in the house next to the man with the fox
            constraints.Add(new ConstraintInteger(fox - chesterfields == 1 | fox - chesterfields == -1));

            // 11. Kools are smoked in the house next to the house where the horse is kept
            constraints.Add(new ConstraintInteger(horse - kools == 1 | horse - kools == -1));

            // 12. The Lucky Strike smoker drinks orange juice
            constraints.Add(new ConstraintInteger(luckystrikes == orange_juice));

            // 13. The Japanese smokes Parliaments
            constraints.Add(new ConstraintInteger(japanese == parliaments));

            // 14. The Norwegian lives next to the blue house
            constraints.Add(new ConstraintInteger(blue - norwegian == 1 | norwegian - blue == -1));
            /* Since in #9 above norwegian == 1, this
             * constraint could be reduced to blue == 2, as 2
             * will be the only feasible value for any attribute
             * next to the Norwegian's house.
             * But to avoid problems if we experiment with 
             * changing other constraints later, make each
             * constraint independent.
             */
            var allVars = colors
                .AddRange(nationalities)
                .AddRange(pets)
                .AddRange(drinks)
                .AddRange(smokes);

            var state = new StateInteger(allVars, constraints);

            return state;
        }
    }
}
