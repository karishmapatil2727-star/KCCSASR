using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Xml.Linq;

/// <summary>
/// Summary description for RadGridPayroll
/// </summary>
public class RadGridXML
{
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    GridBoundColumn column;
    
    public RadGridXML()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private class TableDefi
    {
        public string DataField { get; set; }
        public string HeaderText { get; set; }
        public string Aggregate { get; set; }               //XMlColumn for Aggregation like Sum/count/Min/Max
        public string FooterText { get; set; }              //XMlColumn for Aggregation Footer Text for toal of Aggregation Fields
        public string CustomHeaderText { get; set; }        //XMlColumn for If Datafield Not Exists in Table Defination
    }

    private GridAggregateFunction gridAggregateFunction(string FunctionName)
    {
        if (FunctionName == null){return GridAggregateFunction.None;}

        try
        {
            if (FunctionName.ToUpper() == GridAggregateFunction.Avg.ToString().ToUpper()) { return GridAggregateFunction.Avg; }
            if (FunctionName.ToUpper() == GridAggregateFunction.Count.ToString().ToUpper()) { return GridAggregateFunction.Count; }
            if (FunctionName.ToUpper() == GridAggregateFunction.CountDistinct.ToString().ToUpper()) { return GridAggregateFunction.CountDistinct; }
            if (FunctionName.ToUpper() == GridAggregateFunction.Custom.ToString().ToUpper()) { return GridAggregateFunction.Custom; }
            if (FunctionName.ToUpper() == GridAggregateFunction.First.ToString().ToUpper()) { return GridAggregateFunction.First; }
            if (FunctionName.ToUpper() == GridAggregateFunction.Last.ToString().ToUpper()) { return GridAggregateFunction.Last; }
            if (FunctionName.ToUpper() == GridAggregateFunction.Max.ToString().ToUpper()) { return GridAggregateFunction.Max; }
            if (FunctionName.ToUpper() == GridAggregateFunction.Min.ToString().ToUpper()) { return GridAggregateFunction.Min; }
            if (FunctionName.ToUpper() == GridAggregateFunction.Sum.ToString().ToUpper()) { return GridAggregateFunction.Sum; }

            return GridAggregateFunction.None;
        }

        catch
        {
            return GridAggregateFunction.None;
        }
    }

