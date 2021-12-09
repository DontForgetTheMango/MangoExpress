using System;
using Newtonsoft.Json;

namespace MangoExpressStandard.DTO
{
    public class ExpectedError
    {
        [JsonProperty(Required = Required.Default)]
        public static bool IsResolved { get; set; } = false;

        [JsonProperty(Required = Required.Default)]
        public static string Message { get; set; }
    }
}
