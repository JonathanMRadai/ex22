using System.ComponentModel.DataAnnotations.Schema;

namespace ex22
{

        public class Contact
        {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string Last { get; set; }
        public DateTime LastDate { get; set; }
      
        }
    
}
