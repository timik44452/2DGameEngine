using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Shader : Asset
{
    public string code;
    public string path;
    public Shader(string path)
    {
        this.path = path;

        code = System.IO.File.ReadAllText(path);
    }
}