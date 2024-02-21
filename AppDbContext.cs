using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using C_RayFingerNetwork;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<TemplateModel> MyEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=mydatabase.db");
    }

    public void AddNewItem(TemplateModel newItem)
    {
        MyEntities.Add(newItem);
        SaveChanges();
    }

    public TemplateModel GetItemById(int id)
    {
        return MyEntities.FirstOrDefault(item => item.Id == id);
    }

    public int GetItemCount()
    {
        try
        {
            return MyEntities.Count();
        }catch(Exception ex)
        {
            return 0;
        }        
    }
}
