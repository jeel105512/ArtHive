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
                new MenuItem { Controller = "Shop", Action = "Index", Label = "Shop", DropdownItems = new List<MenuItem>{
                    new MenuItem {Controller = "Shop", Action = "ShopAll", Label = "Shop All"},
                    new MenuItem {Controller = "Shop", Action = "ShopByCollection", Label = "Shop by Collections"},
                } },
                new MenuItem { Controller = "Collections", Action = "Index", Label = "Collections", DropdownItems = new List<MenuItem>{
                    new MenuItem {Controller = "Collections", Action = "Index", Label = "List"},
                    new MenuItem {Controller = "Collections", Action = "Create", Label = "Create"},
                }, Authorized = true, AllowedRoles = new List<string> { "Administrator" } },
                new MenuItem { Controller = "Artworks", Action = "Index", Label = "Artworks", DropdownItems = new List<MenuItem>{
                    new MenuItem {Controller = "Artworks", Action = "Index", Label = "List"},
                    new MenuItem {Controller = "Artworks", Action = "Create", Label = "Create"},
                }, Authorized = true, AllowedRoles = new List<string> { "Administrator" }  },
                new MenuItem { Controller = "Home", Action = "About", Label = "About" },
                new MenuItem { Controller = "Home", Action = "Privacy", Label = "Privacy" },
            };
            return View(menuItems);
        } 
    }
}
