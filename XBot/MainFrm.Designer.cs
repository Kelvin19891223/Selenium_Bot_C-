namespace DentalDoc
{
    partial class MainFrm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_start = new Bunifu.Framework.UI.BunifuFlatButton();
            this.starttime = new System.Windows.Forms.Label();
            this.lab_status1 = new System.Windows.Forms.Label();
            this.panel_tbl = new System.Windows.Forms.TableLayoutPanel();
            this.grid_main = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAB_SPORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAB_Event_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAB_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAB_Away = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAB_HOME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAB_LEAGUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bind_main = new System.Windows.Forms.BindingSource(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel2.SuspendLayout();
            this.panel_tbl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bind_main)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_start);
            this.panel2.Controls.Add(this.starttime);
            this.panel2.Controls.Add(this.lab_status1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 696);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(818, 40);
            this.panel2.TabIndex = 2;
            // 
            // btn_start
            // 
            this.btn_start.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btn_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btn_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_start.BorderRadius = 0;
            this.btn_start.ButtonText = "Start";
            this.btn_start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_start.DisabledColor = System.Drawing.Color.Gray;
            this.btn_start.Font = new System.Drawing.Font("Segoe UI Historic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_start.Iconcolor = System.Drawing.Color.Transparent;
            this.btn_start.Iconimage = null;
            this.btn_start.Iconimage_right = null;
            this.btn_start.Iconimage_right_Selected = null;
            this.btn_start.Iconimage_Selected = null;
            this.btn_start.IconMarginLeft = 0;
            this.btn_start.IconMarginRight = 0;
            this.btn_start.IconRightVisible = true;
            this.btn_start.IconRightZoom = 0D;
            this.btn_start.IconVisible = true;
            this.btn_start.IconZoom = 90D;
            this.btn_start.IsTab = false;
            this.btn_start.Location = new System.Drawing.Point(719, 4);
            this.btn_start.Name = "btn_start";
            this.btn_start.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btn_start.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.btn_start.OnHoverTextColor = System.Drawing.Color.White;
            this.btn_start.selected = false;
            this.btn_start.Size = new System.Drawing.Size(95, 34);
            this.btn_start.TabIndex = 5;
            this.btn_start.Text = "Start";
            this.btn_start.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_start.Textcolor = System.Drawing.Color.White;
            this.btn_start.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click_1);
            // 
            // starttime
            // 
            this.starttime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.starttime.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.starttime.Location = new System.Drawing.Point(91, 10);
            this.starttime.Name = "starttime";
            this.starttime.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.starttime.Size = new System.Drawing.Size(161, 30);
            this.starttime.TabIndex = 0;
            this.starttime.Text = "2019-05-13";
            this.starttime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lab_status1
            // 
            this.lab_status1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lab_status1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lab_status1.Location = new System.Drawing.Point(0, 10);
            this.lab_status1.Name = "lab_status1";
            this.lab_status1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lab_status1.Size = new System.Drawing.Size(91, 30);
            this.lab_status1.TabIndex = 0;
            this.lab_status1.Text = "Last Update";
            this.lab_status1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel_tbl
            // 
            this.panel_tbl.ColumnCount = 1;
            this.panel_tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel_tbl.Controls.Add(this.grid_main, 0, 0);
            this.panel_tbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_tbl.Location = new System.Drawing.Point(0, 0);
            this.panel_tbl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel_tbl.Name = "panel_tbl";
            this.panel_tbl.RowCount = 1;
            this.panel_tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel_tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panel_tbl.Size = new System.Drawing.Size(818, 696);
            this.panel_tbl.TabIndex = 3;
            // 
            // grid_main
            // 
            this.grid_main.AllowUserToAddRows = false;
            this.grid_main.AllowUserToDeleteRows = false;
            this.grid_main.BackgroundColor = System.Drawing.Color.Azure;
            this.grid_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.LAB_SPORT,
            this.LAB_Event_ID,
            this.LAB_TIME,
            this.LAB_Away,
            this.LAB_HOME,
            this.LAB_LEAGUE});
            this.grid_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_main.Location = new System.Drawing.Point(3, 3);
            this.grid_main.Name = "grid_main";
            this.grid_main.ReadOnly = true;
            this.grid_main.RowHeadersVisible = false;
            this.grid_main.RowTemplate.Height = 30;
            this.grid_main.Size = new System.Drawing.Size(812, 690);
            this.grid_main.TabIndex = 9;
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 50;
            // 
            // LAB_SPORT
            // 
            this.LAB_SPORT.HeaderText = "Sport";
            this.LAB_SPORT.Name = "LAB_SPORT";
            this.LAB_SPORT.ReadOnly = true;
            // 
            // LAB_Event_ID
            // 
            this.LAB_Event_ID.HeaderText = "Event ID";
            this.LAB_Event_ID.Name = "LAB_Event_ID";
            this.LAB_Event_ID.ReadOnly = true;
            // 
            // LAB_TIME
            // 
            this.LAB_TIME.HeaderText = "Time";
            this.LAB_TIME.Name = "LAB_TIME";
            this.LAB_TIME.ReadOnly = true;
            // 
            // LAB_Away
            // 
            this.LAB_Away.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LAB_Away.HeaderText = "Away";
            this.LAB_Away.Name = "LAB_Away";
            this.LAB_Away.ReadOnly = true;
            // 
            // LAB_HOME
            // 
            this.LAB_HOME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LAB_HOME.HeaderText = "Home";
            this.LAB_HOME.Name = "LAB_HOME";
            this.LAB_HOME.ReadOnly = true;
            // 
            // LAB_LEAGUE
            // 
            this.LAB_LEAGUE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LAB_LEAGUE.HeaderText = "LEAGUE";
            this.LAB_LEAGUE.Name = "LAB_LEAGUE";
            this.LAB_LEAGUE.ReadOnly = true;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(52)))), ((int)(((byte)(88)))));
            this.ClientSize = new System.Drawing.Size(818, 736);
            this.Controls.Add(this.panel_tbl);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainFrm";
            this.Text = "Form1";
            this.panel2.ResumeLayout(false);
            this.panel_tbl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bind_main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lab_status1;
        private System.Windows.Forms.TableLayoutPanel panel_tbl;
        private System.Windows.Forms.DataGridView grid_main;
        private System.Windows.Forms.BindingSource bind_main;
        private System.Windows.Forms.Label starttime;
        private Bunifu.Framework.UI.BunifuFlatButton btn_start;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAB_SPORT;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAB_Event_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAB_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAB_Away;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAB_HOME;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAB_LEAGUE;
    }
}

