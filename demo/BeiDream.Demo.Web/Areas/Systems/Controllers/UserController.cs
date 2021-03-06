﻿using BeiDream.Demo.Domain.Queries;
using BeiDream.Demo.Web.Areas.Systems.Models.User;
using BeiDream.Utils;
using BeiDream.Web.Mvc.EasyUi;
using System;
using System.Web.Mvc;
using BeiDream.Demo.Service.Users;
using BeiDream.Demo.Web.Areas.Systems.Models;
using BeiDream.Demo.Web.Security.Authorization;

namespace BeiDream.Demo.Web.Areas.Systems.Controllers
{
     [RoleAuthorize(true)]
    public class UserController : EasyUiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region 增删改查
        [RoleAuthorize]
        public ActionResult Index()
        {
            return View(new VmPermission());
        }
        public ActionResult QueryForm()
        {
            return PartialView("Parts/QueryForm", new UserQuery());
        }
        [HttpPost]
        public ActionResult Query(UserQuery query)
        {
            SetPage(query);
            var result = _userService.Query(query).Convert(p => p.ToGridVm());
            return ToDataGridResult(result, result.TotalCount);
        }
        [RoleAuthorize]
        public PartialViewResult Add()
        {
            Guid addId = Guid.NewGuid();
            return PartialView("Parts/Form", new VmUserAddorEdit(addId));
        }
        [RoleAuthorize]
        public PartialViewResult Edit(Guid id)
        {
            var dto = _userService.Find(id);
            return PartialView("Parts/Form", dto.ToFormVm());
        }
        [ValidateAntiForgeryToken]
        public ActionResult Save(VmUserAddorEdit vm)
        {
            _userService.AddorUpdate(vm.ToDto());
            return AjaxOkResponse("保存成功！");
        }
        [RoleAuthorize]
        [HttpPost]
        public ActionResult Delete(string ids)
        {
            _userService.Delete(new Guid(ids));
            return AjaxOkResponse("删除成功！");
        }

        #endregion 增删改查

        #region 角色设置

        /// <summary>
        /// 显示设置角色界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RoleAuthorize]
        public PartialViewResult EditRoles(Guid id)
        {
            return PartialView("Parts/UserRoles", id);
        }

        /// <summary>
        /// 保存用户操作的角色设置信息
        /// </summary>
        /// <param name="userId">当前设置的用户id</param>
        /// <param name="ids">选中的角色id集合</param>
        /// <returns></returns>
        public ActionResult SetRoles(Guid userId, string ids)
        {
            _userService.SetRoles(userId, ConvertHelper.ToList<Guid>(ids));
            return AjaxOkResponse("保存成功！");
        }

        #endregion 角色设置
    }
}