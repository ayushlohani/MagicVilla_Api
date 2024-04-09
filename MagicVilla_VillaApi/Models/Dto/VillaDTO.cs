using System.ComponentModel.DataAnnotations;
namespace MagicVilla_VillaApi.Models.Dto
{
    public class VillaDTO
    {

        //packages req for entity framework  Microsost.EntityFrameworkCore.Sqlserver   Microsost.EntityFrameworkCore.Tools
        //[key]
        public int Id { get; set; }

        //if we wanna make name a required field 
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Details { get; set; }

        public string ImageUrl { get; set; }
        public int Sqft { get; set; }
        public double Rate { get; set; }

        public int occupancy { get; set; }

        //Do not use this because we do not want to expose this

        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdateDate { get; set; }

    }
}
