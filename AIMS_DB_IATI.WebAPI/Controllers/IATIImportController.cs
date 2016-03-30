﻿using AIMS_BD_IATI.DAL;
using AIMS_BD_IATI.Library;
using AIMS_BD_IATI.Library.Parser.ParserIATIv2;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MoreLinq;
using AIMS_DB_IATI.WebAPI.Models.IATIImport;
using AIMS_DB_IATI.WebAPI.Models;

namespace AIMS_BD_IATI.WebAPI.Controllers
{
    [RoutePrefix("api/IATIImport")]
    [Authorize]
    public class IATIImportController : ApiController
    {
        void SetStatics()
        {
            iatiactivity.FundSources = Sessions.FundSources;
        }
        [HttpGet]
        public List<DPLookupItem> GetFundSources()
        {
            Sessions.FundSources = new AimsDAL().GetAllFundSources();
            return new AimsDAL().GetFundSources(Sessions.UserId);
        }

        [HttpGet]
        public List<FundSourceLookupItem> GetAllFundSources()
        {
            return Sessions.FundSources;
        }


        [AcceptVerbs("GET", "POST")]
        public HeirarchyModel GetHierarchyData(DPLookupItem dp)
        {
            bool isDPChanged = Sessions.activitiesContainer.n().DP != dp.n().ID;

            if (isDPChanged)
            {
                Sessions.heirarchyModel = new HeirarchyModel();

                Sessions.activitiesContainer = new AimsDbIatiDAL().GetActivities(dp.ID);

                if (Sessions.activitiesContainer.HasRelatedActivity)
                {
                    var H1Acts = Sessions.activitiesContainer.iatiActivities.FindAll(f => f.hierarchy == 1);
                    var H2Acts = Sessions.activitiesContainer.iatiActivities.FindAll(f => f.hierarchy == 2);

                    var AimsProjects = Sessions.activitiesContainer.AimsProjects;

                    var matchedH1 = (decimal)(from a in AimsProjects
                                              join i in H1Acts on a.IatiIdentifier equals i.IatiIdentifier
                                              select a).Count();

                    var matchedH2 = (decimal)(from a in AimsProjects
                                              join i in H2Acts on a.IatiIdentifier equals i.IatiIdentifier
                                              select a).Count();


                    Sessions.heirarchyModel.H1Percent = H1Acts.Count > 0 ? Math.Round((decimal)(matchedH1 / H1Acts.Count) * 100, 2) : 0;
                    Sessions.heirarchyModel.H2Percent = H2Acts.Count > 0 ? Math.Round((decimal)(matchedH2 / H2Acts.Count) * 100, 2) : 0;



                    #region Populate relatedActivities of the first activity as sample data
                    var parentActivities = Sessions.activitiesContainer.iatiActivities.FindAll(x => x.hierarchy == 1);
                    foreach (var pa in parentActivities)
                    {
                        if (pa.relatedactivity != null)
                        {
                            foreach (var ra in pa.relatedactivity.Where(r => r.type == "2"))
                            {
                                //load related activities
                                var ha = Sessions.activitiesContainer.iatiActivities.Find(f => f.iatiidentifier.Value == ra.@ref);

                                if (ha != null)
                                {
                                    pa.relatedIatiActivities.Add(ha);
                                }
                            }
                            Sessions.heirarchyModel.SampleIatiActivity = pa;
                            break; //we have to show only one hierarchycal project as a sample
                        }
                    }
                    #endregion
                    Sessions.heirarchyModel.SelectedHierarchy = 1;
                }
                else
                {
                    Sessions.heirarchyModel = null;
                }
            }
            return Sessions.heirarchyModel;
        }

        [HttpPost]
        public FilterBDModel SubmitHierarchy(HeirarchyModel heirarchyModel)
        {
            var returnResult = new FilterBDModel();

            if (heirarchyModel == null)
            {
                returnResult.iatiActivities = Sessions.activitiesContainer.iatiActivities;
            }
            else
            {
                returnResult.iatiActivities = Sessions.activitiesContainer.iatiActivities.FindAll(f => f.n().hierarchy == heirarchyModel.n().SelectedHierarchy);

                if (heirarchyModel.SelectedHierarchy == 1)
                {
                    foreach (var pa in returnResult.iatiActivities)
                    {
                        pa.relatedIatiActivities.Clear();
                        if (pa.relatedactivity != null)
                        {
                            foreach (var ra in pa.relatedactivity.Where(r => r.type == "2"))
                            {
                                //load related activities
                                var ha = Sessions.activitiesContainer.iatiActivities.Find(f => f.iatiidentifier.Value == ra.@ref);

                                if (ha != null)
                                {
                                    pa.relatedIatiActivities.Add(ha);
                                }
                            }
                        }
                    }
                }

            }

            returnResult.iatiActivities = returnResult.iatiActivities.OrderByDescending(k => k.IsRelevant).ToList();

            return returnResult;
        }

