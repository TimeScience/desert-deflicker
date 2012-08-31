
// This file has been generated by the GUI designer. Do not modify.
namespace DeSERt
{
	public partial class Settings
	{
		private global::Gtk.Fixed SettingsBG;
		private global::Gtk.Label ProgramUseLabel;
		private global::Gtk.ComboBox ProgramSelBox;
		private global::Gtk.Label ProgramPathLabel;
		private global::Gtk.FileChooserButton ProgramPathChoose;
		private global::Gtk.Label ThreadesLabel;
		private global::Gtk.SpinButton ThreadcountSpin;
		private global::Gtk.CheckButton AutothreadsChkBox;
		private global::Gtk.Label SavingLabel;
		private global::Gtk.ComboBox SavingFormatBox;
		private global::Gtk.Label BitDepthLabel;
		private global::Gtk.SpinButton QualitySpin;
		private global::Gtk.ComboBox BitDepthBox;
		private global::Gtk.Label QualityLabel;
		private global::Gtk.CheckButton CompressChkBox;
		private global::Gtk.Label OtherLabel;
		private global::Gtk.CheckButton KeepPPChkBox;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget DeSERt.Settings
			this.WidthRequest = 285;
			this.HeightRequest = 400;
			this.Name = "DeSERt.Settings";
			this.Title = global::Mono.Unix.Catalog.GetString ("Settings");
			this.Icon = global::Stetic.IconLoader.LoadIcon (this, "gtk-preferences", global::Gtk.IconSize.Menu);
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Modal = true;
			this.Resizable = false;
			this.AllowGrow = false;
			this.DefaultWidth = 350;
			this.DefaultHeight = 410;
			// Internal child DeSERt.Settings.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "SettingsVBox";
			w1.BorderWidth = ((uint)(2));
			// Container child SettingsVBox.Gtk.Box+BoxChild
			this.SettingsBG = new global::Gtk.Fixed ();
			this.SettingsBG.HeightRequest = 320;
			this.SettingsBG.Name = "SettingsBG";
			this.SettingsBG.HasWindow = false;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.ProgramUseLabel = new global::Gtk.Label ();
			this.ProgramUseLabel.Name = "ProgramUseLabel";
			this.ProgramUseLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Program to use:");
			this.SettingsBG.Add (this.ProgramUseLabel);
			global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.ProgramUseLabel]));
			w2.X = 10;
			w2.Y = 10;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.ProgramSelBox = global::Gtk.ComboBox.NewText ();
			this.ProgramSelBox.AppendText (global::Mono.Unix.Catalog.GetString ("Raw Therapee\r"));
			this.ProgramSelBox.AppendText (global::Mono.Unix.Catalog.GetString ("Lightroom"));
			this.ProgramSelBox.WidthRequest = 170;
			this.ProgramSelBox.Name = "ProgramSelBox";
			this.ProgramSelBox.Active = 0;
			this.SettingsBG.Add (this.ProgramSelBox);
			global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.ProgramSelBox]));
			w3.X = 30;
			w3.Y = 30;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.ProgramPathLabel = new global::Gtk.Label ();
			this.ProgramPathLabel.Name = "ProgramPathLabel";
			this.ProgramPathLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Path to Program:");
			this.SettingsBG.Add (this.ProgramPathLabel);
			global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.ProgramPathLabel]));
			w4.X = 10;
			w4.Y = 60;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.ProgramPathChoose = new global::Gtk.FileChooserButton (global::Mono.Unix.Catalog.GetString ("Select Program"), ((global::Gtk.FileChooserAction)(0)));
			this.ProgramPathChoose.WidthRequest = 170;
			this.ProgramPathChoose.Name = "ProgramPathChoose";
			this.ProgramPathChoose.LocalOnly = false;
			this.SettingsBG.Add (this.ProgramPathChoose);
			global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.ProgramPathChoose]));
			w5.X = 30;
			w5.Y = 80;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.ThreadesLabel = new global::Gtk.Label ();
			this.ThreadesLabel.Name = "ThreadesLabel";
			this.ThreadesLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Threads:");
			this.SettingsBG.Add (this.ThreadesLabel);
			global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.ThreadesLabel]));
			w6.X = 10;
			w6.Y = 120;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.ThreadcountSpin = new global::Gtk.SpinButton (1D, 50D, 1D);
			this.ThreadcountSpin.WidthRequest = 40;
			this.ThreadcountSpin.CanFocus = true;
			this.ThreadcountSpin.Name = "ThreadcountSpin";
			this.ThreadcountSpin.Adjustment.PageIncrement = 1D;
			this.ThreadcountSpin.ClimbRate = 1D;
			this.ThreadcountSpin.Numeric = true;
			this.ThreadcountSpin.Value = 1D;
			this.SettingsBG.Add (this.ThreadcountSpin);
			global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.ThreadcountSpin]));
			w7.X = 30;
			w7.Y = 140;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.AutothreadsChkBox = new global::Gtk.CheckButton ();
			this.AutothreadsChkBox.CanFocus = true;
			this.AutothreadsChkBox.Name = "AutothreadsChkBox";
			this.AutothreadsChkBox.Label = global::Mono.Unix.Catalog.GetString ("Autothreads");
			this.AutothreadsChkBox.DrawIndicator = true;
			this.AutothreadsChkBox.UseUnderline = true;
			this.SettingsBG.Add (this.AutothreadsChkBox);
			global::Gtk.Fixed.FixedChild w8 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.AutothreadsChkBox]));
			w8.X = 140;
			w8.Y = 140;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.SavingLabel = new global::Gtk.Label ();
			this.SavingLabel.Name = "SavingLabel";
			this.SavingLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Saving Format:");
			this.SettingsBG.Add (this.SavingLabel);
			global::Gtk.Fixed.FixedChild w9 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.SavingLabel]));
			w9.X = 10;
			w9.Y = 180;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.SavingFormatBox = global::Gtk.ComboBox.NewText ();
			this.SavingFormatBox.AppendText (global::Mono.Unix.Catalog.GetString ("jpg\r"));
			this.SavingFormatBox.AppendText (global::Mono.Unix.Catalog.GetString ("png\r"));
			this.SavingFormatBox.AppendText (global::Mono.Unix.Catalog.GetString ("tiff"));
			this.SavingFormatBox.WidthRequest = 70;
			this.SavingFormatBox.Name = "SavingFormatBox";
			this.SavingFormatBox.Active = 0;
			this.SettingsBG.Add (this.SavingFormatBox);
			global::Gtk.Fixed.FixedChild w10 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.SavingFormatBox]));
			w10.X = 30;
			w10.Y = 200;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.BitDepthLabel = new global::Gtk.Label ();
			this.BitDepthLabel.Name = "BitDepthLabel";
			this.BitDepthLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Bit Depth:");
			this.SettingsBG.Add (this.BitDepthLabel);
			global::Gtk.Fixed.FixedChild w11 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.BitDepthLabel]));
			w11.X = 145;
			w11.Y = 230;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.QualitySpin = new global::Gtk.SpinButton (1D, 100D, 1D);
			this.QualitySpin.CanFocus = true;
			this.QualitySpin.Name = "QualitySpin";
			this.QualitySpin.Adjustment.PageIncrement = 10D;
			this.QualitySpin.ClimbRate = 1D;
			this.QualitySpin.Numeric = true;
			this.QualitySpin.Value = 100D;
			this.SettingsBG.Add (this.QualitySpin);
			global::Gtk.Fixed.FixedChild w12 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.QualitySpin]));
			w12.X = 210;
			w12.Y = 195;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.BitDepthBox = global::Gtk.ComboBox.NewText ();
			this.BitDepthBox.AppendText (global::Mono.Unix.Catalog.GetString ("8\r"));
			this.BitDepthBox.AppendText (global::Mono.Unix.Catalog.GetString ("16"));
			this.BitDepthBox.WidthRequest = 50;
			this.BitDepthBox.Sensitive = false;
			this.BitDepthBox.Name = "BitDepthBox";
			this.BitDepthBox.Active = 0;
			this.SettingsBG.Add (this.BitDepthBox);
			global::Gtk.Fixed.FixedChild w13 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.BitDepthBox]));
			w13.X = 210;
			w13.Y = 225;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.QualityLabel = new global::Gtk.Label ();
			this.QualityLabel.Name = "QualityLabel";
			this.QualityLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Quality:");
			this.SettingsBG.Add (this.QualityLabel);
			global::Gtk.Fixed.FixedChild w14 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.QualityLabel]));
			w14.X = 145;
			w14.Y = 200;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.CompressChkBox = new global::Gtk.CheckButton ();
			this.CompressChkBox.CanFocus = true;
			this.CompressChkBox.Name = "CompressChkBox";
			this.CompressChkBox.Label = global::Mono.Unix.Catalog.GetString ("Compressed");
			this.CompressChkBox.DrawIndicator = true;
			this.CompressChkBox.UseUnderline = true;
			this.SettingsBG.Add (this.CompressChkBox);
			global::Gtk.Fixed.FixedChild w15 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.CompressChkBox]));
			w15.X = 30;
			w15.Y = 227;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.OtherLabel = new global::Gtk.Label ();
			this.OtherLabel.Name = "OtherLabel";
			this.OtherLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Other Settings:");
			this.SettingsBG.Add (this.OtherLabel);
			global::Gtk.Fixed.FixedChild w16 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.OtherLabel]));
			w16.X = 10;
			w16.Y = 260;
			// Container child SettingsBG.Gtk.Fixed+FixedChild
			this.KeepPPChkBox = new global::Gtk.CheckButton ();
			this.KeepPPChkBox.CanFocus = true;
			this.KeepPPChkBox.Name = "KeepPPChkBox";
			this.KeepPPChkBox.Label = global::Mono.Unix.Catalog.GetString ("Keep Postprocessing Files (PP3/XMP)");
			this.KeepPPChkBox.DrawIndicator = true;
			this.KeepPPChkBox.UseUnderline = true;
			this.SettingsBG.Add (this.KeepPPChkBox);
			global::Gtk.Fixed.FixedChild w17 = ((global::Gtk.Fixed.FixedChild)(this.SettingsBG [this.KeepPPChkBox]));
			w17.X = 30;
			w17.Y = 280;
			w1.Add (this.SettingsBG);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(w1 [this.SettingsBG]));
			w18.Position = 0;
			w18.Expand = false;
			w18.Fill = false;
			// Internal child DeSERt.Settings.ActionArea
			global::Gtk.HButtonBox w19 = this.ActionArea;
			w19.Name = "ButtonsBG";
			w19.Spacing = 10;
			w19.BorderWidth = ((uint)(5));
			w19.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child ButtonsBG.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w20 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w19 [this.buttonCancel]));
			w20.Expand = false;
			w20.Fill = false;
			// Container child ButtonsBG.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget (this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w21 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w19 [this.buttonOk]));
			w21.Position = 1;
			w21.Expand = false;
			w21.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
			this.ProgramSelBox.Changed += new global::System.EventHandler (this.OnProgramSelBoxChanged);
			this.ProgramPathChoose.SelectionChanged += new global::System.EventHandler (this.OnProgramPathChooseSelectionChanged);
			this.AutothreadsChkBox.Toggled += new global::System.EventHandler (this.OnAutothreadsChkBoxToggled);
			this.SavingFormatBox.Changed += new global::System.EventHandler (this.OnSavingFormatBoxChanged);
			this.buttonCancel.Clicked += new global::System.EventHandler (this.OnButtonCancelClicked);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
