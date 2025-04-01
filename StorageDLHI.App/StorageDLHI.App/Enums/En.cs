﻿using StorageDLHI.DAL.Models;
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

    public enum TaxUnitCost : int
    {
        Tax = 1,
        Unit = 2,
        Cost = 3,
    }

    public class En
    {

    }

    public class MaterialCommonDto
    {
        public Origins Origins { get; set; }
        public Material_Types MaterialType { get; set; }
        public Material_Standards MaterialStandard { get; set; }
    }

    public class TaxUnitCostDto
    {
        public Taxs Taxs { get; set; }
        public Units Units { get; set; }
        public Costs Costs { get; set; }
    }

}
