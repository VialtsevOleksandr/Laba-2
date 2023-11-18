using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Laba_2.MainPage;

namespace Laba_2;
public interface ISearch
{
    List<Dormitory> Search(string xmlFilePath, Dictionary<string, string> filterDormitory);
}
