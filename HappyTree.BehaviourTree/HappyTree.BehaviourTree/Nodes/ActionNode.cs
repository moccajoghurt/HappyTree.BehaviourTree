namespace HappyTree.BehaviourTree.Nodes
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : IBehaviourTreeNode
    {

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private readonly Func<TimeData, BehaviourTreeStatus> _fn;
        

        public ActionNode(Func<TimeData, BehaviourTreeStatus> fn)
        {
            _fn=fn;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            return _fn(time);
        }
    }
}
