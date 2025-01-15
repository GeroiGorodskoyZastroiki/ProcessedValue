using System;
using System.Linq;
using System.Collections.Generic;

namespace ProcessedValue
{
    [Serializable] public class Processed<TValue> : Processed<int, TValue> 
    {
        public Processed() {}
        public Processed(TValue baseValue) : base(baseValue) {}
    }

    [Serializable] public class Processed<TSort, TValue>
    {
        public bool IsDirty;
        #if UNITY_EDITOR
        [UnityEngine.SerializeReference] 
        #endif
        public TValue BaseValue;
        #if UNITY_EDITOR
        [UnityEngine.SerializeReference] 
        #endif
        protected TValue _value;
        public TValue Value
        { 
            get
            {
                if (IsDirty) ProcessValue();
                return _value;
            }
        }
        public SortedList<TSort, List<Processor<TValue>>> Processors = new SortedList<TSort, List<Processor<TValue>>>();

        public Processed() {}

        public Processed(TValue baseValue) =>
            BaseValue = _value = baseValue;

        public virtual void AddProcessor(Processor<TValue> processor, TSort priority)
        {
            if (Processors.ContainsKey(priority)) 
                Processors[priority].Add(processor);
            else 
                Processors.Add(priority, new List<Processor<TValue>>(){ processor });

            IsDirty = true; 
        }

        public virtual void RemoveProcessor(Processor<TValue> processor, TSort priority)
        {
            if (Processors[priority].FirstOrDefault(x => x == processor) == null) 
            {
                Console.Error.WriteLine("Can't find processor to remove.");
                #if UNITY_EDITOR
                UnityEngine.Debug.LogWarning("Can't find processor to remove.");
                #endif
                return;
            }
            else 
                Processors[priority].Remove(processor);

            IsDirty = true;      
        }

        public void RemoveProcessorAtAll(Processor<TValue> processor)
        {
            foreach (var processorList in Processors.Values)
                if (processorList.Contains(processor)) 
                    processorList.Remove(processor);
                
            IsDirty = true;  
        }

        public virtual bool ContainsProcessor(Processor<TValue> processor) =>
            Processors.Any(x => x.Value.Contains(processor) == true);

        public virtual void ProcessValue()
        {
            IsDirty = false;

            _value = BaseValue;
            foreach (var priorityList in Processors)
                foreach (var processor in priorityList.Value)
                    processor.Delegate(ref _value);
        }
    }
}
