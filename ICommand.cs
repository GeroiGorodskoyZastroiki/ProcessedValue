namespace ProcessedValue
{
    /// <summary>
    /// Interface for a value processing command
    /// </summary>
    /// <typeparam name="TValue">Type of value to process</typeparam>
    public interface ICommand<TValue>
    {
        /// <summary>
        /// Execute the command on a value
        /// </summary>
        /// <param name="value">Value to process</param>
        void Execute(ref TValue value);
    }
}