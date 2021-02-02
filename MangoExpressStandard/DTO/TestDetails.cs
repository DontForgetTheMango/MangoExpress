using Newtonsoft.Json;
using System;

namespace MangoExpressStandard.DTO
{
    /// <inheritdoc/>
    [Serializable]
    public class TestDetails : ITestDetails
    {
        /// <inheritdoc/>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty(Required = Required.Default)]
        public bool Execute { get; set; } = true;

        /// <summary>
        /// Will failing test fail, warn, or ignore
        /// </summary>
        public enum TestFailureEnum
        {
            /// <summary>
            /// Fail (default)
            /// </summary>
            Fail,
            /// <summary>
            /// Warn if test fails
            /// </summary>
            Warn, 
            /// <summary>
            /// Ignore if test fails
            /// </summary>
            Ignore
        }

        /// <inheritdoc/>
        [JsonProperty(Required = Required.Default)]
        public TestFailureEnum UponFailure { get; set; } = TestFailureEnum.Fail;

        /// <inheritdoc/>
        [JsonProperty(Required = Required.Default)]
        public string Description { get; set; }
    }
}
