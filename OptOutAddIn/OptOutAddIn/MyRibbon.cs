using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Tools.Ribbon;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;



namespace OptOutAddIn
{
  public partial class MyRibbon
  {
    private void MyRibbon_Load(object sender, RibbonUIEventArgs e)
    {

    }


    private void btnOptOut_Click(object sender, RibbonControlEventArgs e)
    {
      Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
      OptOutHelper.ProcessSelection(explorer.Selection);
    }
  }
}
