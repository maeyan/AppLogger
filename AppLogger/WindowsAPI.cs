using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AppLogger
{
    /// <summary>
    /// WindowsAPIを扱うクラス
    /// </summary>
    public class WindowsAPI
    {
        /// <summary>
        /// フォアグラウンドウィンドウのハンドルを返す
        /// </summary>
        /// <returns>ウィンドウハンドル</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 指定されたウィンドウのタイトルバーのテキストをバッファへコピーします
        /// </summary>
        /// <param name="hWnd">ウィンドウまたはコントロールのハンドル</param>
        /// <param name="text">テキストバッファ</param>
        /// <param name="length">コピーする最大文字数</param>
        /// <returns>関数が成功：コピーされた文字数(終端の NULL 文字は含まない
        /// それ以外は、0 が返る</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int length);

        /// <summary>
        /// 指定されたウィンドウを作成したスレッドの ID を取得します。必要であれば、ウィンドウを作成したプロセスの ID も取得できます。
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="lpdwProcessId">プロセス ID</param>
        /// <returns>プロセスID</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        /// <summary>
        /// アクティブウィンドウのProcessオブジェクトを取得
        /// </summary>
        /// <returns>プロセスオブジェクト</returns>
        public static Process GetActiveProcess()
        {
            // アクティブなウィンドウハンドルの取得
            IntPtr hWnd = GetForegroundWindow();

            // ウィンドウハンドルからプロセスIDを取得
            int id;
            GetWindowThreadProcessId(hWnd, out id);

            return Process.GetProcessById(id);
        }

        /// <summary>
        /// アクティブなウィンドウのタイトルを取得
        /// </summary>
        /// <returns>ウィンドウタイトル</returns>
        public static string GetActiveWindowTitle()
        {
            IntPtr hWnd = GetForegroundWindow();
            StringBuilder title = new StringBuilder(1048);

            // ウィンドウタイトルを取得
            GetWindowText(hWnd, title, 1024);
            return title.ToString();
        }
    }
}
