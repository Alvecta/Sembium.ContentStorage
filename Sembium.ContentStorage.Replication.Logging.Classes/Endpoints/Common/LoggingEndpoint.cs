using Sembium.ContentStorage.Replication.Common.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Logging.Endpoints.Common
{
    public abstract class LoggingEndpoint : IEndpoint
    {
        protected IEndpoint Endpoint { get; private set; }
        protected ILogger Logger { get; private set; }

        public LoggingEndpoint(IEndpoint endpoint, ILogger logger)
        {
            Endpoint = endpoint;
            Logger = logger;
        }

        public string ID
        {
            get { return Endpoint.ID; }
        }

        public async Task<IEnumerable<IContentIdentifier>> GetContentIdentifiersAsync(DateTimeOffset afterMoment)
        {
            var startMessage = 
                ((afterMoment == DateTimeOffset.MinValue) ? 
                 $"Start getting all content identifiers from {ID}" :
                 $"Start getting content identifiers after {afterMoment} from {ID}");


            Logger.LogInfo(startMessage);
            try
            { 
                var result = await Endpoint.GetContentIdentifiersAsync(afterMoment);

                var listResult = result.ToList();

                Logger.LogInfo($"End getting {listResult.Count()} content identifiers from {ID}");

                return listResult;
            }
            catch (Exception e)
            {
                Logger.LogFatal($"Error getting content identifiers from {ID}" + Environment.NewLine + e.GetAggregateMessages(), e);
                throw;
            }
        }

        public async Task<string> GetContentsHashAsync(DateTimeOffset beforeMoment)
        {
            Logger.LogInfo($"Start getting contents hash until {beforeMoment} from {ID}", beforeMoment, ID);
            try
            {
                var result = await Endpoint.GetContentsHashAsync(beforeMoment);

                Logger.LogInfo($"End getting contents hash from {ID}: {result}");

                return result;
            }
            catch (Exception e)
            {
                Logger.LogFatal($"Error getting contents hash from {ID}" + Environment.NewLine + e.GetAggregateMessages(), e);
                throw;
            }
        }
    }
}
