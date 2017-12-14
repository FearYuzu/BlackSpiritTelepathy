using System;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using System.Collections.Generic;

namespace BlackSpiritTelepathy
{
    class Program
    {
        const string BOT_NAME = "Black Spirit Telepathy"; //:thinking:
        const string BOT_TOKEN_DEV = ""; //Discord BOT トークン（テストサーバ用）
        const string BOT_TOKEN_LIVE = ""; //Discord BOT トークン (ライブサーバー用）
        const ulong CLIENT_GUILDID_DEV = 0;
        const ulong CLIENT_GUILDID_LIVE = 0;
        const string BOSSCALL_CHANNELNAME = "boss-spawn-call";
        const string BOSSSTATUS_CHANNELNAME_JP = "boss-status-jp";
        const string BOSSSTATUS_CHANNELNAME_EN = "boss-status-en";
        const ulong BOSSSTATUS_CHANNELID_JP_DEV = 0;
        const ulong BOSSSTATUS_CHANNELID_JP_LIVE = 0;
        const ulong BOSSSTATUS_CHANNELID_EN_DEV = 0;
        const ulong BOSSSTATUS_CHANNELID_EN_LIVE = 0;
        const char COMMAND_SPLITCHAR = ' '; //半角スペース
        public const bool DEBUGMODE = true; //例外を出力するかどうか リリース時falseにすべき
        public const bool DEVMODE = true; //開発用BOTかライブサーバー用BOTかの切換え (Linux用にビルドする前にFalseにすべき！）
        static int BossSpawnCallCount = 0;
        static int RequiredBossSpawnCallCount;
        static string CurrentReportChannel = "";
        static bool isShowLog = true;
        public static bool isKzarkaAlreadySpawned, isKarandaAlreadySpawned, isNouverAlreadySpawned, isKutumAlreadySpawned, isRednoseAlreadySpawned, isBhegAlreadySpawned, isTreeAlreadySpawned, isMudmanAlreadySpawned, isTargargoAlreadySpawned, isIzabellaAlreadySpawned;
        public static DiscordSocketClient client;
        
        enum BossNameJP { クザカ, カランダ, ヌーベル, クツム, レッドノーズ, ベグ, 愚鈍, マッドマン, タルガルゴ, イザベラ };
        static List<Caller> CallerList = new List<Caller>();
        //
        //
        static void Main(string[] args) => MainAsync().Wait();
        //
        //
        
