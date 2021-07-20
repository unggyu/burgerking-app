namespace HamburgerEx
{
    partial class BurgerKing
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BurgerKing));
            this.flowLayoutPanelRight = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelRight
            // 
            this.flowLayoutPanelRight.AutoScroll = true;
            this.flowLayoutPanelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(21)))), ((int)(((byte)(15)))));
            this.flowLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelRight.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelRight.Name = "flowLayoutPanelRight";
            this.flowLayoutPanelRight.Size = new System.Drawing.Size(1584, 762);
            this.flowLayoutPanelRight.TabIndex = 1;
            this.flowLayoutPanelRight.WrapContents = false;
            this.flowLayoutPanelRight.MouseEnter += new System.EventHandler(this.FlowLayoutPanelRight_MouseEnter);
            // 
            // flowLayoutPanelLeft
            // 
            this.flowLayoutPanelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(32)))), ((int)(((byte)(17)))));
            this.flowLayoutPanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanelLeft.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelLeft.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelLeft.Name = "flowLayoutPanelLeft";
            this.flowLayoutPanelLeft.Size = new System.Drawing.Size(319, 762);
            this.flowLayoutPanelLeft.TabIndex = 2;
            // 
            // BurgerKing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 762);
            this.Controls.Add(this.flowLayoutPanelLeft);
            this.Controls.Add(this.flowLayoutPanelRight);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BurgerKing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BurgerKing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BurgerKing_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelRight;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLeft;
    }
}

