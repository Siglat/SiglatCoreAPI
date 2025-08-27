using System.ComponentModel.DataAnnotations;

namespace Craftmatrix.org.Model
{
    public class FireIncidentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [StringLength(100)]
        public string IncidentType { get; set; } // Structure Fire, Vehicle Fire, Forest Fire, etc.
        
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        
        [Required]
        public string Location { get; set; }
        
        public string? Coordinates { get; set; } // Latitude, Longitude
        
        [Required]
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ResponseTime { get; set; }
        
        [StringLength(50)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical
        
        [StringLength(50)]
        public string Status { get; set; } = "Reported"; // Reported, Responding, OnScene, Controlled, Extinguished
        
        public int EstimatedDamage { get; set; } // In PHP
        
        public int InjuredCount { get; set; } = 0;
        
        public int FatalityCount { get; set; } = 0;
        
        [StringLength(500)]
        public string? CauseOfFire { get; set; }
        
        [StringLength(1000)]
        public string? ActionsTaken { get; set; }
        
        public string? AssignedUnits { get; set; } // JSON array of unit IDs
        
        public Guid ReportedBy { get; set; }
        
        public string? ReporterName { get; set; }
        
        public string? ReporterContact { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class FireUnitDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string UnitName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string UnitType { get; set; } // Fire Truck, Ladder Truck, Rescue Vehicle, Water Tanker
        
        [Required]
        [StringLength(100)]
        public string StationLocation { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Available"; // Available, Responding, OnScene, Maintenance, OutOfService
        
        public int PersonnelCount { get; set; }
        
        public string? CurrentLocation { get; set; }
        
        public Guid? AssignedIncidentId { get; set; }
        
        public DateTime? LastMaintenance { get; set; }
        
        public DateTime? NextMaintenance { get; set; }
        
        public string? Equipment { get; set; } // JSON array of equipment
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}