using System.Collections.Generic;

namespace Backend.Formas.Entities.Services
{
    /// <summary>
    /// Email Info
    /// </summary>
    public class EmailInfo
    {
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public List<string> To { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets cc
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public List<string> CC { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the styles.
        /// </summary>
        /// <value>
        /// The styles.
        /// </value>
        public string Styles { get; set; } = string.Empty;
    }
}