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
    /// Покупаемая валюта
    /// </summary>
    public class RateTo : Entity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(3)]
        public string To { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public RateFrom RateFrom { get; set; }
    }
}
