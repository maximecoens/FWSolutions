using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi.Model
{
    public class Nieuwsbericht
    {
        public int? Id { get; set; }
        [Required] 
        public string Titel { get; set; }
        [Required]
        public string Bericht { get; set; }
        [Required]
        public DateTime Datum { get; set; }
    }
}
