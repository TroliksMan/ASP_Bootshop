using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Models;

public class MyContext : DbContext {
    public DbSet<mAdmin> tbAdmins { get; set; }
    public DbSet<mCategory> tbCategories { get; set; }
    public DbSet<mColor> tbColors { get; set; }
    public DbSet<mDelivery> tbDeliveries { get; set; }
    public DbSet<mImage> tbImages { get; set; }
    public DbSet<mOrderDetail> tbOrderDetails { get; set; }
    public DbSet<mOrder> tbOrders { get; set; }
    public DbSet<mPayment> tbPayments { get; set; }
    public DbSet<mProduct> tbProducts { get; set; }
    public DbSet<mProductVariant> tbProductVariants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseMySQL(
            "server=mysqlstudenti.litv.sssvt.cz;database=4b2_stankomichal_db2;user=stankomichal;password=123456;SslMode=none");
    }
}