using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading.Reports;

public class FileUpdateReport : IReport
{
    public string FileName {  get; set; }
   
    public string OldCheckSum { get; set; }

    public string NewCheckSum { get; set; }

}
