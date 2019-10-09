using ModelHelper.Core.Project;
using DotLiquid;
using ModelHelper.Core.Project.V1;

namespace ModelHelper.Core.Drops
{
    public class ApiDrop : Drop
    {
        public ApiDrop(ProjectApiModelV1 api)
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