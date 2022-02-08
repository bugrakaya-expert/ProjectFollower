using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Models.ViewModels
{
    public class DepartmentMainVM
    {
        List<DepartmentsVM> DepartmentsVMs { get; set; }
        List<ResponsibleUsers> ResponsibleUsers { get; set; }
    }
}
