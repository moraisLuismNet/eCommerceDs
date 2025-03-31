using eCommerceDs.Models;

namespace eCommerceDs.DTOs
{
    public class OrderDTO
    {
        public int IdOrder { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public decimal Total { get; set; }
        public string UserEmail { get; set; } = null!;
        public int CartId { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; } = new();
    }
}
