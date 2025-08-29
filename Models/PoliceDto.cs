using System.ComponentModel.DataAnnotations;

namespace Craftmatrix.org.Model
{
    public class PoliceIncidentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [StringLength(100)]
        public string IncidentType { get; set; } // Traffic Accident, Robbery, Assault, Drug Operation, etc.
        
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
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Emergency
        
        [StringLength(50)]
        public string Status { get; set; } = "Reported"; // Reported, Responding, OnScene, Investigating, Resolved, Closed
        
        [StringLength(50)]
        public string Category { get; set; } // Criminal, Traffic, Civil, Administrative
        
        public int SuspectCount { get; set; } = 0;
        
        public int VictimCount { get; set; } = 0;
        
        public int WitnessCount { get; set; } = 0;
        
        [StringLength(1000)]
        public string? ActionsTaken { get; set; }
        
        [StringLength(1000)]
        public string? EvidenceCollected { get; set; }
        
        public string? AssignedOfficers { get; set; } // JSON array of officer IDs
        
        public string? AssignedUnits { get; set; } // JSON array of unit IDs
        
        public Guid ReportedBy { get; set; }
        
        public string? ReporterName { get; set; }
        
        public string? ReporterContact { get; set; }
        
        public string? CaseNumber { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PoliceUnitDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string UnitName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string UnitType { get; set; } // Patrol Car, Motorcycle, SWAT, K-9, Traffic, Mobile Command
        
        [Required]
        [StringLength(100)]
        public string StationLocation { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Available"; // Available, Responding, OnScene, Patrol, Break, Maintenance
        
        public int OfficerCount { get; set; }
        
        public string? CurrentLocation { get; set; }
        
        public Guid? AssignedIncidentId { get; set; }
        
        public string? CallSign { get; set; }
        
        public DateTime? ShiftStart { get; set; }
        
        public DateTime? ShiftEnd { get; set; }
        
        public string? AssignedOfficers { get; set; } // JSON array of officer names/IDs
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class WarrantDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [StringLength(100)]
        public string SuspectName { get; set; }
        
        [StringLength(500)]
        public string? SuspectDescription { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Charges { get; set; }
        
        [Required]
        public DateTime IssueDate { get; set; }
        
        [StringLength(100)]
        public string IssuingCourt { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Served, Recalled, Expired
        
        public string? LastKnownLocation { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public DateTime? ExpiryDate { get; set; }
        
        public DateTime? ServedDate { get; set; }
        
        public string? ServingOfficer { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class EvidenceDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid IncidentId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string EvidenceType { get; set; } // Physical, Digital, Documentary, Testimonial
        
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        
        [StringLength(200)]
        public string? Location { get; set; } // Where evidence was found
        
        public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        [StringLength(100)]
        public string CollectedBy { get; set; } // Officer name/ID
        
        [StringLength(50)]
        public string Status { get; set; } = "Collected"; // Collected, Processed, Analyzed, Stored
        
        public string? ChainOfCustody { get; set; } // JSON array of custody transfers
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}