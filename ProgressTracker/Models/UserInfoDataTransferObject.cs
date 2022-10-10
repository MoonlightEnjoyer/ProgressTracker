namespace ProgressTracker.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Data transfer object for user information.
    /// </summary>
    public class UserInfoDataTransferObject
    {
        /// <summary>
        /// Gets or sets day.
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets users rank.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets users name.
        /// </summary>
        [JsonPropertyName("User")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets steps.
        /// </summary>
        public int Steps { get; set; }
    }
}
