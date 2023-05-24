using ArtHive.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtHive.Components.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
       public IViewComponentResult Invoke()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem { Controller = "Home", Action = "Index", Label = "Home" },
                new MenuItem { Controller = "Home", Action = "About", Label = "About" },
                new MenuItem { Controller = "Categories", Action = "Index", Label = "Categories", DropdownItems = new List<MenuItem>{
                    new MenuItem {Controller = "Categories", Action = "Index", Label = "List"},
                    new MenuItem {Controller = "Categories", Action = "Create", Label = "Create"},
                } },
                new MenuItem { Controller = "Home", Action = "Privacy", Label = "Privacy" },
            };
            return View(menuItems);
        } 
    }
}
