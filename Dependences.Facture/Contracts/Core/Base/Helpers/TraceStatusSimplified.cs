using System;
using System.Collections.Generic;
using System.Globalization;

namespace Facture.Core.Helpers
{
    public class TraceStatusSimplified
    {
        public const string SEP = " | "; // separator

        public IList<string> Steps { get; set; }
        public double ElapsedTotal { get; set; }
        public string OriginalStages { get; set; }

        public IList<string> LogsAsyncSteps { get; set; }

        public IList<ITraceStatusItem> GetSourceSteps()
        {
            List<ITraceStatusItem> sourceSteps = new List<ITraceStatusItem>();

            // item 0 has headers
            if (Steps?.Count > 1)
            {
                for (int i = 1; i < Steps.Count; i++)
                {
                    var item = Steps[i];
                    var itemArray = item.Split(new string[] { TraceStatusSimplified.SEP }, System.StringSplitOptions.None);
                    var step = new TraceStatusItem(itemArray[1])
                    {
                        Elapsed = Convert.ToDouble(itemArray[0], CultureInfo.InvariantCulture),
                        DelayFromPriorStep = Convert.ToDouble(itemArray[2], CultureInfo.InvariantCulture),
                        ElapsedAccumulated = Convert.ToDouble(itemArray[3], CultureInfo.InvariantCulture),
                    };
                    sourceSteps.Add(step);
                }
            }

            return sourceSteps;
        }

        public TraceStatusSimplified() { }

        public TraceStatusSimplified(int filterMinElapsedMsec, IList<ITraceStatusItem> sourceSteps, double elapsedTotalSeconds = 0, string originalStages = null, List<string> logsAsyncSteps = null)
        {
            this.ElapsedTotal = elapsedTotalSeconds;
            this.Steps = new List<string>();
            this.OriginalStages = originalStages;
            this.LogsAsyncSteps = logsAsyncSteps;

            var index = 0;
            foreach (var item in sourceSteps)
            {
                if (filterMinElapsedMsec > 0)
                {
                    if (item.Elapsed * 1000 < filterMinElapsedMsec) { continue; }
                }

                string line;
                if (index == 0) // add header
                {
                    line = string.Format("{1}{0}{2}{0}{3}{0}{4}", SEP,
                                        nameof(item.Elapsed),
                                        nameof(item.Name),
                                        nameof(item.DelayFromPriorStep),
                                        nameof(item.ElapsedAccumulated));
                    this.Steps.Add(line);
                }
                index++;

                // add body
                line = string.Format("{1}{0}{2}{0}{3}{0}{4}", SEP,
                                    item.Elapsed.ToString("0.000", CultureInfo.InvariantCulture),
                                    item.Name,
                                    item.DelayFromPriorStep.ToString(CultureInfo.InvariantCulture),
                                    item.ElapsedAccumulated.ToString(CultureInfo.InvariantCulture));
                this.Steps.Add(line);
            }                        
        }
    }
}
