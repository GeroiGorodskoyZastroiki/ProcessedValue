# ProcessedValue
ProcessedValue is an implementation of a data structure (represented by a generic class) that stores a base value, a list of delegates that can sequentially (according to priority) modify the base value, and a property from which the current value (base value after modifications) can be retrieved. Value recalcutes on get if marked dirty (after Add/RemoveProcessor() for example).

Your delegates must match ProcessorDelegate signature:
```c#
void ProcessorDelegate<TValue>(ref TValue value)
```

## Usage Example
```c#
//Create Processed
Processed<int> processed = new Processed<int>(2);

//Create Processor from anonymous lambda
Processor<int> processor = new Processor<int>((ref int x) => x *= 2);

//Create Processor from named method
Processor<int> processor2 = new Processor<int>(SomeMethodName);

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

//Add Processor to another Processed
processed2.AddProcessor(processor2, 0);
//(0)(+ 2) = 2

//Remove first such Processor from priority
processed.RemoveProcessor(processor2, 0);
//(2)(* 2 + 2)(+ 2) = 8

//Remove Processor from all priorities
processed.RemoveProcessorAtAll(processor2);
//(2)(* 2) = 4

//Check if Processed contains Processor
processed.ContainsProcessor(processor2);
//false

//Force call ProcessValue
processed.ProcessValue();
```

ProcessedValue was developed for use with Unity, so the code contains #if UNITY_EDITOR directives.  
