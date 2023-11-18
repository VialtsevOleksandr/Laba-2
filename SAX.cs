using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Laba_2.MainPage;

namespace Laba_2;

public class SAX : ISearch
   {
public List<Dormitory> Search(string xmlFilePath, Dictionary<string, string> filterDormitory)
    {
        var dormitories = new List<Dormitory>();
        var dormitory = new Dormitory();
        string currentElement = null;
        bool inAcademicDetails = false;
        bool inPersonalInformation = false;

        using (var reader = XmlReader.Create(xmlFilePath))
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    currentElement = reader.Name;
                    if (currentElement == "AcademicDetails")
                    {
                        inAcademicDetails = true;
                    }
                    if (currentElement == "PersonalInformation")
                    {
                        inPersonalInformation = true;
                    }

                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    if (inAcademicDetails)
                    {
                        switch (currentElement)
                        {
                            case "Faculty":
                                dormitory.Faculty = reader.Value;
                                break;
                            case "Department":
                                dormitory.Department = reader.Value;
                                break;
                            case "Course":
                                dormitory.Course = int.Parse(reader.Value);
                                break;
                        }
                    }
                    else if (inPersonalInformation)
                    {
                        switch (currentElement)
                        {
                            case "FullName":
                                dormitory.FullName = reader.Value;
                                break;
                            case "DormitoryNumber":
                                dormitory.DormitoryNumber = int.Parse(reader.Value);
                                break;
                            case "Floor":
                                dormitory.Floor = int.Parse(reader.Value);
                                break;
                            case "RoomNumber":
                                dormitory.RoomNumber = reader.Value;
                                break;
                            case "ContractEndDate":
                                dormitory.ContractEndDate = DateTime.Parse(reader.Value);
                                break;
                            case "IsResidingInDormitory":
                                dormitory.IsResidingInDormitory = reader.Value;
                                break;
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "AcademicDetails")
                    {
                        inAcademicDetails = false;
                    }
                    if (reader.Name == "PersonalInformation")
                    {
                        inPersonalInformation = false;
                    }
                    else if (reader.Name == "dormitory")
                    {
                        bool match = filterDormitory.All(filter =>
                        {
                            int filterInt;
                            bool isInt = int.TryParse(filter.Value, out filterInt);
                            if (!string.IsNullOrEmpty(filter.Value) && isInt == true)
                            {
                                if (filter.Key == "Course")
                                {
                                    return dormitory.Course == filterInt;
                                }
                                else if (filter.Key == "DormitoryNumber")
                                {
                                    return dormitory.DormitoryNumber == filterInt;
                                }
                                else if (filter.Key == "Floor")
                                {
                                    return dormitory.Floor == filterInt;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (filter.Key == "Faculty")
                                {
                                    return dormitory.Faculty == filter.Value;
                                }
                                if (filter.Key == "Department")
                                {
                                    return dormitory.Department == filter.Value;
                                }
                                if (filter.Key == "FullName")
                                {
                                    return dormitory.FullName.Contains(filter.Value);
                                }
                                if (filter.Key == "RoomNumber")
                                {
                                    return dormitory.RoomNumber == filter.Value;
                                }
                                if (filter.Key == "ContractEndDate")
                                {
                                    return dormitory.ContractEndDate.ToString("yyyy-MM-dd") == filter.Value;
                                }
                                if (filter.Key == "IsResidingInDormitory")
                                {
                                    return dormitory.IsResidingInDormitory == filter.Value;
                                }
                                return false;
                            }
                        });

                        if (match)
                        {
                            dormitories.Add(dormitory);
                        }

                        dormitory = new Dormitory();
                    }
                }
            }
        }

        return dormitories;
    }
}