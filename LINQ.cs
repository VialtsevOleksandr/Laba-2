using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;
using static Laba_2.MainPage;

namespace Laba_2;

public class LINQ: ISearch
{
    public List<Dormitory> Search(string xmlFilePath, Dictionary<string, string> filterDormitory)
    {
        XDocument doc = XDocument.Load(xmlFilePath);
        var dormitories = doc.Descendants("dormitory")
            .Where(d => filterDormitory.All(filter =>
            {
                int filterInt;
                bool isInt = Int32.TryParse(filter.Value, out filterInt);
                if (!string.IsNullOrEmpty(filter.Value) && isInt == true)
                {
                    if (filter.Key == "Course")
                    {
                        return int.Parse(d.Element("PersonalInformation").Element("AcademicDetails").Element(filter.Key).Value) == filterInt;
                    }
                   else if (filter.Key == "DormitoryNumber" || filter.Key == "Floor")
                    {
                        return int.Parse(d.Element("PersonalInformation").Element(filter.Key).Value) == filterInt;
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
                        return d.Element("PersonalInformation").Element("AcademicDetails").Element(filter.Key).Value == filter.Value;
                    }
                    if (filter.Key == "FullName")
                    {
                        return d.Element("PersonalInformation").Value.Contains(filter.Value);
                    }
                    return d.Element("PersonalInformation").Element(filter.Key).Value == filter.Value;
                }
            }))
            .Select(d => new Dormitory
            {
                FullName = d.Element("PersonalInformation").Element("FullName").Value,
                DormitoryNumber = int.Parse(d.Element("PersonalInformation").Element("DormitoryNumber").Value),
                Floor = int.Parse(d.Element("PersonalInformation").Element("Floor").Value),
                RoomNumber = d.Element("PersonalInformation").Element("RoomNumber").Value,
                ContractEndDate = DateTime.Parse(d.Element("PersonalInformation").Element("ContractEndDate").Value),
                IsResidingInDormitory = d.Element("PersonalInformation").Element("IsResidingInDormitory").Value,
                Faculty = d.Element("PersonalInformation").Element("AcademicDetails").Element("Faculty").Value,
                Department = d.Element("PersonalInformation").Element("AcademicDetails").Element("Department").Value,
                Course = int.Parse(d.Element("PersonalInformation").Element("AcademicDetails").Element("Course").Value)
            }).ToList();

        return dormitories;
    }
}