using MangoExpressStandard.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MangoExpressStandard.Util
{
    public static class DeserializeTestDetails
    {
        public static List<T> GetTestDetails<T>(string scenarioPath, string scenarioName = null) where T : ITestDetails
        {
            var attr = File.GetAttributes(scenarioPath);

            // detect if file or directory
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
                return GetTestDetailsFromFile<T>(scenarioPath, scenarioName);
            }
        }

        private static List<T> GetTestDetailsFromFile<T>(string path, string name = null) where T : ITestDetails
        {
            if (!File.Exists(path))
                return new List<T>();

            try
            {
                using (var file = File.OpenText(path))
                {
                    var serializer = new JsonSerializer();
                    List<T> td = (List<T>)serializer.Deserialize(file, typeof(List<T>));

                    var isExecuteOnly = AppSettings.TestDetailsExecuteOnly;

                    var listOfTestDetails = new List<T>();

                    if (isExecuteOnly)
                    {
                        listOfTestDetails = td.Where(item => item.Execute).ToList();
                    }
                    else
                    {
                        listOfTestDetails = td;
                    }

                    if (name == null)
                        return listOfTestDetails;

                    return listOfTestDetails.Where(item => item.Name == name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Newtonsoft unable to decerialize json file:{path}", ex);
            }
        }
    }
}
