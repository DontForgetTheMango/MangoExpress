using System;
using Newtonsoft.Json;

namespace MangoExpressStandard.DTO
{
    public class ExpectedError
    {
        [JsonProperty(Required = Required.Default)]
        public bool IsResolved { get; set; } = false;

        [JsonProperty(Required = Required.Default)]
        public string Message { get; set; }
    }
}
