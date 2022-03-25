using HappyTree.BehaviourTree.Nodes;
using Xunit;

namespace HappyTree.BehaviourTree.Tests
{
    public class ActionNodeTests
    {
        [Fact]
        public void can_run_action()
        {
            var time = new TimeData();

            var invokeCount = 0;
            var testObject = 
                new ActionNode(
                    t =>
                    {
                        Assert.Equal(time, t);

                        ++invokeCount;
                        return BehaviourTreeStatus.Running;
                    }
                );

            Assert.Equal(BehaviourTreeStatus.Running, testObject.Tick(time));
            Assert.Equal(1, invokeCount);            
        }
    }
}
