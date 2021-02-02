using static MangoExpressStandard.DTO.TestDetails;

namespace MangoExpressStandard.DTO
{
    /// <summary>
    /// Details of test
    /// </summary>
    public interface ITestDetails
    {
        /// <summary>
        /// Name of test
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Will test be executed?
        /// </summary>
        bool Execute { get; set; }
        
        /// <summary>
        /// If test fails, will it fail, warn, or be ignored
        /// </summary>
        TestFailureEnum UponFailure { get; set; }

        /// <summary>
        /// Description of test
        /// </summary>
        string Description { get; set; }
    }
}
