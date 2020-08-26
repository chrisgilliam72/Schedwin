namespace Schedwin.Reports.Pilot
{
    partial class PilotFlyingHours
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PilotFlyingHours));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter5 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.Area3 = new Telerik.Reporting.DetailSection();
            this.FlightTime1 = new Telerik.Reporting.TextBox();
            this.TravelDate1 = new Telerik.Reporting.TextBox();
            this.Pilot1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.Area4 = new Telerik.Reporting.ReportFooterSection();
            this.Area5 = new Telerik.Reporting.PageFooterSection();
            this.PageNumber1 = new Telerik.Reporting.TextBox();
            this.Text1 = new Telerik.Reporting.TextBox();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.shape1 = new Telerik.Reporting.Shape();
            this.pictureBox2 = new Telerik.Reporting.PictureBox();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.Text2 = new Telerik.Reporting.TextBox();
            this.Text20 = new Telerik.Reporting.TextBox();
            this.PrintTime1 = new Telerik.Reporting.TextBox();
            this.PrintDate1 = new Telerik.Reporting.TextBox();
            this.Text19 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.Text8 = new Telerik.Reporting.TextBox();
            this.Text7 = new Telerik.Reporting.TextBox();
            this.Text6 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.592000424861908D);
            this.groupFooterSection.Name = "groupFooterSection";
            this.groupFooterSection.PageBreak = Telerik.Reporting.PageBreak.After;
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.39999905228614807D);
            this.groupHeaderSection.Name = "groupHeaderSection";
            // 
            // Area3
            // 
            this.Area3.Height = Telerik.Reporting.Drawing.Unit.Inch(0.19999971985816956D);
            this.Area3.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.FlightTime1,
            this.TravelDate1,
            this.Pilot1,
            this.textBox2});
            this.Area3.KeepTogether = false;
            this.Area3.Name = "Area3";
            this.Area3.PageBreak = Telerik.Reporting.PageBreak.None;
            this.Area3.Style.BackgroundColor = System.Drawing.Color.White;
            this.Area3.Style.Visible = true;
            // 
            // FlightTime1
            // 
            this.FlightTime1.CanGrow = false;
            this.FlightTime1.CanShrink = false;
            this.FlightTime1.Format = "{0:#.00}";
            this.FlightTime1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.3854165077209473D), Telerik.Reporting.Drawing.Unit.Inch(0.02083333395421505D));
            this.FlightTime1.Name = "FlightTime1";
            this.FlightTime1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6000000238418579D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.FlightTime1.Style.BackgroundColor = System.Drawing.Color.White;
            this.FlightTime1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.FlightTime1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.FlightTime1.Style.Color = System.Drawing.Color.Black;
            this.FlightTime1.Style.Font.Bold = false;
            this.FlightTime1.Style.Font.Italic = false;
            this.FlightTime1.Style.Font.Name = "Arial";
            this.FlightTime1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.FlightTime1.Style.Font.Strikeout = false;
            this.FlightTime1.Style.Font.Underline = false;
            this.FlightTime1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.FlightTime1.Style.Visible = true;
            this.FlightTime1.Value = "= Fields.AFlightTime";
            // 
            // TravelDate1
            // 
            this.TravelDate1.CanGrow = false;
            this.TravelDate1.CanShrink = false;
            this.TravelDate1.Format = "{0:dd-MMM-yyy}";
            this.TravelDate1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8541667461395264D), Telerik.Reporting.Drawing.Unit.Inch(0.02083333395421505D));
            this.TravelDate1.Name = "TravelDate1";
            this.TravelDate1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4000002145767212D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.TravelDate1.Style.BackgroundColor = System.Drawing.Color.White;
            this.TravelDate1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.TravelDate1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.TravelDate1.Style.Color = System.Drawing.Color.Black;
            this.TravelDate1.Style.Font.Bold = false;
            this.TravelDate1.Style.Font.Italic = false;
            this.TravelDate1.Style.Font.Name = "Arial";
            this.TravelDate1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.TravelDate1.Style.Font.Strikeout = false;
            this.TravelDate1.Style.Font.Underline = false;
            this.TravelDate1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.TravelDate1.Style.Visible = true;
            this.TravelDate1.Value = "=Fields.[TravelDate]";
            // 
            // Pilot1
            // 
            this.Pilot1.CanGrow = false;
            this.Pilot1.CanShrink = false;
            this.Pilot1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0416666679084301D), Telerik.Reporting.Drawing.Unit.Inch(0.02083333395421505D));
            this.Pilot1.Name = "Pilot1";
            this.Pilot1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.6166665554046631D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.Pilot1.Style.BackgroundColor = System.Drawing.Color.White;
            this.Pilot1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Pilot1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.Pilot1.Style.Color = System.Drawing.Color.Black;
            this.Pilot1.Style.Font.Bold = false;
            this.Pilot1.Style.Font.Italic = false;
            this.Pilot1.Style.Font.Name = "Arial";
            this.Pilot1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.Pilot1.Style.Font.Strikeout = false;
            this.Pilot1.Style.Font.Underline = false;
            this.Pilot1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.Pilot1.Style.Visible = true;
            this.Pilot1.Value = "=Fields.[Pilot]";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.CanShrink = true;
            this.textBox2.Format = "{0:N2}";
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.0354170799255371D), Telerik.Reporting.Drawing.Unit.Inch(0.02083333395421505D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6000000238418579D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.textBox2.Style.BackgroundColor = System.Drawing.Color.White;
            this.textBox2.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.Color = System.Drawing.Color.Black;
            this.textBox2.Style.Font.Bold = false;
            this.textBox2.Style.Font.Italic = false;
            this.textBox2.Style.Font.Name = "Arial";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox2.Style.Font.Strikeout = false;
            this.textBox2.Style.Font.Underline = false;
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.Style.Visible = true;
            this.textBox2.Value = "= Fields.PFlightTime";
            // 
            // Area4
            // 
            this.Area4.Height = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.Area4.KeepTogether = false;
            this.Area4.Name = "Area4";
            this.Area4.PageBreak = Telerik.Reporting.PageBreak.After;
            this.Area4.Style.BackgroundColor = System.Drawing.Color.White;
            this.Area4.Style.Visible = true;
            // 
            // Area5
            // 
            this.Area5.Height = Telerik.Reporting.Drawing.Unit.Inch(0.24301619827747345D);
            this.Area5.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.PageNumber1,
            this.Text1});
            this.Area5.Name = "Area5";
            this.Area5.PrintOnFirstPage = true;
            this.Area5.PrintOnLastPage = true;
            this.Area5.Style.BackgroundColor = System.Drawing.Color.White;
            this.Area5.Style.Visible = true;
            // 
            // PageNumber1
            // 
            this.PageNumber1.CanGrow = false;
            this.PageNumber1.CanShrink = false;
            this.PageNumber1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.2666668891906738D), Telerik.Reporting.Drawing.Unit.Inch(0.08954397588968277D));
            this.PageNumber1.Name = "PageNumber1";
            this.PageNumber1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.4166666567325592D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.PageNumber1.Style.BackgroundColor = System.Drawing.Color.White;
            this.PageNumber1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.PageNumber1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.PageNumber1.Style.Color = System.Drawing.Color.Black;
            this.PageNumber1.Style.Font.Bold = false;
            this.PageNumber1.Style.Font.Italic = false;
            this.PageNumber1.Style.Font.Name = "Arial";
            this.PageNumber1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.PageNumber1.Style.Font.Strikeout = false;
            this.PageNumber1.Style.Font.Underline = false;
            this.PageNumber1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.PageNumber1.Style.Visible = true;
            this.PageNumber1.Value = "=PageNumber";
            // 
            // Text1
            // 
            this.Text1.CanGrow = false;
            this.Text1.CanShrink = false;
            this.Text1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.6000003814697266D), Telerik.Reporting.Drawing.Unit.Inch(0.08954397588968277D));
            this.Text1.Name = "Text1";
            this.Text1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.66666668653488159D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.Text1.Style.BackgroundColor = System.Drawing.Color.White;
            this.Text1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Text1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.Text1.Style.Color = System.Drawing.Color.Black;
            this.Text1.Style.Font.Bold = true;
            this.Text1.Style.Font.Italic = false;
            this.Text1.Style.Font.Name = "Times New Roman";
            this.Text1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.Text1.Style.Font.Strikeout = false;
            this.Text1.Style.Font.Underline = false;
            this.Text1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.Text1.Style.Visible = true;
            this.Text1.Value = "Page:";
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "Schedwin.Reporting.Telerik.Properties.Settings.SefofaneBots";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@StartDate", System.Data.DbType.DateTime, "= Parameters.StartDate.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@EndDate", System.Data.DbType.DateTime, "= Parameters.EndDate.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@IDX_Personnel", System.Data.DbType.Int32, "= Parameters.Pilot.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@PilotMultiplier", System.Data.DbType.Double, null)});
            this.sqlDataSource1.SelectCommand = "VIEW.sr_PilotFlyingHours";
            this.sqlDataSource1.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(2.2834649085998535D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.shape1,
            this.pictureBox2,
            this.pictureBox1,
            this.Text2,
            this.Text20,
            this.PrintTime1,
            this.PrintDate1,
            this.Text19,
            this.textBox7,
            this.textBox6,
            this.textBox8,
            this.textBox9,
            this.textBox1,
            this.Text8,
            this.Text7,
            this.Text6});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0020834605675190687D), Telerik.Reporting.Drawing.Unit.Inch(1.4833332300186157D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.3333330154418945D), Telerik.Reporting.Drawing.Unit.Inch(0.099921219050884247D));
            this.shape1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(69)))), ((int)(((byte)(116)))));
            this.shape1.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(69)))), ((int)(((byte)(116)))));
            this.shape1.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.shape1.Style.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(69)))), ((int)(((byte)(116)))));
            this.shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(2D);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.3354167938232422D), Telerik.Reporting.Drawing.Unit.Inch(1.3791666030883789D));
            this.pictureBox2.MimeType = "image/png";
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.29996046423912048D), Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D));
            this.pictureBox2.Value = ((object)(resources.GetObject("pictureBox2.Value")));
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0020834605675190687D), Telerik.Reporting.Drawing.Unit.Inch(0.056250017136335373D));
            this.pictureBox1.MimeType = "image/jpeg";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.7000000476837158D), Telerik.Reporting.Drawing.Unit.Inch(1.3000000715255737D));
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // Text2
            // 
            this.Text2.CanGrow = false;
            this.Text2.CanShrink = false;
            this.Text2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.702162504196167D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.Text2.Name = "Text2";
            this.Text2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.6999213695526123D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.Text2.Style.BackgroundColor = System.Drawing.Color.White;
            this.Text2.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Text2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.Text2.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(69)))), ((int)(((byte)(116)))));
            this.Text2.Style.Font.Bold = true;
            this.Text2.Style.Font.Italic = false;
            this.Text2.Style.Font.Name = "Arial";
            this.Text2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.Text2.Style.Font.Strikeout = false;
            this.Text2.Style.Font.Underline = false;
            this.Text2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.Text2.Style.Visible = true;
            this.Text2.Value = "Pilot\nFlying Hours";
            // 
            // Text20
            // 
            this.Text20.CanGrow = false;
            this.Text20.CanShrink = false;
            this.Text20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.03337287902832D), Telerik.Reporting.Drawing.Unit.Inch(0.22299535572528839D));
            this.Text20.Name = "Text20";
            this.Text20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.91666668653488159D), Telerik.Reporting.Drawing.Unit.Inch(0.15694443881511688D));
            this.Text20.Style.BackgroundColor = System.Drawing.Color.White;
            this.Text20.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Text20.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.Text20.Style.Color = System.Drawing.Color.Black;
            this.Text20.Style.Font.Bold = true;
            this.Text20.Style.Font.Italic = false;
            this.Text20.Style.Font.Name = "Arial";
            this.Text20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.Text20.Style.Font.Strikeout = false;
            this.Text20.Style.Font.Underline = false;
            this.Text20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.Text20.Style.Visible = true;
            this.Text20.Value = "Print Date:";
            // 
            // PrintTime1
            // 
            this.PrintTime1.CanGrow = false;
            this.PrintTime1.CanShrink = false;
            this.PrintTime1.Format = "{0:T}";
            this.PrintTime1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.9667067527771D), Telerik.Reporting.Drawing.Unit.Inch(0.056250017136335373D));
            this.PrintTime1.Name = "PrintTime1";
            this.PrintTime1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.66666668653488159D), Telerik.Reporting.Drawing.Unit.Inch(0.1666666716337204D));
            this.PrintTime1.Style.BackgroundColor = System.Drawing.Color.White;
            this.PrintTime1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.PrintTime1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.PrintTime1.Style.Color = System.Drawing.Color.Black;
            this.PrintTime1.Style.Font.Bold = false;
            this.PrintTime1.Style.Font.Italic = false;
            this.PrintTime1.Style.Font.Name = "Arial";
            this.PrintTime1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.PrintTime1.Style.Font.Strikeout = false;
            this.PrintTime1.Style.Font.Underline = false;
            this.PrintTime1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.PrintTime1.Style.Visible = true;
            this.PrintTime1.Value = "=Now()";
            // 
            // PrintDate1
            // 
            this.PrintDate1.CanGrow = false;
            this.PrintDate1.CanShrink = false;
            this.PrintDate1.Format = "{0:d}";
            this.PrintDate1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.9667067527771D), Telerik.Reporting.Drawing.Unit.Inch(0.22299535572528839D));
            this.PrintDate1.Name = "PrintDate1";
            this.PrintDate1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.66666668653488159D), Telerik.Reporting.Drawing.Unit.Inch(0.1458333283662796D));
            this.PrintDate1.Style.BackgroundColor = System.Drawing.Color.White;
            this.PrintDate1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.PrintDate1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.PrintDate1.Style.Color = System.Drawing.Color.Black;
            this.PrintDate1.Style.Font.Bold = false;
            this.PrintDate1.Style.Font.Italic = false;
            this.PrintDate1.Style.Font.Name = "Arial";
            this.PrintDate1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.PrintDate1.Style.Font.Strikeout = false;
            this.PrintDate1.Style.Font.Underline = false;
            this.PrintDate1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.PrintDate1.Style.Visible = true;
            this.PrintDate1.Value = "=Now()";
            // 
            // Text19
            // 
            this.Text19.CanGrow = false;
            this.Text19.CanShrink = false;
            this.Text19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.03337287902832D), Telerik.Reporting.Drawing.Unit.Inch(0.06597224622964859D));
            this.Text19.Name = "Text19";
            this.Text19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.91666668653488159D), Telerik.Reporting.Drawing.Unit.Inch(0.15694443881511688D));
            this.Text19.Style.BackgroundColor = System.Drawing.Color.White;
            this.Text19.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Text19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.Text19.Style.Color = System.Drawing.Color.Black;
            this.Text19.Style.Font.Bold = true;
            this.Text19.Style.Font.Italic = false;
            this.Text19.Style.Font.Name = "Arial";
            this.Text19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.Text19.Style.Font.Strikeout = false;
            this.Text19.Style.Font.Underline = false;
            this.Text19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.Text19.Style.Visible = true;
            this.Text19.Value = "Print Time:";
            // 
            // textBox7
            // 
            this.textBox7.Format = "{0:dd-MMM-yyy}";
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.516627311706543D), Telerik.Reporting.Drawing.Unit.Inch(1.7000001668930054D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.99791651964187622D), Telerik.Reporting.Drawing.Unit.Inch(0.23541657626628876D));
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Value = "= Parameters.StartDate.Value";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.016666730865836143D), Telerik.Reporting.Drawing.Unit.Inch(1.7000001668930054D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.49988189339637756D), Telerik.Reporting.Drawing.Unit.Inch(0.23541657626628876D));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Value = "Date:";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.514622688293457D), Telerik.Reporting.Drawing.Unit.Inch(1.7000001668930054D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.2999211847782135D), Telerik.Reporting.Drawing.Unit.Inch(0.23541657626628876D));
            this.textBox8.Value = "To :";
            // 
            // textBox9
            // 
            this.textBox9.Format = "{0:dd-MMM-yyy}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.8146227598190308D), Telerik.Reporting.Drawing.Unit.Inch(1.7000001668930054D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2999999523162842D), Telerik.Reporting.Drawing.Unit.Inch(0.23541657626628876D));
            this.textBox9.Style.Font.Bold = true;
            this.textBox9.Value = "= Parameters.EndDate.Value";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = false;
            this.textBox1.CanShrink = false;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.0354170799255371D), Telerik.Reporting.Drawing.Unit.Inch(2.0472440719604492D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6000000238418579D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.textBox1.Style.BackgroundColor = System.Drawing.Color.White;
            this.textBox1.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.textBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox1.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox1.Style.Color = System.Drawing.Color.Black;
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Italic = false;
            this.textBox1.Style.Font.Name = "Arial";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox1.Style.Font.Strikeout = false;
            this.textBox1.Style.Font.Underline = false;
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.Style.Visible = true;
            this.textBox1.Value = "Pilot Flight Time";
            // 
            // Text8
            // 
            this.Text8.CanGrow = false;
            this.Text8.CanShrink = false;
            this.Text8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.4332947731018066D), Telerik.Reporting.Drawing.Unit.Inch(2.0472440719604492D));
            this.Text8.Name = "Text8";
            this.Text8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6000000238418579D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.Text8.Style.BackgroundColor = System.Drawing.Color.White;
            this.Text8.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Text8.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.Text8.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.Text8.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.Text8.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.Text8.Style.Color = System.Drawing.Color.Black;
            this.Text8.Style.Font.Bold = true;
            this.Text8.Style.Font.Italic = false;
            this.Text8.Style.Font.Name = "Arial";
            this.Text8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.Text8.Style.Font.Strikeout = false;
            this.Text8.Style.Font.Underline = false;
            this.Text8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.Text8.Style.Visible = true;
            this.Text8.Value = "Aircraft Flight Time";
            // 
            // Text7
            // 
            this.Text7.CanGrow = false;
            this.Text7.CanShrink = false;
            this.Text7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.9000003337860107D), Telerik.Reporting.Drawing.Unit.Inch(2.0472440719604492D));
            this.Text7.Name = "Text7";
            this.Text7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4000002145767212D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.Text7.Style.BackgroundColor = System.Drawing.Color.White;
            this.Text7.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Text7.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.Text7.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.Text7.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.Text7.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.Text7.Style.Color = System.Drawing.Color.Black;
            this.Text7.Style.Font.Bold = true;
            this.Text7.Style.Font.Italic = false;
            this.Text7.Style.Font.Name = "Arial";
            this.Text7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.Text7.Style.Font.Strikeout = false;
            this.Text7.Style.Font.Underline = false;
            this.Text7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.Text7.Style.Visible = true;
            this.Text7.Value = "Date";
            // 
            // Text6
            // 
            this.Text6.CanGrow = false;
            this.Text6.CanShrink = false;
            this.Text6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0020834605675190687D), Telerik.Reporting.Drawing.Unit.Inch(2.0472440719604492D));
            this.Text6.Name = "Text6";
            this.Text6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.7000000476837158D), Telerik.Reporting.Drawing.Unit.Inch(0.15347221493721008D));
            this.Text6.Style.BackgroundColor = System.Drawing.Color.White;
            this.Text6.Style.BorderColor.Default = System.Drawing.Color.Black;
            this.Text6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.Text6.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.Text6.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.Text6.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.Text6.Style.Color = System.Drawing.Color.Black;
            this.Text6.Style.Font.Bold = true;
            this.Text6.Style.Font.Italic = false;
            this.Text6.Style.Font.Name = "Arial";
            this.Text6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.Text6.Style.Font.Strikeout = false;
            this.Text6.Style.Font.Underline = false;
            this.Text6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.Text6.Style.Visible = true;
            this.Text6.Value = "Pilot";
            // 
            // PilotFlyingHours
            // 
            this.DataSource = null;
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.Pilot"));
            group1.Name = "group";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.Area3,
            this.Area4,
            this.Area5,
            this.pageHeaderSection1});
            this.Name = "rptPilotFlyingHours";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.25D), Telerik.Reporting.Drawing.Unit.Inch(0.25D), Telerik.Reporting.Drawing.Unit.Inch(0.25D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "StartDate";
            reportParameter1.Text = "StartDate";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter1.Value = "2016-09-01";
            reportParameter2.Name = "EndDate";
            reportParameter2.Text = "EndDate";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter2.Value = "2016-09-30";
            reportParameter3.Name = "Pilot";
            reportParameter3.Text = "IDXPersonnel";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter3.Value = "0";
            reportParameter4.AllowNull = true;
            reportParameter4.Name = "PilotMultiplier";
            reportParameter4.Text = "PilotMultiplier";
            reportParameter4.Type = Telerik.Reporting.ReportParameterType.Float;
            reportParameter4.Value = "= Null";
            reportParameter5.Name = "ConnectionString";
            reportParameter5.Text = "ConnectionString";
            reportParameter5.Value = "Schedwin.Reporting.Telerik.Properties.Settings.SefofaneBots";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            this.ReportParameters.Add(reportParameter5);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(7.7677168846130371D);
            this.NeedDataSource += new System.EventHandler(this.PilotFlyingHours_NeedDataSource);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection Area3;
        private Telerik.Reporting.ReportFooterSection Area4;
        private Telerik.Reporting.PageFooterSection Area5;
        private Telerik.Reporting.TextBox PageNumber1;
        private Telerik.Reporting.TextBox Text1;
        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox FlightTime1;
        private Telerik.Reporting.TextBox TravelDate1;
        private Telerik.Reporting.TextBox Pilot1;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.PictureBox pictureBox2;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.TextBox Text2;
        private Telerik.Reporting.TextBox Text20;
        private Telerik.Reporting.TextBox PrintTime1;
        private Telerik.Reporting.TextBox PrintDate1;
        private Telerik.Reporting.TextBox Text19;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox Text8;
        private Telerik.Reporting.TextBox Text7;
        private Telerik.Reporting.TextBox Text6;
    }
}