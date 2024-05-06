
using System;

namespace Facture.Core.Helpers
{
    public interface ITraceStatusItem
    {
        string Name { get; }
        double DelayFromPriorStep { get; set; }
        double Elapsed { get; }
        double ElapsedAccumulated { get; set; }
        DateTimeOffset Started { get; }
        DateTimeOffset Ended { get; }
    }
}
