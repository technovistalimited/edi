﻿using AIMS_BD_IATI.DAL;
using AIMS_BD_IATI.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AIMS_BD_IATI.Web.Controllers
{
    [RoutePrefix("api/ApiHome")]
    public class ApiHomeController : ApiController
    {
        static List<IatiProject> iatiProjects = new List<IatiProject>
        { 
            new IatiProject { title = "Tomato Soup", description = "Groceries"}, 
            new IatiProject { title = "Yo-yo", description = "Toys" }, 
            new IatiProject { title = "Hammer", description = "Dhaka, BD" } 
        };

        static List<AimsProject> aimsProjects = new List<AimsProject>
        { 
            new AimsProject { title = "SoupTomatoooooooooo ", description = "Groceries", matchedProjects = new List<IatiProject>()}, 
            new AimsProject { title = "-yoYo", description = "Toys", matchedProjects = new List<IatiProject>() }, 
            new AimsProject { title = "merHam", description = "Dhaka, BD", matchedProjects = new List<IatiProject>() } 
        };


        RootObject DataModel = new RootObject() { iatiProjects = iatiProjects, aimsProjects = aimsProjects };

        public object GetProjectHierarchyData(string dp)
        {

            return Ok(DataModel);
        }

        //[HttpGet]
        public RootObject GetData()
        {
            return DataModel;
        }
        public List<DropdownItem> GetFundSources()
        {
            return new AimsDAL().getFundSourcesDropdownData();
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostData(RootObject dataModel)
        {
            return Ok(DataModel);
        }

        //[HttpPut]
        //public async Task<IHttpActionResult> PutEmployee(EmployeeModel employee)
        //{
        //    employee.Id = 22;
        //    emp.Add(employee);
        //    return Ok(employee);
        //}
    }

    public class IatiProject
    {
        public string title { get; set; }
        public string description { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class AimsProject
    {
        public string title { get; set; }
        public string description { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public IList<IatiProject> matchedProjects { get; set; }
    }

    public class RootObject
    {
        public object selected { get; set; }
        public IList<IatiProject> iatiProjects { get; set; }
        public IList<AimsProject> aimsProjects { get; set; }
    }

}

