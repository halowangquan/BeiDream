﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeiDream.Demo.Domain.Queries;
using BeiDream.Demo.Service.Contracts;
using BeiDream.Demo.Service.Dtos;
using BeiDream.Demo.Web.Areas.Systems.Models.Role;
using BeiDream.Utils.PagerHelper;
using BeiDream.Web.Mvc.EasyUi;

namespace BeiDream.Demo.Web.Areas.Systems.Controllers
{
    public class RoleController : EasyUiControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        #region 增删改查
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Query(RoleQuery query)
        {
            SetPage(query);
            var result = _roleService.Query(query).Convert(p => p.ToGridVm());
            return ToDataGridResult(result, result.TotalCount);
        }
        public PartialViewResult Add()
        {
            Guid addId = Guid.NewGuid();
            return PartialView("Parts/Form", new VmRoleAddorEdit(addId));
        }
        public PartialViewResult Edit(Guid id)
        {
            var dto = _roleService.Find(id);
            return PartialView("Parts/Form", dto.ToFormVm());
        }
        public ActionResult Save(VmRoleAddorEdit vm)
        {
            _roleService.AddorUpdate(vm.ToDto());
            return AjaxOkResponse("保存成功！");
        }
        [HttpPost]
        public ActionResult Delete(string ids)
        {
            _roleService.Delete(new Guid(ids));
            return AjaxOkResponse("删除成功！");
        } 
        #endregion
        /// <summary>
        ///获取分页的角色列表，以及查询的用户id的所有角色选中状态
        /// </summary>
        /// <param name="query"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult QueryByUser(RoleQuery query, Guid userId)
        {
            SetPage(query);
            var result = _roleService.Query(query, userId).Convert(p=>p.ToGridVm());
            return ToDataGridResult(result, result.TotalCount);
        }

    }
}