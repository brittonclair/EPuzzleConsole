using System.Text;

namespace EPuzzleConsole
{
    internal class ZebraSolution
    {
        internal string SolverLabel { get; set; }

        internal Dictionary<int, House> Houses { get; private set; } = [];
        internal ZebraSolution(string solverLabel)
        {
            SolverLabel = solverLabel;
            for (int i = 1; i <= 5; i++)
            {
                Houses[i] = new House(i);
            }
        }

        internal House? GetHouseByAttribute(string attribute, string value)
        {
            switch (attribute.ToLower())
            {
                case "color":
                    foreach (var house in Houses.Values)
                    {
                        if (house.Color.Equals(value, StringComparison.OrdinalIgnoreCase))
                        {
                            return house;
                        }
                    }
                    break;
                case "nationality":
                    foreach (var house in Houses.Values)
                    {
                        if (house.Nationality.Equals(value, StringComparison.OrdinalIgnoreCase))
                        {
                            return house;
                        }
                    }
                    break;
                case "drinks":
                    foreach (var house in Houses.Values)
                    {
                        if (house.Drinks.Equals(value, StringComparison.OrdinalIgnoreCase))
                        {
                            return house;
                        }
                    }
                    break;
                case "smokes":
                    foreach (var house in Houses.Values)
                    {
                        if (house.Smokes.Equals(value, StringComparison.OrdinalIgnoreCase))
                        {
                            return house;
                        }
                    }
                    break;
                case "pet":
                    foreach (var house in Houses.Values)
                    {
                        if (house.Pet.Equals(value, StringComparison.OrdinalIgnoreCase))
                        {
                            return house;
                        }
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        internal void Print(string heading = "")
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(heading);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Houses are numbered left-to-right.");
            
            int[] keys = [.. Houses.Keys];
            Array.Sort(keys);
            foreach (int key in keys)
            {
                sb.Append(Houses[key].ToString());
            }
            return sb.ToString();
        }
    }

    internal class House
    {
        internal int Id { get; private set; }
        internal string Color { get; set; } = "";
        internal string Nationality { get; set; } = "";
        internal string Pet { get; set; } = "";
        internal string Drinks { get; set; } = "";
        internal string Smokes { get; set; } = "";

        internal House(int id, string color="", string nationality = "", string pet = "", string drinks = "", string smokes = "")
        {
            Id = id;
            Color = color;
            Nationality = nationality;
            Pet = pet;
            Drinks = drinks;
            Smokes = smokes;
        }

        internal bool SetAttribute(string attribute, string value)
        {
            bool result = true;
            switch (attribute.ToLower())
            {
                case "color":
                    Color = value;
                    break;
                case "nationality":
                    Nationality = value;
                    break;
                case "pet":
                    Pet = value;
                    break;
                case "drinks":
                    Drinks = value;
                    break;
                case "smokes":
                    Smokes = value;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }


        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"\nHouse Number {Id}: {Color}");
            sb.Append($"Nationality: {Nationality}, ");
            sb.Append($"Pet: {Pet}, ");
            sb.Append($"Drinks: {Drinks}, ");
            sb.Append($"Smokes: {Smokes}\n");
            return sb.ToString();
        }

    }

}
