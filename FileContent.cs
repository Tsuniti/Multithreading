using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading;

public class FileContent
{
    public string Name { get; set; }

    public string CheckSum { get; set; }

    public string ShortName => Name.Split(Path.DirectorySeparatorChar).Last();
}
