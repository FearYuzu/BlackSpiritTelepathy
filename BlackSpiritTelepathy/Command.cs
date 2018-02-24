using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace BlackSpiritTelepathy
{
    class ServerCommand
    {
        public static async Task StartConsole()
        {
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
                            Program.isShowLog = false;
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine(SystemMessageDefine.CommanderModeGuide_JP);
                            break;

                        case "showlog":
                            Console.Clear();
                            VersionHeader();
                            Program.isShowLog = true;
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
                                foreach (var Item in await Program.status_ch.GetMessagesAsync(1000).Flatten())
                                {
                                    if (Item.Author.IsBot)
                                    {
                                        Console.WriteLine(ConsoleMessage.ItemClearProcessing + Item.Id);
                                        await Item.DeleteAsync();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Program.WriteLog(ex.ToString());
                            }
                            Console.WriteLine(ConsoleMessage.ItemClearFinished);
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
                                        Program.RequiredBossSpawnCallCount = int.Parse(input2);
                                    }
                                    catch (Exception ex)
                                    {
                                        Program.WriteLog(ex.ToString());
                                    }
                                    Console.WriteLine(SystemMessageDefine.ChangedRequiredSpawnCount_JP);
                                    break;

                            }
                            break;
                        case "check requiredspawncount":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("ボス状況テーブル展開に必要な、現在設定されている報告必要数：" + Program.RequiredBossSpawnCallCount.ToString());
                            break;
                        case "disable kzarka":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("Kzarka Spawn Disabled");
                            Program.isKzarkaAlreadySpawned = false;
                            break;
                        case "disable karanda":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("karanda Spawn Disabled");
                            Program.isKarandaAlreadySpawned = false;
                            break;
                        case "disable nouver":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("Nouver Spawn Disabled");
                            Program.isNouverAlreadySpawned = false;
                            break;
                        case "disable kutum":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("Kutum Spawn Disabled");
                            Program.isKutumAlreadySpawned = false;
                            break;
                        case "disable rednose":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("Rednose Spawn Disabled");
                            Program.isRednoseAlreadySpawned = false;
                            break;
                        case "disable bheg":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("Bheg Spawn Disabled");
                            Program.isBhegAlreadySpawned = false;
                            break;
                        case "disable tree":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("Tree Spawn Disabled");
                            Program.isTreeAlreadySpawned = false;
                            break;
                        case "disable mud":
                            Console.Clear();
                            VersionHeader();
                            Console.WriteLine("Mud Spawn Disabled");
                            Program.isMudmanAlreadySpawned = false;
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
                                        await Program.general_ch.SendMessageAsync(input3);
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
                                        var msg2 = Program.status_ch.GetMessageAsync(msg);
                                        Console.WriteLine(msg2.Result);
                                    }
                                    catch (Exception ex)
                                    {
                                        Program.WriteLog(ex.ToString());
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
                Program.WriteLog(ex.ToString());
            }
        }
        static void VersionHeader() //バージョンヘッダ
        {
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Black Spirit Telepathy BOT Server {0}", Program.SERVER_VERSION);
            Console.WriteLine("-------------------------------------------------------");
        }
        static int ArgIdentify(string cmdfields2)
        {
            return 0;
        }
        static int Arg2Identify(string cmdfields3)
        {
            return 0;
        }
        private static void WriteLog(string LogDetails) //
        {
            Program.WriteLog(LogDetails);
        }
    }
}
