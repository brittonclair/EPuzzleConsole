using Microsoft.Z3;
using System.Collections.Immutable;

namespace EPuzzleConsole.ModelBuilders.Zebra
{
    internal class Z3ModelBuilder
    {
        public static Solver BuildModel()
        {
            /* We will have the solver assign an integer as
             * the 'house number' (1 to 5) to the attributes
             * (the 5 colors, 5 pets, etc.).
             * For modeling spatial constraints, the  
             * leftmost house will be considered number 1.
             */
            int numberOfHouses = 5;

            // One constraint refers to the 'middle house' so
            // establish that house number here.
            int middleHouse = 3;

            var ctx = new Context();
            Solver s = ctx.MkSolver();

            // Color variables
            IntExpr blue = ctx.MkIntConst("blue");  
            IntExpr green = ctx.MkIntConst("green");
            IntExpr ivory = ctx.MkIntConst("ivory");
            IntExpr red = ctx.MkIntConst("red");
            IntExpr yellow = ctx.MkIntConst("yellow");

            // Nationality variables
            IntExpr english = ctx.MkIntConst("english");
            IntExpr japanese = ctx.MkIntConst("japanese");
            IntExpr norwegian = ctx.MkIntConst("norwegian");
            IntExpr spanish = ctx.MkIntConst("spanish");
            IntExpr ukrainian = ctx.MkIntConst("ukrainian");

            // Pet variables
            IntExpr dog = ctx.MkIntConst("dog");
            IntExpr fox = ctx.MkIntConst("fox");
            IntExpr horse = ctx.MkIntConst("horse");
            IntExpr snails = ctx.MkIntConst("snails");
            IntExpr zebra = ctx.MkIntConst("zebra");

            // Drinks variables
            IntExpr milk = ctx.MkIntConst("milk");
            IntExpr orange_juice = ctx.MkIntConst("orange_juice");
            IntExpr tea = ctx.MkIntConst("tea");
            IntExpr water = ctx.MkIntConst("water");
            IntExpr coffee = ctx.MkIntConst("coffee");

            // Smokes variables
            IntExpr chesterfields = ctx.MkIntConst("chesterfields");
            IntExpr kools = ctx.MkIntConst("kools");
            IntExpr luckystrikes = ctx.MkIntConst("luckystrikes");
            IntExpr parliaments = ctx.MkIntConst("parliaments");
            IntExpr oldgolds = ctx.MkIntConst("oldgolds");

            // Make lists of the variables by category, to make some operations a bit easier later
            ImmutableArray<IntExpr> colors = [blue, green, ivory, red, yellow];
            ImmutableArray<IntExpr> nationalities = [english, japanese, norwegian, spanish, ukrainian];
            ImmutableArray<IntExpr> pets = [dog, fox, horse, snails, zebra];
            ImmutableArray<IntExpr> drinks = [coffee, milk, orange_juice, tea, water];
            ImmutableArray<IntExpr> smokes = [chesterfields, kools, luckystrikes, oldgolds, parliaments];

            // Add lower and upper bounds for each variable
            int lowerBound = 1;
            int upperBound = numberOfHouses;
            foreach (var v in colors) s.Add(ctx.MkAnd(ctx.MkLe(ctx.MkInt(lowerBound), v), ctx.MkLe(v, ctx.MkInt(upperBound))));
            foreach (var v in nationalities) s.Add(ctx.MkAnd(ctx.MkLe(ctx.MkInt(lowerBound), v), ctx.MkLe(v, ctx.MkInt(upperBound))));
            foreach (var v in pets) s.Add(ctx.MkAnd(ctx.MkLe(ctx.MkInt(lowerBound), v), ctx.MkLe(v, ctx.MkInt(upperBound))));
            foreach (var v in drinks) s.Add(ctx.MkAnd(ctx.MkLe(ctx.MkInt(lowerBound), v), ctx.MkLe(v, ctx.MkInt(upperBound))));
            foreach (var v in smokes) s.Add(ctx.MkAnd(ctx.MkLe(ctx.MkInt(lowerBound), v), ctx.MkLe(v, ctx.MkInt(upperBound))));

            /* Add Uniqueness constraints across the variables in each
             * category. The same value (house number) should not be 
             * assigned to more than one color variable, more than one
             * pet variable, etc.
             */
            s.Add(ctx.MkDistinct(colors));
            s.Add(ctx.MkDistinct(pets));
            s.Add(ctx.MkDistinct(nationalities));
            s.Add(ctx.MkDistinct(drinks));
            s.Add(ctx.MkDistinct(smokes));

            // Logic constraints from the puzzle statement

            // 1. The Englishman lives in the red house
            s.Add(ctx.MkEq(red, english));

            // 2. The Spaniard owns the dog
            s.Add(ctx.MkEq(dog, spanish));

            // 3. Cofffee is drunk in the green house
            s.Add(ctx.MkEq(green, coffee));

            // 4. The Ukrainian drinks tea
            s.Add(ctx.MkEq(tea, ukrainian));

            // 5. The green house is immediately to the right of the ivory house
            s.Add(ctx.MkEq(green, ivory + 1));

            // 5b. ALTERNATE PUZZLE VERSION:
            // The green house is immediately to the LEFT of the ivory house
            // s.Add(ctx.MkEq(ivory, green + 1));

            // 6. The Old Gold smoker owns snails
            s.Add(ctx.MkEq(snails, oldgolds));

            // 7. Kools are smoked in the yellow house
            s.Add(ctx.MkEq(yellow, kools));

            // 8. Milk is drunk in the middle house
            s.Add(ctx.MkEq(milk, ctx.MkInt(middleHouse)));

            // 9. The Norwegian lives in the first house
            s.Add(ctx.MkEq(norwegian, ctx.MkInt(1)));

            // 10. The man who smokes Chesterfields lives in the house next to the man with the fox
            s.Add(ctx.MkOr([ctx.MkEq(fox - chesterfields, ctx.MkInt(1)), ctx.MkEq(fox - chesterfields, ctx.MkInt(-1))]));

            // 11. Kools are smoked in the house next to the house where the horse is kept
            s.Add(ctx.MkOr([ctx.MkEq(horse - kools, ctx.MkInt(1)), ctx.MkEq(horse - kools, ctx.MkInt(-1))]));

            // 12. The Lucky Strike smoker drinks orange juice
            s.Add(ctx.MkEq(orange_juice, luckystrikes));

            // 13. The Japanese smokes Parliaments
            s.Add(ctx.MkEq(parliaments, japanese));

            // 14. The Norwegian lives next to the blue house
            s.Add(ctx.MkOr([ctx.MkEq(blue - norwegian, ctx.MkInt(1)), ctx.MkEq(blue - norwegian, ctx.MkInt(-1))]));
            /* Since in #9 above norwegian == 1, this
             * constraint could be reduced to blue == 2, as 2
             * will be the only feasible value for any attribute
             * next to the Norwegian's house.
             * But to avoid problems if we experiment with 
             * changing other constraints later, make each
             * constraint independent.
             */
            return s;
        }
    }
}
