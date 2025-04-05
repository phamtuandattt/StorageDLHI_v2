using StorageDLHI.DAL.DataProvider;
using StorageDLHI.DAL.Models;
using StorageDLHI.DAL.QueryStatements;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDLHI.BLL.ProductDAO
{
    public static class ProductDAO
    {
        public static SQLServerProvider data = new SQLServerProvider();

        public static bool Insert(Products product)
        {
            string sqlQuery = string.Format(QueryStatement.ADD_PROD, product.Id, product.Product_Name, product.Product_Des_2, product.Product_Code,
                product.Product_Material_Code, product.PictureLink, product.Picture, product.A_Thinhness, product.B_Depth, product.C_Witdh,
                product.D_Web, product.E_Flag, product.F_Length, product.G_Weight, product.Used_Note, product.UnitId);

            return data.Insert(sqlQuery) > 0;
        }

        public static Products GetProduct(Guid prodId)
        {
            var dt = data.GetData(string.Format(QueryStatement.GET_PROD, prodId), "PRODUCT_BY_ID");
            var row = dt.Rows[0];
            Products prod = new Products()
            {
                Id = Guid.Parse(row[QueryStatement.PROPERTY_PROD_ID].ToString()),
                Product_Name = row[QueryStatement.PROPERTY_PROD_NAME].ToString(),
                Product_Des_2 = row[QueryStatement.PROPERTY_PROD_DES_2].ToString(),
                Product_Code = row[QueryStatement.PROPERTY_PROD_CODE].ToString(),
                Product_Material_Code = row[QueryStatement.PROPERTY_PROD_MATERIAL_CODE].ToString(),
                PictureLink = row[QueryStatement.PROPERTY_PROD_PICTURE_LINK].ToString(),
                Image = row[QueryStatement.PROPERTY_PROD_PICTURE].ToString().Length > 0 && row[QueryStatement.PROPERTY_PROD_PICTURE].ToString() != null ? (byte[])row[QueryStatement.PROPERTY_PROD_PICTURE] : new byte[100],
                A_Thinhness = row[QueryStatement.PROPERTY_PROD_A].ToString(),
                B_Depth = row[QueryStatement.PROPERTY_PROD_B].ToString(),
                C_Witdh = row[QueryStatement.PROPERTY_PROD_C].ToString(),
                D_Web = row[QueryStatement.PROPERTY_PROD_D].ToString(),
                E_Flag = row[QueryStatement.PROPERTY_PROD_E].ToString(),
                F_Length = row[QueryStatement.PROPERTY_PROD_F].ToString(),
                G_Weight = row[QueryStatement.PROPERTY_PROD_G].ToString(),
                Used_Note = row[QueryStatement.PROPERTY_PROD_USAGE].ToString(),
                UnitId = Guid.Parse(row[QueryStatement.PROPERTY_PROD_UNIT_ID].ToString())
            };

            return prod ?? new Products();
        }
    }
}
