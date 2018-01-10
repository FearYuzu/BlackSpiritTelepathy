using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using System.Collections.Generic;

namespace BlackSpiritTelepathy
{
    class Program
    {
        const string SERVER_VERSION = "v1.012";
        const string BOT_NAME = "Black Spirit Telepathy"; //:thinking:
        const string BOT_TOKEN_DEV = ""; //Discord BOT トークン（テストサーバ用）
        const string BOT_TOKEN_LIVE = ""; //Discord BOT トークン (ライブサーバー用）
        const ulong CLIENT_GUILDID_DEV = 384935765194440705;
        const ulong CLIENT_GUILDID_LIVE = 386429936359178240;
        const string BOSSCALL_CHANNELNAME = "boss-spawn-call";
        const string BOSSSTATUS_CHANNELNAME_JP = "boss-status";
        const string BOSSSTATUS_CHANNELNAME_EN = "boss-status-en";
        const ulong GENERAL_CHANNELID_JP_DEV = 3;
        const ulong GENERAL_CHANNELID_JP_LIVE = 3;
        const ulong BOSSSTATUS_CHANNELID_JP_DEV = 3;
        const ulong BOSSSTATUS_CHANNELID_JP_LIVE = 3;
        const ulong BOSSSTATUS_CHANNELID_EN_DEV = 3;
        const ulong BOSSSTATUS_CHANNELID_EN_LIVE = 0;
        const char COMMAND_SPLITCHAR = ' '; //半角スペース
        public const bool DEBUGMODE = true; //例外を出力するかどうか リリース時falseにすべき
        public static bool DEVMODE = false; //開発用BOTかライブサーバー用BOTかの切換え (Linux用にビルドする前にFalseにすべき！）
        static int BossSpawnCallCount = 0;
        static int RequiredBossSpawnCallCount;
        static string CurrentReportChannel = "";
        static bool isShowLog = true;
        static List<ulong> LastBotMessageBuffer = new List<ulong>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static bool isTestAlreadySpawned, isKzarkaAlreadySpawned, isKarandaAlreadySpawned, isNouverAlreadySpawned, isKutumAlreadySpawned, isRednoseAlreadySpawned, isBhegAlreadySpawned, isTreeAlreadySpawned, isMudmanAlreadySpawned, isTargargoAlreadySpawned, isIzabellaAlreadySpawned;
        public static DiscordSocketClient client;
        public static SocketTextChannel status_ch;
        public static SocketTextChannel general_ch;
        
