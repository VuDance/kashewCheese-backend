using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role:BaseEntity
    {
        public string RoleName {  get; set; }
        public ICollection<User> Users { get; set; }
    }
}
