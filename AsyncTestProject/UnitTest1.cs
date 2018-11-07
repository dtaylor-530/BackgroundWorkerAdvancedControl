using System;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveAsyncWorker;

namespace AsyncTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var xx = new TPLDataFlowQueue<string>();
            var yt = Task.Run(() => xx.Enqueue(new AsyncWorkerItem<string>(new Task<string>(() =>
            {
                return "s";
                /*Thread.Sleep(500);*/
            }), "")));

            xx.Resource.Subscribe(_ =>
            {
                if (_.Key == DynamicData.ChangeReason.Remove)
                {

                }
            }, (e) =>
            {
            }, () =>
            {
            });


        }

        [TestMethod]
        public void TestMethod2()
        {
            var ix=Task.Run(async () =>
            { 
            var cts = new System.Threading.CancellationTokenSource();
            var block = new ActionBlock<Hamster>(
    async _ => await _.FeedAsync(),
    new ExecutionDataflowBlockOptions
    {
        BoundedCapacity = 10000, // Cap the item count
        CancellationToken = cts.Token, // Enable cancellation
        MaxDegreeOfParallelism = 1, // Parallelize on all cores
    });
                var hamsters = Enumerable.Range(0, 10).Select(_ => new Hamster());
            // Add items to the block and asynchronously wait if BoundedCapacity is reached
            foreach (Hamster hamster in hamsters)
            {
                var x= block.SendAsync(hamster);
                    x.ToObservable().Subscribe(_ =>
                    {

                    });
                    await x;
            }

            block.Complete();
            await block.Completion;
                return block;
        });
            var t=ix.Result;
        }


        class Hamster
        {

            public async Task<bool> FeedAsync()
            {

                return await Task.Run(() => ds());
            }

            private static bool ds()
            {

                foreach (var x in System.Linq.Enumerable.Range(0, 100000000))
                {
                    if (x > 10000000)
                        return true;
                }

                return false;
            }




        }
    }
}
