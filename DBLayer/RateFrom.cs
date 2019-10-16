using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLayer.Abstraction;

namespace DBLayer
{
    /// <summary>
    /// Продаваемая валюта
    /// </summary>
    public class RateFrom: AuditableEntity<long>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(3)]
        public string From { get; set; }

        [Required]
        public DateTime CreatingDate { get; set; }

        public ICollection<RateTo> To { get; set; }
    }
}
