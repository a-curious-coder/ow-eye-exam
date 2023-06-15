namespace CanYouC_.Models
{
    public class Raw : Schedule
    {
        /// <summary>
        /// Type of the Entry
        /// </summary>
        public string EntryType { get; set; } = string.Empty;

        /// <summary>
        /// Details of the Entry
        /// </summary>
        public List<string> EntryText { get; set; } = new List<string>();
    }
}
