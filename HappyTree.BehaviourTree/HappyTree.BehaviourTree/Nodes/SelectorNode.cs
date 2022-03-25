namespace HappyTree.BehaviourTree.Nodes
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// List of child nodes.
        /// </summary>
        private readonly List<IBehaviourTreeNode> _children = new(); //todo: optimization, bake this to an array.
        public BehaviourTreeStatus Tick(TimeData time)
        {
            foreach (var child in _children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Failure)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Add a child node to the selector.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            _children.Add(child);
        }
    }
}
