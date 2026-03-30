using System;
using System.Linq;
using System.Collections.Generic;

namespace ProcessedValue
{
    /// <summary>
    /// Processed value container with priority-based command queue
    /// </summary>
    /// <typeparam name="TValue">Type of value to process</typeparam>
    [Serializable]
    public class Processed<TValue> : Processed<int, TValue>
    {
        /// <summary>
        /// Create empty processed value
        /// </summary>
        public Processed() { }

        /// <summary>
        /// Create processed value with initial value
        /// </summary>
        /// <param name="initialValue">Initial base value</param>
        public Processed(TValue initialValue) : base(initialValue) { }
    }

    /// <summary>
    /// Processed value container with priority-based command queue
    /// </summary>
    /// <typeparam name="TSort">Type for priority sorting</typeparam>
    /// <typeparam name="TValue">Type of value to process</typeparam>
    [Serializable]
    public class Processed<TSort, TValue> where TSort : IComparable<TSort>
    {
        /// <summary>
        /// Indicates whether the value needs recalculation
        /// </summary>
        public bool IsDirty;

        #if UNITY_EDITOR
        [UnityEngine.SerializeReference]
        #endif
        /// <summary>
        /// Base value before processing
        /// </summary>
        public TValue BaseValue;

        #if UNITY_EDITOR
        [UnityEngine.SerializeReference]
        #endif
        protected TValue _value;

        /// <summary>
        /// Current processed value (recalculated lazily when dirty)
        /// </summary>
        public TValue Value
        {
            get
            {
                if (IsDirty) ProcessValue();
                return _value;
            }
        }

        /// <summary>
        /// Command queue organized by priority
        /// </summary>
        public SortedList<TSort, List<ICommand<TValue>>> Commands = new SortedList<TSort, List<ICommand<TValue>>>();

        /// <summary>
        /// Create empty processed value
        /// </summary>
        public Processed() { }

        /// <summary>
        /// Create processed value with initial value
        /// </summary>
        /// <param name="initialValue">Initial base value</param>
        public Processed(TValue initialValue) =>
            BaseValue = _value = initialValue;

        /// <summary>
        /// Add a command with specified priority
        /// </summary>
        /// <param name="command">Command to add</param>
        /// <param name="priority">Command priority</param>
        public virtual void AddCommand(ICommand<TValue> command, TSort priority)
        {
            if (Commands.ContainsKey(priority))
                Commands[priority].Add(command);
            else
                Commands.Add(priority, new List<ICommand<TValue>>() { command });

            IsDirty = true;
        }

        /// <summary>
        /// Remove a command from specified priority
        /// </summary>
        /// <param name="command">Command to remove</param>
        /// <param name="priority">Priority to remove from</param>
        public virtual void RemoveCommand(ICommand<TValue> command, TSort priority)
        {
            if (Commands[priority].FirstOrDefault(x => x == command) == null)
            {
                Console.Error.WriteLine("Can't find command to remove.");
                #if UNITY_EDITOR
                UnityEngine.Debug.LogWarning("Can't find command to remove.");
                #endif
                return;
            }
            else
                Commands[priority].Remove(command);

            IsDirty = true;
        }

        /// <summary>
        /// Remove a command from all priorities
        /// </summary>
        /// <param name="command">Command to remove</param>
        public virtual void RemoveCommandAtAll(ICommand<TValue> command)
        {
            foreach (var commandList in Commands.Values)
                commandList.RemoveAll(x => x == command);

            IsDirty = true;
        }

        /// <summary>
        /// Check if command exists in any priority
        /// </summary>
        /// <param name="command">Command to check</param>
        /// <returns>True if command exists</returns>
        public virtual bool ContainsCommand(ICommand<TValue> command) =>
            Commands.Any(x => x.Value.Contains(command) == true);

        /// <summary>
        /// Force value recalculation
        /// </summary>
        public virtual void ProcessValue()
        {
            IsDirty = false;

            _value = BaseValue;
            foreach (var priorityList in Commands)
                foreach (var command in priorityList.Value)
                    command.Execute(ref _value);
        }
    }
}