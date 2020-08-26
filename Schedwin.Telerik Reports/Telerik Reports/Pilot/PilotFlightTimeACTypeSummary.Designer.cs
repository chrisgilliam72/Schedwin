namespace Schedwin.Reports.Pilot
{
    partial class PilotFlightTimeACTypeSummary
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PilotFlightTimeACTypeSummary));
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup5 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup6 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup7 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.Area1 = new Telerik.Reporting.ReportHeaderSection();
            this.Text2 = new Telerik.Reporting.TextBox();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.Area3 = new Telerik.Reporting.DetailSection();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            this.Area5 = new Telerik.Reporting.PageFooterSection();
            this.PageNumber1 = new Telerik.Reporting.TextBox();
            this.Text1 = new Telerik.Reporting.TextBox();
            this.Line1 = new Telerik.Reporting.Shape();
            this.pictureBox2 = new Telerik.Reporting.PictureBox();
            this.Text19 = new Telerik.Reporting.TextBox();
            this.PrintDate1 = new Telerik.Reporting.TextBox();
            this.PrintTime1 = new Telerik.Reporting.TextBox();
            this.Text20 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.crosstab1 = new Telerik.Reporting.Crosstab();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Area1
            // 
            this.Area1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.9000000953674316D);
            this.Area1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Text2,
            this.pictureBox1,
            this.Line1,
            this.pictureBox2,
            this.Text19,
            this.PrintDate1,
            this.PrintTime1,
            this.Text20,
            this.textBox7,
            this.textBox9,
            this.textBox8,
            this.textBox11});
            this.Area1.KeepTogether = true;
            this.Area1.Name = "Area1";
            this.Area1.PageBreak = Telerik.Reporting.PageBreak.Before;
            // 
            // Text2
            // 
            this.Text2.CanGrow = false;
            this.Text2.CanShrink = false;
            this.Text2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.Text2.Name = "Text2";
            this.Text2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8833334445953369D), Telerik.Reporting.Drawing.Unit.Inch(0.79166668653488159D));
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
            this.Text2.Value = "Pilot\nFlight Time\nAircraft Type Summary";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.pictureBox1.MimeType = "image/jpeg";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.7000000476837158D), Telerik.Reporting.Drawing.Unit.Inch(1.3000000715255737D));
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // Area3
            // 
            this.Area3.Height = Telerik.Reporting.Drawing.Unit.Inch(1.1000001430511475D);
            this.Area3.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.crosstab1});
            this.Area3.KeepTogether = false;
            this.Area3.Name = "Area3";
            this.Area3.PageBreak = Telerik.Reporting.PageBreak.None;
            this.Area3.Style.BackgroundColor = System.Drawing.Color.White;
            this.Area3.Style.Visible = true;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "Schedwin.Reporting.Telerik.Properties.Settings.SefofaneBots";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@StartDate", System.Data.DbType.DateTime, "2016-07-10"),
            new Telerik.Reporting.SqlDataSourceParameter("@EndDate", System.Data.DbType.DateTime, "2016-07-16")});
            this.sqlDataSource1.SelectCommand = "VIEW.sr_PilotFlightTimeAircraftTypeSummary";
            this.sqlDataSource1.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // Area5
            // 
            this.Area5.Height = Telerik.Reporting.Drawing.Unit.Inch(0.39999982714653015D);
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
            this.PageNumber1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.3666667938232422D), Telerik.Reporting.Drawing.Unit.Inch(0.099999748170375824D));
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
            this.Text1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.7000000476837158D), Telerik.Reporting.Drawing.Unit.Inch(0.099999748170375824D));
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
            // Line1
            // 
            this.Line1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(1.4730743169784546D));
            this.Line1.Name = "Line1";
            this.Line1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.Line1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.1000003814697266D), Telerik.Reporting.Drawing.Unit.Point(9.1471595764160156D));
            this.Line1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(69)))), ((int)(((byte)(116)))));
            this.Line1.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
            this.Line1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.Line1.Style.Visible = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(8.1000795364379883D), Telerik.Reporting.Drawing.Unit.Inch(1.3999608755111694D));
            this.pictureBox2.MimeType = "image/png";
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.29996046423912048D), Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D));
            this.pictureBox2.Value = ((object)(resources.GetObject("pictureBox2.Value")));
            // 
            // Text19
            // 
            this.Text19.CanGrow = false;
            this.Text19.CanShrink = false;
            this.Text19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.8166670799255371D), Telerik.Reporting.Drawing.Unit.Inch(0.099305547773838043D));
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
            // PrintDate1
            // 
            this.PrintDate1.CanGrow = false;
            this.PrintDate1.CanShrink = false;
            this.PrintDate1.Format = "{0:d}";
            this.PrintDate1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.7333335876464844D), Telerik.Reporting.Drawing.Unit.Inch(0.25624999403953552D));
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
            // PrintTime1
            // 
            this.PrintTime1.CanGrow = false;
            this.PrintTime1.CanShrink = false;
            this.PrintTime1.Format = "{0:T}";
            this.PrintTime1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(7.7333335876464844D), Telerik.Reporting.Drawing.Unit.Inch(0.089583314955234528D));
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
            // Text20
            // 
            this.Text20.CanGrow = false;
            this.Text20.CanShrink = false;
            this.Text20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.8166670799255371D), Telerik.Reporting.Drawing.Unit.Inch(0.25624999403953552D));
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
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D), Telerik.Reporting.Drawing.Unit.Inch(1.7000001668930054D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.50000005960464478D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.Value = "Date:";
            // 
            // textBox9
            // 
            this.textBox9.Format = "{0:dd-MMM-yyy}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.8999999761581421D), Telerik.Reporting.Drawing.Unit.Inch(1.7000001668930054D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.89783763885498047D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox9.Style.Font.Bold = true;
            this.textBox9.Value = "= Parameters.EndDate.Value";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(1.7000001668930054D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.2999211847782135D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox8.Value = "To :";
            // 
            // textBox11
            // 
            this.textBox11.Format = "{0:dd-MMM-yyy}";
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.70208340883255D), Telerik.Reporting.Drawing.Unit.Inch(1.7000001668930054D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.89783763885498047D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox11.Style.Font.Bold = true;
            this.textBox11.Value = "= Parameters.StartDate.Value";
            // 
            // textBox5
            // 
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.23125000298023224D));
            this.textBox5.Style.BackgroundColor = System.Drawing.Color.LightBlue;
            this.textBox5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox5.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox5.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.StyleName = "";
            this.textBox5.Value = "Hammer Time";
            // 
            // textBox4
            // 
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.textBox4.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox4.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.StyleName = "";
            this.textBox4.Value = "Aircraft Flight";
            // 
            // textBox3
            // 
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(0.23958326876163483D));
            this.textBox3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox3.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.StyleName = "";
            // 
            // textBox2
            // 
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2708330154418945D), Telerik.Reporting.Drawing.Unit.Inch(0.23958326876163483D));
            this.textBox2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.StyleName = "";
            this.textBox2.Value = "= Fields.ACType";
            // 
            // textBox6
            // 
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9062501192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.48125001788139343D));
            this.textBox6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.StyleName = "";
            this.textBox6.Value = "= Fields.Pilot";
            // 
            // textBox14
            // 
            this.textBox14.Format = "{0:#.00}";
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2708332538604736D), Telerik.Reporting.Drawing.Unit.Inch(0.23125000298023224D));
            this.textBox14.Style.BackgroundColor = System.Drawing.Color.LightBlue;
            this.textBox14.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox14.StyleName = "";
            this.textBox14.Value = "= Sum(Fields.PFlightTime)";
            // 
            // textBox10
            // 
            this.textBox10.Format = "{0:#.00}";
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2708333730697632D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.textBox10.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox10.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox10.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox10.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.StyleName = "";
            this.textBox10.Value = "= Sum(Fields.AFlightTime)";
            // 
            // textBox1
            // 
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9062501192092896D), Telerik.Reporting.Drawing.Unit.Inch(0.23958323895931244D));
            this.textBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // crosstab1
            // 
            this.crosstab1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.99999994039535522D)));
            this.crosstab1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.2708333730697632D)));
            this.crosstab1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24999998509883881D)));
            this.crosstab1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.23124998807907105D)));
            this.crosstab1.Body.SetCellContent(0, 1, this.textBox10);
            this.crosstab1.Body.SetCellContent(1, 1, this.textBox14);
            this.crosstab1.Body.SetCellContent(0, 0, this.textBox4);
            this.crosstab1.Body.SetCellContent(1, 0, this.textBox5);
            tableGroup2.Name = "group2";
            tableGroup1.ChildGroups.Add(tableGroup2);
            tableGroup1.Name = "group1";
            tableGroup1.ReportItem = this.textBox3;
            tableGroup4.Name = "group";
            tableGroup3.ChildGroups.Add(tableGroup4);
            tableGroup3.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.ACType"));
            tableGroup3.Name = "aCType";
            tableGroup3.ReportItem = this.textBox2;
            tableGroup3.Sortings.Add(new Telerik.Reporting.Sorting("= Fields.ACType", Telerik.Reporting.SortDirection.Asc));
            this.crosstab1.ColumnGroups.Add(tableGroup1);
            this.crosstab1.ColumnGroups.Add(tableGroup3);
            this.crosstab1.Corner.SetCellContent(0, 0, this.textBox1);
            this.crosstab1.DataSource = null;
            this.crosstab1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox4,
            this.textBox10,
            this.textBox5,
            this.textBox14,
            this.textBox3,
            this.textBox2,
            this.textBox6});
            this.crosstab1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D), Telerik.Reporting.Drawing.Unit.Inch(0.21527798473834992D));
            this.crosstab1.Name = "crosstab1";
            tableGroup6.Name = "group4";
            tableGroup7.Name = "group6";
            tableGroup5.ChildGroups.Add(tableGroup6);
            tableGroup5.ChildGroups.Add(tableGroup7);
            tableGroup5.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.Pilot"));
            tableGroup5.Name = "pilot";
            tableGroup5.ReportItem = this.textBox6;
            tableGroup5.Sortings.Add(new Telerik.Reporting.Sorting("= Fields.Pilot", Telerik.Reporting.SortDirection.Asc));
            this.crosstab1.RowGroups.Add(tableGroup5);
            this.crosstab1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.1770834922790527D), Telerik.Reporting.Drawing.Unit.Inch(0.72083324193954468D));
            this.crosstab1.Style.Visible = true;
            this.crosstab1.NeedDataSource += new System.EventHandler(this.crosstab1_NeedDataSource);
            // 
            // PilotFlightTimeACTypeSummary
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Area1,
            this.Area3,
            this.Area5});
            this.Name = "rptPilotFlightTimeACTypeSummary";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.25D), Telerik.Reporting.Drawing.Unit.Inch(0.25D), Telerik.Reporting.Drawing.Unit.Inch(0.25D), Telerik.Reporting.Drawing.Unit.Inch(0.25D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "StartDate";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter1.Value = "2016-07-10";
            reportParameter2.Name = "EndDate";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter2.Value = "2016-07-16";
            reportParameter3.Name = "ConnectionString";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.5000009536743164D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.ReportHeaderSection Area1;
        private Telerik.Reporting.TextBox Text2;
        private Telerik.Reporting.DetailSection Area3;
        private Telerik.Reporting.PageFooterSection Area5;
        private Telerik.Reporting.TextBox PageNumber1;
        private Telerik.Reporting.TextBox Text1;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.Shape Line1;
        private Telerik.Reporting.PictureBox pictureBox2;
        private Telerik.Reporting.TextBox Text19;
        private Telerik.Reporting.TextBox PrintDate1;
        private Telerik.Reporting.TextBox PrintTime1;
        private Telerik.Reporting.TextBox Text20;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.Crosstab crosstab1;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox6;
    }
}