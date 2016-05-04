using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Education.Abstract
{
    public interface IEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        Guid Id { get; set; }
    }
}