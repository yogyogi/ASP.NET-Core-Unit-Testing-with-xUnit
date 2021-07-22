using Microsoft.AspNetCore.Mvc;
using MyAppT.Infrastructure;
using MyAppT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAppT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private IRepository repository;
        public ReservationController(IRepository repo) => repository = repo;

        [HttpGet]
        public IEnumerable<Reservation> Get() => repository.Reservations;

        [HttpGet("{id}")]
        public ActionResult<Reservation> Get(int id)
        {
            if (id == 0)
                return BadRequest("Value must be passed in the request body.");

            Reservation r = repository[id];

            if (r is null)
                return NotFound();

            return Ok(r);
        }

        [HttpPost]
        public Reservation Post([FromBody] Reservation res) =>
        repository.AddReservation(new Reservation
        {
            Name = res.Name,
            StartLocation = res.StartLocation,
            EndLocation = res.EndLocation
        });

        [HttpPut]
        public Reservation Put([FromBody] Reservation res) => repository.UpdateReservation(res);

        [HttpDelete("{id}")]
        public void Delete(int id) => repository.DeleteReservation(id);
    }
}
