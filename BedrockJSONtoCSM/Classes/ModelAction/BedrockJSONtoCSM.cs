using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BedrockJSONtoCSM.Classes.FileTypes;
namespace ModelAction
{
    public class BedrockJSONtoCSM
    {
        public string JSONtoCSM(string JsonString)
        {
            dynamic jsonDe = JsonConvert.DeserializeObject<dynamic>(JsonString);
            string NewJSON = JsonConvert.SerializeObject(jsonDe["minecraft:geometry"]);
            JObject[] NewJObject = JsonConvert.DeserializeObject<JObject[]>(NewJSON);

            string CSMData = "";
            foreach (JBone bone in NewJObject[0].bones)
            {
                int i = 0;
                string PARENT = bone.name;
                foreach (JCube Cube in bone.cubes)
                {
                    string name = PARENT + " " + i;

                    float PosXModifier = 0;
                    float PosYModifier = 0;
                    float PosZModifier = 0;

                    switch (PARENT)
                    {
                        case "ARM0":
                            PosXModifier = 5;
                            PosYModifier = 22;
                            break;
                        case "ARM1":
                            PosXModifier = -5;
                            PosYModifier = 22;
                            break;
                        case "LEG0":
                            PosXModifier = 2f;
                            PosYModifier = 12;
                            break;
                        case "LEG1":
                            PosXModifier = -2f;
                            PosYModifier = 12;
                            break;
                        case "BODY":
                            PosYModifier = 24;
                            break;
                        case "HEAD":
                            PosYModifier = 24;
                            break;
                    }


                    float PosX = Cube.origin[0] + PosXModifier;
                    float PosY = Cube.origin[1] + PosYModifier;
                    float PosZ = Cube.origin[2] + PosZModifier;
                    float SizeX = Cube.size[0];
                    float SizeY = Cube.size[1];
                    float SizeZ = Cube.size[2];
                    float UvX = Cube.uv[0];
                    float UvY = Cube.uv[1];

                    CSMData += name + "\n" + PARENT + "\n" + name + "\n" + PosX + "\n" + PosY + "\n" + PosZ + "\n" + SizeX + "\n" + SizeY + "\n" + SizeZ + "\n" + UvX + "\n" + UvY + "\n";
                    i++;
                }
            }
            return CSMData;
        }


        public CSMBFile JSONtoCSMB(string JsonString)
        {
            dynamic jsonDe = JsonConvert.DeserializeObject<dynamic>(JsonString);
            string NewJSON = JsonConvert.SerializeObject(jsonDe["minecraft:geometry"]);
            JObject[] NewJObject = JsonConvert.DeserializeObject<JObject[]>(NewJSON);
            CSMBFile file = new CSMBFile();

            foreach (JBone bone in NewJObject[0].bones)
            {
                int i = 0;
                string PARENT = bone.name;
                foreach (JCube Cube in bone.cubes)
                {
                    CSMBPart part = new CSMBPart();
                    string name = PARENT + " " + i;

                    float PosXModifier = 0;
                    float PosYModifier = 0;
                    float PosZModifier = 0;

                    switch (PARENT)
                    {
                        case "ARM0":
                            PosXModifier = 5;
                            PosYModifier = 22;
                            part.Parent = ParentPart.ARM0;
                            break;
                        case "ARM1":
                            PosXModifier = -5;
                            PosYModifier = 22;
                            part.Parent = ParentPart.ARM1;
                            break;
                        case "LEG0":
                            PosXModifier = 2f;
                            PosYModifier = 12;
                            part.Parent = ParentPart.LEG0;
                            break;
                        case "LEG1":
                            PosXModifier = -2f;
                            PosYModifier = 12;
                            part.Parent = ParentPart.LEG1;
                            break;
                        case "BODY":
                            PosYModifier = 24;
                            part.Parent = ParentPart.BODY;
                            break;
                        case "HEAD":
                            PosYModifier = 24;
                            part.Parent = ParentPart.HEAD;
                            break;
                    }


                    float PosX = Cube.origin[0] + PosXModifier;
                    float PosY = Cube.origin[1] + PosYModifier;
                    float PosZ = Cube.origin[2] + PosZModifier;
                    float SizeX = Cube.size[0];
                    float SizeY = Cube.size[1];
                    float SizeZ = Cube.size[2];
                    float UvX = Cube.uv[0];
                    float UvY = Cube.uv[1];
                    float Inflate = Cube.inflate;

                    part.Name = name;
                    part.posX = PosX;
                    part.posY = PosY;
                    part.posZ = PosZ;
                    part.sizeX = SizeX;
                    part.sizeY = SizeY;
                    part.sizeZ = SizeZ;
                    part.uvX = (int)UvX;
                    part.uvY = (int)UvY;
                    part.Inflation = Inflate;
                    file.Parts.Add(part);
                    //CSMData += name + "\n" + PARENT + "\n" + name + "\n" + PosX + "\n" + PosY + "\n" + PosZ + "\n" + SizeX + "\n" + SizeY + "\n" + SizeZ + "\n" + UvX + "\n" + UvY + "\n";
                    i++;
                }
            }
            return file;
        }
    }

    internal class WholeJSON
    {
        public string format_version = "1.12.0";
        public Dictionary<string, object> entries = new Dictionary<string, object>();
    }

    internal class JObject
    {
        public Dictionary<string, object> description = new Dictionary<string, object>();
        public JBone[] bones = { };
    }
    internal class JBone
    {
        public string name = "";
        public float[] pivot = {0, 0, 0};
        public JCube[] cubes = { };
    }
    internal class JCube
    {
        public float[] origin = new float[3];
        public float[] size = new float [3];
        public float[] uv = new float[2];
        public float inflate = 0.0f;
    }
}
