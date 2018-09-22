using System.Diagnostics;
using System.IO;

namespace StartSurvivalcraft2Cracked
{
    class StartSurvivalcraft2Cracked
    {
        public static void Main(string[] args)
        {
            string dllFileName = "xiaofengdizhuDLL.dll";
            string projectOutputFolder = "K:\\Microsoft Visual Studio\\Projects\\xiaofengdizhuDLL\\xiaofengdizhuDLL\\bin\\Debug\\";
            string gameFolder = "L:\\SC-with-JS-2.1.23\\SurvivalcraftAppX\\";
            File.Copy(projectOutputFolder + dllFileName, gameFolder + dllFileName, true);
            Process.Start(@"Survivalcraft 2 Cracked.lnk");
        }
    }
}
