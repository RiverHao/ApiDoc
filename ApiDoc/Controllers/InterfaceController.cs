﻿using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.Threading.Tasks; 
using ApiDoc.DAL; 
using ApiDoc.IDAL;
using ApiDoc.Middleware;
using ApiDoc.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiDoc.Controllers
{
    public class InterfaceController : Controller
    {
        private readonly IInterfaceDAL infterfaceDAL;
        private readonly IFlowStepDAL flowStepDAL;
        private readonly DBRouteValueDictionary routeDict;

        public InterfaceController(IInterfaceDAL infterfaceDAL, 
            IFlowStepDAL flowStepDAL, 
            DBRouteValueDictionary _routeDict)
        {
            this.infterfaceDAL = infterfaceDAL;
            this.flowStepDAL = flowStepDAL;
            this.routeDict = _routeDict;
        }

        public IActionResult Index(string title,string url, int fksn)
        { 
         
            ViewData.Add("FKSN", fksn);
            ViewData.Add("keyTitle", title);
            ViewData.Add("keyUrl", url);

            List<InterfaceModel> list = this.infterfaceDAL.All(title,url, fksn);
            return View(list);
        }

        public IActionResult Add(int FKSN, int SN)
        {
            List<string> list = new List<string>();
            list.Add("Int");
            list.Add("Scalar");
            list.Add("DataSet");

            ViewData.Add("ExecuteType", list); //执行集合类型

            InterfaceModel model = new InterfaceModel();
            model.SN = SN;
            model.FKSN = FKSN;
            if (SN > 0)
            {
                model =(InterfaceModel)this.infterfaceDAL.Get(model);
            }
            return View(model);
        }

        public List<InterfaceModel> All(int fksn)
        {
            List<InterfaceModel> list = this.infterfaceDAL.All("", "", fksn);
            return list;
        }

        [HttpPost]
        public int Delete(List<int> ids)
        {
            foreach (int sn in ids)
            {
                Models.InterfaceModel model = new InterfaceModel();
                model.SN = sn;
                int result = this.infterfaceDAL.Delete(model);
            }
            return 1;
        }

        [HttpPost]
        public InterfaceModel Save(InterfaceModel model)
        { 
            if (model.SN > 0)
            {  
                this.infterfaceDAL.Update(model); 
            }
            else
            {
                int SN = this.infterfaceDAL.Insert(model);
                model.SN = SN; 
            } 
            return model;
        }

        #region Step

        public IActionResult FlowStepList(int FKSN)
        {
            List<FlowStepModel> list = this.flowStepDAL.Query(FKSN); 
            return PartialView("/Views/Interface/FlowStepList.cshtml", list);
        }

        [HttpPost]
        public IActionResult StepSave(FlowStepModel model)
        {
            if (model.SN > 0)
            {
                this.flowStepDAL.Update(model);
            }
            else
            {
                int SN = this.flowStepDAL.Insert(model);
                model.SN = SN;
            }

            List<FlowStepModel> list = this.flowStepDAL.Query(model.FKSN); 
            return PartialView("/Views/Interface/FlowStepList.cshtml", list);
        }

        public IActionResult DeleteFlowStep(FlowStepModel model)
        {
            int resutl = this.flowStepDAL.Delete(model);

            List<FlowStepModel> list = new List<FlowStepModel>();
            if (resutl > 0)
            {
               list = this.flowStepDAL.Query(model.FKSN);
            }
         
            return PartialView("/Views/Interface/FlowStepList.cshtml", list);
        }

        [HttpPost]
        public int SaveCmdText(int SN, string CommandType, string CommandText)
        {
           return this.flowStepDAL.SaveCmdText(SN, CommandType, CommandText);
        }

        #endregion

        
    }
}

