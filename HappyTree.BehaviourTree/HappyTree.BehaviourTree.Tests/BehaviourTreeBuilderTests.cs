using System;
using HappyTree.BehaviourTree.Nodes;
using Xunit;

namespace HappyTree.BehaviourTree.Tests
{
    public class BehaviourTreeBuilderTests
    {
        BehaviourTreeBuilder testObject;

        void Init()
        {
            testObject = new BehaviourTreeBuilder();
        }

        [Fact]
        public void cant_create_a_behaviour_tree_with_zero_nodes()
        {
            Init();

            Assert.Throws<ApplicationException>(() =>
                {
                    testObject.Build();
                }
            );

        }

        [Fact]
        public void cant_create_an_unested_action_node()
        {
            Init();

            Assert.Throws<ApplicationException>(() =>
                {
                    testObject
                         .Do(t => BehaviourTreeStatus.Running)
                         .Build();
                }
            );
        }

        [Fact]
        public void can_create_inverter_node()
        {
            Init();

            var node = testObject
                .Inverter()
                    .Do(t =>BehaviourTreeStatus.Success)
                .End()
                .Build();

            Assert.IsType<InverterNode>(node);
            Assert.Equal(BehaviourTreeStatus.Failure, node.Tick(new TimeData()));
        }

        [Fact]
        public void cant_create_an_unbalanced_behaviour_tree()
        {
            Init();

            Assert.Throws<ApplicationException>(() =>
            {
                testObject
                    .Inverter()
                    .Do(t => BehaviourTreeStatus.Success)
                .Build();
            });
        }

        [Fact]
        public void condition_is_syntactic_sugar_for_do()
        {
            Init();

            var node = testObject
                .Inverter()
                    .Condition(t => true)
                .End()
                .Build();

            Assert.IsType<InverterNode>(node);
            Assert.Equal(BehaviourTreeStatus.Failure, node.Tick(new TimeData()));
        }

        [Fact]
        public void can_invert_an_inverter()
        {
            Init();

            var node = testObject
                .Inverter()
                    .Inverter()
                        .Do(t => BehaviourTreeStatus.Success)
                    .End()
                .End()
                .Build();

            Assert.IsType<InverterNode>(node);
            Assert.Equal(BehaviourTreeStatus.Success, node.Tick(new TimeData()));
        }

        [Fact]
        public void adding_more_than_a_single_child_to_inverter_throws_exception()
        {
            Init();

            Assert.Throws<ApplicationException>(() =>
            {
                testObject
                    .Inverter()
                        .Do(t => BehaviourTreeStatus.Success)
                        .Do(t => BehaviourTreeStatus.Success)
                    .End()
                    .Build();
            });
        }

        [Fact]
        public void can_create_a_sequence()
        {
            Init();

            var invokeCount = 0;

            var sequence = testObject
                .Sequence()
                    .Do(t => 
                    {
                        ++invokeCount;
                        return BehaviourTreeStatus.Success;
                    })
                    .Do(t =>
                    {
                        ++invokeCount;
                        return BehaviourTreeStatus.Success;
                    })
                .End()
                .Build();

            Assert.IsType<SequenceNode>(sequence);
            Assert.Equal(BehaviourTreeStatus.Success, sequence.Tick(new TimeData()));
            Assert.Equal(2, invokeCount);
        }

        [Fact]
        public void can_create_parallel()
        {
            Init();

            var invokeCount = 0;

            var parallel = testObject
                .Parallel(2, 2)
                    .Do(t =>
                    {
                        ++invokeCount;
                        return BehaviourTreeStatus.Success;
                    })
                    .Do(t =>
                    {
                        ++invokeCount;
                        return BehaviourTreeStatus.Success;
                    })
                .End()
                .Build();

            Assert.IsType<ParallelNode>(parallel);
            Assert.Equal(BehaviourTreeStatus.Success, parallel.Tick(new TimeData()));
            Assert.Equal(2, invokeCount);
        }

        [Fact]
        public void can_create_selector()
        {
            Init();

            var invokeCount = 0;

            var parallel = testObject
                .Selector()
                    .Do(t =>
                    {
                        ++invokeCount;
                        return BehaviourTreeStatus.Failure;
                    })
                    .Do(t =>
                    {
                        ++invokeCount;
                        return BehaviourTreeStatus.Success;
                    })
                .End()
                .Build();

            Assert.IsType<SelectorNode>(parallel);
            Assert.Equal(BehaviourTreeStatus.Success, parallel.Tick(new TimeData()));
            Assert.Equal(2, invokeCount);
        }

        [Fact]
        public void can_splice_sub_tree()
        {
            Init();

            var invokeCount = 0;

            var spliced = testObject
                .Sequence()
                    .Do(t =>
                    {
                        ++invokeCount;
                        return BehaviourTreeStatus.Success;
                    })
                .End()
                .Build();

            var tree = testObject
                .Sequence()
                    .Splice(spliced)                    
                .End()
                .Build();

            tree.Tick(new TimeData());

            Assert.Equal(1, invokeCount);
        }

        [Fact]
        public void splicing_an_unnested_sub_tree_throws_exception()
        {
            Init();

            var invokeCount = 0;

            var spliced = testObject
                .Sequence()
                    .Do(t =>
                    {
                        ++invokeCount;
                        return BehaviourTreeStatus.Success;
                    })
                .End()
                .Build();

            Assert.Throws<ApplicationException>(() =>
            {
                testObject
                    .Splice(spliced);
            });
        }
    }
}
