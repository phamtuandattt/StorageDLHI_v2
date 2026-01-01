using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Material_Types
    {
        public Guid Id { get; set; }
        public string Type_Code { get; set; }
        public string Type_Des { get; set; }
    }

    public class Material_Type_Detail
    {
        public Guid Id { get; set; }
        public string Material_Type_Code { get; set; }
        public string Material_Type_Name { get; set; }
    
        public Guid Material_Types_Id { get; set; }

    }

    public class Material_Type_Detail_Item
    {
        public Guid Id { get; set; }
        public string Item_Number { get; set; }
        public string Item_Name { get; set; }

        public Guid Item_Type {  get; set; }    //-- is MATERIAL_TYPE_DETAL_ID
    }
}