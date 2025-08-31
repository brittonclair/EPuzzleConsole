using EPuzzleConsole;

namespace EPuzzleConsoleTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Setting up for tests...");
        }

        [Test]
        public void Acceptance()
        {
            Zebra zebra = new();
            var solution = zebra.SolveUsingCpSolver();

            // Solution not null
            //Assert.That(solution, Is.Not.EqualTo(null));

            // Solution has 5 houses
            // with Ids 1 to 5
            // and keys match House Id's
            Assert.That(solution.Houses.Count, Is.EqualTo(5));
            for (int idx = 1; idx <= 5; idx++)
            {
                Assert.That(solution.Houses.ContainsKey(idx), Is.True);
            }
            foreach (int key in solution.Houses.Keys)
            {
                Assert.That(solution.Houses[key].Id, Is.EqualTo(key));
                VerifyHouseAttributes(solution.Houses[key]);
            }
            Console.WriteLine("CpSolver solution verified.");

            solution = zebra.SolveUsingDecider();

            // Solution not null
            Assert.That(solution, Is.Not.EqualTo(null));

            // Solution has 5 houses
            // with Ids 1 to 5
            // and keys match House Id's
            Assert.That(solution.Houses.Count, Is.EqualTo(5));
            for (int idx = 1; idx <= 5; idx++)
            {
                Assert.That(solution.Houses.ContainsKey(idx), Is.True);
            }
            foreach (int key in solution.Houses.Keys)
            {
                Assert.That(solution.Houses[key].Id, Is.EqualTo(key));
                VerifyHouseAttributes(solution.Houses[key]);
            }
            Console.WriteLine("Decider solution verified.");


            solution = zebra.SolveUsingZ3();
            // Solution not null
            Assert.That(solution, Is.Not.EqualTo(null));

            // Solution has 5 houses
            // with Ids 1 to 5
            // and keys match House Id's
            Assert.That(solution.Houses.Count, Is.EqualTo(5));
            for (int idx = 1; idx <= 5; idx++)
            {
                Assert.That(solution.Houses.ContainsKey(idx), Is.True);
            }
            foreach (int key in solution.Houses.Keys)
            {
                Assert.That(solution.Houses[key].Id, Is.EqualTo(key));
                VerifyHouseAttributes(solution.Houses[key]);
            }
            Console.WriteLine("Z3 solution verified.");
        }

        private void VerifyHouseAttributes(House house)
        {
            // House 1 attributes are correct
            if (house.Id == 1)
            {
                Assert.That(house.Color, Is.EqualTo("yellow"));
                Assert.That(house.Nationality, Is.EqualTo("norwegian"));
                Assert.That(house.Pet, Is.EqualTo("fox"));
                Assert.That(house.Drinks, Is.EqualTo("water"));
                Assert.That(house.Smokes, Is.EqualTo("kools"));
            }

            // House 2 attributes are correct
            else if(house.Id == 2)
            {
                Assert.That(house.Color, Is.EqualTo("blue"));
                Assert.That(house.Nationality, Is.EqualTo("ukrainian"));
                Assert.That(house.Pet, Is.EqualTo("horse"));
                Assert.That(house.Drinks, Is.EqualTo("tea"));
                Assert.That(house.Smokes, Is.EqualTo("chesterfields"));
            }

            // House 3 attributes are correct
            else if(house.Id == 3)
            {
                Assert.That(house.Color, Is.EqualTo("red"));
                Assert.That(house.Nationality, Is.EqualTo("english"));
                Assert.That(house.Pet, Is.EqualTo("snails"));
                Assert.That(house.Drinks, Is.EqualTo("milk"));
                Assert.That(house.Smokes, Is.EqualTo("oldgolds"));
            }

            // House 4 attributes are correct
            else if (house.Id == 4)
            {
                Assert.That(house.Color, Is.EqualTo("ivory"));
                Assert.That(house.Nationality, Is.EqualTo("spanish"));
                Assert.That(house.Pet, Is.EqualTo("dog"));
                Assert.That(house.Drinks, Is.EqualTo("orange_juice"));
                Assert.That(house.Smokes, Is.EqualTo("luckystrikes"));
            }

            // House 5 attributes are correct
            else if (house.Id == 5)
            {
                Assert.That(house.Color, Is.EqualTo("green"));
                Assert.That(house.Nationality, Is.EqualTo("japanese"));
                Assert.That(house.Pet, Is.EqualTo("zebra"));
                Assert.That(house.Drinks, Is.EqualTo("coffee"));
                Assert.That(house.Smokes, Is.EqualTo("parliaments"));
            }

            else
            {
                Assert.Fail($"House Id {house.Id} is out of expected range.");
            }
        }
    }
}
