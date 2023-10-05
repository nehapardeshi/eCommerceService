using eCommerceService.Entities;
using eCommerceService.Entities.Enums;

namespace eCommerceService.Services.Validators
{
    public interface IOrderValidator
    {
        OrderAction OrderAction { get; }
        bool ValidateOrder(Order order);
    }
}
