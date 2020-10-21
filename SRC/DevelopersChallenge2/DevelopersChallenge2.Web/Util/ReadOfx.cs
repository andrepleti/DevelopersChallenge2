using DevelopersChallenge2.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DevelopersChallenge2.Web.Util
{
    public static class ReadOfx
    {
        /// <summary>
        /// Convert OFX file in List<TransactionBank>
        /// </summary>
        public static List<TransactionBank> toXElement(string pathToOfxFile)
        {
            var tags = from line in File.ReadAllLines(pathToOfxFile)
                       where line.Contains("<STMTTRN>") ||
                       line.Contains("<TRNTYPE>") ||
                       line.Contains("<DTPOSTED>") ||
                       line.Contains("<TRNAMT>") ||
                       line.Contains("<FITID>") ||
                       line.Contains("<CHECKNUM>") ||
                       line.Contains("<MEMO>")
                       select line;


            List<TransactionBank> listResult = new List<TransactionBank>();
            TransactionBank obj = new TransactionBank();
            foreach (var l in tags)
            {
                if (l.IndexOf("<STMTTRN>") != -1)
                {
                    if (!string.IsNullOrEmpty(obj.Description) && 
                        !listResult.Any(x => x.DateOperation == obj.DateOperation && x.Value == obj.Value && x.Description == obj.Description))
                    {
                        obj.Reconciliation = false;
                        listResult.Add(obj);
                    }

                    obj = new TransactionBank();

                    continue;
                }
                else if (l.IndexOf("<TRNTYPE>") != -1)
                {
                    if (TypeOperation.CREDIT.ToString() == l.Replace("<TRNTYPE>", "").Trim())
                        obj.TypeOperation = (int)TypeOperation.CREDIT;
                    else if (TypeOperation.DEBIT.ToString() == l.Replace("<TRNTYPE>", "").Trim())
                        obj.TypeOperation = (int)TypeOperation.DEBIT;

                    continue;
                }
                else if (l.IndexOf("<DTPOSTED>") != -1)
                {
                    string date = l.Replace("<DTPOSTED>", "").Substring(0, 8);

                    DateTime dateOperation = DateTime.ParseExact(date, "yyyyMMdd",
                    CultureInfo.InvariantCulture);

                    obj.DateOperation = dateOperation;

                    continue;
                }
                else if (l.IndexOf("<TRNAMT>") != -1)
                {
                    decimal.TryParse(l.Replace("<TRNAMT>", ""), out decimal resultDecimal);

                    obj.Value = resultDecimal;

                    continue;
                }
                else if (l.IndexOf("<MEMO>") != -1)
                {
                    obj.Description = l.Replace("<MEMO>", "");

                    continue;
                }
            }
            return listResult;

        }
    }
}