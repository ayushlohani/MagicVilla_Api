using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.logging;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/villaApi")] //use controller name as route we write like [Route("api/[controller]")]
    [ApiController] //help us to control basic props like required fields or other thing
    public class MagicVillaController : ControllerBase
    {
        //Dependcy Injection
        private readonly ILogging _logger;

        private readonly ApplicationDbContext _db;  //now replacing all villstore to db
        public MagicVillaController(ILogging logger, ApplicationDbContext db)
        {
            _logger = logger;
            Console.WriteLine("Hello");
            _db = db;
        }


        //Logging


        //private readonly ILogger<MagicVillaController> _logger;
        //public MagicVillaController(ILogger<MagicVillaController> logger) 
        //{
        //    _logger = logger;
        //}


        //using 3rd party for logging  :- serilog.AspNetCore(package) and serilog.SinksFile
        //writ in program.cs

        //Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().Writeto.File("log/villalogs.txt",rollingInterval : RollingInetrval.Day).CreateLogger();
        //builder.Host.UseSerilog();


        //Get 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("Get all villas", "");

            //_logger.LogInformation("Get All Villas");
            return Ok(_db.Villas);
        }

        //for id search
        [HttpGet("{id:int}", Name = "SearchVilla")]  //dont give space btw id:int it cause error

        //to remove undocumented under status code
        //oneway
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]

        //other way more explainatory
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<VillaDTO> SearchVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log($"Get Error Villa With\" + {id}", "error");

                //_logger.LogError("Get Error Villa With" + id);
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        ///************************************************************************************************************************************////

        //Post
        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<VillaDTO> createVilla([FromBody] VillaDTO newVilla)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}


            //custom validation(villa name should unique)

            if (_db.Villas.FirstOrDefault(x => x.Name.ToLower() == newVilla.Name.ToLower()) != null)

            {
                ModelState.AddModelError("CustomError", "Villa Already Exist");
                return BadRequest(ModelState);
            }


            if (newVilla == null)
            {
                return BadRequest(newVilla);
            }

            if (newVilla == null)
            {
                return BadRequest(newVilla);
            }


            if (newVilla.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


            Villa model = new()
            {
                Id = newVilla.Id,
                Name = newVilla.Name,
                Details = newVilla.Details,
                Occupancy = newVilla.Occupancy,
                Sqft = newVilla.Sqft,
                Rate = newVilla.Rate,
                ImageUrl = newVilla.ImageUrl,
            };

            _db.Villas.Add(model);
            _db.SaveChanges(); //save changes whenever needed

            //return Ok(villaDTO);
            return CreatedAtRoute("SearchVilla", new { id = newVilla.Id }, newVilla);

        }

        ///***********************************************************************************************************////
        //Delete
        [HttpDelete("{id:int}", Name = "DeleteVilla")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();



            return NoContent();
        }


        ///***********************************************************************************************************////

        //Update In this we have to change whole for just one model thats why we use httppatch
        [HttpPut("{id:int}", Name = "UpdateVilla")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO newVilla)
        {
            if (newVilla == null || id != newVilla.Id) { return BadRequest(); }
            
            Villa model = new()
            {
                Id = newVilla.Id,
                Name = newVilla.Name,
                Details = newVilla.Details,
                Occupancy = newVilla.Occupancy,
                Sqft = newVilla.Sqft,
                Rate = newVilla.Rate,
                ImageUrl = newVilla.ImageUrl,
            };


            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();

        }

        //Patch(Update)  use jsonpatch.com
        //*we have to download right cliack on main folder select mangae nuget packages and download two packages
        //microsoft.aspnetcore.JsonPatch
        //Microsoft.AspNetCore.Mvc.NewtonsoftJson
        //add this packages  in program.cs where  builder.services.AddControllers() write in place builder.services.AddControllers().AddNewtonsoftJson()

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (id == 0 || patchDTO == null) { return BadRequest(); }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);

            VillaDTO villamodel = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                Occupancy = villa.Occupancy,
                Sqft = villa.Sqft,
                Rate = villa.Rate,
                ImageUrl = villa.ImageUrl,
            };
            if (villa == null) { return BadRequest(); }
            patchDTO.ApplyTo(villamodel, ModelState);
            //convert villadto to villa again for update in db
            Villa model = new()
            {
                Id = villamodel.Id,
                Name = villamodel.Name,
                Details = villamodel.Details,
                Occupancy = villamodel.Occupancy,
                Sqft = villamodel.Sqft,
                Rate = villamodel.Rate,
                ImageUrl = villamodel.ImageUrl,
            };

            _db.Villas.Update(model);
            _db.SaveChanges();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}

