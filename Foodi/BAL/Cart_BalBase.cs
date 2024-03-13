using Foodi.Areas.Admin.Models;
using Foodi.DAL;

namespace Foodi.BAL
{
    public class Cart_BalBase
    {
        #region InsertCart
        public bool InsertCart(Cart cart)
        {
            try
            {
                Cart_DalBase _CartDal = new Cart_DalBase();
                _CartDal.InsertCart(cart);
                return true;
            }
            catch (Exception ex)
            {
                return false; 
            }
           
        }
        #endregion

        public List<Cart> GetAllCarts()
        {
            Cart_DalBase _CartDal = new Cart_DalBase();
            return _CartDal.GetAllCarts();
        }

        public Cart GetByIdCart(int cartId)
        {
            Cart_DalBase _CartDal = new Cart_DalBase();
            return _CartDal.GetCartById(cartId);
        }


        #region FilterCart
        public List<Cart> FilterCart(int? UserId = null, int? ProductId = null)
        {
            Cart_DalBase _CartDal = new Cart_DalBase();
            return _CartDal.GetFilterCarts(ProductId,UserId);
        }
        #endregion

        #region UpdateCart
        public bool UpdateCart(Cart cart)
        {
            try
            {
                Cart_DalBase _CartDal = new Cart_DalBase();
                _CartDal.UpdateCart(cart);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region DeleteCart
        public bool DeleteCart(int cartId)
        {
            try
            {
                Cart_DalBase _CartDal = new Cart_DalBase();
                _CartDal.DeleteCart(cartId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion


        #region DeleteCartByUserId
        public bool DeleteCartByUserId(int userId)
        {
            try
            {
                Cart_DalBase _CartDal = new Cart_DalBase();
                _CartDal.DeleteCartByUserId(userId);
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
