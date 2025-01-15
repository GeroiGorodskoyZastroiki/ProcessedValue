# ProcessedValue
ProcessedValue is an implementation of a data structure (represented by a generic class) that stores a base value, a list of delegates that can sequentially (according to priority) modify the base value, and a property from which the current value (base value after modifications) can be retrieved. Value recalcutes on get if marked dirty (after Add/RemoveProcessor() for example).

Your delegates must match ProcessorDelegate signature:
```c#
void ProcessorDelegate<TValue>(ref TValue value)
```

## Usage Example
```c#
//Create Processed with base value
Processed<int> processed = new Processed<int>(2);

//Create Processed w/o base value (base value = default)
Processed<int> processed2 = new Processed<int>();

//Create Processor from anonymous lambda
Processor<int> processor = new Processor<int>((ref int x) => x *= 2);

//Create Processor from named method
Processor<int> processor2;
void Process(ref int x)
{
    //do whatever
    x += 2;
}
//A field initializer cannot reference the non-static field, so ...
void SomeMethod() =>
    processor2 = new Processor<int>(Process);

//Add Processor to Processed
processed.AddProcessor(processor, 0); //(processor, priority)
//(2)(* 2) = 4

//Add another Processor on same priority
processed.AddProcessor(processor2, 0);
//(2)(* 2 + 2) = 6

//Add same Processor on same priority
processed.AddProcessor(processor2, 0);
//(2)(* 2 + 2 + 2) = 8

//Add same Processor on another priority
processed.AddProcessor(processor2, 1);
//(2)(* 2 + 2 + 2)(+ 2) = 10

//Add Processor on another Processed
processed2.AddProcessor(processor2, 0);
//(0)(+ 2) = 2

//Remove first Processor from priority
processed.RemoveProcessor(processor2, 0);
//(2)(* 2 + 2)(+ 2) = 8

//Remove Processor from all priorities
processed.RemoveProcessorAtAll(processor2);
//(2)(* 2) = 4

//Check if Processed contains Processor
processed.ContainsProcessor(processor2);
//false

//Force call ProcessValue (in most cases it doesn't make sense)
processed.ProcessValue();
```

ProcessedValue was developed for use with Unity, so the code contains #if UNITY_EDITOR directives.  
You can get rid of them if you don't deal with Unity.