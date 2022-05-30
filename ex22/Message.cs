using System.ComponentModel.DataAnnotations.Schema;

namespace ex22
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public bool Sent { get; set; }
        public int ChatId{ get; set; }
    }
}
