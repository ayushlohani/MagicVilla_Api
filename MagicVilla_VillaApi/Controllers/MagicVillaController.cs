﻿using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Data;
namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/villaApi")] //use controller name as route we write like [Route("api/[controller]")]
    [ApiController] //help us to control basic props like required fields or other thing
    public class MagicVillaController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        //for id search
        [HttpGet("id",Name = "SearchVilla")]

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



        //Post
        [HttpPost]
        public ActionResult<VillaDTO> createVilla([FromBody] VillaDTO villaDTO)
        {
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
    }
}