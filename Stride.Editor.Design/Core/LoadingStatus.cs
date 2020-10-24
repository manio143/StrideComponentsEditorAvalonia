namespace Stride.Editor.Design.Core
{
    /// <summary>
    /// Describes a progress bar
    /// </summary>
    public class LoadingStatus
    {
        public enum LoadingMode
        {
            /// <summary>
            /// It's not known how much time will an action take.
            /// </summary>
            Indeterminate,

            /// <summary>
            /// We can approximate the action's duration by updating the status.
            /// </summary>
            Percentage,
        }

        public LoadingStatus(LoadingMode mode)
        {
            Mode = mode;
        }

        public LoadingMode Mode { get; set; }

        /// <summary>
        /// A number from 0 to 100 (rounded percentage).
        /// </summary>
        public int PercentCompleted { get; set; }
    }
}