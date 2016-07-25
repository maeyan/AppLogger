using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Windows.Forms;

namespace AppLogger
{
    /// <summary>
    /// アプリケーションのログを管理するクラス
    /// </summary>
    public class AppLogManage
    {
        public AppLogManage()
        {
            this.CacheAppTitleLog = new AppTitleLogType();

            this.sw = new Stopwatch();
        }

        #region Getter/Setter
        /// <summary>
        /// 前回取得した時のタイトル情報（構造体)
        /// </summary>
        private AppTitleLogType CacheAppTitleLog { get; set; }

        /// <summary>
        /// 前回の累積時間
        /// </summary>
        private TimeSpan CacheTimeSpan { get; set; }

        /// <summary>
        /// ストップウォッチ
        /// </summary>
        private Stopwatch sw { get; set; }

        /// <summary>
        /// タイマー
        /// </summary>
        private System.Timers.Timer Timer { get; set; }
        #endregion

        /// <summary>
        /// タイマー開始時のメソッド
        /// </summary>
        public void StartTimer()
        {
            // WindowTitle取得タイマー開始
            this.Timer = new System.Timers.Timer();
            this.Timer.Elapsed += new ElapsedEventHandler(this.GetAppTitle);
            this.Timer.Interval = 1000;
            this.Timer.Start();

            this.sw.Start();
        }

        /// <summary>
        /// Timer終了時のメソッド
        /// </summary>
        public void EndTimer()
        {
            this.GetAppTitle(null, null);
            this.Timer.Stop();
        }

        /// <summary>
        /// windowのタイトルを取得する処理
        /// </summary>
        /// <param name="sender">Event時Object</param>
        /// <param name="e">Event時Argument</param>
        private void GetAppTitle(object sender, ElapsedEventArgs e)
        {
            Process p = WindowsAPI.GetActiveProcess();
            string applicationName = p.ProcessName;  // アプリ名を返す
            string processTitle = p.MainWindowTitle; // 検索画面であってもウィンドウタイトルを返す
            string windowName = WindowsAPI.GetActiveWindowTitle(); // 本当のアクティブなウィンドウタイトルを返す
            // ディレクトリの場合、GetActiveWinwowTitleは値あり、MainWindowTitleは値なし(ProcessNameがexplorerの時使い分けるとよい)

            // 空だったら初めて代入
            if (this.CacheAppTitleLog.AppName == string.Empty)
            {
                TimeSpan ts = this.sw.Elapsed;
                this.CacheTimeSpan = this.sw.Elapsed;

                AppTitleLogType data = new AppTitleLogType();
                data.AppName = applicationName;
                data.AppTitle = applicationName == "explorer" ? windowName : processTitle;
                data.ActiveStartTime = DateTime.Now.ToString("HH:mm:ss");
                data.ActivateTime = 0;
                this.CacheAppTitleLog = data;
            }
            else if(this.CacheAppTitleLog.AppName != applicationName)
            {
                if (applicationName == string.Empty && windowName == string.Empty)
                {
                    return;
                }

                TimeSpan ts = this.sw.Elapsed;
                AppTitleLogType data = this.CacheAppTitleLog;
                data.ActivateTime = (int)(ts.TotalSeconds - this.CacheTimeSpan.TotalSeconds);
                this.CacheTimeSpan = ts;

                // 出力
                this.OutputAppLog(data);

                data = new AppTitleLogType();
                data.AppName = applicationName;
                data.AppTitle = applicationName == "explorer" ? windowName : processTitle;
                data.ActiveStartTime = DateTime.Now.ToString("HH:mm:ss");
                data.ActivateTime = 0;
                this.CacheAppTitleLog = data;
            }
        }

        public void OutputAppLog(AppTitleLogType data)
        {
            if (data.ActiveStartTime == null)
            {
                return;
            }

            string fileName =DateTime.Now.ToString("yyyyMM") + ".txt";
            string directoryPath = Application.StartupPath + @"\Log";
            string filePath = directoryPath + @"\" + fileName;

            // フォルダがなければ作る
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default);
            string log = string.Format("{0}\t{1}\t{2}\t{3}", data.ActiveStartTime, data.ActivateTime, data.AppName, data.AppTitle);
            sw.WriteLine(log);
            sw.Close();
        }

        #region 構造体
        /// <summary>
        /// アプリケーションのログを扱う型
        /// </summary>
        public struct AppTitleLogType
        {
            /// <summary>
            /// ActiveWindow状態が始まった時間
            /// </summary>
            public string ActiveStartTime;

            /// <summary>
            /// アプリケーション（ウィンドウ）タイトル
            /// </summary>
            public string AppTitle;

            /// <summary>
            /// アプリケーション名
            /// </summary>
            public string AppName;
            
            /// <summary>
            /// 連続するアクティブな時間
            /// </summary>
            public int ActivateTime;
        }
        #endregion
    }
}
