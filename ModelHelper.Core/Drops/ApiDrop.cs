using ModelHelper.Core.Project;
using DotLiquid;

namespace ModelHelper.Core.Drops
{
    public class ApiDrop : Drop
    {
        public ApiDrop(ProjectApiModel api)
        {
            if (api != null)
            {
                UseLogger = api.UseLogger;
                UseTelemetry = api.UseTelemetry;
            }
            else
            {
                UseLogger = false;
                UseTelemetry = false;
            }
        }
        public ApiDrop(bool useLogger, bool useTelemetry)
        {
            UseLogger = useLogger;
            UseTelemetry = useTelemetry;
        }

        public bool UseLogger { get; }

        public bool UseTelemetry { get; }
    }
}