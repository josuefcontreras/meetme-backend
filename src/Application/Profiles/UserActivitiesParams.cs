using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class UserActivitiesParams: PagingParams
    {
        public string? Predicate { get; set; }
    }
}
