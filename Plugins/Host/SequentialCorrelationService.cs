using System;
using Abstractions;

namespace Plugins.Host.AspNetCore
{
    public class SequentialCorrelationService : ICorrelationService
    {
        public string GenerateIdIfNull(string correlationGuid) {
            if (!string.IsNullOrWhiteSpace(correlationGuid)) {
                return correlationGuid;
            }
            
            return SequentialGuid.SequentialGuidGenerator.Instance.NewGuid().ToString();
        }
    }
}