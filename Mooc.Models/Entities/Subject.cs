using Mooc.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Models.Entities
{
    public class Subject : BaseEntity
    {
        // 课程 标识ID
        public string ClassGuid { get; set; }

        // 课程名称 ：数据库
        public string SubjectName { get; set; }

        // 父类别 标识ID 
        public string ParentGuid { get; set; }

        //分类 级别名称 ： 二级分类
        public string Level { get; set; }

        //课程备注： 数据库课程
        public string ClassComment { get; set; }


    }
}