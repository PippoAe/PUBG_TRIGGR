using System;
using numl;
using numl.Model;
using System.IO;

namespace PubgTriggr
{
    public class ScreenData
    {
        [Feature]
        public int Red { get; set; }
        [Feature]
        public int White { get; set; }
        [Label]
        public bool Kill { get; set; }

        public static ScreenData[] GetDataFromFile(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);
            ScreenData[] gData = new ScreenData[lines.Length];
            //for (int i = 0; i < lines.Length; i++)
            //{
            //    string[] sVals = lines[i].Split(';');
            //    int[] iVals = { Convert.ToInt32(sVals[0]), Convert.ToInt32(sVals[1]), Convert.ToInt32(sVals[2]) };
            //    gData[i] = new ScreenData { Red = iVals[0], White = iVals[1], Kill = iVals[2] };
            //}
            return gData;
        }
    }
}
