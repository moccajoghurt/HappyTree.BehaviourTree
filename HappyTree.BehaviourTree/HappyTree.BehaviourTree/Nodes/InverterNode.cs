namespace HappyTree.BehaviourTree.Nodes
{
    /// <summary>
    /// Decorator node that inverts the success/failure of its child.
    /// </summary>
    public class InverterNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// The child to be inverted.
        /// </summary>
        private IBehaviourTreeNode _childNode = null!;

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (_childNode == null)
            {
                throw new ApplicationException("InverterNode must have a child node!");
            }

            var result = _childNode.Tick(time);
            if (result == BehaviourTreeStatus.Failure)
            {
                return BehaviourTreeStatus.Success;
            }
            else if (result == BehaviourTreeStatus.Success)
            {
                return BehaviourTreeStatus.Failure;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            if (_childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to InverterNode!");
            }

            _childNode = child;
        }
    }
}