        [HttpPost]
        public object GetAllImplementingOrg(FilterBDModel filterDBModel)
        {
            if (filterDBModel != null)
                Sessions.activitiesContainer.iatiActivities = filterDBModel.iatiActivities;

            var managingDPs = GetAllFundSources();

            var iOrgs = new List<participatingorg>();
            foreach (var activity in Sessions.activitiesContainer.RelevantActivities)
            {
                var h1Acts = activity.participatingorg.n().Where(w => w.role == "4").ToList();
                if (h1Acts.Count > 0)
                {
                    iOrgs.AddRange(h1Acts);
                }
                else
                {
                    participatingorg dominatingParticipatingorg = null;
                    decimal highestCommitment = 0;
                    foreach (var relatedActivity in activity.relatedIatiActivities) // for h2Acts
                    {
                        var h2Acts = relatedActivity.participatingorg.n().Where(w => w.role == "4").ToList();
                        iOrgs.AddRange(h2Acts);

                        //getting dominating participating org
                        var tc = relatedActivity.TotalCommitment;
                        if (tc > highestCommitment)
                        {
                            highestCommitment = tc;
                            dominatingParticipatingorg = h2Acts.FirstOrDefault();
                        }
                    }

                    //set dominating participating org to h1activity
                    if (dominatingParticipatingorg != null)
                    {
                        List<participatingorg> participatingorgs = activity.participatingorg.n().ToList();
                        participatingorgs.Add(dominatingParticipatingorg);
                        activity.participatingorg = participatingorgs.ToArray();
                    }

                }
            }

            var distictOrgs = iOrgs.DistinctBy(l => l.narrative.n(0).Value).OrderBy(o => o.narrative.n(0).Value);

            foreach (var org in distictOrgs)
            {
                //check for matching managing DP from AIMS
                var managingDP = !string.IsNullOrWhiteSpace(org.@ref) ? managingDPs.FirstOrDefault(q => q.IATICode != null && q.IATICode.n().Contains(org.@ref)) : null;

                //if not found, set to Current DP
                if (managingDP == null)
                    managingDP = managingDPs.FirstOrDefault(q => q.IATICode != null && q.IATICode.n().Contains(Sessions.activitiesContainer.DP));

                //Add selected value
                org.FundSourceIDnIATICode = managingDP == null ? "" : managingDP.IDnIATICode;
            }

            return new
            {
                Orgs = distictOrgs,
                FundSources = managingDPs
            };
        }

        [HttpPost]
        public List<iatiactivity> FilterDP(List<participatingorg> _iOrgs)
        {

            var iOrgs = new List<participatingorg>();
            Sessions.activitiesContainer.RelevantActivities.ForEach(e => iOrgs.AddRange(e.participatingorg.n().Where(w => w.role == "4").ToList()));

            foreach (var iOrg in _iOrgs)
            {
                iOrgs.FindAll(f => f.@ref == iOrg.@ref).ForEach(e => e.FundSourceIDnIATICode = iOrg.FundSourceIDnIATICode);
            }

            return Sessions.activitiesContainer.RelevantActivities;
        }

        [AcceptVerbs("GET", "POST")]
        public int? UpdateActivity(List<iatiactivity> activities)
        {
            return new AimsDbIatiDAL().AssignActivities(activities);
        }

