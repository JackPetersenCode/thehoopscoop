using Python.Runtime;
using System;

namespace ReactApp4.Server
{
    public class PythonCaller
    {
        public dynamic CallSayHello()
        {
            Runtime.PythonDLL = @"C:\Python312\python312.dll";
            PythonEngine.Initialize();

            string scriptDirectory = @"C:\Users\jackp\Desktop\ReactApp4\ReactApp4.Server";
            string pythonCode = $"import sys; sys.path.append(r'{scriptDirectory}')";

            PythonEngine.Exec(pythonCode);

            dynamic exampleScript = Py.Import("testScript");

            var result = exampleScript.sayHello();
            return result;
        }
    }
}
