using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TrackerUser class
public class TimeTable
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("FinalProjectUser")]
    public string UserId{ get; set; }
    public FinalProjectUser FinalProjectUser { get; set; }


    public DateTime Date { get; set; }
    public DateTime Clock_In { get; set; }


    public DateTime? Clock_Out { get; set; }
}

