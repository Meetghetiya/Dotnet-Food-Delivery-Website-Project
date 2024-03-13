using Foodi.Areas.Admin.Models;
using Foodi.DAL;

namespace Foodi.BAL
{
    public class Category_BAlBase
    {

        #region GetAllCategories
        public List<Category> GetAllCategories(string search = null)
        {
            Category_DALBase Category_DALBase = new Category_DALBase();
            return Category_DALBase.GetFilterCategories(search);
        }
        #endregion

        #region GetCategoryById
        public Category GetCategoryById(int categoryId)
        {
            Category_DALBase Category_DALBase = new Category_DALBase();
            return Category_DALBase.GetCategoryByIdFromDatabase(categoryId);
        }
        #endregion

        #region AddCategory
        public bool AddCategory(Category category)
        {

            try
            {
                Category_DALBase categoryDAL = new Category_DALBase();
                categoryDAL.AddCategoryToDatabase(category);
                return true; 
            }
            catch (Exception ex)
            {
                return false; // Failed to add the category
            }
           
        }

        #endregion

        #region UpdateCategory
        public bool UpdateCategory(Category category)
        {
            try
            {
                Category_DALBase categoryDAL = new Category_DALBase();
                categoryDAL.UpdateCategoryInDatabase(category);
                return true; 
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region DeleteCategory
        public bool DeleteCategory(int categoryId)
        {
            try
            {
                Category_DALBase categoryDAL = new Category_DALBase();
                categoryDAL.DeleteCategoryFromDatabase(categoryId);
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

