namespace t1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.importExcel = new System.Windows.Forms.Button();
            this.txtExcelPath = new System.Windows.Forms.TextBox();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // importExcel
            // 
            this.importExcel.Location = new System.Drawing.Point(12, 12);
            this.importExcel.Name = "importExcel";
            this.importExcel.Size = new System.Drawing.Size(86, 21);
            this.importExcel.TabIndex = 47;
            this.importExcel.Text = "ImportExcel";
            this.importExcel.UseVisualStyleBackColor = true;
            this.importExcel.Click += new System.EventHandler(this.importExcel_Click);
            // 
            // txtExcelPath
            // 
            this.txtExcelPath.Location = new System.Drawing.Point(104, 12);
            this.txtExcelPath.Name = "txtExcelPath";
            this.txtExcelPath.Size = new System.Drawing.Size(483, 21);
            this.txtExcelPath.TabIndex = 46;
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(12, 68);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(575, 369);
            this.txtMsg.TabIndex = 49;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(13, 40);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(85, 23);
            this.btnRun.TabIndex = 50;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 449);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.importExcel);
            this.Controls.Add(this.txtExcelPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button importExcel;
        private System.Windows.Forms.TextBox txtExcelPath;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btnRun;
    }
}

