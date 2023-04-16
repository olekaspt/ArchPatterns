using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    class PartView : IObserver<Model.Part>
    {
        private IDisposable? cancellation;
        public PartView() 
        {
            cancellation = null;
        }

        public virtual void Subscribe(Controller.PartController provider)
        {
            cancellation = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            Console.WriteLine("My job is done, and no longer needed");
            if(cancellation != null) 
                cancellation.Dispose();

            
        }

        public virtual void OnCompleted()
        {
            Console.WriteLine("My job is done, and no longer needed");
        }

        // No implementation needed: Method is not called by the BaggageHandler class.
        public virtual void OnError(Exception e)
        {
            // No implementation.
        }

        // Update information.
        public virtual void OnNext(Model.Part info)
        {
            Console.WriteLine("View - Part Name - " + info.PartName + " Part Type " + info.PartType);
        }

    }
}
