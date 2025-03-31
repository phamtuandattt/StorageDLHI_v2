using StorageDLHI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.App.Enums
{
    public enum Materials : int
    {
        Origins = 1,
        Types = 2,
        Standards = 3,
    }

    public class Common
    {

    }

    public class MaterialCommonDto
    {
        public Origins Origins { get; set; }
        public Material_Types MaterialType { get; set; }
        public Material_Standards MaterialStandard { get; set; }
    }
}
