using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

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
        const int BossChannelMapTableCount = 43; //初回起動時に初期化するボス状況内部テーブル数
        //
        //ボス状況の最終報告から以下の閾値(時間）を超えても新規の報告がない場合、自動的に対象ボス状況を初期化
        //
        public static int StatusClearThreshold_H = 0; //閾値（時）
        public static int StatusClearThreshold_M = 30; //閾値（分）
        public static int StatusClearThreshold_S = 0; //閾値（秒）
        //
        //Internal Boss Status Buffer
        //内部ボス状況バッファ
        //
        internal static List<InternalStatBuffer> StatBufferTable = new List<InternalStatBuffer>();
        //
        //Boss Time Buffer
        //ボス状況の時間管理用バッファ
        //
        static DateTime kz_initspawntime, ka_initspawntime, ku_initspawntime, nv_initspawntime, rn_initspawntime, bh_initspawntime, tree_initspawntime, mud_initspawntime, tar_initspawntime, iza_initspawntime, test_initspawntime;
        static DateTime kz_spawntime, ka_spawntime, ku_spawntime, nv_spawntime, rn_spawntime, bh_spawntime, tree_spawntime, mud_spawntime, tar_spawntime, iza_spawntime,test_spawntime;
        static DateTime kz_lastreporttime, ka_lastreporttime, ku_lastreporttime, nv_lastreporttime, rn_lastreporttime, bh_lastreporttime, tree_lastreporttime, mud_lastreporttime, tar_lastreporttime, iza_lastreporttime, test_lastreporttime;
        //static TimeSpan BossStatusLimitTime = TimeSpan.FromHours(1);
        static double RefreshRate = 60000;
        static Timer kz_timer, ka_timer, ku_timer, nv_timer, rn_timer, bh_timer, tree_timer, mud_timer, tar_timer, iza_timer, test_timer;
        //
        public static List<string> LatestBossStatus = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "" }; //Boss Status Buffer using at Auto Refresh.
        //
        //Boss Status Automatically Clearing Event (It works when elapsed 30min since the last report from players.)
        //
        private static bool isEnableGBRProcessingLog = false;
        //
        public static void Flag_GBRProcLog(bool flag)
        {
            isEnableGBRProcessingLog = flag;
        }
        public static void InitStatus()
        {
            for(int i = 0; i <= BossChannelMapTableCount; i++)
            {
                BossChannelMapTable.Insert(i, new BossChannelMap(0, 0, 0, 0, 0, 0, 0));
                StatBufferTable.Insert(i, new InternalStatBuffer("0%", "0%", "0%", "0%", "0%", "0%", "0%"));
            }
            Program.WriteLog(SystemMessageDefine.BossChannelMapInit_JP);
        }
        /// <summary>
        /// Create the Boss Status
        /// </summary>
        /// <param name="BossID"></param>
        /// <returns></returns>
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
                    kz_initspawntime = DateTime.Now;
                    InternalBufferInit(1);
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
                    LatestBossStatus[0] = return_status;
                    break;
                case 2: //カランダ
                    BossChannelMapTable.Insert(4, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(5, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(6, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(7, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    ka_spawntime = DateTime.Now;
                    ka_lastreporttime = DateTime.Now;
                    ka_initspawntime = DateTime.Now;
                    InternalBufferInit(2);
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
                    LatestBossStatus[1] = return_status;
                    break;
                case 3: //ヌーベル
                    BossChannelMapTable.Insert(8, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(9, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(10, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(11, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    nv_spawntime = DateTime.Now;
                    nv_initspawntime = DateTime.Now;
                    nv_lastreporttime = DateTime.Now;
                    InternalBufferInit(3);
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
                    LatestBossStatus[2] = return_status;
                    break;
                case 4: //クツム
                    BossChannelMapTable.Insert(12, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(13, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(14, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(15, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    ku_spawntime = DateTime.Now;
                    ku_initspawntime = DateTime.Now;
                    ku_lastreporttime = DateTime.Now;
                    InternalBufferInit(4);
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
                    LatestBossStatus[3] = return_status;
                    break;
                    
                case 5: //レッドノーズ
                    BossChannelMapTable.Insert(16, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(17, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(18, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(19, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    rn_spawntime = DateTime.Now;
                    rn_initspawntime = DateTime.Now;
                    rn_lastreporttime = DateTime.Now;
                    InternalBufferInit(5);
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
                    LatestBossStatus[4] = return_status;
                    break;
                case 6: //ベグ
                    BossChannelMapTable.Insert(20, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(21, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(22, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(23, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    bh_spawntime = DateTime.Now;
                    bh_initspawntime = DateTime.Now;
                    bh_lastreporttime = DateTime.Now;
                    InternalBufferInit(6);
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
                    LatestBossStatus[5] = return_status;
                    break;
                case 7: //愚鈍
                    BossChannelMapTable.Insert(24, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(25, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(26, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(27, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    tree_spawntime = DateTime.Now;
                    tree_initspawntime = DateTime.Now;
                    tree_lastreporttime = DateTime.Now;
                    InternalBufferInit(7);
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
                    LatestBossStatus[6] = return_status;
                    break;
                case 8: //マッドマン
                    BossChannelMapTable.Insert(28, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(29, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(30, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(31, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    mud_spawntime = DateTime.Now;
                    mud_initspawntime = DateTime.Now;
                    mud_lastreporttime = DateTime.Now;
                    InternalBufferInit(8);
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
                    LatestBossStatus[7] = return_status;
                    break;
                case 9: //タルガルゴ
                    BossChannelMapTable.Insert(32, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(33, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(34, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(35, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    tar_spawntime = DateTime.Now;
                    tar_initspawntime = DateTime.Now;
                    tar_lastreporttime = DateTime.Now;
                    InternalBufferInit(9);
                    RefreshStatus(9);
                    break;
                case 10: //イザベラ
                    BossChannelMapTable.Insert(36, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(37, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(38, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(39, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    iza_spawntime = DateTime.Now;
                    iza_initspawntime = DateTime.Now;
                    iza_lastreporttime = DateTime.Now;
                    InternalBufferInit(10);
                    RefreshStatus(10);
                    break;
                case 20: //Test
                    BossChannelMapTable.Insert(40, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(41, new BossChannelMap(100, 100, 100, 100, 100, 100, 100));
                    BossChannelMapTable.Insert(42, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    BossChannelMapTable.Insert(43, new BossChannelMap(100, 100, 100, 100, 100, 100, 0));
                    test_spawntime = DateTime.Now;
                    test_initspawntime = DateTime.Now;
                    test_lastreporttime = DateTime.Now;
                    InternalBufferInit(20);
                    RefreshStatus(20);
                    ///BossChannelMapHeader = "腐敗の君主クザカ（最終更新　" + jst.ToString("HH時 mm分ss秒") + " : 沸きから" + CalculateElapsedTime(kz_spawntime).Seconds + "秒経過" + "）";
                    BossChannelMapStrBalenos = "Balenos 1ch：" + ValueConverter(40, "Balenos") + SPAN + "2ch：" + ValueConverter(41, "Balenos") + SPAN + "3ch：" + ValueConverter(42, "Balenos") + SPAN + "4ch：" + ValueConverter(43, "Balenos") + SPAN;
                    BossChannelMapStrSerendia = "Serendia 1ch：" + ValueConverter(40, "Serendia") + SPAN + "2ch：" + ValueConverter(41, "Serendia") + SPAN + "3ch：" + ValueConverter(42, "Serendia") + SPAN + "4ch：" + ValueConverter(43, "Serendia") + SPAN;
                    BossChannelMapStrCalpheon = "Calpheon 1ch：" + ValueConverter(40, "Calpheon") + SPAN + "2ch：" + ValueConverter(41, "Calpheon") + SPAN + "3ch：" + ValueConverter(42, "Calpheon") + SPAN + "4ch：" + ValueConverter(43, "Calpheon") + SPAN;
                    BossChannelMapStrMediah = "Media 1ch：" + ValueConverter(40, "Mediah") + SPAN + "2ch：" + ValueConverter(41, "Mediah") + SPAN + "3ch：" + ValueConverter(42, "Mediah") + SPAN + "4ch：" + ValueConverter(43, "Mediah") + SPAN;
                    BossChannelMapStrValencia = "Valencia 1ch：" + ValueConverter(40, "Valencia") + SPAN + "2ch：" + ValueConverter(41, "Valencia") + SPAN + "3ch：" + ValueConverter(42, "Valencia") + SPAN + "4ch：" + ValueConverter(43, "Valencia") + SPAN;
                    BossChannelMapStrMagoria = "Magoria 1ch：" + ValueConverter(40, "Magoria") + SPAN + "2ch：" + ValueConverter(41, "Magoria") + SPAN + "3ch：" + ValueConverter(42, "Magoria") + SPAN + "4ch：" + ValueConverter(43, "Magoria") + SPAN;
                    BossChannelMapStrKamasylvia = "Kamasylvia 1ch：" + ValueConverter(40, "Kamasylvia") + SPAN + "2ch：" + ValueConverter(41, "Kamasylvia") + SPAN;
                    return_status = IND + IND + BossChannelMapStrBalenos + "\n" + BossChannelMapStrSerendia + "\n" + BossChannelMapStrCalpheon + "\n" + BossChannelMapStrMediah + "\n" + BossChannelMapStrValencia + IND + BossChannelMapStrMagoria + IND + BossChannelMapStrKamasylvia;
                    LatestBossStatus[12] = return_status;
                    break;
            }
            return return_status;
        }
        public static string ChangeStatusValue(int BossID, string BossChannel, int BossHP)
        {
            string return_status = "N/A";
            //
            //ボス体力値更新処理開始
            //
            switch (BossID)
            {
                case 1:
                    if (BossChannel.Substring(0,1) == "b")
                    {
                        if (BossChannel.Contains("b1"))
                        {
                            BossChannelMapTable[0].Balenos = BossHP;
                            //kz_b1 = ValueConverter(0, "Balenos");
                            StatBufferTable[0].Balenos = ValueConverter(0, "Balenos");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("b2"))
                        {
                            BossChannelMapTable[1].Balenos = BossHP;
                            //kz_b2 = ValueConverter(1, "Balenos");
                            StatBufferTable[1].Balenos = ValueConverter(1, "Balenos");
                            ShowInternalStatus(1);
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("b3"))
                        {
                            BossChannelMapTable[2].Balenos = BossHP;
                            StatBufferTable[2].Balenos = ValueConverter(2, "Balenos");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("b4"))
                        {
                            BossChannelMapTable[3].Balenos = BossHP;
                            StatBufferTable[3].Balenos = ValueConverter(3, "Balenos");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);

                        }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("s1"))
                        {
                            BossChannelMapTable[0].Serendia = BossHP;
                            StatBufferTable[0].Serendia = ValueConverter(0, "Serendia");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("s2"))
                        {
                            BossChannelMapTable[1].Serendia = BossHP;
                            StatBufferTable[1].Serendia = ValueConverter(1, "Serendia");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[2].Serendia = BossHP;
                            StatBufferTable[2].Serendia = ValueConverter(2, "Serendia");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[3].Serendia = BossHP;
                            StatBufferTable[3].Serendia = ValueConverter(3, "Serendia");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[0].Calpheon = BossHP;
                            StatBufferTable[0].Calpheon = ValueConverter(0, "Calpheon");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[1].Calpheon = BossHP;
                            StatBufferTable[1].Calpheon = ValueConverter(1, "Calpheon");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[2].Calpheon = BossHP;
                            StatBufferTable[2].Calpheon = ValueConverter(2, "Calpheon");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[3].Calpheon = BossHP;
                            StatBufferTable[3].Calpheon = ValueConverter(3, "Calpheon");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        //if (BossChannel.Contains("4") && BossHP == 0) { return_status = DeadStatusNotify(1, BossChannel); }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 2) == "me")   //メディアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[0].Mediah = BossHP;
                            StatBufferTable[0].Mediah = ValueConverter(0, "Mediah");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[1].Mediah = BossHP;
                            StatBufferTable[1].Mediah = ValueConverter(1, "Mediah");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[2].Mediah = BossHP;
                            StatBufferTable[2].Mediah = ValueConverter(2, "Mediah");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[3].Mediah = BossHP;
                            StatBufferTable[3].Mediah = ValueConverter(3, "Mediah");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        
                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[0].Valencia = BossHP;
                            StatBufferTable[0].Valencia = ValueConverter(0, "Valencia");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("2"))
                        {

                            BossChannelMapTable[1].Valencia = BossHP;
                            StatBufferTable[1].Valencia = ValueConverter(1, "Valencia");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[2].Valencia = BossHP;
                            StatBufferTable[2].Valencia = ValueConverter(2, "Valencia");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[3].Valencia = BossHP;
                            StatBufferTable[3].Valencia = ValueConverter(3, "Valencia");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        
                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma") //マゴリアch
                    {
                        if (BossChannel.Contains("ma1"))
                        {
                            BossChannelMapTable[0].Magoria = BossHP;
                            StatBufferTable[0].Magoria = ValueConverter(0, "Magoria");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("ma2"))
                        {
                            BossChannelMapTable[1].Magoria = BossHP;
                            StatBufferTable[1].Magoria = ValueConverter(1, "Magoria");
                            kz_lastreporttime = DateTime.Now;
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("ma3"))
                        {
                            BossChannelMapTable[2].Magoria = BossHP;
                            StatBufferTable[2].Magoria = ValueConverter(2, "Magoria");
                            return_status = GenerateBossStatStr(1);
                        }
                        if (BossChannel.Contains("ma4"))
                        {
                            BossChannelMapTable[3].Magoria = BossHP;
                            StatBufferTable[3].Magoria = ValueConverter(3, "Magoria");
                            return_status = GenerateBossStatStr(1);
                        }
                        
                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k") //カーマスリビアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[0].Kamasylvia = BossHP;
                            StatBufferTable[0].Kamasylvia = ValueConverter(0, "Kamasylvia");
                            kz_lastreporttime = DateTime.Now;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[1].Kamasylvia = BossHP;
                            StatBufferTable[1].Kamasylvia = ValueConverter(1, "Kamasylvia");
                            kz_lastreporttime = DateTime.Now;
                        }
                        if (BossChannel.Contains("3")) {  }
                        if (BossChannel.Contains("4")) {  }
                        
                    } //カーマスリビアch
                    //
                    return_status = GenerateBossStatStr(1);
                    //ShowInternalStatus(1);
                    break;
                case 2: //カランダ
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[4].Balenos = BossHP;
                            StatBufferTable[4].Balenos = ValueConverter(4, "Balenos");
                            ka_lastreporttime = DateTime.Now;
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[5].Balenos = BossHP;
                            StatBufferTable[5].Balenos = ValueConverter(5, "Balenos");
                            ka_lastreporttime = DateTime.Now;
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[6].Balenos = BossHP;
                            StatBufferTable[6].Balenos = ValueConverter(6, "Balenos");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[7].Balenos = BossHP;
                            StatBufferTable[7].Balenos = ValueConverter(7, "Balenos");
                        }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[4].Serendia = BossHP;
                            StatBufferTable[4].Serendia = ValueConverter(4, "Serendia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[5].Serendia = BossHP;
                            StatBufferTable[5].Serendia = ValueConverter(5, "Serendia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[6].Serendia = BossHP;                            
                            StatBufferTable[6].Serendia = ValueConverter(6, "Serendia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[7].Serendia = BossHP;
                            StatBufferTable[7].Serendia = ValueConverter(7, "Serendia");
                        }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c") //カルフェオンch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[4].Calpheon = BossHP;
                            StatBufferTable[4].Calpheon = ValueConverter(4, "Calpheon");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[5].Calpheon = BossHP;
                            StatBufferTable[5].Calpheon = ValueConverter(5, "Calpheon");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[6].Calpheon = BossHP;
                            StatBufferTable[6].Calpheon = ValueConverter(6, "Calpheon");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[7].Calpheon = BossHP;
                            StatBufferTable[7].Calpheon = ValueConverter(7, "Calpheon");
                        }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 1) == "m")   //メディアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[4].Mediah = BossHP;
                            StatBufferTable[4].Mediah = ValueConverter(4, "Mediah");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[5].Mediah = BossHP;
                            StatBufferTable[5].Mediah = ValueConverter(5, "Mediah");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[6].Mediah = BossHP;
                            StatBufferTable[6].Mediah = ValueConverter(6, "Mediah");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[7].Mediah = BossHP;
                            StatBufferTable[7].Mediah = ValueConverter(7, "Mediah");
                        }

                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v") //Valencia
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[4].Valencia = BossHP;
                            StatBufferTable[4].Valencia = ValueConverter(4, "Valencia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[5].Valencia = BossHP;
                            StatBufferTable[5].Valencia = ValueConverter(5, "Valencia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[6].Valencia = BossHP;
                            StatBufferTable[6].Valencia= ValueConverter(6, "Valencia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[7].Valencia = BossHP;
                            StatBufferTable[7].Valencia = ValueConverter(7, "Valencia");
                        }

                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma") //マゴリアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[4].Magoria = BossHP;
                            StatBufferTable[4].Magoria = ValueConverter(4, "Magoria");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[5].Magoria = BossHP;
                            StatBufferTable[5].Magoria = ValueConverter(5, "Magoria");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[6].Magoria = BossHP;
                            StatBufferTable[6].Magoria = ValueConverter(6, "Magoria");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[7].Magoria = BossHP;
                            StatBufferTable[7].Magoria = ValueConverter(7, "Magoria");
                        }

                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k") //カーマスリビアch
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[4].Kamasylvia = BossHP;                            
                            StatBufferTable[4].Kamasylvia = ValueConverter(4, "Kamasylvia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[5].Kamasylvia = BossHP;
                            StatBufferTable[5].Kamasylvia = ValueConverter(5, "Kamasylvia");
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }

                    } //カーマスリビアch
                    ka_lastreporttime = DateTime.Now;
                    return_status = GenerateBossStatStr(2);
                    break;
                case 3: //ヌーベル
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[8].Balenos = BossHP;
                            StatBufferTable[8].Balenos = ValueConverter(8, "Balenos");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[9].Balenos = BossHP;
                            StatBufferTable[9].Balenos = ValueConverter(9, "Balenos");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[10].Balenos = BossHP;
                            StatBufferTable[10].Balenos = ValueConverter(10, "Balenos");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[11].Balenos = BossHP;
                            StatBufferTable[11].Balenos = ValueConverter(11, "Balenos");
                        }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[8].Serendia = BossHP;
                            StatBufferTable[8].Serendia = ValueConverter(8, "Serendia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[9].Serendia = BossHP;
                            StatBufferTable[9].Serendia = ValueConverter(9, "Serendia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[10].Serendia = BossHP;
                            StatBufferTable[10].Serendia = ValueConverter(10, "Serendia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[11].Serendia = BossHP;
                            StatBufferTable[11].Serendia = ValueConverter(11, "Serendia");
                        }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[8].Calpheon = BossHP;
                            StatBufferTable[8].Calpheon = ValueConverter(8, "Calpheon");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[9].Calpheon = BossHP;
                            StatBufferTable[9].Calpheon = ValueConverter(9, "Calpheon");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[10].Calpheon = BossHP;
                            StatBufferTable[10].Calpheon = ValueConverter(10, "Calpheon");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[11].Calpheon = BossHP;
                            StatBufferTable[11].Calpheon = ValueConverter(11, "Calpheon");
                        }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 2) == "me")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[8].Mediah = BossHP;
                            StatBufferTable[8].Mediah = ValueConverter(8, "Mediah");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[9].Mediah = BossHP;
                            StatBufferTable[9].Mediah = ValueConverter(9, "Mediah");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[10].Mediah = BossHP;
                            StatBufferTable[10].Mediah = ValueConverter(10, "Mediah");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[11].Mediah = BossHP;
                            StatBufferTable[11].Mediah = ValueConverter(11, "Mediah");
                        }
                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[8].Valencia = BossHP;
                            StatBufferTable[8].Valencia = ValueConverter(8, "Valencia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[9].Valencia = BossHP;
                            StatBufferTable[9].Valencia = ValueConverter(9, "Valencia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[10].Valencia = BossHP;
                            StatBufferTable[10].Valencia = ValueConverter(10, "Valencia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[11].Valencia = BossHP;
                            StatBufferTable[11].Valencia = ValueConverter(11, "Valencia");
                        }
                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[8].Magoria = BossHP;
                            StatBufferTable[8].Magoria = ValueConverter(8, "Magoria");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[9].Magoria = BossHP;
                            StatBufferTable[9].Magoria = ValueConverter(9, "Magoria");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[10].Magoria = BossHP;
                            StatBufferTable[10].Magoria = ValueConverter(10, "Magoria");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[11].Magoria = BossHP;
                            StatBufferTable[11].Magoria = ValueConverter(11, "Magoria");
                        }
                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[8].Kamasylvia = BossHP;
                            StatBufferTable[8].Kamasylvia = ValueConverter(8, "Kamasylvia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[9].Kamasylvia = BossHP;
                            StatBufferTable[9].Kamasylvia = ValueConverter(9, "Kamasylvia");
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    nv_lastreporttime = DateTime.Now;
                    return_status = GenerateBossStatStr(3);
                    break;
                case 4: //クツム
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[12].Balenos = BossHP;
                            StatBufferTable[12].Balenos = ValueConverter(12, "Balenos");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[13].Balenos = BossHP;
                            StatBufferTable[13].Balenos = ValueConverter(13, "Balenos");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[14].Balenos = BossHP;
                            StatBufferTable[14].Balenos = ValueConverter(14, "Balenos");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[15].Balenos = BossHP;
                            StatBufferTable[15].Balenos = ValueConverter(15, "Balenos");
                        }
                    } //バレノスch
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[12].Serendia = BossHP;
                            StatBufferTable[12].Serendia = ValueConverter(12, "Serendia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[13].Serendia = BossHP;
                            StatBufferTable[13].Serendia = ValueConverter(13, "Serendia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[14].Serendia = BossHP;
                            StatBufferTable[14].Serendia = ValueConverter(14, "Serendia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[15].Serendia = BossHP;
                            StatBufferTable[15].Serendia = ValueConverter(15, "Serendia");
                        }
                    } //セレンディアch
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[12].Calpheon = BossHP;
                            StatBufferTable[12].Calpheon = ValueConverter(12, "Calpheon");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[13].Calpheon = BossHP;
                            StatBufferTable[13].Calpheon = ValueConverter(13, "Calpheon");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[14].Calpheon = BossHP;
                            StatBufferTable[14].Calpheon = ValueConverter(14, "Calpheon");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[15].Calpheon = BossHP;
                            StatBufferTable[15].Calpheon = ValueConverter(15, "Calpheon");
                        }
                    } //カルフェオンch
                    if (BossChannel.Substring(0, 2) == "me")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[12].Mediah = BossHP;
                            StatBufferTable[12].Mediah = ValueConverter(12, "Mediah");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[13].Mediah = BossHP;
                            StatBufferTable[13].Mediah = ValueConverter(13, "Mediah");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[14].Mediah = BossHP;
                            StatBufferTable[14].Mediah = ValueConverter(14, "Mediah");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[15].Mediah = BossHP;
                            StatBufferTable[15].Mediah = ValueConverter(15, "Mediah");
                        }
                    } //メディアch
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[12].Valencia = BossHP;
                            StatBufferTable[12].Valencia = ValueConverter(12, "Valencia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[13].Valencia = BossHP;
                            StatBufferTable[13].Valencia = ValueConverter(13, "Valencia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[14].Valencia = BossHP;
                            StatBufferTable[14].Valencia = ValueConverter(14, "Valencia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[15].Valencia = BossHP;
                            StatBufferTable[15].Valencia = ValueConverter(15, "Valencia");
                        }
                    } //バレンシアch
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[12].Magoria = BossHP;
                            StatBufferTable[12].Magoria = ValueConverter(12, "Magoria");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[13].Magoria = BossHP;
                            StatBufferTable[13].Magoria = ValueConverter(13, "Magoria");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[14].Magoria = BossHP;
                            StatBufferTable[14].Magoria = ValueConverter(14, "Magoria");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[15].Magoria = BossHP;
                            StatBufferTable[15].Magoria = ValueConverter(15, "Magoria");
                        }
                    } //マゴリアch
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[12].Kamasylvia = BossHP;
                            StatBufferTable[12].Kamasylvia = ValueConverter(12, "Kamasylvia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[13].Kamasylvia = BossHP;
                            StatBufferTable[13].Kamasylvia = ValueConverter(13, "Kamasylvia");
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    } //カーマスリビアch
                    ku_lastreporttime = DateTime.Now;
                    return_status = GenerateBossStatStr(4);
                    break;
                case 5: //レッドノーズ
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[16].Balenos = BossHP;
                            StatBufferTable[16].Balenos = ValueConverter(16, "Balenos");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[17].Balenos = BossHP;
                            StatBufferTable[17].Balenos = ValueConverter(17, "Balenos");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[18].Balenos = BossHP;
                            StatBufferTable[18].Balenos = ValueConverter(18, "Balenos");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[19].Balenos = BossHP;
                            StatBufferTable[19].Balenos = ValueConverter(19, "Balenos");
                        }
                    } //バレノス
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[16].Serendia = BossHP;
                            StatBufferTable[16].Serendia = ValueConverter(16, "Serendia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[17].Serendia = BossHP;
                            StatBufferTable[17].Serendia = ValueConverter(17, "Serendia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[18].Serendia = BossHP;
                            StatBufferTable[18].Serendia = ValueConverter(18, "Serendia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[19].Serendia = BossHP;
                            StatBufferTable[19].Serendia = ValueConverter(19, "Serendia");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[16].Calpheon = BossHP;
                            StatBufferTable[16].Calpheon = ValueConverter(16, "Calpheon");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[17].Calpheon = BossHP;
                            StatBufferTable[17].Calpheon = ValueConverter(17, "Calpheon");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[18].Calpheon = BossHP;
                            StatBufferTable[18].Calpheon = ValueConverter(18, "Calpheon");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[19].Calpheon = BossHP;
                            StatBufferTable[19].Calpheon = ValueConverter(19, "Calpheon");
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "me")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[16].Mediah = BossHP;
                            StatBufferTable[16].Mediah = ValueConverter(16, "Mediah");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[17].Mediah = BossHP;
                            StatBufferTable[17].Mediah = ValueConverter(17, "Mediah");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[18].Mediah = BossHP;
                            StatBufferTable[18].Mediah = ValueConverter(18, "Mediah");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[19].Mediah = BossHP;
                            StatBufferTable[19].Mediah = ValueConverter(19, "Mediah");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[16].Valencia = BossHP;
                            StatBufferTable[16].Valencia = ValueConverter(16, "Valencia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[17].Valencia = BossHP;
                            StatBufferTable[17].Valencia = ValueConverter(17, "Valencia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[18].Valencia = BossHP;
                            StatBufferTable[18].Valencia = ValueConverter(18, "Valencia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[19].Valencia = BossHP;
                            StatBufferTable[19].Valencia = ValueConverter(19, "Valencia");
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[16].Magoria = BossHP;
                            StatBufferTable[16].Magoria = ValueConverter(16, "Magoria");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[17].Magoria = BossHP;
                            StatBufferTable[17].Magoria = ValueConverter(17, "Magoria");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[18].Magoria = BossHP;
                            StatBufferTable[18].Magoria = ValueConverter(18, "Magoria");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[19].Magoria = BossHP;
                            StatBufferTable[19].Magoria= ValueConverter(19, "Magoria");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[16].Kamasylvia = BossHP;
                            StatBufferTable[16].Kamasylvia = ValueConverter(16, "Kamasylvia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[17].Kamasylvia = BossHP;
                            StatBufferTable[17].Kamasylvia = ValueConverter(17, "Kamasylvia");
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    rn_lastreporttime = DateTime.Now;
                    return_status = GenerateBossStatStr(5);
                    break;
                case 6: //ベグ
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[20].Balenos = BossHP;
                            StatBufferTable[20].Balenos = ValueConverter(20, "Balenos");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[21].Balenos = BossHP;
                            StatBufferTable[21].Balenos = ValueConverter(21, "Balenos");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[22].Balenos = BossHP;
                            StatBufferTable[22].Balenos = ValueConverter(22, "Balenos");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[23].Balenos = BossHP;
                            StatBufferTable[23].Balenos = ValueConverter(23, "Balenos");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[20].Serendia = BossHP;
                            StatBufferTable[20].Serendia = ValueConverter(20, "Serendia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[21].Serendia = BossHP;
                            StatBufferTable[21].Serendia = ValueConverter(21, "Serendia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[22].Serendia = BossHP;
                            StatBufferTable[22].Serendia = ValueConverter(22, "Serendia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[23].Serendia = BossHP;
                            StatBufferTable[23].Serendia = ValueConverter(23, "Serendia");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[20].Calpheon = BossHP;
                            StatBufferTable[20].Calpheon = ValueConverter(20, "Calpheon");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[21].Calpheon = BossHP;
                            StatBufferTable[21].Calpheon = ValueConverter(21, "Calpheon");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[22].Calpheon = BossHP;
                            StatBufferTable[22].Calpheon = ValueConverter(22, "Calpheon");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[23].Calpheon = BossHP;
                            StatBufferTable[23].Calpheon = ValueConverter(23, "Calpheon");
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "me")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[20].Mediah = BossHP;
                            StatBufferTable[20].Mediah = ValueConverter(20, "Mediah");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[21].Mediah = BossHP;
                            StatBufferTable[21].Mediah = ValueConverter(21, "Mediah");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[22].Mediah = BossHP;
                            StatBufferTable[22].Mediah = ValueConverter(22, "Mediah");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[23].Mediah = BossHP;
                            StatBufferTable[23].Mediah = ValueConverter(23, "Mediah");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[20].Valencia = BossHP;
                            StatBufferTable[20].Valencia = ValueConverter(20, "Valencia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[21].Valencia = BossHP;
                            StatBufferTable[21].Valencia = ValueConverter(21, "Valencia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[22].Valencia = BossHP;
                            StatBufferTable[22].Valencia = ValueConverter(22, "Valencia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[23].Valencia = BossHP;
                            StatBufferTable[23].Valencia = ValueConverter(23, "Valencia");
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[20].Magoria = BossHP;
                            StatBufferTable[20].Magoria = ValueConverter(20, "Magoria");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[21].Magoria = BossHP;
                            StatBufferTable[21].Magoria = ValueConverter(21, "Magoria");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[22].Magoria = BossHP;
                            StatBufferTable[22].Magoria = ValueConverter(22, "Magoria");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[23].Magoria = BossHP;
                            StatBufferTable[23].Magoria = ValueConverter(23, "Magoria");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[20].Kamasylvia = BossHP;
                            StatBufferTable[20].Kamasylvia = ValueConverter(20, "Kamasylvia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[21].Kamasylvia = BossHP;
                            StatBufferTable[21].Kamasylvia = ValueConverter(21, "Kamasylvia");
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    bh_lastreporttime = DateTime.Now;
                    return_status = GenerateBossStatStr(6);
                    break;
                case 7: //愚鈍
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[24].Balenos = BossHP;
                            StatBufferTable[24].Balenos = ValueConverter(24, "Balenos");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[25].Balenos = BossHP;
                            StatBufferTable[25].Balenos = ValueConverter(25, "Balenos");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[26].Balenos = BossHP;
                            StatBufferTable[26].Balenos = ValueConverter(26, "Balenos");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[27].Balenos = BossHP;
                            StatBufferTable[27].Balenos = ValueConverter(27, "Balenos");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[24].Serendia = BossHP;
                            StatBufferTable[24].Serendia = ValueConverter(24, "Serendia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[25].Serendia = BossHP;
                            StatBufferTable[25].Serendia = ValueConverter(25, "Serendia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[26].Serendia = BossHP;
                            StatBufferTable[26].Serendia = ValueConverter(26, "Serendia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[27].Serendia = BossHP;
                            StatBufferTable[27].Serendia = ValueConverter(27, "Serendia");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[24].Calpheon = BossHP;
                            StatBufferTable[24].Calpheon = ValueConverter(24, "Calpheon");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[25].Calpheon = BossHP;
                            StatBufferTable[25].Calpheon = ValueConverter(25, "Calpheon");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[26].Calpheon = BossHP;
                            StatBufferTable[26].Calpheon = ValueConverter(26, "Calpheon");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[27].Calpheon = BossHP;
                            StatBufferTable[27].Calpheon = ValueConverter(27, "Calpheon");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "m")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[24].Mediah = BossHP;
                            StatBufferTable[24].Mediah = ValueConverter(24, "Mediah");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[25].Mediah = BossHP;
                            StatBufferTable[25].Mediah = ValueConverter(25, "Mediah");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[26].Mediah = BossHP;
                            StatBufferTable[26].Mediah = ValueConverter(26, "Mediah");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[27].Mediah = BossHP;
                            StatBufferTable[27].Mediah = ValueConverter(27, "Mediah");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[24].Valencia = BossHP;
                            StatBufferTable[24].Valencia = ValueConverter(24, "Valencia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[25].Valencia = BossHP;
                            StatBufferTable[25].Valencia = ValueConverter(25, "Valencia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[26].Valencia = BossHP;
                            StatBufferTable[26].Valencia = ValueConverter(26, "Valencia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[27].Valencia = BossHP;
                            StatBufferTable[27].Valencia = ValueConverter(27, "Valencia");
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[24].Magoria = BossHP;
                            StatBufferTable[24].Magoria = ValueConverter(24, "Magoria");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[25].Magoria = BossHP;
                            StatBufferTable[25].Magoria = ValueConverter(25, "Magoria");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[26].Magoria = BossHP;
                            StatBufferTable[26].Magoria = ValueConverter(26, "Magoria");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[27].Magoria = BossHP;
                            StatBufferTable[27].Magoria = ValueConverter(27, "Magoria");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[24].Kamasylvia = BossHP;
                            StatBufferTable[24].Kamasylvia = ValueConverter(24, "Kamasylvia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[25].Kamasylvia = BossHP;
                            StatBufferTable[25].Kamasylvia = ValueConverter(25, "Kamasylvia");
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    tree_lastreporttime = DateTime.Now;
                    return_status = GenerateBossStatStr(7);
                    break;
                case 8: //マッドマン
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[28].Balenos = BossHP;
                            StatBufferTable[28].Balenos = ValueConverter(28, "Balenos");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[29].Balenos = BossHP;
                            StatBufferTable[29].Balenos = ValueConverter(29, "Balenos");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[30].Balenos = BossHP;
                            StatBufferTable[30].Balenos = ValueConverter(30, "Balenos");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[31].Balenos = BossHP;
                            StatBufferTable[31].Balenos = ValueConverter(31, "Balenos");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "s")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[28].Serendia = BossHP;
                            StatBufferTable[28].Serendia = ValueConverter(28, "Serendia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[29].Serendia = BossHP;
                            StatBufferTable[29].Serendia = ValueConverter(29, "Serendia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[30].Serendia = BossHP;
                            StatBufferTable[30].Serendia = ValueConverter(30, "Serendia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[31].Serendia = BossHP;
                            StatBufferTable[31].Serendia = ValueConverter(31, "Serendia");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "c")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[28].Calpheon = BossHP;
                            StatBufferTable[28].Calpheon = ValueConverter(28, "Calpheon");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[29].Calpheon = BossHP;
                            StatBufferTable[29].Calpheon = ValueConverter(29, "Calpheon");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[30].Calpheon = BossHP;
                            StatBufferTable[30].Calpheon = ValueConverter(30, "Calpheon");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[31].Calpheon = BossHP;
                            StatBufferTable[31].Calpheon = ValueConverter(31, "Calpheon");
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "me")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[28].Mediah = BossHP;
                            StatBufferTable[28].Mediah = ValueConverter(28, "Mediah");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[29].Mediah = BossHP;
                            StatBufferTable[29].Mediah = ValueConverter(29, "Mediah");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[30].Mediah = BossHP;
                            StatBufferTable[30].Mediah = ValueConverter(30, "Mediah");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[31].Mediah = BossHP;
                            StatBufferTable[31].Mediah = ValueConverter(31, "Mediah");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "v")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[28].Valencia = BossHP;
                            StatBufferTable[28].Valencia = ValueConverter(28, "Valencia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[29].Valencia = BossHP;
                            StatBufferTable[29].Valencia = ValueConverter(29, "Valencia");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[30].Valencia = BossHP;
                            StatBufferTable[30].Valencia = ValueConverter(30, "Valencia");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[31].Valencia = BossHP;
                            StatBufferTable[31].Valencia = ValueConverter(31, "Valencia");
                        }
                    }
                    if (BossChannel.Substring(0, 2) == "ma")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[28].Magoria = BossHP;
                            StatBufferTable[28].Magoria = ValueConverter(28, "Magoria");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[29].Magoria = BossHP;
                            StatBufferTable[29].Magoria = ValueConverter(29, "Magoria");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[30].Magoria = BossHP;
                            StatBufferTable[30].Magoria = ValueConverter(30, "Magoria");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[31].Magoria = BossHP;
                            StatBufferTable[31].Magoria = ValueConverter(31, "Magoria");
                        }
                    }
                    if (BossChannel.Substring(0, 1) == "k")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[28].Kamasylvia = BossHP;
                            StatBufferTable[28].Kamasylvia = ValueConverter(28, "Kamasylvia");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[29].Kamasylvia = BossHP;
                            StatBufferTable[29].Kamasylvia = ValueConverter(29, "Kamasylvia");
                        }
                        if (BossChannel.Contains("3")) { }
                        if (BossChannel.Contains("4")) { }
                    }
                    mud_lastreporttime = DateTime.Now;
                    return_status = GenerateBossStatStr(8);
                    break;
                case 20: //Test
                    if (BossChannel.Substring(0, 1) == "b")
                    {
                        if (BossChannel.Contains("1"))
                        {
                            BossChannelMapTable[40].Balenos = BossHP;
                            StatBufferTable[40].Balenos = ValueConverter(40, "Balenos");
                        }
                        if (BossChannel.Contains("2"))
                        {
                            BossChannelMapTable[41].Balenos = BossHP;
                            StatBufferTable[41].Balenos = ValueConverter(41, "Balenos");
                        }
                        if (BossChannel.Contains("3"))
                        {
                            BossChannelMapTable[42].Balenos = BossHP;
                            StatBufferTable[42].Balenos = ValueConverter(42, "Balenos");
                        }
                        if (BossChannel.Contains("4"))
                        {
                            BossChannelMapTable[43].Balenos = BossHP;
                            StatBufferTable[43].Balenos = ValueConverter(43, "Balenos");
                        }
                    } //バレノスch
                    test_lastreporttime = DateTime.Now;
                    return_status = GenerateBossStatStr(20);
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
                        Program.WriteLog("BossStatus.ValueDefine_ifBalenos : " + "ChID" + ChannelID + "  " + return_value);
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
        private static void InternalBufferInit(int BossID)
        {
            switch (BossID)
            {
                case 1:
                    //Kzarka
                    kz_timer = new Timer();
                    int KzarkaListStartIndex = 0;
                    int KzarkaListEndIndex = 3;
                    for (int i = KzarkaListStartIndex; i <= KzarkaListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
                case 2:
                    //Karanda
                    ka_timer = new Timer();
                    const int KarandaListStartIndex = 4;
                    const int KarandaListEndIndex = 7;
                    for (int i = KarandaListStartIndex; i <= KarandaListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
                case 3:
                    nv_timer = new Timer();
                    const int NouverListStartIndex = 8;
                    const int NouverListEndIndex = 11;
                    for (int i = NouverListStartIndex; i <= NouverListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
                case 4:
                    ku_timer = new Timer();
                    const int KutumListStartIndex = 12;
                    const int KutumListEndIndex = 15;
                    for (int i = KutumListStartIndex; i <= KutumListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
                case 5:
                    rn_timer = new Timer();
                    const int RednoseListStartIndex = 16;
                    const int RednoseListEndIndex = 19;
                    for (int i = RednoseListStartIndex; i <= RednoseListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
                case 6:
                    bh_timer = new Timer();
                    const int BhegListStartIndex = 20;
                    const int BhegListEndIndex = 23;
                    for (int i = BhegListStartIndex; i <= BhegListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
                case 7:
                    tree_timer = new Timer();
                    const int TreeListStartIndex = 24;
                    const int TreeListEndIndex = 27;
                    for (int i = TreeListStartIndex; i <= TreeListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
                case 8:
                    mud_timer = new Timer();
                    const int MudListStartIndex = 28;
                    const int MudListEndIndex = 31;
                    for (int i = MudListStartIndex; i <= MudListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
                case 20:
                    test_timer = new Timer();
                    const int TestListStartIndex = 40;
                    const int TestListEndIndex = 43;
                    for (int i = TestListStartIndex; i <= TestListEndIndex; i++)
                    {
                        StatBufferTable[i].Balenos = ValueConverter(i, "Balenos");
                        StatBufferTable[i].Serendia = ValueConverter(i, "Serendia");
                        StatBufferTable[i].Calpheon = ValueConverter(i, "Calpheon");
                        StatBufferTable[i].Mediah = ValueConverter(i, "Mediah");
                        StatBufferTable[i].Valencia = ValueConverter(i, "Valencia");
                        StatBufferTable[i].Magoria = ValueConverter(i, "Magoria");
                        StatBufferTable[i].Kamasylvia = ValueConverter(i, "Kamasylvia");
                    }
                    break;
            }
        }
        private static TimeSpan CalculateElapsedTime(DateTime StartDT, bool isAutoRefreshed)
        {
            TimeSpan return_value;
            DateTime currentTime = DateTime.Now;
            TimeSpan timespan = currentTime - StartDT;
            TimeSpan through = StartDT - StartDT;
            return_value = timespan;
            if (isAutoRefreshed) { Program.WriteLog("Elapsed Time Calculated : " + return_value); return return_value; }
            else { return through; }
            //return return_value;
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
                case 9:
                    //Offin
                    break;
                case 15:
                    //Event Boss 1 (Targargo)
                    tar_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    tar_timer.Interval = RefreshRate;
                    tar_timer.AutoReset = true;
                    tar_timer.Enabled = false;
                    break;
                case 16:
                    //Event Boss 2 (Izabella)
                    iza_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    iza_timer.Interval = RefreshRate;
                    iza_timer.AutoReset = true;
                    iza_timer.Enabled = false;
                    break;
                case 20:
                    test_timer.Elapsed += new ElapsedEventHandler(RefreshProcess);
                    test_timer.Interval = RefreshRate;
                    test_timer.AutoReset = true;
                    test_timer.Enabled = false;
                    break;
            }

        }
        private static void RefreshProcess(object sender,ElapsedEventArgs e)
        {
            if (Program.DEBUGMODE)
            {
                Program.WriteLog("Refresh Process was Executed.");
            }
            if (kz_timer.Enabled)
            {
                if (CalculateElapsedTime(kz_spawntime, true).Minutes >= StatusClearThreshold_M) { Program.DefeatedBatch(1); ShowInternalStatus(1); kz_timer.Enabled = false; }
                else { Program.RefreshBatch(1); }
            }
            else { Program.WriteLog("Failed to Call Refresh Batch." + kz_timer.Enabled); }
            //
            if (ka_timer.Enabled)
            {
                if (CalculateElapsedTime(ka_spawntime, true).Minutes>= StatusClearThreshold_M) { Program.DefeatedBatch(2);  ka_timer.Enabled = false; }
                else { Program.RefreshBatch(2); }
            }
            else { Program.WriteLog("Failed to Call Refresh Batch." + ka_timer.Enabled); }
            //
            if (nv_timer.Enabled)
            {
                if (CalculateElapsedTime(nv_spawntime, true).Minutes >= StatusClearThreshold_M) { Program.DefeatedBatch(3); nv_timer.Enabled = false; }
                else { Program.RefreshBatch(3); }
            }
            else { Program.WriteLog("Failed to Call Refresh Batch."); }
            //
            if (ku_timer.Enabled)
            {
                if(CalculateElapsedTime(ku_spawntime, true).Minutes >= StatusClearThreshold_M) { Program.DefeatedBatch(4); ku_timer.Enabled = false; }
                else { Program.RefreshBatch(4); }
            }
            else { Program.WriteLog("Failed to Call Refresh Batch."); }
            //
            if (rn_timer.Enabled)
            {
                if (CalculateElapsedTime(rn_spawntime, true).Minutes >= StatusClearThreshold_M) { Program.DefeatedBatch(5); rn_timer.Enabled = false; }
                else { Program.RefreshBatch(5); }
            }
            else { Program.WriteLog("Failed to Call Refresh Batch."); }
            //
            if (bh_timer.Enabled)
            {
                if (CalculateElapsedTime(bh_spawntime, true).Minutes >= StatusClearThreshold_M) { Program.DefeatedBatch(6); bh_timer.Enabled = false; }
                else { Program.RefreshBatch(6); }
            }
            else { Program.WriteLog("Failed to Call Refresh Batch."); }
            //
            if (tree_timer.Enabled)
            {
                if (CalculateElapsedTime(tree_spawntime, true).Minutes >= StatusClearThreshold_M) { Program.DefeatedBatch(7); tree_timer.Enabled = false; }
                else { Program.RefreshBatch(7); }
            }
            else { Program.WriteLog("Failed to Call Refresh Batch."); }
            //
            if (mud_timer.Enabled)
            {
                if(CalculateElapsedTime(mud_spawntime, true).Minutes >= StatusClearThreshold_M) { Program.DefeatedBatch(8); mud_timer.Enabled = false; }
                else { Program.RefreshBatch(8); }
            }
            else { Program.WriteLog("Failed to Call Refresh Batch."); }
            //
            if (test_timer.Enabled) { Program.RefreshBatch(20); } else { Program.WriteLog("Failed to Call Refresh Batch."); }
        }
        //
        //GenerateBossStatStr
        //
        //
        private static string GenerateBossStatStr(int BossID)
        {
            string return_status = "";
            string BCMStrBalenos = "";
            string BCMStrSerendia = "";
            string BCMStrCalpheon = "";
            string BCMStrMediah = "";
            string BCMStrValencia = "";
            string BCMStrMagoria = "";
            string BCMStrKama = "";
            switch (BossID)
            {
                case 1:
                    BCMStrBalenos = "Balenos 1ch：" + StatBufferTable[0].Balenos + SPAN + "2ch：" + StatBufferTable[1].Balenos + SPAN + "3ch：" + StatBufferTable[2].Balenos + SPAN + "4ch：" + StatBufferTable[3].Balenos + SPAN;
                    BCMStrSerendia = "Serendia 1ch：" + StatBufferTable[0].Serendia + SPAN + "2ch：" + StatBufferTable[1].Serendia + SPAN + "3ch：" + StatBufferTable[2].Serendia + SPAN + "4ch：" + StatBufferTable[3].Serendia + SPAN;
                    BCMStrCalpheon = "Calpheon 1ch：" + StatBufferTable[0].Calpheon + SPAN + "2ch：" + StatBufferTable[1].Calpheon + SPAN + "3ch：" + StatBufferTable[2].Calpheon + SPAN + "4ch：" + StatBufferTable[3].Calpheon + SPAN;
                    BCMStrMediah = "Media 1ch：" + StatBufferTable[0].Mediah+ SPAN + "2ch：" + StatBufferTable[1].Mediah + SPAN + "3ch：" + StatBufferTable[2].Mediah + SPAN + "4ch：" + StatBufferTable[3].Mediah + SPAN;
                    BCMStrValencia = "Valencia 1ch：" + StatBufferTable[0].Valencia + SPAN + "2ch：" + StatBufferTable[1].Valencia + SPAN + "3ch：" + StatBufferTable[2].Valencia + SPAN + "4ch：" + StatBufferTable[3].Valencia + SPAN;
                    BCMStrMagoria = "Magoria 1ch：" + StatBufferTable[0].Magoria + SPAN + "2ch：" + StatBufferTable[1].Magoria + SPAN + "3ch：" + StatBufferTable[2].Magoria + SPAN + "4ch：" + StatBufferTable[3].Magoria + SPAN;
                    BCMStrKama = "Kamasylvia 1ch：" + StatBufferTable[0].Kamasylvia + SPAN + "2ch：" + StatBufferTable[1].Kamasylvia + SPAN;
                    return_status = IND + IND + BCMStrBalenos + "\n" + BCMStrSerendia + "\n" + BCMStrCalpheon + "\n" + BCMStrMediah + "\n" + BCMStrValencia + IND + BCMStrMagoria + IND + BCMStrKama;
                    LatestBossStatus[0] = return_status;
                    return return_status;
                case 2:
                    BCMStrBalenos = "Balenos 1ch：" + StatBufferTable[4].Balenos + SPAN + "2ch：" + StatBufferTable[5].Balenos + SPAN + "3ch：" + StatBufferTable[6].Balenos + SPAN + "4ch：" + StatBufferTable[7].Balenos + SPAN;
                    BCMStrSerendia = "Serendia 1ch：" + StatBufferTable[4].Serendia + SPAN + "2ch：" + StatBufferTable[5].Serendia + SPAN + "3ch：" + StatBufferTable[6].Serendia + SPAN + "4ch：" + StatBufferTable[7].Serendia + SPAN;
                    BCMStrCalpheon = "Calpheon 1ch：" + StatBufferTable[4].Calpheon + SPAN + "2ch：" + StatBufferTable[5].Calpheon + SPAN + "3ch：" + StatBufferTable[6].Calpheon + SPAN + "4ch：" + StatBufferTable[7].Calpheon + SPAN;
                    BCMStrMediah = "Media 1ch：" + StatBufferTable[4].Mediah + SPAN + "2ch：" + StatBufferTable[5].Mediah + SPAN + "3ch：" + StatBufferTable[6].Mediah + SPAN + "4ch：" + StatBufferTable[7].Mediah + SPAN;
                    BCMStrValencia = "Valencia 1ch：" + StatBufferTable[4].Valencia + SPAN + "2ch：" + StatBufferTable[5].Valencia + SPAN + "3ch：" + StatBufferTable[6].Valencia + SPAN + "4ch：" + StatBufferTable[7].Valencia + SPAN;
                    BCMStrMagoria = "Magoria 1ch：" + StatBufferTable[4].Magoria + SPAN + "2ch：" + StatBufferTable[5].Magoria + SPAN + "3ch：" + StatBufferTable[6].Magoria + SPAN + "4ch：" + StatBufferTable[7].Magoria + SPAN;
                    BCMStrKama = "Kamasylvia 1ch：" + StatBufferTable[4].Kamasylvia + SPAN + "2ch：" + StatBufferTable[5].Kamasylvia + SPAN;
                    return_status = IND + IND + BCMStrBalenos + "\n" + BCMStrSerendia + "\n" + BCMStrCalpheon + "\n" + BCMStrMediah + "\n" + BCMStrValencia + IND + BCMStrMagoria + IND + BCMStrKama;
                    LatestBossStatus[1] = return_status;
                    return return_status;
                case 3:
                    BCMStrBalenos = "Balenos 1ch：" + StatBufferTable[8].Balenos + SPAN + "2ch：" + StatBufferTable[9].Balenos + SPAN + "3ch：" + StatBufferTable[10].Balenos + SPAN + "4ch：" + StatBufferTable[11].Balenos + SPAN;
                    BCMStrSerendia = "Serendia 1ch：" + StatBufferTable[8].Serendia + SPAN + "2ch：" + StatBufferTable[9].Serendia + SPAN + "3ch：" + StatBufferTable[10].Serendia + SPAN + "4ch：" + StatBufferTable[11].Serendia + SPAN;
                    BCMStrCalpheon = "Calpheon 1ch：" + StatBufferTable[8].Calpheon + SPAN + "2ch：" + StatBufferTable[9].Calpheon + SPAN + "3ch：" + StatBufferTable[10].Calpheon + SPAN + "4ch：" + StatBufferTable[11].Calpheon + SPAN;
                    BCMStrMediah = "Media 1ch：" + StatBufferTable[8].Mediah + SPAN + "2ch：" + StatBufferTable[9].Mediah + SPAN + "3ch：" + StatBufferTable[10].Mediah + SPAN + "4ch：" + StatBufferTable[11].Mediah + SPAN;
                    BCMStrValencia = "Valencia 1ch：" + StatBufferTable[8].Valencia + SPAN + "2ch：" + StatBufferTable[9].Valencia + SPAN + "3ch：" + StatBufferTable[10].Valencia + SPAN + "4ch：" + StatBufferTable[11].Valencia + SPAN;
                    BCMStrMagoria = "Magoria 1ch：" + StatBufferTable[8].Magoria + SPAN + "2ch：" + StatBufferTable[9].Magoria + SPAN + "3ch：" + StatBufferTable[10].Magoria + SPAN + "4ch：" + StatBufferTable[11].Magoria + SPAN;
                    BCMStrKama = "Kamasylvia 1ch：" + StatBufferTable[8].Kamasylvia + SPAN + "2ch：" + StatBufferTable[9].Kamasylvia + SPAN;
                    return_status = IND + IND + BCMStrBalenos + "\n" + BCMStrSerendia + "\n" + BCMStrCalpheon + "\n" + BCMStrMediah + "\n" + BCMStrValencia + IND + BCMStrMagoria + IND + BCMStrKama;
                    LatestBossStatus[2] = return_status;
                    return return_status;
                case 4:
                    BCMStrBalenos = "Balenos 1ch：" + StatBufferTable[12].Balenos + SPAN + "2ch：" + StatBufferTable[13].Balenos + SPAN + "3ch：" + StatBufferTable[14].Balenos + SPAN + "4ch：" + StatBufferTable[15].Balenos + SPAN;
                    BCMStrSerendia = "Serendia 1ch：" + StatBufferTable[12].Serendia + SPAN + "2ch：" + StatBufferTable[13].Serendia + SPAN + "3ch：" + StatBufferTable[14].Serendia + SPAN + "4ch：" + StatBufferTable[15].Serendia + SPAN;
                    BCMStrCalpheon = "Calpheon 1ch：" + StatBufferTable[12].Calpheon + SPAN + "2ch：" + StatBufferTable[13].Calpheon + SPAN + "3ch：" + StatBufferTable[14].Calpheon + SPAN + "4ch：" + StatBufferTable[15].Calpheon + SPAN;
                    BCMStrMediah = "Media 1ch：" + StatBufferTable[12].Mediah + SPAN + "2ch：" + StatBufferTable[13].Mediah + SPAN + "3ch：" + StatBufferTable[14].Mediah + SPAN + "4ch：" + StatBufferTable[15].Mediah + SPAN;
                    BCMStrValencia = "Valencia 1ch：" + StatBufferTable[12].Valencia + SPAN + "2ch：" + StatBufferTable[13].Valencia + SPAN + "3ch：" + StatBufferTable[14].Valencia + SPAN + "4ch：" + StatBufferTable[15].Valencia + SPAN;
                    BCMStrMagoria = "Magoria 1ch：" + StatBufferTable[12].Magoria + SPAN + "2ch：" + StatBufferTable[13].Magoria + SPAN + "3ch：" + StatBufferTable[14].Magoria + SPAN + "4ch：" + StatBufferTable[15].Magoria + SPAN;
                    BCMStrKama = "Kamasylvia 1ch：" + StatBufferTable[12].Kamasylvia + SPAN + "2ch：" + StatBufferTable[13].Kamasylvia + SPAN;
                    return_status = IND + IND + BCMStrBalenos + "\n" + BCMStrSerendia + "\n" + BCMStrCalpheon + "\n" + BCMStrMediah + "\n" + BCMStrValencia + IND + BCMStrMagoria + IND + BCMStrKama;
                    LatestBossStatus[3] = return_status;
                    return return_status;
                case 5:
                    BCMStrBalenos = "Balenos 1ch：" + StatBufferTable[16].Balenos + SPAN + "2ch：" + StatBufferTable[17].Balenos + SPAN + "3ch：" + StatBufferTable[18].Balenos + SPAN + "4ch：" + StatBufferTable[19].Balenos + SPAN;
                    BCMStrSerendia = "Serendia 1ch：" + StatBufferTable[16].Serendia + SPAN + "2ch：" + StatBufferTable[17].Serendia + SPAN + "3ch：" + StatBufferTable[18].Serendia + SPAN + "4ch：" + StatBufferTable[19].Serendia + SPAN;
                    BCMStrCalpheon = "Calpheon 1ch：" + StatBufferTable[16].Calpheon + SPAN + "2ch：" + StatBufferTable[17].Calpheon + SPAN + "3ch：" + StatBufferTable[18].Calpheon + SPAN + "4ch：" + StatBufferTable[19].Calpheon + SPAN;
                    BCMStrMediah = "Media 1ch：" + StatBufferTable[16].Mediah + SPAN + "2ch：" + StatBufferTable[17].Mediah + SPAN + "3ch：" + StatBufferTable[18].Mediah + SPAN + "4ch：" + StatBufferTable[19].Mediah + SPAN;
                    BCMStrValencia = "Valencia 1ch：" + StatBufferTable[16].Valencia + SPAN + "2ch：" + StatBufferTable[17].Valencia + SPAN + "3ch：" + StatBufferTable[18].Valencia + SPAN + "4ch：" + StatBufferTable[19].Valencia + SPAN;
                    BCMStrMagoria = "Magoria 1ch：" + StatBufferTable[16].Magoria + SPAN + "2ch：" + StatBufferTable[17].Magoria + SPAN + "3ch：" + StatBufferTable[18].Magoria + SPAN + "4ch：" + StatBufferTable[19].Magoria + SPAN;
                    BCMStrKama = "Kamasylvia 1ch：" + StatBufferTable[16].Kamasylvia + SPAN + "2ch：" + StatBufferTable[17].Kamasylvia + SPAN;
                    return_status = IND + IND + BCMStrBalenos + "\n" + BCMStrSerendia + "\n" + BCMStrCalpheon + "\n" + BCMStrMediah + "\n" + BCMStrValencia + IND + BCMStrMagoria + IND + BCMStrKama;
                    LatestBossStatus[4] = return_status;
                    return return_status;
                case 6:
                    BCMStrBalenos = "Balenos 1ch：" + StatBufferTable[20].Balenos + SPAN + "2ch：" + StatBufferTable[21].Balenos + SPAN + "3ch：" + StatBufferTable[22].Balenos + SPAN + "4ch：" + StatBufferTable[23].Balenos + SPAN;
                    BCMStrSerendia = "Serendia 1ch：" + StatBufferTable[20].Serendia + SPAN + "2ch：" + StatBufferTable[21].Serendia + SPAN + "3ch：" + StatBufferTable[22].Serendia + SPAN + "4ch：" + StatBufferTable[23].Serendia + SPAN;
                    BCMStrCalpheon = "Calpheon 1ch：" + StatBufferTable[20].Calpheon + SPAN + "2ch：" + StatBufferTable[21].Calpheon + SPAN + "3ch：" + StatBufferTable[22].Calpheon + SPAN + "4ch：" + StatBufferTable[23].Calpheon + SPAN;
                    BCMStrMediah = "Media 1ch：" + StatBufferTable[20].Mediah + SPAN + "2ch：" + StatBufferTable[21].Mediah + SPAN + "3ch：" + StatBufferTable[22].Mediah + SPAN + "4ch：" + StatBufferTable[23].Mediah + SPAN;
                    BCMStrValencia = "Valencia 1ch：" + StatBufferTable[20].Valencia + SPAN + "2ch：" + StatBufferTable[21].Valencia + SPAN + "3ch：" + StatBufferTable[22].Valencia + SPAN + "4ch：" + StatBufferTable[23].Valencia + SPAN;
                    BCMStrMagoria = "Magoria 1ch：" + StatBufferTable[20].Magoria + SPAN + "2ch：" + StatBufferTable[21].Magoria + SPAN + "3ch：" + StatBufferTable[22].Magoria + SPAN + "4ch：" + StatBufferTable[23].Magoria + SPAN;
                    BCMStrKama = "Kamasylvia 1ch：" + StatBufferTable[20].Kamasylvia + SPAN + "2ch：" + StatBufferTable[21].Kamasylvia + SPAN;
                    return_status = IND + IND + BCMStrBalenos + "\n" + BCMStrSerendia + "\n" + BCMStrCalpheon + "\n" + BCMStrMediah + "\n" + BCMStrValencia + IND + BCMStrMagoria + IND + BCMStrKama;
                    LatestBossStatus[5] = return_status;
                    return return_status;
                case 7:
                    BCMStrBalenos = "Balenos 1ch：" + StatBufferTable[24].Balenos + SPAN + "2ch：" + StatBufferTable[25].Balenos + SPAN + "3ch：" + StatBufferTable[26].Balenos + SPAN + "4ch：" + StatBufferTable[27].Balenos + SPAN;
                    BCMStrSerendia = "Serendia 1ch：" + StatBufferTable[24].Serendia + SPAN + "2ch：" + StatBufferTable[25].Serendia + SPAN + "3ch：" + StatBufferTable[26].Serendia + SPAN + "4ch：" + StatBufferTable[27].Serendia + SPAN;
                    BCMStrCalpheon = "Calpheon 1ch：" + StatBufferTable[24].Calpheon + SPAN + "2ch：" + StatBufferTable[25].Calpheon + SPAN + "3ch：" + StatBufferTable[26].Calpheon + SPAN + "4ch：" + StatBufferTable[27].Calpheon + SPAN;
                    BCMStrMediah = "Media 1ch：" + StatBufferTable[24].Mediah + SPAN + "2ch：" + StatBufferTable[25].Mediah + SPAN + "3ch：" + StatBufferTable[26].Mediah + SPAN + "4ch：" + StatBufferTable[27].Mediah + SPAN;
                    BCMStrValencia = "Valencia 1ch：" + StatBufferTable[24].Valencia + SPAN + "2ch：" + StatBufferTable[25].Valencia + SPAN + "3ch：" + StatBufferTable[26].Valencia + SPAN + "4ch：" + StatBufferTable[27].Valencia + SPAN;
                    BCMStrMagoria = "Magoria 1ch：" + StatBufferTable[24].Magoria + SPAN + "2ch：" + StatBufferTable[25].Magoria + SPAN + "3ch：" + StatBufferTable[26].Magoria + SPAN + "4ch：" + StatBufferTable[27].Magoria + SPAN;
                    BCMStrKama = "Kamasylvia 1ch：" + StatBufferTable[24].Kamasylvia + SPAN + "2ch：" + StatBufferTable[25].Kamasylvia + SPAN;
                    return_status = IND + IND + BCMStrBalenos + "\n" + BCMStrSerendia + "\n" + BCMStrCalpheon + "\n" + BCMStrMediah + "\n" + BCMStrValencia + IND + BCMStrMagoria + IND + BCMStrKama;
                    LatestBossStatus[6] = return_status;
                    return return_status;
                case 8:
                    BCMStrBalenos = "Balenos 1ch：" + StatBufferTable[28].Balenos + SPAN + "2ch：" + StatBufferTable[29].Balenos + SPAN + "3ch：" + StatBufferTable[30].Balenos + SPAN + "4ch：" + StatBufferTable[31].Balenos + SPAN;
                    BCMStrSerendia = "Serendia 1ch：" + StatBufferTable[28].Serendia + SPAN + "2ch：" + StatBufferTable[29].Serendia + SPAN + "3ch：" + StatBufferTable[30].Serendia + SPAN + "4ch：" + StatBufferTable[31].Serendia + SPAN;
                    BCMStrCalpheon = "Calpheon 1ch：" + StatBufferTable[28].Calpheon + SPAN + "2ch：" + StatBufferTable[29].Calpheon + SPAN + "3ch：" + StatBufferTable[30].Calpheon + SPAN + "4ch：" + StatBufferTable[31].Calpheon + SPAN;
                    BCMStrMediah = "Media 1ch：" + StatBufferTable[28].Mediah + SPAN + "2ch：" + StatBufferTable[29].Mediah + SPAN + "3ch：" + StatBufferTable[30].Mediah + SPAN + "4ch：" + StatBufferTable[31].Mediah + SPAN;
                    BCMStrValencia = "Valencia 1ch：" + StatBufferTable[28].Valencia + SPAN + "2ch：" + StatBufferTable[29].Valencia + SPAN + "3ch：" + StatBufferTable[30].Valencia + SPAN + "4ch：" + StatBufferTable[31].Valencia + SPAN;
                    BCMStrMagoria = "Magoria 1ch：" + StatBufferTable[28].Magoria + SPAN + "2ch：" + StatBufferTable[29].Magoria + SPAN + "3ch：" + StatBufferTable[30].Magoria + SPAN + "4ch：" + StatBufferTable[31].Magoria + SPAN;
                    BCMStrKama = "Kamasylvia 1ch：" + StatBufferTable[28].Kamasylvia + SPAN + "2ch：" + StatBufferTable[29].Kamasylvia + SPAN;
                    return_status = IND + IND + BCMStrBalenos + "\n" + BCMStrSerendia + "\n" + BCMStrCalpheon + "\n" + BCMStrMediah + "\n" + BCMStrValencia + IND + BCMStrMagoria + IND + BCMStrKama;
                    LatestBossStatus[7] = return_status;
                    return return_status;
            }
            return "N/A";
        }
        //GenerateBossResults
        //討伐完了時のボス討伐内訳を生成
        //
        public static string GenerateBossResults(int BossID, int Type)
        {
            StringBuilder KilledList = new StringBuilder();
            StringBuilder UnreportedList = new StringBuilder();
            int ChannelNumber;
            string BossSpawnTime = "N/A";
            switch (BossID)
            {
                case 1:
                    int KzarkaListStartIndex = 0;
                    int KzarkaListEndIndex = 3;
                    for (int i = KzarkaListStartIndex; i <= KzarkaListEndIndex; i++)
                    {
                        switch (Type)
                        {
                            case 0:
                                if (isEnableGBRProcessingLog)
                                {
                                    WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                                }
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //KilledListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 0) { KilledList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 0) { KilledList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 0) { KilledList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 0) { KilledList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 0) { KilledList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 0) { KilledList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 0) { KilledList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 1:
                                if(isEnableGBRProcessingLog)
                                {
                                    WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                                }
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //UnreportedListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 100) { UnreportedList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 100) { UnreportedList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 100) { UnreportedList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 100) { UnreportedList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 100) { UnreportedList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 100) { UnreportedList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 100) { UnreportedList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 2:
                                BossSpawnTime = kz_initspawntime.ToString("MM月dd日 HH時mm分ss秒");
                                break;
                        }

                    }
                    break;
                case 2:
                    int KarandaListStartIndex = 4;
                    int KarandaListEndIndex = 7;
                    for (int i = KarandaListStartIndex; i <= KarandaListEndIndex; i++)
                    {
                        switch (Type)
                        {
                            case 0:
                                if (isEnableGBRProcessingLog)
                                {
                                    WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                                }
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //KilledListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 0) { KilledList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 0) { KilledList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 0) { KilledList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 0) { KilledList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 0) { KilledList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 0) { KilledList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 0) { KilledList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 1:
                                if (isEnableGBRProcessingLog)
                                {
                                    WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                                }
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //UnreportedListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 100) { UnreportedList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 100) { UnreportedList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 100) { UnreportedList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 100) { UnreportedList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 100) { UnreportedList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 100) { UnreportedList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 100) { UnreportedList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 2:
                                BossSpawnTime = ka_initspawntime.ToString("MM月dd日 HH時mm分ss秒");
                                break;
                        }

                    }
                    break;
                case 3:
                    int NouverListStartIndex = 8;
                    int NouverListEndIndex = 11;
                    for (int i = NouverListStartIndex; i <= NouverListEndIndex; i++)
                    {
                        switch (Type)
                        {
                            case 0:
                                if (isEnableGBRProcessingLog)
                                {
                                    WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                                }
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //KilledListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 0) { KilledList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 0) { KilledList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 0) { KilledList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 0) { KilledList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 0) { KilledList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 0) { KilledList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 0) { KilledList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 1:
                                if (isEnableGBRProcessingLog)
                                {
                                    WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                                    WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                                }
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //UnreportedListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 100) { UnreportedList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 100) { UnreportedList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 100) { UnreportedList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 100) { UnreportedList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 100) { UnreportedList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 100) { UnreportedList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 100) { UnreportedList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 2:
                                BossSpawnTime = nv_initspawntime.ToString("MM月dd日 HH時mm分ss秒");
                                break;
                        }

                    }
                    break;
                case 4:
                    int KutumListStartIndex = 12;
                    int KutumListEndIndex = 15;
                    for (int i = KutumListStartIndex; i <= KutumListEndIndex; i++)
                    {
                        if (isEnableGBRProcessingLog)
                        {
                            WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                            WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                            WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                            WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                            WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                        }
                        switch (Type)
                        {
                            case 0:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //KilledListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 0) { KilledList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 0) { KilledList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 0) { KilledList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 0) { KilledList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 0) { KilledList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 0) { KilledList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 0) { KilledList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 1:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //UnreportedListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 100) { UnreportedList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 100) { UnreportedList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 100) { UnreportedList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 100) { UnreportedList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 100) { UnreportedList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 100) { UnreportedList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 100) { UnreportedList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 2:
                                BossSpawnTime = ku_initspawntime.ToString("MM月dd日 HH時mm分ss秒");
                                break;
                        }

                    }
                    break;
                case 5:
                    int RednoseListStartIndex = 16;
                    int RednoseListEndIndex = 19;
                    for (int i = RednoseListStartIndex; i <= RednoseListEndIndex; i++)
                    {
                        if (isEnableGBRProcessingLog)
                        {
                            WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                            WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                            WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                            WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                            WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                        }
                        switch (Type)
                        {
                            case 0:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //KilledListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 0) { KilledList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 0) { KilledList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 0) { KilledList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 0) { KilledList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 0) { KilledList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 0) { KilledList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 0) { KilledList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 1:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //UnreportedListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 100) { UnreportedList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 100) { UnreportedList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 100) { UnreportedList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 100) { UnreportedList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 100) { UnreportedList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 100) { UnreportedList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 100) { UnreportedList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 2:
                                BossSpawnTime = rn_initspawntime.ToString("MM月dd日 HH時mm分ss秒");
                                break;
                        }
                    }
                    break;
                case 6:
                    int BhegListStartIndex = 20;
                    int BhegListEndIndex = 23;
                    for (int i = BhegListStartIndex; i <= BhegListEndIndex; i++)
                    {
                        if (isEnableGBRProcessingLog)
                        {
                            WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                            WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                            WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                            WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                            WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                        }
                        switch (Type)
                        {
                            case 0:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //KilledListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 0) { KilledList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 0) { KilledList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 0) { KilledList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 0) { KilledList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 0) { KilledList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 0) { KilledList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 0) { KilledList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 1:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //UnreportedListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 100) { UnreportedList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 100) { UnreportedList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 100) { UnreportedList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 100) { UnreportedList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 100) { UnreportedList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 100) { UnreportedList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 100) { UnreportedList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 2:
                                BossSpawnTime = bh_initspawntime.ToString("MM月dd日 HH時mm分ss秒");
                                break;
                        }
                    }
                    break;
                case 7:
                    int TreeListStartIndex = 24;
                    int TreeListEndIndex = 27;
                    for (int i = TreeListStartIndex; i <= TreeListEndIndex; i++)
                    {
                        if (isEnableGBRProcessingLog)
                        {
                            WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                            WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                            WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                            WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                            WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                        }
                        switch (Type)
                        {
                            case 0:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //KilledListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 0) { KilledList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 0) { KilledList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 0) { KilledList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 0) { KilledList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 0) { KilledList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 0) { KilledList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 0) { KilledList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 1:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //UnreportedListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 100) { UnreportedList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 100) { UnreportedList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 100) { UnreportedList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 100) { UnreportedList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 100) { UnreportedList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 100) { UnreportedList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 100) { UnreportedList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 2:
                                BossSpawnTime = tree_initspawntime.ToString("MM月dd日 HH時mm分ss秒");
                                break;
                        }
                    }
                    break;
                case 8:
                    int MudListStartIndex = 28;
                    int MudListEndIndex = 31;
                    for (int i = MudListStartIndex; i <= MudListEndIndex; i++)
                    {
                        if (isEnableGBRProcessingLog)
                        {
                            WriteLog(DebugMsg.GBRLogging_Balenos + i + BossChannelMapTable[i].Balenos.ToString());
                            WriteLog(DebugMsg.GBRLogging_Serendia + i + BossChannelMapTable[i].Serendia.ToString());
                            WriteLog(DebugMsg.GBRLogging_Calpheon + i + BossChannelMapTable[i].Calpheon.ToString());
                            WriteLog(DebugMsg.GBRLogging_Mediah + i + BossChannelMapTable[i].Mediah.ToString());
                            WriteLog(DebugMsg.GBRLogging_Valencia + i + BossChannelMapTable[i].Valencia.ToString());
                        }
                        switch (Type)
                        {
                            case 0:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //KilledListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 0) { KilledList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 0) { KilledList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 0) { KilledList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 0) { KilledList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 0) { KilledList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 0) { KilledList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 0) { KilledList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 1:
                                ChannelNumber = ConvertChannelNumber(i);
                                //
                                //UnreportedListProcessing
                                //
                                if (BossChannelMapTable[i].Balenos == 100) { UnreportedList.Append("B" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Serendia == 100) { UnreportedList.Append("S" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Calpheon == 100) { UnreportedList.Append("C" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Mediah == 100) { UnreportedList.Append("M" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Valencia == 100) { UnreportedList.Append("V" + ChannelNumber + "/"); }
                                if (BossChannelMapTable[i].Magoria == 100) { UnreportedList.Append("Ma" + ChannelNumber + "/"); }
                                if (ChannelNumber <= 2) { if (BossChannelMapTable[i].Kamasylvia == 100) { UnreportedList.Append("K" + ChannelNumber + "/"); } }
                                break;
                            case 2:
                                BossSpawnTime = mud_initspawntime.ToString("MM月dd日 HH時mm分ss秒");
                                break;
                        }
                    }
                    break;


            }
            string KilledList_r = KilledList.ToString().TrimEnd('/');
            string UnreportedList_r = UnreportedList.ToString().TrimEnd('/');
            if(Type == 0) { return KilledList_r; }
            if(Type == 1) { return UnreportedList_r; }
            if(Type == 2) { return BossSpawnTime; }
            return "0";
        }
        //
        //ShowInternalStatus() ... for debug purpose
        //
        private static void ShowInternalStatus(int BossID)
        {
            switch (BossID)
            {
                case 1:
                    int KzarkaListStartIndex = 0;
                    int KzarkaListEndIndex = 3;
                    for (int i = KzarkaListStartIndex; i <= KzarkaListEndIndex; i++)
                    {
                        string balenos = BossChannelMapTable[i].Balenos.ToString();
                        string serendia = BossChannelMapTable[i].Serendia.ToString();
                        string calpheon = BossChannelMapTable[i].Calpheon.ToString();
                        string mediah = BossChannelMapTable[i].Mediah.ToString();
                        string valencia = BossChannelMapTable[i].Valencia.ToString();
                        string magoria = BossChannelMapTable[i].Magoria.ToString();
                        string kama = BossChannelMapTable[i].Kamasylvia.ToString();
                        WriteLog("[INTERNAL] Balenos ID" + i + " : " + balenos);
                        WriteLog("[INTERNAL] Serendia ID" + i + " : " + serendia);
                        WriteLog("[INTERNAL] Calpheon ID" + i + " : " + calpheon);
                        WriteLog("[INTERNAL] Mediah ID" + i + " : " + mediah);
                        WriteLog("[INTERNAL] Valencia ID" + i + " : " + valencia);
                        WriteLog("[INTERNAL] Magoria ID" + i + " : " + magoria);
                        WriteLog("[INTERNAL] KAMA ID" + i + " : " + kama);
                    }
                    break;
            }
        }
        private static int ConvertChannelNumber(int TableID)
        {
            int[] Channel1 = new int[] { 0, 4, 8, 12, 16, 20, 24, 28, 32, 36 };
            int[] Channel2 = new int[] { 1, 5, 9, 13, 17, 21, 25, 29, 33, 37 };
            int[] Channel3 = new int[] { 2, 6, 10, 14, 18, 22, 26, 30, 34, 38 };
            int[] Channel4 = new int[] { 3, 7, 11, 15, 19, 23, 27, 31, 35, 39 };
            for (int i = 0; i < Channel1.Length; i++)
            {
                if (TableID == Channel1[i]) { return 1; }
            }
            for (int i = 0; i < Channel2.Length; i++)
            {
                if (TableID == Channel2[i]) { return 2; }
            }
            for (int i = 0; i < Channel3.Length; i++)
            {
                if (TableID == Channel3[i]) { return 3; }
            }
            for (int i = 0; i < Channel4.Length; i++)
            {
                if (TableID == Channel4[i]) { return 4; }
            }
            return 0;
        }
        public static string GetBossTimeStamp(int BossID, bool IsFirstCall, bool IsAutoRoutine)
        {
            try
            {
                string return_str = "";
                switch (BossID)
                {
                    case 1: //Kzarka
                        string kz_lastreportedtime;
                        if (!IsAutoRoutine) { kz_lastreportedtime = CalculateElapsedTime(kz_lastreporttime, false).Hours + "時間" + CalculateElapsedTime(kz_lastreporttime, false).Minutes + "分" + CalculateElapsedTime(kz_lastreporttime, false).Seconds + "秒"; }
                        else { kz_lastreportedtime = CalculateElapsedTime(kz_lastreporttime, true).Hours + "時間" + CalculateElapsedTime(kz_lastreporttime, true).Minutes + "分" + CalculateElapsedTime(kz_lastreporttime, true).Seconds + "秒"; }
                        //if (Program.DEBUGMODE)
                        //{
                        //    Program.WriteLog("kz_lastreportedtime_h : " + CalculateElapsedTime(kz_lastreporttime).Hours);
                        //    Program.WriteLog("kz_lastreportedtime_m : " + CalculateElapsedTime(kz_lastreporttime).Minutes);
                        //    Program.WriteLog("kz_lastreportedtime_s : " + CalculateElapsedTime(kz_lastreporttime).Seconds);
                        //}
                        var kz_elapsedtime = CalculateElapsedTime(kz_spawntime, true).Hours + "時間" + CalculateElapsedTime(kz_spawntime, true).Minutes + "分" + CalculateElapsedTime(kz_spawntime, true).Seconds + "秒";
                        return_str = "最終報告から" + kz_lastreportedtime + "経過" + "/" + "沸きから" + kz_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 2: //karanda
                        string ka_lastreportedtime;
                        if (!IsAutoRoutine) { ka_lastreportedtime = CalculateElapsedTime(ka_lastreporttime, false).Hours + "時間" + CalculateElapsedTime(ka_lastreporttime, false).Minutes + "分" + CalculateElapsedTime(ka_lastreporttime, false).Seconds + "秒"; }
                        else { ka_lastreportedtime = CalculateElapsedTime(ka_lastreporttime, true).Hours + "時間" + CalculateElapsedTime(ka_lastreporttime, true).Minutes + "分" + CalculateElapsedTime(ka_lastreporttime, true).Seconds + "秒"; }
                        var ka_elapsedtime = CalculateElapsedTime(ka_spawntime, true).Hours + "時間" + CalculateElapsedTime(ka_spawntime, true).Minutes + "分" + CalculateElapsedTime(ka_spawntime, true).Seconds + "秒";
                        return_str = "最終報告から" + ka_lastreportedtime + "経過" + "/" + "沸きから" + ka_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 3: //ヌーベル（Nouver）
                        var nv_lastreportedtime = CalculateElapsedTime(nv_lastreporttime, false).Hours + "時間" + CalculateElapsedTime(nv_lastreporttime, false).Minutes + "分" + CalculateElapsedTime(nv_lastreporttime, false).Seconds + "秒";
                        var nv_elapsedtime = CalculateElapsedTime(nv_spawntime, true).Hours + "時間" + CalculateElapsedTime(nv_spawntime, true).Minutes + "分" + CalculateElapsedTime(nv_spawntime, true).Seconds + "秒";
                        return_str = "最終報告から" + nv_lastreportedtime + "経過" + "/" + "沸きから" + nv_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 4: //クツム
                        var ku_lastreportedtime = CalculateElapsedTime(ku_lastreporttime, false).Hours + "時間" + CalculateElapsedTime(ku_lastreporttime, false).Minutes + "分" + CalculateElapsedTime(ku_lastreporttime, false).Seconds + "秒";
                        var ku_elapsedtime = CalculateElapsedTime(ku_spawntime, true).Hours + "時間" + CalculateElapsedTime(ku_spawntime, true).Minutes + "分" + CalculateElapsedTime(ku_spawntime, true).Seconds + "秒";
                        return_str = "最終報告から" + ku_lastreportedtime + "経過" + "/" + "沸きから" + ku_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 5: //レッドノーズ
                        string rn_lastreportedtime;
                        if (!IsAutoRoutine) { rn_lastreportedtime = CalculateElapsedTime(rn_lastreporttime, false).Hours + "時間" + CalculateElapsedTime(rn_lastreporttime, false).Minutes + "分" + CalculateElapsedTime(rn_lastreporttime, false).Seconds + "秒"; }
                        else { rn_lastreportedtime = CalculateElapsedTime(rn_lastreporttime, true).Hours + "時間" + CalculateElapsedTime(rn_lastreporttime, true).Minutes + "分" + CalculateElapsedTime(rn_lastreporttime, true).Seconds + "秒"; }
                        var rn_elapsedtime = CalculateElapsedTime(rn_spawntime, true).Hours + "時間" + CalculateElapsedTime(rn_spawntime, true).Minutes + "分" + CalculateElapsedTime(rn_spawntime, true).Seconds + "秒";
                        return_str = "最終報告から" + rn_lastreportedtime + "経過" + "/" + "沸きから" + rn_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 6: //bheg
                        string bh_lastreportedtime;
                        if (!IsAutoRoutine) { bh_lastreportedtime = CalculateElapsedTime(bh_lastreporttime, false).Hours + "時間" + CalculateElapsedTime(bh_lastreporttime, false).Minutes + "分" + CalculateElapsedTime(bh_lastreporttime, false).Seconds + "秒"; }
                        else { bh_lastreportedtime = CalculateElapsedTime(bh_lastreporttime, true).Hours + "時間" + CalculateElapsedTime(bh_lastreporttime, true).Minutes + "分" + CalculateElapsedTime(bh_lastreporttime, true).Seconds + "秒"; }
                        var bh_elapsedtime = CalculateElapsedTime(bh_spawntime, true).Hours + "時間" + CalculateElapsedTime(bh_spawntime, true).Minutes + "分" + CalculateElapsedTime(bh_spawntime, true).Seconds + "秒";
                        return_str = "最終報告から" + bh_lastreportedtime + "経過" + "/" + "沸きから" + bh_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 7:
                        string tree_lastreportedtime;
                        if (!IsAutoRoutine) { tree_lastreportedtime = CalculateElapsedTime(tree_lastreporttime, false).Hours + "時間" + CalculateElapsedTime(tree_lastreporttime, false).Minutes + "分" + CalculateElapsedTime(tree_lastreporttime, false).Seconds + "秒"; }
                        else { tree_lastreportedtime = CalculateElapsedTime(tree_lastreporttime, true).Hours + "時間" + CalculateElapsedTime(tree_lastreporttime, true).Minutes + "分" + CalculateElapsedTime(tree_lastreporttime, true).Seconds + "秒"; }
                        var tree_elapsedtime = CalculateElapsedTime(tree_spawntime, true).Hours + "時間" + CalculateElapsedTime(tree_spawntime, true).Minutes + "分" + CalculateElapsedTime(tree_spawntime, true).Seconds + "秒";
                        return_str = "最終報告から" + tree_lastreportedtime + "経過" + "/" + "沸きから" + tree_elapsedtime + "経過" + "/";
                        if (IsFirstCall)
                        {
                            return_str = "最終報告から0時間0分0秒経過" + "/" + "沸きから0時間0分0秒経過" + "/";
                        }
                        break;
                    case 8:
                        string mud_lastreportedtime;
                        if (!IsAutoRoutine) { mud_lastreportedtime = CalculateElapsedTime(mud_lastreporttime, false).Hours + "時間" + CalculateElapsedTime(mud_lastreporttime, false).Minutes + "分" + CalculateElapsedTime(mud_lastreporttime, false).Seconds + "秒"; }
                        else { mud_lastreportedtime = CalculateElapsedTime(mud_lastreporttime, true).Hours + "時間" + CalculateElapsedTime(mud_lastreporttime, true).Minutes + "分" + CalculateElapsedTime(mud_lastreporttime, true).Seconds + "秒"; }
                        var mud_elapsedtime = CalculateElapsedTime(mud_spawntime, true).Hours + "時間" + CalculateElapsedTime(mud_spawntime, true).Minutes + "分" + CalculateElapsedTime(mud_spawntime, true).Seconds + "秒";
                        return_str = "最終報告から" + mud_lastreportedtime + "経過" + "/" + "沸きから" + mud_elapsedtime + "経過" + "/";
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
        private static void WriteLog(string LogDetails)
        {
            Program.WriteLog(LogDetails);
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
public class InternalStatBuffer
{
    public string Balenos;
    public string Serendia;
    public string Calpheon;
    public string Mediah;
    public string Valencia;
    public string Magoria;
    public string Kamasylvia;
    public InternalStatBuffer(string balenos, string serendia, string calpheon, string mediah, string valencia, string magoria, string kamasylvia)
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
