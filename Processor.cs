namespace ProcessedValue
{
    public delegate void ProcessorDelegate<TValue>(ref TValue value);

    public class Processor<TValue>
    {
        public ProcessorDelegate<TValue> Delegate;

        public Processor() {}

        public Processor(ProcessorDelegate<TValue> processor) =>
            Delegate = processor;
    }
}
