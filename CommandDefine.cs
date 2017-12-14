using System;
using System.Collections.Generic;
using System.Text;

namespace BlackSpiritTelepathy
{
    class CommandDefine
    {
        public static List<BossData> BossDataTable = new List<BossData>();
        public string GetSysMsg(string msgtag)
        {
            for (int i = 0; i < BossDataTable.Count; i++)
            {
                if (BossDataTable[i].SysMessageTag.Contains(msgtag))
                {
                    
                }

            }
            return msgtag;
        }
    }
    public class BossData
    {
        public int SysMessageKey;
        public string SysMessageTag;
        public string Content;
        public BossData(int key, string tag, string content)
        {
            SysMessageKey = key;
            SysMessageTag = tag;
            Content = content;
        }
    }
}

