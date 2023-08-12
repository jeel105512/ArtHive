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
                new MenuItem { Controller = "Shop", Action = "ViewMyCart", Label = "Cart", Authorized = true },
                new MenuItem { Controller = "Shop", Action = "Orders", Label = "Orders", Authorized = true },
                new MenuItem { Controller = "Shop", Action = "Index", Label = "Shop", DropdownItems = new List<MenuItem>{
                    new MenuItem {Controller = "Shop", Action = "ShopAll", Label = "Shop All"},
                    new MenuItem {Controller = "Shop", Action = "ShopByCollection", Label = "Shop by Collections"},
                } },
                new MenuItem { Controller = "Orders", Action = "Index", Label = "Admin", DropdownItems = new List<MenuItem>{
                    new MenuItem {Controller = "Collections", Action = "Index", Label = "Collections"},
                    new MenuItem {Controller = "Artworks", Action = "Index", Label = "Artworks"},
                    new MenuItem {Controller = "Orders", Action = "Index", Label = "Orders"},
                    new MenuItem {Controller = "Carts", Action = "Index", Label = "Carts"},
                }, Authorized = true, AllowedRoles = new List<string>{ "Administrator" } },
                new MenuItem { Controller = "Home", Action = "About", Label = "About" },
                new MenuItem { Controller = "Home", Action = "ContactUs", Label = "ContactUs" },
            };
            return View(menuItems);
        } 
    }
}
