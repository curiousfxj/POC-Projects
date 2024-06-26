using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace test1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonData = @"
                    [
                        { ""ParentId"": 1, ""childParentId"": null },
                        { ""ParentId"": 2, ""childParentId"": 1 },
                        { ""ParentId"": 3, ""childParentId"": 1 },
                        { ""ParentId"": 4, ""childParentId"": null },
                        { ""ParentId"": 5, ""childParentId"": 4 },
                        { ""ParentId"": 6, ""childParentId"": 5 },
                        { ""ParentId"": 7, ""childParentId"": 5 }
                    ]";

            List<Record> records = JsonConvert.DeserializeObject<List<Record>>(jsonData);

            // Filter master records having null childParentId
            List<Record> masterRecords = records.Where(r => r.childParentId == null).ToList();

            foreach (var masterRecord in masterRecords)
            {
                Console.WriteLine("Master Record: " + masterRecord.ParentId);
                List<Record> subList = new List<Record>();
                GetSubList(records, masterRecord.ParentId, subList);
                string subListJson = JsonConvert.SerializeObject(subList, Newtonsoft.Json.Formatting.Indented);
                Console.WriteLine(subListJson);
                Console.WriteLine("-----------------------");
            }
            Console.ReadLine();
        }
        public class Record
        {
            public int ParentId { get; set; }
            public int? childParentId { get; set; }
        }
        public static void GetSubList(List<Record> records, int parentId, List<Record> subList)
        {
            var children = records.Where(r => r.ParentId == parentId || r.childParentId == parentId).ToList();
            foreach (var child in children)
            {
                if (!subList.Contains(child))
                {
                    subList.Add(child);
                    GetSubList(records, child.ParentId, subList);
                }
            }
        }

    }
}
