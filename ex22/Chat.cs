using System.ComponentModel.DataAnnotations.Schema;

namespace ex22
{
    public class Chat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public Contact Contact { get; set; }
        
    }
}
