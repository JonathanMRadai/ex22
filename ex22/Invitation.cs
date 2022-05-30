using System.ComponentModel.DataAnnotations.Schema;

namespace ex22
{
    public class Invitation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } 
        public string from { get; set; }    
        public string to { get; set; }  
        public string server { get; set; }

    }
}
