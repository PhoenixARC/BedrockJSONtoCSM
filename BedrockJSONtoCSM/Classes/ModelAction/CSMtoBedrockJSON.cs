using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BedrockJSONtoCSM.Classes.FileTypes;

namespace ModelAction
{
    public class CSMtoBedrockJSON
    {
        public string CSMtoJSON(string CSMString)
        {
            List<string> CSMLIST = new List<string>();
            CSMLIST.AddRange(CSMString.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            List<List<string>> CSMS = SplitToSublists(CSMLIST, 11);

            JObject jobj = new JObject();
            List<JBone> bones = new List<JBone>();
            Dictionary<string, List<JCube>> CubeList = new Dictionary<string, List<JCube>>
            {
                {"HEAD", new List<JCube>()},
                {"BODY", new List<JCube>()},
                {"ARM0", new List<JCube>()},
                {"ARM1", new List<JCube>()},
                {"LEG0", new List<JCube>()},
                {"LEG1", new List<JCube>()}
            };

            foreach (List<string> CSMItem in CSMS)
            {
                JCube NewCube = new JCube();


                float PosXModifier = 0;
                float PosYModifier = 0;
                float PosZModifier = 0;

                switch (CSMItem[1])
                {
                    case "ARM0":
                        PosXModifier = -5;
                        PosYModifier = -22;
                        break;
                    case "ARM1":
                        PosXModifier = 5;
                        PosYModifier = -22;
                        break;
                    case "LEG0":
                        PosXModifier = -2f;
                        PosYModifier = -12;
                        break;
                    case "LEG1":
                        PosXModifier = 2f;
                        PosYModifier = -12;
                        break;
                    case "BODY":
                        PosYModifier = -24;
                        break;
                    case "HEAD":
                        PosYModifier = -24;
                        break;
                }
                NewCube.origin[0] = float.Parse(CSMItem[3]) + PosXModifier;
                NewCube.origin[1] = float.Parse(CSMItem[4]) + PosYModifier;
                NewCube.origin[2] = float.Parse(CSMItem[5]) + PosZModifier;
                NewCube.size[0] = float.Parse(CSMItem[6]);
                NewCube.size[1] = float.Parse(CSMItem[7]);
                NewCube.size[2] = float.Parse(CSMItem[8]);
                NewCube.uv[0] = float.Parse(CSMItem[9]);
                NewCube.uv[1] = float.Parse(CSMItem[10]);
                CubeList[CSMItem[1]].Add(NewCube);
            }
            foreach (KeyValuePair<string, List<JCube>> bone in CubeList)
            {
                JBone jb = new JBone();
                jb.name = bone.Key;
                jb.cubes = bone.Value.ToArray();
                bones.Add(jb);
            }
            jobj.bones = bones.ToArray();
            jobj.description.Add("identifier", "geometry.steve");
            jobj.description.Add("texture_width", 64);
            jobj.description.Add("texture_height", 64);
            jobj.description.Add("visible_bounds_width", 2);
            jobj.description.Add("visible_bounds_height", 3.5f);
            jobj.description.Add("visible_bounds_offset", new float[] { 0, 1.25f, 0 });
            WholeJSON WJ = new WholeJSON();
            WJ.entries.Add("format_version", "1.12.0");
            WJ.entries.Add("minecraft:geometry", new JObject[] { jobj });
            string JSONDATA = JsonConvert.SerializeObject(WJ.entries, Formatting.Indented);
            return JSONDATA;
        }


        public string CSMBtoJSON(CSMBFile CSMB)
        {

            JObject jobj = new JObject();
            List<JBone> bones = new List<JBone>();
            Dictionary<string, List<JCube>> CubeList = new Dictionary<string, List<JCube>>
            {
                {"HEAD", new List<JCube>()},
                {"BODY", new List<JCube>()},
                {"ARM0", new List<JCube>()},
                {"ARM1", new List<JCube>()},
                {"LEG0", new List<JCube>()},
                {"LEG1", new List<JCube>()}
            };

            foreach (CSMBPart CSMItem in CSMB.Parts)
            {
                JCube NewCube = new JCube();


                float PosXModifier = 0;
                float PosYModifier = 0;
                float PosZModifier = 0;

                switch (CSMItem.Parent)
                {
                    case ParentPart.ARM0:
                        PosXModifier = -5;
                        PosYModifier = -22;
                        break;
                    case ParentPart.ARM1:
                        PosXModifier = 5;
                        PosYModifier = -22;
                        break;
                    case ParentPart.LEG0:
                        PosXModifier = -2f;
                        PosYModifier = -12;
                        break;
                    case ParentPart.LEG1:
                        PosXModifier = 2f;
                        PosYModifier = -12;
                        break;
                    case ParentPart.BODY:
                        PosYModifier = -24;
                        break;
                    case ParentPart.HEAD:
                        PosYModifier = -24;
                        break;
                }
                NewCube.origin[0] = CSMItem.posX + PosXModifier;
                NewCube.origin[1] = CSMItem.posY + PosYModifier;
                NewCube.origin[2] = CSMItem.posZ + PosZModifier;
                NewCube.size[0] = CSMItem.sizeX;
                NewCube.size[1] = CSMItem.sizeY;
                NewCube.size[2] = CSMItem.sizeZ;
                NewCube.uv[0] = CSMItem.uvX;
                NewCube.uv[1] = CSMItem.uvY;
                NewCube.inflate = CSMItem.Inflation;
                CubeList[CSMItem.Name].Add(NewCube);
            }
            foreach (KeyValuePair<string, List<JCube>> bone in CubeList)
            {
                JBone jb = new JBone();
                jb.name = bone.Key;
                jb.cubes = bone.Value.ToArray();
                bones.Add(jb);
            }
            jobj.bones = bones.ToArray();
            jobj.description.Add("identifier", "geometry.steve");
            jobj.description.Add("texture_width", 64);
            jobj.description.Add("texture_height", 64);
            jobj.description.Add("visible_bounds_width", 2);
            jobj.description.Add("visible_bounds_height", 3.5f);
            jobj.description.Add("visible_bounds_offset", new float[] { 0, 1.25f, 0 });
            WholeJSON WJ = new WholeJSON();
            WJ.entries.Add("format_version", "1.12.0");
            WJ.entries.Add("minecraft:geometry", new JObject[] { jobj });
            string JSONDATA = JsonConvert.SerializeObject(WJ.entries, Formatting.Indented);
            return JSONDATA;
        }

        public List<List<string>> SplitToSublists(List<string> source, int size)
        {
            return source
                     .Select((x, i) => new { Index = i, Value = x })
                     .GroupBy(x => x.Index / size)
                     .Select(x => x.Select(v => v.Value).ToList())
                     .ToList();
        }
    }

}
