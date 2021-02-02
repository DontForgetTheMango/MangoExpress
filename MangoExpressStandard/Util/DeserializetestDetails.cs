using MangoExpressStandard.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MangoExpressStandard.Util
{
    public class DeserializetestDetails
    {
        public static List<T> GetTestDetails<T>(string scenarioPath, string scenarioName = null) where T : ITestDetails
        {
            var attr = File.GetAttributes(scenarioPath);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var retList = new List<T>();

                // get llist of json files
                var files = Directory.GetFiles(scenarioPath, "*.json");
                foreach(var file in files)
                {
                    var scenario = GetTestDetails<T>(file, scenarioName);
                    retList.AddRange(scenario);
                }

                return retList;
            }
            else
            {
                return GetTestDetails<T>(scenarioPath, scenarioName);
            }
        }
    }
}
