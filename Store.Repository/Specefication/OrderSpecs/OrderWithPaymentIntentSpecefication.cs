using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specefication.OrderSpecs
{
    public class OrderWithPaymentIntentSpecefication : BaseSpecefication<Order>
    {
        public OrderWithPaymentIntentSpecefication(string? paymentIntentId) : base(order => order.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
