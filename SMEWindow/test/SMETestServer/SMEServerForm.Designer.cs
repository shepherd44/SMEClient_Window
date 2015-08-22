namespace SMETestServer
{
    partial class SMEServerForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            richTB_Main = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(471, 425);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // richTB_Main
            // 
            richTB_Main.ImeMode = System.Windows.Forms.ImeMode.Off;
            richTB_Main.Location = new System.Drawing.Point(13, 13);
            richTB_Main.Name = "richTB_Main";
            richTB_Main.Size = new System.Drawing.Size(533, 406);
            richTB_Main.TabIndex = 1;
            richTB_Main.Text = "";
            // 
            // SMEServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 460);
            this.Controls.Add(richTB_Main);
            this.Controls.Add(this.button1);
            this.Name = "SMEServerForm";
            this.Text = "SMETestServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SMEServerForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        public static System.Windows.Forms.RichTextBox richTB_Main;
    }
}

