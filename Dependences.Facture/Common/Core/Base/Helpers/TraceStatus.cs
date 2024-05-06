using Facture.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Web;

namespace Facture.Core.Helpers
{
    public class TraceStatus
    {
        #region Properties
        [JsonIgnore]
        public bool IsLoggingDisabled { get; private set; }
        public List<TraceStatusItem> Steps { get; private set; }
        public double ElapsedTotal { get; private set; }
        #endregion


        #region Fields
        private TraceStatusItem _PreviousItem;
        private TraceStatusItem _CurrentItem;
        private readonly DateTime _FirstStarted;
        private int _MessageHandlerNestedLevel;
        private Dictionary<string, int> _MessageHandlerNestedLevels;
        #endregion


        #region Constructors
        public TraceStatus() : base()
        {
            _FirstStarted = DateTime.Now;
            this.Steps = new List<TraceStatusItem>();
            _MessageHandlerNestedLevels = new Dictionary<string, int>();
        }

        public TraceStatus(HttpRequestMessage request) : this()
        {
            IsLoggingDisabled = !(request.Headers.Contains("X-TRACE") && request.Headers.GetValues("X-TRACE").FirstOrDefault() == "True");
        }


        public TraceStatus(string firstStepName) : this()
        {
            StepBegin(firstStepName);
            //IsLoggingDisabled = HttpContext.Current?.Request?.Headers["X-TRACE"] != "True";
        }
        #endregion


        #region Step Begin/End
        public void StepBegin(string name)
        {
            if (IsLoggingDisabled) { return; }

            if (_CurrentItem != null) { StepEnd(); }
            _CurrentItem = new TraceStatusItem(name);
        }

        public void StepEnd()
        {
            if (IsLoggingDisabled) { return; }
            if (_CurrentItem == null) { throw new NotImplementedException(); }

            _CurrentItem.Stop(_PreviousItem, _FirstStarted);
            this.Steps.Add(_CurrentItem);
            _PreviousItem = _CurrentItem; _CurrentItem = null;
            ElapsedTotal = Math.Round((DateTime.Now - _FirstStarted).TotalSeconds, 3);
        }
        #endregion


        public void Merge(TraceStatusItem[] stageStatuses, string beforeFirst)
        {
            if (stageStatuses == null || stageStatuses.Length == 0) { return; }

            var pos = -1;
            for (int i = 1 /*second item*/; i < Steps.Count; i++)
            {
                var item = Steps[i];
                if (item.Name.EndsWith(beforeFirst))
                {
                    // get index of previous position
                    pos = i;
                    break;
                }
            }
            if (pos == -1)
            {
                pos = Steps.Count - 1;
#if DEBUG
                throw new NotSupportedException("Unexpected scenario. Please review!");
#endif
            }

            Steps.InsertRange(pos, stageStatuses);

            // TODO: recalculate elapsed time after merging
            for (int i = 1 /*second item*/; i < Steps.Count; i++)
            {
                var previousStep = Steps[i - 1];
                var item = Steps[i];
                item.DelayFromPriorStep = Math.Round((item.Started - previousStep.Ended).TotalSeconds, 3);
                item.ElapsedAccumulated = Math.Round(item.Elapsed + previousStep.DelayFromPriorStep + previousStep.ElapsedAccumulated, 3);
            }
        }

        #region MessageHandler Nested Level
        public void MessageHandlerNestedLevelSet(string className)
        {
            _MessageHandlerNestedLevels.Add(className, _MessageHandlerNestedLevel);
            _MessageHandlerNestedLevel++;
        }

        public int MessageHandlerNestedLevelGet(string className)
        {
            return _MessageHandlerNestedLevels[className];
        }
        #endregion


        #region Static
        
        #endregion

        public TraceStatusSimplified ToSimple(int filterMinElapsedMsec)
            => new TraceStatusSimplified(filterMinElapsedMsec, this.Steps.ToList<ITraceStatusItem>(), this.ElapsedTotal);
    }
}
