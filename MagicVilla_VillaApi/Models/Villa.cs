using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_VillaApi.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //create dbcontext in data folder.
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }

        public string ImageUrl { get; set; }
        public int Sqft { get; set; }
        public double Rate { get; set; }

        public int Occupancy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }


    }
}
