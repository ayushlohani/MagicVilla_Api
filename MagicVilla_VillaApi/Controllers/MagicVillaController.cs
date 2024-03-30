﻿using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Data;
using Microsoft.AspNetCore.JsonPatch;
namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/villaApi")] //use controller name as route we write like [Route("api/[controller]")]
    [ApiController] //help us to control basic props like required fields or other thing
    public class MagicVillaController : ControllerBase
    {
        //Get 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        //for id search
        [HttpGet("{id:int}",Name = "SearchVilla")]  //dont give space btw id:int it cause error

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
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

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

        public ActionResult<VillaDTO> createVilla([FromBody] VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}


            //custom validation(villa name should unique)
            if(VillaStore.villaList.FirstOrDefault(x=>x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exist");
                return BadRequest(ModelState);
            }

            if (villaDTO == null) { 
            return BadRequest(villaDTO);}

            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDTO.Id = VillaStore.villaList.OrderByDescending(x=>x.Id).FirstOrDefault().Id+1;
            VillaStore.villaList.Add(villaDTO);

            //return Ok(villaDTO);
            return CreatedAtRoute("SearchVilla",new {id=villaDTO.Id},villaDTO);
        }

        ///***********************************************************************************************************////
        //Delete
        [HttpDelete("{id:int}",Name = "DeleteVilla")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult DeleteVilla(int id) {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa=VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }


        ///***********************************************************************************************************////

        //Update In this we have to change whole for just one model thats why we use httppatch
        [HttpPut("{id:int}",Name ="UpdateVilla")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if(villaDTO == null || id!=villaDTO.Id) { return BadRequest(); }
            var villa=VillaStore.villaList.FirstOrDefault(u=>u.Id == id);

            villa.Name= villaDTO.Name;
            villa.Sqft=villaDTO.Sqft;
            villa.Occupancy=villaDTO.Occupancy;

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
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null) { return BadRequest(); }
            patchDTO.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent(); 
        }
    }
}
