using DotNetCoreMVC.Models;
using DotNetCoreMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Controllers
{
    [Authorize]
    public class TodoController: Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //查找当前登陆用户
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Challenge(); //强制用户再次登录

            // 从数据库获取 to-do 条目
            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

            // 把条目置于 model 中
            var model = new TodoViewModel()
            {
                Items = items
            };

            // 使用 model 渲染视图
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Challenge();

            var successful = await _todoItemService.AddItemAsync(newItem, currentUser);
            if (!successful)
                return BadRequest("Could not add item.");

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Index");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Challenge();

            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);
            if (!successful)
                return BadRequest("Could not mark item as done");

            return RedirectToAction("Index");
        }
    }
}
