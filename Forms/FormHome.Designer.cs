namespace Parking
{
    partial class FormHome
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ajsutesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listaVehiculosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitleParking = new System.Windows.Forms.Label();
            this.labelElapsedValue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelCostValue = new System.Windows.Forms.Label();
            this.buttonGenerateBill = new System.Windows.Forms.Button();
            this.buttonPayed = new System.Windows.Forms.Button();
            this.buttonSaveVehicle = new System.Windows.Forms.Button();
            this.labelMessageError = new System.Windows.Forms.Label();
            this.textBoxScanner = new System.Windows.Forms.TextBox();
            this.buttonFocuScanner = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ajsutesToolStripMenuItem,
            this.listaVehiculosToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1325, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ajsutesToolStripMenuItem
            // 
            this.ajsutesToolStripMenuItem.Name = "ajsutesToolStripMenuItem";
            this.ajsutesToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.ajsutesToolStripMenuItem.Text = "Ajustes";
            this.ajsutesToolStripMenuItem.Click += new System.EventHandler(this.ajsutesToolStripMenuItem_Click);
            // 
            // listaVehiculosToolStripMenuItem
            // 
            this.listaVehiculosToolStripMenuItem.Name = "listaVehiculosToolStripMenuItem";
            this.listaVehiculosToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.listaVehiculosToolStripMenuItem.Text = "Lista vehiculos";
            this.listaVehiculosToolStripMenuItem.Click += new System.EventHandler(this.listaVehiculosToolStripMenuItem_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 4);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox3);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(930, 151);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.tableLayoutPanel1.SetRowSpan(this.flowLayoutPanel1, 7);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(252, 237);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(64, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 29);
            this.label3.TabIndex = 0;
            this.label3.Text = "Bicicletas";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox3.Image = global::Parking.Properties.Resources.Bicicleta;
            this.pictureBox3.Location = new System.Drawing.Point(25, 32);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(25, 3, 25, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(200, 200);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 1;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel2, 4);
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.pictureBox1);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(72, 151);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.tableLayoutPanel1.SetRowSpan(this.flowLayoutPanel2, 7);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(252, 237);
            this.flowLayoutPanel2.TabIndex = 1;
            this.flowLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel2_Paint);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(81, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Carros";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Image = global::Parking.Properties.Resources.Automovil;
            this.pictureBox1.Location = new System.Drawing.Point(25, 32);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(25, 3, 25, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 200);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel4, 4);
            this.flowLayoutPanel4.Controls.Add(this.label2);
            this.flowLayoutPanel4.Controls.Add(this.pictureBox2);
            this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(534, 151);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.tableLayoutPanel1.SetRowSpan(this.flowLayoutPanel4, 7);
            this.flowLayoutPanel4.Size = new System.Drawing.Size(252, 237);
            this.flowLayoutPanel4.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(84, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Motos";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox2.Image = global::Parking.Properties.Resources.Moto;
            this.pictureBox2.Location = new System.Drawing.Point(25, 32);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(25, 3, 25, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(200, 200);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 6);
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(361, 504);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(296, 36);
            this.label4.TabIndex = 7;
            this.label4.Text = "Tiempo transcurrido: ";
            this.label4.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label6, 4);
            this.label6.Dock = System.Windows.Forms.DockStyle.Right;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(362, 432);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(229, 36);
            this.label6.TabIndex = 12;
            this.label6.Text = "Ingresar numero de identificacion:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label6.Visible = false;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 2);
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox1.Location = new System.Drawing.Point(597, 435);
            this.textBox1.MaxLength = 0;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(126, 24);
            this.textBox1.TabIndex = 11;
            this.textBox1.Visible = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 20;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 14, 4);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 8, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 4, 14);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 9, 12);
            this.tableLayoutPanel1.Controls.Add(this.label6, 5, 12);
            this.tableLayoutPanel1.Controls.Add(this.labelTitleParking, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelElapsedValue, 10, 14);
            this.tableLayoutPanel1.Controls.Add(this.label7, 6, 16);
            this.tableLayoutPanel1.Controls.Add(this.labelCostValue, 10, 16);
            this.tableLayoutPanel1.Controls.Add(this.buttonGenerateBill, 7, 18);
            this.tableLayoutPanel1.Controls.Add(this.buttonPayed, 11, 18);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveVehicle, 11, 12);
            this.tableLayoutPanel1.Controls.Add(this.labelMessageError, 7, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxScanner, 19, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonFocuScanner, 18, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 20;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1325, 739);
            this.tableLayoutPanel1.TabIndex = 5;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // labelTitleParking
            // 
            this.labelTitleParking.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelTitleParking, 6);
            this.labelTitleParking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitleParking.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelTitleParking.Location = new System.Drawing.Point(465, 0);
            this.labelTitleParking.Name = "labelTitleParking";
            this.labelTitleParking.Size = new System.Drawing.Size(390, 36);
            this.labelTitleParking.TabIndex = 13;
            this.labelTitleParking.Text = "Nombre parqueadero";
            this.labelTitleParking.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelTitleParking.Click += new System.EventHandler(this.label7_Click);
            // 
            // labelElapsedValue
            // 
            this.labelElapsedValue.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelElapsedValue, 3);
            this.labelElapsedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelElapsedValue.Location = new System.Drawing.Point(663, 504);
            this.labelElapsedValue.Name = "labelElapsedValue";
            this.labelElapsedValue.Size = new System.Drawing.Size(111, 29);
            this.labelElapsedValue.TabIndex = 15;
            this.labelElapsedValue.Text = "02:10:30";
            this.labelElapsedValue.Visible = false;
            this.labelElapsedValue.Click += new System.EventHandler(this.labelElapsedValue_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label7, 4);
            this.label7.Dock = System.Windows.Forms.DockStyle.Right;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(556, 576);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 36);
            this.label7.TabIndex = 14;
            this.label7.Text = "Costo:";
            this.label7.Visible = false;
            // 
            // labelCostValue
            // 
            this.labelCostValue.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelCostValue, 3);
            this.labelCostValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelCostValue.Location = new System.Drawing.Point(663, 576);
            this.labelCostValue.Name = "labelCostValue";
            this.labelCostValue.Size = new System.Drawing.Size(184, 29);
            this.labelCostValue.TabIndex = 16;
            this.labelCostValue.Text = "labelCostValue";
            this.labelCostValue.Visible = false;
            // 
            // buttonGenerateBill
            // 
            this.buttonGenerateBill.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonGenerateBill, 2);
            this.buttonGenerateBill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGenerateBill.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buttonGenerateBill.Location = new System.Drawing.Point(465, 651);
            this.buttonGenerateBill.Name = "buttonGenerateBill";
            this.buttonGenerateBill.Size = new System.Drawing.Size(126, 30);
            this.buttonGenerateBill.TabIndex = 17;
            this.buttonGenerateBill.Text = "Generar factura";
            this.buttonGenerateBill.UseVisualStyleBackColor = true;
            this.buttonGenerateBill.Visible = false;
            this.buttonGenerateBill.Click += new System.EventHandler(this.buttonGenerateBill_Click);
            // 
            // buttonPayed
            // 
            this.buttonPayed.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonPayed, 2);
            this.buttonPayed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPayed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buttonPayed.Location = new System.Drawing.Point(729, 651);
            this.buttonPayed.Name = "buttonPayed";
            this.buttonPayed.Size = new System.Drawing.Size(126, 30);
            this.buttonPayed.TabIndex = 18;
            this.buttonPayed.Text = "Factura pagada";
            this.buttonPayed.UseVisualStyleBackColor = true;
            this.buttonPayed.Visible = false;
            this.buttonPayed.Click += new System.EventHandler(this.buttonPayed_Click);
            // 
            // buttonSaveVehicle
            // 
            this.buttonSaveVehicle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonSaveVehicle, 3);
            this.buttonSaveVehicle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buttonSaveVehicle.Location = new System.Drawing.Point(729, 435);
            this.buttonSaveVehicle.Name = "buttonSaveVehicle";
            this.buttonSaveVehicle.Size = new System.Drawing.Size(176, 28);
            this.buttonSaveVehicle.TabIndex = 19;
            this.buttonSaveVehicle.Text = "Guardar y generar ticket";
            this.buttonSaveVehicle.UseVisualStyleBackColor = true;
            this.buttonSaveVehicle.Visible = false;
            this.buttonSaveVehicle.Click += new System.EventHandler(this.buttonSaveVehicle_Click);
            // 
            // labelMessageError
            // 
            this.labelMessageError.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelMessageError, 6);
            this.labelMessageError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessageError.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelMessageError.ForeColor = System.Drawing.Color.Red;
            this.labelMessageError.Location = new System.Drawing.Point(465, 72);
            this.labelMessageError.Name = "labelMessageError";
            this.labelMessageError.Size = new System.Drawing.Size(390, 36);
            this.labelMessageError.TabIndex = 20;
            this.labelMessageError.Text = "Error";
            this.labelMessageError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelMessageError.Visible = false;
            this.labelMessageError.Click += new System.EventHandler(this.labelMessageError_Click);
            // 
            // textBoxScanner
            // 
            this.textBoxScanner.Location = new System.Drawing.Point(1257, 39);
            this.textBoxScanner.Name = "textBoxScanner";
            this.textBoxScanner.Size = new System.Drawing.Size(65, 20);
            this.textBoxScanner.TabIndex = 21;
            // 
            // buttonFocuScanner
            // 
            this.buttonFocuScanner.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonFocuScanner, 2);
            this.buttonFocuScanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFocuScanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buttonFocuScanner.Location = new System.Drawing.Point(1191, 435);
            this.buttonFocuScanner.Name = "buttonFocuScanner";
            this.buttonFocuScanner.Size = new System.Drawing.Size(131, 30);
            this.buttonFocuScanner.TabIndex = 22;
            this.buttonFocuScanner.Text = "Escanear";
            this.buttonFocuScanner.UseVisualStyleBackColor = true;
            // 
            // FormHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1325, 763);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "FormHome";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ajsutesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listaVehiculosToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label labelTitleParking;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelElapsedValue;
        private System.Windows.Forms.Label labelCostValue;
        private System.Windows.Forms.Button buttonGenerateBill;
        private System.Windows.Forms.Button buttonPayed;
        private System.Windows.Forms.Button buttonSaveVehicle;
        private System.Windows.Forms.Label labelMessageError;
        private System.Windows.Forms.TextBox textBoxScanner;
        private System.Windows.Forms.Button buttonFocuScanner;
    }
}

