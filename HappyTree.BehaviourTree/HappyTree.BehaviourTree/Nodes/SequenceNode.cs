namespace HappyTree.BehaviourTree.Nodes
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : IParentBehaviourTreeNode
    {

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private readonly List<IBehaviourTreeNode> _children = new(); //todo: this could be optimized as a baked array.

        public BehaviourTreeStatus Tick(TimeData time)
        {
            foreach (var child in _children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Success)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Add a child to the sequence.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            _children.Add(child);
        }
    }
}
