namespace ProcessedValue
{
    /// <summary>
    /// Delegate for a parametrized command
    /// </summary>
    /// <typeparam name="TValue">Type of value to process</typeparam>
    /// <typeparam name="TParams">Type of command parameters</typeparam>
    /// <param name="value">Value to process (passed by reference)</param>
    /// <param name="parameters">Command parameters</param>
    public delegate void ParametrizedCommandDelegate<TValue, TParams>(ref TValue value, TParams parameters);

    /// <summary>
    /// Parametrized command for processing values
    /// </summary>
    /// <typeparam name="TValue">Type of value to process</typeparam>
    /// <typeparam name="TParams">Type of command parameters</typeparam>
    public class ParametrizedCommand<TValue, TParams> : ICommand<TValue>
    {
        /// <summary>
        /// Command delegate
        /// </summary>
        public CommandDelegate<TValue> Delegate { get; set; }

        /// <summary>
        /// Parametrized command delegate
        /// </summary>
        public ParametrizedCommandDelegate<TValue, TParams> ParametrizedDelegate { get; set; }

        /// <summary>
        /// Command parameters
        /// </summary>
        public TParams Parameters { get; set; }

        /// <summary>
        /// Create an empty command
        /// </summary>
        public ParametrizedCommand() { }

        /// <summary>
        /// Create command with delegate only (parameters can be set later)
        /// </summary>
        /// <param name="parametrizedDelegate">Parametrized command delegate</param>
        public ParametrizedCommand(ParametrizedCommandDelegate<TValue, TParams> parametrizedDelegate)
        {
            ParametrizedDelegate = parametrizedDelegate;
        }

        /// <summary>
        /// Create command with delegate and parameters
        /// </summary>
        /// <param name="parametrizedDelegate">Parametrized command delegate</param>
        /// <param name="parameters">Command parameters</param>
        public ParametrizedCommand(ParametrizedCommandDelegate<TValue, TParams> parametrizedDelegate, TParams parameters)
        {
            ParametrizedDelegate = parametrizedDelegate;
            Parameters = parameters;
        }

        /// <inheritdoc/>
        public void Execute(ref TValue value)
        {
            if (ParametrizedDelegate != null && Parameters != null)
            {
                ParametrizedDelegate(ref value, Parameters);
            }
        }
    }

    /// <summary>
    /// Short alias for ParametrizedCommand
    /// </summary>
    /// <typeparam name="TValue">Type of value to process</typeparam>
    /// <typeparam name="TParams">Type of command parameters</typeparam>
    public class PCommand<TValue, TParams> : ParametrizedCommand<TValue, TParams>
    {
        /// <summary>
        /// Create an empty command
        /// </summary>
        public PCommand() { }

        /// <summary>
        /// Create command with delegate only (parameters can be set later)
        /// </summary>
        /// <param name="parametrizedDelegate">Parametrized command delegate</param>
        public PCommand(ParametrizedCommandDelegate<TValue, TParams> parametrizedDelegate) : base(parametrizedDelegate) { }

        /// <summary>
        /// Create command with delegate and parameters
        /// </summary>
        /// <param name="parametrizedDelegate">Parametrized command delegate</param>
        /// <param name="parameters">Command parameters</param>
        public PCommand(ParametrizedCommandDelegate<TValue, TParams> parametrizedDelegate, TParams parameters) : base(parametrizedDelegate, parameters) { }
    }
}