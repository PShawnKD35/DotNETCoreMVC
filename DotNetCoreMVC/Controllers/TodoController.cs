using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreMVC.Controllers
{
    public class TodoController: Controller
    {
        public IActionResult Index()
        {
            // 从数据库获取 to-do 条目

            // 把条目置于 model 中

            // 使用 model 渲染视图
        }
    }
}