        enum BossNameJP {クザカ, カランダ, ヌーベル, クツム, レッドノーズ, ベグ, 愚鈍, マッドマン, タルガルゴ, イザベラ };
        static List<Caller> CallerList = new List<Caller>();
        //
        //
        static void Main(string[] args) => MainAsync().Wait();
        //
        //
        //コンソール操作部（Console Operation)
        //
        static async Task MainAsync()
        {
            try
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Black Spirit Telepathy BOT Server {0}", SERVER_VERSION);
                client = new DiscordSocketClient();
                WriteLog(SystemMessageDefine.DiscordAPIInit_JP);
                if (DEVMODE && client != null)
                {
                    try
                    {
                        await client.LoginAsync(TokenType.Bot, BOT_TOKEN_DEV); //開発用サーバにログイン
                    }
                    catch (Exception ex)
                    {
                        WriteLog(SystemMessageDefine.FailedToAPILogin + "\n" + ex.ToString());
                    }
                    WriteLog(SystemMessageDefine.LaunchedAsDevelopment);
                }
                else if (client != null)
                {
                    try
                    {
                        await client.LoginAsync(TokenType.Bot, BOT_TOKEN_LIVE); //ライブサーバにログイン
                    }
                    catch (Exception ex)
                    {
                        WriteLog(SystemMessageDefine.FailedToAPILogin + "\n" + ex.ToString());
                    }
                    WriteLog(SystemMessageDefine.LaunchedAsLiveService);
                }
                
                WriteLog(SystemMessageDefine.DiscordBOTLoggedIn_JP);
                await client.StartAsync();
                WriteLog(SystemMessageDefine.DiscordClientStarted_JP);
                InitFlags();
                BossStatus.InitStatus();
                //
                client.MessageReceived += Client_MessageReceived;
                WriteLog(SystemMessageDefine.DiscordBOTIsListening_JP);
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine(SystemMessageDefine.CommandAccepting_JP);
                //
                //Command Accepting
                //
                //AcceptCommand();
                try
                {
                    
                    //WriteLog(status_ch.ToString());
                    //WriteLog(client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_JP_DEV).ToString());
                    //Console.ReadLine();
                    while (true)
                    {
                        string input;
                        input = Console.ReadLine();
                        switch (input)
                        {
                            default:
                                isShowLog = false;
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine(SystemMessageDefine.CommanderModeGuide_JP);
                                break;

                            case "showlog":
                                Console.Clear();
                                VersionHeader();
                                isShowLog = true;
                                Console.WriteLine(SystemMessageDefine.LogShowMode_JP);
                                Console.WriteLine(SystemMessageDefine.CommandAccepting_JP);
                                break;
                            case "quit":
                                Console.Clear();
                                VersionHeader();
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
                                VersionHeader();
                                try
                                {
                                    foreach (var Item in await status_ch.GetMessagesAsync(100).Flatten())
                                    {
                                        if (Item.Author.IsBot)
                                        {
                                            await Item.DeleteAsync();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                }
                                break;
                            case "change requiredspawncount":
                                Console.Clear();
                                VersionHeader();
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
                                VersionHeader();
                                Console.WriteLine("ボス状況テーブル展開に必要な、現在設定されている報告必要数：" + RequiredBossSpawnCallCount.ToString());
                                break;
                            case "disable kzarka":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("Kzarka Spawn Disabled");
                                isKzarkaAlreadySpawned = false;
                                break;
                            case "disable karanda":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("karanda Spawn Disabled");
                                isKarandaAlreadySpawned = false;
                                break;
                            case "disable nouver":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("Nouver Spawn Disabled");
                                isNouverAlreadySpawned = false;
                                break;
                            case "disable kutum":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("Kutum Spawn Disabled");
                                isKutumAlreadySpawned = false;
                                break;
                            case "disable rednose":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("Rednose Spawn Disabled");
                                isRednoseAlreadySpawned = false;
                                break;
                            case "disable bheg":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("Bheg Spawn Disabled");
                                isBhegAlreadySpawned = false;
                                break;
                            case "disable tree":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("Tree Spawn Disabled");
                                isTreeAlreadySpawned = false;
                                break;
                            case "disable mud":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("Mud Spawn Disabled");
                                isMudmanAlreadySpawned = false;
                                break;
                            case "sendmsg general":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine(SystemMessageDefine.SendMessage_JP);
                                var input3 = Console.ReadLine();
                                switch (input3)
                                {
                                    default:
                                        try
                                        {
                                            await general_ch.SendMessageAsync(input3);
                                        }
                                        catch (Exception ex)
                                        {
                                            WriteLog(ex.ToString());
                                        }
                                        Console.WriteLine(SystemMessageDefine.ChangedRequiredSpawnCount_JP);
                                        break;

                                }
                                break;
                            case "getmessage":
                                Console.Clear();
                                VersionHeader();
                                Console.WriteLine("Enter Message ID");
                                
                                var msgid = Console.ReadLine();
                                switch (msgid)
                                {
                                    default:
                                        try
                                        {
                                            var msg = ulong.Parse(msgid);
                                            var msg2 = status_ch.GetMessageAsync(msg);
                                            Console.WriteLine(msg2.Result);
                                        }
                                        catch (Exception ex)
                                        {
                                            WriteLog(ex.ToString());
                                        }
                                        Console.WriteLine("Enter to back menu");
                                        break;

                                }
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog("Exception on MainAsync() " + ex.ToString());
            }
        }
        static void VersionHeader() //バージョンヘッダ
        {
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Black Spirit Telepathy BOT Server {0}",SERVER_VERSION);
            Console.WriteLine("-------------------------------------------------------");
        }
        static void AcceptCommand()
        {
            
        }
        public static void WriteLog(string LogDetails)
        {
            if (isShowLog)
            {
                if(!File.Exists("bot.log")) { File.Create("bot.log").Close(); }
                StreamWriter sw = new StreamWriter("bot.log", true);
                Console.WriteLine("{0}  {1}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), LogDetails);
                string logoutput = string.Format("{0}  {1}", DateTime.Now.ToString("yyyy/MM/dd/ hh:mm:ss"), LogDetails);
                sw.WriteLine(logoutput);
                sw.Close();
            }
        }
        static void InitFlags()
        {
            try
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
                System.Threading.Thread.Sleep(2000); //<------------REQUIRED! DO NOT REMOVE(´・ω・`)
                
                if (DEVMODE)
                {
                    status_ch = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(BOSSSTATUS_CHANNELID_JP_DEV);
                    general_ch = client.GetGuild(CLIENT_GUILDID_DEV).GetTextChannel(GENERAL_CHANNELID_JP_DEV);
                }
                else
                {
                    status_ch = client.GetGuild(CLIENT_GUILDID_LIVE).GetTextChannel(BOSSSTATUS_CHANNELID_JP_LIVE);
                    general_ch = client.GetGuild(CLIENT_GUILDID_LIVE).GetTextChannel(GENERAL_CHANNELID_JP_LIVE);
                }

            }
            catch(Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }
        //
        //
        //Discordコマンド処理部（Discord Command Processing）
        //
        //
        static async Task Client_MessageReceived(SocketMessage arg)
        {
            try
            {
                if (!arg.Author.Username.Equals(BOT_NAME) && arg.Channel.Name == BOSSCALL_CHANNELNAME) //ボス湧き通知。boss-spawn-callチャンネルでのみ有効
                {
                    string[] CommandFields = new string[] { };
                    var BossType = 0;
                    var CommandArg = 0;
                    var eb = new EmbedBuilder();
                    var ebb = new EmbedFieldBuilder();
                    var cond = arg as SocketUserMessage;
                    //Console.WriteLine(isShowLog);

                    CommandFields = arg.Content.Split(COMMAND_SPLITCHAR);
                    if (CommandFields[0] != null) { BossType = BossIdentify(CommandFields[0]); }
                    if (CommandFields[1] != null) { CommandArg = CommandArgIdentify(CommandFields[1]); }
                    WriteLog(arg.Author.Username + "が入力 : " + arg.Content);
                    ////////////////////////////////////////////////////////////////////////

                    switch (BossType) //ボス種類はなあに？
                    {
                        case 1: //クザカだ！
                            switch (CommandArg) //クザカに対するコマンドは？
                            {

                                case 101: //クザカ沸き報告！
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 1)); //通知したユーザーを通知済みリストに追加する
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

                                                eb.WithTitle(BossNameJP.クザカ.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(1));
                                                eb.WithThumbnailUrl(ResourceDefine.KzarkaThumbURI);
                                                eb.WithColor(Color.Red);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                if (DEBUGMODE)
                                                {
                                                    WriteLog(BossStatus.GetBossTimeStamp(1, true));
                                                }

                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(1);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
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
                            }
                            break;
                        case 2: //カランダだ！
                            switch (CommandArg)
                            {
                                case 101:
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 2)); //通知したユーザーを通知済みリストに追加する
                                        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                    }
                                    //
                                    if (!isKarandaAlreadySpawned)
                                    {
                                        if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                        {
                                            //カランダ沸き確認。クザカボス状況テーブル展開
                                            await arg.Channel.SendMessageAsync(MessageDefine.KarandaSpawnMessage_JP);
                                            WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.カランダ.ToString());
                                            BossSpawnCallCount = 0;
                                            isKarandaAlreadySpawned = true;
                                            //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            try
                                            {
                                                eb.WithTitle(BossNameJP.カランダ.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(2));
                                                eb.WithThumbnailUrl(ResourceDefine.KarandaThumbURI);
                                                eb.WithColor(Color.DarkBlue);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(2);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLog(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WriteLog(SystemMessageDefine.KarandaAlreadySpawned_JP);
                                    }
                                    break;

                            }
                            break;
                        case 3: //ヌーベル
                            switch (CommandArg)
                            {
                                case 101:
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 3)); //通知したユーザーを通知済みリストに追加する
                                        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                    }
                                    //
                                    if (!isNouverAlreadySpawned)
                                    {
                                        if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                        {
                                            //カランダ沸き確認。クザカボス状況テーブル展開
                                            await arg.Channel.SendMessageAsync(MessageDefine.NouverSpawnMessage_JP);
                                            WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.ヌーベル.ToString());
                                            BossSpawnCallCount = 0;
                                            isNouverAlreadySpawned = true;
                                            //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            try
                                            {
                                                eb.WithTitle(BossNameJP.ヌーベル.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(3));
                                                eb.WithThumbnailUrl(ResourceDefine.NouverThumbURI);
                                                eb.WithColor(Color.DarkOrange);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(3);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLog(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WriteLog(SystemMessageDefine.NouverAlreadySpawned_JP);
                                    }
                                    break;

                            }
                            break;
                        case 4: //クツム（Kutum）
                            switch (CommandArg)
                            {
                                case 101:
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 4)); //通知したユーザーを通知済みリストに追加する
                                        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                    }
                                    //
                                    if (!isKutumAlreadySpawned)
                                    {
                                        if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                        {
                                            //カランダ沸き確認。クザカボス状況テーブル展開
                                            await arg.Channel.SendMessageAsync(MessageDefine.KutumSpawnMessage_JP);
                                            WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.クツム.ToString());
                                            BossSpawnCallCount = 0;
                                            isKutumAlreadySpawned = true;
                                            //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            try
                                            {
                                                eb.WithTitle(BossNameJP.クツム.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(4));
                                                eb.WithThumbnailUrl(ResourceDefine.KutumThumbURI);
                                                eb.WithColor(Color.DarkPurple);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(4);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLog(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WriteLog(SystemMessageDefine.KutumAlreadySpawned_JP);
                                    }
                                    break;

                            }
                            break;
                        case 5: //レッドノーズ（Rednose)
                            switch (CommandArg)
                            {
                                case 101:
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 5)); //通知したユーザーを通知済みリストに追加する
                                        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                    }
                                    //
                                    if (!isRednoseAlreadySpawned)
                                    {
                                        if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                        {
                                            //カランダ沸き確認。クザカボス状況テーブル展開
                                            await arg.Channel.SendMessageAsync(MessageDefine.RednoseSpawnMessage_JP);
                                            WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.レッドノーズ.ToString());
                                            BossSpawnCallCount = 0;
                                            isRednoseAlreadySpawned = true;
                                            //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            try
                                            {
                                                eb.WithTitle(BossNameJP.レッドノーズ.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(5));
                                                eb.WithThumbnailUrl(ResourceDefine.RednoseThumbURI);
                                                eb.WithColor(Color.DarkGrey);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(5);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLog(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WriteLog(SystemMessageDefine.RednoseAlreadySpawned_JP);
                                    }
                                    break;

                            }
                            break;
                        case 6: //ベグ（Bheg)
                            switch (CommandArg)
                            {
                                case 101:
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 6)); //通知したユーザーを通知済みリストに追加する
                                        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                    }
                                    //
                                    if (!isNouverAlreadySpawned)
                                    {
                                        if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                        {
                                            //カランダ沸き確認。クザカボス状況テーブル展開
                                            await arg.Channel.SendMessageAsync(MessageDefine.BhegSpawnMessage_JP);
                                            WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.ベグ.ToString());
                                            BossSpawnCallCount = 0;
                                            isBhegAlreadySpawned = true;
                                            //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            try
                                            {
                                                eb.WithTitle(BossNameJP.ベグ.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(6));
                                                eb.WithThumbnailUrl(ResourceDefine.BhegThumbURI);
                                                eb.WithColor(Color.DarkTeal);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(6);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLog(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WriteLog(SystemMessageDefine.BhegAlreadySpawned_JP);
                                    }
                                    break;

                            }
                            break;
                        case 7: //愚鈍（Tree)
                            switch (CommandArg)
                            {
                                case 101:
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 7)); //通知したユーザーを通知済みリストに追加する
                                        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                    }
                                    //
                                    if (!isTreeAlreadySpawned)
                                    {
                                        if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                        {
                                            //カランダ沸き確認。クザカボス状況テーブル展開
                                            await arg.Channel.SendMessageAsync(MessageDefine.TreeSpawnMessage_JP);
                                            WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.ヌーベル.ToString());
                                            BossSpawnCallCount = 0;
                                            isTreeAlreadySpawned = true;
                                            //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            try
                                            {
                                                eb.WithTitle(BossNameJP.愚鈍.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(7));
                                                eb.WithThumbnailUrl(ResourceDefine.TreeThumbURI);
                                                eb.WithColor(Color.DarkGreen);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(7);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLog(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WriteLog(SystemMessageDefine.DimtreeAlreadySpawned_JP);
                                    }
                                    break;

                            }
                            break;
                        case 8: //マッドマン（Mudman）
                            switch (CommandArg)
                            {
                                case 101:
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 8)); //通知したユーザーを通知済みリストに追加する
                                        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                    }
                                    //
                                    if (!isMudmanAlreadySpawned)
                                    {
                                        if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                        {
                                            //カランダ沸き確認。クザカボス状況テーブル展開
                                            await arg.Channel.SendMessageAsync(MessageDefine.MudSpawnMessage_JP);
                                            WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.マッドマン.ToString());
                                            BossSpawnCallCount = 0;
                                            isMudmanAlreadySpawned = true;
                                            //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            try
                                            {
                                                eb.WithTitle(BossNameJP.マッドマン.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(8));
                                                eb.WithThumbnailUrl(ResourceDefine.MudThumbURI);
                                                eb.WithColor(Color.LightGrey);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(8);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLog(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WriteLog(SystemMessageDefine.MudmanAlreadySpawned_JP);
                                    }
                                    break;

                            }
                            break;
                        case 20: //Test Boss
                            switch (CommandArg)
                            {
                                case 101:
                                    BossSpawnCallCount++;
                                    if (!isCallerAlreadyInList(arg.Author.Username)) //重複通知でなければ
                                    {
                                        CallerList.Add(new Caller(BossSpawnCallCount, arg.Author.Username, 20)); //通知したユーザーを通知済みリストに追加する
                                        WriteLog(SystemMessageDefine.CallerAddedInList_JP + arg.Author.Username);
                                    }
                                    //
                                    if (!isTestAlreadySpawned)
                                    {
                                        if (CallerList.Count >= RequiredBossSpawnCallCount) //報告数が規定値以上であれば
                                        {
                                            //カランダ沸き確認。クザカボス状況テーブル展開
                                            await arg.Channel.SendMessageAsync(MessageDefine.TestSpawnMessage_JP);
                                            WriteLog(SystemMessageDefine.BossSpawnConfirmed_JP + BossNameJP.マッドマン.ToString());
                                            BossSpawnCallCount = 0;
                                            isTestAlreadySpawned = true;
                                            //await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP + "\n" + BossStatus.CreateStatus(1));
                                            try
                                            {
                                                eb.WithTitle("ライブサーバーテスト");
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.CreateStatus(20));
                                                eb.WithThumbnailUrl(ResourceDefine.MudThumbURI);
                                                eb.WithColor(Color.LightGrey);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                MemoryBotMessageIdToBuffer(20);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN, false, eb);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLog(ex.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WriteLog(SystemMessageDefine.MudmanAlreadySpawned_JP);
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
                    string[] CommandFields = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                    var BossType = 0;
                    var CommandArg = 0;
                    var CommandArg2 = 0;
                    try
                    {
                        var eb = new EmbedBuilder();
                        var ebb = new EmbedFieldBuilder();
                        //入力値チェック（例外予防）| Input Value Check for prevent Exception.
                        CommandFields = arg.Content.Split(COMMAND_SPLITCHAR);
                        if (CommandFields[0] != null) { BossType = BossIdentify(CommandFields[0]); };
                        if (CommandFields[1] != null) { CommandArg = CommandArgIdentify(CommandFields[1]); }
                        if (CommandFields[2] != null) { CommandArg2 = CommandArg2Identify(CommandFields[2]); }
                        WriteLog(arg.Author.Username + "が入力 : " + arg.Content);
                        WriteLog("Command Fields Length : " + CommandFields.Length + "\nCommandFields1 : " + CommandFields[0] + "\nCommandFields2 : " + CommandFields[1] + "\nCommandFields3 : " + CommandFields[2]);
                        //
                        //一時的なヘルプコマンド(temp help command)
                        //
                        //if (arg.Content == "help")
                        //{
                        //    var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                        //    await dmchannel.SendMessageAsync("105 " + MessageDefine.Help_JP + ResourceDefine.HelpURI, false);
                        //}

                        //////////////////////////////////////////////////////////////////////////////////
                        switch (BossType) //ボス種類はなあに？
                        {
                            case 1: //クザカだ！
                                switch (CommandArg) //クザカに対するコマンドは？
                                {
                                    case 105: //ボス体力通知だ！
                                        switch (CommandArg2) //ボスの体力値は？
                                        {
                                            default:
                                                if (isKzarkaAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {
                                                    eb.WithTitle(BossNameJP.クザカ.ToString());
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.KzarkaThumbURI);
                                                    eb.WithColor(Color.Red);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(1);
                                                    MemoryBotMessageIdToBuffer(1);
                                                    
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                    await dmchannel.SendMessageAsync("105 " + MessageDefine.InvalidCommand_JP, false);
                                                }
                                                break;
                                            case 103: //ボス討伐完了時（ボスHP0％）
                                                eb.WithTitle(BossNameJP.クザカ.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.KzarkaThumbURI);
                                                eb.WithColor(Color.Red);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(1);
                                                MemoryBotMessageIdToBuffer(1);
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
                                                if (isKarandaAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {
                                                    if (DEBUGMODE) { WriteLog("isKarandaAlreadySpawned : " + isKarandaAlreadySpawned); }
                                                    eb.WithTitle(BossNameJP.カランダ.ToString());
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.KarandaThumbURI);
                                                    eb.WithColor(Color.DarkBlue);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(2);
                                                    MemoryBotMessageIdToBuffer(2);
                                                    //await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    if (!isKarandaAlreadySpawned)
                                                    {
                                                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                        await dmchannel.SendMessageAsync(MessageDefine.BossNotFound_JP, false);
                                                    }
                                                }
                                                break;
                                            case 103:
                                                eb.WithTitle(BossNameJP.カランダ.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(2, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.KarandaThumbURI);
                                                eb.WithColor(Color.DarkBlue);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(2);
                                                MemoryBotMessageIdToBuffer(2);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case 3: //ヌーベル(Nouver)
                                switch (CommandArg) //クザカに対するコマンドは？
                                {
                                    case 105: //ボス体力通知だ！
                                        switch (CommandArg2) //ボスの体力値は？
                                        {
                                            default:
                                                if (isNouverAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {

                                                    eb.WithTitle(BossNameJP.ヌーベル.ToString());
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(3, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.NouverThumbURI);
                                                    eb.WithColor(Color.DarkOrange);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(3);
                                                    MemoryBotMessageIdToBuffer(3);
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    if (!isNouverAlreadySpawned)
                                                    {
                                                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                        await dmchannel.SendMessageAsync(MessageDefine.BossNotFound_JP, false);
                                                    }
                                                }
                                                break;
                                            case 103: //ボス討伐完了時（ボスHP0％）
                                                eb.WithTitle(BossNameJP.ヌーベル.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(3, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.NouverThumbURI);
                                                eb.WithColor(Color.DarkOrange);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(3);
                                                MemoryBotMessageIdToBuffer(3);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case 4: //クツム（Kutum）
                                switch (CommandArg)
                                {
                                    case 105: //ボス体力通知だ！
                                        switch (CommandArg2) //ボスの体力値は？
                                        {
                                            default:
                                                if (isKutumAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {
                                                    if (DEBUGMODE) { WriteLog("isKutumAlreadySpawned : " + isKutumAlreadySpawned); }
                                                    eb.WithTitle(BossNameJP.クツム.ToString());
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(4, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.KutumThumbURI);
                                                    eb.WithColor(Color.DarkPurple);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(4);
                                                    MemoryBotMessageIdToBuffer(4);
                                                    //await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    if (!isKutumAlreadySpawned)
                                                    {
                                                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                        await dmchannel.SendMessageAsync(MessageDefine.BossNotFound_JP, false);
                                                    }
                                                }
                                                break;
                                            case 103:
                                                eb.WithTitle(BossNameJP.クツム.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(4, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.KutumThumbURI);
                                                eb.WithColor(Color.DarkPurple);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(4);
                                                MemoryBotMessageIdToBuffer(4);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case 5: //レッドノーズ
                                switch (CommandArg)
                                {
                                    case 105: //ボス体力通知だ！
                                        switch (CommandArg2) //ボスの体力値は？
                                        {
                                            default:
                                                if (isRednoseAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {
                                                    if (DEBUGMODE) { WriteLog("isRednoseAlreadySpawned : " + isRednoseAlreadySpawned); }
                                                    eb.WithTitle(BossNameJP.レッドノーズ.ToString());
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(5, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.RednoseThumbURI);
                                                    eb.WithColor(Color.DarkGrey);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(5);
                                                    MemoryBotMessageIdToBuffer(5);
                                                    //await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    if (!isRednoseAlreadySpawned)
                                                    {
                                                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                        await dmchannel.SendMessageAsync(MessageDefine.BossNotFound_JP, false);
                                                    }
                                                }
                                                break;
                                            case 103:
                                                eb.WithTitle(BossNameJP.レッドノーズ.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(5, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.RednoseThumbURI);
                                                eb.WithColor(Color.DarkGrey);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(5);
                                                MemoryBotMessageIdToBuffer(5);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                break;
                                        }
                                        break;

                                }
                                break;
                            case 6:  //ベグ(Bheg)
                                switch (CommandArg)
                                {
                                    case 105: //ボス体力通知だ！
                                        switch (CommandArg2) //ボスの体力値は？
                                        {
                                            default:
                                                if (isBhegAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {
                                                    if (DEBUGMODE) { WriteLog("isBhegAlreadySpawned : " + isBhegAlreadySpawned); }
                                                    eb.WithTitle(BossNameJP.ベグ.ToString());
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(6, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.BhegThumbURI);
                                                    eb.WithColor(Color.DarkTeal);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(6);
                                                    MemoryBotMessageIdToBuffer(6);
                                                    //await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    if (!isBhegAlreadySpawned)
                                                    {
                                                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                        await dmchannel.SendMessageAsync(MessageDefine.BossNotFound_JP, false);
                                                    }
                                                }
                                                break;
                                            case 103:
                                                eb.WithTitle(BossNameJP.ベグ.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(6, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.BhegThumbURI);
                                                eb.WithColor(Color.DarkTeal);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(6);
                                                MemoryBotMessageIdToBuffer(6);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case 7: //愚鈍（Dim Tree)
                                switch (CommandArg)
                                {
                                    case 105: //ボス体力通知だ！
                                        switch (CommandArg2) //ボスの体力値は？
                                        {
                                            default:
                                                if (isTreeAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {
                                                    if (DEBUGMODE) { WriteLog("isKutumAlreadySpawned : " + isTreeAlreadySpawned); }
                                                    eb.WithTitle(BossNameJP.愚鈍.ToString());
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(7, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.TreeThumbURI);
                                                    eb.WithColor(Color.DarkGreen);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(7);
                                                    MemoryBotMessageIdToBuffer(7);
                                                    //await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    if (!isTreeAlreadySpawned)
                                                    {
                                                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                        await dmchannel.SendMessageAsync(MessageDefine.BossNotFound_JP, false);
                                                    }
                                                }
                                                break;
                                            case 103:
                                                eb.WithTitle(BossNameJP.愚鈍.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(7, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.TreeThumbURI);
                                                eb.WithColor(Color.DarkGreen);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(7);
                                                MemoryBotMessageIdToBuffer(7);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case 8: //マッドマン（Mudman)
                                switch (CommandArg)
                                {
                                    case 105: //ボス体力通知だ！
                                        switch (CommandArg2) //ボスの体力値は？
                                        {
                                            default:
                                                if (isMudmanAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {
                                                    if (DEBUGMODE) { WriteLog("isMudmanAlreadySpawned : " + isMudmanAlreadySpawned); }
                                                    eb.WithTitle(BossNameJP.マッドマン.ToString());
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(8, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.MudThumbURI);
                                                    eb.WithColor(Color.LightGrey);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(8);
                                                    MemoryBotMessageIdToBuffer(8);
                                                    //await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    if (!isMudmanAlreadySpawned)
                                                    {
                                                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                        await dmchannel.SendMessageAsync(MessageDefine.BossNotFound_JP, false);
                                                    }
                                                }
                                                break;
                                            case 103:
                                                eb.WithTitle(BossNameJP.マッドマン.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(8, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.MudThumbURI);
                                                eb.WithColor(Color.LightGrey);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(8);
                                                MemoryBotMessageIdToBuffer(8);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case 20: //テストボス
                                switch (CommandArg)
                                {
                                    case 105: //ボス体力通知だ！
                                        switch (CommandArg2) //ボスの体力値は？
                                        {
                                            default:
                                                if (isTestAlreadySpawned && (CommandArg2 < 100 && CommandArg2 > 1)) //入力値が1~99までなら
                                                {
                                                    if (DEBUGMODE) { WriteLog("isMudmanAlreadySpawned : " + isTestAlreadySpawned); }
                                                    eb.WithTitle("ライブサーバーテスト用ボス");
                                                    eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(20, CurrentReportChannel, CommandArg2));
                                                    eb.WithThumbnailUrl(ResourceDefine.MudThumbURI);
                                                    eb.WithColor(Color.LightGrey);
                                                    eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                    await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                    DeletePreviousBotMessage(20);
                                                    MemoryBotMessageIdToBuffer(20);
                                                    //await arg.Channel.SendMessageAsync(BossStatus.ChangeStatusValue(2, CurrentReportChannel, CommandArg2));
                                                    //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, CommandArg2));
                                                }
                                                else
                                                {
                                                    if (!isTestAlreadySpawned)
                                                    {
                                                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                                                        await dmchannel.SendMessageAsync(MessageDefine.BossNotFound_JP, false);
                                                    }
                                                }
                                                break;
                                            case 103:
                                                eb.WithTitle(BossNameJP.マッドマン.ToString());
                                                eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.ChangeStatusValue(20, CurrentReportChannel, 0));
                                                eb.WithThumbnailUrl(ResourceDefine.MudThumbURI);
                                                eb.WithColor(Color.LightGrey);
                                                eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                                                await arg.Channel.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                                                DeletePreviousBotMessage(20);
                                                MemoryBotMessageIdToBuffer(20);
                                                //await status_ch_en.SendMessageAsync(MessageDefine.BossStatusMessage_EN + "\n" + BossStatus.ChangeStatusValue(1, CurrentReportChannel, 0));
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DEBUGMODE)
                        {
                            WriteLog(ex.ToString());
                            WriteLog("Debug Information : " + "BossType : " + BossType + " CommandArg : " + CommandArg + " CommandArg2 : " + CommandArg2);
                            WriteLog(CommandFields.ToString());
                        }
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
                if (!arg.Author.Username.Equals(BOT_NAME) && arg.Channel.Name == "help")
                {
                    if (arg.Content == "help")
                    {
                        var dmchannel = await arg.Author.GetOrCreateDMChannelAsync();
                        await dmchannel.SendMessageAsync(MessageDefine.Help_JP + ResourceDefine.HelpURI, false);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Expection on Client_MessageReceived() : " + ex.ToString());
            }

        }
        public async static void RefreshBatch(int BossID)
        {
            try
            {
                var eb = new EmbedBuilder();
                switch (BossID)
                {
                    case 1:
                        eb.WithTitle(BossNameJP.クザカ.ToString());
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[0]);
                        eb.WithThumbnailUrl(ResourceDefine.KzarkaThumbURI);
                        eb.WithColor(Color.Red);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(1);
                        MemoryBotMessageIdToBuffer(1);
                        break;
                    case 2:
                        eb.WithTitle(BossNameJP.カランダ.ToString());
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[1]);
                        eb.WithThumbnailUrl(ResourceDefine.KarandaThumbURI);
                        eb.WithColor(Color.DarkBlue);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(2);
                        MemoryBotMessageIdToBuffer(2);
                        break;
                    case 3:
                        eb.WithTitle(BossNameJP.ヌーベル.ToString());
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[2]);
                        eb.WithThumbnailUrl(ResourceDefine.NouverThumbURI);
                        eb.WithColor(Color.DarkOrange);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        if (DEBUGMODE)
                        {
                            WriteLog("Program.RefreshBatch() : Nouver's Refresh Embed Created.");
                        }
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(3);
                        MemoryBotMessageIdToBuffer(3);
                        break;
                    case 4:
                        eb.WithTitle(BossNameJP.クツム.ToString());
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[3]);
                        eb.WithThumbnailUrl(ResourceDefine.KutumThumbURI);
                        eb.WithColor(Color.DarkPurple);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(4);
                        MemoryBotMessageIdToBuffer(4);
                        break;
                    case 5:
                        eb.WithTitle(BossNameJP.レッドノーズ.ToString());
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[4]);
                        eb.WithThumbnailUrl(ResourceDefine.RednoseThumbURI);
                        eb.WithColor(Color.DarkGrey);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(5);
                        MemoryBotMessageIdToBuffer(5);
                        break;
                    case 6:
                        eb.WithTitle(BossNameJP.ベグ.ToString());
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[5]);
                        eb.WithThumbnailUrl(ResourceDefine.BhegThumbURI);
                        eb.WithColor(Color.DarkTeal);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(6);
                        MemoryBotMessageIdToBuffer(6);
                        break;
                    case 7:
                        eb.WithTitle(BossNameJP.愚鈍.ToString());
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[6]);
                        eb.WithThumbnailUrl(ResourceDefine.TreeThumbURI);
                        eb.WithColor(Color.DarkGreen);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(7);
                        MemoryBotMessageIdToBuffer(7);
                        break;
                    case 8:
                        eb.WithTitle(BossNameJP.マッドマン.ToString());
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[7]);
                        eb.WithThumbnailUrl(ResourceDefine.MudThumbURI);
                        eb.WithColor(Color.LightGrey);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(8);
                        MemoryBotMessageIdToBuffer(8);
                        break;
                    case 20:
                        eb.WithTitle("ライブサーバー用テスト");
                        eb.AddInlineField("時間計算：バグ修正の為一時的に無効化", BossStatus.LatestBossStatus[12]);
                        eb.WithThumbnailUrl(ResourceDefine.MudThumbURI);
                        eb.WithColor(Color.LightGrey);
                        eb.WithFooter(MessageDefine.BossStatusFooter_JP);
                        await status_ch.SendMessageAsync(MessageDefine.BossStatusMessage_JP, false, eb);
                        DeletePreviousBotMessage(20);
                        MemoryBotMessageIdToBuffer(20);
                        break;
                }
            }
            catch (Exception ex) { WriteLog("Expection on RefreshBatch() : \n" + ex.ToString()); }
        }
        //
        //MemoryBotMessageIdToBuffer
        //
        private static async void MemoryBotMessageIdToBuffer(int BossID)
        {
            switch (BossID)
            {
                case 1:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(1).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(0, BossMsg.Id);
                            WriteLog("Recorded the ID to Kzarka Buffer. (Recorded: " + BossMsg.Id + ")");
                            continue;
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Kzarka buffer.\nActual ID: " + BossMsg.Id);
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer0: " + LastBotMessageBuffer[0]); }
                    break;
                case 2:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(1).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(1, BossMsg.Id);
                            WriteLog("Recorded the ID to Karanda Buffer. (Recorded: " + BossMsg.Id + ")");
                            continue;
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Karanda buffer.\nActual ID: " + BossMsg.Id);
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer1: " + LastBotMessageBuffer[1]); }
                    break;
                case 3:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(1).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(2, BossMsg.Id);
                            continue;
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Nouver buffer.(Actual ID: " + BossMsg.Id + ")");
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer2: " + LastBotMessageBuffer[2]); }
                    break;
                case 4:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(1).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(3, BossMsg.Id);
                            continue;
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Kutum buffer.");
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer3: " + LastBotMessageBuffer[3]); }
                    break;
                case 5:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(10).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(4, BossMsg.Id);
                            continue;
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Rednose buffer.");
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer4: " + LastBotMessageBuffer[4]); }
                    break;
                case 6:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(1).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(5, BossMsg.Id);
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Bheg buffer.");
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer5: " + LastBotMessageBuffer[5]); }
                    break;
                case 7:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(1).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(6, BossMsg.Id);
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Tree buffer.");
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer6: " + LastBotMessageBuffer[6]); }
                    break;
                case 8:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(1).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(7, BossMsg.Id);
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Mud buffer.");
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer7: " + LastBotMessageBuffer[7]); }
                    break;
                case 20:
                    foreach (var BossMsg in await status_ch.GetMessagesAsync(1).Flatten())
                    {
                        if (BossMsg.Author.IsBot)
                        {
                            LastBotMessageBuffer.Insert(19, BossMsg.Id);
                        }
                        else
                        {
                            WriteLog("Failed to find messages via message id on Test buffer.");
                        }
                    }
                    if (DEBUGMODE) { WriteLog("LastbotMessageBuffer19: " + LastBotMessageBuffer[19]); }
                    break;
            }
        }
        //
        //DeletePreviousBotMessage
        //
        public async static void DeletePreviousBotMessage(int BossID)
        {
            IMessage target;
            try
            {
                switch (BossID)
                {
                    case 1:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[0]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[0]); }
                        await target.DeleteAsync();
                        break;
                    case 2:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[1]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[1]); }
                        await target.DeleteAsync();
                        break;
                    case 3:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[2]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[2]); }
                        await target.DeleteAsync();
                        break;
                    case 4:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[3]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[3]); }
                        await target.DeleteAsync();
                        break;
                    case 5:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[4]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[4]); }
                        await target.DeleteAsync();
                        break;
                    case 6:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[5]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[5]); }
                        await target.DeleteAsync();
                        break;
                    case 7:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[6]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[6]); }
                        await target.DeleteAsync();
                        break;
                    case 8:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[7]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[7]); }
                        await target.DeleteAsync();
                        break;
                    case 20:
                        target = await status_ch.GetMessageAsync(LastBotMessageBuffer[19]);
                        if (DEBUGMODE) { WriteLog("Target ID : " + LastBotMessageBuffer[19]); }
                        await target.DeleteAsync();
                        break;

                }
            }catch(Exception ex)
            {
                WriteLog("Exception on DeletePreviousBotMessage : \n" + ex.ToString());
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
            //トリガーワードの定義（Definition of Trigger words for spawn/status call.)
            string[] TestIdentify = new string[] { "test" }; //ライブサーバテスト用
            string[] KzarkaIdentify = new string[] { "クザカ", "kzarka", "kz" };
            string[] KarandaIdentify = new string[] { "カランダ", "karanda", "Karanda", "ka", "kar" };
            string[] NouverIdentify = new string[] { "ヌーベル", "Nouver", "nouver", "nv" };
            string[] KutumIdentify = new string[] { "クツム", "Kutum", "kutum", "ku", "kut" };
            string[] RednoseIdentify = new string[] { "レッドノーズ", "赤鼻", "レドノ", "rn", "rednose" };
            string[] BhegIdentify = new string[] { "ベグ", "ベ", "bheg", "bh" };
            string[] TreeIdentify = new string[] { "愚鈍", "ぐどん", "tree", "tr" };
            string[] MudIdentify = new string[] { "マッド", "泥", "mud" };
            //
            ////////////////////////////////////////////////////////////////////////////////
            //
            for (int i = 0; i < KzarkaIdentify.Length; i++)
            {
                try
                {
                    if (cmdfields1 == KzarkaIdentify[i]) { return 1; }
                }
                catch (FormatException)
                {
                    continue;
                }
                

            }
            for (int i = 0; i < KarandaIdentify.Length; i++)
            {
                if(cmdfields1 == KarandaIdentify[i]) { return 2; }
            }
            for (int i = 0; i < NouverIdentify.Length; i++)
            {
                if(cmdfields1 == NouverIdentify[i]) { return 3; }
            }
            for (int i = 0; i < KutumIdentify.Length; i++)
            {
                if(cmdfields1 == KutumIdentify[i]) { return 4; }
            }
            for (int i = 0; i < RednoseIdentify.Length; i++)
            {
                if (cmdfields1 == RednoseIdentify[i]) { return 5; }
            }
            for (int i = 0; i < BhegIdentify.Length; i++)
            {
                if(cmdfields1 == BhegIdentify[i]) { return 6; }
            }
            for (int i = 0; i < TreeIdentify.Length; i++)
            {
                if(cmdfields1 == TreeIdentify[i]) { return 7; }
            }
            for (int i = 0; i < MudIdentify.Length; i++)
            {
                if(cmdfields1 == MudIdentify[i]) { return 8; }
            }
            for (int i = 0; i < TestIdentify.Length; i++)
            {
                if (cmdfields1 == TestIdentify[i]) { return 20; }
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
                try
                {
                    if (cmdfields3 == SpawnIdentify[i]) { return 101; }
                }
                catch (FormatException) { continue; }
                
            }
            
            for (int i = 0; i < UndoIdentify.Length; i++)
            {
                try { if (cmdfields3 == UndoIdentify[i]) { return 102; } } catch (FormatException) { continue; }
            }
            for (int i = 0; i < KilledIdentify.Length; i++)
            {
                try { if (cmdfields3 == KilledIdentify[i]) { return 103; } } catch (FormatException) { continue; }
                
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
                try { if (int.Parse(cmdfields3) == PercentIdentify[i]) { return PercentIdentify[i]; } } catch (FormatException) { break; }
            }
            return 105;
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
