namespace Backend.Formas.Entities.Services
{
    /// <summary>
    /// Users Info
    /// </summary>
    public class UsersInfo
    {
        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>
        /// The name of the member.
        /// </value>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the member email.
        /// </summary>
        /// <value>
        /// The member email.
        /// </value>
        public string MemberEmail { get; set; }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        public string ObjectId { get; set; }
    }
}