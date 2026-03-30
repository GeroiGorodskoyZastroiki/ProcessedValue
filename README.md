# ProcessedValue

ProcessedValue is a mini library for creating flexible value processing pipelines. It implements the Command pattern with priority support and lazy evaluation.

## Main Components

- **`ICommand<TValue>`** — command interface for value processing
- **`Command<TValue>`** — simple command based on delegate
- **`ParametrizedCommand<TValue, TParams>`** — parametrized command
- **`Processed<TSort, TValue>`** — container with original value, command queue and priorities

## Usage

### Simple Command

```csharp
// Create Processed
Processed<int> processed = new Processed<int>(2);

// Create command from anonymous method
Command<int> multiplyByTwo = new Command<int>((ref int x) => x *= 2);

// Create command from named method
Command<int> addFive = new Command<int>(AddFiveMethod);

// Add commands with same priority
processed.AddCommand(multiplyByTwo, 0);
// (2)(* 2) = 4

processed.AddCommand(addFive, 0);
// (2)(* 2 + 5) = 9

// Add same command with different priority
processed.AddCommand(addFive, 1);
// (2)(* 2 + 5)(+ 5) = 14
```

### Parametrized Command

```csharp
// Parameter struct
public struct MultiplyParams
{
    public int Factor;
    public string Description;
}

// Create parameters
var parameters = new MultiplyParams { Factor = 3, Description = "Multiply by 3" };

// Create parametrized command
ParametrizedCommand<int, MultiplyParams> multiplyCommand = 
    new ParametrizedCommand<int, MultiplyParams>(
        (ref int value, MultiplyParams p) => value *= p.Factor,
        parameters
    );

// Add to Processed
processed.AddCommand(multiplyCommand, 0);
```

### Command Management

```csharp
// Remove command by priority
processed.RemoveCommand(command, 0);

// Remove command from all priorities
processed.RemoveCommandAtAll(command);

// Check if command exists
bool exists = processed.ContainsCommand(command);

// Force value recalculation
processed.ProcessValue();

// Get current value (recalculated automatically when needed)
int result = processed.Value;
```

### Priorities

```csharp
// Processed<int> uses int as priority type
// Can use enum for type safety

Processed<int> processed = new Processed<int>(10);

// Low priority (executed first)
processed.AddCommand(new Command<int>((ref int x) => x += 1), 0);

// Medium priority
processed.AddCommand(new Command<int>((ref int x) => x *= 2), 100);

// High priority (executed last)
processed.AddCommand(new Command<int>((ref int x) => x -= 5), 200);

// Result: (10 + 1) * 2 - 5 = 17
Console.WriteLine(processed.Value);
```