namespace HappyTree.BehaviourTree.Nodes
{
    /// <summary>
    /// Runs childs nodes in parallel.
    /// </summary>
    public class ParallelNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// List of child nodes.
        /// </summary>
        private readonly List<IBehaviourTreeNode> _children = new();

        /// <summary>
        /// Number of child failures required to terminate with failure.
        /// </summary>
        private readonly int _numRequiredToFail;

        /// <summary>
        /// Number of child successess require to terminate with success.
        /// </summary>
        private readonly int _numRequiredToSucceed;

        public ParallelNode(int numRequiredToFail, int numRequiredToSucceed)
        {
            _numRequiredToFail = numRequiredToFail;
            _numRequiredToSucceed = numRequiredToSucceed;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            var numChildrenSuceeded = 0;
            var numChildrenFailed = 0;

            foreach (var child in _children)
            {
                var childStatus = child.Tick(time);
                switch (childStatus)
                {
                    case BehaviourTreeStatus.Success: ++numChildrenSuceeded; break;
                    case BehaviourTreeStatus.Failure: ++numChildrenFailed; break;
                }
            }

            if (_numRequiredToSucceed > 0 && numChildrenSuceeded >= _numRequiredToSucceed)
            {
                return BehaviourTreeStatus.Success;
            }

            if (_numRequiredToFail > 0 && numChildrenFailed >= _numRequiredToFail)
            {
                return BehaviourTreeStatus.Failure;
            }

            return BehaviourTreeStatus.Running;
        }

        public void AddChild(IBehaviourTreeNode child)
        {
            _children.Add(child);
        }
    }
}