        [AcceptVerbs("GET", "POST")]
        public ProjectMapModel SubmitActivities(List<iatiactivity> relevantActivies)
        {
            if (relevantActivies == null)
                relevantActivies = Sessions.activitiesContainer.RelevantActivities;

            Sessions.ExchangeRates = new AimsDAL().GetExchangesRateToUSD(relevantActivies.n(1).defaultcurrency);
            SetStatics();//since we have no access to session at library project, so we pass it in a static variables

            var ProjectsOwnedByOther = relevantActivies.FindAll(f => f.IATICode != Sessions.activitiesContainer.DP);

            relevantActivies.RemoveAll(f => f.IATICode != Sessions.activitiesContainer.DP);

            var AimsProjects = new AimsDAL().GetAIMSDataInIATIFormat(Sessions.activitiesContainer.n().DP);

            var MatchedProjects = (from i in relevantActivies
                                   from a in AimsProjects.Where(k => i.iatiidentifier.Value.Replace("-", "").EndsWith(k.iatiidentifier.Value.Replace("-", "")))
                                   orderby i.iatiidentifier.Value

                                   select i).ToList();

            //for showing mathced projects side by side And field mapping later
            var MatchedProjects2 = (from i in relevantActivies
                                    from a in AimsProjects.Where(k => i.iatiidentifier.Value.Replace("-", "").EndsWith(k.iatiidentifier.Value.Replace("-", "")))
                                    orderby i.iatiidentifier.Value
                                    select new ProjectFieldMapModel(i, a)
                                    ).ToList();

            var IatiActivityNotInAims = relevantActivies.Except(MatchedProjects).ToList();


            var AimsProjectNotInIati = AimsProjects.ExceptBy(MatchedProjects, f => f.iatiidentifier.Value).ToList();

            Sessions.ProjectMapModel = new ProjectMapModel
             {
                 MatchedProjects = MatchedProjects2,
                 IatiActivitiesNotInAims = IatiActivityNotInAims,
                 AimsProjectsNotInIati = AimsProjectNotInIati,
                 NewProjectsToAddInAims = new List<iatiactivity>(),
                 ProjectsOwnedByOther = ProjectsOwnedByOther
             };
            return Sessions.ProjectMapModel;
        }
        [AcceptVerbs("GET", "POST")]
        public bool SubmitManualMatching(List<iatiactivity> projects)
        {
            Sessions.ProjectMapModel.AimsProjectsNotInIati = projects;

            Sessions.ProjectMapModel.MatchedProjects.RemoveAll(r => r.IsManuallyMapped);

            //add manually matched projects
            foreach (var project in Sessions.ProjectMapModel.AimsProjectsNotInIati)
            {
                if (project.MatchedProjects.Count > 0)
                {
                    Sessions.ProjectMapModel.MatchedProjects.Add(new ProjectFieldMapModel(project.MatchedProjects.First(), project) { IsManuallyMapped = true });
                }
            }


            return true;
        }
        [HttpGet]
        public ProjectFieldMapModel GetGeneralPreferences()
        {
            var savedPreferences = new AimsDbIatiDAL().GetFieldMappingPreferenceGeneral(Sessions.activitiesContainer.DP);



            var returnModel = (from a in Sessions.ProjectMapModel.n().MatchedProjects.n()
                               select new ProjectFieldMapModel(a.iatiActivity, a.aimsProject, savedPreferences)).FirstOrDefault();

            if (returnModel == null)
            {
                //List<FieldMap> flds = new List<FieldMap>();
                //foreach (var item in savedPreferences)
                //{
                //    FieldMap fld = new FieldMap
                //    {
                //        Field = item.FieldName,
                //        IsSourceIATI = item.IsSourceIATI,
                //        AIMSValue = item.FieldName,
                //        IATIValue = item.FieldName
                //    };
                //    flds.Add(fld);
                //}
                returnModel = new ProjectFieldMapModel(new iatiactivity(), new iatiactivity(), savedPreferences);
            }
            Sessions.GeneralPreferences = returnModel;

            return returnModel;
        }

        [AcceptVerbs("GET", "POST")]
        public int? SaveGeneralPreferences(ProjectFieldMapModel generalPreferences)
        {
            if (generalPreferences == null) return null;
            Sessions.GeneralPreferences = generalPreferences;

            List<FieldMappingPreferenceGeneral> entities = new List<FieldMappingPreferenceGeneral>();

            foreach (var fieldMap in generalPreferences.Fields)
            {
                var entity = new FieldMappingPreferenceGeneral
                {
                    FieldName = fieldMap.Field,
                    OrgId = Sessions.activitiesContainer.DP,
                    IsSourceIATI = fieldMap.IsSourceIATI
                };


                entities.Add(entity);
            }

            return new AimsDbIatiDAL().SaveFieldMappingPreferenceGeneral(entities);
        }

        [AcceptVerbs("GET", "POST")]
        public int? SaveActivityPreferences(ProjectFieldMapModel activityPreferences)
        {
            if (activityPreferences == null) return null;

            List<FieldMappingPreferenceActivity> entities = new List<FieldMappingPreferenceActivity>();

            foreach (var fieldMap in activityPreferences.Fields)
            {
                var entity = new FieldMappingPreferenceActivity
                {
                    FieldName = fieldMap.Field,
                    IATIIdentifier = activityPreferences.iatiActivity.IatiIdentifier,
                    //ProjectId = activityPreferences.aimsProject.ProjectId,
                    IsSourceIATI = fieldMap.IsSourceIATI
                };


                entities.Add(entity);
            }

            return new AimsDbIatiDAL().SaveFieldMappingPreferenceActivity(entities);
        }

