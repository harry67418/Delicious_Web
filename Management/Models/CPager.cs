using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{
    public class CPager
    {
        public int Totalitems { get;private set; }
        public int Currentpage { get; private set; }
        public int Pagesize { get; private set; }
        public int Totalpage { get; private set; }
        public int Startpage { get; private set; }
        public int Endpage { get; private set; }
        public CPager() {
        }
        public CPager(int totalitems,int page,int pagesize)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalitems / (decimal)pagesize);

            int currentPage = page;

            int startPage = page - 5;
            int endPage = page + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            Totalitems = totalitems;
            Currentpage = currentPage;
            Pagesize = pagesize;
            Totalpage = totalPages;
            Startpage = startPage;
            Endpage = endPage;


        }

    }
}
