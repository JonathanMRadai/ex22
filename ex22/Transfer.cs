using System.ComponentModel.DataAnnotations.Schema;

namespace ex22
{
    public class Transfer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

    }
}
