using BeliefBase;
using System;
using System.Collections.Generic;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        Belief b = new Belief("p, q, p > q, a, !b, !c, y, z, z > x, y > a");
        b.PrintBeliefs();
        Console.WriteLine("\n");
        b.Revise("!a");
        Console.WriteLine("\n");
        b.PrintBeliefs();

    }
}

namespace BeliefBase
{

    class Belief
    {
        string beliefSet;
        public List<string> beliefs { get; set; }
        
        public Belief(string beliefSet) 
        {
            beliefs = new List<string>();
            this.beliefSet = beliefSet;
            this.beliefs.AddRange(DelimitSet(beliefSet, ", "));
        }

        public string[] DelimitSet(string beliefSet, string delimiter)
        {
            return beliefSet.Split(delimiter);
        }

        public void PrintBeliefs()
        {
            string printstring = "";
            foreach (string belief in beliefs)
            {
                printstring += belief + ", ";
            }
            Console.WriteLine(printstring);
        }

        public void Contract(string belief) 
        {
            if (beliefs.Remove(belief)) Console.WriteLine("Belief: '" + belief +  "' was successfully removed");
        }

        //Negates the belief in the argument
        public string GetOppositeMember(string member)
        {
            //case belief = "p"
            if(member.Length == 1)
            {
                return "!" + member;
            }
            //case belief = "!p"
            else if (member.Length == 2)
            {
                return member.Substring(1);
            }
            else
            {
                return "error";
            }
        }

        public void ReviewBeliefSet()
        {
            List<string> SingleBelief = new List<string>();
            List<string> MultiBelief = new List<string>();
            foreach(string b in beliefs)
            {
                if(b.Length <= 2) SingleBelief.Add(b);
                else MultiBelief.Add(b);
            }
            for(int i = 0; i < SingleBelief.Count; i++)
            {
                for(int j = 0; j < SingleBelief.Count; j++)
                {
                    foreach(string b in MultiBelief)
                    {
                        string[] beliefArray = DelimitSet(b, " ");
                        if(beliefArray[0].Equals(GetOppositeMember(SingleBelief[i])) && beliefArray[2].Equals(SingleBelief[j]))
                        {
                            if(beliefArray[1].Equals("<>"))
                            {
                                Contract(b);
                            }
                        }
                        if (beliefArray[0].Equals(SingleBelief[i]) && beliefArray[2].Equals(GetOppositeMember(SingleBelief[j])))
                        {
                            if (beliefArray[1].Equals("<>"))
                            {
                                Contract(b);
                            }
                            if (beliefArray[1].Equals(">"))
                            {
                                Contract(b);
                            }
                        }
                    }
                }
            }
        }
        public void ContractMemberInconsistencies(string belief)
        {
            List<string> beliefCopy = new List<string>(beliefs);
            foreach(string b in beliefCopy)
            {
                string[] beliefArray = DelimitSet(b, " ");
                if (beliefArray.Length > 1)
                {
                    if (beliefArray[0].Equals(GetOppositeMember(belief)))
                    {
                        Contract(GetOppositeMember(belief));
                        Contract(GetOppositeMember(belief) + " & " + beliefArray[2]);
                        Contract(GetOppositeMember(belief) + " & " + GetOppositeMember(beliefArray[2]));
                    }
                    if (beliefArray[2].Equals(GetOppositeMember(belief)))
                    {
                        Contract(beliefArray[0] + " & " + GetOppositeMember(belief));
                    }
                }
            }
        }

        //Removes logical inconcistencies of new belief
        public void ContractInconsistencies(string belief)
        {
            string[] beliefArray = DelimitSet(belief, " ");
            if (beliefArray.Length == 1)
            {
                Contract(GetOppositeMember(belief));
                ContractMemberInconsistencies(belief);
            }
            else if (beliefArray[1].Equals(">"))
            {
                Contract(GetOppositeMember(beliefArray[0]) + " > " + beliefArray[2]);
                Contract(beliefArray[0] + " > " + GetOppositeMember(beliefArray[2]));
            }
            else if (beliefArray[1].Equals("<>"))
            {
                Contract(beliefArray[0] + " <> " + GetOppositeMember(beliefArray[2]));
                Contract(GetOppositeMember(beliefArray[0]) + " <> " + beliefArray[2]);
            }
            else if (beliefArray[1].Equals("&"))
            {
                Contract(beliefArray[0] + " & " + GetOppositeMember(beliefArray[2]));
                Contract(GetOppositeMember(beliefArray[0]) + " & " + beliefArray[2]);
                Contract(GetOppositeMember(beliefArray[0]) + " & " + GetOppositeMember(beliefArray[2]));
            }
            else if (beliefArray[1].Equals("V"))
            {
                Contract(GetOppositeMember(beliefArray[0]) + " V " + GetOppositeMember(beliefArray[2]));
            }
        }

        //Belief has to be of type 'member -space- operator -space- member' or else brick.
        //Belief member has to be one letter.
        public void Revise(string belief) 
        {
            ContractInconsistencies(belief);
            Expand(belief);
            ReviewBeliefSet();
        }

        //We are not allowing expansion without revision
        private void Expand(string belief) 
        {
            beliefs.Add(belief);
        }
    }

    
}
