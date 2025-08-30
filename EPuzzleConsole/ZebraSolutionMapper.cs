using Decider.Csp.BaseTypes;
using Google.OrTools.Sat;
using Microsoft.Z3;
using System.Collections.Immutable;

namespace EPuzzleConsole
{
    internal class ZebraSolutionMapper
    {
        ZebraSolution? solution;
        public ZebraSolution ToSolutionFromDecider(IDictionary<string, IVariable<int>> solutionVariables)
        {
            solution = new();
            foreach (var solutionVariableEntry in solutionVariables)
            {
                IVariable<int> solutionVar = solutionVariableEntry.Value;
                string attribute = DecideAttribute(solutionVar.Name);
                string attributeValue = solutionVar.Name;
                int houseNumber = solutionVar.InstantiatedValue;
                SetAttributeValue(houseNumber, attribute, attributeValue);
            }
            return solution;
        }
        public ZebraSolution ToSolutionFromCpSolver(ImmutableArray<IntVar> variablesOfInterest, CpSolver solver)
        {
            solution = new();

            foreach (IntVar modelIntVar in variablesOfInterest)
            {
                string attribute = DecideAttribute(modelIntVar.Name());
                string attributeValue = modelIntVar.Name();
                int houseNumber = (int)solver.Value(modelIntVar);
                SetAttributeValue(houseNumber, attribute, attributeValue);
            }
            return solution;
        }

        public ZebraSolution ToSolutionFromZ3(Solver s)
        {
            solution = new();
            IEnumerable<KeyValuePair<FuncDecl, Expr>> cs = s.Model.Consts;
            foreach (var c in cs)
            {
                string attribute = DecideAttribute(c.Key.Name.ToString());
                string attributeValue = c.Key.Name.ToString();
                int houseNumber = ((IntNum)c.Value).Int;
                SetAttributeValue(houseNumber, attribute, attributeValue);
            }
            return solution;
        }

        private void SetAttributeValue(int houseNumber, string attribute, string attributeValue)
        {
            House? house = solution?.Houses[houseNumber];
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
