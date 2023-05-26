using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Models;

public class MyContext : DbContext {
    private const string HOST = "%HOST%";
    private const string DBNAME = "%DB_NAME%";
    private const string USER = "%USER%";
    private const string PASSWORD = "%PASSWORD%";

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
    public DbSet<mType> tbTypes { get; set; }
    public DbSet<mProductType> tbProductTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseMySQL(
            $"server={HOST};database={DBNAME};user={USER};password={PASSWORD};SslMode=none");
    }
}