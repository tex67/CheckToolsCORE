using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckToolsCORE.Services
{
    public class CheckListService
    {
        public static Tuple<bool, string> IsValidCheckList(string dataDoc, string riferimento)
        {

            if ((string.IsNullOrEmpty(dataDoc) || string.IsNullOrWhiteSpace(dataDoc)))
            {
                return new Tuple<bool, string>(false, "Data documento non valorizzata");
            }
            else 
            {
                DateTime result;
                if (!DateTime.TryParse(dataDoc, out result))
                {
                    return new Tuple<bool, string>(false, "Data documento non valida");
                }
            }
            if ((string.IsNullOrEmpty(riferimento) || string.IsNullOrWhiteSpace(riferimento)))
            {
                return new Tuple<bool, string>(false, "Riferimento  non valorizzato");
            }
            return new Tuple<bool, string>(true, "");
        }
    }
}