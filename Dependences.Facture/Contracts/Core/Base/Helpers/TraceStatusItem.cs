using System;

namespace Facture.Core.Helpers
{
    public class TraceStatusItem : ITraceStatusItem
    {
        // WARNING: 'public' in order to make them serializable
        public DateTimeOffset Started { get; set; }
        // WARNING: 'public' in order to make them serializable
        public DateTimeOffset Ended { get; set; }

        private TraceStatusItem() { }

        public TraceStatusItem(string name)
        {
            this.Name = name;
            Started = DateTime.Now;
        }

        public void Stop(TraceStatusItem previousStep, DateTime firstStarted)
        {
            this.Ended = DateTimeOffset.Now;
            this.Elapsed = Math.Round((this.Ended - Started).TotalSeconds, 3);

            var priorStepTime = previousStep == null ? firstStarted : previousStep.Ended;
            this.DelayFromPriorStep = Math.Round((Started - priorStepTime).TotalSeconds, 3);

            this.ElapsedAccumulated = Math.Round(this.Elapsed + this.DelayFromPriorStep + (previousStep?.ElapsedAccumulated ?? 0), 3);
        }

        public string Name { get; set; }
        public double DelayFromPriorStep { get; set; }
        public double Elapsed { get; set; }
        public double ElapsedAccumulated { get; set; }
    }
}
