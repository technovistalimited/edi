﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AIMS_BD_IATI.Library;
using AIMS_BD_IATI.Library.Parser;
using AIMS_BD_IATI.Library.Parser.ParserIATIv2;
using AIMS_BD_IATI.Library.Parser.ParserIATIv1;

namespace AIMS_BD_IATI.Web.Controllers
{
    [RoutePrefix("api/ApiIatiConverter")]
    public class ApiIatiConverterController : ApiController
    {
        /// <summary>
        /// Convert Data from v1.05 to v2.02
        /// </summary>
        /// <param name="activitiesURL"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        public XmlResultv2 ConvertIATI(string org, string country)
        {
            string activitiesURL;
            IParserIATI parserIATI;
            XmlResultv2 returnResult2 = null;
            XmlResultv1 returnResult1 = null;

            try
            {
                activitiesURL = "http://datastore.iatistandard.org/api/1/access/activity.xml?recipient-country=" + country + "&reporting-org=" + org + "&stream=True";
                //Parser v2.01
                parserIATI = new ParserIATIv2();

                returnResult2 = (XmlResultv2)parserIATI.ParseIATIXML(activitiesURL);

                var iatiactivityArray = returnResult2.n().iatiactivities.n().iatiactivity;
                if (iatiactivityArray != null && iatiactivityArray.n()[0].AnyAttr.n()[0].Value == "1.05")
                {
                    //Parser v1.05
                    parserIATI = new ParserIATIv1();
                    returnResult1 = (XmlResultv1)parserIATI.ParseIATIXML(activitiesURL);

                    //Conversion
                    ConvertIATIv2 convertIATIv2 = new ConvertIATIv2();
                    returnResult2 = convertIATIv2.ConvertIATI105to201XML(returnResult1, returnResult2);
                }
            }
            catch (Exception ex)
            {
                returnResult2.n().Value = ex.Message;
            }

            return returnResult2;
            //return Newtonsoft.Json.JsonConvert.SerializeObject(returnResult2);
        }

        public List<AIMS_BD_IATI.Library.Parser.ParserIATIv2.iatiactivity> ConvertAIMStoIATI(string org)
        {
            List<AIMS_BD_IATI.Library.Parser.ParserIATIv2.iatiactivity> iatiactivityList = new List<Library.Parser.ParserIATIv2.iatiactivity>();
            try
            {
                iatiactivityList = new AIMS_BD_IATI.DAL.AimsDAL().getAIMSDataInIATIFormat(org);
            }
            catch (Exception)
            {
                
            }
            return iatiactivityList;
        }
        
    }
}
