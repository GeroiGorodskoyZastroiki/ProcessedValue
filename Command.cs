namespace ProcessedValue
{
    /// <summary>
    /// Command delegate for processing a value
    /// </summary>
    /// <typeparam name="TValue">Type of value to process</typeparam>
    /// <param name="value">Value to process (passed by reference)</param>
    public delegate void CommandDelegate<TValue>(ref TValue value);

    /// <summary>
    /// Simple command for processing values
    /// </summary>
    /// <typeparam name="TValue">Type of value to process</typeparam>
    public class Command<TValue> : ICommand<TValue>
    {
        /// <summary>
        /// Command delegate
        /// </summary>
        public CommandDelegate<TValue> Delegate { get; set; }

        /// <summary>
        /// Create an empty command
        /// </summary>
        public Command() { }

        /// <summary>
        /// Create a command with delegate
        /// </summary>
        /// <param name="commandDelegate">Value processing delegate</param>
        public Command(CommandDelegate<TValue> commandDelegate)
        {
            Delegate = commandDelegate;
        }

        /// <inheritdoc/>
        public void Execute(ref TValue value)
        {
            Delegate?.Invoke(ref value);
        }
    }
}