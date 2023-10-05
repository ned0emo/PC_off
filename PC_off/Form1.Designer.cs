namespace PC_off
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.sevenBuildingEditButton = new System.Windows.Forms.Button();
            this.firstBuildingEditButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.delayTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.firstBuildingTab = new System.Windows.Forms.TabPage();
            this.refreshFirstCompsButton = new System.Windows.Forms.Button();
            this.firstBuildingOffButton = new System.Windows.Forms.Button();
            this.sevenBuildingTab = new System.Windows.Forms.TabPage();
            this.refreshSevenCompsButton = new System.Windows.Forms.Button();
            this.sevenBuildingOffButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.firstBuildingTab.SuspendLayout();
            this.sevenBuildingTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // sevenBuildingEditButton
            // 
            this.sevenBuildingEditButton.Location = new System.Drawing.Point(471, 12);
            this.sevenBuildingEditButton.Name = "sevenBuildingEditButton";
            this.sevenBuildingEditButton.Size = new System.Drawing.Size(85, 23);
            this.sevenBuildingEditButton.TabIndex = 0;
            this.sevenBuildingEditButton.Text = "7 корпус";
            this.sevenBuildingEditButton.UseVisualStyleBackColor = true;
            this.sevenBuildingEditButton.Click += new System.EventHandler(this.sevenBuildingButton_Click);
            // 
            // firstBuildingEditButton
            // 
            this.firstBuildingEditButton.Location = new System.Drawing.Point(380, 12);
            this.firstBuildingEditButton.Name = "firstBuildingEditButton";
            this.firstBuildingEditButton.Size = new System.Drawing.Size(85, 23);
            this.firstBuildingEditButton.TabIndex = 1;
            this.firstBuildingEditButton.Text = "1 корпус";
            this.firstBuildingEditButton.UseVisualStyleBackColor = true;
            this.firstBuildingEditButton.Click += new System.EventHandler(this.firstBuildingButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(290, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Списки компов:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Задержка выключения (мин):";
            // 
            // delayTextBox
            // 
            this.delayTextBox.Location = new System.Drawing.Point(174, 12);
            this.delayTextBox.MaxLength = 3;
            this.delayTextBox.Name = "delayTextBox";
            this.delayTextBox.Size = new System.Drawing.Size(100, 20);
            this.delayTextBox.TabIndex = 4;
            this.delayTextBox.TextChanged += new System.EventHandler(this.delayTextBox_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.firstBuildingTab);
            this.tabControl1.Controls.Add(this.sevenBuildingTab);
            this.tabControl1.Location = new System.Drawing.Point(12, 38);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(548, 306);
            this.tabControl1.TabIndex = 5;
            // 
            // firstBuildingTab
            // 
            this.firstBuildingTab.AutoScroll = true;
            this.firstBuildingTab.Controls.Add(this.refreshFirstCompsButton);
            this.firstBuildingTab.Controls.Add(this.firstBuildingOffButton);
            this.firstBuildingTab.Location = new System.Drawing.Point(4, 22);
            this.firstBuildingTab.Name = "firstBuildingTab";
            this.firstBuildingTab.Padding = new System.Windows.Forms.Padding(3);
            this.firstBuildingTab.Size = new System.Drawing.Size(540, 280);
            this.firstBuildingTab.TabIndex = 0;
            this.firstBuildingTab.Text = "1 корпус";
            this.firstBuildingTab.UseVisualStyleBackColor = true;
            // 
            // refreshFirstCompsButton
            // 
            this.refreshFirstCompsButton.Location = new System.Drawing.Point(125, 6);
            this.refreshFirstCompsButton.Name = "refreshFirstCompsButton";
            this.refreshFirstCompsButton.Size = new System.Drawing.Size(75, 23);
            this.refreshFirstCompsButton.TabIndex = 1;
            this.refreshFirstCompsButton.Text = "Обновить";
            this.refreshFirstCompsButton.UseVisualStyleBackColor = true;
            // 
            // firstBuildingOffButton
            // 
            this.firstBuildingOffButton.Location = new System.Drawing.Point(6, 6);
            this.firstBuildingOffButton.Name = "firstBuildingOffButton";
            this.firstBuildingOffButton.Size = new System.Drawing.Size(113, 23);
            this.firstBuildingOffButton.TabIndex = 0;
            this.firstBuildingOffButton.Text = "Все выделенное";
            this.firstBuildingOffButton.UseVisualStyleBackColor = true;
            // 
            // sevenBuildingTab
            // 
            this.sevenBuildingTab.AutoScroll = true;
            this.sevenBuildingTab.Controls.Add(this.refreshSevenCompsButton);
            this.sevenBuildingTab.Controls.Add(this.sevenBuildingOffButton);
            this.sevenBuildingTab.Location = new System.Drawing.Point(4, 22);
            this.sevenBuildingTab.Name = "sevenBuildingTab";
            this.sevenBuildingTab.Padding = new System.Windows.Forms.Padding(3);
            this.sevenBuildingTab.Size = new System.Drawing.Size(540, 280);
            this.sevenBuildingTab.TabIndex = 1;
            this.sevenBuildingTab.Text = "7 корпус";
            this.sevenBuildingTab.UseVisualStyleBackColor = true;
            // 
            // refreshSevenCompsButton
            // 
            this.refreshSevenCompsButton.Location = new System.Drawing.Point(125, 6);
            this.refreshSevenCompsButton.Name = "refreshSevenCompsButton";
            this.refreshSevenCompsButton.Size = new System.Drawing.Size(75, 23);
            this.refreshSevenCompsButton.TabIndex = 2;
            this.refreshSevenCompsButton.Text = "Обновить";
            this.refreshSevenCompsButton.UseVisualStyleBackColor = true;
            // 
            // sevenBuildingOffButton
            // 
            this.sevenBuildingOffButton.Location = new System.Drawing.Point(6, 6);
            this.sevenBuildingOffButton.Name = "sevenBuildingOffButton";
            this.sevenBuildingOffButton.Size = new System.Drawing.Size(113, 23);
            this.sevenBuildingOffButton.TabIndex = 1;
            this.sevenBuildingOffButton.Text = "Все выделенное";
            this.sevenBuildingOffButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(290, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(268, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Для применения изменений требуется перезапуск";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 356);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.delayTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.firstBuildingEditButton);
            this.Controls.Add(this.sevenBuildingEditButton);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Отключение компов (1.0)";
            this.tabControl1.ResumeLayout(false);
            this.firstBuildingTab.ResumeLayout(false);
            this.sevenBuildingTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sevenBuildingEditButton;
        private System.Windows.Forms.Button firstBuildingEditButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox delayTextBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage sevenBuildingTab;
        private System.Windows.Forms.Button firstBuildingOffButton;
        private System.Windows.Forms.Button sevenBuildingOffButton;
        public System.Windows.Forms.TabPage firstBuildingTab;
        private System.Windows.Forms.Button refreshFirstCompsButton;
        private System.Windows.Forms.Button refreshSevenCompsButton;
        private System.Windows.Forms.Label label3;
    }
}

