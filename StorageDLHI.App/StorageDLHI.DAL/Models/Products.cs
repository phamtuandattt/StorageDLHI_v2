using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.DAL.Models
{
    public class Products
    {
        public Guid Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Des_2 { get; set; }
        public string Product_Code { get; set; }
        /// <summary>
        /// It is a Standard name
        /// </summary>
        public string Product_Material_Code { get; set; } // StandardName
        public string PictureLink { get; set; }
        public string Picture { get; set; }
        public byte[] Image { get; set; }
        public string A_Thinhness { get; set; }
        public string B_Depth { get; set; }
        public string C_Witdh {  get; set; }
        public string D_Web {  get; set; }
        public string E_Flag { get; set; }
        public string F_Length { get; set; }
        public string G_Weight { get; set; }
        public string Used_Note { get; set; }
        public Guid UnitId { get; set; }
        public Guid Product_TypeId { get; set; }

        public Guid Origin_Id { get; set; }
        public Guid M_Type_Id { get; set; }
        public Guid Stand_Id { get; set; }

    }
}
