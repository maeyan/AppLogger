using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppLogger
{
    /// <summary>
    /// フォームクラス
    /// </summary>
    public partial class MainForm : Form
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Getter/Setter
        /// <summary>
        /// Windowタイトルを管理するクラス
        /// </summary>
        private AppLogManage TitleManager { get; set; }
        #endregion

        /// <summary>
        /// フォームロード時の処理
        /// </summary>
        /// <param name="sender">Event時Object</param>
        /// <param name="e">Event時Argument</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.TitleManager = new AppLogManage();
            this.TitleManager.StartTimer();
        }
    }
}
