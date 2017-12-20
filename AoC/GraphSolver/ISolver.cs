namespace AoC.GraphSolver
{
    public interface ISolver
    {
        TNode Evaluate<TNode>(TNode start) where TNode : Node<TNode>;
    }
}