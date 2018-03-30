using PornSite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PornSite.Repositories
{
    public class CookieRepository
    {
        public void BubbleSort(string cookie, ref string[] arr, int type)
        {
            if (!string.IsNullOrEmpty(cookie))
            {
                arr = cookie.Split(':');
            }
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (Convert.ToInt32(arr[j + 1].Split(',')[type]) > Convert.ToInt32(arr[j].Split(',')[type]))
                    {
                        string tmp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = tmp;
                    }
                }
            }
        }
        public void UpdateCategoryCookieValue(ref string[] inputArray, int key)
        {
            int min = 0;
            bool found = false;
            int max = inputArray.Length - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                if (key == Convert.ToInt32(inputArray[mid].Split(',')[0]))
                {
                    var cat = inputArray[mid].Split(',')[0];
                    var count = Convert.ToInt32(inputArray[mid].Split(',')[1]);
                    count++;
                    inputArray[mid] = cat + "," + count;
                    found = true;
                    break;
                }
                else if (key > Convert.ToInt32(inputArray[mid].Split(',')[0]))
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }
            if (!found)
            {
                Array.Resize(ref inputArray, inputArray.Length + 1);
                inputArray[inputArray.Length - 1] = key + "," + "1";

            }
        }
        public void UpdateCategoryCookie(IEnumerable<CategoryDTO> CategoryIds)
        {
            string cookie = null;
            if (HttpContext.Current.Request.Cookies["CategoryCount"] != null)
            {
                cookie = HttpContext.Current.Request.Cookies["CategoryCount"].Value;
            }
            if (!string.IsNullOrEmpty(cookie))
            {
                string[] sorted = null;
                try
                {
                    BubbleSort(cookie, ref sorted, 0);
                    foreach (CategoryDTO item in CategoryIds)
                    {
                        UpdateCategoryCookieValue(ref sorted, item.Id);
                    }
                    BubbleSort(string.Empty, ref sorted, 1);
                    HttpContext.Current.Response.Cookies["CategoryCount"].Value = String.Join(":", sorted);
                }
                catch
                {
                    HttpContext.Current.Request.Cookies["CategoryCount"].Expires = DateTime.Now.AddDays(-1);
                }
            }
            else
            {
                string newCookieString = null;
                foreach (CategoryDTO item in CategoryIds)
                {
                    newCookieString = newCookieString + item.Id + ",1:";
                }
                HttpCookie myCookie = new HttpCookie("CategoryCount");
                myCookie.Value = newCookieString.Remove(newCookieString.Length - 1);
                myCookie.Expires = DateTime.Now.AddDays(36500d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
        public void UpdateHistoryCookie(string Id)
        {
            string cookie = null;
            if (HttpContext.Current.Request.Cookies["History"] != null)
            {
                cookie = HttpContext.Current.Request.Cookies["History"].Value;
            }
            if (!string.IsNullOrEmpty(cookie))
            {
                string[] array;
                List<string> list;
                if (cookie.Contains(","))
                {
                    array = cookie.Split(',');
                    list = new List<string>(array);
                }
                else
                {
                    list = new List<string>();
                    list.Add(cookie);
                }
                bool found = false;
                foreach (string item in list)
                {
                    if (item == Id)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    list.Insert(0, Id);
                }
                if (list.Count > 4)
                {
                    list.RemoveAt(list.Count - 1);
                }
                HttpContext.Current.Response.Cookies["History"].Value = String.Join(",", list.ToArray());
            }
            else
            {
                HttpCookie myCookie = new HttpCookie("History");
                myCookie.Value = Id;
                myCookie.Expires = DateTime.Now.AddDays(36500d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
    }
}