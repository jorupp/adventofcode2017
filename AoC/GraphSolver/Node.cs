using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.GraphSolver
{
    public abstract class Node<TNode> where TNode : Node<TNode>
    {
        public abstract IEnumerable<TNode> GetAdjacent();
        public abstract bool IsValid { get; }
        public abstract bool IsComplete { get; }
        public abstract decimal CurrentCost { get; }
        public abstract decimal EstimatedCost { get; }
        public abstract object[] Keys { get; }
        public string Key { get { return string.Join("_", Keys); } }
        public abstract string Description { get; }
    }
}
