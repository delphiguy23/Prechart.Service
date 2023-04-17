using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Prechart.Service.Globals.Helper;

public static class EmailHelper
{
    public static StringBuilder EmailBodyCreator(string header, string columns)
    {
        var body = new StringBuilder();
        var formatProvider = CultureInfo.InvariantCulture;
        var tableHeader = columns.Split(';').ToList();
        try
        {
            body.AppendLine(@"<html>");
            body.AppendLine(@"  <body>");
            body.AppendLine(@"     <br>");
            body.AppendLine(@"      <span style=""font-family:monospace;""><big><big><span style=""font-family:monospace;""><span style=""font-weight:bold;""> ");
            body.AppendLine(formatProvider, $@"{header}");
            body.AppendLine(@"      </span></span></big></big></span><br><br>");
            body.AppendLine(@"     <style type=""text/css"">");
            body.AppendLine(@"      table.tftable {font-size:12px;color:#333333;width:100%;border-width: 1px;border-color: #729ea5;border-collapse: collapse;}");
            body.AppendLine(@"      table.tftable th {font-family:monospace;font-size:12px;background-color:#acc8cc;border-width: 1px;padding: 8px;border-style: solid;border-color: #729ea5;text-align:left;} ");
            body.AppendLine(@"          table.tftable tr {background-color:#ffffff;} table.tftable td {font-family:monospace;font-size:12px;border-width: 1px;padding: 8px;border-style: solid;border-color: #729ea5;} ");
            body.AppendLine(@"  </style>");
            body.AppendLine(@"     <table id=""tfhover"" class=""tftable"" border=""1"">");
            body.AppendLine(@"        <tr>");

            foreach (var th in tableHeader)
            {
                body.AppendLine(formatProvider, $"<th>{th}</th>");
            }

            body.AppendLine(@"        </tr>");
            body.AppendLine(@"        <tr>");
            body.AppendLine(@"{__emailbody__}");
            body.AppendLine(@"        </tr>");

            body.AppendLine(@" </table>");
            body.AppendLine(@" </body>");
            body.AppendLine(@"</html>");

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            body.Clear();
        }

        return body;
    }

    public static bool IsValidEmail(string email) {
        var regex = new Regex(@"^[a-zA-Z0-9.a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9]+\.[a-zA-Z]+");
        var match = regex.Match(email); 
        return match.Success;
    }
}

