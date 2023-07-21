using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace DreamToEarn.Helper
{
    public abstract class AppController : Controller
    {
        public AppUser CurrentUser
        {
            get
            {
                return new AppUser(this.User as ClaimsPrincipal);
            }
        }
    }

    public enum Roles
    {
        Admin = 1,
        Investor = 2
    }

    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal) : base(principal)
        {

        }
        public string UserName
        {
            get
            {
                return this.FindFirst("UserName").Value;
            }
        }
        public string DisplayName
        {
            get
            {
                return this.FindFirst("DisplayName").Value;
            }
        }
        public string FirstName
        {
            get
            {
                return this.FindFirst("FirstName").Value;
            }
        }
        public string LastName
        {
            get
            {
                return this.FindFirst("LastName").Value;
            }
        }
        public string Email
        {
            get
            {
                return this.FindFirst(ClaimTypes.Email).Value;
            }
        }
        public int UserId
        {
            get
            {
                return this.FindFirst("UserId").Value.ToInt();
            }
        }
        public string Role
        {
            get
            {
                return this.FindFirst(ClaimTypes.Role).Value.ToDefaultString();
            }
        }
    }

    public static class EnumerableExtensions
    {

        public static int ToInt(this string number)
        {
            try
            {
                if (number != string.Empty)
                {
                    number = number.Replace(",", "").Replace("$", "");
                    var nm = int.Parse(number);
                    return nm;
                }
            }
            catch
            {

            }
            return 0;
        }


        public static int ToInt(this int? number)
        {
            try
            {
                if (number != null)
                {
                    return number.Value;
                }
            }
            catch
            {

            }
            return 0;
        }

        public static decimal ToDecimal(this object number)
        {
            try
            {
                if (number != null)
                {
                    return Convert.ToDecimal(number);
                }
            }
            catch
            {

            }
            return 0;
        }

        public static string ToDefaultString(this object obj)
        {
            try
            {
                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        public static string GetRoleName(this int Role)
        {
            var rolename = "";

            switch (Role)
            {
                case 1:
                    rolename = "Admin";
                    break;
                case 2:
                    rolename = "User";
                    break;               
                default:
                    break;
            }

            return rolename;
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, string selectedValue)
        {
            return items.ToSelectListItems(name, value, selectedValue, false);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, string selectedValue, bool includeNotApplicable, string notApplicableValue = "", string notApplicableText = "(Not Applicable)")
        {
            return items.ToSelectListItems(name, value, x => value(x) == selectedValue, includeNotApplicable, notApplicableValue, notApplicableText);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, Func<T, bool> isSelected)
        {
            return items.ToSelectListItems(name, value, isSelected, false);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, Func<T, bool> isSelected, bool includeNotApplicable, string notApplicableValue = "", string notApplicableText = "(Not Applicable)")
        {
            if (includeNotApplicable)
            {
                var returnItems = new List<SelectListItem>
                          {
                            new SelectListItem
                              {
                                Text = notApplicableText,
                                Value = notApplicableValue,
                                Selected = false
                              }
                          };

                if (items != null)
                {
                    returnItems.AddRange(items.Select(item => new SelectListItem
                    {
                        Text = name(item),
                        Value = value(item),
                        Selected = isSelected(item)
                    }));
                }
                return returnItems;
            }

            if (items == null)
                return new List<SelectListItem>();

            return items.Select(item => new SelectListItem
            {
                Text = name(item),
                Value = value(item),
                Selected = isSelected(item)
            });
        }

        public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<int> selectedValues)
        {
            if (selectedValues == null)
                selectedValues = new List<int>();
            return items.ToMultiSelectListItems(name, value, selectedValues.Select(x => x.ToString()));
        }

        public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<long> selectedValues)
        {
            if (selectedValues == null)
                selectedValues = new List<long>();
            return items.ToMultiSelectListItems(name, value, selectedValues.Select(x => x.ToString()));
        }

        public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<double> selectedValues)
        {
            if (selectedValues == null)
                selectedValues = new List<double>();
            return items.ToMultiSelectListItems(name, value, selectedValues.Select(x => x.ToString()));
        }

        public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<decimal> selectedValues)
        {
            if (selectedValues == null)
                selectedValues = new List<decimal>();
            return items.ToMultiSelectListItems(name, value, selectedValues.Select(x => x.ToString()));
        }

        public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<string> selectedValues)
        {
            if (items == null)
                return new List<SelectListItem>();

            if (selectedValues == null)
                selectedValues = new List<string>();

            return items.ToMultiSelectListItems(name, value, x => selectedValues.Contains(value(x)));
        }

        public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, Func<T, bool> isSelected)
        {
            if (items == null)
                return new List<SelectListItem>();

            return items.Select(item => new SelectListItem
            {
                Text = name(item),
                Value = value(item),
                Selected = isSelected(item)
            });
        }
    }
}