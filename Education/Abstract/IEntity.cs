using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Education.Abstract
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}