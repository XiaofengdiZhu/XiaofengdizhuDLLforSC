using System.Diagnostics;
using System.IO;

namespace StartSurvivalcraft2Cracked
{
    class StartSurvivalcraft2Cracked
    {
        public static void Main(string[] args)
        {
            string dllFileName = "xiaofengdizhuDLL.dll";
            string projectOutputFolder = "E:\\C项目\\xiaofengdizhuDLL\\xiaofengdizhuDLL\\bin\\Debug\\";
            string gameFolder = "F:\\SC-with-JS-2.1.23\\SurvivalcraftAppX\\";
            File.Copy(projectOutputFolder + dllFileName, gameFolder + dllFileName, true);
            Process.Start(@"Survivalcraft 2 Cracked.lnk");
        }
    }
}
