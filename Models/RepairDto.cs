using CarAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarAPI.Models
{
    public class RepairDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime RepairDate { get; set; }
        public decimal RepairCost { get; set; }
        public int EngineHorsepower { get; set; }
        public decimal EngineDisplacement { get; set; }
        public FuelType FuelType { get; set; }
    }
}
