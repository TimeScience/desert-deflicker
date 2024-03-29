
// This file has been generated by the GUI designer. Do not modify.

public partial class DeSERtMain
{
	private global::Gtk.UIManager UIManager;
	private global::Gtk.Action MenuAction;
	private global::Gtk.Action SettingsAction;
	private global::Gtk.Action AboutAction;
	private global::Gtk.Action HelpAction;
	private global::Gtk.Action newAction;
	private global::Gtk.Action saveAction;
	private global::Gtk.Action saveAsAction;
	private global::Gtk.Action openAction;
	private global::Gtk.Action quitAction;
	private global::Gtk.Action helpAction;
	private global::Gtk.Action aboutAction;
	private global::Gtk.Action preferencesAction;
	private global::Gtk.Action ExtrasAction;
	private global::Gtk.Action CreateFilterAction;
	private global::Gtk.Fixed BG;
	private global::Gtk.MenuBar Menu;
	private global::Gtk.Notebook MainNotebook;
	private global::Gtk.Fixed BGLoading;
	private global::Gtk.ScrolledWindow ValueTableScroll;
	private global::Gtk.TreeView ValueTable;
	private global::Gtk.Button ImageOpenButton;
	private global::Gtk.Button ProcOpenButton;
	private global::Gtk.Button CalculateButton;
	private global::Gtk.Image PreviewImg;
	private global::Gtk.Label label2;
	private global::Gtk.Fixed EditingBG;
	private global::Gtk.Entry XValBox;
	private global::Gtk.Entry YValBox;
	private global::Gtk.Label Ylabel;
	private global::Gtk.Label Xlabel;
	private global::Gtk.EventBox GraphEvents;
	private global::Gtk.Image GraphArea;
	private global::Gtk.ComboBox CurveSelectBox;
	private global::Gtk.HScale BrScale;
	private global::Gtk.Button EndToStartButton;
	private global::Gtk.Button StartToEndButton;
	private global::Gtk.Button ResetCurveButton;
	private global::Gtk.Label label3;
	private global::Gtk.Entry BrScaleEntry;
	private global::Gtk.Label label4;
	private global::Gtk.Button PreviewRenderButton;
	private global::Gtk.SpinButton PrevCountSpin;
	private global::Gtk.Label label5;
	private global::Gtk.SpinButton PrevIndexSpin;
	private global::Gtk.Label label1;
	private global::Gtk.Button SaveButton;
	private global::Gtk.ProgressBar ProgressBar;
	private global::Gtk.Label TimeLabel;
	private global::Gtk.Label InfoLabel;
	private global::Gtk.Button CancelButton;
	private global::Gtk.ComboBox FinishedDoBox;
	
	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget DeSERtMain
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.MenuAction = new global::Gtk.Action ("MenuAction", global::Mono.Unix.Catalog.GetString ("Menu"), null, null);
		this.MenuAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Menu");
		w1.Add (this.MenuAction, null);
		this.SettingsAction = new global::Gtk.Action ("SettingsAction", global::Mono.Unix.Catalog.GetString ("Settings"), null, null);
		this.SettingsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Settings");
		w1.Add (this.SettingsAction, null);
		this.AboutAction = new global::Gtk.Action ("AboutAction", global::Mono.Unix.Catalog.GetString ("About"), null, null);
		this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("About");
		w1.Add (this.AboutAction, null);
		this.HelpAction = new global::Gtk.Action ("HelpAction", global::Mono.Unix.Catalog.GetString ("Help"), null, null);
		this.HelpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Help");
		w1.Add (this.HelpAction, null);
		this.newAction = new global::Gtk.Action ("newAction", global::Mono.Unix.Catalog.GetString ("New Project"), null, "gtk-new");
		this.newAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("New Project");
		w1.Add (this.newAction, null);
		this.saveAction = new global::Gtk.Action ("saveAction", global::Mono.Unix.Catalog.GetString ("Save Project"), null, "gtk-save");
		this.saveAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Save Project");
		w1.Add (this.saveAction, null);
		this.saveAsAction = new global::Gtk.Action ("saveAsAction", global::Mono.Unix.Catalog.GetString ("Save Project To"), null, "gtk-save-as");
		this.saveAsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Save Project To");
		w1.Add (this.saveAsAction, null);
		this.openAction = new global::Gtk.Action ("openAction", global::Mono.Unix.Catalog.GetString ("Open Project"), null, "gtk-open");
		this.openAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Open Project");
		w1.Add (this.openAction, null);
		this.quitAction = new global::Gtk.Action ("quitAction", global::Mono.Unix.Catalog.GetString ("Quit"), null, "gtk-quit");
		this.quitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Quit");
		w1.Add (this.quitAction, null);
		this.helpAction = new global::Gtk.Action ("helpAction", global::Mono.Unix.Catalog.GetString ("Help"), null, "gtk-help");
		this.helpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Help");
		w1.Add (this.helpAction, null);
		this.aboutAction = new global::Gtk.Action ("aboutAction", global::Mono.Unix.Catalog.GetString ("_About"), null, "gtk-about");
		this.aboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_About");
		w1.Add (this.aboutAction, null);
		this.preferencesAction = new global::Gtk.Action ("preferencesAction", global::Mono.Unix.Catalog.GetString ("Settings"), null, "gtk-preferences");
		this.preferencesAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Settings");
		w1.Add (this.preferencesAction, null);
		this.ExtrasAction = new global::Gtk.Action ("ExtrasAction", global::Mono.Unix.Catalog.GetString ("Extras"), null, null);
		this.ExtrasAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Extras");
		w1.Add (this.ExtrasAction, null);
		this.CreateFilterAction = new global::Gtk.Action ("CreateFilterAction", global::Mono.Unix.Catalog.GetString ("Create Filter"), null, null);
		this.CreateFilterAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Add Filter");
		w1.Add (this.CreateFilterAction, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.WidthRequest = 910;
		this.HeightRequest = 520;
		this.Name = "DeSERtMain";
		this.Title = global::Mono.Unix.Catalog.GetString ("DeSERt");
		this.Icon = global::Gdk.Pixbuf.LoadFromResource ("DeSERt.DeSERt-Icon.ico");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.Resizable = false;
		this.AllowGrow = false;
		this.DefaultWidth = 900;
		this.DefaultHeight = 500;
		// Container child DeSERtMain.Gtk.Container+ContainerChild
		this.BG = new global::Gtk.Fixed ();
		this.BG.Name = "BG";
		this.BG.HasWindow = false;
		// Container child BG.Gtk.Fixed+FixedChild
		this.UIManager.AddUiFromString (@"<ui><menubar name='Menu'><menu name='MenuAction' action='MenuAction'><menuitem name='newAction' action='newAction'/><separator/><menuitem name='saveAction' action='saveAction'/><menuitem name='saveAsAction' action='saveAsAction'/><separator/><menuitem name='openAction' action='openAction'/><separator/><menuitem name='preferencesAction' action='preferencesAction'/><separator/><menuitem name='quitAction' action='quitAction'/></menu><menu name='ExtrasAction' action='ExtrasAction'><menuitem name='CreateFilterAction' action='CreateFilterAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='helpAction' action='helpAction'/><menuitem name='aboutAction' action='aboutAction'/></menu></menubar></ui>");
		this.Menu = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/Menu")));
		this.Menu.WidthRequest = 920;
		this.Menu.Name = "Menu";
		this.BG.Add (this.Menu);
		// Container child BG.Gtk.Fixed+FixedChild
		this.MainNotebook = new global::Gtk.Notebook ();
		this.MainNotebook.WidthRequest = 900;
		this.MainNotebook.HeightRequest = 450;
		this.MainNotebook.CanFocus = true;
		this.MainNotebook.Name = "MainNotebook";
		this.MainNotebook.CurrentPage = 1;
		// Container child MainNotebook.Gtk.Notebook+NotebookChild
		this.BGLoading = new global::Gtk.Fixed ();
		this.BGLoading.Name = "BGLoading";
		this.BGLoading.HasWindow = false;
		// Container child BGLoading.Gtk.Fixed+FixedChild
		this.ValueTableScroll = new global::Gtk.ScrolledWindow ();
		this.ValueTableScroll.WidthRequest = 650;
		this.ValueTableScroll.HeightRequest = 400;
		this.ValueTableScroll.Name = "ValueTableScroll";
		this.ValueTableScroll.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child ValueTableScroll.Gtk.Container+ContainerChild
		this.ValueTable = new global::Gtk.TreeView ();
		this.ValueTable.CanFocus = true;
		this.ValueTable.Name = "ValueTable";
		this.ValueTable.EnableSearch = false;
		this.ValueTableScroll.Add (this.ValueTable);
		this.BGLoading.Add (this.ValueTableScroll);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.BGLoading [this.ValueTableScroll]));
		w4.X = 235;
		w4.Y = 10;
		// Container child BGLoading.Gtk.Fixed+FixedChild
		this.ImageOpenButton = new global::Gtk.Button ();
		this.ImageOpenButton.WidthRequest = 150;
		this.ImageOpenButton.HeightRequest = 35;
		this.ImageOpenButton.CanFocus = true;
		this.ImageOpenButton.Name = "ImageOpenButton";
		this.ImageOpenButton.UseUnderline = true;
		// Container child ImageOpenButton.Gtk.Container+ContainerChild
		global::Gtk.Alignment w5 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		global::Gtk.HBox w6 = new global::Gtk.HBox ();
		w6.Spacing = 2;
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Image w7 = new global::Gtk.Image ();
		w7.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-open", global::Gtk.IconSize.Menu);
		w6.Add (w7);
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Label w9 = new global::Gtk.Label ();
		w9.LabelProp = global::Mono.Unix.Catalog.GetString ("Add Image(s)");
		w9.UseUnderline = true;
		w6.Add (w9);
		w5.Add (w6);
		this.ImageOpenButton.Add (w5);
		this.BGLoading.Add (this.ImageOpenButton);
		global::Gtk.Fixed.FixedChild w13 = ((global::Gtk.Fixed.FixedChild)(this.BGLoading [this.ImageOpenButton]));
		w13.X = 40;
		w13.Y = 30;
		// Container child BGLoading.Gtk.Fixed+FixedChild
		this.ProcOpenButton = new global::Gtk.Button ();
		this.ProcOpenButton.WidthRequest = 150;
		this.ProcOpenButton.HeightRequest = 35;
		this.ProcOpenButton.CanFocus = true;
		this.ProcOpenButton.Name = "ProcOpenButton";
		this.ProcOpenButton.UseUnderline = true;
		// Container child ProcOpenButton.Gtk.Container+ContainerChild
		global::Gtk.Alignment w14 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		global::Gtk.HBox w15 = new global::Gtk.HBox ();
		w15.Spacing = 2;
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Image w16 = new global::Gtk.Image ();
		w16.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-open", global::Gtk.IconSize.Menu);
		w15.Add (w16);
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Label w18 = new global::Gtk.Label ();
		w18.LabelProp = global::Mono.Unix.Catalog.GetString ("Open PP3");
		w18.UseUnderline = true;
		w15.Add (w18);
		w14.Add (w15);
		this.ProcOpenButton.Add (w14);
		this.BGLoading.Add (this.ProcOpenButton);
		global::Gtk.Fixed.FixedChild w22 = ((global::Gtk.Fixed.FixedChild)(this.BGLoading [this.ProcOpenButton]));
		w22.X = 40;
		w22.Y = 80;
		// Container child BGLoading.Gtk.Fixed+FixedChild
		this.CalculateButton = new global::Gtk.Button ();
		this.CalculateButton.WidthRequest = 180;
		this.CalculateButton.HeightRequest = 30;
		this.CalculateButton.CanFocus = true;
		this.CalculateButton.Name = "CalculateButton";
		this.CalculateButton.UseUnderline = true;
		// Container child CalculateButton.Gtk.Container+ContainerChild
		global::Gtk.Alignment w23 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		global::Gtk.HBox w24 = new global::Gtk.HBox ();
		w24.Spacing = 2;
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Image w25 = new global::Gtk.Image ();
		w25.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-execute", global::Gtk.IconSize.Menu);
		w24.Add (w25);
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Label w27 = new global::Gtk.Label ();
		w27.LabelProp = global::Mono.Unix.Catalog.GetString ("Calculate Brightness");
		w27.UseUnderline = true;
		w24.Add (w27);
		w23.Add (w24);
		this.CalculateButton.Add (w23);
		this.BGLoading.Add (this.CalculateButton);
		global::Gtk.Fixed.FixedChild w31 = ((global::Gtk.Fixed.FixedChild)(this.BGLoading [this.CalculateButton]));
		w31.X = 25;
		w31.Y = 325;
		// Container child BGLoading.Gtk.Fixed+FixedChild
		this.PreviewImg = new global::Gtk.Image ();
		this.PreviewImg.WidthRequest = 160;
		this.PreviewImg.HeightRequest = 120;
		this.PreviewImg.Name = "PreviewImg";
		this.BGLoading.Add (this.PreviewImg);
		global::Gtk.Fixed.FixedChild w32 = ((global::Gtk.Fixed.FixedChild)(this.BGLoading [this.PreviewImg]));
		w32.X = 35;
		w32.Y = 175;
		this.MainNotebook.Add (this.BGLoading);
		// Notebook tab
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Loading");
		this.MainNotebook.SetTabLabel (this.BGLoading, this.label2);
		this.label2.ShowAll ();
		// Container child MainNotebook.Gtk.Notebook+NotebookChild
		this.EditingBG = new global::Gtk.Fixed ();
		this.EditingBG.Name = "EditingBG";
		this.EditingBG.HasWindow = false;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.XValBox = new global::Gtk.Entry ();
		this.XValBox.WidthRequest = 60;
		this.XValBox.CanFocus = true;
		this.XValBox.Name = "XValBox";
		this.XValBox.Text = global::Mono.Unix.Catalog.GetString ("0");
		this.XValBox.IsEditable = true;
		this.XValBox.InvisibleChar = '?';
		this.EditingBG.Add (this.XValBox);
		global::Gtk.Fixed.FixedChild w34 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.XValBox]));
		w34.X = 110;
		w34.Y = 350;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.YValBox = new global::Gtk.Entry ();
		this.YValBox.WidthRequest = 60;
		this.YValBox.CanFocus = true;
		this.YValBox.Name = "YValBox";
		this.YValBox.Text = global::Mono.Unix.Catalog.GetString ("0");
		this.YValBox.IsEditable = true;
		this.YValBox.InvisibleChar = '?';
		this.EditingBG.Add (this.YValBox);
		global::Gtk.Fixed.FixedChild w35 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.YValBox]));
		w35.X = 110;
		w35.Y = 380;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.Ylabel = new global::Gtk.Label ();
		this.Ylabel.Name = "Ylabel";
		this.Ylabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Y:");
		this.EditingBG.Add (this.Ylabel);
		global::Gtk.Fixed.FixedChild w36 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.Ylabel]));
		w36.X = 90;
		w36.Y = 385;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.Xlabel = new global::Gtk.Label ();
		this.Xlabel.Name = "Xlabel";
		this.Xlabel.LabelProp = global::Mono.Unix.Catalog.GetString ("X:");
		this.EditingBG.Add (this.Xlabel);
		global::Gtk.Fixed.FixedChild w37 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.Xlabel]));
		w37.X = 90;
		w37.Y = 355;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.GraphEvents = new global::Gtk.EventBox ();
		this.GraphEvents.WidthRequest = 700;
		this.GraphEvents.HeightRequest = 400;
		this.GraphEvents.Name = "GraphEvents";
		// Container child GraphEvents.Gtk.Container+ContainerChild
		this.GraphArea = new global::Gtk.Image ();
		this.GraphArea.Name = "GraphArea";
		this.GraphEvents.Add (this.GraphArea);
		this.EditingBG.Add (this.GraphEvents);
		global::Gtk.Fixed.FixedChild w39 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.GraphEvents]));
		w39.X = 185;
		w39.Y = 10;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.CurveSelectBox = global::Gtk.ComboBox.NewText ();
		this.CurveSelectBox.WidthRequest = 150;
		this.CurveSelectBox.Name = "CurveSelectBox";
		this.EditingBG.Add (this.CurveSelectBox);
		global::Gtk.Fixed.FixedChild w40 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.CurveSelectBox]));
		w40.X = 15;
		w40.Y = 20;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.BrScale = new global::Gtk.HScale (null);
		this.BrScale.WidthRequest = 160;
		this.BrScale.CanFocus = true;
		this.BrScale.Name = "BrScale";
		this.BrScale.Adjustment.Lower = -300D;
		this.BrScale.Adjustment.Upper = 400D;
		this.BrScale.Adjustment.PageIncrement = 5D;
		this.BrScale.Adjustment.StepIncrement = 1D;
		this.BrScale.Adjustment.Value = 100D;
		this.BrScale.DrawValue = false;
		this.BrScale.Digits = 2;
		this.BrScale.ValuePos = ((global::Gtk.PositionType)(2));
		this.EditingBG.Add (this.BrScale);
		global::Gtk.Fixed.FixedChild w41 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.BrScale]));
		w41.X = 10;
		w41.Y = 85;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.EndToStartButton = new global::Gtk.Button ();
		this.EndToStartButton.WidthRequest = 75;
		this.EndToStartButton.HeightRequest = 20;
		this.EndToStartButton.CanFocus = true;
		this.EndToStartButton.Name = "EndToStartButton";
		this.EndToStartButton.UseUnderline = true;
		this.EndToStartButton.Label = global::Mono.Unix.Catalog.GetString ("End = Start");
		this.EditingBG.Add (this.EndToStartButton);
		global::Gtk.Fixed.FixedChild w42 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.EndToStartButton]));
		w42.X = 10;
		w42.Y = 140;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.StartToEndButton = new global::Gtk.Button ();
		this.StartToEndButton.WidthRequest = 75;
		this.StartToEndButton.HeightRequest = 20;
		this.StartToEndButton.CanFocus = true;
		this.StartToEndButton.Name = "StartToEndButton";
		this.StartToEndButton.UseUnderline = true;
		this.StartToEndButton.Label = global::Mono.Unix.Catalog.GetString ("Start = End");
		this.EditingBG.Add (this.StartToEndButton);
		global::Gtk.Fixed.FixedChild w43 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.StartToEndButton]));
		w43.X = 95;
		w43.Y = 140;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.ResetCurveButton = new global::Gtk.Button ();
		this.ResetCurveButton.WidthRequest = 75;
		this.ResetCurveButton.HeightRequest = 20;
		this.ResetCurveButton.CanFocus = true;
		this.ResetCurveButton.Name = "ResetCurveButton";
		this.ResetCurveButton.UseUnderline = true;
		this.ResetCurveButton.Label = global::Mono.Unix.Catalog.GetString ("Reset Curve");
		this.EditingBG.Add (this.ResetCurveButton);
		global::Gtk.Fixed.FixedChild w44 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.ResetCurveButton]));
		w44.X = 10;
		w44.Y = 170;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Brightness Scale:");
		this.EditingBG.Add (this.label3);
		global::Gtk.Fixed.FixedChild w45 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.label3]));
		w45.X = 5;
		w45.Y = 65;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.BrScaleEntry = new global::Gtk.Entry ();
		this.BrScaleEntry.WidthRequest = 60;
		this.BrScaleEntry.CanFocus = true;
		this.BrScaleEntry.Name = "BrScaleEntry";
		this.BrScaleEntry.Text = global::Mono.Unix.Catalog.GetString ("100,00");
		this.BrScaleEntry.IsEditable = true;
		this.BrScaleEntry.InvisibleChar = '?';
		this.EditingBG.Add (this.BrScaleEntry);
		global::Gtk.Fixed.FixedChild w46 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.BrScaleEntry]));
		w46.X = 105;
		w46.Y = 55;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.label4 = new global::Gtk.Label ();
		this.label4.Name = "label4";
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Startindex:");
		this.EditingBG.Add (this.label4);
		global::Gtk.Fixed.FixedChild w47 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.label4]));
		w47.X = 55;
		w47.Y = 255;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.PreviewRenderButton = new global::Gtk.Button ();
		this.PreviewRenderButton.WidthRequest = 160;
		this.PreviewRenderButton.CanFocus = true;
		this.PreviewRenderButton.Name = "PreviewRenderButton";
		this.PreviewRenderButton.UseUnderline = true;
		// Container child PreviewRenderButton.Gtk.Container+ContainerChild
		global::Gtk.Alignment w48 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		global::Gtk.HBox w49 = new global::Gtk.HBox ();
		w49.Spacing = 2;
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Image w50 = new global::Gtk.Image ();
		w50.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-refresh", global::Gtk.IconSize.Menu);
		w49.Add (w50);
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Label w52 = new global::Gtk.Label ();
		w52.LabelProp = global::Mono.Unix.Catalog.GetString ("Brightness Preview");
		w52.UseUnderline = true;
		w49.Add (w52);
		w48.Add (w49);
		this.PreviewRenderButton.Add (w48);
		this.EditingBG.Add (this.PreviewRenderButton);
		global::Gtk.Fixed.FixedChild w56 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.PreviewRenderButton]));
		w56.X = 10;
		w56.Y = 220;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.PrevCountSpin = new global::Gtk.SpinButton (2D, 100D, 1D);
		this.PrevCountSpin.CanFocus = true;
		this.PrevCountSpin.Name = "PrevCountSpin";
		this.PrevCountSpin.Adjustment.PageIncrement = 10D;
		this.PrevCountSpin.ClimbRate = 1D;
		this.PrevCountSpin.Numeric = true;
		this.PrevCountSpin.Value = 2D;
		this.EditingBG.Add (this.PrevCountSpin);
		global::Gtk.Fixed.FixedChild w57 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.PrevCountSpin]));
		w57.X = 120;
		w57.Y = 285;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.label5 = new global::Gtk.Label ();
		this.label5.Name = "label5";
		this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Count:");
		this.EditingBG.Add (this.label5);
		global::Gtk.Fixed.FixedChild w58 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.label5]));
		w58.X = 74;
		w58.Y = 290;
		// Container child EditingBG.Gtk.Fixed+FixedChild
		this.PrevIndexSpin = new global::Gtk.SpinButton (1D, 100D, 1D);
		this.PrevIndexSpin.CanFocus = true;
		this.PrevIndexSpin.Name = "PrevIndexSpin";
		this.PrevIndexSpin.Adjustment.PageIncrement = 10D;
		this.PrevIndexSpin.ClimbRate = 1D;
		this.PrevIndexSpin.Numeric = true;
		this.PrevIndexSpin.Value = 1D;
		this.EditingBG.Add (this.PrevIndexSpin);
		global::Gtk.Fixed.FixedChild w59 = ((global::Gtk.Fixed.FixedChild)(this.EditingBG [this.PrevIndexSpin]));
		w59.X = 120;
		w59.Y = 250;
		this.MainNotebook.Add (this.EditingBG);
		global::Gtk.Notebook.NotebookChild w60 = ((global::Gtk.Notebook.NotebookChild)(this.MainNotebook [this.EditingBG]));
		w60.Position = 1;
		// Notebook tab
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Editing");
		this.MainNotebook.SetTabLabel (this.EditingBG, this.label1);
		this.label1.ShowAll ();
		this.BG.Add (this.MainNotebook);
		global::Gtk.Fixed.FixedChild w61 = ((global::Gtk.Fixed.FixedChild)(this.BG [this.MainNotebook]));
		w61.Y = 25;
		// Container child BG.Gtk.Fixed+FixedChild
		this.SaveButton = new global::Gtk.Button ();
		this.SaveButton.WidthRequest = 100;
		this.SaveButton.HeightRequest = 30;
		this.SaveButton.CanFocus = true;
		this.SaveButton.Name = "SaveButton";
		this.SaveButton.UseUnderline = true;
		// Container child SaveButton.Gtk.Container+ContainerChild
		global::Gtk.Alignment w62 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		global::Gtk.HBox w63 = new global::Gtk.HBox ();
		w63.Spacing = 2;
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Image w64 = new global::Gtk.Image ();
		w64.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-apply", global::Gtk.IconSize.Menu);
		w63.Add (w64);
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Label w66 = new global::Gtk.Label ();
		w66.LabelProp = global::Mono.Unix.Catalog.GetString ("Save Images");
		w66.UseUnderline = true;
		w63.Add (w66);
		w62.Add (w63);
		this.SaveButton.Add (w62);
		this.BG.Add (this.SaveButton);
		global::Gtk.Fixed.FixedChild w70 = ((global::Gtk.Fixed.FixedChild)(this.BG [this.SaveButton]));
		w70.X = 795;
		w70.Y = 480;
		// Container child BG.Gtk.Fixed+FixedChild
		this.ProgressBar = new global::Gtk.ProgressBar ();
		this.ProgressBar.Name = "ProgressBar";
		this.BG.Add (this.ProgressBar);
		global::Gtk.Fixed.FixedChild w71 = ((global::Gtk.Fixed.FixedChild)(this.BG [this.ProgressBar]));
		w71.X = 10;
		w71.Y = 485;
		// Container child BG.Gtk.Fixed+FixedChild
		this.TimeLabel = new global::Gtk.Label ();
		this.TimeLabel.Name = "TimeLabel";
		this.TimeLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("0h 0m 0s left");
		this.BG.Add (this.TimeLabel);
		global::Gtk.Fixed.FixedChild w72 = ((global::Gtk.Fixed.FixedChild)(this.BG [this.TimeLabel]));
		w72.X = 179;
		w72.Y = 490;
		// Container child BG.Gtk.Fixed+FixedChild
		this.InfoLabel = new global::Gtk.Label ();
		this.InfoLabel.Name = "InfoLabel";
		this.InfoLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Ready to do work!");
		this.BG.Add (this.InfoLabel);
		global::Gtk.Fixed.FixedChild w73 = ((global::Gtk.Fixed.FixedChild)(this.BG [this.InfoLabel]));
		w73.X = 298;
		w73.Y = 490;
		// Container child BG.Gtk.Fixed+FixedChild
		this.CancelButton = new global::Gtk.Button ();
		this.CancelButton.WidthRequest = 100;
		this.CancelButton.HeightRequest = 30;
		this.CancelButton.CanFocus = true;
		this.CancelButton.Name = "CancelButton";
		this.CancelButton.UseUnderline = true;
		// Container child CancelButton.Gtk.Container+ContainerChild
		global::Gtk.Alignment w74 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		global::Gtk.HBox w75 = new global::Gtk.HBox ();
		w75.Spacing = 2;
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Image w76 = new global::Gtk.Image ();
		w76.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
		w75.Add (w76);
		// Container child GtkHBox.Gtk.Container+ContainerChild
		global::Gtk.Label w78 = new global::Gtk.Label ();
		w78.LabelProp = global::Mono.Unix.Catalog.GetString ("Cancel");
		w78.UseUnderline = true;
		w75.Add (w78);
		w74.Add (w75);
		this.CancelButton.Add (w74);
		this.BG.Add (this.CancelButton);
		global::Gtk.Fixed.FixedChild w82 = ((global::Gtk.Fixed.FixedChild)(this.BG [this.CancelButton]));
		w82.X = 685;
		w82.Y = 480;
		// Container child BG.Gtk.Fixed+FixedChild
		this.FinishedDoBox = global::Gtk.ComboBox.NewText ();
		this.FinishedDoBox.AppendText (global::Mono.Unix.Catalog.GetString ("Do Nothing"));
		this.FinishedDoBox.AppendText (global::Mono.Unix.Catalog.GetString ("Close DeSERt"));
		this.FinishedDoBox.AppendText (global::Mono.Unix.Catalog.GetString ("Suspend"));
		this.FinishedDoBox.AppendText (global::Mono.Unix.Catalog.GetString ("Shut Down"));
		this.FinishedDoBox.WidthRequest = 120;
		this.FinishedDoBox.HeightRequest = 25;
		this.FinishedDoBox.Name = "FinishedDoBox";
		this.FinishedDoBox.Active = 0;
		this.BG.Add (this.FinishedDoBox);
		global::Gtk.Fixed.FixedChild w83 = ((global::Gtk.Fixed.FixedChild)(this.BG [this.FinishedDoBox]));
		w83.X = 545;
		w83.Y = 485;
		this.Add (this.BG);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.ExposeEvent += new global::Gtk.ExposeEventHandler (this.OnExposeEvent);
		this.newAction.Activated += new global::System.EventHandler (this.OnNewActionActivated);
		this.saveAction.Activated += new global::System.EventHandler (this.OnSaveActionActivated);
		this.saveAsAction.Activated += new global::System.EventHandler (this.OnSaveAsActionActivated);
		this.openAction.Activated += new global::System.EventHandler (this.OnOpenActionActivated);
		this.quitAction.Activated += new global::System.EventHandler (this.OnQuitActionActivated);
		this.helpAction.Activated += new global::System.EventHandler (this.OnHelpActionActivated);
		this.aboutAction.Activated += new global::System.EventHandler (this.OnAboutActionActivated);
		this.preferencesAction.Activated += new global::System.EventHandler (this.OnPreferencesActionActivated);
		this.CreateFilterAction.Activated += new global::System.EventHandler (this.OnCreateFilterActionActivated);
		this.ValueTable.CursorChanged += new global::System.EventHandler (this.OnValueTableCursorChanged);
		this.ImageOpenButton.Clicked += new global::System.EventHandler (this.OnImageOpenButtonClicked);
		this.ProcOpenButton.Clicked += new global::System.EventHandler (this.OnProcOpenButtonClicked);
		this.CalculateButton.Clicked += new global::System.EventHandler (this.OnCalculateButtonClicked);
		this.XValBox.TextInserted += new global::Gtk.TextInsertedHandler (this.OnXValBoxTextInserted);
		this.YValBox.TextInserted += new global::Gtk.TextInsertedHandler (this.OnYValBoxTextInserted);
		this.GraphEvents.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnGraphEventsButtonPressEvent);
		this.GraphEvents.ButtonReleaseEvent += new global::Gtk.ButtonReleaseEventHandler (this.OnGraphEventsButtonReleaseEvent);
		this.CurveSelectBox.Changed += new global::System.EventHandler (this.OnCurveSelectBoxChanged);
		this.BrScale.ValueChanged += new global::System.EventHandler (this.OnBrScaleValueChanged);
		this.EndToStartButton.Clicked += new global::System.EventHandler (this.OnEndToStartButtonClicked);
		this.StartToEndButton.Clicked += new global::System.EventHandler (this.OnStartToEndButtonClicked);
		this.ResetCurveButton.Clicked += new global::System.EventHandler (this.OnResetCurveButtonClicked);
		this.BrScaleEntry.KeyPressEvent += new global::Gtk.KeyPressEventHandler (this.OnBrScaleEntryKeyPressEvent);
		this.PreviewRenderButton.Clicked += new global::System.EventHandler (this.OnPreviewRenderButtonClicked);
		this.PrevIndexSpin.Changed += new global::System.EventHandler (this.OnPrevIndexSpinChanged);
		this.SaveButton.Clicked += new global::System.EventHandler (this.OnSaveButtonClicked);
		this.CancelButton.Clicked += new global::System.EventHandler (this.OnCancelButtonClicked);
	}
}
