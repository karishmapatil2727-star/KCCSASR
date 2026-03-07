using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using Telerik.Web.UI.Grid;
using System.Data;
using System.Xml.Linq;

/// <summary>
/// Summary description for RadGridPayroll
/// </summary>
public class RadGridPayroll
{
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    GridBoundColumn column;
    
    public RadGridPayroll()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public enum PayrollGridsEnum
    {
        Summary, DayWiseList
    }

    private class TableDefi
    {
        public string DataField { get; set; }
        public string HeaderText { get; set; }
        public string Aggregate { get; set; }               //XMlColumn for Aggregation like Sum/count/Min/Max
        public string FooterText { get; set; }              //XMlColumn for Aggregation Footer Text for toal of Aggregation Fields
        public string CustomHeaderText { get; set; }        //XMlColumn for If Datafield Not Exists in Table Defination
    }

    //private RadGrid ReadXML(RadGrid grid, PayrollGridsEnum GridType, int InstId)
    //{
    //    string xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/").ToString();

    //    XElement TableDefination = XElement.Load(xmlPath+"/" + InstId.ToString() + "Defination.xml");
    //    XElement file = XElement.Load(xmlPath + "/" + InstId.ToString() + "XMLFile.xml");

    //    List<TableDefi> root = (from td in TableDefination.Descendants("Header")

    //               join c in file.Descendants("Header") on

    //           (string)td.Attribute("DataField") equals (string)c.Attribute("DataField")

    //       select  new TableDefi  
    //       {
    //           DataField = (string)td.Attribute("DataField"),
    //           HeaderText = (string)td.Element("HeaderText"),
    //           Aggregate = (string)td.Element("Aggregate"),
    //       }).ToList();




    //    for (int aa = 1; aa <= root.Count; aa++)
    //    {
    //        column = new GridBoundColumn();
    //        column.DataField = root[aa-1].DataField;
    //        column.HeaderText = root[aa-1].HeaderText;
    //        column.Aggregate = gridAggregateFunction(root[aa - 1].Aggregate);
    //        grid.Columns.Add(column);
    //    }

    //    return grid;
        
    //    //if (GridType == PayrollGridsEnum.Summary)
    //    //{
    //    //    xmlPath =xmlPath+"/XMLFile" + InstId.ToString() + ".xml";
    //    //}

    //    //ds.ReadXml(xmlPath);
    //    //return ds;
    //}

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

    public RadGrid BindGrid(RadGrid grid, string XMLColumnFile, int InstId)
    {
          grid.ShowHeader = true;
          grid.ShowFooter = true;
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

          XElement TableDefination = XElement.Load(xmlPath + "/" + "Defination.xml");  //+ InstId.ToString()
          XElement file = XElement.Load(xmlPath + "/" + XMLColumnFile + ".xml");  // + InstId.ToString() 

        List<TableDefi> root = (from c in file.Descendants("Header")
                                join td in TableDefination.Descendants("Header") on

                            (string)c.Attribute("DataField") equals (string)td.Attribute("DataField") 

                                select new TableDefi
                                {
                                    DataField = (string)td.Attribute("DataField"),
                                    HeaderText = (string)td.Element("HeaderText"),
                                    Aggregate = (string)c.Element("Aggregate"),
                                    FooterText = (string)c.Element("FooterText"),
                                    CustomHeaderText = (string)c.Element("CustomHeaderText"),
                                }).ToList();


        if (grid.Columns.Count > root.Count)
        {
            return grid;
        }

        for (int aa = 1; aa <= root.Count; aa++)
        {
            column = new GridBoundColumn();
            grid.Columns.Add(column);

            
            string DataField=root[aa - 1].DataField;
            string HeaderText=root[aa - 1].HeaderText;
            string Aggregate=root[aa - 1].Aggregate;
            string FooterText=root[aa - 1].FooterText;
            string CustomHeaderText = root[aa - 1].CustomHeaderText;

            
            column.DataField = DataField;

            
            if (CustomHeaderText != null) HeaderText = CustomHeaderText;// if DataField is not Exists in Table Defination, it will get from XML File
            column.HeaderText = HeaderText;


            /// **********Aggregation Function**********
            column.Aggregate= gridAggregateFunction(Aggregate);

            // Functions is Not Null But Footer Title Not Mentioned
            if (column.Aggregate.ToString() != "None" && FooterText == null)
            {
                column.FooterText = "Total " + HeaderText;
            }
            else
            {
                column.FooterText = FooterText +" ";
            }



            /// **********Custom HeaderText**********

            
        }






        //if (GridType == PayrollGridsEnum.Summary)
        //{
        //   //grid= BindXmlFile(ReadXML(GridType, InstId), grid);

        //    return grid = ReadXML(grid, GridType, InstId);
        //    //grid=newBindXmlFile(ReadXML(GridType,InstId),grid);
        //}
        return grid;
    }


   //private RadGrid newBindXmlFile (TableDefi td, RadGrid grid)
   //{
   //    foreach (DataRow dr in ds.Tables[0].Rows)
   //    {
   //        column = new GridBoundColumn();
   //        column.HeaderText = dr["HeaderText"].ToString();
   //        column.DataField = dr["DataField"].ToString();
   //        grid.Columns.Add(column);
   //    }
   //    return grid;
   //}


   // private RadGrid BindXmlFile(DataSet ds,  RadGrid grid)
   // {
   //     foreach (DataRow dr in ds.Tables[0].Rows)
   //     {
   //         column = new GridBoundColumn();
   //         column.HeaderText = dr["HeaderText"].ToString();
   //         column.DataField = dr["DataField"].ToString();
   //         grid.Columns.Add(column);
   //     }
   //     return grid;
   // }


}