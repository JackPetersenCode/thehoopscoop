using Python.Runtime;
using System;

namespace ReactApp4.Server.Services
{
    public class PythonCaller : IDisposable
    {
        public PythonCaller()
        {
            // Initialize Python runtime if not already initialized
            if (!PythonEngine.IsInitialized)
            {
                Runtime.PythonDLL = @"C:\Python312\python312.dll"; // Adjust path as needed
                PythonEngine.Initialize();
            }
        }

        public void Dispose()
        {
            // Finalize Python runtime
            PythonEngine.Shutdown();
        }

        public dynamic CallSayHello()
        {
            if (!PythonEngine.IsInitialized)
            {
                throw new InvalidOperationException("Python runtime is not initialized.");
            }

            string scriptDirectory = @"C:\Users\jackp\Desktop\ReactApp4\ReactApp4.Server"; // Adjust as per your environment
            string pythonCode = $"import sys; sys.path.append(r'{scriptDirectory}')";

            try
            {
                using (Py.GIL())
                {
                    PythonEngine.Exec(pythonCode);

                    dynamic exampleScript = Py.Import("testScript");
                    var result = exampleScript.sayHello();
                    return result;
                }
            }
            catch (PythonException ex)
            {
                throw new InvalidOperationException($"An error occurred in Python script: {ex.Message}");
            }
        }
    }
}