    /// <summary>
    /// RadGrid binds from XMl Fields
    /// </summary>
    /// <param name="grid">RadGrid</param>
    /// <param name="XMLFields">XML File eg: SalaryDetailUserWise</param>
    /// <param name="InstId"></param>
    /// <returns></returns>
    public RadGrid RadGridXMLBind(RadGrid grid, string XMLFields, int InstId)
    {
          grid.ShowHeader = true;
          grid.ShowFooter = true;
          grid.AutoGenerateColumns = false;
          grid.GridLines = GridLines.Both;  
          grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;

          grid.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
          grid.MasterTableView.CommandItemSettings.ShowExportToExcelButton = true;
          grid.MasterTableView.CommandItemSettings.ShowExportToWordButton = true;
          grid.MasterTableView.CommandItemSettings.ShowRefreshButton = false;

          grid.ClientSettings.AllowColumnsReorder = true;
          grid.ClientSettings.ReorderColumnsOnClient = true;
          grid.ClientSettings.EnableRowHoverStyle = true;
          grid.ClientSettings.AllowDragToGroup = true;
  
          // Excel Settings/// 
          Guid guid = Guid.NewGuid(); guid.ToString();
          grid.ExportSettings.ExportOnlyData = true;
          grid.ExportSettings.IgnorePaging = true;
          grid.ExportSettings.OpenInNewWindow = true;
          grid.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
          grid.ExportSettings.FileName = guid.ToString();


        string xmlPath = HttpContext.Current.Server.MapPath("~/XMLFilleRadGrid/").ToString();

        string definationPath = xmlPath + "/" + "Defination.xml";//+ InstId.ToString() 
        string xMlFilePath = xmlPath + "/" + XMLFields + ".xml";  //+ InstId.ToString() 

        XElement TableDefination = XElement.Load(definationPath);
        XElement file = XElement.Load(xMlFilePath);


        var root = (from c in file.Descendants("Header")
                    join td in TableDefination.Descendants("Header") on (string)c.Attribute("DataField") equals (string)td.Attribute("DataField") into abc
                    from td in abc.DefaultIfEmpty()
                    select new TableDefi
                    {
                        DataField = (string)td.Attribute("DataField"),
                        HeaderText = (string)td.Element("HeaderText"),
                        Aggregate = (string)c.Element("Aggregate"),
                        FooterText = (string)c.Element("FooterText"),
                        CustomHeaderText = (string)c.Element("CustomHeaderText"),
                    }).ToList();


        //DataSet ds1 = new DataSet();
        //DataSet ds2 = new DataSet();

        //ds1.ReadXml(definationPath);
        //ds2.ReadXml(xMlFilePath);

        //var root = (from c in ds2.Tables[0].AsEnumerable()
        //            join td in ds1.Tables[0].AsEnumerable() on c.Field<string>("DataField") equals td.Field<string>("DataField") into UP
        //          from q in UP.DefaultIfEmpty()
        //          select new TableDefi
        //            {
        //                //DataField = (string)td.Attribute("DataField"),
        //                //HeaderText = (string)td.Element("HeaderText"),
        //                //Aggregate = (string)c.Element("Aggregate"),
        //                //FooterText = (string)c.Element("FooterText"),
        //                //CustomHeaderText = (string)c.Element("CustomHeaderText"),


        //                //DataField =  td.Attribute("DataField"),
        //                //HeaderText = (string)td.Element("HeaderText"),
        //                //Aggregate = (string)c.Element("Aggregate"),
        //                //FooterText = (string)c.Element("FooterText"),
        //                //CustomHeaderText = (string)c.Element("CustomHeaderText"),

        //                DataField = c.Field<string>("DataField"),
        //                HeaderText = q.Field<string>("HeaderText"),
        //               // Aggregate = c.Field<string>("Aggregate"),
        //                //FooterText = c.Field<string>("FooterText"),
        //                CustomHeaderText = c.Field<string>("CustomHeaderText"),
        //            }).ToList();








        //for (int aa = 1; aa <= root.Count; aa++)
        //{   
        //    string DataField=root[aa - 1].DataField;
        //    string HeaderText=root[aa - 1].HeaderText;
        //    string Aggregate=root[aa - 1].Aggregate;
        //    string FooterText=root[aa - 1].FooterText;
        //    string CustomHeaderText = root[aa - 1].CustomHeaderText;

        //    column = new GridBoundColumn();
        //    column.DataField = DataField;

            
        //    if (CustomHeaderText != null) HeaderText = CustomHeaderText;// if DataField is not Exists in Table Defination, it will get from XML File
        //    column.HeaderText = HeaderText;


        //    /// **********Aggregation Function**********
        //    column.Aggregate= gridAggregateFunction(Aggregate);

        //    // Functions is Not Null But Footer Title Not Mentioned
        //    if (column.Aggregate.ToString() != "None" && FooterText == null)
        //    {
        //        column.FooterText = "Total " + HeaderText;
        //    }
        //    else
        //    {
        //        column.FooterText = FooterText +" ";
        //    }



        //    /// **********Custom HeaderText**********

        //    grid.Columns.Add(column);
        //}





        if (grid.Columns.Count>=root.Count)
        {
            return grid;
        }


        for (int aa = 1; aa <= root.Count; aa++)
        {
            column = new GridBoundColumn();
            grid.Columns.Add(column);
            
            
            string DataField = root[aa - 1].DataField;
            string HeaderText = root[aa - 1].HeaderText;
            string Aggregate = root[aa - 1].Aggregate;
            string FooterText = root[aa - 1].FooterText;
            string CustomHeaderText = root[aa - 1].CustomHeaderText;

            
            
            
            column.DataField = DataField;
            
            if (CustomHeaderText != null) HeaderText = CustomHeaderText;// if DataField is not Exists in Table Defination, it will get from XML File
            column.HeaderText = HeaderText;


            /// **********Aggregation Function**********
            column.Aggregate = gridAggregateFunction(Aggregate);

            // Functions is Not Null But Footer Title Not Mentioned
            if (column.Aggregate.ToString() != "None" && FooterText == null)
            {
                column.FooterText = "Total " + HeaderText;
            }
            else
            {
                column.FooterText = FooterText + " ";
            }



            /// **********Custom HeaderText**********

            
        }



        return grid;
    }
}