namespace OptOutAddIn
{
  partial class MyRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public MyRibbon()
      : base(Globals.Factory.GetRibbonFactory())
    {
      InitializeComponent();
    }

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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tab1 = this.Factory.CreateRibbonTab();
      this.grpOptOut = this.Factory.CreateRibbonGroup();
      this.btnOptOut = this.Factory.CreateRibbonButton();
      this.tab1.SuspendLayout();
      this.grpOptOut.SuspendLayout();
      // 
      // tab1
      // 
      this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
      this.tab1.ControlId.OfficeId = "TabMail";
      this.tab1.Groups.Add(this.grpOptOut);
      this.tab1.Label = "TabMail";
      this.tab1.Name = "tab1";
      // 
      // grpOptOut
      // 
      this.grpOptOut.Items.Add(this.btnOptOut);
      this.grpOptOut.Label = "ACI";
      this.grpOptOut.Name = "grpOptOut";
      // 
      //.btnOptOut
      // 
      this.btnOptOut.Description = "Generate SQL to process Opt-out emails";
      this.btnOptOut.Label = "OptOut SQL";
      this.btnOptOut.Name = "btnOptOut";
      this.btnOptOut.ScreenTip = "Generate SQL to process Opt-out emails";
      this.btnOptOut.ShowImage = true;
      this.btnOptOut.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.btnOptOut.OfficeImageId = "QueryDelete";
      this.btnOptOut.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnOptOut_Click);
      // 
      // Ribbon1
      // 
      this.Name = "Ribbon1";
      this.RibbonType = "Microsoft.Outlook.Explorer";
      this.Tabs.Add(this.tab1);
      this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MyRibbon_Load);
      this.tab1.ResumeLayout(false);
      this.tab1.PerformLayout();
      this.grpOptOut.ResumeLayout(false);
      this.grpOptOut.PerformLayout();

    }

    #endregion

    internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpOptOut;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton btnOptOut;
  }

  partial class ThisRibbonCollection
  {
    internal MyRibbon Ribbon1
    {
      get { return this.GetRibbon<MyRibbon>(); }
    }
  }
}
