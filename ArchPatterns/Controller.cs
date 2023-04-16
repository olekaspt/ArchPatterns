using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Controller
{

    class Unsubscriber : IDisposable
    {
        List<IObserver<Model.Part>> _observers;
        IObserver<Model.Part> _observer;

        public Unsubscriber(List<IObserver<Model.Part>> observers, IObserver<Model.Part> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    class PartController : IObservable<Model.Part>
    {


        public PartController()
        {
            observers = new List<IObserver<Model.Part>>();
            parts = new List<Model.Part>();
        }

        public IDisposable Subscribe(IObserver<Model.Part> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Provide observer with existing data.
                foreach (var item in parts)
                    observer.OnNext(item);
            }
            return new Unsubscriber(observers, observer);
        }

        public void CreatePart(String partName, String PartType)
        {
            Console.WriteLine("Controller - CreatePart " + partName + " " + PartType);

            var part = new Model.Part(partName, PartType);

            parts.Add(part);


            Notify(part);
        }

        public void RemovePart(String partName)
        {
            Console.WriteLine("Controller - RemovePart " + partName);

            foreach (var item in parts)
            {
                if (item.PartName == partName)
                {
                    Notify(item);

                    parts.Remove(item);
                    return;
                }
            }

            

        }

        private void Notify(Model.Part part)
        {
            foreach(var obs in observers)
            {
                obs.OnNext(part);
            }
        }

        public void CloseAllParts()
        {
            Console.WriteLine("Controller - CloseAllParts ");
            foreach (var part in parts)
            {
                Notify(part);
            }
            parts.Clear();

            foreach (var observer in observers)
                observer.OnCompleted();

            observers.Clear(); 
        }

        private List<IObserver<Model.Part>> observers;
        private List<Model.Part> parts;
    }
}



