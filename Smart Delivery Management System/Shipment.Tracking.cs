using System;

public abstract partial class Shipment
{
    // Tracking status property
    public string TrackingStatus { get; set; } = "In Transit";

    // Returns current tracking status
    public string GetTrackingStatus()
    {
        return TrackingStatus;
    }

    // Updates tracking status and triggers the partial method
    public void UpdateTrackingStatus(string newStatus)
    {
        if (!string.IsNullOrWhiteSpace(newStatus))
        {
            TrackingStatus = newStatus;
            OnTrackingStatusChanged(newStatus);
        }
    }

    // Declaration of the partial method
    partial void OnTrackingStatusChanged(string newStatus);

    // Implementation of the partial method
    partial void OnTrackingStatusChanged(string newStatus)
    {
        Console.WriteLine($"Tracking status changed to: {newStatus}");
    }
}