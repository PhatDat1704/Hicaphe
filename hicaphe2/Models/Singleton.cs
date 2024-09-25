using hicaphe2.Models;
using System.Collections.Generic;
using System.Linq;

public class Singleton
{
    private static Singleton instance;
    private readonly HiCaPheEntities2 database = new HiCaPheEntities2();
    private List<SANPHAM> products;

    private Singleton()
    {
        products = database.SANPHAMs.ToList();
    }

    public static Singleton GetInstance()
    {
        if (instance == null)
        {
            instance = new Singleton();
        }
        return instance;
    }

    public List<SANPHAM> Products
    {
        get { return products; }
    }

    public void AddProduct(SANPHAM product)
    {
        products.Add(product);
        database.SANPHAMs.Add(product);
        database.SaveChanges();
    }    
}
