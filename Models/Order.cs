using System.ComponentModel.DataAnnotations;

namespace ArtHive.Models
{
    public enum PaymentMethods
    {
        VISA,
        MasterCard,
        InteractDebit,
        PayPel,
        Strip
    }
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int CartId { get; set; }

        [Required()]
        [Display(Name = "Order")]
        public int OrderDetailId { get; set; }

        //[Required(ErrorMessage = "You must provide your First Name"), MaxLength(200)]
        //public string FirstName { get; set; }

        //[Required(ErrorMessage = "You must provide your Last Name"), MaxLength(200)]
        //public string LastName { get; set; }

        //public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
        
        [Required(ErrorMessage = "You must enter your Address")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "You must enter City")]
        public string City { get; set; }
        
        [Required(ErrorMessage = "You must enter City")]
        public string Province { get; set; }
        
        [Required(ErrorMessage = "You must enter your Postal Code")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        
        [Required(ErrorMessage = "You must enter your Phone Number"), MaxLength(10)]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "You must enter your Email Id"), MaxLength(256)]
        public string Email { get; set; }

        public bool PaymentReceived { get; set; }

        [Required(ErrorMessage = "You must select a Payment Method"), MaxLength(256)]
        public PaymentMethods PaymentMethod { get; set; }

        public User? User { get; set; }

        public Cart? Cart { get; set; }
    }
}
