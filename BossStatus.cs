using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace BlackSpiritTelepathy
{
    class BossStatus
    {
        //ボス情報管理用List "BossChannelMapTable"についての解説コメント
        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        static List<BossChannelMap> BossChannelMapTable = new List<BossChannelMap>();
        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        //
        //このList一つで全てのボスの情報を管理している。
        //Listの配列番号でボス種類とchを指定する。
        //0:クザカ1ch, 1:クザカ2ch, 2:クザカ3ch, 3:クザカ4ch
        //4:カランダ1ch, 5:カランダ2ch, 6:カランダ3ch, 7:カランダ4ch
        //8
        const string PERCENT = "% ";
        const string SPAN = "| ";
        const string IND = "\n";
        const string HIGHLIGHT = "```";
        const string DEAD = "X ";
        //
        //Internal Boss Status Buffer
        //
        static string kz_b1 = "";//= ValueDefine(0, "Balenos");
        static string kz_b2 = "";//= ValueDefine(1, "Balenos");
        static string kz_b3 = "";//= ValueDefine(2, "Balenos");
        static string kz_b4 = "";//= ValueDefine(3, "Balenos");
        static string kz_s1 = "";//= ValueDefine(0, "Serendia");
        static string kz_s2 = "";//= ValueDefine(1, "Serendia");
        static string kz_s3 = "";//= ValueDefine(2, "Serendia");
        static string kz_s4 = "";//= ValueDefine(3, "Serendia");
        static string kz_c1 = "";//= ValueDefine(0, "Calpheon");
        static string kz_c2 = "";//= ValueDefine(1, "Calpheon");
        static string kz_c3 = "";//= ValueDefine(2, "Calpheon");
        static string kz_c4 = "";//= ValueDefine(3, "Calpheon");
        static string kz_m1 = "";//= ValueDefine(0, "Mediah");
        static string kz_m2 = "";//= ValueDefine(1, "Mediah");
        static string kz_m3 = "";//= ValueDefine(2, "Mediah");
        static string kz_m4 = "";//= ValueDefine(3, "Mediah");
        static string kz_v1 = "";//= ValueDefine(0, "Valencia");
        static string kz_v2 = "";//= ValueDefine(1, "Valencia");
        static string kz_v3 = "";//= ValueDefine(2, "Valencia");
        static string kz_v4 = "";//= ValueDefine(3, "Valencia");
        static string kz_ma1 = "";//= ValueDefine(0, "Magoria");
        static string kz_ma2 = "";//= ValueDefine(1, "Magoria");
        static string kz_ma3 = "";//= ValueDefine(2, "Magoria");
        static string kz_ma4 = "";//= ValueDefine(3, "Magoria");
        static string kz_k1 = "";//= ValueDefine(0, "Kamasylvia");
        static string kz_k2 = "";//= ValueDefine(1, "Kamasylvia");
        static string ka_b1, ka_b2, ka_b3, ka_b4, ka_s1, ka_s2, ka_s3, ka_s4, ka_c1, ka_c2, ka_c3, ka_c4, ka_m1, ka_m2, ka_m3, ka_m4, ka_v1, ka_v2, ka_v3, ka_v4, ka_ma1, ka_ma2, ka_ma3, ka_ma4, ka_k1, ka_k2;
        static string ku_b1, ku_b2, ku_b3, ku_b4, ku_s1, ku_s2, ku_s3, ku_s4, ku_c1, ku_c2, ku_c3, ku_c4, ku_m1, ku_m2, ku_m3, ku_m4, ku_v1, ku_v2, ku_v3, ku_v4, ku_ma1, ku_ma2, ku_ma3, ku_ma4, ku_k1, ku_k2;
        static string nv_b1, nv_b2, nv_b3, nv_b4, nv_s1, nv_s2, nv_s3, nv_s4, nv_c1, nv_c2, nv_c3, nv_c4, nv_m1, nv_m2, nv_m3, nv_m4, nv_v1, nv_v2, nv_v3, nv_v4, nv_ma1, nv_ma2, nv_ma3, nv_ma4, nv_k1, nv_k2;
        static string rn_b1, rn_b2, rn_b3, rn_b4, rn_s1, rn_s2, rn_s3, rn_s4, rn_c1, rn_c2, rn_c3, rn_c4, rn_m1, rn_m2, rn_m3, rn_m4, rn_v1, rn_v2, rn_v3, rn_v4, rn_ma1, rn_ma2, rn_ma3, rn_ma4, rn_k1, rn_k2;
        static string bh_b1, bh_b2, bh_b3, bh_b4, bh_s1, bh_s2, bh_s3, bh_s4, bh_c1, bh_c2, bh_c3, bh_c4, bh_m1, bh_m2, bh_m3, bh_m4, bh_v1, bh_v2, bh_v3, bh_v4, bh_ma1, bh_ma2, bh_ma3, bh_ma4, bh_k1, bh_k2;
        static string tree_b1, tree_b2, tree_b3, tree_b4, tree_s1, tree_s2, tree_s3, tree_s4, tree_c1, tree_c2, tree_c3, tree_c4, tree_m1, tree_m2, tree_m3, tree_m4, tree_v1, tree_v2, tree_v3, tree_v4, tree_ma1, tree_ma2, tree_ma3, tree_ma4, tree_k1, tree_k2;
        static string mud_b1, mud_b2, mud_b3, mud_b4, mud_s1, mud_s2, mud_s3, mud_s4, mud_c1, mud_c2, mud_c3, mud_c4, mud_m1, mud_m2, mud_m3, mud_m4, mud_v1, mud_v2, mud_v3, mud_v4, mud_ma1, mud_ma2, mud_ma3, mud_ma4, mud_k1, mud_k2;
        //
        //Boss Time Buffer
        //
        static DateTime kz_spawntime, ka_spawntime, ku_spawntime, nv_spawntime, rn_spawntime, bh_spawntime, tree_spawntime, mud_spawntime, tar_spawntime, iza_spawntime;
        //static TimeSpan BossStatusLimitTime = TimeSpan.FromHours(1);
        static double RefreshRate = 100000;
        static TimeSpan BossStatusLimitTime = TimeSpan.FromSeconds(30);
        static Timer kz_timer, ka_timer, ku_timer, nv_timer, rn_timer, bh_timer, tree_timer, mud_timer, tar_timer, iza_timer;
        static string LatestBossStatus;
        //
        //Boss Status Automatically Clearing Event (It works when elapsed 30min since the last report from players.)
        //

        //
        public static string CreateStatus(int BossID)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime DatetimeUTC = DateTime.UtcNow;
            DateTime jst = TimeZoneInfo.ConvertTimeFromUtc(DatetimeUTC, tzi);
            string BossChannelMapHeader;
            string BossChannelMapStrBalenos;
            string BossChannelMapStrSerendia;
            string BossChannelMapStrCalpheon;
            string BossChannelMapStrMediah;
            string BossChannelMapStrValencia;
            string BossChannelMapStrMagoria;
            string BossChannelMapStrKamasylvia;
            string p_s = PERCENT + SPAN;
            string return_status = "0";
            switch (BossID)
            {
                default:
                    break;
                case 1: //クザカ沸き時のチャンネルマップ生成
                    //ボス体力管理 : BossChannelMap(バレノス,セレンディア,カルフェオン,メディア,バレンシア,マゴリア,カーマス);
                    //チャンネルごとのボス体力値をInsertで挿入。0 = 1ch, 1 = 2ch, 2 = 3ch, 3 = 4ch
                    BossChannelMapTable.Insert(0, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(1, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(2, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(3, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    kz_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(1);
                    BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(kz_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + BossChannelMapTable[0].Balenos.ToString() + p_s + "2ch：" + BossChannelMapTable[1].Balenos.ToString() + p_s + "3ch：" + BossChannelMapTable[2].Balenos.ToString() + p_s + "4ch：" + BossChannelMapTable[3].Balenos.ToString() + p_s;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + BossChannelMapTable[0].Serendia.ToString() + p_s + "2ch：" + BossChannelMapTable[1].Serendia.ToString() + p_s + "3ch：" + BossChannelMapTable[2].Serendia.ToString() + p_s + "4ch：" + BossChannelMapTable[3].Serendia.ToString() + p_s;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + BossChannelMapTable[0].Calpheon.ToString() + p_s + "2ch：" + BossChannelMapTable[1].Calpheon.ToString() + p_s + "3ch：" + BossChannelMapTable[2].Calpheon.ToString() + p_s + "4ch：" + BossChannelMapTable[3].Calpheon.ToString() + p_s;
                    BossChannelMapStrMediah = "Media 1ch：" + BossChannelMapTable[0].Mediah.ToString() + p_s + "2ch：" + BossChannelMapTable[1].Mediah.ToString() + p_s + "3ch：" + BossChannelMapTable[2].Mediah.ToString() + p_s + "4ch：" + BossChannelMapTable[3].Mediah.ToString() + p_s;
                    BossChannelMapStrValencia = "Valencia 1ch：" + BossChannelMapTable[0].Valencia.ToString() + p_s + "2ch：" + BossChannelMapTable[1].Valencia.ToString() + p_s + "3ch：" + BossChannelMapTable[2].Valencia.ToString() + p_s + "4ch：" + BossChannelMapTable[3].Valencia.ToString() + p_s;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + BossChannelMapTable[0].Magoria.ToString() + p_s + "2ch：" + BossChannelMapTable[1].Magoria.ToString() + p_s + "3ch：" + BossChannelMapTable[2].Magoria.ToString() + p_s + "4ch：" + BossChannelMapTable[3].Magoria.ToString() + p_s;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + BossChannelMapTable[0].Kamasylvia.ToString() + p_s + "2ch：" + BossChannelMapTable[1].Kamasylvia.ToString() + p_s;
                    return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                    break;
                case 2: //カランダ
                    BossChannelMapTable.Insert(4, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(5, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(6, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(7, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    ka_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(2);
                    BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(ka_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + BossChannelMapTable[4].Balenos.ToString() + p_s + "2ch：" + BossChannelMapTable[5].Balenos.ToString() + p_s + "3ch：" + BossChannelMapTable[6].Balenos.ToString() + p_s + "4ch：" + BossChannelMapTable[7].Balenos.ToString() + p_s;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + BossChannelMapTable[4].Serendia.ToString() + p_s + "2ch：" + BossChannelMapTable[5].Serendia.ToString() + p_s + "3ch：" + BossChannelMapTable[6].Serendia.ToString() + p_s + "4ch：" + BossChannelMapTable[7].Serendia.ToString() + p_s;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + BossChannelMapTable[4].Calpheon.ToString() + p_s + "2ch：" + BossChannelMapTable[5].Calpheon.ToString() + p_s + "3ch：" + BossChannelMapTable[6].Calpheon.ToString() + p_s + "4ch：" + BossChannelMapTable[7].Calpheon.ToString() + p_s;
                    BossChannelMapStrMediah = "Media 1ch：" + BossChannelMapTable[4].Mediah.ToString() + p_s + "2ch：" + BossChannelMapTable[5].Mediah.ToString() + p_s + "3ch：" + BossChannelMapTable[6].Mediah.ToString() + p_s + "4ch：" + BossChannelMapTable[7].Mediah.ToString() + p_s;
                    BossChannelMapStrValencia = "Valencia 1ch：" + BossChannelMapTable[4].Valencia.ToString() + p_s + "2ch：" + BossChannelMapTable[5].Valencia.ToString() + p_s + "3ch：" + BossChannelMapTable[6].Valencia.ToString() + p_s + "4ch：" + BossChannelMapTable[7].Valencia.ToString() + p_s;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + BossChannelMapTable[4].Magoria.ToString() + p_s + "2ch：" + BossChannelMapTable[5].Magoria.ToString() + p_s + "3ch：" + BossChannelMapTable[6].Magoria.ToString() + p_s + "4ch：" + BossChannelMapTable[7].Magoria.ToString() + p_s;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + BossChannelMapTable[4].Kamasylvia.ToString() + p_s + "2ch：" + BossChannelMapTable[5].Kamasylvia.ToString() + p_s;
                    return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                    break;
                case 3: //ヌーベル
                    BossChannelMapTable.Insert(8, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(9, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(10, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(11, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    nv_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(3);
                    BossChannelMapHeader = "ヌーベル（最終更新　" + jst.ToString("HH時 mm分ss秒")  + " : 沸きから" + CalculateElapsedTime(nv_spawntime).Seconds + "秒経過" +"）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + BossChannelMapTable[8].Balenos.ToString() + p_s + "2ch：" + BossChannelMapTable[9].Balenos.ToString() + p_s + "3ch：" + BossChannelMapTable[10].Balenos.ToString() + p_s + "4ch：" + BossChannelMapTable[11].Balenos.ToString() + p_s;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + BossChannelMapTable[8].Serendia.ToString() + p_s + "2ch：" + BossChannelMapTable[9].Serendia.ToString() + p_s + "3ch：" + BossChannelMapTable[10].Serendia.ToString() + p_s + "4ch：" + BossChannelMapTable[11].Serendia.ToString() + p_s;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + BossChannelMapTable[8].Calpheon.ToString() + p_s + "2ch：" + BossChannelMapTable[9].Calpheon.ToString() + p_s + "3ch：" + BossChannelMapTable[10].Calpheon.ToString() + p_s + "4ch：" + BossChannelMapTable[11].Calpheon.ToString() + p_s;
                    BossChannelMapStrMediah = "Media 1ch：" + BossChannelMapTable[8].Mediah.ToString() + p_s + "2ch：" + BossChannelMapTable[9].Mediah.ToString() + p_s + "3ch：" + BossChannelMapTable[10].Mediah.ToString() + p_s + "4ch：" + BossChannelMapTable[11].Mediah.ToString() + p_s;
                    BossChannelMapStrValencia = "Valencia 1ch：" + BossChannelMapTable[8].Valencia.ToString() + p_s + "2ch：" + BossChannelMapTable[9].Valencia.ToString() + p_s + "3ch：" + BossChannelMapTable[10].Valencia.ToString() + p_s + "4ch：" + BossChannelMapTable[11].Valencia.ToString() + p_s;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + BossChannelMapTable[8].Magoria.ToString() + p_s + "2ch：" + BossChannelMapTable[9].Magoria.ToString() + p_s + "3ch：" + BossChannelMapTable[10].Magoria.ToString() + p_s + "4ch：" + BossChannelMapTable[11].Magoria.ToString() + p_s;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + BossChannelMapTable[8].Kamasylvia.ToString() + p_s + "2ch：" + BossChannelMapTable[9].Kamasylvia.ToString() + p_s;
                    return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                    break;
                case 4: //クツム
                    BossChannelMapTable.Insert(12, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(13, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(14, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(15, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    ku_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(4);
                    BossChannelMapHeader = "クツム（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(ku_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + BossChannelMapTable[12].Balenos.ToString() + p_s + "2ch：" + BossChannelMapTable[13].Balenos.ToString() + p_s + "3ch：" + BossChannelMapTable[14].Balenos.ToString() + p_s + "4ch：" + BossChannelMapTable[15].Balenos.ToString() + p_s;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + BossChannelMapTable[12].Serendia.ToString() + p_s + "2ch：" + BossChannelMapTable[13].Serendia.ToString() + p_s + "3ch：" + BossChannelMapTable[14].Serendia.ToString() + p_s + "4ch：" + BossChannelMapTable[15].Serendia.ToString() + p_s;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + BossChannelMapTable[12].Calpheon.ToString() + p_s + "2ch：" + BossChannelMapTable[13].Calpheon.ToString() + p_s + "3ch：" + BossChannelMapTable[14].Calpheon.ToString() + p_s + "4ch：" + BossChannelMapTable[15].Calpheon.ToString() + p_s;
                    BossChannelMapStrMediah = "Media 1ch：" + BossChannelMapTable[12].Mediah.ToString() + p_s + "2ch：" + BossChannelMapTable[13].Mediah.ToString() + p_s + "3ch：" + BossChannelMapTable[14].Mediah.ToString() + p_s + "4ch：" + BossChannelMapTable[15].Mediah.ToString() + p_s;
                    BossChannelMapStrValencia = "Valencia 1ch：" + BossChannelMapTable[12].Valencia.ToString() + p_s + "2ch：" + BossChannelMapTable[13].Valencia.ToString() + p_s + "3ch：" + BossChannelMapTable[14].Valencia.ToString() + p_s + "4ch：" + BossChannelMapTable[15].Valencia.ToString() + p_s;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + BossChannelMapTable[12].Magoria.ToString() + p_s + "2ch：" + BossChannelMapTable[13].Magoria.ToString() + p_s + "3ch：" + BossChannelMapTable[14].Magoria.ToString() + p_s + "4ch：" + BossChannelMapTable[15].Magoria.ToString() + p_s;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + BossChannelMapTable[12].Kamasylvia.ToString() + p_s + "2ch：" + BossChannelMapTable[13].Kamasylvia.ToString() + p_s;
                    return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                    break;
                    
                case 5: //レッドノーズ
                    BossChannelMapTable.Insert(16, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(17, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(18, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(19, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    rn_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(5);
                    BossChannelMapHeader = "レッドノーズ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(rn_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + BossChannelMapTable[16].Balenos.ToString() + p_s + "2ch：" + BossChannelMapTable[17].Balenos.ToString() + p_s + "3ch：" + BossChannelMapTable[18].Balenos.ToString() + p_s + "4ch：" + BossChannelMapTable[19].Balenos.ToString() + p_s;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + BossChannelMapTable[16].Serendia.ToString() + p_s + "2ch：" + BossChannelMapTable[17].Serendia.ToString() + p_s + "3ch：" + BossChannelMapTable[18].Serendia.ToString() + p_s + "4ch：" + BossChannelMapTable[19].Serendia.ToString() + p_s;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + BossChannelMapTable[16].Calpheon.ToString() + p_s + "2ch：" + BossChannelMapTable[17].Calpheon.ToString() + p_s + "3ch：" + BossChannelMapTable[18].Calpheon.ToString() + p_s + "4ch：" + BossChannelMapTable[19].Calpheon.ToString() + p_s;
                    BossChannelMapStrMediah = "Media 1ch：" + BossChannelMapTable[16].Mediah.ToString() + p_s + "2ch：" + BossChannelMapTable[17].Mediah.ToString() + p_s + "3ch：" + BossChannelMapTable[18].Mediah.ToString() + p_s + "4ch：" + BossChannelMapTable[19].Mediah.ToString() + p_s;
                    BossChannelMapStrValencia = "Valencia 1ch：" + BossChannelMapTable[16].Valencia.ToString() + p_s + "2ch：" + BossChannelMapTable[17].Valencia.ToString() + p_s + "3ch：" + BossChannelMapTable[18].Valencia.ToString() + p_s + "4ch：" + BossChannelMapTable[19].Valencia.ToString() + p_s;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + BossChannelMapTable[16].Magoria.ToString() + p_s + "2ch：" + BossChannelMapTable[17].Magoria.ToString() + p_s + "3ch：" + BossChannelMapTable[18].Magoria.ToString() + p_s + "4ch：" + BossChannelMapTable[19].Magoria.ToString() + p_s;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + BossChannelMapTable[16].Kamasylvia.ToString() + p_s + "2ch：" + BossChannelMapTable[17].Kamasylvia.ToString() + p_s;
                    return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                    break;
                case 6: //ベグ
                    BossChannelMapTable.Insert(20, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(21, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(22, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(23, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    bh_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(6);
                    BossChannelMapHeader = "ベグ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(rn_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + BossChannelMapTable[20].Balenos.ToString() + p_s + "2ch：" + BossChannelMapTable[21].Balenos.ToString() + p_s + "3ch：" + BossChannelMapTable[22].Balenos.ToString() + p_s + "4ch：" + BossChannelMapTable[23].Balenos.ToString() + p_s;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + BossChannelMapTable[20].Serendia.ToString() + p_s + "2ch：" + BossChannelMapTable[21].Serendia.ToString() + p_s + "3ch：" + BossChannelMapTable[22].Serendia.ToString() + p_s + "4ch：" + BossChannelMapTable[23].Serendia.ToString() + p_s;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + BossChannelMapTable[20].Calpheon.ToString() + p_s + "2ch：" + BossChannelMapTable[21].Calpheon.ToString() + p_s + "3ch：" + BossChannelMapTable[22].Calpheon.ToString() + p_s + "4ch：" + BossChannelMapTable[23].Calpheon.ToString() + p_s;
                    BossChannelMapStrMediah = "Media 1ch：" + BossChannelMapTable[20].Mediah.ToString() + p_s + "2ch：" + BossChannelMapTable[21].Mediah.ToString() + p_s + "3ch：" + BossChannelMapTable[22].Mediah.ToString() + p_s + "4ch：" + BossChannelMapTable[23].Mediah.ToString() + p_s;
                    BossChannelMapStrValencia = "Valencia 1ch：" + BossChannelMapTable[20].Valencia.ToString() + p_s + "2ch：" + BossChannelMapTable[21].Valencia.ToString() + p_s + "3ch：" + BossChannelMapTable[22].Valencia.ToString() + p_s + "4ch：" + BossChannelMapTable[23].Valencia.ToString() + p_s;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + BossChannelMapTable[20].Magoria.ToString() + p_s + "2ch：" + BossChannelMapTable[21].Magoria.ToString() + p_s + "3ch：" + BossChannelMapTable[22].Magoria.ToString() + p_s + "4ch：" + BossChannelMapTable[23].Magoria.ToString() + p_s;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + BossChannelMapTable[20].Kamasylvia.ToString() + p_s + "2ch：" + BossChannelMapTable[21].Kamasylvia.ToString() + p_s;
                    return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                    break;
                case 7: //愚鈍
                    BossChannelMapTable.Insert(24, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(25, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(26, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(27, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    tree_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(7);
                    BossChannelMapHeader = "愚鈍な木の精霊（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(rn_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + BossChannelMapTable[24].Balenos.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Balenos.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Balenos.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Balenos.ToString() + p_s;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + BossChannelMapTable[24].Serendia.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Serendia.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Serendia.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Serendia.ToString() + p_s;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + BossChannelMapTable[24].Calpheon.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Calpheon.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Calpheon.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Calpheon.ToString() + p_s;
                    BossChannelMapStrMediah = "Media 1ch：" + BossChannelMapTable[24].Mediah.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Mediah.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Mediah.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Mediah.ToString() + p_s;
                    BossChannelMapStrValencia = "Valencia 1ch：" + BossChannelMapTable[24].Valencia.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Valencia.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Valencia.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Valencia.ToString() + p_s;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + BossChannelMapTable[24].Magoria.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Magoria.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Magoria.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Magoria.ToString() + p_s;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + BossChannelMapTable[24].Kamasylvia.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Kamasylvia.ToString() + p_s;
                    return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                    break;
                case 8: //マッドマン
                    BossChannelMapTable.Insert(28, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(29, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(30, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(31, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    mud_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(8);
                    BossChannelMapHeader = "愚鈍な木の精霊（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(rn_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + BossChannelMapTable[24].Balenos.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Balenos.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Balenos.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Balenos.ToString() + p_s;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + BossChannelMapTable[24].Serendia.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Serendia.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Serendia.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Serendia.ToString() + p_s;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + BossChannelMapTable[24].Calpheon.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Calpheon.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Calpheon.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Calpheon.ToString() + p_s;
                    BossChannelMapStrMediah = "Media 1ch：" + BossChannelMapTable[24].Mediah.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Mediah.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Mediah.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Mediah.ToString() + p_s;
                    BossChannelMapStrValencia = "Valencia 1ch：" + BossChannelMapTable[24].Valencia.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Valencia.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Valencia.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Valencia.ToString() + p_s;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + BossChannelMapTable[24].Magoria.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Magoria.ToString() + p_s + "3ch：" + BossChannelMapTable[26].Magoria.ToString() + p_s + "4ch：" + BossChannelMapTable[27].Magoria.ToString() + p_s;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + BossChannelMapTable[24].Kamasylvia.ToString() + p_s + "2ch：" + BossChannelMapTable[25].Kamasylvia.ToString() + p_s;
                    return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                    break;
                case 9: //タルガルゴ
                    BossChannelMapTable.Insert(32, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(33, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(34, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(35, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    tar_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(9);
                    break;
                case 10: //イザベラ
                    BossChannelMapTable.Insert(36, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(37, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(38, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(39, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    tar_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(10);
                    break;
            }
            return return_status;
        }
        public static string ChangeStatusValue(int BossID, string BossChannel, int BossHP)
        {
            string return_status = "";
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime DatetimeUTC = DateTime.UtcNow;
            DateTime jst = TimeZoneInfo.ConvertTimeFromUtc(DatetimeUTC, tzi);
            
            //
            //現在の各chボス体力値取得
            //
            //Kzarka Status
            //
            int Balenos1chValue = BossChannelMapTable[0].Balenos;
            int Balenos2chValue = BossChannelMapTable[1].Balenos;
            int Balenos3chValue = BossChannelMapTable[2].Balenos;
            int Balenos4chValue = BossChannelMapTable[3].Balenos;
            int Serendia1chValue = BossChannelMapTable[0].Serendia;
            int Serendia2chValue = BossChannelMapTable[1].Serendia;
            int Serendia3chValue = BossChannelMapTable[2].Serendia;
            int Serendia4chValue = BossChannelMapTable[3].Serendia;
            int Calpheon1chValue = BossChannelMapTable[0].Calpheon;
            int Calpheon2chValue = BossChannelMapTable[1].Calpheon;
            int Calpheon3chValue = BossChannelMapTable[2].Calpheon;
            int Calpheon4chValue = BossChannelMapTable[3].Calpheon;
            int Mediah1chValue = BossChannelMapTable[0].Mediah;
            int Mediah2chValue = BossChannelMapTable[1].Mediah;
            int Mediah3chValue = BossChannelMapTable[2].Mediah;
            int Mediah4chValue = BossChannelMapTable[3].Mediah;
            int Valencia1chValue = BossChannelMapTable[0].Valencia;
            int Valencia2chValue = BossChannelMapTable[1].Valencia;
            int Valencia3chValue = BossChannelMapTable[2].Valencia;
            int Valencia4chValue = BossChannelMapTable[3].Valencia;
            int Magoria1chValue = BossChannelMapTable[0].Magoria;
            int Magoria2chValue = BossChannelMapTable[1].Magoria;
            int Magoria3chValue = BossChannelMapTable[2].Magoria;
            int Magoria4chValue = BossChannelMapTable[3].Magoria;
            int Kms1chValue = BossChannelMapTable[0].Kamasylvia;
            int Kms2chValue = BossChannelMapTable[1].Kamasylvia;
            //
            //Karanda Status
            //
            int Ka_Balenos1chValue = BossChannelMapTable[4].Balenos;
            int Ka_Balenos2chValue = BossChannelMapTable[5].Balenos;
            int Ka_Balenos3chValue = BossChannelMapTable[6].Balenos;
            int Ka_Balenos4chValue = BossChannelMapTable[7].Balenos;
            int Ka_Serendia1chValue = BossChannelMapTable[4].Serendia;
            int Ka_Serendia2chValue = BossChannelMapTable[5].Serendia;
            int Ka_Serendia3chValue = BossChannelMapTable[6].Serendia;
            int Ka_Serendia4chValue = BossChannelMapTable[7].Serendia;
            int Ka_Calpheon1chValue = BossChannelMapTable[4].Calpheon;
            int Ka_Calpheon2chValue = BossChannelMapTable[5].Calpheon;
            int Ka_Calpheon3chValue = BossChannelMapTable[6].Calpheon;
            int Ka_Calpheon4chValue = BossChannelMapTable[7].Calpheon;
            int Ka_Mediah1chValue = BossChannelMapTable[4].Mediah;
            int Ka_Mediah2chValue = BossChannelMapTable[5].Mediah;
            int Ka_Mediah3chValue = BossChannelMapTable[6].Mediah;
            int Ka_Mediah4chValue = BossChannelMapTable[7].Mediah;
            int Ka_Valencia1chValue = BossChannelMapTable[4].Valencia;
            int Ka_Valencia2chValue = BossChannelMapTable[5].Valencia;
            int Ka_Valencia3chValue = BossChannelMapTable[6].Valencia;
            int Ka_Valencia4chValue = BossChannelMapTable[7].Valencia;
            int Ka_Magoria1chValue = BossChannelMapTable[4].Magoria;
            int Ka_Magoria2chValue = BossChannelMapTable[5].Magoria;
            int Ka_Magoria3chValue = BossChannelMapTable[6].Magoria;
            int Ka_Magoria4chValue = BossChannelMapTable[7].Magoria;
            int Ka_Kms1chValue = BossChannelMapTable[4].Kamasylvia;
            int Ka_Kms2chValue = BossChannelMapTable[5].Kamasylvia;

            if (Program.DEBUGMODE == true)
            {
                Program.WriteLog("Debug_PassedStats : " + BossID + " " + BossChannel + " " + BossHP);
                //Program.WriteLog("Debug_BossStatusValue: " + kz_b1 + kz_b2 + kz_b3 + kz_b4 + kz_s1 + kz_s2 + kz_s3 + kz_s4 + kz_c1 + kz_c2 + kz_c3 + kz_c4);
            }
            //
            //ボス体力値更新処理開始
            //
            switch (BossID)
            {
                case 1:
                    var kz_elapsedtime = CalculateElapsedTime(kz_spawntime).Hours + "時間" + CalculateElapsedTime(kz_spawntime).Minutes + "分" + CalculateElapsedTime(kz_spawntime).Seconds + "秒";
                    if (Program.DEBUGMODE)
                    {
                        Program.WriteLog(SystemMessageDefine.Kz_ElapsedTime_JP + CalculateElapsedTime(kz_spawntime).Seconds + "秒");
                        
                    }
                    if (BossChannel.Substring(0,1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(BossHP, Serendia1chValue, Calpheon1chValue, Mediah1chValue, Valencia1chValue, Magoria1chValue, Kms1chValue));
                            //ValueCheck(kz_b1, kz_b2, kz_b3, kz_b4, kz_s1, kz_s2, kz_s3, kz_s4, kz_c1, kz_c2, kz_c3, kz_c4, kz_m1, kz_m2, kz_m3, kz_m4, kz_v1, kz_v2, kz_v3, kz_v4, kz_ma1, kz_ma2, kz_ma3, kz_ma4, kz_k1, kz_k2);
                            kz_b1 = ValueConverter(0, "Balenos");
                            string p_s = PERCENT + SPAN;
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + kz_elapsedtime + "経過" + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(BossHP, Serendia2chValue, Calpheon2chValue, Mediah2chValue, Valencia2chValue, Magoria2chValue, Kms2chValue));
                            kz_b2 = ValueConverter(1, "Balenos");
                            string p_s = PERCENT + SPAN;
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + kz_elapsedtime + "経過" + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(BossHP, Serendia3chValue, Calpheon3chValue, Mediah3chValue, Valencia3chValue, Magoria3chValue, 0));
                            kz_b3 = ValueConverter(2, "Balenos");
                            string p_s = PERCENT + SPAN;
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + kz_elapsedtime + "経過" + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(BossHP, Serendia4chValue, Calpheon4chValue, Mediah4chValue, Valencia4chValue, Magoria4chValue, 0));
                            kz_b4 = ValueConverter(3, "Balenos");
                            string p_s = PERCENT + SPAN;
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + kz_elapsedtime + "経過" + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;

                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Balenos1chValue, BossHP, Calpheon1chValue, Mediah1chValue, Valencia1chValue, Magoria1chValue, Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            kz_s1 = ValueConverter(0, "Serendia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Balenos2chValue, BossHP, Calpheon2chValue, Mediah2chValue, Valencia2chValue, Magoria2chValue, Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            kz_s2 = ValueConverter(1, "Serendia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Balenos3chValue, BossHP, Calpheon3chValue, Mediah3chValue, Valencia3chValue, Magoria3chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_s3 = ValueConverter(2, "Serendia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Balenos4chValue, BossHP, Calpheon4chValue, Mediah4chValue, Valencia4chValue, Magoria4chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_s4 = ValueConverter(3, "Serendia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Balenos1chValue, Serendia1chValue, BossHP, Mediah1chValue, Valencia1chValue, Magoria1chValue, Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            kz_c1 = ValueConverter(0, "Calpheon");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Balenos2chValue, Serendia2chValue, BossHP, Mediah2chValue, Valencia2chValue, Magoria2chValue, Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            kz_c2 = ValueConverter(1, "Calpheon");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Balenos3chValue, Serendia3chValue, BossHP, Mediah3chValue, Valencia3chValue, Magoria3chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_c3 = ValueConverter(2, "Calpheon");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Balenos4chValue, Serendia4chValue, BossHP, Mediah4chValue, Valencia4chValue, Magoria4chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_c4 = ValueConverter(3, "Calpheon");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 1) == "m")   //メディアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Balenos1chValue, Serendia1chValue, Calpheon1chValue, BossHP, Valencia1chValue, Magoria1chValue, Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            kz_m1 = ValueConverter(0, "Mediah");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Balenos2chValue, Serendia2chValue, Calpheon2chValue, BossHP, Valencia2chValue, Magoria2chValue, Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            kz_m2 = ValueConverter(1, "Mediah");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Balenos3chValue, Serendia3chValue, Calpheon3chValue, BossHP, Valencia3chValue, Magoria3chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_m3 = ValueConverter(2, "Mediah");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Balenos4chValue, Serendia4chValue, Calpheon4chValue, BossHP, Valencia4chValue, Magoria4chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_m4 = ValueConverter(3, "Mediah");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        
                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Balenos1chValue, Serendia1chValue, Calpheon1chValue, Mediah1chValue, BossHP, Magoria1chValue, Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            kz_v1 = ValueConverter(0, "Valencia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Balenos2chValue, Serendia2chValue, Calpheon2chValue, Mediah2chValue, BossHP, Magoria2chValue, Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            kz_v2 = ValueConverter(1, "Valencia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Balenos3chValue, Serendia3chValue, Calpheon3chValue, Mediah3chValue, BossHP, Magoria3chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_v3 = ValueConverter(2, "Valencia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Balenos4chValue, Serendia4chValue, Calpheon4chValue, Mediah4chValue, BossHP, Magoria4chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_v4 = ValueConverter(3, "Valencia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        
                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma") //マゴリアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Balenos1chValue, Serendia1chValue, Calpheon1chValue, Mediah1chValue, Valencia1chValue, BossHP, Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            kz_ma1 = ValueConverter(0, "Magoria");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Balenos2chValue, Serendia2chValue, Calpheon2chValue, Mediah2chValue, Valencia2chValue, BossHP, Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            kz_ma2 = ValueConverter(1, "Magoria");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Balenos3chValue, Serendia3chValue, Calpheon3chValue, Mediah3chValue, Valencia3chValue, BossHP, 0));
                            string p_s = PERCENT + SPAN;
                            kz_ma3 = ValueConverter(2, "Magoria");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Balenos4chValue, Serendia4chValue, Calpheon4chValue, Mediah4chValue, Valencia4chValue, BossHP, 0));
                            string p_s = PERCENT + SPAN;
                            kz_ma4 = ValueConverter(3, "Magoria");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        
                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k") //カーマスリビアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Balenos1chValue, Serendia1chValue, Calpheon1chValue, Mediah1chValue, Valencia1chValue, Magoria1chValue, BossHP));
                            string p_s = PERCENT + SPAN;
                            kz_k1 = ValueConverter(0, "Kamasylvia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Balenos2chValue, Serendia2chValue, Calpheon2chValue, Mediah2chValue, Valencia2chValue, Magoria2chValue, BossHP));
                            string p_s = PERCENT + SPAN;
                            kz_k2 = ValueConverter(1, "Kamasylvia");
                            string BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + p_s + "2ch：" + kz_b2 + p_s + "3ch：" + kz_b3 + p_s + "4ch：" + kz_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + p_s + "2ch：" + kz_s2 + p_s + "3ch：" + kz_s3 + p_s + "4ch：" + kz_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + p_s + "2ch：" + kz_c2 + p_s + "3ch：" + kz_c3 + p_s + "4ch：" + kz_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + p_s + "2ch：" + kz_m2 + p_s + "3ch：" + kz_m3 + p_s + "4ch：" + kz_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + p_s + "2ch：" + kz_v2 + p_s + "3ch：" + kz_v3 + "4ch：" + kz_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + p_s + "2ch：" + kz_ma2 + p_s + "3ch：" + kz_ma3 + p_s + "4ch：" + kz_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + p_s + "2ch：" + kz_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("3")) {  }
                        if (BossChannel.Contains("4")) {  }
                        
                    } //カーマスリビアch
                    break;
                case 2: //カランダ
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(BossHP, Ka_Serendia1chValue, Ka_Calpheon1chValue, Ka_Mediah1chValue, Ka_Valencia1chValue, Ka_Magoria1chValue, Ka_Kms1chValue));
                            //ValueCheck(kz_b1, kz_b2, kz_b3, kz_b4, kz_s1, kz_s2, kz_s3, kz_s4, kz_c1, kz_c2, kz_c3, kz_c4, kz_m1, kz_m2, kz_m3, kz_m4, kz_v1, kz_v2, kz_v3, kz_v4, kz_ma1, kz_ma2, kz_ma3, kz_ma4, kz_k1, kz_k2);
                            ka_b1 = ValueConverter(4, "Balenos");
                            string p_s = PERCENT + SPAN;
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(BossHP, Ka_Serendia2chValue, Ka_Calpheon2chValue, Ka_Mediah2chValue, Ka_Valencia2chValue, Ka_Magoria2chValue, Ka_Kms2chValue));
                            ka_b2 = ValueConverter(5, "Balenos");
                            string p_s = PERCENT + SPAN;
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(BossHP, Serendia3chValue, Calpheon3chValue, Mediah3chValue, Valencia3chValue, Magoria3chValue, 0));
                            ka_b3 = ValueConverter(6, "Balenos");
                            string p_s = PERCENT + SPAN;
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(BossHP, Serendia4chValue, Calpheon4chValue, Mediah4chValue, Valencia4chValue, Magoria4chValue, 0));
                            ka_b4 = ValueConverter(7, "Balenos");
                            string p_s = PERCENT + SPAN;
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;

                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, BossHP, Ka_Calpheon1chValue, Ka_Mediah1chValue, Ka_Valencia1chValue, Ka_Magoria1chValue, Ka_Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            ka_s1 = ValueConverter(4, "Serendia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, BossHP, Ka_Calpheon2chValue, Ka_Mediah2chValue, Ka_Valencia2chValue, Ka_Magoria2chValue, Ka_Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            ka_s2 = ValueConverter(5, "Serendia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, BossHP, Ka_Calpheon3chValue, Ka_Mediah3chValue, Ka_Valencia3chValue, Ka_Magoria3chValue, 0));
                            string p_s = PERCENT + SPAN;
                            ka_s3 = ValueConverter(6, "Serendia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, BossHP, Ka_Calpheon4chValue, Ka_Mediah4chValue, Ka_Valencia4chValue, Ka_Magoria4chValue, 0));
                            string p_s = PERCENT + SPAN;
                            kz_s4 = ValueConverter(7, "Serendia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, BossHP, Ka_Mediah1chValue, Ka_Valencia1chValue, Ka_Magoria1chValue, Ka_Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            ka_c1 = ValueConverter(4, "Calpheon");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, BossHP, Ka_Mediah2chValue, Ka_Valencia2chValue, Ka_Magoria2chValue, Ka_Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            ka_c2 = ValueConverter(5, "Calpheon");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, Ka_Serendia3chValue, BossHP, Ka_Mediah3chValue, Ka_Valencia3chValue, Ka_Magoria3chValue, 0));
                            string p_s = PERCENT + SPAN;
                            ka_c3 = ValueConverter(6, "Calpheon");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, Ka_Serendia4chValue, BossHP, Ka_Mediah4chValue, Ka_Valencia4chValue, Ka_Magoria4chValue, 0));
                            string p_s = PERCENT + SPAN;
                            ka_c4 = ValueConverter(7, "Calpheon");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 1) == "m")   //メディアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, Ka_Calpheon1chValue, BossHP, Ka_Valencia1chValue, Ka_Magoria1chValue, Ka_Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            ka_m1 = ValueConverter(4, "Mediah");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, Ka_Calpheon2chValue, BossHP, Ka_Valencia2chValue, Ka_Magoria2chValue, Ka_Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            ka_m2 = ValueConverter(5, "Mediah");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, Ka_Serendia3chValue, Ka_Calpheon3chValue, BossHP, Ka_Valencia3chValue, Ka_Magoria3chValue, 0));
                            string p_s = PERCENT + SPAN;
                            ka_m3 = ValueConverter(6, "Mediah");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, Ka_Serendia4chValue, Ka_Calpheon4chValue, BossHP, Ka_Valencia4chValue, Ka_Magoria4chValue, 0));
                            string p_s = PERCENT + SPAN;
                            ka_m4 = ValueConverter(7, "Mediah");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }

                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v") //Valencia
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, Ka_Calpheon1chValue, Ka_Mediah1chValue, BossHP, Ka_Magoria1chValue, Ka_Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            ka_v1 = ValueConverter(4, "Valencia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, Ka_Calpheon2chValue, Ka_Mediah2chValue, BossHP, Ka_Magoria2chValue, Ka_Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            ka_v2 = ValueConverter(5, "Valencia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, Ka_Serendia3chValue, Ka_Calpheon3chValue, Ka_Mediah3chValue, BossHP, Magoria3chValue, 0));
                            string p_s = PERCENT + SPAN;
                            ka_v3 = ValueConverter(6, "Valencia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, Ka_Serendia4chValue, Ka_Calpheon4chValue, Ka_Mediah4chValue, BossHP, Ka_Magoria4chValue, 0));
                            string p_s = PERCENT + SPAN;
                            ka_v4 = ValueConverter(7, "Valencia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }

                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma") //マゴリアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, Ka_Calpheon1chValue, Ka_Mediah1chValue, Ka_Valencia1chValue, BossHP, Ka_Kms1chValue));
                            string p_s = PERCENT + SPAN;
                            ka_ma1 = ValueConverter(4, "Magoria");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, Ka_Calpheon2chValue, Ka_Mediah2chValue, Ka_Valencia2chValue, BossHP, Ka_Kms2chValue));
                            string p_s = PERCENT + SPAN;
                            ka_ma2 = ValueConverter(5, "Magoria");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, Ka_Serendia3chValue, Ka_Calpheon3chValue, Ka_Mediah3chValue, Ka_Valencia3chValue, BossHP, 0));
                            string p_s = PERCENT + SPAN;
                            ka_ma3 = ValueConverter(6, "Magoria");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, Ka_Serendia4chValue, Ka_Calpheon4chValue, Ka_Mediah4chValue, Ka_Valencia4chValue, BossHP, 0));
                            string p_s = PERCENT + SPAN;
                            ka_ma4 = ValueConverter(7, "Magoria");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }

                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k") //カーマスリビアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, Ka_Calpheon1chValue, Ka_Mediah1chValue, Ka_Valencia1chValue, Ka_Magoria1chValue, BossHP));
                            string p_s = PERCENT + SPAN;
                            ka_k1 = ValueConverter(4, "Kamasylvia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, Ka_Calpheon2chValue, Ka_Mediah2chValue, Ka_Valencia2chValue, Ka_Magoria2chValue, BossHP));
                            string p_s = PERCENT + SPAN;
                            ka_k2 = ValueConverter(5, "Kamasylvia");
                            string BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + "）";
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + p_s + "2ch：" + ka_b2 + p_s + "3ch：" + ka_b3 + p_s + "4ch：" + ka_b4 + p_s;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + p_s + "2ch：" + ka_s2 + p_s + "3ch：" + ka_s3 + p_s + "4ch：" + ka_s4 + p_s;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + p_s + "2ch：" + ka_c2 + p_s + "3ch：" + ka_c3 + p_s + "4ch：" + ka_c4 + p_s;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + p_s + "2ch：" + ka_m2 + p_s + "3ch：" + ka_m3 + p_s + "4ch：" + ka_m4 + p_s;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + p_s + "2ch：" + ka_v2 + p_s + "3ch：" + ka_v3 + "4ch：" + ka_v4 + p_s;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + p_s + "2ch：" + ka_ma2 + p_s + "3ch：" + ka_ma3 + p_s + "4ch：" + ka_ma4 + p_s;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + p_s + "2ch：" + ka_k2 + p_s;
                            return_status = HIGHLIGHT + BossChannelMapHeader + IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia + HIGHLIGHT;
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }

                    } //カーマスリビアch
                    break;
            }
            
            return return_status;
        }

        private static string ValueConverter(int ChannelID, string ChannelName)
        {
            string return_value = "";
            if(Program.DEBUGMODE == true)
            {
                //Program.WriteLog("BossStatus.ValueDefine_ArgCheck : " + ChannelID + " " + ChannelName);
            }
            switch (ChannelName)
            {
                case "Balenos":
                    if(BossChannelMapTable[ChannelID].Balenos > 0) { return_value = BossChannelMapTable[ChannelID].Balenos.ToString(); }
                    if(BossChannelMapTable[ChannelID].Balenos == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifBalenos : " + return_value);
                    }
                    break;
                case "Serendia":
                    if (BossChannelMapTable[ChannelID].Serendia > 0) { return_value = BossChannelMapTable[ChannelID].Serendia.ToString(); }
                    if (BossChannelMapTable[ChannelID].Serendia == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifSerendia : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Calpheon":
                    if (BossChannelMapTable[ChannelID].Calpheon > 0) { return_value = BossChannelMapTable[ChannelID].Calpheon.ToString(); }
                    if (BossChannelMapTable[ChannelID].Calpheon == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifCalpheon : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Mediah":
                    if (BossChannelMapTable[ChannelID].Mediah > 0) { return_value = BossChannelMapTable[ChannelID].Mediah.ToString(); }
                    if (BossChannelMapTable[ChannelID].Mediah == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifMediah : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Valencia":
                    if (BossChannelMapTable[ChannelID].Valencia> 0) { return_value = BossChannelMapTable[ChannelID].Valencia.ToString(); }
                    if (BossChannelMapTable[ChannelID].Valencia == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifValencia : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Magoria":
                    if (BossChannelMapTable[ChannelID].Magoria > 0) { return_value = BossChannelMapTable[ChannelID].Magoria.ToString(); }
                    if (BossChannelMapTable[ChannelID].Magoria == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifMagoria : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Kamasylvia":
                    if (BossChannelMapTable[ChannelID].Kamasylvia > 0) { return_value = BossChannelMapTable[ChannelID].Kamasylvia.ToString(); }
                    if (BossChannelMapTable[ChannelID].Kamasylvia == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifKamasylvia : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
            }
            return return_value;
        }
        private static void InternalBufferInit()
        {
            
            if (Program.isKzarkaAlreadySpawned)
            {
                //Kzarka
                kz_timer = new Timer();
                kz_b1 = ValueConverter(0, "Balenos");
                kz_b2 = ValueConverter(1, "Balenos");
                kz_b3 = ValueConverter(2, "Balenos");
                kz_b4 = ValueConverter(3, "Balenos");
                kz_s1 = ValueConverter(0, "Serendia");
                kz_s2 = ValueConverter(1, "Serendia");
                kz_s3 = ValueConverter(2, "Serendia");
                kz_s4 = ValueConverter(3, "Serendia");
                kz_c1 = ValueConverter(0, "Calpheon");
                kz_c2 = ValueConverter(1, "Calpheon");
                kz_c3 = ValueConverter(2, "Calpheon");
                kz_c4 = ValueConverter(3, "Calpheon");
                kz_m1 = ValueConverter(0, "Mediah");
                kz_m2 = ValueConverter(1, "Mediah");
                kz_m3 = ValueConverter(2, "Mediah");
                kz_m4 = ValueConverter(3, "Mediah");
                kz_v1 = ValueConverter(0, "Valencia");
                kz_v2 = ValueConverter(1, "Valencia");
                kz_v3 = ValueConverter(2, "Valencia");
                kz_v4 = ValueConverter(3, "Valencia");
                kz_ma1 = ValueConverter(0, "Magoria");
                kz_ma2 = ValueConverter(1, "Magoria");
                kz_ma3 = ValueConverter(2, "Magoria");
                kz_ma4 = ValueConverter(3, "Magoria");
                kz_k1 = ValueConverter(0, "Kamasylvia");
                kz_k2 = ValueConverter(1, "Kamasylvia");
            } //Kzarka くざか
            if (Program.isKarandaAlreadySpawned)
            {
                //Karanda
                ka_timer = new Timer();
                ka_b1 = ValueConverter(4, "Balenos");
                ka_b2 = ValueConverter(5, "Balenos");
                ka_b3 = ValueConverter(6, "Balenos");
                ka_b4 = ValueConverter(7, "Balenos");
                ka_s1 = ValueConverter(4, "Serendia");
                ka_s2 = ValueConverter(5, "Serendia");
                ka_s3 = ValueConverter(6, "Serendia");
                ka_s4 = ValueConverter(7, "Serendia");
                ka_c1 = ValueConverter(4, "Calpheon");
                ka_c2 = ValueConverter(5, "Calpheon");
                ka_c3 = ValueConverter(6, "Calpheon");
                ka_c4 = ValueConverter(7, "Calpheon");
                ka_m1 = ValueConverter(4, "Mediah");
                ka_m2 = ValueConverter(5, "Mediah");
                ka_m3 = ValueConverter(6, "Mediah");
                ka_m4 = ValueConverter(7, "Mediah");
                ka_v1 = ValueConverter(4, "Valencia");
                ka_v2 = ValueConverter(5, "Valencia");
                ka_v3 = ValueConverter(6, "Valencia");
                ka_v4 = ValueConverter(7, "Valencia");
                ka_ma1 = ValueConverter(4, "Magoria");
                ka_ma2 = ValueConverter(5, "Magoria");
                ka_ma3 = ValueConverter(6, "Magoria");
                ka_ma4 = ValueConverter(7, "Magoria");
                ka_k1 = ValueConverter(4, "Kamasylvia");
                ka_k2 = ValueConverter(5, "Kamasylvia");
            } //Karanda からんだ
            if (Program.isNouverAlreadySpawned)
            {
                nv_timer = new Timer();
                nv_b1 = ValueConverter(8, "Balenos");
                nv_b2 = ValueConverter(9, "Balenos");
                nv_b3 = ValueConverter(10, "Balenos");
                nv_b4 = ValueConverter(11, "Balenos");
                nv_s1 = ValueConverter(8, "Serendia");
                nv_s2 = ValueConverter(9, "Serendia");
                nv_s3 = ValueConverter(10, "Serendia");
                nv_s4 = ValueConverter(11, "Serendia");
                nv_c1 = ValueConverter(8, "Calpheon");
                nv_c2 = ValueConverter(9, "Calpheon");
                nv_c3 = ValueConverter(10, "Calpheon");
                nv_c4 = ValueConverter(11, "Calpheon");
                nv_m1 = ValueConverter(8, "Mediah");
                nv_m2 = ValueConverter(9, "Mediah");
                nv_m3 = ValueConverter(10, "Mediah");
                nv_m4 = ValueConverter(11, "Mediah");
                nv_v1 = ValueConverter(8, "Valencia");
                nv_v2 = ValueConverter(9, "Valencia");
                nv_v3 = ValueConverter(10, "Valencia");
                nv_v4 = ValueConverter(11, "Valencia");
                nv_ma1 = ValueConverter(8, "Magoria");
                nv_ma2 = ValueConverter(9, "Magoria");
                nv_ma3 = ValueConverter(10, "Magoria");
                nv_ma4 = ValueConverter(11, "Magoria");
                nv_k1 = ValueConverter(8, "Kamasylvia");
                nv_k2 = ValueConverter(9, "Kamasylvia");
            } //Nouver ぬーべる
            if (Program.isKutumAlreadySpawned)
            {
                ku_b1 = ValueConverter(12, "Balenos");
                ku_b2 = ValueConverter(13, "Balenos");
                ku_b3 = ValueConverter(14, "Balenos");
                ku_b4 = ValueConverter(15, "Balenos");
                ku_s1 = ValueConverter(12, "Serendia");
                ku_s2 = ValueConverter(13, "Serendia");
                ku_s3 = ValueConverter(14, "Serendia");
                ku_s4 = ValueConverter(15, "Serendia");
                ku_c1 = ValueConverter(12, "Calpheon");
                ku_c2 = ValueConverter(13, "Calpheon");
                ku_c3 = ValueConverter(14, "Calpheon");
                ku_c4 = ValueConverter(15, "Calpheon");
                ku_m1 = ValueConverter(12, "Mediah");
                ku_m2 = ValueConverter(13, "Mediah");
                ku_m3 = ValueConverter(14, "Mediah");
                ku_m4 = ValueConverter(15, "Mediah");
                ku_v1 = ValueConverter(12, "Valencia");
                ku_v2 = ValueConverter(13, "Valencia");
                ku_v3 = ValueConverter(14, "Valencia");
                ku_v4 = ValueConverter(15, "Valencia");
                ku_ma1 = ValueConverter(12, "Magoria");
                ku_ma2 = ValueConverter(13, "Magoria");
                ku_ma3 = ValueConverter(14, "Magoria");
                ku_ma4 = ValueConverter(15, "Magoria");
                ku_k1 = ValueConverter(12, "Kamasylvia");
                ku_k2 = ValueConverter(13, "Kamasylvia");
            } //Kutum くつむ
            if (Program.isRednoseAlreadySpawned)
            {
                rn_b1 = ValueConverter(16, "Balenos");
                rn_b2 = ValueConverter(17, "Balenos");
                rn_b3 = ValueConverter(18, "Balenos");
                rn_b4 = ValueConverter(19, "Balenos");
                rn_s1 = ValueConverter(16, "Serendia");
                rn_s2 = ValueConverter(17, "Serendia");
                rn_s3 = ValueConverter(18, "Serendia");
                rn_s4 = ValueConverter(19, "Serendia");
                rn_c1 = ValueConverter(16, "Calpheon");
                rn_c2 = ValueConverter(17, "Calpheon");
                rn_c3 = ValueConverter(18, "Calpheon");
                rn_c4 = ValueConverter(19, "Calpheon");
                rn_m1 = ValueConverter(16, "Mediah");
                rn_m2 = ValueConverter(17, "Mediah");
                rn_m3 = ValueConverter(18, "Mediah");
                rn_m4 = ValueConverter(19, "Mediah");
                rn_v1 = ValueConverter(16, "Valencia");
                rn_v2 = ValueConverter(17, "Valencia");
                rn_v3 = ValueConverter(18, "Valencia");
                rn_v4 = ValueConverter(19, "Valencia");
                rn_ma1 = ValueConverter(16, "Magoria");
                rn_ma2 = ValueConverter(17, "Magoria");
                rn_ma3 = ValueConverter(18, "Magoria");
                rn_ma4 = ValueConverter(19, "Magoria");
                rn_k1 = ValueConverter(16, "Kamasylvia");
                rn_k2 = ValueConverter(17, "Kamasylvia");
            } //Rednose れっどのーず
            if (Program.isBhegAlreadySpawned)
            {
                bh_b1 = ValueConverter(20, "Balenos");
                bh_b2 = ValueConverter(21, "Balenos");
                bh_b3 = ValueConverter(22, "Balenos");
                bh_b4 = ValueConverter(23, "Balenos");
                bh_s1 = ValueConverter(20, "Serendia");
                bh_s2 = ValueConverter(21, "Serendia");
                bh_s3 = ValueConverter(22, "Serendia");
                bh_s4 = ValueConverter(23, "Serendia");
                bh_c1 = ValueConverter(20, "Calpheon");
                bh_c2 = ValueConverter(21, "Calpheon");
                bh_c3 = ValueConverter(22, "Calpheon");
                bh_c4 = ValueConverter(23, "Calpheon");
                bh_m1 = ValueConverter(20, "Mediah");
                bh_m2 = ValueConverter(21, "Mediah");
                bh_m3 = ValueConverter(22, "Mediah");
                bh_m4 = ValueConverter(23, "Mediah");
                bh_v1 = ValueConverter(20, "Valencia");
                bh_v2 = ValueConverter(21, "Valencia");
                bh_v3 = ValueConverter(22, "Valencia");
                bh_v4 = ValueConverter(23, "Valencia");
                bh_ma1 = ValueConverter(20, "Magoria");
                bh_ma2 = ValueConverter(21, "Magoria");
                bh_ma3 = ValueConverter(22, "Magoria");
                bh_ma4 = ValueConverter(23, "Magoria");
                bh_k1 = ValueConverter(20, "Kamasylvia");
                bh_k2 = ValueConverter(21, "Kamasylvia");
            } //Bheg べぐ
            if (Program.isTreeAlreadySpawned)
            {
                tree_b1 = ValueConverter(24, "Balenos");
                tree_b2 = ValueConverter(25, "Balenos");
                tree_b3 = ValueConverter(26, "Balenos");
                tree_b4 = ValueConverter(27, "Balenos");
                tree_s1 = ValueConverter(24, "Serendia");
                tree_s2 = ValueConverter(25, "Serendia");
                tree_s3 = ValueConverter(26, "Serendia");
                tree_s4 = ValueConverter(27, "Serendia");
                tree_c1 = ValueConverter(24, "Calpheon");
                tree_c2 = ValueConverter(25, "Calpheon");
                tree_c3 = ValueConverter(26, "Calpheon");
                tree_c4 = ValueConverter(27, "Calpheon");
                tree_m1 = ValueConverter(24, "Mediah");
                tree_m2 = ValueConverter(25, "Mediah");
                tree_m3 = ValueConverter(26, "Mediah");
                tree_m4 = ValueConverter(27, "Mediah");
                tree_v1 = ValueConverter(24, "Valencia");
                tree_v2 = ValueConverter(25, "Valencia");
                tree_v3 = ValueConverter(26, "Valencia");
                tree_v4 = ValueConverter(27, "Valencia");
                tree_ma1 = ValueConverter(24, "Magoria");
                tree_ma2 = ValueConverter(25, "Magoria");
                tree_ma3 = ValueConverter(26, "Magoria");
                tree_ma4 = ValueConverter(27, "Magoria");
                tree_k1 = ValueConverter(24, "Kamasylvia");
                tree_k2 = ValueConverter(25, "Kamasylvia");
            } //Tree ぐどん
            if (Program.isMudmanAlreadySpawned)
            {
                mud_b1 = ValueConverter(28, "Balenos");
                mud_b2 = ValueConverter(29, "Balenos");
                mud_b3 = ValueConverter(30, "Balenos");
                mud_b4 = ValueConverter(31, "Balenos");
                mud_s1 = ValueConverter(28, "Serendia");
                mud_s2 = ValueConverter(29, "Serendia");
                mud_s3 = ValueConverter(30, "Serendia");
                mud_s4 = ValueConverter(31, "Serendia");
                mud_c1 = ValueConverter(28, "Calpheon");
                mud_c2 = ValueConverter(29, "Calpheon");
                mud_c3 = ValueConverter(30, "Calpheon");
                mud_c4 = ValueConverter(31, "Calpheon");
                mud_m1 = ValueConverter(28, "Mediah");
                mud_m2 = ValueConverter(29, "Mediah");
                mud_m3 = ValueConverter(30, "Mediah");
                mud_m4 = ValueConverter(31, "Mediah");
                mud_v1 = ValueConverter(28, "Valencia");
                mud_v2 = ValueConverter(29, "Valencia");
                mud_v3 = ValueConverter(30, "Valencia");
                mud_v4 = ValueConverter(31, "Valencia");
                mud_ma1 = ValueConverter(28, "Magoria");
                mud_ma2 = ValueConverter(29, "Magoria");
                mud_ma3 = ValueConverter(30, "Magoria");
                mud_ma4 = ValueConverter(31, "Magoria");
                mud_k1 = ValueConverter(28, "Kamasylvia");
                mud_k2 = ValueConverter(29, "Kamasylvia");
            }
        }
        private static bool IsBossDead(int BossID)
        {
            switch (BossID)
            {
                case 1:
                    for (int KzarkaChAmount = 0; 3 > KzarkaChAmount; KzarkaChAmount++)
                    {
                        if(BossChannelMapTable[KzarkaChAmount].Balenos == 0 && BossChannelMapTable[KzarkaChAmount].Serendia == 0 && BossChannelMapTable[KzarkaChAmount].Calpheon == 0)
                        {
                            //Boss End
                            return true;
                        }
                    }
                    break;
            }
            return false;
            
        }
        private static void ResetBossStatusByDeath(int BossID)
        {
            switch (BossID)
            {
                case 1:
                    BossChannelMapTable.RemoveAt(0);
                    BossChannelMapTable.RemoveAt(1);
                    BossChannelMapTable.RemoveAt(2);
                    BossChannelMapTable.RemoveAt(3);
                    break;
                case 2:
                    BossChannelMapTable.RemoveAt(4);
                    BossChannelMapTable.RemoveAt(5);
                    BossChannelMapTable.RemoveAt(6);
                    BossChannelMapTable.RemoveAt(7);
                    break;
                case 3:
                    BossChannelMapTable.RemoveAt(8);
                    BossChannelMapTable.RemoveAt(9);
                    BossChannelMapTable.RemoveAt(10);
                    BossChannelMapTable.RemoveAt(11);
                    break;
                case 4:
                    BossChannelMapTable.RemoveAt(12);
                    BossChannelMapTable.RemoveAt(13);
                    BossChannelMapTable.RemoveAt(14);
                    BossChannelMapTable.RemoveAt(15);
                    break;
                case 5:
                    BossChannelMapTable.RemoveAt(16);
                    BossChannelMapTable.RemoveAt(17);
                    BossChannelMapTable.RemoveAt(18);
                    BossChannelMapTable.RemoveAt(19);
                    break;
                case 6:
                    BossChannelMapTable.RemoveAt(20);
                    BossChannelMapTable.RemoveAt(21);
                    BossChannelMapTable.RemoveAt(22);
                    BossChannelMapTable.RemoveAt(23);
                    break;
                case 7:
                    BossChannelMapTable.RemoveAt(24);
                    BossChannelMapTable.RemoveAt(25);
                    BossChannelMapTable.RemoveAt(26);
                    BossChannelMapTable.RemoveAt(27);
                    break;
                case 8:
                    BossChannelMapTable.RemoveAt(28);
                    BossChannelMapTable.RemoveAt(29);
                    BossChannelMapTable.RemoveAt(30);
                    BossChannelMapTable.RemoveAt(31);
                    break;
                case 9:
                    BossChannelMapTable.RemoveAt(32);
                    BossChannelMapTable.RemoveAt(33);
                    BossChannelMapTable.RemoveAt(34);
                    BossChannelMapTable.RemoveAt(35);
                    break;

            }
        }
        private static TimeSpan CalculateElapsedTime(DateTime StartDT)
        {
            TimeSpan return_value;
            DateTime currentTime = DateTime.Now;
            TimeSpan timespan = currentTime - StartDT;
            return_value = timespan;
            return return_value;
        }
        private static void RefreshStatus(int BossID)
        {
            switch (BossID)
            {
                case 1:
                    kz_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    kz_timer.Interval = RefreshRate;
                    kz_timer.AutoReset = true;
                    kz_timer.Enabled = true;
                    break;
                case 2:
                    ka_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    ka_timer.Interval = RefreshRate;
                    ka_timer.AutoReset = true;
                    ka_timer.Enabled = true;
                    break;
                case 3:
                    nv_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    nv_timer.Interval = RefreshRate;
                    nv_timer.AutoReset = true;
                    nv_timer.Enabled = true;
                    break;
                case 4:
                    ku_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    ku_timer.Interval = RefreshRate;
                    ku_timer.AutoReset = true;
                    ku_timer.Enabled = true;
                    break;
                case 5:
                    rn_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    rn_timer.Interval = RefreshRate;
                    rn_timer.AutoReset = true;
                    rn_timer.Enabled = true;
                    break;

            }

        }
        private static void RefreshProcess(object sender,ElapsedEventArgs e)
        {
            if (Program.DEBUGMODE)
            {
                Program.WriteLog("Refresh Process was Executed.");
            }
        }
        private void AutoShutStatus(object sender,ElapsedEventArgs e)
        {

        }
        
        private void InvalidChannel()
        {
            //Program.client.
        }
    }
}
public class BossChannelMap
{
    public int Balenos;
    public int Serendia;
    public int Calpheon;
    public int Mediah;
    public int Valencia;
    public int Magoria;
    public int Kamasylvia;
    public BossChannelMap(int balenos, int serendia, int calpheon, int mediah, int valencia, int magoria, int kamasylvia)
    {
        Balenos = balenos;
        Serendia = serendia;
        Calpheon = calpheon;
        Mediah = mediah;
        Valencia = valencia;
        Magoria = magoria;
        Kamasylvia = kamasylvia;
    }
}
