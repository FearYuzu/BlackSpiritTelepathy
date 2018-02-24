using System;
using System.Collections.Generic;
using System.Text;

namespace BlackSpiritTelepathy
{
    class StringDefine
    {
        //ログ接頭辞（Log Prefix)
        public static string Info = "[INFO] ";
        public static string Fatal = "[FATAL] ";
        public static string Warning = "[WARNING] ";

        //汎用テキスト（General Text）
        public static string InitialSpawnTime = "出現時間";
        public static string BossKilled = "討伐完了ch";
        public static string BossUnreported = "ボス未報告ch";
        public static string TimerTemporaryDisabled = "時間計算：バグ修正の為一時的に無効化";
        
    }
    class BossName
    {
        //ボス名（Boss Name）
        public static string Kzarka = "腐敗の君主クザカ";
        public static string Karanda = "カランダ";
        public static string Nouver = "ヌーベル";
        public static string Kutum = "古代のクツム";
        public static string Rednose = "レッドノーズ";
        public static string Bheg = "臆病なベグ";
        public static string Tree = "愚鈍な木の精霊";
        public static string Mud = "巨大マッドマン";
        public static string Offin = "オピン";
        public static string EvBoss1 = "イベントボス1";
        public static string EvBoss2 = "イベントボス2";
        public static string Test = "ライブサーバー用テスト";
    }
    class Error
    {
        public static string FailedToGetSocketChannel = "[WARNING] Failed to Initialize the Socket Channel. attempting again...";
    }
    class Info
    {
        public static string InitDevSocketChannel = "[INFO] Initialized a Socket Channel for developer guild.";
        public static string InitLiveSocketChannel = "[INFO] Initialized a Socket Channel for live guild.";
        public static string BossTypeOnEmbedMessage = "[INFO] Boss Name on Embed Message : ";
    }
    class MessageDefine
    {
        //闇の精霊メッセージ（Black Spirit Messages)
        public static string KzarkaSpawnMessage_JP = "@everyone \nおい、セレンディア神殿のほうからクザカの咆哮が聞こえたぞ。\n早く仲間を呼んで倒しに行こう！";
        public static string KarandaSpawnMessage_JP = "@everyone \nカランダの尾根でハーピーの女王カランダが羽ばたいているようだ\n早く仲間を呼んで倒しに行こう！";
        public static string NouverSpawnMessage_JP = "@everyone \nバレンシア大砂漠でヌーベルの痕跡が見つかったようだ。仲間を呼んで立ち向かう準備をしよう！";
        public static string KutumSpawnMessage_JP = "@everyone \n赤い砂の石室から古代のクツムの心臓の音が聞こえる。早く仲間を呼んで倒しに行こう！";
        public static string RednoseSpawnMessage_JP = "@everyone \n西部警部キャンプの兵士たちがレッドノーズ相手に苦戦しているようだぞ。\n早く仲間を呼んで助けに行こう！";
        public static string BhegSpawnMessage_JP = "@everyone \nセレンディア北部平原で臆病なベグが祭壇インプたちを束ねているのが見えた。\n早く仲間を呼んで倒しに行こう！";
        public static string TreeSpawnMessage_JP = "@everyone \n木の精霊たちが隠遁の森で愚鈍な木の精霊を起こしたようだ。\n早く仲間を呼んで倒しに行こう！";
        public static string MudSpawnMessage_JP = "@everyone \nフォガンたちが巨大マッドマンによって苦しめているようだ。\n早く仲間を呼んで助けに行こう！";
        public static string TestSpawnMessage_JP = "デバッグテスト用ボス状況テーブル起動";
        public static string BossStatusMessage_JP = "全体のボスの状況は他の冒険者を通じてオレ様が常に教えてやる。だからオマエはボスの討伐に専念するんだ。\nただ、出来ればオマエが今戦っているボスの状態も教えてくれ。他の冒険者も助かるからな！";
        public static string InvalidCommand_JP = "おい！ボスの状態の報告の仕方が間違ってるぞ！\nもし報告の仕方を知らないなら、helpと入力すればオレ様が教えてやる！";
        public static string BossNotFound_JP = "おい！報告したボスはまだ居ないぞ！";
        public static string BossStatusFooter_JP = "もしボス状況の報告の仕方が分からないなら、「help」と入力すれば教えてやるぞ！";
        public static string Help_JP = "何か分からないことがあるか？なんでも教えてやるぞ！\n例えば、ボス状況の報告の仕方とかが分からないなら以下を見るといい。（内容改善予定）\n";
        public static string BossDefeated_JP = "よくやった！ボスは討伐されたようだ。またボスが現れたら教えてやるからな！";
        public static string BossStatusMessage_EN = "I'll tell you the overall boss status via other adventurers. So you can focus to beat'em freely!\nbut tell me the boss status that you beating if possible. It'll be helpful for other adventurers!";
    }
    class SystemMessageDefine
    {
        //コンソール側メッセージ（Console Message）
        public static string WelcomeMessage = "-------------------------------------------------------------\nBlack Spirit Telepathy BOT Server\n-------------------------------------------------------------";
        public static string CallerAddedInList_JP = "[INFO] User Attempted a Boss Spawn Notify. Add following user to Caller's List. Username : ";
        public static string BossSpawnConfirmed_JP = "[INFO] Confirmed a Boss Spawn by several user or special role user's notify. Activate the Boss Status List. \nBoss Type : ";
        public static string DiscordAPIInit_JP = "[INFO] Created a Discord API Client.";
        public static string DiscordBOTLoggedIn_JP = "[INFO] Discord BOT logged in to Community Guild.";
        public static string DiscordClientStarted_JP = "[INFO] Started Sync with Discord API.";
        public static string DiscordBOTIsListening_JP = "[INFO] Waiting for commands from users...";
        public static string BossChannelMapInit_JP = "[INFO] Completed to initialized the Boss status mapping.";
        public static string CommandAccepting_JP = "Command is now accepting... Enter to input commands.";
        public static string CommanderModeGuide_JP = "Input commands you want.\nCheck Readme for command lists.\n";
        public static string LogShowMode_JP = "Entered to BOT Log Mode.";
        public static string Shutdown_JP = "BOTサーバーを停止しますか？\nyで停止します。それ以外の入力でコマンド受付モードへ戻ります。";
        public static string ChangeRequiredSpawnCount_JP = "ボス湧き確認に必要な報告数を変更します。\n5または10で該当の報告数へ変更、それ以外の入力でデフォルト値へ変更されます。";
        public static string ChangedRequiredSpawnCount_JP = "ボス湧き確認に必要な報告数を変更しました。Enterキーを押してコマンドメニューへ戻れます。";
        public static string KzarkaAlreadySpawned_JP = "Got a spawn report about the boss <Kzarka>, but BOT Server already activated a Boss Status Table.";
        public static string KarandaAlreadySpawned_JP = "Got a spawn report about the boss <Karanda>, but BOT Server already activated a Boss Status Table.";
        public static string NouverAlreadySpawned_JP = "ボス「ヌーベル」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string KutumAlreadySpawned_JP = "ボス「クツム」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string RednoseAlreadySpawned_JP = "ボス「レッドノーズ」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string BhegAlreadySpawned_JP = "ボス「ベグ」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string DimtreeAlreadySpawned_JP = "ボス「愚鈍の木の精霊」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string MudmanAlreadySpawned_JP = "ボス「巨大マッドマン」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string TargargoAlreadySpawned_JP = "ボス「タルガルゴ」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string IzabellaAlreadySpawned_JP = "ボス「イザベラ」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string SendMessage_JP = "メッセージを発信します。発信したいメッセージを入力してください。";
        public static string Kz_ElapsedTime_JP = "「クザカ」沸きから経過した時間： ";
        public static string Kz_StatusRefresh_JP = "ワールドボス「クザカ」の状況を自動更新しました。";
        public static string Ka_StatusRefresh_JP = "ワールドボス「カランダ」の状況を自動更新しました。";
        public static string Nv_StatusRefresh_JP = "ワールドボス「ヌーベル」の状況を自動更新しました。";
        public static string Ku_StatusRefresh_JP = "ワールドボス「クツム」の状況を自動更新しました。";
        public static string Rn_StatusRefresh_JP = "ワールドボス「レッドノーズ」の状況を自動更新しました。";
        public static string Bh_StatusRefresh_JP = "ワールドボス「臆病なベグ」の状況を自動更新しました。";
        public static string Tr_StatusRefresh_JP = "ワールドボス「愚鈍な木の精霊」の状況を自動更新しました。";
        public static string Iz_StatusRefresh_JP = "ワールドボス「魔女イザベラ」の状況を自動更新しました。";
        public static string Test_StatusRefresh_JP = "テスト用ボスの状況を自動更新しました。";
        public static string KzarkaKilled = "[INFO] The World Boss <Kzarka> was now killed. Issued a Result Messages. <Kzarka> status deactivated.";
        //
        //Discord API Related
        //
        public static string FailedToAPILogin = "[FATAL] Could not login to Discord API. Try launch again later if your environment hasn't any issues.";
        public static string LaunchedAsLiveService = "[INFO] Logged into the Discord Guild for Live Service.";
        public static string LaunchedAsDevelopment = "[INFO] Logged into the Discord Guild for Development.";
    }
    class DebugMsg
    {
        public static string GBRLogging_Balenos = "[GBRLog] Balenos Value : ";
        public static string GBRLogging_Serendia = "[GBRLog] Serendia Value : ";
        public static string GBRLogging_Calpheon = "[GBRLog] Calpheon Value : ";
        public static string GBRLogging_Mediah = "[GBRLog] Mediah Value : ";
        public static string GBRLogging_Valencia = "[GBRLog] Valencia Value : ";
        public static string GBRLogging_Magoria = "[GBRLog] Magoria Value : ";
        public static string GBRLogging_Kamasylvia = "[GBRLog] Kamasylvia Value : ";
    }
    class ConsoleMessage
    {
        public static string ItemClearProcessing = "Deleting a Message.. ID : ";
        public static string ItemClearFinished = "Done. Enter to back menu.";
    }
}
