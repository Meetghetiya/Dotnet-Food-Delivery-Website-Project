using Foodi.Areas.Admin.Models;
using Foodi.DAL;

namespace Foodi.BAL
{
    public class Product_BalBase
    {

        #region GetAllProducts
        public List<Product> GetAllProducts(string searchName = null, bool? isActive = null)
        {

            Product_DalBase Product_DalBase = new Product_DalBase();
            return Product_DalBase.GetFilterProducts(searchName , isActive);
        }
        #endregion

        #region GetProductById
        public Product GetProductById(int ProductId)
        {
            Product_DalBase Product_DalBase = new Product_DalBase();
            return Product_DalBase.GetProductById(ProductId);
        }
        #endregion

        #region AddProduct
        public bool AddProduct(Product product)
        {

            try
            {
                Product_DalBase Product_DalBase = new Product_DalBase();
                Product_DalBase.InsertProduct(product);
                return true; // Successfully added the category
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return false; // Failed to add the category
            }

        }

        #endregion

        #region UpdateProduct
        public bool UpdateProduct(Product product)
        {
            try
            {
                Product_DalBase Product_DalBase = new Product_DalBase();
                Product_DalBase.UpdateProduct(product);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region DeleteProduct
        public bool DeleteProduct(int ProductId)
        {
            try
            {
                Product_DalBase Product_DalBase = new Product_DalBase();
                Product_DalBase.Delete(ProductId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
