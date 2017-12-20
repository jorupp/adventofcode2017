using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.GraphSolver
{
    public abstract class Node<TNode> where TNode : Node<TNode>
    {
        protected Node()
        {
            _keyValue = new Lazy<string>(() =>
            {
                var keys = this.Keys;
                if (keys.Length == 0)
                    return "";
                if (keys.Length == 1)
                    return keys[0].ToString();
                return string.Join("_", this.Keys);
            });
        }

        public abstract IEnumerable<TNode> GetAdjacent();
        public abstract bool IsValid { get; }
        public abstract bool IsComplete { get; }
        public abstract decimal CurrentCost { get; }
        public abstract decimal EstimatedCost { get; }
        public abstract object[] Keys { get; }
        private Lazy<string> _keyValue;
        public string Key => _keyValue.Value;
        public abstract string Description { get; }
    }
}
