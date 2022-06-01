using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ex22
{
    public class Invitation
    {
        [Key]
        public string From { get; set; }    
        public string To { get; set; }  
        public string Server { get; set; }

    }
}