        [HttpPost]
        public ProjectMapModel GetProjectsToMap(ProjectFieldMapModel GeneralPreference)
        {
            if (GeneralPreference != null)
                Sessions.GeneralPreferences = GeneralPreference;

            //set general or activity preferences
            foreach (var mapModel in Sessions.ProjectMapModel.MatchedProjects)
            {

                var activityPreference = new AimsDbIatiDAL().GetFieldMappingPreferenceActivity(mapModel.iatiActivity.IatiIdentifier);
                foreach (var field in mapModel.Fields)
                {
                    //get GetFieldMappingPreferenceActivity for this field
                    var activityFieldSource = activityPreference.Find(f => f.FieldName == field.Field);
                    if (activityFieldSource != null)
                    {
                        field.IsSourceIATI = activityFieldSource.IsSourceIATI;
                    }
                    else // apply general preferences
                    {
                        var generalFieldSource = Sessions.GeneralPreferences.Fields.Find(f => f.Field == field.Field);
                        if (generalFieldSource != null)
                            field.IsSourceIATI = generalFieldSource.IsSourceIATI;

                    }
                }

            }


            return new ProjectMapModel
            {
                MatchedProjects = Sessions.ProjectMapModel.MatchedProjects,
                IatiActivitiesNotInAims = null,
                AimsProjectsNotInIati = null,
                NewProjectsToAddInAims = null,
                ProjectsOwnedByOther = null
            };
        }

        [HttpGet]
        public object GetAssignedActivities(string dp)
        {
            //var projects = new AimsDAL().GetProjects(dp);
            Sessions.SubmitAssignedActivities = new AimsDAL().GetAIMSDataInIATIFormat(dp);
            var assignedActivities = new AimsDbIatiDAL().GetAssignActivities(dp);
            var trustFunds = new AimsDAL().GetTrustFunds(dp);
            return new
            {
                AssignedActivities = assignedActivities,
                Projects = Sessions.SubmitAssignedActivities,
                TrustFunds = trustFunds
            };
        }

        [AcceptVerbs("GET", "POST")]
        public object SubmitAssignedActivities(List<iatiactivity> assignedActivities)
        {
            if (assignedActivities == null) return null;

            var aimsCoFinancedProjects = (from i in assignedActivities
                                         join a in Sessions.SubmitAssignedActivities on i.MappedProjectId equals a.ProjectId
                                         
                                         select a).DistinctBy(d=>d.ProjectId);

            foreach (var project in aimsCoFinancedProjects)
            {
                var acts = assignedActivities.FindAll(f=>f.MappedProjectId == project.ProjectId);
                project.MatchedProjects.AddRange(acts);
            }

            return aimsCoFinancedProjects;
        }

        [HttpGet]
        public List<transaction> GetTrustFundDetails(int trustFundId)
        {
            return new AimsDAL().GetTrustFundDetails(trustFundId);

        }
        [AcceptVerbs("GET", "POST")]
        public int? ImportProjects(ProjectMapModel projectMapModel)
        {
            var matchedProjects = projectMapModel.MatchedProjects;

            var margedProjects = new List<iatiactivity>();

            foreach (var matchedProject in matchedProjects)
            {
                matchedProject.aimsProject.FundSourceIDnIATICode = matchedProject.iatiActivity.FundSourceIDnIATICode;
                foreach (var field in matchedProject.Fields)
                {
                    if (field.IsSourceIATI)
                    {
                        if (field.Field == "title")
                        {
                            matchedProject.aimsProject.Title = matchedProject.iatiActivity.Title;
                        }
                        if (field.Field == "description")
                        {
                            matchedProject.aimsProject.Description = matchedProject.iatiActivity.Description;
                        }

                    }
                }

                var trns = new List<transaction>();
                var planDis = new List<planneddisbursement>();
                foreach (var field in matchedProject.TransactionFields)
                {
                    if (field.Field == "commitment")
                    {
                        if (field.IsSourceIATI)
                            trns.AddRange(matchedProject.iatiActivity.Commitments);
                        else
                            trns.AddRange(matchedProject.aimsProject.Commitments);
                    }
                    else if (field.Field == "disbursment")
                    {
                        if (field.IsSourceIATI)
                            trns.AddRange(matchedProject.iatiActivity.Disbursments);
                        else
                            trns.AddRange(matchedProject.aimsProject.Disbursments);
                    }
                    else if (field.Field == "planned-disbursment")
                    {
                        if (field.IsSourceIATI)
                            planDis.AddRange(matchedProject.iatiActivity.PlannedDisbursments);
                        else
                            planDis.AddRange(matchedProject.aimsProject.PlannedDisbursments);
                    }
                }

                matchedProject.aimsProject.transaction = trns.ToArray();
                matchedProject.aimsProject.planneddisbursement = planDis.ToArray();
                margedProjects.Add(matchedProject.aimsProject);
            }

            return new AimsDAL().UpdateProjects(margedProjects, Sessions.UserId);
        }

    }


}

