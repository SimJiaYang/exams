using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q1.Base
{
    /**
     * Model base entity
     */
    public class BaseEntity<T>
    {
        public T Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string UpdateBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
