using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Laba_2.MainPage;

namespace Laba_2;

public class DOM : ISearch
{
    public List<Dormitory> Search(string xmlFilePath, Dictionary<string, string> filterDormitory)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFilePath);

        var dormitories = new List<Dormitory>();
        var nodes = doc.SelectNodes("//dormitory");

        foreach (XmlNode node in nodes)
        {
            var dormitory = new Dormitory
            {
                FullName = node.SelectSingleNode("PersonalInformation/FullName").InnerText,
                DormitoryNumber = int.Parse(node.SelectSingleNode("PersonalInformation/DormitoryNumber").InnerText),
                Floor = int.Parse(node.SelectSingleNode("PersonalInformation/Floor").InnerText),
                RoomNumber = node.SelectSingleNode("PersonalInformation/RoomNumber").InnerText,
                ContractEndDate = DateTime.Parse(node.SelectSingleNode("PersonalInformation/ContractEndDate").InnerText),
                IsResidingInDormitory = node.SelectSingleNode("PersonalInformation/IsResidingInDormitory").InnerText,
                Faculty = node.SelectSingleNode("PersonalInformation/AcademicDetails/Faculty").InnerText,
                Department = node.SelectSingleNode("PersonalInformation/AcademicDetails/Department").InnerText,
                Course = int.Parse(node.SelectSingleNode("PersonalInformation/AcademicDetails/Course").InnerText)
            };

            bool match = filterDormitory.All(filter =>
            {
                int filterInt;
                bool isInt = int.TryParse(filter.Value, out filterInt);
                if (!string.IsNullOrEmpty(filter.Value) && isInt)
                {
                    if (filter.Key == "Course")
                    {
                        return dormitory.Course == filterInt;
                    }
                    else if (filter.Key == "DormitoryNumber" || filter.Key == "Floor")
                    {
                        return int.Parse(node.SelectSingleNode($"PersonalInformation/{filter.Key}").InnerText) == filterInt;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (filter.Key == "Faculty" || filter.Key == "Department")
                    {
                        return node.SelectSingleNode($"PersonalInformation/AcademicDetails/{filter.Key}").InnerText == filter.Value;
                    }
                    if (filter.Key == "FullName")
                    {
                        return node.SelectSingleNode("PersonalInformation").InnerText.Contains(filter.Value);
                    }
                    return node.SelectSingleNode($"PersonalInformation/{filter.Key}").InnerText == filter.Value;
                }
            });

            if (match)
            {
                dormitories.Add(dormitory);
            }
        }

        return dormitories;
    }
}