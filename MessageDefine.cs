using System;
using System.Collections.Generic;
using System.Text;

namespace BlackSpiritTelepathy
{
    class MessageDefine
    {
        public static string KzarkaSpawnMessage_JP = "@everyone \nおい、セレンディア神殿のほうからクザカの咆哮が聞こえたぞ。\n早く仲間を呼んで倒しに行こう！";
        public static string BossStatusMessage_JP = "全体のボスの状況は他の冒険者を通じてオレ様が常に教えてやる。だからオマエはボスの討伐に専念するんだ。\nただ、出来ればオマエが今戦っているボスの状態も教えてくれ。他の冒険者も助かるからな！";
        public static string InvalidCommand_JP = "おい！ボスの状態の報告の仕方が間違ってるぞ！\nもし報告の仕方を知らないなら、helpと入力すればオレ様が教えてやる！";
        public static string BossStatusMessage_EN = "I'll tell you the overall boss status via other adventurers. So you can focus to beat'em freely!\nbut tell me the boss status that you beating if possible. It'll be helpful for other adventurers!";
    }
    class SystemMessageDefine
    {
        public static string CallerAddedInList_JP = "ユーザーがボス通知を行いました。以下のユーザーを通知済みリストへ追加。ユーザー名 : ";
        public static string BossSpawnConfirmed_JP = "複数のユーザーまたは特権者による通知によりボス出現を確認しました。ボス状況リストを活性化します。\nボス種類 : ";
        public static string DiscordAPIInit_JP = "Discord APIクライアントを生成しました。";
        public static string DiscordBOTLoggedIn_JP = "Discord BOTがログインしました。";
        public static string DiscordClientStarted_JP = "Discord APIとの同期を開始しました。";
        public static string DiscordBOTIsListening_JP = "ユーザーからのコマンドを待機しています...";
        public static string CommandAccepting_JP = "コマンド入力を受け付けています...コマンドを入力する場合は一度Enterキーを入力してください。";
        public static string CommanderModeGuide_JP = "コマンド入力してください。\nコマンド一覧は同梱のReadmeを参照ください。\n";
        public static string LogShowMode_JP = "BOTログ表示モードへ移行しました。";
        public static string Shutdown_JP = "BOTサーバーを停止しますか？\nyで停止します。それ以外の入力でコマンド受付モードへ戻ります。";
        public static string ChangeRequiredSpawnCount_JP = "ボス湧き確認に必要な報告数を変更します。\n5または10で該当の報告数へ変更、それ以外の入力でデフォルト値へ変更されます。";
        public static string ChangedRequiredSpawnCount_JP = "ボス湧き確認に必要な報告数を変更しました。Enterキーを押してコマンドメニューへ戻れます。";
        public static string KzarkaAlreadySpawned_JP = "ボス「クザカ」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
        public static string KarandaAlreadySpawned_JP = "ボス「カランダ」の沸き報告を受け取りましたが既にボス状況テーブルを展開しています。";
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
    }
}
