using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject.Areas.Identity.Data;


public class FinalProjectUser : IdentityUser
{

    

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }

    [RegularExpression(@"^[0-9]{10}$")]
    public int? PhoneNum { get; set; }


    public DateTime? DOB { get; set; }




    [PersonalData]
    public byte[]? Photo { get; set; }


}

