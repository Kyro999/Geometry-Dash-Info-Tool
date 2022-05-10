using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GD_Info_Tool
{
    internal class LevelProperties
    {
        public class Root
        {
            public string name { get; set; }
            public string songName { get; set; }
            public string id { get; set; }
            public string description { get; set; }
            public string author { get; set; }
            public int downloads { get; set; }
            public string diamonds { get; set; }
            public int likes { get; set; }
            public bool disliked { get; set; }
            public string length { get; set; }
            public string orbs { get; set; }
            public int version { get; set; }
            public string difficulty { get; set; }
            public int coins { get; set; }
            public bool verifiedCoins { get; set; }
            public int starsRequested { get; set; }
            public int objects { get; set; }
            public string difficultyFace { get; set; }
            public string songLink { get; set; }
        }
    }
}