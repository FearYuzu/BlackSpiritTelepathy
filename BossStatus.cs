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
        const int BossChannelMapTableCount = 39; //初回起動時に初期化するボス状況内部テーブル数
        //
        //ボス状況の最終報告から以下の閾値(時間）を超えても新規の報告がない場合、自動的に対象ボス状況を初期化
        //
        public int StatusClearThreshold_H = 0; //閾値（時）
        public int StatusClearThreshold_M = 30; //閾値（分）
        public int StatusClearThreshold_S = 0; //閾値（秒）
        //
        //Internal Boss Status Buffer
        //内部ボス状況バッファ
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
        //ボス状況の時間管理用バッファ
        //
        static DateTime kz_spawntime, ka_spawntime, ku_spawntime, nv_spawntime, rn_spawntime, bh_spawntime, tree_spawntime, mud_spawntime, tar_spawntime, iza_spawntime;
        static DateTime kz_lastreporttime, ka_lastreporttime, ku_lastreporttime, nv_lastreporttime, rn_lastreporttime, bh_lastreporttime, tree_lastreporttime, mud_lastreporttime, tar_lastreporttime, iza_lastreporttime;
        //static TimeSpan BossStatusLimitTime = TimeSpan.FromHours(1);
        static double RefreshRate = 100000;
        static TimeSpan BossStatusLimitTime = TimeSpan.FromSeconds(30);
        static Timer kz_timer, ka_timer, ku_timer, nv_timer, rn_timer, bh_timer, tree_timer, mud_timer, tar_timer, iza_timer;
        //
        public static string LatestBossStatus; //Boss Status Buffer using at Auto Refresh.
        //
        //Boss Status Automatically Clearing Event (It works when elapsed 30min since the last report from players.)
        //

        //
        public static void InitStatus()
        {
            for(int i = 0; i <= BossChannelMapTableCount; i++)
            {
                BossChannelMapTable.Insert(i, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
            }
            Program.WriteLog(SystemMessageDefine.BossChannelMapInit_JP);
        }
        public static string CreateStatus(int BossID)
        {
            string BossChannelMapStrBalenos;
            string BossChannelMapStrSerendia;
            string BossChannelMapStrCalpheon;
            string BossChannelMapStrMediah;
            string BossChannelMapStrValencia;
            string BossChannelMapStrMagoria;
            string BossChannelMapStrKamasylvia;
            
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
                    kz_lastreporttime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(1);
                    ///BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(kz_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(0, "Balenos") + SPAN + "2ch：" + ValueConverter(1, "Balenos") + SPAN + "3ch：" + ValueConverter(2, "Balenos") + SPAN + "4ch：" + ValueConverter(3, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(0, "Serendia") + SPAN + "2ch：" + ValueConverter(1, "Serendia") + SPAN + "3ch：" + ValueConverter(2, "Serendia") + SPAN + "4ch：" + ValueConverter(3, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(0, "Calpheon") + SPAN + "2ch：" + ValueConverter(1, "Calpheon") + SPAN + "3ch：" + ValueConverter(2, "Calpheon") + SPAN + "4ch：" + ValueConverter(3, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(0, "Mediah") + SPAN + "2ch：" + ValueConverter(1, "Mediah") + SPAN + "3ch：" + ValueConverter(2, "Mediah") + SPAN + "4ch：" + ValueConverter(3, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(0, "Valencia") + SPAN + "2ch：" + ValueConverter(1, "Valencia") + SPAN + "3ch：" + ValueConverter(2, "Valencia") + SPAN + "4ch：" + ValueConverter(3, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(0, "Magoria") + SPAN + "2ch：" + ValueConverter(1, "Magoria") + SPAN + "3ch：" + ValueConverter(2, "Magoria") + SPAN + "4ch：" + ValueConverter(3, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(0, "Kamasylvia") + SPAN + "2ch：" + ValueConverter(1, "Kamasylvia") + SPAN;
                    return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus = return_status;
                    break;
                case 2: //カランダ
                    BossChannelMapTable.Insert(4, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(5, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(6, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(7, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    ka_spawntime = DateTime.Now;
                    ka_lastreporttime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(2);
                    //BossChannelMapHeader = "カランダ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(ka_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(4, "Balenos") + SPAN + "2ch：" + ValueConverter(5, "Balenos") + SPAN + "3ch：" + ValueConverter(6, "Balenos") + SPAN + "4ch：" + ValueConverter(7, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(4, "Serendia") + SPAN + "2ch：" + ValueConverter(5, "Serendia") + SPAN + "3ch：" + ValueConverter(6, "Serendia") + SPAN + "4ch：" + ValueConverter(7, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(4, "Calpheon") + SPAN + "2ch：" + ValueConverter(5, "Calpheon") + SPAN + "3ch：" + ValueConverter(6, "Calpheon") + SPAN + "4ch：" + ValueConverter(7, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(4, "Mediah") + SPAN + "2ch：" + ValueConverter(5, "Mediah") + SPAN + "3ch：" + ValueConverter(6, "Mediah") + SPAN + "4ch：" + ValueConverter(7, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(4, "Valencia") + SPAN + "2ch：" + ValueConverter(5, "Valencia") + SPAN + "3ch：" + ValueConverter(6, "Valencia") + SPAN + "4ch：" + ValueConverter(7, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(4, "Magoria") + SPAN + "2ch：" + ValueConverter(5, "Magoria") + SPAN + "3ch：" + ValueConverter(6, "Magoria") + SPAN + "4ch：" + ValueConverter(7, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(4, "Kamasylvia") + SPAN + "2ch：" + ValueConverter(5, "Kamasylvia") + SPAN;
                    return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus = return_status;
                    break;
                case 3: //ヌーベル
                    BossChannelMapTable.Insert(8, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(9, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(10, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(11, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    nv_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(3);
                    //BossChannelMapHeader = "ヌーベル（最終更新　" + jst.ToString("HH時 mm分ss秒")  + " : 沸きから" + CalculateElapsedTime(nv_spawntime).Seconds + "秒経過" +"）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(8, "Balenos") + SPAN + "2ch：" + ValueConverter(9, "Balenos") + SPAN + "3ch：" + ValueConverter(10, "Balenos") + SPAN + "4ch：" + ValueConverter(11, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(8, "Serendia") + SPAN + "2ch：" + ValueConverter(9, "Serendia") + SPAN + "3ch：" + ValueConverter(10, "Serendia") + SPAN + "4ch：" + ValueConverter(11, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(8, "Calpheon") + SPAN + "2ch：" + ValueConverter(9, "Calpheon") + SPAN + "3ch：" + ValueConverter(10, "Calpheon") + SPAN + "4ch：" + ValueConverter(11, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(8, "Mediah") + SPAN + "2ch：" + ValueConverter(9, "Mediah") + SPAN + "3ch：" + ValueConverter(10, "Mediah") + SPAN + "4ch：" + ValueConverter(11, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(8, "Valencia") + SPAN + "2ch：" + ValueConverter(9, "Valencia") + SPAN + "3ch：" + ValueConverter(10, "Valencia") + SPAN + "4ch：" + ValueConverter(11, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(8, "Magoria") + SPAN + "2ch：" + ValueConverter(9, "Magoria") + SPAN + "3ch：" + ValueConverter(10, "Magoria") + SPAN + "4ch：" + ValueConverter(11, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(8, "Kamasylvia") + SPAN + "2ch：" + ValueConverter(9, "Kamasylvia") + SPAN;
                    return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus = return_status;
                    break;
                case 4: //クツム
                    BossChannelMapTable.Insert(12, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(13, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(14, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(15, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    ku_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(4);
                    //BossChannelMapHeader = "クツム（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(ku_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(12, "Balenos") + SPAN + "2ch：" + ValueConverter(13, "Balenos") + SPAN + "3ch：" + ValueConverter(14, "Balenos") + SPAN + "4ch：" + ValueConverter(15, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(12, "Serendia") + SPAN + "2ch：" + ValueConverter(13, "Serendia") + SPAN + "3ch：" + ValueConverter(14, "Serendia") + SPAN + "4ch：" + ValueConverter(15, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(12, "Calpheon") + SPAN + "2ch：" + ValueConverter(13, "Calpheon") + SPAN + "3ch：" + ValueConverter(14, "Calpheon") + SPAN + "4ch：" + ValueConverter(15, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(12, "Mediah") + SPAN + "2ch：" + ValueConverter(13, "Mediah") + SPAN + "3ch：" + ValueConverter(14, "Mediah") + SPAN + "4ch：" + ValueConverter(15, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(12, "Valencia") + SPAN + "2ch：" + ValueConverter(13, "Valencia") + SPAN + "3ch：" + ValueConverter(14, "Valencia") + SPAN + "4ch：" + ValueConverter(15, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(12, "Magoria") + SPAN + "2ch：" + ValueConverter(13, "Magoria") + SPAN + "3ch：" + ValueConverter(14, "Magoria") + SPAN + "4ch：" + ValueConverter(15, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(12, "Kamasylvia") + SPAN + "2ch：" + ValueConverter(13, "Kamasylvia") + SPAN;
                    return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus = return_status;
                    break;
                    
                case 5: //レッドノーズ
                    BossChannelMapTable.Insert(16, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(17, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(18, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(19, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    rn_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(5);
                    //BossChannelMapHeader = "レッドノーズ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(rn_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(16, "Balenos") + SPAN + "2ch：" + ValueConverter(17, "Balenos") + SPAN + "3ch：" + ValueConverter(18, "Balenos") + SPAN + "4ch：" + ValueConverter(19, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(16, "Serendia") + SPAN + "2ch：" + ValueConverter(17, "Serendia") + SPAN + "3ch：" + ValueConverter(18, "Serendia") + SPAN + "4ch：" + ValueConverter(19, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(16, "Calpheon") + SPAN + "2ch：" + ValueConverter(17, "Calpheon") + SPAN + "3ch：" + ValueConverter(18, "Calpheon") + SPAN + "4ch：" + ValueConverter(19, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(16, "Mediah") + SPAN + "2ch：" + ValueConverter(17, "Mediah") + SPAN + "3ch：" + ValueConverter(18, "Mediah") + SPAN + "4ch：" + ValueConverter(19, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(16, "Valencia") + SPAN + "2ch：" + ValueConverter(17, "Valencia") + SPAN + "3ch：" + ValueConverter(18, "Valencia") + SPAN + "4ch：" + ValueConverter(19, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(16, "Magoria") + SPAN + "2ch：" + ValueConverter(17, "Magoria") + SPAN + "3ch：" + ValueConverter(18, "Magoria") + SPAN + "4ch：" + ValueConverter(19, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(16, "Magoria") + SPAN + "2ch：" + ValueConverter(17, "Magoria") + SPAN;
                    return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus = return_status;
                    break;
                case 6: //ベグ
                    BossChannelMapTable.Insert(20, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(21, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(22, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(23, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    bh_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(6);
                    //BossChannelMapHeader = "ベグ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(rn_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(20, "Balenos") + SPAN + "2ch：" + ValueConverter(21, "Balenos") + SPAN + "3ch：" + ValueConverter(22, "Balenos") + SPAN + "4ch：" + ValueConverter(23, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(20, "Serendia") + SPAN + "2ch：" + ValueConverter(21, "Serendia") + SPAN + "3ch：" + ValueConverter(22, "Serendia") + SPAN + "4ch：" + ValueConverter(23, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(20, "Calpheon") + SPAN + "2ch：" + ValueConverter(21, "Calpheon") + SPAN + "3ch：" + ValueConverter(22, "Calpheon") + SPAN + "4ch：" + ValueConverter(23, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(20, "Mediah") + SPAN + "2ch：" + ValueConverter(21, "Mediah") + SPAN + "3ch：" + ValueConverter(22, "Mediah") + SPAN + "4ch：" + ValueConverter(23, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(20, "Valencia") + SPAN + "2ch：" + ValueConverter(21, "Valencia") + SPAN + "3ch：" + ValueConverter(22, "Valencia") + SPAN + "4ch：" + ValueConverter(23, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(20, "Magoria") + SPAN + "2ch：" + ValueConverter(21, "Magoria") + SPAN + "3ch：" + ValueConverter(22, "Magoria") + SPAN + "4ch：" + ValueConverter(23, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(20, "Kamasylvia") + SPAN + "2ch：" + ValueConverter(21, "Kamasylvia") + SPAN;
                    return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus = return_status;
                    break;
                case 7: //愚鈍
                    BossChannelMapTable.Insert(24, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(25, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(26, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(27, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    tree_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(7);
                    //BossChannelMapHeader = "愚鈍な木の精霊（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(rn_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(24, "Balenos") + SPAN + "2ch：" + ValueConverter(25, "Balenos") + SPAN + "3ch：" + ValueConverter(26, "Balenos") + SPAN + "4ch：" + ValueConverter(27, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(24, "Serendia") + SPAN + "2ch：" + ValueConverter(25, "Serendia") + SPAN + "3ch：" + ValueConverter(26, "Serendia") + SPAN + "4ch：" + ValueConverter(27, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(24, "Calpheon") + SPAN + "2ch：" + ValueConverter(25, "Calpheon") + SPAN + "3ch：" + ValueConverter(26, "Calpheon") + SPAN + "4ch：" + ValueConverter(27, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(24, "Mediah") + SPAN + "2ch：" + ValueConverter(25, "Mediah") + SPAN + "3ch：" + ValueConverter(26, "Mediah") + SPAN + "4ch：" + ValueConverter(27, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(24, "Valencia") + SPAN + "2ch：" + ValueConverter(25, "Valencia") + SPAN + "3ch：" + ValueConverter(26, "Valencia") + SPAN + "4ch：" + ValueConverter(27, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(24, "Magoria") + SPAN + "2ch：" + ValueConverter(25, "Magoria") + SPAN + "3ch：" + ValueConverter(26, "Magoria") + SPAN + "4ch：" + ValueConverter(27, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(24, "Kamasylvia") + SPAN + "2ch：" + ValueConverter(25, "Kamasylvia") + SPAN;
                    return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus = return_status;
                    break;
                case 8: //マッドマン
                    BossChannelMapTable.Insert(28, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(29, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(30, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(31, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    mud_spawntime = DateTime.Now;
                    InternalBufferInit();
                    RefreshStatus(8);
                    //BossChannelMapHeader = "愚鈍な木の精霊（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(rn_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(28, "Balenos") + SPAN + "2ch：" + ValueConverter(29, "Balenos") + SPAN + "3ch：" + ValueConverter(30, "Balenos") + SPAN + "4ch：" + ValueConverter(31, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(28, "Serendia") + SPAN + "2ch：" + ValueConverter(29, "Serendia") + SPAN + "3ch：" + ValueConverter(30, "Serendia") + SPAN + "4ch：" + ValueConverter(31, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(28, "Calpheon") + SPAN + "2ch：" + ValueConverter(29, "Calpheon") + SPAN + "3ch：" + ValueConverter(30, "Calpheon") + SPAN + "4ch：" + ValueConverter(31, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(28, "Mediah") + SPAN + "2ch：" + ValueConverter(29, "Mediah") + SPAN + "3ch：" + ValueConverter(30, "Mediah") + SPAN + "4ch：" + ValueConverter(31, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(28, "Valencia") + SPAN + "2ch：" + ValueConverter(29, "Valencia") + SPAN + "3ch：" + ValueConverter(30, "Valencia") + SPAN + "4ch：" + ValueConverter(31, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(28, "Magoria") + SPAN + "2ch：" + ValueConverter(29, "Magoria") + SPAN + "3ch：" + ValueConverter(30, "Magoria") + SPAN + "4ch：" + ValueConverter(31, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(28, "Kamasylvia") + SPAN + "2ch：" + ValueConverter(29, "Kamasylvia") + SPAN;
                    return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus = return_status;
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
            string return_status = "N/A";

            //
            //現在の各chボス体力値取得
            //
            //Kzarka Status
            //
            int Kz_Balenos1chValue = BossChannelMapTable[0].Balenos;
            int Kz_Balenos2chValue = BossChannelMapTable[1].Balenos;
            int Kz_Balenos3chValue = BossChannelMapTable[2].Balenos;
            int Kz_Balenos4chValue = BossChannelMapTable[3].Balenos;
            int Kz_Serendia1chValue = BossChannelMapTable[0].Serendia;
            int Kz_Serendia2chValue = BossChannelMapTable[1].Serendia;
            int Kz_Serendia3chValue = BossChannelMapTable[2].Serendia;
            int Kz_Serendia4chValue = BossChannelMapTable[3].Serendia;
            int Kz_Calpheon1chValue = BossChannelMapTable[0].Calpheon;
            int Kz_Calpheon2chValue = BossChannelMapTable[1].Calpheon;
            int Kz_Calpheon3chValue = BossChannelMapTable[2].Calpheon;
            int Kz_Calpheon4chValue = BossChannelMapTable[3].Calpheon;
            int Kz_Mediah1chValue = BossChannelMapTable[0].Mediah;
            int Kz_Mediah2chValue = BossChannelMapTable[1].Mediah;
            int Kz_Mediah3chValue = BossChannelMapTable[2].Mediah;
            int Kz_Mediah4chValue = BossChannelMapTable[3].Mediah;
            int Kz_Valencia1chValue = BossChannelMapTable[0].Valencia;
            int Kz_Valencia2chValue = BossChannelMapTable[1].Valencia;
            int Kz_Valencia3chValue = BossChannelMapTable[2].Valencia;
            int Kz_Valencia4chValue = BossChannelMapTable[3].Valencia;
            int Kz_Magoria1chValue = BossChannelMapTable[0].Magoria;
            int Kz_Magoria2chValue = BossChannelMapTable[1].Magoria;
            int Kz_Magoria3chValue = BossChannelMapTable[2].Magoria;
            int Kz_Magoria4chValue = BossChannelMapTable[3].Magoria;
            int Kz_Kms1chValue = BossChannelMapTable[0].Kamasylvia;
            int Kz_Kms2chValue = BossChannelMapTable[1].Kamasylvia;
            if (Program.DEBUGMODE)
            {
                Program.WriteLog("BossStatus.ChangeStatusValue() : Kzarka Status Value Init Finished.");
            }
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
            if (Program.DEBUGMODE) { Program.WriteLog("BossStatus.ChangeStatusValue() : Karanda Status Value Init Finished."); }
            //
            //Nouver Status
            //
            int Nv_Balenos1chValue = BossChannelMapTable[8].Balenos;
            int Nv_Balenos2chValue = BossChannelMapTable[9].Balenos;
            int Nv_Balenos3chValue = BossChannelMapTable[10].Balenos;
            int Nv_Balenos4chValue = BossChannelMapTable[11].Balenos;
            int Nv_Serendia1chValue = BossChannelMapTable[8].Serendia;
            int Nv_Serendia2chValue = BossChannelMapTable[9].Serendia;
            int Nv_Serendia3chValue = BossChannelMapTable[10].Serendia;
            int Nv_Serendia4chValue = BossChannelMapTable[11].Serendia;
            int Nv_Calpheon1chValue = BossChannelMapTable[8].Calpheon;
            int Nv_Calpheon2chValue = BossChannelMapTable[9].Calpheon;
            int Nv_Calpheon3chValue = BossChannelMapTable[10].Calpheon;
            int Nv_Calpheon4chValue = BossChannelMapTable[11].Calpheon;
            int Nv_Mediah1chValue = BossChannelMapTable[8].Mediah;
            int Nv_Mediah2chValue = BossChannelMapTable[9].Mediah;
            int Nv_Mediah3chValue = BossChannelMapTable[10].Mediah;
            int Nv_Mediah4chValue = BossChannelMapTable[11].Mediah;
            int Nv_Valencia1chValue = BossChannelMapTable[8].Valencia;
            int Nv_Valencia2chValue = BossChannelMapTable[9].Valencia;
            int Nv_Valencia3chValue = BossChannelMapTable[10].Valencia;
            int Nv_Valencia4chValue = BossChannelMapTable[11].Valencia;
            int Nv_Magoria1chValue = BossChannelMapTable[8].Magoria;
            int Nv_Magoria2chValue = BossChannelMapTable[9].Magoria;
            int Nv_Magoria3chValue = BossChannelMapTable[10].Magoria;
            int Nv_Magoria4chValue = BossChannelMapTable[11].Magoria;
            int Nv_Kms1chValue = BossChannelMapTable[8].Kamasylvia;
            int Nv_Kms2chValue = BossChannelMapTable[9].Kamasylvia;
            if (Program.DEBUGMODE) { Program.WriteLog("NouverStatusInitFinished."); }
            //
            //クツム（Kutum）
            //
            int Ku_Balenos1chValue = BossChannelMapTable[12].Balenos;
            int Ku_Balenos2chValue = BossChannelMapTable[13].Balenos;
            int Ku_Balenos3chValue = BossChannelMapTable[14].Balenos;
            int Ku_Balenos4chValue = BossChannelMapTable[15].Balenos;
            int Ku_Serendia1chValue = BossChannelMapTable[12].Serendia;
            int Ku_Serendia2chValue = BossChannelMapTable[13].Serendia;
            int Ku_Serendia3chValue = BossChannelMapTable[14].Serendia;
            int Ku_Serendia4chValue = BossChannelMapTable[15].Serendia;
            int Ku_Calpheon1chValue = BossChannelMapTable[12].Calpheon;
            int Ku_Calpheon2chValue = BossChannelMapTable[13].Calpheon;
            int Ku_Calpheon3chValue = BossChannelMapTable[14].Calpheon;
            int Ku_Calpheon4chValue = BossChannelMapTable[15].Calpheon;
            int Ku_Mediah1chValue = BossChannelMapTable[12].Mediah;
            int Ku_Mediah2chValue = BossChannelMapTable[13].Mediah;
            int Ku_Mediah3chValue = BossChannelMapTable[14].Mediah;
            int Ku_Mediah4chValue = BossChannelMapTable[15].Mediah;
            int Ku_Valencia1chValue = BossChannelMapTable[12].Valencia;
            int Ku_Valencia2chValue = BossChannelMapTable[13].Valencia;
            int Ku_Valencia3chValue = BossChannelMapTable[14].Valencia;
            int Ku_Valencia4chValue = BossChannelMapTable[15].Valencia;
            int Ku_Magoria1chValue = BossChannelMapTable[12].Magoria;
            int Ku_Magoria2chValue = BossChannelMapTable[13].Magoria;
            int Ku_Magoria3chValue = BossChannelMapTable[14].Magoria;
            int Ku_Magoria4chValue = BossChannelMapTable[15].Magoria;
            int Ku_Kms1chValue = BossChannelMapTable[12].Kamasylvia;
            int Ku_Kms2chValue = BossChannelMapTable[13].Kamasylvia;
            //
            //レッドノーズ(Rednose)
            //
            int Rn_Balenos1chValue = BossChannelMapTable[16].Balenos;
            int Rn_Balenos2chValue = BossChannelMapTable[17].Balenos;
            int Rn_Balenos3chValue = BossChannelMapTable[18].Balenos;
            int Rn_Balenos4chValue = BossChannelMapTable[19].Balenos;
            int Rn_Serendia1chValue = BossChannelMapTable[16].Serendia;
            int Rn_Serendia2chValue = BossChannelMapTable[17].Serendia;
            int Rn_Serendia3chValue = BossChannelMapTable[18].Serendia;
            int Rn_Serendia4chValue = BossChannelMapTable[19].Serendia;
            int Rn_Calpheon1chValue = BossChannelMapTable[16].Calpheon;
            int Rn_Calpheon2chValue = BossChannelMapTable[17].Calpheon;
            int Rn_Calpheon3chValue = BossChannelMapTable[18].Calpheon;
            int Rn_Calpheon4chValue = BossChannelMapTable[19].Calpheon;
            int Rn_Mediah1chValue = BossChannelMapTable[16].Mediah;
            int Rn_Mediah2chValue = BossChannelMapTable[17].Mediah;
            int Rn_Mediah3chValue = BossChannelMapTable[18].Mediah;
            int Rn_Mediah4chValue = BossChannelMapTable[19].Mediah;
            int Rn_Valencia1chValue = BossChannelMapTable[16].Valencia;
            int Rn_Valencia2chValue = BossChannelMapTable[17].Valencia;
            int Rn_Valencia3chValue = BossChannelMapTable[18].Valencia;
            int Rn_Valencia4chValue = BossChannelMapTable[19].Valencia;
            int Rn_Magoria1chValue = BossChannelMapTable[16].Magoria;
            int Rn_Magoria2chValue = BossChannelMapTable[17].Magoria;
            int Rn_Magoria3chValue = BossChannelMapTable[18].Magoria;
            int Rn_Magoria4chValue = BossChannelMapTable[19].Magoria;
            int Rn_Kms1chValue = BossChannelMapTable[16].Kamasylvia;
            int Rn_Kms2chValue = BossChannelMapTable[17].Kamasylvia;
            //
            //Bheg
            //
            int Bh_Balenos1chValue = BossChannelMapTable[20].Balenos;
            int Bh_Balenos2chValue = BossChannelMapTable[21].Balenos;
            int Bh_Balenos3chValue = BossChannelMapTable[22].Balenos;
            int Bh_Balenos4chValue = BossChannelMapTable[23].Balenos;
            int Bh_Serendia1chValue = BossChannelMapTable[20].Serendia;
            int Bh_Serendia2chValue = BossChannelMapTable[21].Serendia;
            int Bh_Serendia3chValue = BossChannelMapTable[22].Serendia;
            int Bh_Serendia4chValue = BossChannelMapTable[23].Serendia;
            int Bh_Calpheon1chValue = BossChannelMapTable[20].Calpheon;
            int Bh_Calpheon2chValue = BossChannelMapTable[21].Calpheon;
            int Bh_Calpheon3chValue = BossChannelMapTable[22].Calpheon;
            int Bh_Calpheon4chValue = BossChannelMapTable[23].Calpheon;
            int Bh_Mediah1chValue = BossChannelMapTable[20].Mediah;
            int Bh_Mediah2chValue = BossChannelMapTable[21].Mediah;
            int Bh_Mediah3chValue = BossChannelMapTable[22].Mediah;
            int Bh_Mediah4chValue = BossChannelMapTable[23].Mediah;
            int Bh_Valencia1chValue = BossChannelMapTable[20].Valencia;
            int Bh_Valencia2chValue = BossChannelMapTable[21].Valencia;
            int Bh_Valencia3chValue = BossChannelMapTable[22].Valencia;
            int Bh_Valencia4chValue = BossChannelMapTable[23].Valencia;
            int Bh_Magoria1chValue = BossChannelMapTable[20].Magoria;
            int Bh_Magoria2chValue = BossChannelMapTable[21].Magoria;
            int Bh_Magoria3chValue = BossChannelMapTable[22].Magoria;
            int Bh_Magoria4chValue = BossChannelMapTable[23].Magoria;
            int Bh_Kms1chValue = BossChannelMapTable[20].Kamasylvia;
            int Bh_Kms2chValue = BossChannelMapTable[21].Kamasylvia;
            //
            //Tree
            //
            int Tr_Balenos1chValue = BossChannelMapTable[24].Balenos;
            int Tr_Balenos2chValue = BossChannelMapTable[25].Balenos;
            int Tr_Balenos3chValue = BossChannelMapTable[26].Balenos;
            int Tr_Balenos4chValue = BossChannelMapTable[27].Balenos;
            int Tr_Serendia1chValue = BossChannelMapTable[24].Serendia;
            int Tr_Serendia2chValue = BossChannelMapTable[25].Serendia;
            int Tr_Serendia3chValue = BossChannelMapTable[26].Serendia;
            int Tr_Serendia4chValue = BossChannelMapTable[27].Serendia;
            int Tr_Calpheon1chValue = BossChannelMapTable[24].Calpheon;
            int Tr_Calpheon2chValue = BossChannelMapTable[25].Calpheon;
            int Tr_Calpheon3chValue = BossChannelMapTable[26].Calpheon;
            int Tr_Calpheon4chValue = BossChannelMapTable[27].Calpheon;
            int Tr_Mediah1chValue = BossChannelMapTable[24].Mediah;
            int Tr_Mediah2chValue = BossChannelMapTable[25].Mediah;
            int Tr_Mediah3chValue = BossChannelMapTable[26].Mediah;
            int Tr_Mediah4chValue = BossChannelMapTable[27].Mediah;
            int Tr_Valencia1chValue = BossChannelMapTable[24].Valencia;
            int Tr_Valencia2chValue = BossChannelMapTable[25].Valencia;
            int Tr_Valencia3chValue = BossChannelMapTable[26].Valencia;
            int Tr_Valencia4chValue = BossChannelMapTable[27].Valencia;
            int Tr_Magoria1chValue = BossChannelMapTable[24].Magoria;
            int Tr_Magoria2chValue = BossChannelMapTable[25].Magoria;
            int Tr_Magoria3chValue = BossChannelMapTable[26].Magoria;
            int Tr_Magoria4chValue = BossChannelMapTable[27].Magoria;
            int Tr_Kms1chValue = BossChannelMapTable[24].Kamasylvia;
            int Tr_Kms2chValue = BossChannelMapTable[25].Kamasylvia;
            //
            //Mud
            //
            int Md_Balenos1chValue = BossChannelMapTable[28].Balenos;
            int Md_Balenos2chValue = BossChannelMapTable[29].Balenos;
            int Md_Balenos3chValue = BossChannelMapTable[30].Balenos;
            int Md_Balenos4chValue = BossChannelMapTable[31].Balenos;
            int Md_Serendia1chValue = BossChannelMapTable[28].Serendia;
            int Md_Serendia2chValue = BossChannelMapTable[29].Serendia;
            int Md_Serendia3chValue = BossChannelMapTable[30].Serendia;
            int Md_Serendia4chValue = BossChannelMapTable[31].Serendia;
            int Md_Calpheon1chValue = BossChannelMapTable[28].Calpheon;
            int Md_Calpheon2chValue = BossChannelMapTable[29].Calpheon;
            int Md_Calpheon3chValue = BossChannelMapTable[30].Calpheon;
            int Md_Calpheon4chValue = BossChannelMapTable[31].Calpheon;
            int Md_Mediah1chValue = BossChannelMapTable[28].Mediah;
            int Md_Mediah2chValue = BossChannelMapTable[29].Mediah;
            int Md_Mediah3chValue = BossChannelMapTable[30].Mediah;
            int Md_Mediah4chValue = BossChannelMapTable[31].Mediah;
            int Md_Valencia1chValue = BossChannelMapTable[28].Valencia;
            int Md_Valencia2chValue = BossChannelMapTable[29].Valencia;
            int Md_Valencia3chValue = BossChannelMapTable[30].Valencia;
            int Md_Valencia4chValue = BossChannelMapTable[31].Valencia;
            int Md_Magoria1chValue = BossChannelMapTable[28].Magoria;
            int Md_Magoria2chValue = BossChannelMapTable[29].Magoria;
            int Md_Magoria3chValue = BossChannelMapTable[30].Magoria;
            int Md_Magoria4chValue = BossChannelMapTable[31].Magoria;
            int Md_Kms1chValue = BossChannelMapTable[28].Kamasylvia;
            int Md_Kms2chValue = BossChannelMapTable[29].Kamasylvia;
            //
            //ボス体力値更新処理開始
            //
            switch (BossID)
            {
                case 1:
                    if (Program.DEBUGMODE)
                    {
                        Program.WriteLog(SystemMessageDefine.Kz_ElapsedTime_JP + CalculateElapsedTime(kz_spawntime).Seconds + "秒");
                        
                    }
                    if (BossChannel.Substring(0,1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(BossHP, Kz_Serendia1chValue, Kz_Calpheon1chValue, Kz_Mediah1chValue, Kz_Valencia1chValue, Kz_Magoria1chValue, Kz_Kms1chValue));
                            //ValueCheck(kz_b1, kz_b2, kz_b3, kz_b4, kz_s1, kz_s2, kz_s3, kz_s4, kz_c1, kz_c2, kz_c3, kz_c4, kz_m1, kz_m2, kz_m3, kz_m4, kz_v1, kz_v2, kz_v3, kz_v4, kz_ma1, kz_ma2, kz_ma3, kz_ma4, kz_k1, kz_k2);
                            kz_b1 = ValueConverter(0, "Balenos");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(BossHP, Kz_Serendia2chValue, Kz_Calpheon2chValue, Kz_Mediah2chValue, Kz_Valencia2chValue, Kz_Magoria2chValue, Kz_Kms2chValue));
                            kz_b2 = ValueConverter(1, "Balenos");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(BossHP, Kz_Serendia3chValue, Kz_Calpheon3chValue, Kz_Mediah3chValue, Kz_Valencia3chValue, Kz_Magoria3chValue, 0));
                            kz_b3 = ValueConverter(2, "Balenos");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(BossHP, Kz_Serendia4chValue, Kz_Calpheon4chValue, Kz_Mediah4chValue, Kz_Valencia4chValue, Kz_Magoria4chValue, 0));
                            kz_b4 = ValueConverter(3, "Balenos");
                            kz_lastreporttime = DateTime.Now;

                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;

                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Kz_Balenos1chValue, BossHP, Kz_Calpheon1chValue, Kz_Mediah1chValue, Kz_Valencia1chValue, Kz_Magoria1chValue, Kz_Kms1chValue));
                            
                            kz_s1 = ValueConverter(0, "Serendia");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Kz_Balenos2chValue, BossHP, Kz_Calpheon2chValue, Kz_Mediah2chValue, Kz_Valencia2chValue, Kz_Magoria2chValue, Kz_Kms2chValue));
                            
                            kz_s2 = ValueConverter(1, "Serendia");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Kz_Balenos3chValue, BossHP, Kz_Calpheon3chValue, Kz_Mediah3chValue, Kz_Valencia3chValue, Kz_Magoria3chValue, 0));
                            kz_s3 = ValueConverter(2, "Serendia");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Kz_Balenos4chValue, BossHP, Kz_Calpheon4chValue, Kz_Mediah4chValue, Kz_Valencia4chValue, Kz_Magoria4chValue, 0));
                            
                            kz_s4 = ValueConverter(3, "Serendia");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Kz_Balenos1chValue, Kz_Serendia1chValue, BossHP, Kz_Mediah1chValue, Kz_Valencia1chValue, Kz_Magoria1chValue, Kz_Kms1chValue));
                            
                            kz_c1 = ValueConverter(0, "Calpheon");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Kz_Balenos2chValue, Kz_Serendia2chValue, BossHP, Kz_Mediah2chValue, Kz_Valencia2chValue, Kz_Magoria2chValue, Kz_Kms2chValue));
                            
                            kz_c2 = ValueConverter(1, "Calpheon");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Kz_Balenos3chValue, Kz_Serendia3chValue, BossHP, Kz_Mediah3chValue, Kz_Valencia3chValue, Kz_Magoria3chValue, 0));
                            
                            kz_c3 = ValueConverter(2, "Calpheon");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Kz_Balenos4chValue, Kz_Serendia4chValue, BossHP, Kz_Mediah4chValue, Kz_Valencia4chValue, Kz_Magoria4chValue, 0));
                            
                            kz_c4 = ValueConverter(3, "Calpheon");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 1) == "m")   //メディアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Kz_Balenos1chValue, Kz_Serendia1chValue, Kz_Calpheon1chValue, BossHP, Kz_Valencia1chValue, Kz_Magoria1chValue, Kz_Kms1chValue));
                            
                            kz_m1 = ValueConverter(0, "Mediah");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Kz_Balenos2chValue, Kz_Serendia2chValue, Kz_Calpheon2chValue, BossHP, Kz_Valencia2chValue, Kz_Magoria2chValue, Kz_Kms2chValue));
                            
                            kz_m2 = ValueConverter(1, "Mediah");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Kz_Balenos3chValue, Kz_Serendia3chValue, Kz_Calpheon3chValue, BossHP, Kz_Valencia3chValue, Kz_Magoria3chValue, 0));
                            
                            kz_m3 = ValueConverter(2, "Mediah");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Kz_Balenos4chValue, Kz_Serendia4chValue, Kz_Calpheon4chValue, BossHP, Kz_Valencia4chValue, Kz_Magoria4chValue, 0));
                            
                            kz_m4 = ValueConverter(3, "Mediah");
                            kz_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        
                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Kz_Balenos1chValue, Kz_Serendia1chValue, Kz_Calpheon1chValue, Kz_Mediah1chValue, BossHP, Kz_Magoria1chValue, Kz_Kms1chValue));
                            
                            kz_v1 = ValueConverter(0, "Valencia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Kz_Balenos2chValue, Kz_Serendia2chValue, Kz_Calpheon2chValue, Kz_Mediah2chValue, BossHP, Kz_Magoria2chValue, Kz_Kms2chValue));
                            
                            kz_v2 = ValueConverter(1, "Valencia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Kz_Balenos3chValue, Kz_Serendia3chValue, Kz_Calpheon3chValue, Kz_Mediah3chValue, BossHP, Kz_Magoria3chValue, 0));
                            
                            kz_v3 = ValueConverter(2, "Valencia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Kz_Balenos4chValue, Kz_Serendia4chValue, Kz_Calpheon4chValue, Kz_Mediah4chValue, BossHP, Kz_Magoria4chValue, 0));
                            
                            kz_v4 = ValueConverter(3, "Valencia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        
                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma") //マゴリアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Kz_Balenos1chValue, Kz_Serendia1chValue, Kz_Calpheon1chValue, Kz_Mediah1chValue, Kz_Valencia1chValue, BossHP, Kz_Kms1chValue));
                            
                            kz_ma1 = ValueConverter(0, "Magoria");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Kz_Balenos2chValue, Kz_Serendia2chValue, Kz_Calpheon2chValue, Kz_Mediah2chValue, Kz_Valencia2chValue, BossHP, Kz_Kms2chValue));
                            
                            kz_ma2 = ValueConverter(1, "Magoria");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(2, new BossChannelMap(Kz_Balenos3chValue, Kz_Serendia3chValue, Kz_Calpheon3chValue, Kz_Mediah3chValue, Kz_Valencia3chValue, BossHP, 0));
                            
                            kz_ma3 = ValueConverter(2, "Magoria");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(3, new BossChannelMap(Kz_Balenos4chValue, Kz_Serendia4chValue, Kz_Calpheon4chValue, Kz_Mediah4chValue, Kz_Valencia4chValue, BossHP, 0));
                            
                            kz_ma4 = ValueConverter(3, "Magoria");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        
                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k") //カーマスリビアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(0, new BossChannelMap(Kz_Balenos1chValue, Kz_Serendia1chValue, Kz_Calpheon1chValue, Kz_Mediah1chValue, Kz_Valencia1chValue, Kz_Magoria1chValue, BossHP));
                            
                            kz_k1 = ValueConverter(0, "Kamasylvia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(1, new BossChannelMap(Kz_Balenos2chValue, Kz_Serendia2chValue, Kz_Calpheon2chValue, Kz_Mediah2chValue, Kz_Valencia2chValue, Kz_Magoria2chValue, BossHP));
                            
                            kz_k2 = ValueConverter(1, "Kamasylvia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + kz_b1 + SPAN + "2ch：" + kz_b2 + SPAN + "3ch：" + kz_b3 + SPAN + "4ch：" + kz_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + kz_s1 + SPAN + "2ch：" + kz_s2 + SPAN + "3ch：" + kz_s3 + SPAN + "4ch：" + kz_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + kz_c1 + SPAN + "2ch：" + kz_c2 + SPAN + "3ch：" + kz_c3 + SPAN + "4ch：" + kz_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + kz_m1 + SPAN + "2ch：" + kz_m2 + SPAN + "3ch：" + kz_m3 + SPAN + "4ch：" + kz_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + kz_v1 + SPAN + "2ch：" + kz_v2 + SPAN + "3ch：" + kz_v3 + SPAN + "4ch：" + kz_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + kz_ma1 + SPAN + "2ch：" + kz_ma2 + SPAN + "3ch：" + kz_ma3 + SPAN + "4ch：" + kz_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + kz_k1 + SPAN + "2ch：" + kz_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
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
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(BossHP, Ka_Serendia2chValue, Ka_Calpheon2chValue, Ka_Mediah2chValue, Ka_Valencia2chValue, Ka_Magoria2chValue, Ka_Kms2chValue));
                            ka_b2 = ValueConverter(5, "Balenos");
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(BossHP, Kz_Serendia3chValue, Kz_Calpheon3chValue, Kz_Mediah3chValue, Kz_Valencia3chValue, Kz_Magoria3chValue, 0));
                            ka_b3 = ValueConverter(6, "Balenos");
                            
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(BossHP, Kz_Serendia4chValue, Kz_Calpheon4chValue, Kz_Mediah4chValue, Kz_Valencia4chValue, Kz_Magoria4chValue, 0));
                            ka_b4 = ValueConverter(7, "Balenos");
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                            LatestBossStatus = return_status;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, BossHP, Ka_Calpheon1chValue, Ka_Mediah1chValue, Ka_Valencia1chValue, Ka_Magoria1chValue, Ka_Kms1chValue));
                            
                            ka_s1 = ValueConverter(4, "Serendia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, BossHP, Ka_Calpheon2chValue, Ka_Mediah2chValue, Ka_Valencia2chValue, Ka_Magoria2chValue, Ka_Kms2chValue));
                            
                            ka_s2 = ValueConverter(5, "Serendia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, BossHP, Ka_Calpheon3chValue, Ka_Mediah3chValue, Ka_Valencia3chValue, Ka_Magoria3chValue, 0));
                            
                            ka_s3 = ValueConverter(6, "Serendia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, BossHP, Ka_Calpheon4chValue, Ka_Mediah4chValue, Ka_Valencia4chValue, Ka_Magoria4chValue, 0));
                            
                            kz_s4 = ValueConverter(7, "Serendia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, BossHP, Ka_Mediah1chValue, Ka_Valencia1chValue, Ka_Magoria1chValue, Ka_Kms1chValue));
                            
                            ka_c1 = ValueConverter(4, "Calpheon");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        //if (BossChannel.Contains("1") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, BossHP, Ka_Mediah2chValue, Ka_Valencia2chValue, Ka_Magoria2chValue, Ka_Kms2chValue));
                            
                            ka_c2 = ValueConverter(5, "Calpheon");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        //if (BossChannel.Contains("2") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, Ka_Serendia3chValue, BossHP, Ka_Mediah3chValue, Ka_Valencia3chValue, Ka_Magoria3chValue, 0));
                            
                            ka_c3 = ValueConverter(6, "Calpheon");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        //if (BossChannel.Contains("3") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, Ka_Serendia4chValue, BossHP, Ka_Mediah4chValue, Ka_Valencia4chValue, Ka_Magoria4chValue, 0));
                            
                            ka_c4 = ValueConverter(7, "Calpheon");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 1) == "m")   //メディアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, Ka_Calpheon1chValue, BossHP, Ka_Valencia1chValue, Ka_Magoria1chValue, Ka_Kms1chValue));
                            
                            ka_m1 = ValueConverter(4, "Mediah");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, Ka_Calpheon2chValue, BossHP, Ka_Valencia2chValue, Ka_Magoria2chValue, Ka_Kms2chValue));
                            
                            ka_m2 = ValueConverter(5, "Mediah");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, Ka_Serendia3chValue, Ka_Calpheon3chValue, BossHP, Ka_Valencia3chValue, Ka_Magoria3chValue, 0));
                            
                            ka_m3 = ValueConverter(6, "Mediah");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, Ka_Serendia4chValue, Ka_Calpheon4chValue, BossHP, Ka_Valencia4chValue, Ka_Magoria4chValue, 0));
                            
                            ka_m4 = ValueConverter(7, "Mediah");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }

                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v") //Valencia
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, Ka_Calpheon1chValue, Ka_Mediah1chValue, BossHP, Ka_Magoria1chValue, Ka_Kms1chValue));
                            
                            ka_v1 = ValueConverter(4, "Valencia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, Ka_Calpheon2chValue, Ka_Mediah2chValue, BossHP, Ka_Magoria2chValue, Ka_Kms2chValue));
                            
                            ka_v2 = ValueConverter(5, "Valencia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, Ka_Serendia3chValue, Ka_Calpheon3chValue, Ka_Mediah3chValue, BossHP, Kz_Magoria3chValue, 0));
                            
                            ka_v3 = ValueConverter(6, "Valencia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, Ka_Serendia4chValue, Ka_Calpheon4chValue, Ka_Mediah4chValue, BossHP, Ka_Magoria4chValue, 0));
                            
                            ka_v4 = ValueConverter(7, "Valencia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }

                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma") //マゴリアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, Ka_Calpheon1chValue, Ka_Mediah1chValue, Ka_Valencia1chValue, BossHP, Ka_Kms1chValue));
                            
                            ka_ma1 = ValueConverter(4, "Magoria");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, Ka_Calpheon2chValue, Ka_Mediah2chValue, Ka_Valencia2chValue, BossHP, Ka_Kms2chValue));
                            
                            ka_ma2 = ValueConverter(5, "Magoria");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(6, new BossChannelMap(Ka_Balenos3chValue, Ka_Serendia3chValue, Ka_Calpheon3chValue, Ka_Mediah3chValue, Ka_Valencia3chValue, BossHP, 0));
                            
                            ka_ma3 = ValueConverter(6, "Magoria");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(7, new BossChannelMap(Ka_Balenos4chValue, Ka_Serendia4chValue, Ka_Calpheon4chValue, Ka_Mediah4chValue, Ka_Valencia4chValue, BossHP, 0));
                            
                            ka_ma4 = ValueConverter(7, "Magoria");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }

                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k") //カーマスリビアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(4, new BossChannelMap(Ka_Balenos1chValue, Ka_Serendia1chValue, Ka_Calpheon1chValue, Ka_Mediah1chValue, Ka_Valencia1chValue, Ka_Magoria1chValue, BossHP));
                            
                            ka_k1 = ValueConverter(4, "Kamasylvia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(5, new BossChannelMap(Ka_Balenos2chValue, Ka_Serendia2chValue, Ka_Calpheon2chValue, Ka_Mediah2chValue, Ka_Valencia2chValue, Ka_Magoria2chValue, BossHP));
                            
                            ka_k2 = ValueConverter(5, "Kamasylvia");
                            
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ka_b1 + SPAN + "2ch：" + ka_b2 + SPAN + "3ch：" + ka_b3 + SPAN + "4ch：" + ka_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ka_s1 + SPAN + "2ch：" + ka_s2 + SPAN + "3ch：" + ka_s3 + SPAN + "4ch：" + ka_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ka_c1 + SPAN + "2ch：" + ka_c2 + SPAN + "3ch：" + ka_c3 + SPAN + "4ch：" + ka_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ka_m1 + SPAN + "2ch：" + ka_m2 + SPAN + "3ch：" + ka_m3 + SPAN + "4ch：" + ka_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ka_v1 + SPAN + "2ch：" + ka_v2 + SPAN + "3ch：" + ka_v3 + SPAN + "4ch：" + ka_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ka_ma1 + SPAN + "2ch：" + ka_ma2 + SPAN + "3ch：" + ka_ma3 + SPAN + "4ch：" + ka_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ka_k1 + SPAN + "2ch：" + ka_k2 + SPAN;
                            return_status =  IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia ;
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }

                    } //カーマスリビアch
                    break;
                case 3: //ヌーベル
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(8, new BossChannelMap(BossHP, Nv_Serendia1chValue, Nv_Calpheon1chValue, Nv_Mediah1chValue, Nv_Valencia1chValue, Nv_Magoria1chValue, Nv_Kms1chValue));
                            nv_b1 = ValueConverter(8, "Balenos");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(9, new BossChannelMap(BossHP, Nv_Serendia2chValue, Nv_Calpheon2chValue, Nv_Mediah2chValue, Nv_Valencia2chValue, Nv_Magoria2chValue, Nv_Kms2chValue));
                            nv_b2 = ValueConverter(9, "Balenos");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(10, new BossChannelMap(BossHP, Nv_Serendia3chValue, Nv_Calpheon3chValue, Nv_Mediah3chValue, Nv_Valencia3chValue, Nv_Magoria3chValue, 0));
                            nv_b3 = ValueConverter(10, "Balenos");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(11, new BossChannelMap(BossHP, Nv_Serendia4chValue, Nv_Calpheon4chValue, Nv_Mediah4chValue, Nv_Valencia4chValue, Nv_Magoria4chValue, 0));
                            nv_b4 = ValueConverter(11, "Balenos");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(8, new BossChannelMap(Nv_Balenos1chValue, BossHP, Nv_Calpheon1chValue, Nv_Mediah1chValue, Nv_Valencia1chValue, Nv_Magoria1chValue, Nv_Kms1chValue));
                            nv_s1 = ValueConverter(8, "Serendia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(9, new BossChannelMap(Nv_Balenos2chValue, BossHP, Nv_Calpheon2chValue, Nv_Mediah2chValue, Nv_Valencia2chValue, Nv_Magoria2chValue, Nv_Kms2chValue));
                            nv_s2 = ValueConverter(9, "Serendia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(10, new BossChannelMap(Nv_Balenos3chValue, BossHP, Nv_Calpheon3chValue, Nv_Mediah3chValue, Nv_Valencia3chValue, Nv_Magoria3chValue, 0));
                            nv_s3 = ValueConverter(10, "Serendia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(11, new BossChannelMap(Nv_Balenos4chValue, BossHP, Nv_Calpheon4chValue, Nv_Mediah4chValue, Nv_Valencia4chValue, Nv_Magoria4chValue, 0));
                            nv_s4 = ValueConverter(11, "Serendia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(8, new BossChannelMap(Nv_Balenos1chValue, Nv_Serendia1chValue, BossHP, Nv_Mediah1chValue, Nv_Valencia1chValue, Nv_Magoria1chValue, Nv_Kms1chValue));
                            nv_c1 = ValueConverter(8, "Calpheon");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(9, new BossChannelMap(Nv_Balenos2chValue, Nv_Serendia2chValue, BossHP, Nv_Mediah2chValue, Nv_Valencia2chValue, Nv_Magoria2chValue, Nv_Kms2chValue));
                            nv_c2 = ValueConverter(9, "Calpheon");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(10, new BossChannelMap(Nv_Balenos3chValue, Nv_Serendia3chValue, BossHP, Nv_Mediah3chValue, Nv_Valencia3chValue, Nv_Magoria3chValue, 0));
                            nv_c3 = ValueConverter(10, "Calpheon");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(11, new BossChannelMap(Nv_Balenos4chValue, Nv_Serendia4chValue, BossHP, Nv_Mediah4chValue, Nv_Valencia4chValue, Nv_Magoria4chValue, 0));
                            nv_c4 = ValueConverter(11, "Calpheon");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 1) == "m")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(8, new BossChannelMap(Nv_Balenos1chValue, Nv_Serendia1chValue, Nv_Calpheon1chValue, BossHP, Nv_Valencia1chValue, Nv_Magoria1chValue, Nv_Kms1chValue));
                            nv_m1 = ValueConverter(8, "Mediah");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(9, new BossChannelMap(Nv_Balenos2chValue, Nv_Serendia2chValue, Nv_Calpheon2chValue, BossHP, Nv_Valencia2chValue, Nv_Magoria2chValue, Nv_Kms2chValue));
                            nv_m2 = ValueConverter(9, "Mediah");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(10, new BossChannelMap(Nv_Balenos3chValue, Nv_Serendia3chValue, Nv_Calpheon3chValue, BossHP, Nv_Valencia3chValue, Nv_Magoria3chValue, 0));
                            nv_m3 = ValueConverter(10, "Mediah");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(11, new BossChannelMap(Nv_Balenos4chValue, Nv_Serendia4chValue, Nv_Calpheon4chValue, BossHP, Nv_Valencia4chValue, Nv_Magoria4chValue, 0));
                            nv_m4 = ValueConverter(11, "Mediah");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(8, new BossChannelMap(Nv_Balenos1chValue, Nv_Serendia1chValue, Nv_Calpheon1chValue, Nv_Mediah1chValue, BossHP, Nv_Magoria1chValue, Nv_Kms1chValue));
                            nv_v1 = ValueConverter(8, "Valencia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(9, new BossChannelMap(Nv_Balenos2chValue, Nv_Serendia2chValue, Nv_Calpheon2chValue, Nv_Mediah2chValue, BossHP, Nv_Magoria2chValue, Nv_Kms2chValue));
                            nv_v2 = ValueConverter(9, "Valencia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(10, new BossChannelMap(Nv_Balenos3chValue, Nv_Serendia3chValue, Nv_Calpheon3chValue, Nv_Mediah3chValue, BossHP, Nv_Magoria3chValue, 0));
                            nv_v3 = ValueConverter(10, "Valencia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(11, new BossChannelMap(Nv_Balenos4chValue, Nv_Serendia4chValue, Nv_Calpheon4chValue, Nv_Mediah4chValue, BossHP, Nv_Magoria4chValue, 0));
                            nv_v4 = ValueConverter(11, "Valencia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(8, new BossChannelMap(Nv_Balenos1chValue, Nv_Serendia1chValue, Nv_Calpheon1chValue, Nv_Mediah1chValue, Nv_Valencia1chValue, BossHP, Nv_Kms1chValue));
                            nv_ma1 = ValueConverter(8, "Magoria");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(9, new BossChannelMap(Nv_Balenos2chValue, Nv_Serendia2chValue, Nv_Calpheon2chValue, Nv_Mediah2chValue, Nv_Valencia2chValue, BossHP, Nv_Kms2chValue));
                            nv_ma2 = ValueConverter(9, "Magoria");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(10, new BossChannelMap(Nv_Balenos3chValue, Nv_Serendia3chValue, Nv_Calpheon3chValue, Nv_Mediah3chValue, Nv_Valencia3chValue, BossHP, 0));
                            nv_ma3 = ValueConverter(10, "Magoria");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(11, new BossChannelMap(Nv_Balenos4chValue, Nv_Serendia4chValue, Nv_Calpheon4chValue, Nv_Mediah4chValue, Nv_Valencia4chValue, BossHP, 0));
                            nv_ma4 = ValueConverter(11, "Magoria");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(8, new BossChannelMap(Nv_Balenos1chValue, Nv_Serendia1chValue, Nv_Calpheon1chValue, Nv_Mediah1chValue, Nv_Valencia1chValue, Nv_Magoria1chValue, BossHP));
                            nv_k1 = ValueConverter(8, "Kamasylvia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(9, new BossChannelMap(Nv_Balenos2chValue, Nv_Serendia2chValue, Nv_Calpheon2chValue, Nv_Mediah2chValue, Nv_Valencia2chValue, Nv_Magoria2chValue, BossHP));
                            nv_k2 = ValueConverter(9, "Kamasylvia");
                            nv_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + nv_b1 + SPAN + "2ch：" + nv_b2 + SPAN + "3ch：" + nv_b3 + SPAN + "4ch：" + nv_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + nv_s1 + SPAN + "2ch：" + nv_s2 + SPAN + "3ch：" + nv_s3 + SPAN + "4ch：" + nv_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + nv_c1 + SPAN + "2ch：" + nv_c2 + SPAN + "3ch：" + nv_c3 + SPAN + "4ch：" + nv_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + nv_m1 + SPAN + "2ch：" + nv_m2 + SPAN + "3ch：" + nv_m3 + SPAN + "4ch：" + nv_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + nv_v1 + SPAN + "2ch：" + nv_v2 + SPAN + "3ch：" + nv_v3 + SPAN + "4ch：" + nv_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + nv_ma1 + SPAN + "2ch：" + nv_ma2 + SPAN + "3ch：" + nv_ma3 + SPAN + "4ch：" + nv_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + nv_k1 + SPAN + "2ch：" + nv_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    break;
                case 4: //クツム
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(12, new BossChannelMap(BossHP, Ku_Serendia1chValue, Ku_Calpheon1chValue, Ku_Mediah1chValue, Ku_Valencia1chValue, Ku_Magoria1chValue, Ku_Kms1chValue));
                            ku_b1 = ValueConverter(12, "Balenos");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(13, new BossChannelMap(BossHP, Ku_Serendia2chValue, Ku_Calpheon2chValue, Ku_Mediah2chValue, Ku_Valencia2chValue, Ku_Magoria2chValue, Ku_Kms2chValue));
                            ku_b2 = ValueConverter(13, "Balenos");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(14, new BossChannelMap(BossHP, Ku_Serendia3chValue, Ku_Calpheon3chValue, Ku_Mediah3chValue, Ku_Valencia3chValue, Ku_Magoria3chValue, 0));
                            ku_b3 = ValueConverter(14, "Balenos");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(15, new BossChannelMap(BossHP, Ku_Serendia4chValue, Ku_Calpheon4chValue, Ku_Mediah4chValue, Ku_Valencia4chValue, Ku_Magoria4chValue, 0));
                            ku_b4 = ValueConverter(15, "Balenos");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(12, new BossChannelMap(Ku_Balenos1chValue, BossHP, Ku_Calpheon1chValue, Ku_Mediah1chValue, Ku_Valencia1chValue, Ku_Magoria1chValue, Ku_Kms1chValue));
                            ku_s1 = ValueConverter(12, "Serendia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(13, new BossChannelMap(Ku_Balenos2chValue, BossHP, Ku_Calpheon2chValue, Ku_Mediah2chValue, Ku_Valencia2chValue, Ku_Magoria2chValue, Ku_Kms2chValue));
                            ku_s2 = ValueConverter(13, "Serendia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(14, new BossChannelMap(Ku_Balenos3chValue, BossHP, Ku_Calpheon3chValue, Ku_Mediah3chValue, Ku_Valencia3chValue, Ku_Magoria3chValue, 0));
                            ku_s3 = ValueConverter(14, "Serendia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(15, new BossChannelMap(Ku_Balenos4chValue, BossHP, Ku_Calpheon4chValue, Ku_Mediah4chValue, Ku_Valencia4chValue, Ku_Magoria4chValue, 0));
                            ku_s4 = ValueConverter(15, "Serendia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(12, new BossChannelMap(Ku_Balenos1chValue, Ku_Serendia1chValue, BossHP, Ku_Mediah1chValue, Ku_Valencia1chValue, Ku_Magoria1chValue, Ku_Kms1chValue));
                            ku_c1 = ValueConverter(12, "Calpheon");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(13, new BossChannelMap(Ku_Balenos2chValue, Ku_Serendia2chValue, BossHP, Ku_Mediah2chValue, Ku_Valencia2chValue, Ku_Magoria2chValue, Ku_Kms2chValue));
                            ku_c2 = ValueConverter(13, "Calpheon");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(14, new BossChannelMap(Ku_Balenos3chValue, Ku_Serendia3chValue, BossHP, Ku_Mediah3chValue, Ku_Valencia3chValue, Ku_Magoria3chValue, 0));
                            ku_c3 = ValueConverter(14, "Calpheon");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(15, new BossChannelMap(Ku_Balenos4chValue, Ku_Serendia4chValue, BossHP, Ku_Mediah4chValue, Ku_Valencia4chValue, Ku_Magoria4chValue, 0));
                            ku_c4 = ValueConverter(15, "Calpheon");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 1) == "m")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(12, new BossChannelMap(Ku_Balenos1chValue, Ku_Serendia1chValue, Ku_Calpheon1chValue, BossHP, Ku_Valencia1chValue, Ku_Magoria1chValue, Ku_Kms1chValue));
                            ku_m1 = ValueConverter(12, "Mediah");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(13, new BossChannelMap(Ku_Balenos2chValue, Ku_Serendia2chValue, Ku_Calpheon2chValue, BossHP, Ku_Valencia2chValue, Ku_Magoria2chValue, Ku_Kms2chValue));
                            ku_m2 = ValueConverter(13, "Mediah");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(14, new BossChannelMap(Ku_Balenos3chValue, Ku_Serendia3chValue, Ku_Calpheon3chValue, BossHP, Ku_Valencia3chValue, Ku_Magoria3chValue, 0));
                            ku_m3 = ValueConverter(14, "Mediah");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(15, new BossChannelMap(Ku_Balenos4chValue, Ku_Serendia4chValue, Ku_Calpheon4chValue, BossHP, Ku_Valencia4chValue, Ku_Magoria4chValue, 0));
                            ku_m4 = ValueConverter(15, "Mediah");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(12, new BossChannelMap(Ku_Balenos1chValue, Ku_Serendia1chValue, Ku_Calpheon1chValue, Ku_Mediah1chValue, BossHP, Ku_Magoria1chValue, Ku_Kms1chValue));
                            ku_v1 = ValueConverter(12, "Valencia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(13, new BossChannelMap(Ku_Balenos2chValue, Ku_Serendia2chValue, Ku_Calpheon2chValue, Ku_Mediah2chValue, BossHP, Ku_Magoria2chValue, Ku_Kms2chValue));
                            ku_v2 = ValueConverter(13, "Valencia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(14, new BossChannelMap(Ku_Balenos3chValue, Ku_Serendia3chValue, Ku_Calpheon3chValue, Ku_Mediah3chValue, BossHP, Ku_Magoria3chValue, 0));
                            ku_v3 = ValueConverter(14, "Valencia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(15, new BossChannelMap(Ku_Balenos4chValue, Ku_Serendia4chValue, Ku_Calpheon4chValue, Ku_Mediah4chValue, BossHP, Ku_Magoria4chValue, 0));
                            ku_v4 = ValueConverter(15, "Valencia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(12, new BossChannelMap(Ku_Balenos1chValue, Ku_Serendia1chValue, Ku_Calpheon1chValue, Ku_Mediah1chValue, Ku_Valencia1chValue, BossHP, Ku_Kms1chValue));
                            ku_ma1 = ValueConverter(12, "Magoria");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(13, new BossChannelMap(Ku_Balenos2chValue, Ku_Serendia2chValue, Ku_Calpheon2chValue, Ku_Mediah2chValue, Ku_Valencia2chValue, BossHP, Ku_Kms2chValue));
                            ku_ma2 = ValueConverter(13, "Magoria");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(14, new BossChannelMap(Ku_Balenos3chValue, Ku_Serendia3chValue, Ku_Calpheon3chValue, Ku_Mediah3chValue, Ku_Valencia3chValue, BossHP, 0));
                            ku_ma3 = ValueConverter(14, "Magoria");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(15, new BossChannelMap(Ku_Balenos4chValue, Ku_Serendia4chValue, Ku_Calpheon4chValue, Ku_Mediah4chValue, Ku_Valencia4chValue, BossHP, 0));
                            ku_ma4 = ValueConverter(15, "Magoria");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(12, new BossChannelMap(Ku_Balenos1chValue, Ku_Serendia1chValue, Ku_Calpheon1chValue, Ku_Mediah1chValue, Ku_Valencia1chValue, Ku_Magoria1chValue, BossHP));
                            ku_k1 = ValueConverter(12, "Kamasylvia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(13, new BossChannelMap(Ku_Balenos2chValue, Ku_Serendia2chValue, Ku_Calpheon2chValue, Ku_Mediah2chValue, Ku_Valencia2chValue, Ku_Magoria2chValue, BossHP));
                            ku_k2 = ValueConverter(13, "Kamasylvia");
                            ku_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + ku_b1 + SPAN + "2ch：" + ku_b2 + SPAN + "3ch：" + ku_b3 + SPAN + "4ch：" + ku_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + ku_s1 + SPAN + "2ch：" + ku_s2 + SPAN + "3ch：" + ku_s3 + SPAN + "4ch：" + ku_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + ku_c1 + SPAN + "2ch：" + ku_c2 + SPAN + "3ch：" + ku_c3 + SPAN + "4ch：" + ku_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + ku_m1 + SPAN + "2ch：" + ku_m2 + SPAN + "3ch：" + ku_m3 + SPAN + "4ch：" + ku_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + ku_v1 + SPAN + "2ch：" + ku_v2 + SPAN + "3ch：" + ku_v3 + SPAN + "4ch：" + ku_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + ku_ma1 + SPAN + "2ch：" + ku_ma2 + SPAN + "3ch：" + ku_ma3 + SPAN + "4ch：" + ku_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ku_k1 + SPAN + "2ch：" + ku_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    } //カーマスリビアch
                    break;
                case 5: //レッドノーズ
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(16, new BossChannelMap(BossHP, Rn_Serendia1chValue, Rn_Calpheon1chValue, Rn_Mediah1chValue, Rn_Valencia1chValue, Rn_Magoria1chValue, Rn_Kms1chValue));
                            rn_b1 = ValueConverter(16, "Balenos");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(17, new BossChannelMap(BossHP, Rn_Serendia2chValue, Rn_Calpheon2chValue, Rn_Mediah2chValue, Rn_Valencia2chValue, Rn_Magoria2chValue, Rn_Kms2chValue));
                            rn_b2 = ValueConverter(17, "Balenos");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(18, new BossChannelMap(BossHP, Rn_Serendia3chValue, Rn_Calpheon3chValue, Rn_Mediah3chValue, Rn_Valencia3chValue, Rn_Magoria3chValue, 0));
                            rn_b3 = ValueConverter(18, "Balenos");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(19, new BossChannelMap(BossHP, Rn_Serendia4chValue, Rn_Calpheon4chValue, Rn_Mediah4chValue, Rn_Valencia4chValue, Rn_Magoria4chValue, 0));
                            rn_b4 = ValueConverter(19, "Balenos");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    } //バレノス
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(16, new BossChannelMap(Rn_Balenos1chValue, BossHP, Rn_Calpheon1chValue, Rn_Mediah1chValue, Rn_Valencia1chValue, Rn_Magoria1chValue, Rn_Kms1chValue));
                            rn_s1 = ValueConverter(16, "Serendia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(17, new BossChannelMap(Rn_Balenos2chValue, BossHP, Rn_Calpheon2chValue, Rn_Mediah2chValue, Rn_Valencia2chValue, Rn_Magoria2chValue, Rn_Kms2chValue));
                            rn_s2 = ValueConverter(17, "Serendia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(18, new BossChannelMap(Rn_Balenos3chValue, BossHP, Rn_Calpheon3chValue, Rn_Mediah3chValue, Rn_Valencia3chValue, Rn_Magoria3chValue, 0));
                            rn_s3 = ValueConverter(18, "Serendia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(19, new BossChannelMap(Rn_Balenos4chValue, BossHP, Rn_Calpheon4chValue, Rn_Mediah4chValue, Rn_Valencia4chValue, Rn_Magoria4chValue, 0));
                            rn_s4 = ValueConverter(19, "Serendia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(16, new BossChannelMap(Rn_Balenos1chValue, Rn_Serendia1chValue, BossHP, Rn_Mediah1chValue, Rn_Valencia1chValue, Rn_Magoria1chValue, Rn_Kms1chValue));
                            rn_c1 = ValueConverter(16, "Calpheon");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(17, new BossChannelMap(Rn_Balenos2chValue, Rn_Serendia2chValue, BossHP, Rn_Mediah2chValue, Rn_Valencia2chValue, Rn_Magoria2chValue, Rn_Kms2chValue));
                            rn_c2 = ValueConverter(17, "Calpheon");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(18, new BossChannelMap(Rn_Balenos3chValue, Rn_Serendia3chValue, BossHP, Rn_Mediah3chValue, Rn_Valencia3chValue, Rn_Magoria3chValue, 0));
                            rn_c3 = ValueConverter(18, "Calpheon");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(19, new BossChannelMap(Rn_Balenos4chValue, Rn_Serendia4chValue, BossHP, Rn_Mediah4chValue, Rn_Valencia4chValue, Rn_Magoria4chValue, 0));
                            rn_c4 = ValueConverter(19, "Calpheon");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "m")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(16, new BossChannelMap(Rn_Balenos1chValue, Rn_Serendia1chValue, Rn_Calpheon1chValue, BossHP, Rn_Valencia1chValue, Rn_Magoria1chValue, Rn_Kms1chValue));
                            rn_m1 = ValueConverter(16, "Mediah");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(17, new BossChannelMap(Rn_Balenos2chValue, Rn_Serendia2chValue, Rn_Calpheon2chValue, BossHP, Rn_Valencia2chValue, Rn_Magoria2chValue, Rn_Kms2chValue));
                            rn_m2 = ValueConverter(17, "Mediah");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(18, new BossChannelMap(Rn_Balenos3chValue, Rn_Serendia3chValue, Rn_Calpheon3chValue, BossHP, Rn_Valencia3chValue, Rn_Magoria3chValue, 0));
                            rn_m3 = ValueConverter(18, "Mediah");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(19, new BossChannelMap(Rn_Balenos4chValue, Rn_Serendia4chValue, Rn_Calpheon4chValue, BossHP, Rn_Valencia4chValue, Rn_Magoria4chValue, 0));
                            rn_m4 = ValueConverter(19, "Mediah");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(16, new BossChannelMap(Rn_Balenos1chValue, Rn_Serendia1chValue, Rn_Calpheon1chValue, Rn_Mediah1chValue, BossHP, Rn_Magoria1chValue, Rn_Kms1chValue));
                            rn_v1 = ValueConverter(16, "Valencia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(17, new BossChannelMap(Rn_Balenos2chValue, Rn_Serendia2chValue, Rn_Calpheon2chValue, Rn_Mediah2chValue, BossHP, Rn_Magoria2chValue, Rn_Kms2chValue));
                            rn_v2 = ValueConverter(17, "Valencia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(18, new BossChannelMap(Rn_Balenos3chValue, Rn_Serendia3chValue, Rn_Calpheon3chValue, Rn_Mediah3chValue, BossHP, Rn_Magoria3chValue, 0));
                            rn_v3 = ValueConverter(18, "Valencia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(19, new BossChannelMap(Rn_Balenos4chValue, Rn_Serendia4chValue, Rn_Calpheon4chValue, Rn_Mediah4chValue, BossHP, Rn_Magoria4chValue, 0));
                            rn_v4 = ValueConverter(19, "Valencia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(16, new BossChannelMap(Rn_Balenos1chValue, Rn_Serendia1chValue, Rn_Calpheon1chValue, Rn_Mediah1chValue, Rn_Valencia1chValue, BossHP, Rn_Kms1chValue));
                            rn_ma1 = ValueConverter(16, "Magoria");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(17, new BossChannelMap(Rn_Balenos2chValue, Rn_Serendia2chValue, Rn_Calpheon2chValue, Rn_Mediah2chValue, Rn_Valencia2chValue, BossHP, Rn_Kms2chValue));
                            rn_ma2 = ValueConverter(17, "Magoria");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(18, new BossChannelMap(Rn_Balenos3chValue, Rn_Serendia3chValue, Rn_Calpheon3chValue, Rn_Mediah3chValue, Rn_Valencia3chValue, BossHP, 0));
                            rn_ma3 = ValueConverter(18, "Magoria");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(19, new BossChannelMap(Rn_Balenos4chValue, Rn_Serendia4chValue, Rn_Calpheon4chValue, Rn_Mediah4chValue, Rn_Valencia4chValue, BossHP, 0));
                            rn_ma4 = ValueConverter(19, "Magoria");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(16, new BossChannelMap(Rn_Balenos1chValue, Rn_Serendia1chValue, Rn_Calpheon1chValue, Rn_Mediah1chValue, Rn_Valencia1chValue, Rn_Magoria1chValue, BossHP));
                            rn_k1 = ValueConverter(16, "Kamasylvia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(17, new BossChannelMap(Rn_Balenos2chValue, Rn_Serendia2chValue, Rn_Calpheon2chValue, Rn_Mediah2chValue, Rn_Valencia2chValue, Rn_Magoria2chValue, BossHP));
                            rn_k2 = ValueConverter(17, "Kamasylvia");
                            rn_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + rn_b1 + SPAN + "2ch：" + rn_b2 + SPAN + "3ch：" + rn_b3 + SPAN + "4ch：" + rn_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + rn_s1 + SPAN + "2ch：" + rn_s2 + SPAN + "3ch：" + rn_s3 + SPAN + "4ch：" + rn_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + rn_c1 + SPAN + "2ch：" + rn_c2 + SPAN + "3ch：" + rn_c3 + SPAN + "4ch：" + rn_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + rn_m1 + SPAN + "2ch：" + rn_m2 + SPAN + "3ch：" + rn_m3 + SPAN + "4ch：" + rn_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + rn_v1 + SPAN + "2ch：" + rn_v2 + SPAN + "3ch：" + rn_v3 + SPAN + "4ch：" + rn_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + rn_ma1 + SPAN + "2ch：" + rn_ma2 + SPAN + "3ch：" + rn_ma3 + SPAN + "4ch：" + rn_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + rn_k1 + SPAN + "2ch：" + rn_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    break;
                case 6: //ベグ
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(20, new BossChannelMap(BossHP, Bh_Serendia1chValue, Bh_Calpheon1chValue, Bh_Mediah1chValue, Bh_Valencia1chValue, Bh_Magoria1chValue, Bh_Kms1chValue));
                            bh_b1 = ValueConverter(20, "Balenos");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(21, new BossChannelMap(BossHP, Bh_Serendia2chValue, Bh_Calpheon2chValue, Bh_Mediah2chValue, Bh_Valencia2chValue, Bh_Magoria2chValue, Bh_Kms2chValue));
                            bh_b2 = ValueConverter(21, "Balenos");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(22, new BossChannelMap(BossHP, Bh_Serendia3chValue, Bh_Calpheon3chValue, Bh_Mediah3chValue, Bh_Valencia3chValue, Bh_Magoria3chValue, 0));
                            bh_b3 = ValueConverter(22, "Balenos");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(23, new BossChannelMap(BossHP, Bh_Serendia4chValue, Bh_Calpheon4chValue, Bh_Mediah4chValue, Bh_Valencia4chValue, Bh_Magoria4chValue, 0));
                            bh_b4 = ValueConverter(23, "Balenos");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(20, new BossChannelMap(Bh_Balenos1chValue, BossHP, Bh_Calpheon1chValue, Bh_Mediah1chValue, Bh_Valencia1chValue, Bh_Magoria1chValue, Bh_Kms1chValue));
                            bh_s1 = ValueConverter(20, "Serendia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(21, new BossChannelMap(Bh_Balenos2chValue, BossHP, Bh_Calpheon2chValue, Bh_Mediah2chValue, Bh_Valencia2chValue, Bh_Magoria2chValue, Bh_Kms2chValue));
                            bh_s2 = ValueConverter(21, "Serendia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(22, new BossChannelMap(Bh_Balenos3chValue, BossHP, Bh_Calpheon3chValue, Bh_Mediah3chValue, Bh_Valencia3chValue, Bh_Magoria3chValue, 0));
                            bh_s3 = ValueConverter(22, "Serendia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(23, new BossChannelMap(Bh_Balenos4chValue, BossHP, Bh_Calpheon4chValue, Bh_Mediah4chValue, Bh_Valencia4chValue, Bh_Magoria4chValue, 0));
                            bh_s4 = ValueConverter(23, "Serendia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(20, new BossChannelMap(Bh_Balenos1chValue, Bh_Serendia1chValue, BossHP, Bh_Mediah1chValue, Bh_Valencia1chValue, Bh_Magoria1chValue, Bh_Kms1chValue));
                            bh_c1 = ValueConverter(20, "Calpheon");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(21, new BossChannelMap(Bh_Balenos2chValue, Bh_Serendia2chValue, BossHP, Bh_Mediah2chValue, Bh_Valencia2chValue, Bh_Magoria2chValue, Bh_Kms2chValue));
                            bh_c2 = ValueConverter(21, "Calpheon");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(22, new BossChannelMap(Bh_Balenos3chValue, Bh_Serendia3chValue, BossHP, Bh_Mediah3chValue, Bh_Valencia3chValue, Bh_Magoria3chValue, 0));
                            bh_c3 = ValueConverter(22, "Calpheon");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(23, new BossChannelMap(Bh_Balenos4chValue, Bh_Serendia4chValue, BossHP, Bh_Mediah4chValue, Bh_Valencia4chValue, Bh_Magoria4chValue, 0));
                            bh_c4 = ValueConverter(23, "Calpheon");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "m")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(20, new BossChannelMap(Bh_Balenos1chValue, Bh_Serendia1chValue, Bh_Calpheon1chValue, BossHP, Bh_Valencia1chValue, Bh_Magoria1chValue, Bh_Kms1chValue));
                            bh_m1 = ValueConverter(20, "Mediah");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(21, new BossChannelMap(Bh_Balenos2chValue, Bh_Serendia2chValue, Bh_Calpheon2chValue, BossHP, Bh_Valencia2chValue, Bh_Magoria2chValue, Bh_Kms2chValue));
                            bh_m2 = ValueConverter(21, "Mediah");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(22, new BossChannelMap(Bh_Balenos3chValue, Bh_Serendia3chValue, Bh_Calpheon3chValue, BossHP, Bh_Valencia3chValue, Bh_Magoria3chValue, 0));
                            bh_m3 = ValueConverter(22, "Mediah");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(23, new BossChannelMap(Bh_Balenos4chValue, Bh_Serendia4chValue, Bh_Calpheon4chValue, BossHP, Bh_Valencia4chValue, Bh_Magoria4chValue, 0));
                            bh_m4 = ValueConverter(23, "Mediah");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(20, new BossChannelMap(Bh_Balenos1chValue, Bh_Serendia1chValue, Bh_Calpheon1chValue, Bh_Mediah1chValue, BossHP, Bh_Magoria1chValue, Bh_Kms1chValue));
                            bh_v1 = ValueConverter(20, "Valencia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(21, new BossChannelMap(Bh_Balenos2chValue, Bh_Serendia2chValue, Bh_Calpheon2chValue, Bh_Mediah2chValue, BossHP, Bh_Magoria2chValue, Bh_Kms2chValue));
                            bh_v2 = ValueConverter(21, "Valencia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(22, new BossChannelMap(Bh_Balenos3chValue, Bh_Serendia3chValue, Bh_Calpheon3chValue, Bh_Mediah3chValue, BossHP, Bh_Magoria3chValue, 0));
                            bh_v3 = ValueConverter(22, "Valencia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(23, new BossChannelMap(Bh_Balenos4chValue, Bh_Serendia4chValue, Bh_Calpheon4chValue, Bh_Mediah4chValue, BossHP, Bh_Magoria4chValue, 0));
                            bh_v4 = ValueConverter(23, "Valencia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(20, new BossChannelMap(Bh_Balenos1chValue, Bh_Serendia1chValue, Bh_Calpheon1chValue, Bh_Mediah1chValue, Bh_Valencia1chValue, BossHP, Bh_Kms1chValue));
                            bh_ma1 = ValueConverter(20, "Magoria");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(21, new BossChannelMap(Bh_Balenos2chValue, Bh_Serendia2chValue, Bh_Calpheon2chValue, Bh_Mediah2chValue, Bh_Valencia2chValue, BossHP, Bh_Kms2chValue));
                            bh_ma2 = ValueConverter(21, "Magoria");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(22, new BossChannelMap(Bh_Balenos3chValue, Bh_Serendia3chValue, Bh_Calpheon3chValue, Bh_Mediah3chValue, Bh_Valencia3chValue, BossHP, 0));
                            bh_ma3 = ValueConverter(22, "Magoria");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(23, new BossChannelMap(Bh_Balenos4chValue, Bh_Serendia4chValue, Bh_Calpheon4chValue, Bh_Mediah4chValue, Bh_Valencia4chValue, BossHP, 0));
                            bh_ma4 = ValueConverter(23, "Magoria");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(20, new BossChannelMap(Bh_Balenos1chValue, Bh_Serendia1chValue, Bh_Calpheon1chValue, Bh_Mediah1chValue, Bh_Valencia1chValue, Bh_Magoria1chValue, BossHP));
                            bh_k1 = ValueConverter(20, "Kamasylvia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(21, new BossChannelMap(Bh_Balenos2chValue, Bh_Serendia2chValue, Bh_Calpheon2chValue, Bh_Mediah2chValue, Bh_Valencia2chValue, Bh_Magoria2chValue, BossHP));
                            bh_k2 = ValueConverter(21, "Kamasylvia");
                            bh_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + bh_b1 + SPAN + "2ch：" + bh_b2 + SPAN + "3ch：" + bh_b3 + SPAN + "4ch：" + bh_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + bh_s1 + SPAN + "2ch：" + bh_s2 + SPAN + "3ch：" + bh_s3 + SPAN + "4ch：" + bh_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + bh_c1 + SPAN + "2ch：" + bh_c2 + SPAN + "3ch：" + bh_c3 + SPAN + "4ch：" + bh_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + bh_m1 + SPAN + "2ch：" + bh_m2 + SPAN + "3ch：" + bh_m3 + SPAN + "4ch：" + bh_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + bh_v1 + SPAN + "2ch：" + bh_v2 + SPAN + "3ch：" + bh_v3 + SPAN + "4ch：" + bh_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + bh_ma1 + SPAN + "2ch：" + bh_ma2 + SPAN + "3ch：" + bh_ma3 + SPAN + "4ch：" + bh_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + bh_k1 + SPAN + "2ch：" + bh_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    break;
                case 7: //愚鈍
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(24, new BossChannelMap(BossHP, Tr_Serendia1chValue, Tr_Calpheon1chValue, Tr_Mediah1chValue, Tr_Valencia1chValue, Tr_Magoria1chValue, Tr_Kms1chValue));
                            tree_b1 = ValueConverter(24, "Balenos");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(25, new BossChannelMap(BossHP, Tr_Serendia2chValue, Tr_Calpheon2chValue, Tr_Mediah2chValue, Tr_Valencia2chValue, Tr_Magoria2chValue, Tr_Kms2chValue));
                            tree_b2 = ValueConverter(25, "Balenos");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(26, new BossChannelMap(BossHP, Tr_Serendia3chValue, Tr_Calpheon3chValue, Tr_Mediah3chValue, Tr_Valencia3chValue, Tr_Magoria3chValue, 0));
                            tree_b3 = ValueConverter(26, "Balenos");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(27, new BossChannelMap(BossHP, Tr_Serendia4chValue, Tr_Calpheon4chValue, Tr_Mediah4chValue, Tr_Valencia4chValue, Tr_Magoria4chValue, 0));
                            tree_b4 = ValueConverter(27, "Balenos");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(24, new BossChannelMap(Tr_Balenos1chValue, BossHP, Tr_Calpheon1chValue, Tr_Mediah1chValue, Tr_Valencia1chValue, Tr_Magoria1chValue, Tr_Kms1chValue));
                            tree_s1 = ValueConverter(24, "Serendia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(25, new BossChannelMap(Tr_Balenos2chValue, BossHP, Tr_Calpheon2chValue, Tr_Mediah2chValue, Tr_Valencia2chValue, Tr_Magoria2chValue, Tr_Kms2chValue));
                            tree_s2 = ValueConverter(25, "Serendia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(26, new BossChannelMap(Tr_Balenos3chValue, BossHP, Tr_Calpheon3chValue, Tr_Mediah3chValue, Tr_Valencia3chValue, Tr_Magoria3chValue, 0));
                            tree_s3 = ValueConverter(26, "Serendia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(27, new BossChannelMap(Tr_Balenos4chValue, BossHP, Tr_Calpheon4chValue, Tr_Mediah4chValue, Tr_Valencia4chValue, Tr_Magoria4chValue, 0));
                            tree_s4 = ValueConverter(27, "Serendia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(24, new BossChannelMap(Tr_Balenos1chValue, Tr_Serendia1chValue, BossHP, Tr_Mediah1chValue, Tr_Valencia1chValue, Tr_Magoria1chValue, Tr_Kms1chValue));
                            tree_c1 = ValueConverter(24, "Calpheon");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(25, new BossChannelMap(Tr_Balenos2chValue, Tr_Serendia2chValue, BossHP, Tr_Mediah2chValue, Tr_Valencia2chValue, Tr_Magoria2chValue, Tr_Kms2chValue));
                            tree_c2 = ValueConverter(25, "Calpheon");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(26, new BossChannelMap(Tr_Balenos3chValue, Tr_Serendia3chValue, BossHP, Tr_Mediah3chValue, Tr_Valencia3chValue, Tr_Magoria3chValue, 0));
                            tree_c3 = ValueConverter(26, "Calpheon");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(27, new BossChannelMap(Tr_Balenos4chValue, Tr_Serendia4chValue, BossHP, Tr_Mediah4chValue, Tr_Valencia4chValue, Tr_Magoria4chValue, 0));
                            tree_c4 = ValueConverter(27, "Calpheon");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "m")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(24, new BossChannelMap(Tr_Balenos1chValue, Tr_Serendia1chValue, Tr_Calpheon1chValue, BossHP, Tr_Valencia1chValue, Tr_Magoria1chValue, Tr_Kms1chValue));
                            tree_m1 = ValueConverter(24, "Mediah");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(25, new BossChannelMap(Tr_Balenos2chValue, Tr_Serendia2chValue, Tr_Calpheon2chValue, BossHP, Tr_Valencia2chValue, Tr_Magoria2chValue, Tr_Kms2chValue));
                            tree_m2 = ValueConverter(25, "Mediah");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(26, new BossChannelMap(Tr_Balenos3chValue, Tr_Serendia3chValue, Tr_Calpheon3chValue, BossHP, Tr_Valencia3chValue, Tr_Magoria3chValue, 0));
                            tree_m3 = ValueConverter(26, "Mediah");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(27, new BossChannelMap(Tr_Balenos4chValue, Tr_Serendia4chValue, Tr_Calpheon4chValue, BossHP, Tr_Valencia4chValue, Tr_Magoria4chValue, 0));
                            tree_m4 = ValueConverter(27, "Mediah");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(24, new BossChannelMap(Tr_Balenos1chValue, Tr_Serendia1chValue, Tr_Calpheon1chValue, Tr_Mediah1chValue, BossHP, Tr_Magoria1chValue, Tr_Kms1chValue));
                            tree_v1 = ValueConverter(24, "Valencia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(25, new BossChannelMap(Tr_Balenos2chValue, Tr_Serendia2chValue, Tr_Calpheon2chValue, Tr_Mediah2chValue, BossHP, Tr_Magoria2chValue, Tr_Kms2chValue));
                            tree_v2 = ValueConverter(25, "Valencia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(26, new BossChannelMap(Tr_Balenos3chValue, Tr_Serendia3chValue, Tr_Calpheon3chValue, Tr_Mediah3chValue, BossHP, Tr_Magoria3chValue, 0));
                            tree_v3 = ValueConverter(26, "Valencia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(27, new BossChannelMap(Tr_Balenos4chValue, Tr_Serendia4chValue, Tr_Calpheon4chValue, Tr_Mediah4chValue, BossHP, Tr_Magoria4chValue, 0));
                            tree_v4 = ValueConverter(27, "Valencia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(24, new BossChannelMap(Tr_Balenos1chValue, Tr_Serendia1chValue, Tr_Calpheon1chValue, Tr_Mediah1chValue, Tr_Valencia1chValue, BossHP, Tr_Kms1chValue));
                            tree_ma1 = ValueConverter(24, "Magoria");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(25, new BossChannelMap(Tr_Balenos2chValue, Tr_Serendia2chValue, Tr_Calpheon2chValue, Tr_Mediah2chValue, Tr_Valencia2chValue, BossHP, Tr_Kms2chValue));
                            tree_ma2 = ValueConverter(25, "Magoria");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(26, new BossChannelMap(Tr_Balenos3chValue, Tr_Serendia3chValue, Tr_Calpheon3chValue, Tr_Mediah3chValue, Tr_Valencia3chValue, BossHP, 0));
                            tree_ma3 = ValueConverter(26, "Magoria");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(27, new BossChannelMap(Tr_Balenos4chValue, Tr_Serendia4chValue, Tr_Calpheon4chValue, Tr_Mediah4chValue, Tr_Valencia4chValue, BossHP, 0));
                            tree_ma4 = ValueConverter(27, "Magoria");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(24, new BossChannelMap(Tr_Balenos1chValue, Tr_Serendia1chValue, Tr_Calpheon1chValue, Tr_Mediah1chValue, Tr_Valencia1chValue, Tr_Magoria1chValue, BossHP));
                            tree_k1 = ValueConverter(24, "Kamasylvia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(25, new BossChannelMap(Tr_Balenos2chValue, Tr_Serendia2chValue, Tr_Calpheon2chValue, Tr_Mediah2chValue, Tr_Valencia2chValue, Tr_Magoria2chValue, BossHP));
                            tree_k2 = ValueConverter(25, "Kamasylvia");
                            tree_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + tree_b1 + SPAN + "2ch：" + tree_b2 + SPAN + "3ch：" + tree_b3 + SPAN + "4ch：" + tree_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + tree_s1 + SPAN + "2ch：" + tree_s2 + SPAN + "3ch：" + tree_s3 + SPAN + "4ch：" + tree_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + tree_c1 + SPAN + "2ch：" + tree_c2 + SPAN + "3ch：" + tree_c3 + SPAN + "4ch：" + tree_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + tree_m1 + SPAN + "2ch：" + tree_m2 + SPAN + "3ch：" + tree_m3 + SPAN + "4ch：" + tree_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + tree_v1 + SPAN + "2ch：" + tree_v2 + SPAN + "3ch：" + tree_v3 + SPAN + "4ch：" + tree_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + tree_ma1 + SPAN + "2ch：" + tree_ma2 + SPAN + "3ch：" + tree_ma3 + SPAN + "4ch：" + tree_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + tree_k1 + SPAN + "2ch：" + tree_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    break;
                case 8: //マッドマン
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(28, new BossChannelMap(BossHP, Md_Serendia1chValue, Md_Calpheon1chValue, Md_Mediah1chValue, Md_Valencia1chValue, Md_Magoria1chValue, Md_Kms1chValue));
                            mud_b1 = ValueConverter(28, "Balenos");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(29, new BossChannelMap(BossHP, Md_Serendia2chValue, Md_Calpheon2chValue, Md_Mediah2chValue, Md_Valencia2chValue, Md_Magoria2chValue, Md_Kms2chValue));
                            mud_b2 = ValueConverter(29, "Balenos");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(30, new BossChannelMap(BossHP, Md_Serendia3chValue, Md_Calpheon3chValue, Md_Mediah3chValue, Md_Valencia3chValue, Md_Magoria3chValue, 0));
                            mud_b3 = ValueConverter(30, "Balenos");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(31, new BossChannelMap(BossHP, Md_Serendia4chValue, Md_Calpheon4chValue, Md_Mediah4chValue, Md_Valencia4chValue, Md_Magoria4chValue, 0));
                            mud_b4 = ValueConverter(31, "Balenos");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(28, new BossChannelMap(Md_Balenos1chValue, BossHP, Md_Calpheon1chValue, Md_Mediah1chValue, Md_Valencia1chValue, Md_Magoria1chValue, Md_Kms1chValue));
                            mud_s1 = ValueConverter(28, "Serendia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(29, new BossChannelMap(Md_Balenos2chValue, BossHP, Md_Calpheon2chValue, Md_Mediah2chValue, Md_Valencia2chValue, Md_Magoria2chValue, Md_Kms2chValue));
                            mud_s2 = ValueConverter(29, "Serendia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(30, new BossChannelMap(Md_Balenos3chValue, BossHP, Md_Calpheon3chValue, Md_Mediah3chValue, Md_Valencia3chValue, Md_Magoria3chValue, 0));
                            mud_s3 = ValueConverter(30, "Serendia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(31, new BossChannelMap(Md_Balenos4chValue, BossHP, Md_Calpheon4chValue, Md_Mediah4chValue, Md_Valencia4chValue, Md_Magoria4chValue, 0));
                            mud_s4 = ValueConverter(31, "Serendia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(28, new BossChannelMap(Md_Balenos1chValue, Md_Serendia1chValue, BossHP, Md_Mediah1chValue, Md_Valencia1chValue, Md_Magoria1chValue, Md_Kms1chValue));
                            mud_c1 = ValueConverter(28, "Calpheon");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(29, new BossChannelMap(Md_Balenos2chValue, Md_Serendia2chValue, BossHP, Md_Mediah2chValue, Md_Valencia2chValue, Md_Magoria2chValue, Md_Kms2chValue));
                            mud_c2 = ValueConverter(29, "Calpheon");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(30, new BossChannelMap(Md_Balenos3chValue, Md_Serendia3chValue, BossHP, Md_Mediah3chValue, Md_Valencia3chValue, Md_Magoria3chValue, 0));
                            mud_c3 = ValueConverter(30, "Calpheon");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(31, new BossChannelMap(Md_Balenos4chValue, Md_Serendia4chValue, BossHP, Md_Mediah4chValue, Md_Valencia4chValue, Md_Magoria4chValue, 0));
                            mud_c4 = ValueConverter(31, "Calpheon");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "m")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(28, new BossChannelMap(Md_Balenos1chValue, Md_Serendia1chValue, Md_Calpheon1chValue, BossHP, Md_Valencia1chValue, Md_Magoria1chValue, Md_Kms1chValue));
                            mud_m1 = ValueConverter(28, "Mediah");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(29, new BossChannelMap(Md_Balenos2chValue, Md_Serendia2chValue, Md_Calpheon2chValue, BossHP, Md_Valencia2chValue, Md_Magoria2chValue, Md_Kms2chValue));
                            mud_m2 = ValueConverter(29, "Mediah");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(30, new BossChannelMap(Md_Balenos3chValue, Md_Serendia3chValue, Md_Calpheon3chValue, BossHP, Md_Valencia3chValue, Md_Magoria3chValue, 0));
                            mud_m3 = ValueConverter(30, "Mediah");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(31, new BossChannelMap(Md_Balenos4chValue, Md_Serendia4chValue, Md_Calpheon4chValue, BossHP, Md_Valencia4chValue, Md_Magoria4chValue, 0));
                            mud_m4 = ValueConverter(31, "Mediah");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(28, new BossChannelMap(Md_Balenos1chValue, Md_Serendia1chValue, Md_Calpheon1chValue, Md_Mediah1chValue, BossHP, Md_Magoria1chValue, Md_Kms1chValue));
                            mud_v1 = ValueConverter(28, "Valencia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(29, new BossChannelMap(Md_Balenos2chValue, Md_Serendia2chValue, Md_Calpheon2chValue, Md_Mediah2chValue, BossHP, Md_Magoria2chValue, Md_Kms2chValue));
                            mud_v2 = ValueConverter(29, "Valencia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(30, new BossChannelMap(Md_Balenos3chValue, Md_Serendia3chValue, Md_Calpheon3chValue, Md_Mediah3chValue, BossHP, Md_Magoria3chValue, 0));
                            mud_v3 = ValueConverter(30, "Valencia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(31, new BossChannelMap(Md_Balenos4chValue, Md_Serendia4chValue, Md_Calpheon4chValue, Md_Mediah4chValue, BossHP, Md_Magoria4chValue, 0));
                            mud_v4 = ValueConverter(31, "Valencia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(28, new BossChannelMap(Md_Balenos1chValue, Md_Serendia1chValue, Md_Calpheon1chValue, Md_Mediah1chValue, Md_Valencia1chValue, BossHP, Md_Kms1chValue));
                            mud_ma1 = ValueConverter(28, "Magoria");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(29, new BossChannelMap(Md_Balenos2chValue, Md_Serendia2chValue, Md_Calpheon2chValue, Md_Mediah2chValue, Md_Valencia2chValue, BossHP, Md_Kms2chValue));
                            mud_ma2 = ValueConverter(29, "Magoria");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable.Insert(30, new BossChannelMap(Md_Balenos3chValue, Md_Serendia3chValue, Md_Calpheon3chValue, Md_Mediah3chValue, Md_Valencia3chValue, BossHP, 0));
                            mud_ma3 = ValueConverter(30, "Magoria");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable.Insert(31, new BossChannelMap(Md_Balenos4chValue, Md_Serendia4chValue, Md_Calpheon4chValue, Md_Mediah4chValue, Md_Valencia4chValue, BossHP, 0));
                            mud_ma4 = ValueConverter(31, "Magoria");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable.Insert(28, new BossChannelMap(Md_Balenos1chValue, Md_Serendia1chValue, Md_Calpheon1chValue, Md_Mediah1chValue, Md_Valencia1chValue, Md_Magoria1chValue, BossHP));
                            mud_k1 = ValueConverter(28, "Kamasylvia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable.Insert(29, new BossChannelMap(Md_Balenos2chValue, Md_Serendia2chValue, Md_Calpheon2chValue, Md_Mediah2chValue, Md_Valencia2chValue, Md_Magoria2chValue, BossHP));
                            mud_k2 = ValueConverter(29, "Kamasylvia");
                            mud_lastreporttime = DateTime.Now;
                            string BossChannelMapStrBalenos = "Balenos 1ch：" + mud_b1 + SPAN + "2ch：" + mud_b2 + SPAN + "3ch：" + mud_b3 + SPAN + "4ch：" + mud_b4 + SPAN;
                            string BossChannelMapStrSerendia = "Serendia 1ch：" + mud_s1 + SPAN + "2ch：" + mud_s2 + SPAN + "3ch：" + mud_s3 + SPAN + "4ch：" + mud_s4 + SPAN;
                            string BossChannelMapStrCalpheon = "Calpheon 1ch：" + mud_c1 + SPAN + "2ch：" + mud_c2 + SPAN + "3ch：" + mud_c3 + SPAN + "4ch：" + mud_c4 + SPAN;
                            string BossChannelMapStrMediah = "Media 1ch：" + mud_m1 + SPAN + "2ch：" + mud_m2 + SPAN + "3ch：" + mud_m3 + SPAN + "4ch：" + mud_m4 + SPAN;
                            string BossChannelMapStrValencia = "Valencia 1ch：" + mud_v1 + SPAN + "2ch：" + mud_v2 + SPAN + "3ch：" + mud_v3 + SPAN + "4ch：" + mud_v4 + SPAN;
                            string BossChannelMapStrMagoria = "Magoria 1ch：" + mud_ma1 + SPAN + "2ch：" + mud_ma2 + SPAN + "3ch：" + mud_ma3 + SPAN + "4ch：" + mud_ma4 + SPAN;
                            string BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + mud_k1 + SPAN + "2ch：" + mud_k2 + SPAN;
                            return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                            LatestBossStatus = return_status;
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
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
                    if(BossChannelMapTable[ChannelID].Balenos > 0) { return_value = BossChannelMapTable[ChannelID].Balenos.ToString() + PERCENT; }
                    if(BossChannelMapTable[ChannelID].Balenos == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifBalenos : " + return_value);
                    }
                    break;
                case "Serendia":
                    if (BossChannelMapTable[ChannelID].Serendia > 0) { return_value = BossChannelMapTable[ChannelID].Serendia.ToString() + PERCENT; }
                    if (BossChannelMapTable[ChannelID].Serendia == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifSerendia : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Calpheon":
                    if (BossChannelMapTable[ChannelID].Calpheon > 0) { return_value = BossChannelMapTable[ChannelID].Calpheon.ToString() + PERCENT; }
                    if (BossChannelMapTable[ChannelID].Calpheon == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifCalpheon : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Mediah":
                    if (BossChannelMapTable[ChannelID].Mediah > 0) { return_value = BossChannelMapTable[ChannelID].Mediah.ToString() + PERCENT; }
                    if (BossChannelMapTable[ChannelID].Mediah == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifMediah : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Valencia":
                    if (BossChannelMapTable[ChannelID].Valencia> 0) { return_value = BossChannelMapTable[ChannelID].Valencia.ToString() + PERCENT; }
                    if (BossChannelMapTable[ChannelID].Valencia == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifValencia : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Magoria":
                    if (BossChannelMapTable[ChannelID].Magoria > 0) { return_value = BossChannelMapTable[ChannelID].Magoria.ToString() + PERCENT; }
                    if (BossChannelMapTable[ChannelID].Magoria == 0) { return_value = DEAD; }
                    if (Program.DEBUGMODE == true)
                    {
                        Program.WriteLog("BossStatus.ValueDefine_ifMagoria : " + "ChID: " + ChannelID + "  " + return_value);
                    }
                    break;
                case "Kamasylvia":
                    if (BossChannelMapTable[ChannelID].Kamasylvia > 0) { return_value = BossChannelMapTable[ChannelID].Kamasylvia.ToString() + PERCENT; }
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
                ku_timer = new Timer();
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
                rn_timer = new Timer();
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
                bh_timer = new Timer();
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
                tree_timer = new Timer();
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
                mud_timer = new Timer();
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
                    BossChannelMapTable.Insert(0, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(1, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(2, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(3, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;
                case 2:
                    BossChannelMapTable.Insert(4, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(5, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(6, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(7, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;
                case 3:
                    BossChannelMapTable.Insert(8, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(9, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(10, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(11, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;
                case 4:
                    BossChannelMapTable.Insert(12, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(13, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(14, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(15, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;
                case 5:
                    BossChannelMapTable.Insert(16, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(17, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(18, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(19, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;
                case 6:
                    BossChannelMapTable.Insert(20, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(21, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(22, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(23, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;
                case 7:
                    BossChannelMapTable.Insert(24, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(25, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(26, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(27, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;
                case 8:
                    BossChannelMapTable.Insert(28, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(29, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(30, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(31, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;
                case 9:
                    BossChannelMapTable.Insert(32, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(33, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(34, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    BossChannelMapTable.Insert(35, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                    break;

            }
        }
        private static TimeSpan CalculateElapsedTime(DateTime StartDT)
        {
            TimeSpan return_value;
            DateTime currentTime = DateTime.Now;
            TimeSpan timespan = currentTime - StartDT;
            return_value = timespan;
            if (Program.DEBUGMODE)
            {
                Program.WriteLog("Elapsed Time Calculated : " + return_value);
            }
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
                case 6:
                    bh_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    bh_timer.Interval = RefreshRate;
                    bh_timer.AutoReset = true;
                    bh_timer.Enabled = true;
                    break;
                case 7:
                    tree_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    tree_timer.Interval = RefreshRate;
                    tree_timer.AutoReset = true;
                    tree_timer.Enabled = true;
                    break;
                case 8:
                    mud_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    mud_timer.Interval = RefreshRate;
                    mud_timer.AutoReset = true;
                    mud_timer.Enabled = true;
                    break;

            }

        }
        private static void RefreshProcess(object sender,ElapsedEventArgs e)
        {
            if (Program.DEBUGMODE)
            {
                Program.WriteLog("Refresh Process was Executed.");
            }
            if (kz_timer.Enabled) { Program.RefreshBatch(1); }
            if (ka_timer.Enabled) { Program.RefreshBatch(2); }
            if (nv_timer.Enabled) { Program.RefreshBatch(3); }
            if (ku_timer.Enabled) { Program.RefreshBatch(4); }
            if (rn_timer.Enabled) { Program.RefreshBatch(5); }
            if (bh_timer.Enabled) { Program.RefreshBatch(6); }
            if (tree_timer.Enabled) { Program.RefreshBatch(7); }
            if (mud_timer.Enabled) { Program.RefreshBatch(8); }
        }
        private void AutoShutStatus(object sender,ElapsedEventArgs e)
        {

        }
        
        private void InvalidChannel()
        {
            //Program.client.
        }
        public static string GetBossTimeStamp(int BossID, bool IsFirstCall)
        {
            try
            {
                string return_str = "";
                switch (BossID)
                {
                    case 1: //Kzarka
                        var kz_lastreportedtime = CalculateElapsedTime(kz_lastreporttime).Hours + "時間" + CalculateElapsedTime(kz_lastreporttime).Minutes + "分" + CalculateElapsedTime(kz_lastreporttime).Seconds + "秒";
                        if (Program.DEBUGMODE)
                        {
                            Program.WriteLog("kz_lastreportedtime_h : " + CalculateElapsedTime(kz_lastreporttime).Hours);
                            Program.WriteLog("kz_lastreportedtime_m : " + CalculateElapsedTime(kz_lastreporttime).Minutes);
                            Program.WriteLog("kz_lastreportedtime_s : " + CalculateElapsedTime(kz_lastreporttime).Seconds);
                        }
                        var kz_elapsedtime = CalculateElapsedTime(kz_spawntime).Hours + "時間" + CalculateElapsedTime(kz_spawntime).Minutes + "分" + CalculateElapsedTime(kz_spawntime).Seconds + "秒";
                        return_str = "最終報告から" + kz_lastreportedtime + "経過" + "/" + "沸きから" + kz_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 2: //karanda
                        var ka_lastreportedtime = CalculateElapsedTime(ka_lastreporttime).Hours + "時間" + CalculateElapsedTime(ka_lastreporttime).Minutes + "分" + CalculateElapsedTime(ka_lastreporttime).Seconds + "秒";
                        if (Program.DEBUGMODE)
                        {
                            Program.WriteLog("ka_lastreportedtime_h : " + CalculateElapsedTime(ka_lastreporttime).Hours);
                            Program.WriteLog("ka_lastreportedtime_m : " + CalculateElapsedTime(ka_lastreporttime).Minutes);
                            Program.WriteLog("ka_lastreportedtime_s : " + CalculateElapsedTime(ka_lastreporttime).Seconds);
                        }
                        var ka_elapsedtime = CalculateElapsedTime(ka_spawntime).Hours + "時間" + CalculateElapsedTime(ka_spawntime).Minutes + "分" + CalculateElapsedTime(ka_spawntime).Seconds + "秒";
                        return_str = "最終報告から" + ka_lastreportedtime + "経過" + "/" + "沸きから" + ka_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 3: //ヌーベル（Nouver）
                        var nv_lastreportedtime = CalculateElapsedTime(nv_lastreporttime).Hours + "時間" + CalculateElapsedTime(nv_lastreporttime).Minutes + "分" + CalculateElapsedTime(nv_lastreporttime).Seconds + "秒";
                        if (Program.DEBUGMODE)
                        {
                            Program.WriteLog("nv_lastreportedtime_h : " + CalculateElapsedTime(nv_lastreporttime).Hours);
                            Program.WriteLog("nv_lastreportedtime_m : " + CalculateElapsedTime(nv_lastreporttime).Minutes);
                            Program.WriteLog("nv_lastreportedtime_s : " + CalculateElapsedTime(nv_lastreporttime).Seconds);
                        }
                        var nv_elapsedtime = CalculateElapsedTime(nv_spawntime).Hours + "時間" + CalculateElapsedTime(nv_spawntime).Minutes + "分" + CalculateElapsedTime(nv_spawntime).Seconds + "秒";
                        return_str = "最終報告から" + nv_lastreportedtime + "経過" + "/" + "沸きから" + nv_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 4: //クツム
                        var ku_lastreportedtime = CalculateElapsedTime(ku_lastreporttime).Hours + "時間" + CalculateElapsedTime(ku_lastreporttime).Minutes + "分" + CalculateElapsedTime(ku_lastreporttime).Seconds + "秒";
                        if (Program.DEBUGMODE)
                        {
                            Program.WriteLog("nv_lastreportedtime_h : " + CalculateElapsedTime(nv_lastreporttime).Hours);
                            Program.WriteLog("nv_lastreportedtime_m : " + CalculateElapsedTime(nv_lastreporttime).Minutes);
                            Program.WriteLog("nv_lastreportedtime_s : " + CalculateElapsedTime(nv_lastreporttime).Seconds);
                        }
                        var ku_elapsedtime = CalculateElapsedTime(ku_spawntime).Hours + "時間" + CalculateElapsedTime(ku_spawntime).Minutes + "分" + CalculateElapsedTime(ku_spawntime).Seconds + "秒";
                        return_str = "最終報告から" + ku_lastreportedtime + "経過" + "/" + "沸きから" + ku_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                }
                return return_str;
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.ToString());
                return "N/A";
            }
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
