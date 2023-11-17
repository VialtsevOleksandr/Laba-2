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
    public List<Dormitory> Search(string xmlFilePath, List<string> filterDormitory)
    {
        XDocument doc = XDocument.Load(xmlFilePath);
        var dormitories = doc.Descendants("dormitory")
            .Where(d => filterDormitory.All(filter =>
            {
                int filterInt;
                bool isInt = Int32.TryParse(filter, out filterInt);
                if (isInt)
                {
                    // Якщо фільтр є цілим числом, порівнюємо його зі значеннями "DormitoryNumber", "Floor" та "Course" як цілими числами
                    //return d.Element("PersonalInformation").Value.Contains(filterInt);
                    return int.Parse(d.Element("PersonalInformation").Element("DormitoryNumber").Value) == filterInt
                        || int.Parse(d.Element("PersonalInformation").Element("Floor").Value) == filterInt
                        || int.Parse(d.Element("PersonalInformation").Element("AcademicDetails").Element("Course").Value) == filterInt;
                }
                else
                {
                    // Якщо фільтр не є цілим числом, порівнюємо його зі значеннями як рядками
                    return d.Element("PersonalInformation").Value.Contains(filter);
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