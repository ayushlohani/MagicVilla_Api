using System.ComponentModel.DataAnnotations;
namespace MagicVilla_VillaApi.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }

        //if we wanna make name a required field 
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }=DateTime.Now;

        public int Occupancy {  get; set; }

        public int Sqft { get; set; }

    }
}
