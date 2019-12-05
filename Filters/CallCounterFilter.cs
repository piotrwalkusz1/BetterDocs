using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BetterDocs.Filters
{
    public class CallCounterFilter : IActionFilter
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CallCounterFilter> _logger;

        public CallCounterFilter(IDistributedCache cache, ILogger<CallCounterFilter> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var count = int.Parse(_cache.GetString("callCounterFilter/" + context.ActionDescriptor.DisplayName) ?? "0");
            count++;
            
            _logger.Log(LogLevel.Information, "Action " + context.ActionDescriptor.DisplayName + " was called " + count + " times");
            
            _cache.SetString("callCounterFilter/" + context.ActionDescriptor.DisplayName, count.ToString());
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}