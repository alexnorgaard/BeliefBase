using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Frede har ingen appelsiner");
    }
}

namespace BeliefBase
{

    class Belief
    {

        string beliefSet;
        List<string> beliefs;
        
        public Belief(string beliefSet) 
        {
            this.beliefSet = beliefSet;
            this.beliefs.AddRange(DelimitSet(beliefSet, ","));
            // Porno og Store løg // Ja hvorfor ikke?
        }

        public string[] DelimitSet_xD(string beliefSet, string delimiter)
        {
            return beliefSet.Split(delimiter);
        }

        public void Contract_xD(string belief) 
        {
            if (beliefs.Remove(belief)) Console.WriteLine("Belief was successfully removed");
            else Console.WriteLine("Error: Belief was not removed");
        }

        public void Revise_xD(string belief) 
        {
            Contract(!belief);
            Expand(belief);
        }

        //We are not allowing expansion without revision
        private void Expand(string belief) 
        {
            beliefs.Add(belief);
        }
    }

    
}
