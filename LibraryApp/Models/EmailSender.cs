using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    /// <summary>
    /// class for creating and sending messages 
    /// </summary>
    public class EmailSender
    {
        [Required, Display(Name="LibraryApp")]
        public string FromName { get; set; }
        [Required, Display(Name = "f59fe8fec4-16af91@inbox.mailtrap.io"), EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
