using System;

namespace BookingService.Model.Entity;

public class OutBoxMasaages
{
    public Guid OutBoxMasaageId {get; set;}
    public string EventType {get; set;} = string.Empty;

    public string Payload {get;set;} = string.Empty;

    public DateTime CreatedAt {get;set;} = DateTime.Now;

    public DateTime? ProcessedAt {get;set;} 

    public string? Error{get ; set;}

}
