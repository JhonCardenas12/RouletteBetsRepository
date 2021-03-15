using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RouletteBets.Core;
using RouletteBets.Core.Services;
using RouletteBets.DataBase.Modelo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteBets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteServices rouletteServices;
        private readonly IBetRouletteServices betRouletteServices;
        private readonly IDistributedCache distributedCache;
        private Roulette roulette;
        private int time = 10;
        public RouletteController(IRouletteServices rouletteServices, IBetRouletteServices betRouletteServices, IDistributedCache distributedCache)
        {
            this.rouletteServices = rouletteServices;
            this.distributedCache = distributedCache;
            this.betRouletteServices = betRouletteServices;
            roulette = new Roulette();
        }

        [HttpPost("{nameRoulette}/CreateRoulette")]
        public IActionResult CreateRoulette([FromRoute(Name = "nameRoulette")] string nameRoulette)
        {
            try
            {
                roulette.NameRoulette = nameRoulette;
                roulette.CreationDate = this.rouletteServices.FormatDateIso8601();
                this.rouletteServices.CreateRoulette(roulette);
                this.distributedCache.RemoveAsync("GetRoulette");

                return Ok(new { mensaje = "Se creó correctamente el id ruleta: " + roulette.Id });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{idRoulette}/OpeningRoulette")]
        public IActionResult OpeningRoulette([FromRoute(Name = "idRoulette")] string idRoulette)
        {
            try
            {
                roulette.Id = idRoulette;
                Roulette existRoulette = this.rouletteServices.GetRoulette(roulette.Id);
                if (existRoulette != null)
                {
                    roulette.NameRoulette = existRoulette.NameRoulette;
                    roulette.CreationDate = existRoulette.CreationDate;
                    roulette.OpenRoulette = true;
                    this.rouletteServices.UpdateRoulette(roulette);
                    this.distributedCache.RemoveAsync("GetRoulette");

                    return Ok(new { mensaje = "Se ha realizado correctamente la apertura de la ruleta!" });
                }
                else
                {
                    return Ok(new { mensaje = "El id de la ruleta que ingreso no existe: " + idRoulette });
                }
            }
            catch
            {
                return Ok(new { mensaje = "El id de la ruleta que ingreso no existe: " + idRoulette });
            }
        }

        [HttpPost("{idRoulette}/BetRoulette")]
        public IActionResult BetRoulette([FromHeader(Name = "userId")] string userId, [FromHeader(Name = "userName")] string userName, [FromRoute(Name = "idRoulette")] string idRoulette, [FromBody] BetRequest request)
        {
            try
            {
                Roulette roulette = this.rouletteServices.GetRoulette(idRoulette);
                BetRoulette betRoulette = this.betRouletteServices.BetRoulette(roulette, userId, userName, request.position, request.money);
                return Ok(betRoulette);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{idRoulette}/ClosingRoulette")]
        public IActionResult ClosingRoulette([FromRoute(Name = "idRoulette")] string idRoulette)
        {
            try
            {
                roulette.Id = idRoulette;
                Roulette existRoulette = this.rouletteServices.GetRoulette(roulette.Id);
                if (existRoulette != null)
                {
                    Random random = new Random();
                    int numberWinner = random.Next(37); // creates a number between 0 and 36,for 37 Black and 38 Red.
                    this.betRouletteServices.SetWinnerRoulette(roulette.Id, numberWinner);
                    roulette.ClosingDate = this.rouletteServices.FormatDateIso8601();
                    roulette.NameRoulette = existRoulette.NameRoulette;
                    roulette.CreationDate = existRoulette.CreationDate;
                    roulette.OpenRoulette = false;
                    this.rouletteServices.UpdateRoulette(roulette);
                    this.distributedCache.RemoveAsync("GetRoulette");
                    return Ok(this.betRouletteServices.GetBetRoulette(roulette.Id));
                }
                else
                {
                    return Ok(new { mensaje = "El id de la ruleta no existe: " + idRoulette });
                }
            }
            catch
            {
                return Ok(new { mensaje = "El id de la ruleta no existe: " + idRoulette });
            }
        }

        [HttpGet("GetRoulette")]
        public async Task<IActionResult> GetRoulette()
        {
            string cacheData = await this.distributedCache.GetStringAsync("GetRoulette");
            try
            {
                if (string.IsNullOrEmpty(cacheData))
                {
                    DistributedCacheEntryOptions cacheOptions = this.rouletteServices.DistributedCacheEntry(time);
                    List<Roulette> listRoulette = this.rouletteServices.GetRoulette();
                    await this.distributedCache.SetStringAsync("GetRoulette", JsonConvert.SerializeObject(listRoulette), cacheOptions);

                    return Ok(listRoulette);
                }
                else
                {
                    return Ok(JsonConvert.DeserializeObject<IEnumerable<Roulette>>(cacheData));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
