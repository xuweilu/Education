using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Education.Abstract
{
    public interface IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        Guid Id { get; set; }
    }
}