using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading.Reports;

public class FileRemoveReport : IReport
{
    public string FileName { get; set; }

    public string CheckSum { get; set; }

}