        static async Task MainAsync()
        {
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
            client = new DiscordSocketClient();
            InitFlags();
            WriteLog(SystemMessageDefine.DiscordAPIInit_JP);
            var token = BOT_TOKEN_DEV;
            await client.LoginAsync(TokenType.Bot, token);
            WriteLog(SystemMessageDefine.DiscordBOTLoggedIn_JP);
            await client.StartAsync();
            WriteLog(SystemMessageDefine.DiscordClientStarted_JP);
            //
            client.MessageReceived += Client_MessageReceived;
            WriteLog(SystemMessageDefine.DiscordBOTIsListening_JP);
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine(SystemMessageDefine.CommandAccepting_JP);
            while (true)
            {
                string input;
                input = Console.ReadLine();
                switch (input)
                {
                    default:
                        isShowLog = false;
                        Console.Clear();
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine(SystemMessageDefine.CommanderModeGuide_JP);
                        break;

                    case "showlog":
                        Console.Clear();
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
                        Console.WriteLine("-------------------------------------------------------");
                        isShowLog = true;
                        Console.WriteLine(SystemMessageDefine.LogShowMode_JP);
                        Console.WriteLine(SystemMessageDefine.CommandAccepting_JP);
                        break;
                    case "quit":
                        Console.Clear();
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
                        Console.WriteLine("-------------------------------------------------------");
                        Environment.Exit(0);
                        //Console.WriteLine(SystemMessageDefine.Shutdown_JP);
                        //switch (input)
                        //{
                        //    default:
                        //        break;
                        //    case "y":
                        //        Environment.Exit(0);
                        //        break;
                        //    case "n":
                        //        break;
                        //}
                        break;
                    case "clearchannelmsg status-jp":
                        Console.Clear();
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
                        Console.WriteLine("-------------------------------------------------------");
                        try
                        {
                            var status_ch = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_JP_DEV);
                            foreach (var Item in await status_ch.GetMessagesAsync(100).Flatten())
                            {
                                await Item.DeleteAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex.ToString());
                            var status_ch = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_JP_DEV);
                            foreach (var Item in await status_ch.GetMessagesAsync(100).Flatten())
                            {
                                await Item.DeleteAsync();
                            }
                        }
                        break;
                    case "clearchannelmsg status-en":
                        Console.Clear();
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
                        Console.WriteLine("-------------------------------------------------------");
                        try
                        {
                            var status_ch_en = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_EN_DEV);
                            foreach (var Item in await status_ch_en.GetMessagesAsync(100).Flatten())
                            {
                                await Item.DeleteAsync();
                            }
                        }
                        catch(Exception ex)
                        {
                            WriteLog(ex.ToString());
                            var status_ch_en = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_EN_DEV);
                            foreach (var Item in await status_ch_en.GetMessagesAsync(100).Flatten())
                            {
                                await Item.DeleteAsync();
                            }
                        }
                        break;
                    case "change requiredspawncount":
                        Console.Clear();
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine(SystemMessageDefine.ChangeRequiredSpawnCount_JP);
                        var input2 = Console.ReadLine();
                        switch (input2)
                        {
                            default:
                                try
                                {
                                    RequiredBossSpawnCallCount = int.Parse(input2);
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                }
                                Console.WriteLine(SystemMessageDefine.ChangedRequiredSpawnCount_JP);
                                break;
                            
                        }
                        break;
                    case "check requiredspawncount":
                        Console.Clear();
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("ボス状況テーブル展開に必要な、現在設定されている報告必要数：" + RequiredBossSpawnCallCount.ToString());
                        break;
                    case "sendmsg general":
                        Console.Clear();
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("Black Spirit Telepathy BOT Server v0.01");
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine(SystemMessageDefine.SendMessage_JP);
                        var input3 = Console.ReadLine();
                        switch (input3)
                        {
                            default:
                                try
                                {
                                    
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                }
                                Console.WriteLine(SystemMessageDefine.ChangedRequiredSpawnCount_JP);
                                break;

                        }
                        break;
                }
            }
        }
        static void InitFlags()
        {
            RequiredBossSpawnCallCount = 1;
            isKzarkaAlreadySpawned = false;
            isKarandaAlreadySpawned = false;
            isNouverAlreadySpawned = false;
            isKutumAlreadySpawned = false;
            isRednoseAlreadySpawned = false;
            isBhegAlreadySpawned = false;
            isTreeAlreadySpawned = false;
            isMudmanAlreadySpawned = false;
            isTargargoAlreadySpawned = false;
            isIzabellaAlreadySpawned = false;
        }
        static async Task Client_MessageReceived(SocketMessage arg)
        {
            if (!arg.Author.Username.Equals(BOT_NAME) && arg.Channel.Name == BOSSCALL_CHANNELNAME) //ボス湧き通知。boss-spawn-callチャンネルでのみ有効
            {
                string[] CommandFields = new string[] { };
                var BossType = 0;
                var CommandArg = 0;
                var CommandArg2 = 0;
                //Console.WriteLine(isShowLog);
                try
                {
                    CommandFields = arg.Content.Split(COMMAND_SPLITCHAR);
                    BossType = BossIdentify(CommandFields[0]);
                    CommandArg = CommandArgIdentify(CommandFields[1]);
                }
                catch(Exception ex)
                {
                    var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                    await dmchannel.SendMessageAsync(MessageDefine.InvalidCommand_JP,false);
                    if (DEBUGMODE)
                    {
                        await dmchannel.SendMessageAsync("--------------------Exception Debug Log--------------------");
                        await dmchannel.SendMessageAsync(ex.ToString());
                        await dmchannel.SendMessageAsync("-----------------------------------------------------------");
                    }
                }
                try
                {
                    WriteLog(arg.Author.Username + "が入力 : " + CommandFields[0] + " " + CommandFields[1] + " ");
                    
                }
                catch(System.IndexOutOfRangeException ex)
                {
                    WriteLog(arg.Author.Username + "が無効なコマンドを入力 : " + arg.Content);
                    WriteLog(ex.ToString());
                }
                catch(Exception ex)
                {
                    WriteLog(ex.ToString());
                }
                
                switch (BossType) //ボス種類はなあに？
                {
                    case 1: //クザカだ！
                        switch (CommandArg) //クザカに対するコマンドは？
                        {

                            case 101: //クザカ沸き報告！
                                BossSpawnCallCount++;
                                if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                {
                                    CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username,1)); //通知したユーザーを通知済みリストに追加する
                                    WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                }
                                //
                                if (!isKzarkaAlreadySpawned)
                                {
                                    if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                    {
                                        //クザカ沸き確認。クザカボス状況テーブル展開
                                        await arg.Channel.SendMessageAsync(MessageDefine.KzarkaSpawnMessage_JP);
                                        WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.クザカ.ToString());
                                        BossSpawnCallCount = 0;
                                        isKzarkaAlreadySpawned = true;
                                        //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                        try
                                        {
                                            var status_ch = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_JP_DEV);
                                            var status_ch_en = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_EN_DEV);
                                            await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.CreateStatus(1));
                                        }
                                        catch (Exception ex)
                                        {
                                            WriteLog(ex.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    WriteLog(SystemMessageDefine.KzarkaAlreadySpawned_JP);
                                }
                                break;
                            case 102:
                                break;
                            case 105: //ボス体力通知だ！
                                switch (CommandArg2) //ボスの体力値は？
                                {
                                    default: 
                                        if (CommandArg2 > 100 || CommandArg2 < 1) //入力値が1~99までなら
                                        {
                                            BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2);
                                        }
                                        break;
                                    case 103:
                                        BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0);
                                        break;
                                }
                                break;
                        }



                        break;
                    case 2: //カランダだ！
                        switch (CommandArg)
                        {
                            case 101:
                                BossSpawnCallCount++;
                                if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                {
                                    CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username,2)); //通知したユーザーを通知済みリストに追加する
                                    WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                }
                                //
                                if (!isKarandaAlreadySpawned)
                                {
                                    if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                    {
                                        //カランダ沸き確認。クザカボス状況テーブル展開
                                        await arg.Channel.SendMessageAsync(MessageDefine.KzarkaSpawnMessage_JP);
                                        WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.カランダ.ToString());
                                        BossSpawnCallCount = 0;
                                        isKarandaAlreadySpawned = true;
                                        //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                        try
                                        {
                                            var status_ch = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_JP_DEV);
                                            var status_ch_en = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_EN_DEV);
                                            await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(2));
                                            await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.CreateStatus(2));
                                        }
                                        catch (Exception ex)
                                        {
                                            WriteLog(ex.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    WriteLog(SystemMessageDefine.KzarkaAlreadySpawned_JP);
                                }
                                break;
                            case 102:
                                break;
                            case 105: //ボス体力通知だ！
                                switch (CommandArg2) //ボスの体力値は？
                                {
                                    default:
                                        if (CommandArg2 > 100 || CommandArg2 < 1) //入力値が1~99までなら
                                        {
                                            BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2);
                                        }
                                        break;
                                    case 103:
                                        BossStatus.ChangeStatusValue(2, CurrentReportChannel, 0);
                                        break;
                                }
                                break;
                        }
                        break;
                }
                
                //if(arg.Content.Contains("kzarka spawn"))
                //{
                //    BossSpawnCallCount++;
                //    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                //    {
                //        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username)); //通知したユーザーを通知済みリストに追加する
                //        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                //    }
                //    //
                //    if(CallerList.Count >= RequiredBossSpawnCallCount)
                //    {
                //        await arg.Channel.SendMessageAsync(MessageDefine.KzarkaSpawnMessage_JP);
                //        WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.クザカ.ToString());
                //    }
                //}
            }
            if (!arg.Author.Username.Equals(BOT_NAME) && arg.Channel.Name == BOSSSTATUS_CHANNELNAME_JP) //ボス状況通知。boss-statusチャンネルでのみ有効
            {
                string[] CommandFields = new string[] { };
                var BossType = 0;
                var CommandArg = 0;
                var CommandArg2 = 0;
                var status_ch_en = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_EN_DEV);
                //Console.WriteLine(isShowLog);
                try
                {
                    CommandFields = arg.Content.Split(COMMAND_SPLITCHAR);
                    BossType = BossIdentify(CommandFields[0]);
                    CommandArg = CommandArgIdentify(CommandFields[1]);
                    CommandArg2 = CommandArg2Identify(CommandFields[2]);
                }
                catch (Exception ex)
                {
                    var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                    await dmchannel.SendMessageAsync(MessageDefine.InvalidCommand_JP, false);
                    if (DEBUGMODE)
                    {
                        await dmchannel.SendMessageAsync(ex.Message);
                        await dmchannel.SendMessageAsync(ex.StackTrace);
                        await dmchannel.SendMessageAsync(ex.InnerException.ToString());
                        await dmchannel.SendMessageAsync(ex.TargetSite.ToString());
                    }
                }
                try
                {
                    WriteLog(arg.Author.Username + "が入力 : " + CommandFields[0] + " " + CommandFields[1] + " " + CommandFields[2]);
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    WriteLog(arg.Author.Username + "が無効なコマンドを入力 : " + arg.Content);
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                }

                switch (BossType) //ボス種類はなあに？
                {
                    case 1: //クザカだ！
                        switch (CommandArg) //クザカに対するコマンドは？
                        {

                            case 101: //クザカ沸いた！
                                BossSpawnCallCount++;
                                if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                {
                                    CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username,1)); //通知したユーザーを通知済みリストに追加する
                                    WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                }
                                //
                                if (CallerList.Count >= RequiredBossSpawnCallCount)
                                {
                                    await arg.Channel.SendMessageAsync(MessageDefine.KzarkaSpawnMessage_JP);
                                    WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.クザカ.ToString());
                                    BossSpawnCallCount = 0;
                                }
                                break;
                            case 102:
                                break;
                            case 105: //ボス体力通知だ！
                                switch (CommandArg2) //ボスの体力値は？
                                {
                                    default:
                                        if (CommandArg2 < 100 || CommandArg2 > 1) //入力値が1~99までなら
                                        {
                                            await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                            
                                            //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                        }
                                        else
                                        {
                                            var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                            await dmchannel.SendMessageAsync("105 " + MessageDefine.InvalidCommand_JP, false);
                                        }
                                        break;
                                    case 103:
                                        await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                        //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                        break;
                                }
                                break;
                        }
                        break;
                    case 2: //Karanda
                        switch (CommandArg)
                        {
                            case 105: //ボス体力通知だ！
                                switch (CommandArg2) //ボスの体力値は？
                                {
                                    default:
                                        if (CommandArg2 < 100 || CommandArg2 > 1) //入力値が1~99までなら
                                        {
                                            await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));

                                            //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                        }
                                        else
                                        {
                                            var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                            await dmchannel.SendMessageAsync("105 " + MessageDefine.InvalidCommand_JP, false);
                                        }
                                        break;
                                    case 103:
                                        await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, 0));
                                        //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                        break;
                                }
                                break;
                        }
                        break;
                }
                //if(arg.Content.Contains("kzarka spawn"))
                //{
                //    BossSpawnCallCount++;
                //    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                //    {
                //        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username)); //通知したユーザーを通知済みリストに追加する
                //        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                //    }
                //    //
                //    if(CallerList.Count >= RequiredBossSpawnCallCount)
                //    {
                //        await arg.Channel.SendMessageAsync(MessageDefine.KzarkaSpawnMessage_JP);
                //        WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.クザカ.ToString());
                //    }
                //}
            }

        }
        //
        // -isCallerAlreadyInList-
        //ボス湧き通知ユーザーが既に通知済みユーザーリストに居ないか検索（重複通知防止）
        // return trueで既に通知済み、通知は無効。return falseで通知有効。
        //
        public static bool isCallerAlreadyInList(string SearchName)
        {
            for (int i = 0; i < CallerList.Count; i++)
            {
                if (CallerList[i].CallerName == SearchName)
                {
                    return true;
                }

            }
            return false;
        }
        // -BossIdentify-
        //コマンド中のボスの種類を識別する。
        //返り値：1=クザカ,2=カランダ,3=ヌーベル,4=クツム,5=レッドノーズ,6=ベグ,7=愚鈍,8=マッドマン,9=タルガルゴ(有効無効制御可)10=イザベラ(有効無効制御可)
        //
        static int BossIdentify(string cmdfields1)
        {
            string[] KzarkaIdentify = new string[] { "クザカ", "kzarka", "kz" };
            string[] KarandaIdentify = new string[] { "カランダ", "karanda", "Karanda", "ka", "kar" };
            string[] NouverIdentify = new string[] { "ヌーベル", "Nouver", "nouver", "nv" };
            string[] KutumIdentify = new string[] { "クツム", "Kutum", "kutum", "ku", "kut" };

            for (int i = 0; i < KzarkaIdentify.Length; i++)
            {
                if (cmdfields1 == KzarkaIdentify[i])
                {
                    return 1;
                }

            }
            for (int i = 0; i < KarandaIdentify.Length; i++)
            {
                if(cmdfields1 == KarandaIdentify[i])
                {
                    return 2;
                }
            }
            return 0;
        }
        static int CommandArgIdentify(string cmdfields2)
        {
            string[] HelpIdentify = new string[] { "help", "!h", "ヘルプ" };
            string[] SpawnIdentify = new string[] { "湧き", "沸き", "Spawn", "spawn" };
            //int[] PercentIdentify = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
            string[] UndoIdentify = new string[] { "修正", "Undo", "undo" };
            string[] KilledIdentify = new string[] { "没", "終わり", "終了", "Dead", "dead", "d" };
            string[] DespawnedIdentify = new string[] { "消え","消滅", "居ない", "Despawn", "des" };
            string[] ChannelIdentify = new string[] { "b1", "b2", "b3", "b4", "s1", "s2", "s3", "s4", "c1", "c2", "c3", "c4", "m1", "m2", "m3", "m4", "v1", "v2", "v3", "v4", "ma1", "ma2", "ma3", "ma4", "k1", "k2" };

            for (int i = 0; i < SpawnIdentify.Length; i++)
            {
                if(cmdfields2 == SpawnIdentify[i])
                {
                    return 101;
                }
            }
            for (int i = 0; i < ChannelIdentify.Length; i++)
            {
                if (cmdfields2 == ChannelIdentify[i])
                {
                    CurrentReportChannel = ChannelIdentify[i];
                    return 105;
                }
            }
            for (int i = 0; i < UndoIdentify.Length; i++)
            {
                if (cmdfields2 == UndoIdentify[i])
                {
                    return 102;
                }
            }
            for (int i = 0; i < KilledIdentify.Length; i++)
            {
                if (cmdfields2 == KilledIdentify[i])
                {
                    return 103;
                }
            }
            for (int i = 0; i < DespawnedIdentify.Length; i++)
            {
                if (cmdfields2 == DespawnedIdentify[i])
                {
                    return 104;
                }
            }
            return 106;
        }
        static int CommandArg2Identify(string cmdfields3)
        {
            string[] SpawnIdentify = new string[] { "湧き", "沸き", "Spawn", "spawn" };
            int[] PercentIdentify = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
            string[] UndoIdentify = new string[] { "修正", "Undo", "undo" };
            string[] KilledIdentify = new string[] { "没", "終わり", "終了", "Dead", "dead", "d" };
            string[] DespawnedIdentify = new string[] { "消え", "消滅", "居ない", "Despawn", "des" };
            string[] ChannelIdentify = new string[] { "b1", "b2", "b3", "b4", "s1", "s2", "s3", "s4", "c1", "c2", "c3", "c4", };
            
            for (int i = 0; i < SpawnIdentify.Length; i++)
            {
                if (cmdfields3 == SpawnIdentify[i])
                {
                    return 101;
                }
            }
            
            for (int i = 0; i < UndoIdentify.Length; i++)
            {
                if (cmdfields3 == UndoIdentify[i])
                {
                    return 102;
                }
            }
            for (int i = 0; i < KilledIdentify.Length; i++)
            {
                if (cmdfields3 == KilledIdentify[i])
                {
                    return 103;
                }
            }
            for (int i = 0; i < DespawnedIdentify.Length; i++)
            {
                if (cmdfields3 == DespawnedIdentify[i])
                {
                    return 104;
                }
            }
            for (int i = 0; i < PercentIdentify.Length; i++)
            {
                if (int.Parse(cmdfields3) == PercentIdentify[i])
                {
                    return PercentIdentify[i];
                }
            }
            return 105;
        }
        public static void WriteLog(string LogDetails)
        {
            if (isShowLog)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "  " + LogDetails);
            }
        }
        
    }
    public class Caller
    {
        public int CallerID;
        public int CalledBossID;
        public string CallerName;
        public Caller(int callerid, string callername, int calledbossid)
        {
            CallerID = callerid;
            CallerName = callername;
            CalledBossID = calledbossid;
        }
    }
}
