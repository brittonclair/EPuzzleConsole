using EPuzzleConsole.Adapters;

namespace EPuzzleConsole
{
    internal class ZebraSolutionMapper
    {
        ZebraSolution? zebraSolution;
        /* 
         * Map from a generic SolverSolution (a collection of 
         * variable-value pairs extracted from a specific solver)
         * to a specific problem solution (a ZebraSolution).
         */

        internal ZebraSolution FromSolverSolution(SolverSolution solverSolution)
        {
            zebraSolution = new(solverSolution.SolverLabel);
            foreach (var solutionVariableEntry in solverSolution.SolutionValues)
            {
                string attribute = DecideAttribute(solutionVariableEntry.Key);
                string attributeValue = solutionVariableEntry.Key;
                int houseNumber = solutionVariableEntry.Value;
                SetAttributeValue(houseNumber, attribute, attributeValue);
            }
            return zebraSolution;
        }

        private void SetAttributeValue(int houseNumber, string attribute, string attributeValue)
        {
            House? house = zebraSolution?.Houses[houseNumber];
            if (house == null)
            {
                throw new ArgumentException($"House with index {houseNumber} does not exist.");
            }
            bool attributeWasSet = house.SetAttribute(attribute, attributeValue);
            if (!attributeWasSet)
            {
                throw new ArgumentException($"Failed to set attribute {attribute} with value {attributeValue} for house index {houseNumber}.");
            }
        }

        private static string DecideAttribute(string modelVariableName)
        {
            string attribute = string.Empty;
            switch (modelVariableName)
            {
                case "blue":
                case "green":
                case "ivory":
                case "red":
                case "yellow":
                    attribute = "Color";
                    break;
                case "dog":
                case "fox":
                case "horse":
                case "snails":
                case "zebra":
                    attribute = "Pet";
                    break;
                case "english":
                case "japanese":
                case "norwegian":
                case "spanish":
                case "ukrainian":
                    attribute = "Nationality";
                    break;
                case "coffee":
                case "milk":
                case "orange_juice":
                case "tea":
                case "water":
                    attribute = "Drinks";
                    break;
                case "chesterfields":
                case "kools":
                case "luckystrikes":
                case "oldgolds":
                case "parliaments":
                    attribute = "Smokes";
                    break;
                default:
                    throw new ArgumentException($"Unexpected variable name: {modelVariableName}");
            }
            return attribute;
        }
    }
}
