using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class AdminLog
    {
        [Key]
        public long AdminLogId { get; set; }

        public string? GiverId { get; set; }
        public IdentityUser? Giver { get; set; }
        public string? ReceiverId { get; set; }
        public IdentityUser? Receiver { get; set; }

        public string? Message { get; set; }
        public DateTimeOffset Date { get; set; }

        public AdminLog(string? giverId, string? receiverId)
        {
            GiverId = giverId;
            ReceiverId = receiverId;

            Date = DateTimeOffset.Now;
        }

        public AdminLog(string? giverId, string? receiverId, string? message)
        {
            GiverId = giverId;
            ReceiverId = receiverId;
            Message = message;

            Date = DateTimeOffset.Now;
        }
    }
}
