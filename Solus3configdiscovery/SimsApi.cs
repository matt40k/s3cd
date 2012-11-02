using System;
using System.Reflection;

namespace Solus3configdiscovery
{
    /// <summary>
    /// This class uses the Capita Business Objects to collect the schools
    /// Name, DFE number (include LA) and postcode from the SIMS database
    /// 
    /// It has been excluded from the public repository due to copyright.
    /// </summary>
    public class SimsApi
    {
        private string dfeCode;
        private string schName;
        private string postCode;
        private string simsDir;

        public SimsApi(string simspath)
        {
            simsDir = simspath;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);

            dfeCode = "<UNKNOWN>";
            schName = "<UNKNOWN>";
            postCode = "<UNKNOWN>";
        }

        public void Connect()
        {
            // Removed due to copyright.
        }

        Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly MyAssembly, objExecutingAssemblies;
            string strTempAssmbPath = null;

            objExecutingAssemblies = Assembly.GetExecutingAssembly();
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    //Build the path of the assembly from where it has to be loaded.				
                    strTempAssmbPath = simsDir;
                    if (!strTempAssmbPath.EndsWith("\\")) strTempAssmbPath += "\\";
                    strTempAssmbPath += args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    break;
                }
            }

            //Load the assembly from the specified path. 					
            MyAssembly = Assembly.LoadFrom(strTempAssmbPath);

            //Return the loaded assembly.
            return MyAssembly;
        }

        public string GetDfeCode
        {
            get { return dfeCode; }
        }

        public string GetSchName
        {
            get { return schName; }
        }

        public string GetPostCode
        {
            get { return postCode; }
        }
    }
}
