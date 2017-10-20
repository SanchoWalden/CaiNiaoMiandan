using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models.DataModel
{
    public class MianDanContext : DbContext
    {
        public MianDanContext()
            : base("MianDanContext")
        {

        }

        public DbSet<Test> Tests { get; set; }

        /// <summary>
        /// OnModelCreating方法中的modelBuilder.Conventions.Remove语句禁止表名称正在多元化。如果你不这样做，所生成的表将命名为Students、Courses和Enrollments。相反，表名称将是Student、Course和Enrollment。开发商不同意关于表名称应该多数。本教程使用的是单数形式，但重要的一点是，您可以选择哪个你更喜欢通过包括或省略这行代码的形式。
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}