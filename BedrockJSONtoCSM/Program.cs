using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ModelAction;
using BedrockJSONtoCSM.Classes.FileTypes;
using BedrockJSONtoCSM.Classes.IO.CSMB;

namespace BedrockJSONtoCSM
{
    internal class Program
    {
        static void Main(string[] args)
        {
                CSMtoBedrockJSON btc = new CSMtoBedrockJSON();
            ModelAction.BedrockJSONtoCSM ctb = new ModelAction.BedrockJSONtoCSM();
            
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                string FilePath = args[1];
                string JSONData = "";
                string CSMData = "";
                switch (args[0].ToLower())
                {
                    case ("--bedrock"):
                        JSONData = File.ReadAllText(FilePath);
                        File.WriteAllText(FilePath + ".json", btc.CSMtoJSON(JSONData));
                        break;
                    case ("-b"):
                        JSONData = File.ReadAllText(FilePath);
                        File.WriteAllText(FilePath + ".json", btc.CSMtoJSON(JSONData));
                        break;
                    case ("--legacy"):
                        CSMData = File.ReadAllText(FilePath);
                        File.WriteAllText(FilePath + ".csm", ctb.JSONtoCSM(CSMData));
                        break;
                    case ("-l"):
                        CSMData = File.ReadAllText(FilePath);
                        File.WriteAllText(FilePath + ".csm", ctb.JSONtoCSM(CSMData));
                        break;
                    default:
                        throw new Exception("Unsupported Arguments");
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                string JSONData = "";
                string CSMData = "";
                switch (ex.GetType().ToString())
                {
                    case ("System.IndexOutOfRangeException"):
                        switch (args.Length)
                        {
                            case 1:
                                switch (Path.GetExtension(args[0]).ToLower())
                                {
                                    case ".csm":
                                        JSONData = File.ReadAllText(args[0]);
                                        File.WriteAllText(args[0] + ".json", btc.CSMtoJSON(JSONData));
                                        break;
                                    case ".csmb":
                                        FileStream fs = File.OpenRead(args[0]);
                                        CSMBFile csmb = new CSMBFileReader().Read(fs);
                                        fs.Close();
                                        File.WriteAllText(args[0] + ".json", btc.CSMBtoJSON(csmb));
                                        break;
                                    case ".json":

                                        JSONData = File.ReadAllText(args[0]);
                                        FileStream fs1 = File.OpenWrite(args[0] + ".csmb");
                                        CSMBFile csmb1 = ctb.JSONtoCSMB(JSONData);
                                        new CSMBFileWriter().Write(fs1, csmb1);
                                        fs1.Close();
                                        break;
                                    default:
                                        Console.WriteLine("Program Requires argument as well as filepath!");
                                        break;
                                }
                                break;
                            default:
                                Console.WriteLine("Program does not support an argument count of " + args.Length + "!");
                                break;
                        }
                        break;
                    case ("System.IO.FileNotFoundException"):
                        Console.WriteLine("Program can not locate file: " + args[1]);
                        break;
                    case ("System.Exception"):
                        Console.WriteLine("Unsupported Arguments!");
                        break;
                    default:
                        Console.WriteLine("Unspecified Error!");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(ex.Message);
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Usage: CSMConversionTool.exe <argument> \"<filepath>\"");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine(" -b  --bedrock\t\tConverts CSM file to bedrock JSON");
                Console.WriteLine("");
                Console.WriteLine(" -l  --legacy\t\tConverts bedrock JSON file to CSM");
                Console.WriteLine("");
                Console.WriteLine("Press Enter to exit program...");
                Console.ReadLine();
            }

        }
    }

}
