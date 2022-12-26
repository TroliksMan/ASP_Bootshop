using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models;

public class mOrder {
    [Key] public int id { get; set; }
    [ForeignKey("Delivery")] public int deliveryID { get; set; }
    [ForeignKey("Payment")] public int paymentID { get; set; }
    public DateTime orderDate { get; set; } = DateTime.Now;
    public string name { get; set; }
    public string surname { get; set; }
    public string shipStreet { get; set; }
    public string shipCity { get; set; }
    public string shipZipCode { get; set; }
    public string shipCountry { get; set; }

    public mDelivery Delivery { get; set; }
    public mPayment Payment { get; set; }
}