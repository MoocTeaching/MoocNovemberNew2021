using Mooc.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Models.Entities
{
    public class Catagory : BaseEntity
    {
        public int ParentId { get; set; }

        public int IsDel { get; set; }

        public int Type { get; set; }

        public DateTime CreateTime { get; set; }
        //分类 名称 ： 计算机
        public string CategoryName { get; set; }

        //分类备注： 计算机课程
        public string Remark { get; set; }


    }
}
