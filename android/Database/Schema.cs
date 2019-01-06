using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace mtnc.android.Database
{
    public abstract class AbstractDataTable : AbstractTable
    {
        [Column("cOnUtc")]
        public DateTime CreatedOnUtc { get; set; }

        [Column("mOnUtc")]
        public DateTime ModifiedOnUtc { get; set; }
    }

    public abstract class AbstractTable
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
    }

    [Table("Bills")]
    public class Bill : AbstractDataTable
    {
        [ForeignKey(typeof(Valuable), Name = "idValuable")]
        public int IdValuable { get; set; }

        public decimal Price { get; set; }
    }

    [Table("sqlite_master")]
    public class Sqlite_Master
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("rootpahe")]
        public int RootPage { get; set; }
        [Column("sql")]
        public string Sql { get; set; }
        [Column("tbl_name")]
        public string Tbl_Name { get; set; }
        [Column("type")]
        public string Type { get; set; }
    }

    [Table("Valuables")]
    public class Valuable : AbstractTable
    {
        [MaxLength(3), Indexed(Name = "ValuableSymbol", Order = 1, Unique = true)]
        public string Symbol { get; set; }
    }
}