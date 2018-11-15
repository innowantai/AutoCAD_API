namespace ConcreteAndBarCount
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEnter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLayers = new System.Windows.Forms.Label();
            this.txtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdb1 = new System.Windows.Forms.RadioButton();
            this.rdb2 = new System.Windows.Forms.RadioButton();
            this.cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chLine = new System.Windows.Forms.CheckBox();
            this.chPolyline = new System.Windows.Forms.CheckBox();
            this.chArc = new System.Windows.Forms.CheckBox();
            this.chCircle = new System.Windows.Forms.CheckBox();
            this.chText = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSave = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnEnter
            // 
            this.btnEnter.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnEnter.Location = new System.Drawing.Point(286, 309);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(102, 46);
            this.btnEnter.TabIndex = 0;
            this.btnEnter.Text = "確認";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(129, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 1;
            // 
            // lblLayers
            // 
            this.lblLayers.AutoSize = true;
            this.lblLayers.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLayers.Location = new System.Drawing.Point(80, 60);
            this.lblLayers.Name = "lblLayers";
            this.lblLayers.Size = new System.Drawing.Size(95, 24);
            this.lblLayers.TabIndex = 2;
            this.lblLayers.Text = "相關圖層 :";
            this.lblLayers.Click += new System.EventHandler(this.lblLayers_Click);
            // 
            // txtBox
            // 
            this.txtBox.Location = new System.Drawing.Point(190, 63);
            this.txtBox.Margin = new System.Windows.Forms.Padding(5);
            this.txtBox.Name = "txtBox";
            this.txtBox.Size = new System.Drawing.Size(125, 22);
            this.txtBox.TabIndex = 3;
            this.txtBox.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(81, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "選擇圖層 :";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // rdb1
            // 
            this.rdb1.AutoSize = true;
            this.rdb1.Location = new System.Drawing.Point(62, 66);
            this.rdb1.Name = "rdb1";
            this.rdb1.Size = new System.Drawing.Size(14, 13);
            this.rdb1.TabIndex = 6;
            this.rdb1.TabStop = true;
            this.rdb1.UseVisualStyleBackColor = true;
            // 
            // rdb2
            // 
            this.rdb2.AutoSize = true;
            this.rdb2.Location = new System.Drawing.Point(62, 100);
            this.rdb2.Name = "rdb2";
            this.rdb2.Size = new System.Drawing.Size(14, 13);
            this.rdb2.TabIndex = 7;
            this.rdb2.TabStop = true;
            this.rdb2.UseVisualStyleBackColor = true;
            // 
            // cmb
            // 
            this.cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmb.FormattingEnabled = true;
            this.cmb.Location = new System.Drawing.Point(85, 122);
            this.cmb.Name = "cmb";
            this.cmb.Size = new System.Drawing.Size(287, 28);
            this.cmb.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(81, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "勾選處理項目 :";
            // 
            // chLine
            // 
            this.chLine.AutoSize = true;
            this.chLine.Font = new System.Drawing.Font("新細明體", 12F);
            this.chLine.Location = new System.Drawing.Point(86, 199);
            this.chLine.Name = "chLine";
            this.chLine.Size = new System.Drawing.Size(43, 20);
            this.chLine.TabIndex = 11;
            this.chLine.Text = "線";
            this.chLine.UseVisualStyleBackColor = true;
            // 
            // chPolyline
            // 
            this.chPolyline.AutoSize = true;
            this.chPolyline.Font = new System.Drawing.Font("新細明體", 12F);
            this.chPolyline.Location = new System.Drawing.Point(86, 225);
            this.chPolyline.Name = "chPolyline";
            this.chPolyline.Size = new System.Drawing.Size(75, 20);
            this.chPolyline.TabIndex = 12;
            this.chPolyline.Text = "聚合線";
            this.chPolyline.UseVisualStyleBackColor = true;
            // 
            // chArc
            // 
            this.chArc.AutoSize = true;
            this.chArc.Font = new System.Drawing.Font("新細明體", 12F);
            this.chArc.Location = new System.Drawing.Point(86, 251);
            this.chArc.Name = "chArc";
            this.chArc.Size = new System.Drawing.Size(59, 20);
            this.chArc.TabIndex = 13;
            this.chArc.Text = "弧線";
            this.chArc.UseVisualStyleBackColor = true;
            // 
            // chCircle
            // 
            this.chCircle.AutoSize = true;
            this.chCircle.Font = new System.Drawing.Font("新細明體", 12F);
            this.chCircle.Location = new System.Drawing.Point(86, 277);
            this.chCircle.Name = "chCircle";
            this.chCircle.Size = new System.Drawing.Size(43, 20);
            this.chCircle.TabIndex = 14;
            this.chCircle.Text = "圓";
            this.chCircle.UseVisualStyleBackColor = true;
            // 
            // chText
            // 
            this.chText.AutoSize = true;
            this.chText.Font = new System.Drawing.Font("新細明體", 12F);
            this.chText.Location = new System.Drawing.Point(86, 303);
            this.chText.Name = "chText";
            this.chText.Size = new System.Drawing.Size(101, 20);
            this.chText.TabIndex = 15;
            this.chText.Text = "文字(多行)";
            this.chText.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(80, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 24);
            this.label4.TabIndex = 16;
            this.label4.Text = "儲存檔名 :";
            // 
            // txtSave
            // 
            this.txtSave.Location = new System.Drawing.Point(190, 27);
            this.txtSave.Margin = new System.Windows.Forms.Padding(5);
            this.txtSave.Name = "txtSave";
            this.txtSave.Size = new System.Drawing.Size(125, 22);
            this.txtSave.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 382);
            this.Controls.Add(this.txtSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chText);
            this.Controls.Add(this.chCircle);
            this.Controls.Add(this.chArc);
            this.Controls.Add(this.chPolyline);
            this.Controls.Add(this.chLine);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmb);
            this.Controls.Add(this.rdb2);
            this.Controls.Add(this.rdb1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBox);
            this.Controls.Add(this.lblLayers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEnter);
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Name = "Form1";
            this.Text = "選擇處理圖層";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLayers;
        private System.Windows.Forms.TextBox txtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdb1;
        private System.Windows.Forms.RadioButton rdb2;
        private System.Windows.Forms.ComboBox cmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chLine;
        private System.Windows.Forms.CheckBox chPolyline;
        private System.Windows.Forms.CheckBox chArc;
        private System.Windows.Forms.CheckBox chCircle;
        public System.Windows.Forms.CheckBox chText;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtSave;
    }
}