using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    //public class WorkerArgument
    //{
    //    public int BaseNumber { get; set; }
    //    public double Rand { get; set; }
    //}

    public class DummyMethodContainer: ReactiveAsyncWorker.IMethodContainer<object>
    {



        public DummyMethodContainer ()
        {
    
        }

        public object Method(int i)
        {
            int k;
            for ( k = 0; k < 1000*i; k++)
            {
                for (int j = 0; j < 100000; j++)
                {

                }
            }

            return k.ToString();//new Firefly(((double)StrongRandom.Next(0,10))/10);
        }

        public void PreliminaryMethod()
        {

        }
    }

}
