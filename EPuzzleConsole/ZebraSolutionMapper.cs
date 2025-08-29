using Decider.Csp.BaseTypes;
using Google.OrTools.Sat;
using System.Collections.Immutable;

namespace EPuzzleConsole
{
    internal class ZebraSolutionMapper
    {
        public static ZebraSolution ToSolutionFromDecider(IDictionary<string, IVariable<int>> solutionVariables)
        {
            ZebraSolution solution = new();
            foreach (var solutionVariableEntry in solutionVariables)
            {
                IVariable<int> solutionVar = solutionVariableEntry.Value;
                int value = solutionVar.InstantiatedValue;
                switch (solutionVar.Name)
                {
                    case "blue":
                    case "green":
                    case "ivory":
                    case "red":
                    case "yellow":
                        solution.Houses[value].Color = solutionVar.Name;
                        break;
                    case "dog":
                    case "fox":
                    case "horse":
                    case "snails":
                    case "zebra":
                        solution.Houses[value].Pet = solutionVar.Name;
                        break;
                    case "english":
                    case "japanese":
                    case "norwegian":
                    case "spanish":
                    case "ukrainian":
                        solution.Houses[value].Nationality = solutionVar.Name;
                        break;
                    case "coffee":
                    case "milk":
                    case "orange_juice":
                    case "tea":
                    case "water":
                        solution.Houses[value].Drinks = solutionVar.Name;
                        break;
                    case "chesterfields":
                    case "kools":
                    case "luckystrikes":
                    case "oldgolds":
                    case "parliaments":
                        solution.Houses[value].Smokes = solutionVar.Name;
                        break;
                }
            }
            return solution;
        }
        public static ZebraSolution ToSolutionFromCpSolver(ImmutableArray<IntVar> variablesOfInterest, CpSolver solver)
        {
            ZebraSolution solution = new();

            foreach (IntVar modelIntVar in variablesOfInterest)
            {
                int value = (int)solver.Value(modelIntVar);
                switch(modelIntVar.Name())
                {
                    case "blue":
                    case "green":
                    case "ivory":
                    case "red":
                    case "yellow":
                        solution.Houses[value].Color = modelIntVar.Name();
                        break;
                    case "dog":
                    case "fox":
                    case "horse":
                    case "snails":
                    case "zebra":
                        solution.Houses[value].Pet = modelIntVar.Name();
                        break;
                    case "english":
                    case "japanese":
                    case "norwegian":
                    case "spanish":
                    case "ukrainian":
                        solution.Houses[value].Nationality = modelIntVar.Name();
                        break;
                    case "coffee":
                    case "milk":
                    case "orange_juice":
                    case "tea":
                    case "water":
                        solution.Houses[value].Drinks = modelIntVar.Name();
                        break;
                    case "chesterfields":
                    case "kools":
                    case "luckystrikes":
                    case "parliaments":
                    case "oldgolds":
                        solution.Houses[value].Smokes = modelIntVar.Name();
                        break;
                    default:
                        throw new ArgumentException($"Unexpected variable name: {modelIntVar.Name()}");
                }
            }
            return solution;
        }
    }
}
