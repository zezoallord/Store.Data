using Store.Data.Entities.OrderEntities;

namespace Store.Repository.Specefication.OrderSpecs
{
    public class OrderWithItemSpecefication : BaseSpecefication<Order>
    {
        public OrderWithItemSpecefication(string buyerEmail) : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }
        public OrderWithItemSpecefication(Guid id) : base(order => order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            
        }
    }
}
